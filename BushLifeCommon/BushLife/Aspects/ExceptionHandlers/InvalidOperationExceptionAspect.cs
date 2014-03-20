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
 * @(#) InvalidOperationExceptionAspect.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

namespace AU.Com.BushLife.Aspects.ExceptionHandlers
{
	/// <summary>
	/// An aspect to provide meaningful exception where an InvalidOperation exception is thrown
	/// </summary>
	[Serializable]
	[ProvideAspectRole(StandardRoles.ExceptionHandling)]
	[InvalidOperationExceptionAspect(AttributeExclude=true)]
	public sealed class InvalidOperationExceptionAspect : OnExceptionAspect
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

		/// <summary>
		/// If this flag is set to true then the arguments associated with the exception
		/// will not be included in the exception message
		/// <para>By default the arguments are included</para>
		/// </summary>
		public bool IgnoreArguments { get; set; }

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
		public override Type GetExceptionType(MethodBase targetMethod)
		{
			//base.GetExceptionType(targetMethod);
			return typeof(System.InvalidOperationException);
		}

		/// <summary>
		/// Handle the exception if it occurs
		/// </summary>
		/// <param name="args"></param>
		public override void OnException(MethodExecutionArgs args)
		{
			base.OnException(args);
			string message;
			if (IgnoreArguments == false && args.Arguments != null)
				message = string.Format("{0}({1})", Message, string.Join(", ", args.Arguments.Select(a => a == null ? "null" : a.ToString())));
			else
				message = Message;
			Exception ex = (Exception) Activator.CreateInstance(ThrowExceptionType, message);
			throw ex;
		}


		/// <summary>
		/// Compiler check for validity of usage of annotation
		/// </summary>
		/// <param name="method"></param>
		/// <param name="aspectInfo"></param>
		//public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
		//{
		//    if (ThrowExceptionType == null)
		//        throw new InvalidAnnotationException(string.Format("ThrowExceptionType is not defined and must be defined: {0}.{1}()", method.ReflectedType.FullName, method.Name));
		//    if (!typeof(Exception).IsAssignableFrom(ThrowExceptionType))
		//        throw new InvalidAnnotationException(string.Format("ThrowExceptionType is not derived from System.Exception: {0}.{1}()", method.ReflectedType.FullName, method.Name));
		//    if (Message == null || Message.Length == 0)
		//        throw new InvalidAnnotationException(string.Format("Message is not defined: {0}.{1}()", method.ReflectedType.FullName, method.Name));
		//}
	}
}
