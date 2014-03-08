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
 * @(#) PathTravellingSalesmanEdge.cs
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
	/// Test class for ant colony implementing an edge for the travelling salesman problem
	/// </summary>
	[DebuggerDisplay("Edge {StartStep.StepName} --{Score}--> {EndStep.StepName}")]
	public class PathTravellingSalesmanEdge : IEdge
	{
		public PathTravellingSalesmanEdge(IStep start, IStep end, decimal score)
		{
			StartStep = start;
			EndStep = end;
			Score = score;
		}

		#region IEdge Members

		public IStep StartStep { get; set; }

		public IStep EndStep { get; set; }

		public decimal Score { get; set; }

		#endregion

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is PathTravellingSalesmanEdge))
				return false;
			PathTravellingSalesmanEdge rhs = obj as PathTravellingSalesmanEdge;

			return StartStep.Equals(rhs.StartStep)
				&& EndStep.Equals(rhs.EndStep)
				&& Score.Equals(rhs.Score);
		}

		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(StartStep, EndStep, Score);
		}


		#region IComparable Members

		public int CompareTo(object obj)
		{
			if (obj == null)
				return -1;
			if (!(obj is IEdge))
				return 1;
			return this.CompareTo(obj as IEdge);
		}

		#endregion

		//#region IComparable<IEdge<int>> Members

		//public int CompareTo(IEdge<int> other)
		//{
		//    if (other == null)
		//        return -1;
		//    if (Score.CompareTo(other.Score) != 0)
		//        return Score.CompareTo(other.Score);
		//    if (StartStep.CompareTo(other.StartStep) != 0)
		//        return Score.CompareTo(other.StartStep);
		//    return Score.CompareTo(other.EndStep);
		//}

		//#endregion

		#region IPheremoneKey Members

		public IEnumerable<IPheremoneKey> Available(IAntColonyContext context)
		{
			return EndStep.Available(context);
		}

		#endregion
	}
}
