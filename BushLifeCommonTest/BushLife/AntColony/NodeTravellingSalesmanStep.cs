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
 * @(#) NodeTravellingSalesmanStep.cs
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	/// Test class for a AntColony test using the travelling salesman
	/// algorithm
	/// </summary>
	[DebuggerDisplay("Step {StepName}:[{Edges.Count}]")]
	public class NodeTravellingSalesmanStep : IStep
	{
		public NodeTravellingSalesmanStep(string name)
		{
			StepName = name;
		}


		#region IStep<int> Members

		public string StepName { get; set; }

		[Obsolete("",true)]
		public ICollection<IEdge> Edges { get; set; }

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is NodeTravellingSalesmanStep))
				return false;
			NodeTravellingSalesmanStep rhs = obj as NodeTravellingSalesmanStep;

			bool result = StepName.Equals(rhs.StepName);
//				&& Edges.Count == rhs.Edges.Count;
//				&& Edges.Equals(rhs.Edges);
			return result;
		}

		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(StepName);//, Edges.Count);
		}



		#region IPheremoneKey Members

		public IEnumerable<IPheremoneKey> Available(IAntColonyContext context)
		{
			foreach (IPheremoneKey key in context.Steps.Where(s => !((context.Path as INodePath).Steps.OfType<IStep>().Any(k => k.Equals(s)))).OfType<IPheremoneKey>())
				yield return key;
		}

		public decimal Score
		{
			get
			{
				return 0;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion
	}
}
