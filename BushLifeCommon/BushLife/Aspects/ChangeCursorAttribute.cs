/**
 * Copyright (C) 2014 Bush Life Pty Limited
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
 * @(#) CjangeCursorAttribute.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Text;

using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

namespace AU.Com.BushLife.Aspects
{
	/// <summary>
	/// Aspect to set the cursor for a windows form to a particular
	/// cursor type and reset it back to the default type on exit
	/// </summary>
	[Serializable]
	[AttributeUsage(AttributeTargets.Method)]
	[MulticastAttributeUsage(MulticastTargets.Method)]
	[ProvideAspectRole(StandardRoles.DataBinding)]
	public sealed class ChangeCursorAttribute : OnMethodBoundaryAspect
	{
		/// <summary>
		/// The name of the property that will be available in the instance
		/// of the method that this aspect advises.
		/// <para>It is expected to derive from System.Windows.Forms but
		/// does not necessarily have to provided it has a System.Windows.Form.Cursor property
		/// that matches this name</para>
		/// </summary>
		public string CursorPropertyName { get; set; }

		/// <summary>
		/// The name of the cursor to set to a standard System.Windows.Forms.Cursors type
		/// </summary>
		public string NewCursorTypeName { get; set; }

		/// <summary>
		/// The type of the cursor to set on entry
		/// </summary>
		private Cursor NewCursorType { get; set; }

		/// <summary>
		/// The property info for the cursor property name
		/// </summary>
		private PropertyInfo CursorPropertyInfo { get; set; }

		/// <summary>
		/// The aspect is advising on an extension method
		/// instead of a method in the class with the Cursors attribute
		/// </summary>
		private bool IsExtensionMethodAttribute { get; set; }

		/// <summary>
		/// Validate the necessary properties are set in the attribute at compile time
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		public override bool CompileTimeValidate(MethodBase method)
		{
			if (CursorPropertyName == null)
				throw new InvalidAnnotationException(string.Format("CursorPropertyName must be defined: {0}.{1}", method.DeclaringType.FullName, method.Name));
			if (NewCursorTypeName == null)
				throw new InvalidAnnotationException(string.Format("NewCursorType must be defined: {0}.{1}", method.DeclaringType.FullName, method.Name));
			return base.CompileTimeValidate(method);
		}

		/// <summary>
		/// Initialise the information required for this attribute
		/// at runtime
		/// </summary>
		/// <param name="method"></param>
		public override void RuntimeInitialize(MethodBase method)
		{
			base.RuntimeInitialize(method);
			PropertyInfo pi = typeof(Cursors).GetProperty(NewCursorTypeName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			NewCursorType = (Cursor)pi.GetValue(null, null);

			try
			{
				// If attribute associated with extension method use the type of the 
				// first parameter to associate the property with
				if (method.IsDefined(typeof(ExtensionAttribute), false))
				{
					ParameterInfo paramInfo = method.GetParameters()[0];
					Type type1 = paramInfo.ParameterType;
					CursorPropertyInfo = type1.GetProperty(CursorPropertyName);
					IsExtensionMethodAttribute = true;
				}
				else
					CursorPropertyInfo = method.DeclaringType.GetProperty(CursorPropertyName);
			}
			catch (Exception ex)
			{
				throw new InvalidAnnotationException(string.Format("CursorPropertyName {2} not found in type: {0}.{1}\n{3}\n", method.DeclaringType.FullName, method.Name, CursorPropertyName, ex.GetType().FullName, ex.Message));
			}
		}

		/// <summary>
		/// On entry to a method set the cursor type to the required
		/// type as specified in the attribute arguments
		/// </summary>
		/// <param name="args">The arguments to the method</param>
		public override sealed void OnEntry(MethodExecutionArgs args)
		{
			CursorPropertyInfo.SetValue(GetInstance(args), NewCursorType, null);
		}

		/// <summary>
		/// On method exit, regardless of success or failure reset
		/// the form cursor to the default cursor type
		/// </summary>
		/// <param name="args">The arguments to the method</param>
		public override sealed void OnExit(MethodExecutionArgs args)
		{
			CursorPropertyInfo.SetValue(GetInstance(args), Cursors.Default, null);
		}

		/// <summary>
		/// Get the object instance that contains the Cursor property
		/// depending on whether this attribute is attached to a method 
		/// within a class or an extension method
		/// </summary>
		/// <param name="args">The arguments to the method</param>
		/// <returns>The instance object</returns>
		private object GetInstance(MethodExecutionArgs args)
		{
			object instance = args.Instance;
			if (IsExtensionMethodAttribute)
				instance = args.Arguments[0];
			return instance;
		}
	}
}
