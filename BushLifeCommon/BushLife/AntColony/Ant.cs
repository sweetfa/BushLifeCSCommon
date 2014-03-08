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
 * @(#) Ant.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Aspects;
using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	/// An ant forages along all the steps in a graph
	/// until it has travelled to all steps just once,
	/// or has found a point where it can not reach somewhere
	/// that it has not already been.
	/// </summary>
	[ProfilerAspect(IgnoreMillisecondsLessThan = 100)]
	public abstract class Ant<TPheremoneKey>
		where TPheremoneKey : IPheremoneKey
	{
		/// <summary>
		/// The initial value to set a pheremone to when adding
		/// an entry to the PheremoneMap.  Whilst this should be
		/// zero in nature, this tends to exclude a node/edge
		/// from natural selection so a range between 0.01 and 0.5
		/// is suggested.
		/// </summary>
		protected decimal InitialPheremoneValue { get; set; }

		/// <summary>
		/// Map of nodes/edges to track pheremone levels associated with 
		/// each node/edge of a path.
		/// <para>This value is supplied by the colony and is 
		/// only read by the ant.  The colony is responsible 
		/// for updating the map after each ant trip has completed</para>
		/// <para>If a path is found that has not been configured in
		/// the map however, it will be added</para>
		/// <para>NOTE: The implementation of the dictionary must be 
		/// thread safe (concurrent) type as multiple threads
		/// are used to perform the processing</para>
		/// </summary>
		private IDictionary<TPheremoneKey, Pheremone> PheremoneMap { get; set; }

		/// <summary>
		/// The random number generator instance (maintained per thread)
		/// to maximise the successfulness of the random generator
		/// </summary>
		protected Random randomInstance;

		public void Initialise(Int32 seed, IDictionary<TPheremoneKey,Pheremone> pheremoneMap, decimal initialPheremoneLevel)
		{
			DateTime now = DateTime.Now;
			randomInstance = new Random(seed * now.Millisecond);
			PheremoneMap = pheremoneMap;
			InitialPheremoneValue = initialPheremoneLevel;
		}



		/// <summary>
		/// Constructor for the ant
		/// </summary>
		/// <param name="seed">The seed for the random number generator.  This
		/// is used to overcome the less than randomness of multiple threads
		/// hitting the random number generator for a request in a small
		/// time period</param>
		/// <param name="pheremoneMap">The colony supplied map of routes and how favourable they are</param>
		//public Ant(int seed, IDictionary<TPheremoneKey,Pheremone> pheremoneMap, ForageMode forageMode)
		//{
		//    DateTime now = DateTime.Now;
		//    randomInstance = new Random(seed * now.Millisecond);
		//    PheremoneMap = pheremoneMap;
		//    AntForageMode = forageMode;
		//}


		/// <summary>
		/// Determine which edge traversal to travel next.
		/// <para>Each traversal is given a probability based on the
		/// current pheremone level for the traversal combined with
		/// it's score</para>
		/// </summary>
		/// <param name="nextPossibleEdges">The edges that can be traversed from the current point</param>
		/// <returns>The next edge selected</returns>
		protected TPheremoneKey SelectNext(ICollection<TPheremoneKey> nextPossibleSteps)
		{
			if (nextPossibleSteps.Count() == 0)
				throw new ArgumentException("No next steps to find");

			IList<decimal> allScores = CalculatePheremoneLevels(nextPossibleSteps);
			decimal totalScore = allScores.Sum(s => s);
			if (totalScore == 0)
				totalScore = allScores.Count;

			IList<KeyValuePair<TPheremoneKey, decimal>> edgeProbability = nextPossibleSteps
				.ToDictionary(k => k, s => CalculatePheremoneLevel(s) / totalScore)
				.OrderByDescending(kvp => kvp.Value)
				.ToList();

			decimal selectedValue = (decimal) randomInstance.NextDouble();
			decimal accumulator = 0.0m;
			foreach (KeyValuePair<TPheremoneKey, decimal> kvp in edgeProbability)
			{
				decimal probability = kvp.Value;
				accumulator += probability;
				if (accumulator > selectedValue)
					return kvp.Key;
			}
			return edgeProbability.Last().Key;
		}

		/// <summary>
		/// Calculate the pheremone level for all possible next locations
		/// </summary>
		/// <param name="nextPossibleSteps">The collection of next potential destinations</param>
		/// <returns>The list of pheremone levels for each key in the list</returns>
		protected IList<decimal> CalculatePheremoneLevels(ICollection<TPheremoneKey> nextPossibleSteps)
		{
			return nextPossibleSteps.Select(v => CalculatePheremoneLevel(v)).ToList();
		}

		/// <summary>
		/// Calculate the pheremone level for a single key
		/// </summary>
		/// <param name="key">The key to calculate the pheremone level for</param>
		/// <returns>The calculated pheremone level</returns>
		protected decimal CalculatePheremoneLevel(TPheremoneKey key)
		{
			decimal score = key.Score;
			decimal pheremone = PheremoneMap[key].PheremoneLevel;
			if (score == 0)
				score = 1;
			return (1 / score) * pheremone;
		}

	}
}
