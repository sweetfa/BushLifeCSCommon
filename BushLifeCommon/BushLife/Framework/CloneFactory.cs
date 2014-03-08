/**
 * Copyright (C) 2012 Bush Life Pty Limited
 * 
 * All rights reserved.  No unauthorised copying or redistribution without the prior written 
 * consent of the management of Bush Life Pty Limited.
 * 
 * www.bushlife.com.au
 * sales@bushlife.com.au
 * 
 * PO Box 865, Redcliffe, QLD, 4020, Australia
 * 
 * 
 * @(#) CloneFactory.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq.Expressions;

namespace AU.Com.BushLife.Framework
{
	/// <summary>
	/// Class to provide generic cloning capability
	/// <para>Caution should be used in cases where cloning of all objects is not required</para>
	/// </summary>
	public class CloneFactory
	{
		#region Cloning
		/// <summary>
		/// This dictionary caches the delegates for each 'to-clone' type.
		/// </summary>
		private static Dictionary<Type, Delegate> _cachedIL = new Dictionary<Type, Delegate>();
		private static Dictionary<Type, Delegate> _cachedILDeep = new Dictionary<Type, Delegate>();
		private LocalBuilder _lbfTemp;

		/// <summary>
		/// Generic cloning method that clones an object using IL.
		/// Only the first call of a certain type will hold back performance.
		/// After the first call, the compiled IL is executed.
		/// <para>This is a shallow copy only</para>
		/// </summary>
		/// <typeparam name="T">Type of object to clone</typeparam>
		/// <param name="myObject">Object to clone</param>
		/// <returns>Cloned object</returns>
		public static T CloneObjectWithILShallow<T>(T myObject)
		{
			Delegate myExec = null;
			if (!_cachedIL.TryGetValue(typeof(T), out myExec))
			{
				// Create ILGenerator
				DynamicMethod dymMethod = new DynamicMethod("DoClone", typeof(T), new Type[] { typeof(T) }, true);
				ConstructorInfo cInfo = myObject.GetType().GetConstructor(new Type[] { });

				ILGenerator generator = dymMethod.GetILGenerator();

				LocalBuilder lbf = generator.DeclareLocal(typeof(T));
				//lbf.SetLocalSymInfo("_temp");

				generator.Emit(OpCodes.Newobj, cInfo);
				generator.Emit(OpCodes.Stloc_0);
				foreach (FieldInfo field in myObject.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
				{
					// Load the new object on the eval stack... (currently 1 item on eval stack)
					generator.Emit(OpCodes.Ldloc_0);
					// Load initial object (parameter)          (currently 2 items on eval stack)
					generator.Emit(OpCodes.Ldarg_0);
					// Replace value by field value             (still currently 2 items on eval stack)
					generator.Emit(OpCodes.Ldfld, field);
					// Store the value of the top on the eval stack into the object underneath that value on the value stack.
					//  (0 items on eval stack)
					generator.Emit(OpCodes.Stfld, field);
				}

				// Load new constructed obj on eval stack -> 1 item on stack
				generator.Emit(OpCodes.Ldloc_0);
				// Return constructed object.   --> 0 items on stack
				generator.Emit(OpCodes.Ret);

				myExec = dymMethod.CreateDelegate(typeof(Func<T, T>));
				_cachedIL.Add(typeof(T), myExec);
			}
			return ((Func<T, T>)myExec)(myObject);
		}

		/// <summary>
		/// Generic cloning method that clones an object deeply using IL.
		/// Only the first call of a certain type will hold back performance.
		/// After the first call, the compiled IL is executed.
		/// </summary>
		/// <typeparam name="T">Type of object to clone</typeparam>
		/// <param name="myObject">Object to clone</param>
		/// <returns>Cloned object</returns>
		public T CloneObjectWithILDeep<T>(T myObject)
		{
			Delegate myExec = null;
			if (!_cachedILDeep.TryGetValue(typeof(T), out myExec))
			{
				// Create ILGenerator (both DM declarations work)
				// DynamicMethod dymMethod = new DynamicMethod("DoClone", typeof(T), 
				//      new Type[] { typeof(T) }, true);
				DynamicMethod dymMethod = new DynamicMethod("DoClone", typeof(T),
					new Type[] { typeof(T) }, Assembly.GetExecutingAssembly().ManifestModule, true);
				ConstructorInfo cInfo = myObject.GetType().GetConstructor(new Type[] { });
				ILGenerator generator = dymMethod.GetILGenerator();
				LocalBuilder lbf = generator.DeclareLocal(typeof(T));
				generator.Emit(OpCodes.Newobj, cInfo);
				generator.Emit(OpCodes.Stloc_0);

				foreach (FieldInfo field in typeof(T).GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public))
				{
					if (field.FieldType.IsValueType || field.FieldType == typeof(string))
						CopyValueType(generator, field);
					else if (field.FieldType.IsClass)
						CopyReferenceType(generator, field);
				}
				generator.Emit(OpCodes.Ldloc_0);
				generator.Emit(OpCodes.Ret);
				myExec = dymMethod.CreateDelegate(typeof(Func<T, T>));
				_cachedILDeep.Add(typeof(T), myExec);
			}
			return ((Func<T, T>)myExec)(myObject);
		}

		private void CopyReferenceType(ILGenerator generator, FieldInfo field)
		{
			// We have a reference type.
			_lbfTemp = generator.DeclareLocal(field.FieldType);
			if (field.FieldType.GetInterface("IEnumerable") != null)
			{
				// We have a list type (generic).
				if (field.FieldType.IsGenericType)
				{
					// Get argument of list type
					Type argType = field.FieldType.GetGenericArguments()[0];
					// Check that it has a constructor that accepts another IEnumerable.
					Type genericType = Type.GetType("System.Collections.Generic.IEnumerable`1["
							+ argType.FullName + "]");

					ConstructorInfo ci = field.FieldType.GetConstructor(new Type[] { genericType });
					if (ci != null)
					{
						// It has! (Like the List<> class)
						generator.Emit(OpCodes.Ldarg_0);
						generator.Emit(OpCodes.Ldfld, field);
						generator.Emit(OpCodes.Newobj, ci);
						generator.Emit(OpCodes.Stloc, _lbfTemp);
						PlaceNewTempObjInClone(generator, field);
					}
				}
			}
			else
			{
				CreateNewTempObject(generator, field.FieldType);
				PlaceNewTempObjInClone(generator, field);
				foreach (FieldInfo fi in field.FieldType.GetFields(System.Reflection.BindingFlags.Instance
					| System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public))
				{
					if (fi.FieldType.IsValueType || fi.FieldType == typeof(string))
						CopyValueTypeTemp(generator, field, fi);
					else if (fi.FieldType.IsClass)
						CopyReferenceType(generator, fi);
				}
			}
		}

		private void CreateNewTempObject(ILGenerator generator, Type type)
		{
			ConstructorInfo cInfo = type.GetConstructor(new Type[] { });
			generator.Emit(OpCodes.Newobj, cInfo);
			generator.Emit(OpCodes.Stloc, _lbfTemp);
		}

		private void CopyValueType(ILGenerator generator, FieldInfo field)
		{
			generator.Emit(OpCodes.Ldloc_0);
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldfld, field);
			generator.Emit(OpCodes.Stfld, field);
		}

		private void CopyValueTypeTemp(ILGenerator generator, FieldInfo fieldParent, FieldInfo fieldDetail)
		{
			generator.Emit(OpCodes.Ldloc_1);
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldfld, fieldParent);
			generator.Emit(OpCodes.Ldfld, fieldDetail);
			generator.Emit(OpCodes.Stfld, fieldDetail);
		}

		private void PlaceNewTempObjInClone(ILGenerator generator, FieldInfo field)
		{
			// Get object from custom location and store it in right field of location 0
			generator.Emit(OpCodes.Ldloc_0);
			generator.Emit(OpCodes.Ldloc, _lbfTemp);
			generator.Emit(OpCodes.Stfld, field);
		}


		#endregion
	}
}
