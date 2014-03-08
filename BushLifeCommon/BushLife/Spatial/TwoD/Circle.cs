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
 * @(#) Circle.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Spatial.TwoD
{
	/// <summary>
	/// A two dimensional circle
	/// </summary>
	public class Circle<TPoint,TRadius>
	{
		public TRadius Radius { get; set; }
		public Point2D<TPoint> Centre { get; set; }

		public Circle(Point2D<TPoint> centre, TRadius radius)
		{
			Radius = radius;
			Centre = centre;
		}

		/// <summary>
		/// Does another circle intersect this circle at any point
		/// <para>This also returns true for a circle completly encompassed by another circle</para>
		/// </summary>
		/// <param name="rhs">The other circle</param>
		/// <returns>true if any part of the circle intercepts or is enclosed by the other circle</returns>
		public bool Intersects(Circle<TPoint, TRadius> rhs)
		{
			Line2D<TPoint> vector = new Line2D<TPoint>(Centre, rhs.Centre);
			TRadius sumRadius = (dynamic) Radius + rhs.Radius;
			return (dynamic) vector.Length < sumRadius;
		}

		/// <summary>
		/// Determine if a line intersects the circle at any point
		/// <para>This does not intersect if the line is completely enclosed in the circle</para>
		/// </summary>
		/// <param name="lineSegment">The line segment to check for intersection</param>
		/// <returns>False if no intersection occurs</returns>
		public bool Intersects(Line2D<TPoint> lineSegment)
		{
			return !(Intersection(lineSegment) is EmptyLocus<TPoint>);
		}

		/// <summary>
		/// Retrieve the intersection points for a line with this circle
		/// </summary>
		/// <param name="lineSegment">The line segment to check for intersection</param>
		/// <returns>A locus indicating the points of intersection, if any</returns>
		public Locus<TPoint> Intersection(Line2D<TPoint> lineSegment)
		{
			Point2D<TPoint> d = lineSegment.Ends[1] - lineSegment.Ends[0];
			Point2D<TPoint> f = lineSegment.Ends[0] - Centre;

			decimal a = d.DotProduct(d);
			decimal b = 2 * f.DotProduct(d);
			decimal c = f.DotProduct(f) - (dynamic)Radius * (dynamic)Radius;

			decimal discriminant = b * b - 4 * a * c;

			if (discriminant < 0)
				return new EmptyLocus<TPoint>();

			// ray didn't totally miss sphere,
			// so there is a solution to
			// the equation.

			discriminant = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(discriminant)));

			// either solution may be on or off the ray so need to test both
			// t1 is always the smaller value, because BOTH discriminant and
			// a are nonnegative.
			decimal t1 = (-b - discriminant) / (2 * a);
			decimal t2 = (-b + discriminant) / (2 * a);

			// 3x HIT cases:
			//          -o->             --|-->  |            |  --|->
			// Impale(t1 hit,t2 hit), Poke(t1 hit,t2>1), ExitWound(t1<0, t2 hit), 

			// 3x MISS cases:
			//       ->  o                     o ->              | -> |
			// FallShort (t1>1,t2>1), Past (t1<0,t2<0), CompletelyInside(t1<0, t2>1)

			if (0 <= t1 && t1 <= 1)
			{
				// t1 is an intersection, and if it hits,
				// it's closer than t2 would be
				if (0 <= t2 && t2 <= 1)
					// Impale
					return new MultiPointLocus<TPoint>(Point(t1, d, f), Point(t2, d, f));
				else
					// Poke
					return new PointLocus<TPoint>(Point(t1, d, f));
			}

			// here t1 didn't intersect so we are either started
			// inside the sphere or completely past it
			if (0 <= t2 && t2 <= 1)
			{
				// ExitWound
				return new PointLocus<TPoint>(Point(t2, d,f));
			}

			// no intn: FallShort, Past, CompletelyInside
			return new EmptyLocus<TPoint>();

		}

		/// <summary>
		/// Create a point from the ratio
		/// </summary>
		/// <param name="t1">The ratio for the intersection point on the line segment</param>
		/// <param name="p1">The point of the line</param>
		/// <param name="p2">The second point of the line</param>
		/// <returns>The interception point</returns>
		private Point2D<TPoint> Point(decimal t1, Point2D<TPoint> p1, Point2D<TPoint> p2)
		{
			if (t1 < 0 || 1 < t1)
				throw new ArgumentOutOfRangeException("t1", "The t1 ratio is out of range for an intersection and therefore cannot be used to determine an intersection point");
			TPoint x = (TPoint)(dynamic)(p1.X + (t1 * ((dynamic)p2.X - p1.X)));
			TPoint y = (TPoint)(dynamic)(p1.Y + (t1 * ((dynamic)p2.Y - p1.Y)));
			return new Point2D<TPoint>(x, y);
		}

		public override bool Equals(object obj)
		{
			Circle<TPoint, TRadius> rhs = obj as Circle<TPoint, TRadius>;
			if (rhs == null)
				return false;
			return Centre.Equals(rhs.Centre)
				&& (dynamic) Radius == rhs.Radius;
		}

		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(Centre, Radius);
		}
	}
}
