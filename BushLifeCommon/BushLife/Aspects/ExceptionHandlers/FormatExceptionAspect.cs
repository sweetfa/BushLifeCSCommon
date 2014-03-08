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
 * @(#) FormatExceptionAspect.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

namespace AU.Com.BushLife.Aspects.ExceptionHandlers
{
	/// <summary>
	///
	/// </summary>
	[Serializable]
	[ProvideAspectRole(StandardRoles.ExceptionHandling)]
	[FormatExceptionAspect(AttributeExclude = true)]
	public sealed class FormatExceptionAspect : OnExceptionAspect
	{
		/// <summary>
		/// The type of the exception to rethrow this exception as.  Must
		/// be derived from System.Exception
		/// </summary>
		public Type ThrowExceptionType { get; set; }

		/// <summary>
		/// A message to add into the exception
		/// </summary>
		public string Message { get; set; }

		public override bool CompileTimeValidate(MethodBase method)
		{
			if (ThrowExceptionType == null)
				throw new InvalidAnnotationException(string.Format("ThrowExceptionType is not defined and must be defined: {0}.{1}()", method.ReflectedType.FullName, method.Name));
			if (!typeof(Exception).IsAssignableFrom(ThrowExceptionType))
				throw new InvalidAnnotationException(string.Format("ThrowExceptionType is not derived from System.Exception: {0}.{1}()", method.ReflectedType.FullName, method.Name));
			if (Message == null || Message.Length == 0)
				throw new InvalidAnnotationException(string.Format("Message is not defined: {0}.{1}()", method.ReflectedType.FullName, method.Name));
			return base.CompileTimeValidate(method);
		}

		/// <summary>
		/// Specify the type of exception that this aspect handles
		/// </summary>
		/// <param name="targetMethod"></param>
		/// <returns></returns>
		public override Type GetExceptionType(System.Reflection.MethodBase targetMethod)
		{
			return typeof(System.FormatException);
		}

		/// <summary>
		/// Handle the exception if it occurs
		/// </summary>
		/// <param name="args"></param>
		public override void OnException(MethodExecutionArgs args)
		{
			base.OnException(args);
			string message = string.Format("{0}({1})", Message, string.Join(", ", args.Arguments.Select(a => a == null ? "null" : a.ToString())));
			Exception ex = (Exception)Activator.CreateInstance(ThrowExceptionType, message);
			throw ex;
		}
	}
}
