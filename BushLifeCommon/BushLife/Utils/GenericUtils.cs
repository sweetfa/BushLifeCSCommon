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
 * @(#) GenericUtils.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AU.Com.BushLife.Utils
{
	/// <summary>
	/// Utilities associated with generics
	/// </summary>
	public static class GenericUtils
	{
		#region Working With Generics
		/// <summary>
		/// Create an instance of a generic type from the supplied arguments
		/// <para>The genericClass must implement a default constructor taking no arguments</para>
		/// </summary>
		/// <param name="genericClass">The class type to instantiate</param>
		/// <param name="genericType">The generic parameter (or parameters) for the generic type</param>
		/// <returns>The new instantiated instance</returns>
		public static dynamic MakeGenericType(Type genericClass, params Type[] genericType)
		{
			Type constructedClass = genericClass.MakeGenericType(genericType);
			return Activator.CreateInstance(constructedClass);
		}

		/// <summary>
		/// Determine if a type is the same type as some unqualified generic type or a sub-type thereof
		/// </summary>
		/// <param name="typeToCheck">The type to check</param>
		/// <param name="genericType">The generic class hierarchy to test against</param>
		/// <returns>True if the type to check is the same type or a descendant thereof</returns>
		public static bool IsTypeDerivedFromGenericType(this Type typeToCheck, Type genericType)
		{
			if (typeToCheck == typeof(object))
			{
				return false;
			}
			else if (typeToCheck == null)
			{
				return false;
			}
			else if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
			{
				return true;
			}
			else
			{
				return IsTypeDerivedFromGenericType(typeToCheck.BaseType, genericType);
			}
		}

		/// <summary>
		/// Create and invoke a generic method
		/// </summary>
		/// <typeparam name="T">The type of the return value</typeparam>
		/// <param name="methodInfo">The method information for the basic method</param>
		/// <param name="instance">The instance of the non-static object to invoke the method on.  Use null if the method is static</param>
		/// <param name="type">The generic type argument for the method</param>
		/// <param name="args">The arguments to the method</param>
		/// <returns>The response from the invoked method</returns>
		public static T InvokeGenericMethod<T>(this MethodInfo methodInfo, object instance, Type type, params object[] args)
		{
			MethodInfo genericMethod = methodInfo.MakeGenericMethod(type);
			return (T)genericMethod.Invoke(instance, args);
		}

		/// <summary>
		/// Determine if a type implements a generic interface
		/// </summary>
		/// <param name="typeToCheck">The type to check for implementation of a generic interface</param>
		/// <param name="interfaceType">The generic interface to check for</param>
		/// <returns>true if the type implements the interface, false otherwise.</returns>
		public static bool ImplementsGenericInterface(this Type typeToCheck, Type interfaceType)
		{
			foreach (Type i in typeToCheck.GetInterfaces())
				if (i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
					return true;
			return false;
		}

		#region Get Generic Method
		private class SimpleTypeComparer : IEqualityComparer<Type>
		{
			public bool Equals(Type x, Type y)
			{
				// If parameter is generic type then match anyway
				if (x.IsGenericParameter || x.FullName == null)
					return true;
				return x.Assembly == y.Assembly &&
					x.Namespace == y.Namespace &&
					x.Name == y.Name;
			}

			public int GetHashCode(Type obj)
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Fetch the generic method of a particular type containing the parameters supplied
		/// </summary>
		/// <param name="type">The type of the entity to get the generic methods from</param>
		/// <param name="name">The name of the generic method</param>
		/// <param name="parameterTypes">The parameters for the generic method</param>
		/// <returns>The method info for the generic method</returns>
		public static MethodInfo GetGenericMethod(this Type type, string name, Type[] parameterTypes)
		{
			var methods = type.GetMethods();
			foreach (var method in methods.Where(m => m.Name == name))
			{
				var methodParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();

				if (methodParameterTypes.SequenceEqual(parameterTypes, new SimpleTypeComparer()))
				{
					return method;
				}
			}
			return null;
		}

		#endregion

		#endregion

	}
}
