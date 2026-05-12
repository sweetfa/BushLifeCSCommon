// <copyright file="LineSegmentLocus.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Spatial.TwoD
{
	/// <summary>
	/// A locus containing line segments
	/// </summary>
	[DebuggerDisplay(@"Line Segment Locus \{{Segments.Count}\}[{Segments.First()}]")]
	public class LineSegmentLocus<T> : Locus<T>
	{
		public ICollection<Line2D<T>> Segments { get; set; }

		public LineSegmentLocus()
		{
			Segments = new List<Line2D<T>>();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (!(obj is LineSegmentLocus<T>))
				return false;
			return Equals<LineSegmentLocus<T>>(obj as LineSegmentLocus<T>);
		}

		public bool Equals<T1>(LineSegmentLocus<T> rhs)
		{
			if (rhs == null)
				return false;
			if (Segments == null && rhs.Segments == null)
				return true;
			if (Segments == null || rhs.Segments == null)
				return false;
			if (Segments.Count != rhs.Segments.Count)
				return false;
			foreach (Line2D<T> line in Segments)
			{
				if (!rhs.Segments.Contains(line))
					return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(Segments);
		}


	}
}
