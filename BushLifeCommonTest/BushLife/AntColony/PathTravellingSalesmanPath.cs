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
 * @(#) PathTravellingSalesmanPath.cs
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
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class PathTravellingSalesmanPath : IPathPath
	{
		private bool InvalidPath = false;

		#region IPath<int> Members

		public decimal Score { get { return InvalidPath ? decimal.MaxValue : Edges == null ? 0 : Edges.Sum(e => e.Score); } }

		public ICollection<IEdge> Edges { get; set; }

		public bool Contains(IStep step)
		{
			return Edges.Where(e => e.StartStep.Equals(step) || e.EndStep.Equals(step)).Count() > 0;
		}

		public void Add(IStep step, decimal score)
		{
			Add(Edges.Last().EndStep, step, score);
		}

		public void Add(IStep startPoint, IStep endPoint, decimal score)
		{
			IEdge edge = new PathTravellingSalesmanEdge(startPoint, endPoint, score);
			Edges.Add(edge);
		}

		public void Reset()
		{
			Edges = new List<IEdge>();
			InvalidPath = false;
		}

		public void Validate(object contextObject)
		{
			AntColonyContext context = contextObject as AntColonyContext;
			if (Edges.Count != (context.Steps.Count - 1))
				InvalidPath = true;
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			return MemberwiseClone();
		}

		#endregion

		#region Debugger Display
		internal string DebuggerDisplay
		{
			get
			{
				if (Edges == null || Edges.Count() == 0)
					return this.GetType().Name;
				return string.Format("{0} [{3}] {1},{2}", this.GetType().Name, Edges.First().StartStep.StepName, string.Join(",", Edges.Select(e => e.EndStep.StepName)), Score);
			}
		}
		#endregion


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is PathTravellingSalesmanPath))
				return false;
			PathTravellingSalesmanPath rhs = obj as PathTravellingSalesmanPath;
			return Score == rhs.Score &&
				Edges.SequenceEqual(rhs.Edges);
		}

		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(Score);
		}
	}
}
