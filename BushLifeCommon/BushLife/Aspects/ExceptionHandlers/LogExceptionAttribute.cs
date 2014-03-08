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
 * @(#) LogExceptionAttribute.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

using log4net;

namespace AU.Com.BushLife.Aspects.ExceptionHandlers
{
	/// <summary>
	/// Aspect to write a log entry of an exception and it's details to the log file
	/// </summary>
	[Serializable]
	[ProvideAspectRole(StandardRoles.ExceptionHandling)]
	[LogExceptionAttribute(AttributeExclude = true)]
	public class LogExceptionAttribute : OnExceptionAspect
	{
		/// <summary>
		/// If flag is true just log the error and continue on.
		/// <para>If flag is false log the error and rethrow the exception</para>
		/// </summary>
		public bool IgnoreAndContinue { get; set; }

		private ILog Logger { get; set; }

		public override void RuntimeInitialize(System.Reflection.MethodBase method)
		{
			base.RuntimeInitialize(method);
			Logger = LogManager.GetLogger(method.DeclaringType.Assembly, method.DeclaringType);
		}

		/// <summary>
		/// Handle the exception by writing it's details to the log file if
		/// logging is enabled at the error level
		/// </summary>
		/// <param name="args"></param>
		public override void OnException(MethodExecutionArgs args)
		{
			base.OnException(args);
			if (Logger.IsErrorEnabled)
			{
				string message = args.Exception.Message;
				Exception exception = args.Exception;
				while (exception.InnerException != null)
				{
					message += "\n" + exception.Message;
					exception = exception.InnerException;
				}
				Logger.Error(message, exception);
			}
			if (IgnoreAndContinue)
				args.FlowBehavior = FlowBehavior.Continue;
			else
				args.FlowBehavior = FlowBehavior.Default;
		}
	}
}
