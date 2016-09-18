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
    [MulticastAttributeUsage(MulticastTargets.Method, Inheritance = MulticastInheritance.Multicast)]
    public sealed class ShowExceptionAttribute : OnExceptionAspect
	{
		/// <summary>
		/// The caption on the message box.  If this value is null no caption is displayed
		/// </summary>
		public string Caption { get; set; }

        /// <summary>
        /// The message to display in the dialog box
        /// <para>This parameter is mutually exclusive with the Formatter parameter</para>
        /// </summary>
        public string Message { get; set; }

		/// <summary>
		/// If flag is set to true the details of the exception are added to the message
		/// </summary>
		public bool ShowExceptionDetail { get; set; }

		/// <summary>
		/// Flag indicating if this attribute is an exclusion attribute.
		/// <para>No message will be shown for the specific exception if this attribute is true</para>
		/// <para>If this value is set requires a SpecificExceptionType to also be set</para>
		/// </summary>
		public bool ExcludeMessageShow { get; set; }

		/// <summary>
		/// If this value is set only trigger this advice on the specific exception type
		/// </summary>
		public Type SpecificExceptionType { get; set; }

        /// <summary>
        /// If this value is set the message displayed will be generated through this formatter
        /// <para>This parameter is ignored if ExcludeMessageShow is true</para>
        /// <para>This parameter is mutually exclusive with the Message parameter</para>
        /// </summary>
        public Type Formatter { get; set; }

        /// <summary>
        /// The instantiated instance of the message formatter selected
        /// </summary>
        private ExceptionFormatter MessageFormatter { get; set; }

		/// <summary>
		/// Ensure the required properties are provided at compile time
		/// </summary>
		/// <param name="method"></param>
		/// <returns>true if no compilation issues</returns>
		public override bool CompileTimeValidate(MethodBase method)
		{
			if (ExcludeMessageShow)
			{
				if (SpecificExceptionType == null)
					throw new InvalidAnnotationException(string.Format("A specific exception must be defined when ExcludeMessageShow == true: {0}.{1}()", method.ReflectedType.FullName, method.Name));
                if (Formatter == null)
                    throw new InvalidAnnotationException(string.Format("No message will be shown.  Formatter argument is useless: {0}.{1}()", method.ReflectedType.FullName, method.Name));
                if (string.IsNullOrEmpty(Message))
                    throw new InvalidAnnotationException(string.Format("No message will be shown.  Message argument is useless: {0}.{1}()", method.ReflectedType.FullName, method.Name));
            }
            else
			{
                if (string.IsNullOrEmpty(Message) && Formatter == null)
                    throw new InvalidAnnotationException(string.Format("Message and Formatter property are not defined and one only must be defined: {0}.{1}()", method.ReflectedType.FullName, method.Name));
                if (Formatter != null)
                {
                    if ((Formatter as ExceptionFormatter) != null)
                        throw new InvalidAnnotationException(string.Format("Formatter property must be of type ExceptionFormatter: {0}.{1}()", method.ReflectedType.FullName, method.Name));
                }
            }
            return base.CompileTimeValidate(method);
		}

        public override void RuntimeInitialize(MethodBase method)
        {
            base.RuntimeInitialize(method);
            if (Formatter != null)
            {
                MessageFormatter = Activator.CreateInstance(Formatter) as ExceptionFormatter;
            }
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
				if (!ExcludeMessageShow)
				{
                    string message = BuildMessage(args.Exception);

					// Get the parent window if available
					DependencyObject dependencyObject = args.Instance as DependencyObject;
					Window window = null;
					if (dependencyObject != null)
						window = Window.GetWindow(dependencyObject);

					if (window != null)
					{
                        if (Caption != null)
                            ShowWindowMessage(window, Caption, Message);
						else
                            ShowWindowMessage(window, window.Name, Message);
					}
					else
					{
                        if (Caption != null)
                            ShowMessage(Caption, message);
						else
                            ShowMessage("Exception Occurred", message);
					}
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

        /// <summary>
        /// Build a message from the exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private string BuildMessage(Exception exception)
        {
            var message = Message;
            if (ShowExceptionDetail && message != null)
                message = string.Format("{0}\n\n{1}\n{2}", Message, exception.GetType().FullName, exception.Message);
            if (Formatter != null)
            {
                if (MessageFormatter == null)
                    MessageFormatter = Activator.CreateInstance(Formatter) as ExceptionFormatter;
                message = MessageFormatter.FormatException(exception, ShowExceptionDetail);
            }
            return message;
        }

        /// <summary>
        /// Show a message box associated with a Window
        /// </summary>
        /// <param name="window">The window the message is to associate with</param>
        /// <param name="caption">The caption on the message box</param>
        /// <param name="message">The message to display in the message box</param>
        private void ShowWindowMessage(Window window, string caption, string message)
        {
            System.Windows.MessageBox.Show(window, message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Show a message box 
        /// </summary>
        /// <param name="caption">The caption on the message box</param>
        /// <param name="message">The message to display in the message box</param>
        private void ShowMessage(string caption, string message)
        {
            System.Windows.MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
