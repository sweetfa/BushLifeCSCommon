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
	public sealed class ShowExceptionAttribute : OnExceptionAspect
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
		/// If flag is set to true the details of the exception are added to the message
		/// </summary>
		public bool ShowExceptionDetail { get; set; }

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
			string message = Message;
			if (ShowExceptionDetail)
				message = string.Format("{0}\n\n{1}\n{2}", Message, args.Exception.GetType().FullName, args.Exception.Message);
			if (Caption == null)
				MessageBox.Show(message);
			else
				MessageBox.Show(message, Caption);
			base.OnException(args);
		}
	}
}
