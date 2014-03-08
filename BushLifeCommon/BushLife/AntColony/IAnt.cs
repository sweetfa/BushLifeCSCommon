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
 * @(#) IAnt.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	/// The interface an ant must implement
	/// </summary>
	public interface IAnt<TPheremoneKey>
	{
		/// <summary>
		/// The context contains the information the ant uses
		/// and updates during it's travels
		/// </summary>
		IAntColonyContext Context { get; set; }

		/// <summary>
		/// Initialise the ant.  Perform the necessary initialisation that is required
		/// before an Ant can begin it's journey
		/// </summary>
		/// <param name="seed">A number to use as a seed to the random number generator</param>
		/// <param name="pheremoneMap">The pherenome map that all ants use (same for all ants)</param>
		/// <param name="initialPheremoneLevel">The initial level to set the pheremones for on the map</param>
		void Initialise(Int32 seed, IDictionary<TPheremoneKey, Pheremone> pheremoneMap, decimal initialPheremoneLevel);

		/// <summary>
		/// Send the ant on it's journey
		/// </summary>
		/// <param name="context">The information the ant needs to travel with</param>
		void Forage(IAntColonyContext context);
	}
}
