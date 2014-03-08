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
 * @(#) NullArgumentCheckAspect.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using PostSharp.Aspects;

namespace AU.Com.BushLife.Aspects
{
	/// <summary>
	/// Base class for aspects performing null argument checking
	/// </summary>
	[Serializable]
	public abstract class NullArgumentCheckAspect : OnMethodBoundaryAspect
	{
		/// <summary>
		/// The argument name to check for the method this aspect is attached to
		/// <para>Where the argument name is a property hierarchy
		/// descend each level of the heirarchy checking for nullness</para>
		/// </summary>
		public string ArgumentName { get; set; }
		/// <summary>
		/// The index of the argument name to check.  This will be overridden 
		/// if the ArgumentName property is set as well.
		/// </summary>
		public Int32 ArgumentIndex { get; set; }

		/// <summary>
		/// The property name to check in the instance this aspect is attached to.
		/// <para>Where the property name is a property hierarchy
		/// descend each level of the heirarchy checking for nullness</para>
		/// </summary>
		public string PropertyName { get; set; }

		private MethodBase Method;
		private string MethodName;

		/// <summary>
		/// Construct the method name for later use
		/// </summary>
		/// <param name="method"></param>
		/// <param name="aspectInfo"></param>
		public override void CompileTimeInitialize(System.Reflection.MethodBase method, AspectInfo aspectInfo)
		{
			base.CompileTimeInitialize(method, aspectInfo);
			MethodName = string.Format("{0}.{1}", method.DeclaringType.Name, method.Name);
			if (ArgumentName != null)
			{
				string name = ArgumentName;
				if (name.Contains("."))
					name = name.Substring(0, name.IndexOf('.'));
				ParameterInfo[] parameters = method.GetParameters();
				foreach (ParameterInfo parameter in parameters)
				{
					if (parameter.Name == name)
						ArgumentIndex = parameter.Position;
				}
			}
		}

		/// <summary>
		/// Save the method that this aspect is attached to
		/// </summary>
		/// <param name="method"></param>
		public override void RuntimeInitialize(MethodBase method)
		{
			Method = method;
			base.RuntimeInitialize(method);
		}

		/// <summary>
		/// Check that a argument to the method is not null.
		/// </summary>
		/// <param name="argumentName">The name of the argumentName to check</param>
		/// <exception cref="ArgumentNullException">The argument value is null</exception>
		/// <exception cref="ArgumentException">The specified argument name does not exist in the object</exception>
		protected void CheckArgument(Arguments args, string argumentName)
		{
			if (argumentName.Contains("."))
			{
				object instance = args[ArgumentIndex];
				// iterative/recursive loop to check each level of the property
				string[] propertyNames = argumentName.Split('.');
				foreach (string property in propertyNames.Skip(1))
				{
					CheckProperty(instance, property);
					// Get the child instance
					instance = GetInstance(instance, property);
				}
			}
			else
			{
				if (args[ArgumentIndex] == null)
					throw new ArgumentNullException(string.Format("{0} has Null Property {1}", MethodName, argumentName));
			}
		}

		/// <summary>
		/// Check that a property in the supplied instance is not null.
		/// <para>Where the property name is a property hierarchy
		/// descend each level of the heirarchy checking for nullness</para>
		/// </summary>
		/// <param name="instance">The instance of the object to check the property in</param>
		/// <param name="propertyName">The name of the property to check, with full-stops between hierarchy levels</param>
		/// <exception cref="ArgumentNullException">The property value is null</exception>
		/// <exception cref="ArgumentException">The specified property name does not exist in the object</exception>
		protected void CheckProperty(object instance, string propertyName)
		{
			if (propertyName.Contains("."))
			{
				// iterative/recursive loop to check each level of the property
				foreach (string property in propertyName.Split('.'))
				{
					CheckProperty(instance, property);
					// Get the child instance
					instance = GetInstance(instance, property);
				}
			}
			else
			{
				if (GetInstance(instance, propertyName) == null)
					throw new ArgumentNullException(string.Format("{0} has Null Property {1}", MethodName, propertyName));
			}
		}

		/// <summary>
		/// Extract the instance value for the specified property name
		/// </summary>
		/// <param name="instance">The instance of the object to check the property in</param>
		/// <param name="propertyName">The name of the property to check, with full-stops between hierarchy levels</param>
		/// <returns>The value for the property name extracted from the supplied instance</returns>
		/// <exception cref="ArgumentNullException">The property value is null</exception>
		/// <exception cref="ArgumentException">The specified property name does not exist in the object</exception>
		private object GetInstance(object instance, string propertyName)
		{
			PropertyInfo property = instance.GetType().GetProperty(propertyName);
			if (property == null)
				throw new ArgumentException(string.Format("{0} does not contain a property called {1}", MethodName, propertyName));
			object value = property.GetValue(instance, null);
			return value;
		}
	}
}
