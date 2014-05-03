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
 * @(#) ProfilerAspect.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Diagnostics;

using log4net;
using log4net.Core;

using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;

namespace AU.Com.BushLife.Aspects
{
    /// <summary>
    /// Aspect to profile method execution with output directed to log4net
	/// if the debug level is set to INFO for the logger
	/// <para>Note the logger is the logger for the method that is wrapped,
	/// not this class</para>
    /// <para>To use: add "[assembly: ProfilerAspect()]" to AssemblyInfo.cs </para>
    /// </summary>
    [Serializable]
    [ProfilerAspect(AttributeExclude = true)]
	[ProvideAspectRole(StandardRoles.PerformanceInstrumentation)]
    public sealed class ProfilerAspect : OnMethodBoundaryAspect
    {
		/// <summary>
		/// The log4net logger to use
		/// </summary>
		private ILog Logger { get; set; }

		/// <summary>
		/// Ignore output of any trace for methods that execute quicker than this time
		/// </summary>
		public Int32 IgnoreMillisecondsLessThan { get; set; }

		/// <summary>
		/// The action to use to output the trace information
		/// <para>The default is to use Debug.WriteLine to place output on the Debugger console</para>
		/// <para>Alternative actions could be: s => LogManager.GetLogger(this.GetType()).Info(s)</para>
		/// </summary>
		private Action<ILog, string> LogAction;

		private string Message { get; set; }

		/// <summary>
		/// Initialise the logger to use to output the log messages
		/// </summary>
		/// <param name="method"></param>
		public override void RuntimeInitialize(MethodBase method)
		{
			base.RuntimeInitialize(method);
			Logger = LogManager.GetLogger(method.DeclaringType.Assembly, method.DeclaringType);
			LogAction = (logger, str) => logger.Info(str);

			Message = string.Format("{3}] {0}::{1}{2}({3})", 
					Thread.CurrentThread.Name,
					method.DeclaringType.FullName,
					method.Name,
					GenericArguments(method),
					Arguments(method)
					);
		}

		private string GenericArguments(MethodBase method)
		{
			var genericArguments = method.GetGenericArguments();
			if (genericArguments.Count() > 0)
				return string.Format("<{0}>", string.Join(",", genericArguments.Select(a => a.Name)));
			return "";
		}

		private string Arguments(MethodBase method)
		{
			var arguments = method.GetParameters();
			if (arguments.Count() > 0)
				return string.Join(",", arguments.Select(a => a.ParameterType.Name + " " + a.Name));
			return "";
		}

        /// <summary>
        /// Start a new stop watch for this method
        /// </summary>
        /// <param name="args"></param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = Stopwatch.StartNew();
        }

        /// <summary>
        /// Stop the stopwatch for this method and output the time taken
        /// </summary>
        /// <param name="args"></param>
        public override void OnExit(MethodExecutionArgs args)
        {
            Stopwatch stopWatch = (Stopwatch)args.MethodExecutionTag;
            stopWatch.Stop();
			if (stopWatch.ElapsedMilliseconds >= IgnoreMillisecondsLessThan)
			{
				string output = string.Format("{0} executed in {1} milliseconds",
					Message,
					stopWatch.ElapsedMilliseconds);
				LogAction(Logger, output);
			}
        }
    }
}
