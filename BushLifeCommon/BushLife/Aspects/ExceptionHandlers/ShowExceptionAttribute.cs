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
using System.Windows;
using System.Windows.Forms;
using System.Text;
using System.Reflection;

using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

using AU.Com.BushLife.Exceptions;

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
		/// If this value is set only trigger this advice on the specific exception type
		/// </summary>
		public Type SpecificExceptionType { get; set; }

		/// <summary>
		/// Ensure the required properties are provided at compile time
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		public override bool CompileTimeValidate(MethodBase method)
		{
			if (Message == null)
				throw new InvalidAnnotationException(string.Format("Message property is not defined and must be defined: {0}.{1}()", method.ReflectedType.FullName, method.Name));
			return base.CompileTimeValidate(method);
		}

		public override Type GetExceptionType(MethodBase targetMethod)
		{
			if (SpecificExceptionType != null)
				return SpecificExceptionType;
			return base.GetExceptionType(targetMethod);
		}

		public override void OnException(MethodExecutionArgs args)
		{
			if ((SpecificExceptionType == null && !args.Exception.IsAlreadyHandled(this.GetType()))
				|| (SpecificExceptionType != null && args.Exception.GetType().Equals(SpecificExceptionType)))
			{
				string message = Message;
				if (ShowExceptionDetail)
					message = string.Format("{0}\n\n{1}\n{2}", Message, args.Exception.GetType().FullName, args.Exception.Message);

				// Get the parent window if available
				DependencyObject dependencyObject = args.Instance as DependencyObject;
				Window window = null;
				if (dependencyObject != null)
					window = Window.GetWindow(dependencyObject);

				if (window != null)
				{
					if (Caption != null)
						System.Windows.MessageBox.Show(window, Message, Caption, MessageBoxButton.OK, MessageBoxImage.Error);
					else
						System.Windows.MessageBox.Show(window, Message, window.Name, MessageBoxButton.OK, MessageBoxImage.Error);
				}
				else
				{
					if (Caption != null)
						System.Windows.MessageBox.Show(message, Caption, MessageBoxButton.OK, MessageBoxImage.Error);
					else
						System.Windows.MessageBox.Show(message, "Exception Occurred", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
			if (SpecificExceptionType != null && !args.Exception.IsAlreadyHandled(this.GetType()))
			{
				args.Exception = new AlreadyHandledException(this.GetType(), args.Exception);
				args.FlowBehavior = FlowBehavior.ThrowException;
			}
			else
				args.FlowBehavior = FlowBehavior.Default;
			base.OnException(args);
		}

	}
}
