// <copyright file="Pheremone.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	/// A pheremone is a level indicator deposited at edges (step traversals)
	/// by the ant colony. 
	/// </summary>
	[DebuggerDisplay("Pheremone {PheremoneLevel}")]
	public class Pheremone
	{
		/// <summary>
		/// Constructor taking the initial value to set
		/// the level at.  This value should never be zero
		/// </summary>
		/// <param name="initialValue">The initial value to set the pheremone level to</param>
		/// <exception cref="System.ArgumentException">The provided initial value cannot be zero or less than</exception>
		public Pheremone(decimal initialValue)
		{
			if (initialValue <= 0)
				throw new ArgumentException("Initial pheremone level cannot be zero or less than zero");
			PheremoneLevel = initialValue;
		}

		/// <summary>
		/// The current level of pheremone at this point.
		/// </summary>
		public decimal PheremoneLevel { get; set; }

		/// <summary>
		/// Decay the pheremone at the prescribed evaporation rate.
		/// </summary>
		/// <param name="EvaporationRate">The pheremone evaporation rate</param>
		internal void Decay(decimal EvaporationRate)
		{
			decimal rate = 1 - EvaporationRate;
			PheremoneLevel =  PheremoneLevel * rate;
		}
	}
}
