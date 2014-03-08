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
 * @(#) IPath.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	///
	/// </summary>
	public interface IPath : ICloneable
	{
		/// <summary>
		/// The score for the path indicates the cost of the path.
		/// <para>The higher the cost, the worse the journey and the
		/// less likely the ant (or any other) is going to come back
		/// along this path</para>
		/// </summary>
		decimal Score { get; }

		/// <summary>
		/// This method is called when the ant is about to start it's journey
		/// or begin it's next journey.  This allows the path to reinitialise
		/// anything it needs to before the journey begins
		/// </summary>
		void Reset();

		/// <summary>
		/// Check to determine if the ant has already been to the step or not.
		/// <para>An ant is only ever allowed to go to a step once so when this
		/// method returns true the provided step is no longer considered traversable
		/// to</para>
		/// </summary>
		/// <param name="step">The step to check if it has been travelled to before</param>
		/// <returns>true if the step has been travelled to, otherwise false</returns>
		bool Contains(IStep step);

		/// <summary>
		/// Validate the resultant path.  This is a fail-safe hook to ensure that the
		/// path meets a set of criteria that the path must meet before it is considered
		/// a valid complete path.
		/// <para>If the path is invalid for any reason the Score value should be set to
		/// decimal.MaxValue</para>
		/// </summary>
		/// <param name="context"></param>
		void Validate(object validationObject);

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
