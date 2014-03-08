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
 * @(#) ShowExceptionAttribute.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;

using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

namespace AU.Com.BushLife.Aspects.ExceptionHandlers
{
	/// <summary>
	/// Exception handler that shows a windows form message box when an exception occurs
	/// </summary>
	[Serializable]
	[ProvideAspectRole(StandardRoles.ExceptionHandling)]
	[ShowExceptionAttribute(AttributeExclude = true)]
	public class ShowExceptionAttribute : OnExceptionAspect
	{
		/// <summary>
		/// The caption on the message box.  If this value is null no caption is displayed
		/// </summary>
		public string Caption { get; set; }

		/// <summary>
		/// The message to display in the dialog box
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Ensure the required properties are provided at compile time
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		public override bool CompileTimeValidate(System.Reflection.MethodBase method)
		{
			if (Message == null)
				throw new InvalidAnnotationException(string.Format("Message property is not defined and must be defined: {0}.{1}()", method.ReflectedType.FullName, method.Name));
			return base.CompileTimeValidate(method);
		}


		public override void OnException(MethodExecutionArgs args)
		{
			if (Caption == null)
				MessageBox.Show(Message);
			else
				MessageBox.Show(Message, Caption);
			base.OnException(args);
		}
	}
}
