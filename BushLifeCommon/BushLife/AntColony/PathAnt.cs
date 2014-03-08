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
 * @(#) PathAnt.cs
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
	///
	/// </summary>
	public class PathAnt : Ant<IEdge>, IAnt<IEdge>
	{
		#region IAnt Members

		public IAntColonyContext Context { get; set; }

		/// <summary>
		/// The main task of an ant is to forage, and this method is the entry
		/// point for the ant thread to perform its travels.
		/// </summary>
		/// <param name="context">The context providing details for this particular ant
		/// <para>This context must be a separate instance for each thread.</para></param>
		/// <exception cref="System.ArgumentNullException">The context.Steps property is null</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">The context.Steps property is empty</exception>
		[EntryNullArgumentCheckAspect(ArgumentName = "context.Steps")]
		public void Forage(IAntColonyContext context)
		{
			if (context.Steps.Count == 0)
				throw new ArgumentOutOfRangeException("No steps provided");

			ICollection<IStep> availableSteps = context.Steps;
			Int32 randomIndex = randomInstance.Next(availableSteps.Count());

			// Set the initial start point
			IStep currentStep = default(IStep);
			if (availableSteps.Count > 0)
			{
				//Console.WriteLine(string.Format("Initial random index = {0}", randomIndex));
				currentStep = availableSteps.ElementAt(randomIndex);
				//availableSteps.Remove(currentStep);
			}

			// Process each step until no steps remain
			ICollection<IEdge> availableEdges = currentStep.Available(context).OfType<IEdge>().ToList();
			while (availableEdges.Count() > 0)
			{
				IEdge nextEdge = SelectNext(availableEdges) as IEdge;
				IStep nextStep;
				if (nextEdge.StartStep.Equals(currentStep))
					nextStep = nextEdge.EndStep;
				else
					nextStep = nextEdge.StartStep;
				(context.Path as IPathPath).Add(currentStep, nextStep, nextEdge.Score);
				currentStep = nextStep;
				availableEdges = currentStep.Available(context).OfType<IEdge>().ToList();
			}
			context.Path.Validate(context);
			//Console.WriteLine(string.Format("Path: {0} {1} {2}", context.Path.Score, context.Path.Edges.First().StartStep.StepName, string.Join(",", context.Path.Edges.Select(e => e.EndStep.StepName))));
		}

		#endregion

	}
}
