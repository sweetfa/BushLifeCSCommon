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
 * @(#) ExitNullArgumentCheckAspect.cs
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
	/// Provide a null check of a specified argument on exit
	/// </summary>
	[Serializable]
	public sealed class ExitNullArgumentCheckAspect : NullArgumentCheckAspect
	{
		/// <summary>
		/// Check the state of any property or arguments as required
		/// on exit from the method the aspect is applied to
		/// </summary>
		/// <param name="args">The arguments for the method</param>
		/// <exception cref="ArgumentNullException">Thrown if an argument or property specified is null</exception>
		public override void OnExit(MethodExecutionArgs args)
		{
			base.OnExit(args);
			if (PropertyName != null)
				CheckProperty(args.Instance, PropertyName);
			if (ArgumentName != null)
				CheckArgument(args.Arguments, ArgumentName);
		}

	}
}
