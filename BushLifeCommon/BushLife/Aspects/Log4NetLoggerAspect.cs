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
 * @(#) Log4NetLoggerAspect.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

using log4net;
using log4net.Config;
using log4net.Repository;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Aspects
{
	/// <summary>
	/// Aspect using Log4Net to log entry, exit, and exception trace for methods that
	/// are crosscut.
	/// </summary>
	[Serializable]
    [ProvideAspectRole(StandardRoles.Tracing)]
    [Log4NetLoggerAspect(AttributeExclude=true)]
	[MulticastAttributeUsage(MulticastTargets.Method, Inheritance = MulticastInheritance.Multicast)]
	public sealed class Log4NetLoggerAspect : OnMethodBoundaryAspect
	{
		private static bool Initialised = false;

		private ILog Logger { get; set; }

		/// <summary>
		/// The name of the logfile (no path)
		/// <para>The log4net.config file should include the following
		/// definition for the file in a rollingfileappender</para>
		/// <para>        &lt;file type="log4net.Util.PatternString" value="%property{LogFileName}"/></para>
		/// <para>The log file is created in the LocalApplicationData directory</para>
		/// </summary>
		public string LogFileName { get; set; }

		/// <summary>
		/// The name of the log4net config file
		/// <para>This log file must exist in the directory of the assembly the attribute is applied to</para>
		/// </summary>
		public string ConfigFileName { get; set; }

		/// <summary>
		/// Check that the minimum required parameters are set during compilation
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		public override bool CompileTimeValidate(MethodBase method)
		{
			if (ConfigFileName == null || ConfigFileName.Length == 0)
				throw new InvalidAnnotationException(string.Format("ConfigFileName is not defined and must be defined: {0}.{1}()", method.ReflectedType.FullName, method.Name));
			if (LogFileName == null || LogFileName.Length == 0)
				throw new InvalidAnnotationException(string.Format("LogFileName is not defined and must be defined: {0}.{1}()", method.ReflectedType.FullName, method.Name));
			return base.CompileTimeValidate(method);
		}

		/// <summary>
		/// If not already initialised initialise the log4net system
		/// <para>NOTE: This will not support multiple assemblies using the attribute concurrently at the moment</para>
		/// </summary>
		/// <param name="method"></param>
		public override void RuntimeInitialize(MethodBase method)
		{
			if (Initialised == false)
			{
				lock (this)
				{
					if (Initialised == false)
					{
						InitialiseLog4Net(method);
						Initialised = true;
					}
				}
			}
			// Initialise the logger to use for this instance of the aspect
			Logger = LogManager.GetLogger(method.DeclaringType.Assembly, method.DeclaringType);
		}

		/// <summary>
		/// Initialise the log4net framework
		/// </summary>
		/// <param name="method"></param>
		private void InitialiseLog4Net(MethodBase method)
		{
			// Initialise the underlying log4net system. This only needs to be done once per application
			string configDirectory = Path.GetDirectoryName(method.DeclaringType.Assembly.Location);
			string configPath = Path.Combine(configDirectory, ConfigFileName);
			FileInfo fileInfo = new FileInfo(configPath);
			ILoggerRepository repository = LogManager.GetRepository(method.DeclaringType.Assembly);

			string pathName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string logFilePath = Path.Combine(pathName, LogFileName);
			GlobalContext.Properties["LogFileName"] = logFilePath;
			XmlConfigurator.Configure(repository, fileInfo);
		}

		/// <summary>
		/// Output a logger message indicating method entry
		/// </summary>
		/// <param name="args"></param>
		public override void OnEntry(MethodExecutionArgs args)
		{
			if (Logger.IsInfoEnabled)
			{
                string message = args.Method.MethodSignature() + ": Entered";
				Logger.Info(message);
			}
			if (Logger.IsDebugEnabled)
			{
				foreach (var arg in args.Arguments)
				{
					Logger.DebugFormat("{0}: {1}", 
                        arg == null ? "null" : arg.GetType().Name, 
                        arg == null ? "null" : arg.ToString());
				}
			}
			base.OnEntry(args);
		}

		/// <summary>
		/// Output a logger message indicating method exit
		/// </summary>
		/// <param name="args"></param>
		public override void OnExit(MethodExecutionArgs args)
		{
			base.OnExit(args);
			if (Logger.IsInfoEnabled)
			{
				string message = args.Method.MethodSignature() + ": Exited";
				Logger.Info(message);
			}
			if (Logger.IsDebugEnabled)
			{
				foreach (var arg in args.Arguments)
				{
					Logger.DebugFormat("{0}: {1}", 
                        arg == null ? "null" : arg.GetType().Name, 
                        arg == null ? "null" : arg.ToString());
				}
				Logger.DebugFormat("Returning {0}: {1}", 
                    args.ReturnValue == null ? "null" : args.ReturnValue.GetType().Name, 
                    args.ReturnValue == null ? "null" : args.ReturnValue.ToString());
			}
		}

		/// <summary>
		/// Output a logger message indicating an exception occurred in the message
		/// </summary>
		/// <param name="args"></param>
		public override void OnException(MethodExecutionArgs args)
		{
			base.OnException(args);
			if (Logger.IsErrorEnabled)
			{
				string message = args.Method.MethodSignature() + ": Exception";
				Logger.Error(message, args.Exception);
			}
		}

	}
}
