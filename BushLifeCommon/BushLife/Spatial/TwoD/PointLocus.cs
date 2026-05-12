// <copyright file="PointLocus.cs" company="Bush Life Pty Limited">
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
	/// A locus representing an intersection at a single point in space
	/// </summary>
	[DebuggerDisplay("Point Locus [{IntersectionPoint}]")]
	public class PointLocus<T> : Locus<T>
	{
		/// <summary>
		/// Constructor taking the intersection point
		/// </summary>
		/// <param name="point">The intersection point</param>
		public PointLocus(Point2D<T> point)
		{
			IntersectionPoint = point;
		}

		/// <summary>
		/// The point at which the two line segments intersect
		/// </summary>
		public Point2D<T> IntersectionPoint { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (!(obj is PointLocus<T>))
				return false;
			return IntersectionPoint.Equals((obj as PointLocus<T>).IntersectionPoint);
		}

		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(IntersectionPoint);
		}

	}
}
