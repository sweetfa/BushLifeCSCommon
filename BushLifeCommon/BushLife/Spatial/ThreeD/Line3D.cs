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
 * @(#) Line3D.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Spatial.ThreeD
{
	/// <summary>
	/// Representation of a line in 3 dimensional space
	/// </summary>
	[Serializable]
	[DebuggerDisplay("Line3D [{Ends[0]}]-[{Ends[1]}] {Length}")]
	public class Line3D<T> : ICloneable
	{
		/// <summary>
		/// Initialising constructor
		/// </summary>
		/// <param name="x1">X co-ordinate for start of line</param>
		/// <param name="y1">Y co-ordinate for start of line</param>
		/// <param name="z1">Z co-ordinate for start of line</param>
		/// <param name="x2">X co-ordinate for end of line</param>
		/// <param name="y2">Y co-ordinate for end of line</param>
		/// <param name="z2">Z co-ordinate for end of line</param>
		public Line3D(T x1, T y1, T z1, T x2, T y2, T z2)
		{
			Ends = new List<Point3D<T>>();
			Ends.Add(new Point3D<T>(x1, y1, z1));
			Ends.Add(new Point3D<T>(x2, y2, z2));
		}

		/// <summary>
		/// Initialising constructor
		/// </summary>
		/// <param name="start">The point at the start of the line</param>
		/// <param name="end">The point at the end of the line</param>
		public Line3D(Point3D<T> start, Point3D<T> end)
		{
			Ends = new List<Point3D<T>>();
			Ends.Add(start);
			Ends.Add(end);
		}

		/// <summary>
		/// The line segments of a line (most lines only have two ends)
		/// Can be used as line segments by adding additional end points
		/// </summary>
		public IList<Point3D<T>> Ends { get; set; }

		/// <summary>
		/// The length of the line from point to point
		/// </summary>
		public T Length { get { return Ends[0].DistanceTo(Ends[1]); } }

		/// <summary>
		/// Determine if this line intersects with the other line
		/// </summary>
		/// <param name="rhs">The other line</param>
		/// <param name="ip">The resulting intersection point if any</param>
		/// <returns>true if the lines intersect, false otherwise</returns>
		[Obsolete("Function does not work correctly")]
		public bool Intersects(Line3D<T> rhs, out Point3D<T> ip)
		{
			Point3D<double> lhsEnd1 = new Point3D<double>((dynamic)Ends[0].X, (dynamic)Ends[0].Y, (dynamic)Ends[0].Z);
			Point3D<double> lhsEnd2 = new Point3D<double>((dynamic)Ends[1].X, (dynamic)Ends[1].Y, (dynamic)Ends[1].Z);
			Point3D<double> rhsEnd1 = new Point3D<double>((dynamic)rhs.Ends[0].X, (dynamic)rhs.Ends[0].Y, (dynamic)rhs.Ends[0].Z);
			Point3D<double> rhsEnd2 = new Point3D<double>((dynamic)rhs.Ends[1].X, (dynamic)rhs.Ends[1].Y, (dynamic)rhs.Ends[1].Z);

			Point3D<double> da = lhsEnd2 - lhsEnd1;
			Point3D<double> db = rhsEnd2 - rhsEnd1;
			Point3D<double> dc = rhsEnd1 - lhsEnd1;

			Point3D<double> c1 = Point3D<double>.CrossProduct(dc, db);
			Point3D<double> c2 = Point3D<double>.CrossProduct(da, db);

			double dp2 = Point3D<double>.DotProduct(dc, c2);

			if (dp2 != 0.0) // lines are not coplanar
			{
				ip = null;
				return false;
			}

			double dp1 = Point3D<double>.DotProduct(c1, c2);
			double nm1 = Point3D<double>.Normalise2(c2);
			double s =  dp1 / nm1;
			if ((0.0 <= s && s <= 1.0) || (nm1 == 0 && dp1 == 0))
			{
				Point3D<double> intersectingPoint = lhsEnd1 + (dynamic) da * new Point3D<double>(s, s, s);
				ip = new Point3D<T>((T)(dynamic)intersectingPoint.X, (T)(dynamic)intersectingPoint.Y, (T)(dynamic)intersectingPoint.Z);
				return true;
			}
			ip = null;
			return false;
		}
		
		/// <summary>
		/// Shift each point in the line by the same adjustment amount
		/// for each line point
		/// <para>This function is mutable</para>
		/// </summary>
		/// <param name="xAdjust">The amount to adjust the X axis.  Negative values shift the other way</param>
		/// <param name="yAdjust">The amount to adjust the Y axis.  Negative values shift the other way</param>
		/// <param name="zAdjust">The amount to adjust the Z axis.  Negative values shift the other way</param>
		public void Shift(T xAdjust, T yAdjust, T zAdjust)
		{
			for (int i = 0; i < Ends.Count; ++i)
				Ends[i] = Ends[i].Shift(xAdjust, yAdjust, zAdjust);
		}

		/// <summary>
		/// Calculate the midpoint of the line
		/// </summary>
		/// <returns>The point at the midpoint of the line</returns>
		public Point3D<T> MidPoint()
		{
			T rx = (((dynamic)Ends[1].X - Ends[0].X) / 2) + Ends[0].X;
			T ry = (((dynamic)Ends[1].Y - Ends[0].Y) / 2) + Ends[0].Y;
			T rz = (((dynamic)Ends[1].Z - Ends[0].Z) / 2) + Ends[0].Z;
			Point3D<T> result = new Point3D<T>(rx, ry, rz);
			return result;
		}

		#region ICloneable Members

		public object Clone()
		{
			return Clone<Line3D<T>>();
		}

		public Line3D<T> Clone<T1>()
		{
			return new Line3D<T>((Point3D<T>)Ends[0].Clone(), (Point3D<T>)Ends[1].Clone());
		}
		#endregion

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (!(obj is Line3D<T>))
				return false;
			return Equals<Line3D<T>>(obj as Line3D<T>);
		}

		public bool Equals<T1>(T1 rhs)
			where T1 : Line3D<T>
		{
			if (rhs == null) return false;
			return Ends[0].Equals(rhs.Ends[0])
				&& Ends[1].Equals(rhs.Ends[1]);
		}

		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(Ends[0], Ends[1]);
		}
	}
}
