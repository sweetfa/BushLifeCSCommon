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
 * @(#) IAntColonyContext.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	/// The context passed to an ant to process.
	/// </summary>
	public interface IAntColonyContext : ICloneable
	{
		/// <summary>
		/// The id of the ant that owns this context.
		/// <para>The initial context will have zero for this value</para>
		/// <para>All other context clones will have the id of the ant</para>
		/// </summary>
		Int32 Id { get; set; }

		/// <summary>
		/// The available steps an ant can forage over
		/// </summary>
		ICollection<IStep> Steps { get; set; }

		/// <summary>
		/// The path that this ant is currently utilising
		/// <para>This is the path the ant has found during it's iteration</para>
		/// </summary>
		IPath Path { get; set; }

		/// <summary>
		/// The best paths located by all the ants
		/// <para>This is intended to be returned from the colony as
		/// an amalgamation of all the best paths found by all the ants
		/// in the last iteration</para>
		/// </summary>
		ICollection<IPath> BestPaths { get; set; }


		/// <summary>
		/// Reset the context for the next iteration.
		/// <para>This hook allows the colony to reset
		/// a context prior to the execution of an iteration</para>
		/// </summary>
		void Reset();
	}
}
