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
 * @(#) PathTravellingSalesmanStep.cs
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
	public class PathTravellingSalesmanStep : IStep
	{
		public PathTravellingSalesmanStep(string name)
		{
			StepName = name;
		}


		#region IStep<int> Members

		public string StepName { get; set; }
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
			if (obj == null || !(obj is PathTravellingSalesmanStep))
				return false;
			PathTravellingSalesmanStep rhs = obj as PathTravellingSalesmanStep;

			bool result = StepName.Equals(rhs.StepName)
				&& Edges.Count == rhs.Edges.Count;
//				&& Edges.Equals(rhs.Edges);
			return result;
		}

		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(StepName, Edges.Count);
		}



		#region IPheremoneKey Members

		public IEnumerable<IPheremoneKey> Available(IAntColonyContext context)
		{
			foreach (IPheremoneKey key in Edges.Where(e => !context.Path.Contains(e.StartStep.StepName.Equals(StepName) ? e.EndStep : e.StartStep)).OfType<IPheremoneKey>())
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
