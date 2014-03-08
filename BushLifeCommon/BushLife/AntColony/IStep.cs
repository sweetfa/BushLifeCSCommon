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
 * @(#) IStep.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	/// A step is a point that the ant must traverse through on it's journey
	/// <para>All steps must be traversed, once and only once on the journey</para>
	/// </summary>
	public interface IStep : IPheremoneKey, ICloneable
	{
		/// <summary>
		/// The name of the step
		/// </summary>
		string StepName { get; }

		/// <summary>
		/// Select the steps that are available from this step
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		IEnumerable<IPheremoneKey> Available(IAntColonyContext context);

		/// <summary>
		/// The equals method must be overridden for this object
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		bool Equals(object obj);

		/// <summary>
		/// The GetHashCode method must be overridden for this object
		/// </summary>
		/// <returns></returns>
		int GetHashCode();
	}
}
