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
 * @(#) Colony.cs
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.AntColony
{

	/// <summary>
	/// The main controlling class for the ant colony and the ant colony processing
	/// </summary>
	public class Colony<TAnt, TPheremoneKey>
		where TAnt : IAnt<TPheremoneKey>, new()
	{
		/// <summary>
		/// The number of times an ant will get sent out to forage
		/// </summary>
		public Int32 Iterations { get; set; }

		/// <summary>
		/// The number of ants that live and work in the colony
		/// </summary>
		public Int32 NumberOfAnts { get; set; }

		/// <summary>
		/// The initial level to set for each edge traversal
		/// in the pheremone trail.  A value of zero will fail
		/// to locate an optimal solution.
		/// <para>The recommended value is 0.001</para>
		/// </summary>
		public decimal InitialPheremoneLevel { get; set; }

		/// <summary>
		/// The rate at which pheremones will decay.  Over time
		/// the pheremone level for any particular edge traversal 
		/// will reduce, so less ants travelling the edge will 
		/// move it lower down the list of edges to travel next
		/// time an ant gets to the same point
		/// </summary>
		public decimal EvaporationRate { get; set; }

		/// <summary>
		/// The context that was initially provided to the colony to 
		/// get the foraging started
		/// <para>When completed the information regarding the
		/// best path will be placed in the InitialContext
		/// for return to the invoker</para>
		/// </summary>
		private IAntColonyContext InitialContext;

		/// <summary>
		/// The contexts that each ant use.  This context is a clone of
		/// the InitialContext and a context exists for each ant.
		/// <para>Each ant will forage with it's own context
		/// providing the results of it's travels in it's own
		/// context</para>
		/// </summary>
		private IList<IAntColonyContext> Contexts;

		/// <summary>
		/// The map of pheremone levels that exist for each traversal.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public IDictionary<TPheremoneKey, Pheremone> PheremoneMap;

		private Action<IAntColonyContext, IDictionary<TPheremoneKey, Pheremone>, decimal> InitialisePheremonesAction = null;

		/// <summary>
		/// Constructor for the colony.  
		/// </summary>
		/// <param name="numberOfAnts">The number of ants that live in the colony</param>
		/// <param name="numberOfIterations">The number of times an ant will forage</param>
		/// <param name="initialPheremoneLevel">The initial level to place on each edge</param>
		/// <param name="evaporationRate">The rate of decay of pheremones</param>
		public Colony(Int32 numberOfAnts, Int32 numberOfIterations, decimal initialPheremoneLevel, decimal evaporationRate, Action<IAntColonyContext, IDictionary<TPheremoneKey,Pheremone>, decimal> initialisePheremonesAction = null)
		{
			NumberOfAnts = numberOfAnts;
			Iterations = numberOfIterations;
			InitialPheremoneLevel = initialPheremoneLevel;
			EvaporationRate = evaporationRate;
			InitialisePheremonesAction = initialisePheremonesAction;
		}

		/// <summary>
		/// Initialise the pheremone map
		/// </summary>
		public void InitialisePheremones()
		{
			PheremoneMap = new ConcurrentDictionary<TPheremoneKey, Pheremone>();
			if (InitialisePheremonesAction != null)
				InitialisePheremonesAction(InitialContext, PheremoneMap, InitialPheremoneLevel);
		}

		/// <summary>
		/// Initialise each of the ants.
		/// <para>At this point is only required to make a clone
		/// of the InitialContext for each of the ants to use</para>
		/// </summary>
		/// <param name="context">The initial context</param>
		public void InitialiseAnts(IAntColonyContext context)
		{
			InitialContext = context;
			Contexts = new List<IAntColonyContext>();
			for (int i = 0; i < NumberOfAnts; ++i)
			{
				IAntColonyContext c = context.Clone() as IAntColonyContext;
				c.Id = i + 1;
				Contexts.Add(c);
			}
		}

		/// <summary>
		/// Set each of the ants out on it's forage track,
		/// and wait for them all to return.
		/// <para>Select the best path from all of paths travelled by
		/// the ants and drop pheremones on that path.</para>
		/// <para>Decay the pheremone level in all paths in the pheremone map</para>
		/// <para>Repeat the above process for the required number of iterations</para>
		/// <para>Return the result in the InitialContext.BestPaths property</para>
		/// </summary>
		public void RunAnts()
		{
			const int MaxRepetitions = 2000;
			Int32 sameScore = 0;
			InitialisePheremones();
			ICollection<IPath> paths = new List<IPath>();
			for (int i = 0; i < Iterations; ++i)
			{
				ICollection<TAnt> ants = new List<TAnt>(NumberOfAnts);
				for (int a = 0; a < NumberOfAnts; ++a)
				{
					IAntColonyContext c = Contexts[a];
					c.Reset();
					TAnt ant = new TAnt();
					ant.Initialise(a * i + 1, PheremoneMap, InitialPheremoneLevel);
					ant.Context = c;
					ants.Add(ant);
				}

				ants.AsParallel().ForEach(a => a.Forage(a.Context));

				ICollection<IPath> bestPaths = DetermineBestPaths();
				// If new paths are more efficient than old paths save them
				if (paths.Count() == 0 || (paths.First().Score > bestPaths.First().Score))
				{
					paths = new List<IPath>(bestPaths);
					sameScore = 0;
				}
				// If same score merge the set of paths
				else if (paths.First().Score == bestPaths.First().Score)
				{
					paths = paths.Union(bestPaths).Distinct().ToList();
				}

				// TODO: Make the exit condition a predicate instead
				if (sameScore++ > MaxRepetitions)
				{
					Console.WriteLine(string.Format("Exiting due to repetition of same score on iteration {0}", i));
					break;
				}
				//Console.WriteLine("Iteration {0}: Score: {1} Returned: {2}", i, paths.First().Score, bestPaths.First().Score);

				// Manage the pheremone levels after the iteration completes
				PheremoneMap.ForEach(e => e.Value.Decay(EvaporationRate));
				if (paths.First().Score < decimal.MaxValue)
					paths.ForEach(p => DepositPheremones(PheremoneMap, (dynamic) p));
			}
			InitialContext.BestPaths = paths.OfType<IPath>().ToList();
		}

		/// <summary>
		/// Determine the best path from all of the paths travelled by
		/// the ants in a single iteration.  The best path is the path
		/// with the lowest score. 
		/// <para>Multiple different paths may have the same score</para>
		/// </summary>
		/// <returns>The best path (or paths)</returns>
		private ICollection<IPath> DetermineBestPaths()
		{
			decimal minScore = Contexts
				.Select (c => c.Path)
				.Min(c => c.Score);

			return Contexts
				.Where(c => c.Path.Score == minScore)
				.Select(c => c.Path.Clone() as IPath)
				.Distinct()
				.ToList();
		}

		/// <summary>
		/// Leave pheremones on the edges traversed along the considered best path.
		/// </summary>
		/// <param name="PheremoneMap">The map of pheremone levels for all edges</param>
		/// <param name="path">The path travelled by the ant</param>
		private void DepositPheremones(IDictionary<TPheremoneKey, Pheremone> PheremoneMap, INodePath path)
		{
			decimal pathScore = path.Score == 0 ? 1 : path.Score;
			decimal delta = (1.0m / pathScore);
			if (path.Steps != null)
			{
				foreach (TPheremoneKey steps in path.Steps)
				{
					//if (!PheremoneMap.ContainsKey(steps))
					//    PheremoneMap[steps] = new Pheremone(InitialPheremoneLevel);
					PheremoneMap[steps].PheremoneLevel += delta;
				}
			}
		}

		/// <summary>
		/// Leave pheremones on the edges traversed along the considered best path.
		/// </summary>
		/// <param name="PheremoneMap">The map of pheremone levels for all edges</param>
		/// <param name="path">The path travelled by the ant</param>
		private void DepositPheremones(IDictionary<TPheremoneKey, Pheremone> PheremoneMap, IPathPath path)
		{
			decimal pathScore = path.Score;
			decimal delta = (1.0m / pathScore);
			foreach (TPheremoneKey steps in path.Edges)
			{
				//if (!PheremoneMap.ContainsKey(steps))
				//    PheremoneMap[steps] = new Pheremone(InitialPheremoneLevel);
				PheremoneMap[steps].PheremoneLevel += delta;
			}
		}

	}
}
