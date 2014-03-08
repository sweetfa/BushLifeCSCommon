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
 * @(#) ReflectionUtils.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AU.Com.BushLife.Utils
{
	/// <summary>
	/// A class of utility methods around the Reflection package
	/// </summary>
	public static class ReflectionUtils
	{
		/// <summary>
		/// Format a method signature from the method supplied
		/// </summary>
		/// <param name="method">The method in question</param>
		/// <returns>A formatted string containing the method signature as it would appear in source codes</returns>
		public static string MethodSignature(this MethodBase method)
		{
			return string.Format("{0}( {1})", method.Name, method.MethodArgumentsAsString());
		}

		/// <summary>
		/// Format the arguments in the method execution arguments into a string
		/// containing the parameter types and their names, similiar to the actual source
		/// code for the method name.
		/// </summary>
		/// <param name="args">The method execution arguments</param>
		/// <returns>A string containing the method argument types and names joined together with ','</returns>
		public static string MethodArgumentsAsString(this MethodBase method)
		{
			return string.Join(", ", method.GetParameters().Select(pi => pi.ParameterType.Name + " " + pi.Name));
		}
	}
}
