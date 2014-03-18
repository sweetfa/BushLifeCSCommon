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

		public override bool CompileTimeValidate(System.Reflection.MethodBase method)
		{
			if (CursorPropertyName == null)
				throw new InvalidAnnotationException(string.Format("CursorPropertyName must be defined: {0}.{1}", method.DeclaringType.FullName, method.Name));
			if (NewCursorTypeName == null)
				throw new InvalidAnnotationException(string.Format("NewCursorType must be defined: {0}.{1}", method.DeclaringType.FullName, method.Name));
			try
			{
				CursorPropertyInfo = method.DeclaringType.GetProperty(CursorPropertyName);
			}
			catch (Exception ex)
			{
				throw new InvalidAnnotationException(string.Format("CursorPropertyName {2} not found in type: {0}.{1}\n{3}\n", method.DeclaringType.FullName, method.Name, CursorPropertyName, ex.GetType().FullName, ex.Message));
			}
			return base.CompileTimeValidate(method);
		}

		public override void RuntimeInitialize(MethodBase method)
		{
			base.RuntimeInitialize(method);
			PropertyInfo pi = typeof(Cursors).GetProperty(NewCursorTypeName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			NewCursorType = (Cursor)pi.GetValue(null, null);
		}

		public override sealed void OnEntry(MethodExecutionArgs args)
		{
			CursorPropertyInfo.SetValue(args.Instance, NewCursorType, null);
		}

		public override sealed void OnExit(MethodExecutionArgs args)
		{
			CursorPropertyInfo.SetValue(args.Instance, Cursors.Default, null);
		}
	}
}
