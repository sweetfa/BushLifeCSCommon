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

using AU.Com.BushLife.Exceptions;

namespace AU.Com.BushLife.Aspects.ExceptionHandlers
{
	/// <summary>
	/// Aspect to write a log entry of an exception and it's details to the log file
	/// </summary>
	[Serializable]
	[ProvideAspectRole(StandardRoles.ExceptionHandling)]
	[LogExceptionAttribute(AttributeExclude = true)]
	public sealed class LogExceptionAttribute : OnExceptionAspect
	{
		/// <summary>
		/// If flag is true just log the error and continue on.
		/// <para>If flag is false log the error and rethrow the exception</para>
		/// </summary>
		public bool IgnoreAndContinue { get; set; }

		/// <summary>
		/// If set the type of exception that this logger will work for
		/// <para>Add multiple advices for multiple exception types or leave null for any exception type</para>
		/// </summary>
		public Type ExceptionType { get; set; }

		private ILog Logger { get; set; }

		public override void RuntimeInitialize(System.Reflection.MethodBase method)
		{
			base.RuntimeInitialize(method);
			Logger = LogManager.GetLogger(method.DeclaringType.Assembly, method.DeclaringType);
		}

		/// <summary>
		/// Set the exception type to use for this aspect advice
		/// </summary>
		/// <param name="targetMethod"></param>
		/// <returns></returns>
		public override Type GetExceptionType(System.Reflection.MethodBase targetMethod)
		{
			if (ExceptionType != null)
				return ExceptionType;
			return base.GetExceptionType(targetMethod);
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
				if (!args.Exception.IsAlreadyHandled(this.GetType()))
				{
					string message = "\n" + args.Exception.Message;
					Exception exception = args.Exception;
					while (exception.InnerException != null)
					{
						message += "\n" + exception.Message;
						exception = exception.InnerException;
					}
					Logger.Error(message, args.Exception);
				}
			}
			if (IgnoreAndContinue)
				args.FlowBehavior = FlowBehavior.Continue;
			else if (ExceptionType != null && !args.Exception.IsAlreadyHandled(this.GetType()))
			{
				args.Exception = new AlreadyHandledException(this.GetType(), args.Exception);
				args.FlowBehavior = FlowBehavior.ThrowException;
			}
			else
				args.FlowBehavior = FlowBehavior.Default;
		}
	}
}
