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
 * @(#) Locus.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Spatial.TwoD
{
	/// <summary>
	/// Abstract class for Locus to determine line intersections
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Locus<T>
	{
		public static Locus<T> Intersects(Line2D<T> line1, Line2D<T> line2)
		{
			return line1.Intersection(line2);
		}

		public override abstract bool Equals(object obj);
		public override abstract int GetHashCode();
	}
}
