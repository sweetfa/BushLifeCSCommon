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
 * @(#) MultiPointLocus.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Spatial.TwoD
{
	/// <summary>
	/// A locus of intersection containing many points of intersection
	/// where a ray intercepts an object at multiple points such
	/// as through a rectangle or circle or other shape
	/// </summary>
	public class MultiPointLocus<T> : Locus<T>
	{
		public IList<Point2D<T>> Points { get; set; }

		public MultiPointLocus(params Point2D<T>[] points)
		{
			Points = new List<Point2D<T>>();
			foreach (Point2D<T> point in points)
				Points.Add(point);
		}

		public override bool Equals(object obj)
		{
			MultiPointLocus<T> rhs = obj as MultiPointLocus<T>;
			if (rhs == null)
				return false;
			return Points.SequenceEqual(rhs.Points);
		}

		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(Points);
		}
	}
}
