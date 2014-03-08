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
 * @(#) NodeAnt.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Aspects;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	///
	/// </summary>
	public class NodeAnt : Ant<IStep>, IAnt<IStep>
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
				currentStep = availableSteps.ElementAt(randomIndex);

				// Process each step until no steps remain
				availableSteps = currentStep.Available(context).OfType<IStep>().ToList();
				while (availableSteps.Count() > 0)
				{
					IStep nextStep = SelectNext(availableSteps);
					(context.Path as INodePath).Add(nextStep, nextStep.Score);
					currentStep = nextStep;
					availableSteps = currentStep.Available(context).OfType<IStep>().ToList();
				}
				context.Path.Validate(context);
				//Console.WriteLine("Path: {0}: {1}", context.Path.Score, 
				//    (((INodePath) context.Path).Steps != null
				//        ? string.Join(",", ((INodePath) context.Path).Steps.Select(e => e.StepName))
				//        : ""));
			}
		}

		#endregion

	}
}
