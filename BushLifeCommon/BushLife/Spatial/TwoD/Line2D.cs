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
 * @(#) Line2D.cs
 */

using System;
using System.Collections.Generic;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Spatial.TwoD
{
	/// <summary>
	/// A straight line between two points in two dimensional space
	/// </summary>
	/// <typeparam name="T">The type of the storage for each of the offset values</typeparam>
	public class Line2D<T> : IComparable<Line2D<T>>, ICloneable
	{
		/// <summary>
		/// Basic holder for line intersection formulae
		/// </summary>
		private class Formula
		{
			public double Udenominator { get; set; }
			public double UA { get; set; }
			public double UB { get; set; }
			public Formula(double ua, double ub, double denominator)
			{
				Udenominator = denominator;
				UA = ua;
				UB = ub;
			}
		}

		/// <summary>
		/// The ends of the line
		/// </summary>
        public List<Point2D<T>> Ends { get; set; }

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="newLine">The line to replace this line with</param>
        public Line2D(Line2D<T> newLine)
        {
            this.Ends = newLine.Ends.CloneDeepCollection();
        }

		public Line2D(Point2D<T> point1, Point2D<T> point2)
		{
			Ends = new List<Point2D<T>>(2);
			Ends.Add(point1);
			Ends.Add(point2);
		}

		/// <summary>
		/// Simple constructor taking each of the points at either end of the line
		/// </summary>
		/// <param name="x1">The x axis co-ordinate at the start of the line</param>
		/// <param name="y1">The y axis co-ordinate at the start of the line</param>
		/// <param name="x2">The x axis co-ordinate at the end of the line</param>
		/// <param name="y2">The y axis co-ordinate at the end of the line</param>
        public Line2D(T x1, T y1, T x2, T y2)
        {
            Ends = new List<Point2D<T>>(2);
            Ends.Add(new Point2D<T>(x1, y1));
            Ends.Add(new Point2D<T>(x2, y2));
        }

		/// <summary>
		/// Shift each end of the line by the specified amount.  A negative amount will shift in the negative direction of the particular axis
		/// <para>This method is NOT immutable</para>
		/// </summary>
		/// <param name="xAdjust">The amount to shift along the x axis</param>
		/// <param name="yAdjust">The amount to shift along the y axis</param>
		/// <returns>This line to allow chaining</returns>
        public Line2D<T> Shift(T xAdjust, T yAdjust)
        {
            foreach (Point2D<T> point in Ends)
            {
                point.Shift(xAdjust, yAdjust);
            }
			return this;
        }

		/// <summary>
		/// Invert the line through the X axis
		/// <para>This method is immutable</para>
		/// </summary>
		public Line2D<T> InvertX()
		{
			Point2D<T> start = new Point2D<T>( -((dynamic)Ends[0].X), Ends[0].Y);
			Point2D<T> end = new Point2D<T>( -((dynamic)Ends[1].X), Ends[1].Y);
			return new Line2D<T>(start, end);
		}

		/// <summary>
		/// Invert the line through the Y axis
		/// <para>This method is immutable</para>
		/// </summary>
		public Line2D<T> InvertY()
		{
			Point2D<T> start = new Point2D<T>(Ends[0].X, -((dynamic)Ends[0].Y));
			Point2D<T> end = new Point2D<T>(Ends[1].X, -((dynamic)Ends[1].Y));
			return new Line2D<T>(start, end);
		}

		/// <summary>
		/// Return the length of the line
		/// </summary>
		/// <returns>The length of the line specified in the units that the points are designated in</returns>
        public T Length
        {
			get
			{
				return Ends[0].DistanceTo(Ends[1]);
			}
        }

		/// <summary>
		/// Determine the point at the middle of the line
		/// </summary>
		/// <returns></returns>
		public Point2D<T> MidPoint()
		{
			T x = ((dynamic) Ends[0].X - Ends[1].X) / 2 + Ends[1].X;
			T y = ((dynamic) Ends[0].Y - Ends[1].Y) / 2 + Ends[1].Y;
			return new Point2D<T>(x, y);
		}

		#region Line2D Geometry
		/// <summary>
		/// Return the intersection point of two lines
		/// </summary>
		/// <param name="rhs">the other line</param>
		/// <returns>The locus describing the intersection type 
		/// and any intersecting points or segments</returns>
		public Locus<T> Intersection(Line2D<T> rhs)
		{
			Point2D<T> p1 = Ends[0];
			Point2D<T> p2 = Ends[1];

			Formula f = CalculateIntersectionFormulae(rhs);
			double uA = f.UA / f.Udenominator;
			double uB = f.UB / f.Udenominator;
			if (0 <= uA && uA <= 1 && 0 <= uB && uB <= 1)
			{
				try
				{
					T x = (T)(dynamic)(p1.X + (uA * ((dynamic)p2.X - p1.X)));
					T y = (T)(dynamic)(p1.Y + (uA * ((dynamic)p2.Y - p1.Y)));
					return new PointLocus<T>(new Point2D<T>(x, y));
				}
				catch (DivideByZeroException)
				{
					// Parallel lines do not intersect unless they are coincident
					return Overlaps(rhs);
				}
			}
			else
			{
				// Lines do not intersect due to shortness (endpoints) of lines
				// or they may be on the same plane in which case check for overlap
				return Overlaps(rhs);
			}
		}

		/// <summary>
		/// Determine if a line overlaps another line
		/// and fetch the line segment of the overlap
		/// <para>The two lines must be coincident for this function to work</para>
		/// </summary>
		/// <param name="rhs">The line to check against</param>
		/// <returns>A locus containing the overlapping segments</returns>
		public Locus<T> Overlaps(Line2D<T> rhs)
		{
			if (!Coincident(rhs))
				return new EmptyLocus<T>();

			Point2D<T> p1 = null;
			Point2D<T> p2 = null;

			double r1 = CalculatePerpindicularOffset(rhs.Ends[0]);
			if (0 <= r1 && r1 <= 1)
				p1 = CalculatePerpindicularPoint(rhs.Ends[0]);
			else
			{
				r1 = rhs.CalculatePerpindicularOffset(Ends[0]);
				if (0 <= r1 && r1 <= 1)
					p1 = rhs.CalculatePerpindicularPoint(Ends[0]);
				else
				{
					r1 = rhs.CalculatePerpindicularOffset(Ends[1]);
					if (0 <= r1 && r1 <= 1)
						p1 = rhs.CalculatePerpindicularPoint(Ends[1]);
					else
						return new EmptyLocus<T>();
				}
			}


			double r2 = CalculatePerpindicularOffset(rhs.Ends[1]);
			if (0 <= r2 && r2 <= 1)
				p2 = CalculatePerpindicularPoint(rhs.Ends[1]);
			else
			{
				r2 = rhs.CalculatePerpindicularOffset(Ends[1]);
				if (0 <= r2 && r2 <= 1)
					p2 = rhs.CalculatePerpindicularPoint(Ends[1]);
				else
				{
					r2 = rhs.CalculatePerpindicularOffset(Ends[0]);
					if (0 <= r2 && r2 <= 1)
						p2 = rhs.CalculatePerpindicularPoint(Ends[0]);
					else
						return new EmptyLocus<T>();
				}
			}

			if (p1 == null || p2 == null)
				return new EmptyLocus<T>();

			if (p1.Equals(p2))
				return new PointLocus<T>(p1);

			LineSegmentLocus<T> result = new LineSegmentLocus<T>();
			result.Segments.Add(new Line2D<T>(p1, p2));
			return result;
		}

		/// <summary>
		/// Determine if two lines are parallel
		/// </summary>
		/// <param name="rhs">the other line</param>
		/// <returns>true if the lines are parallel, false otherwise</returns>
		public bool Parallel(Line2D<T> rhs)
		{
			Formula f = CalculateIntersectionFormulae(rhs);
			return f.Udenominator == 0;
		}

		/// <summary>
		/// Determine if a point lands on a line
		/// </summary>
		/// <param name="point">The point to check</param>
		/// <returns>true if the point lands on a line, false otherwise</returns>
		public bool Contains(Point2D<T> point)
		{
			T distance = this.ShortestDistanceTo(point);
			return (dynamic)distance == 0;
		}

		/// <summary>
		/// Determine if two lines intersect anywhere
		/// </summary>
		/// <param name="rhs">The other line</param>
		/// <returns>true if the lines exist, otherwise false</returns>
		public bool Intersects(Line2D<T> rhs)
		{
			Locus<T> result1 = Intersection(rhs);
			Locus<T> result2 = Overlaps(rhs);
			return !(result1 is EmptyLocus<T> && result2 is EmptyLocus<T>);
		}

		/// <summary>
		/// Determine if two lines are coincident (they lie one on top of the other)
		/// <para>This function does not consider endpoints of lines, so non-overlapping
		/// lines will be considered co-incident if they have the same linear function</para>
		/// </summary>
		/// <param name="rhs">The other line</param>
		/// <returns>true if the lines are in the same space</returns>
		public bool Coincident(Line2D<T> rhs)
		{
			Formula f = CalculateIntersectionFormulae(rhs);
			return (f.Udenominator == 0 && f.UA == 0 && f.UB == 0);
		}

		/// <summary>
		/// <para>Calculate the equations used for intersection calculation</para>
		/// <para>Formula extracted from http://paulbourke.net/geometry/lineline2d
		/// </para>
		/// </summary>
		/// <param name="rhs">The line to calculate with</param>
		/// <returns>The formula object contain the formula to use</returns>
		private Formula CalculateIntersectionFormulae(Line2D<T> rhs)
		{
			Point2D<T> p1 = Ends[0];
			Point2D<T> p2 = Ends[1];
			Point2D<T> p3 = rhs.Ends[0];
			Point2D<T> p4 = rhs.Ends[1];

			double UDenominator = ((((dynamic)p4.Y - p3.Y) * ((dynamic)p2.X - p1.X)) - (((dynamic)p4.X - p3.X) * ((dynamic)p2.Y - p1.Y)));
			double UaNumerator = ((((dynamic)p4.X - p3.X) * ((dynamic)p1.Y - p3.Y)) - (((dynamic)p4.Y - p3.Y) * ((dynamic)p1.X - p3.X)));
			double UbNumerator = ((((dynamic)p2.X - p1.X) * ((dynamic)p1.Y - p3.Y)) - (((dynamic)p2.Y - p1.Y) * ((dynamic)p1.X - p3.X)));
			return new Formula(UaNumerator, UbNumerator, UDenominator);
		}
		#endregion

		/// <summary>
		/// Find the shortest distance from the specified point to 
		/// this line
		/// </summary>
		/// <param name="point">The point to find the shortest distance from to this line</param>
		/// <returns>The actual distance to the closest point of the line to this point</returns>
		public T ShortestDistanceTo(Point2D<T> point)
		{
			Point2D<T> P = CalculatePerpindicularPoint(point);
			return P.DistanceTo(point);
		}

		/// <summary>
		/// Determine if the supplied line is perpendicular to this line
		/// </summary>
		/// <param name="rhs">The supplied line</param>
		/// <returns>true if perpendicular, false otherwise</returns>
		public bool IsPerpendicular(Line2D<T> rhs)
		{
			double angle = AngleTo(rhs);
			return angle == 90 || angle == 270;
		}

		/// <summary>
		/// Calculate the angle in degrees of this line
		/// </summary>
		/// <returns>The angle in degrees of this line where 0 degrees is vertically up or Y axis</returns>
		public double Angle()
		{
			double angle1 = Math.Atan2((dynamic)Ends[1].Y - Ends[0].Y, (dynamic)Ends[1].X - Ends[0].X);
			double angle1d = (angle1.ToDegrees() + 360) % 360;
			return angle1d;
		}

		/// <summary>
		/// Calculate the angle in degrees betwee two lines
		/// </summary>
		/// <param name="rhs">The other line</param>
		/// <returns>The angle in degrees</returns>
		public double AngleTo(Line2D<T> rhs)
		{
			double angle1d = Angle();
			double angle2d = rhs.Angle();
			return ((angle1d - angle2d) + 360) % 360;
		}

		/// <summary>
		/// Calculate the perpindicular point on this line to the relative point
		/// <para>The point may not be within the bounds of this line segment but may be
		/// the point on the extension either side of the actual line</para>
		/// </summary>
		/// <param name="point">The point to find the perpendicular point for</param>
		/// <returns>The perpindicular point</returns>
		public Point2D<T> CalculatePerpindicularPoint(Point2D<T> point)
		{
			Point2D<T> A = this.Ends[0];
			Point2D<T> B = this.Ends[1];
			Point2D<T> C = point;

			double r = CalculatePerpindicularOffset(point);

			Point2D<T> P = new Point2D<T>(
				(T) (A.X + (r * ((dynamic)B.X - A.X))),
				(T) (A.Y + (r * ((dynamic)B.Y - A.Y))));
			return P;
		}

		/// <summary>
		/// Calculate the distance along this line of the point
		/// for the perpindicular point to the provided point
		/// <para>The offset may not be within the bounds of this line segment but may be
		/// the point on the extension either side of the actual line</para>
		/// </summary>
		/// <param name="point">The point to find the perpindicular offset along</param>
		/// <returns>The perpindicular offset
		/// <para>    r=0      P = A</para>
		/// <para>    r=1      P = B</para>
		/// <para>    r&lt;0      P is on the backward extension of AB</para>
		/// <para>    r>1      P is on the forward extension of AB</para>
		/// <para>    0&lt;r&lt;1    P is interior to AB</para>
		/// </returns>
		public double CalculatePerpindicularOffset(Point2D<T> point)
		{
			//Let the point be C (Cx,Cy) and the line be AB (Ax,Ay) to (Bx,By).
			//Let P be the point of perpendicular projection of C on AB.  The parameter
			//r, which indicates P's position along AB, is computed by the dot product 
			//of AC and AB divided by the square of the length of AB:

			//(1)     AC dot AB
			//    r = ---------  
			//        ||AB||^2

			//r has the following meaning:

			//    r=0      P = A
			//    r=1      P = B
			//    r<0      P is on the backward extension of AB
			//    r>1      P is on the forward extension of AB
			//    0<r<1    P is interior to AB

			//The length of a line segment in d dimensions, AB is computed by:
			//    L = sqrt( (Bx-Ax)^2 + (By-Ay)^2 + ... + (Bd-Ad)^2)
			//so in 2D:   
			//    L = sqrt( (Bx-Ax)^2 + (By-Ay)^2 )

			//and the dot product of two vectors in d dimensions, U dot V is computed:
			//    D = (Ux * Vx) + (Uy * Vy) + ... + (Ud * Vd)
			//so in 2D:   
			//    D = (Ux * Vx) + (Uy * Vy) 

			//So (1) expands to:
			//        (Cx-Ax)(Bx-Ax) + (Cy-Ay)(By-Ay)
			//    r = -------------------------------
			//                      L^2

			//The point P can then be found:

			//    Px = Ax + r(Bx-Ax)
			//    Py = Ay + r(By-Ay)

			//And the distance from A to P = r*L.

			Point2D<T> A = this.Ends[0];
			Point2D<T> B = this.Ends[1];
			Point2D<T> C = point;

			double r = ((((dynamic)C.X - A.X) * ((dynamic)B.X - A.X))
				+ (((dynamic)C.Y - A.Y) * ((dynamic)B.Y - A.Y)))
				/ Math.Pow((dynamic) this.Length, 2);
			return r;
		}

		#region Rotation
		/// <summary>
		/// Rotate a line segment around a pivot point
		/// <para>This method is immutable</para>
		/// </summary>
		/// <param name="pivotPoint">The point to pivot the line around</param>
		/// <param name="degreeOfRotation">The degrees of rotation</param>
		/// <returns>A new line segment at the new position rotated relative to the pivot point</returns>
		public Line2D<T> Rotate(Point2D<T> pivotPoint, double degreeOfRotation)
		{
			return new Line2D<T>(
				Ends[0].Rotate(pivotPoint, degreeOfRotation),
				Ends[1].Rotate(pivotPoint, degreeOfRotation));
		}

		#endregion

		/// <summary>
		/// Output a string representation of the line
		/// </summary>
		/// <returns>A string representation of the line</returns>
		public override string ToString()
        {
            string result = "Line2D [" + Ends[0].ToString() + "-" + Ends[1].ToString() + "]";
			result += " {" + Ends[0].DistanceTo(Ends[1]) + "} ";
			result += "[(" + Math.Abs((dynamic)Ends[0].X - (dynamic)Ends[1].X) + "),";
			result += "(" + Math.Abs((dynamic)Ends[0].Y - (dynamic)Ends[1].Y) + ")]";
			return result;
        }

		#region Equality Methods

		/// <summary>
		/// <para>Compare this object instance for value equality</para>
		/// <para>Compare this object instance to ensure that all fields 
		/// of the object are equal and the object is of the same type</para>
		/// </summary>
		/// <param name="obj">The object to compare against</param>
		/// <returns>true if this object equals the other object</returns>
		public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Line2D<T>))
            {
                return false;
            }
            return this.Equals((Line2D<T>)obj);
        }

		/// <summary>
		/// <para>Compare this class field values to another instance</para>
		/// <para>Compare each of the fields of this instance with the supplied instance to ensure that each field contains the same value</para>
		/// <para>Reversed lines are considered equivalent</para>
		/// </summary>
		/// <param name="rhs">The object to compare against</param>
		/// <returns>true if this object equals the other object</returns>
		public bool Equals(Line2D<T> rhs)
        {
            if (Ends.Count != rhs.Ends.Count)
            {
                return false;
            }
			return ((Ends[0].Equals(rhs.Ends[0]) && Ends[1].Equals(rhs.Ends[1])) 
				|| (Ends[1].Equals(rhs.Ends[0]) && Ends[0].Equals(rhs.Ends[1]))); 
        }

		/// <summary>
		/// Generate the hash code for this object
		/// </summary>
		/// <returns>The hash code for the object</returns>
        public override int GetHashCode()
        {
			return LanguageUtils.RSHash(Ends);
		}
		#endregion

		#region IComparable<Line2D<T>> Members

		/// <summary>
		/// Compare this line to another line
		/// </summary>
		/// <param name="other">The line to compare with</param>
		/// <returns>The integer difference between the X-axis if not the same, otherwise the integer difference between the Y-axis values</returns>
		public int CompareTo(Line2D<T> other)
		{
			int result;
			if ((result = Ends[0].CompareTo(other.Ends[0])) != 0)
			{
				result = Ends[1].CompareTo(other.Ends[1]);
			}
			return result;
		}

		#endregion

		#region ICloneable Members

		public T1 Clone<T1, T2>()
		{
			Line2D<T2> result = new Line2D<T2>((dynamic) Ends[0].Clone(), (dynamic) Ends[1].Clone());
			return (dynamic) result;
		}

		public object Clone()
		{
			return this.Clone<Line2D<Int32>,Int32>();
		}

		#endregion

		/// <summary>
		/// Extend a line the specified distance in either/both directions
		/// <para>This method is immutable</para>
		/// </summary>
		/// <param name="bottomLeftExtension">The amount to extend the logical bottom left length of the line</param>
		/// <param name="topRightExtension">The amount to extend the logical top right length of the line</param>
		/// <returns>A new line with the extensions added as required</returns>
		public Line2D<T> Extend(T bottomLeftExtension, T topRightExtension)
		{
			Point2D<T> p1 = Ends[0];
			Point2D<T> p2 = Ends[1];
			T adj = (dynamic) p2.X - p1.X;
			T opp = (dynamic) p2.Y - p1.Y;
			double divisor = (double)(dynamic)opp / (double)(dynamic)adj;
			double atan = Math.Atan(divisor);
			double angle = MathsUtils.ToDegrees(atan);
			if ((dynamic)opp == 0)
				if ((dynamic)p1.X > p2.X)
					angle = 180;
			if ((dynamic)adj == 0)
				if ((dynamic)p1.Y > p2.Y)
					angle = 270;
			if ((dynamic) topRightExtension != 0)
			{
				p2 = p2.LocatePointAtDistance(angle, topRightExtension);
			}
			if ((dynamic) bottomLeftExtension != 0)
			{
				p1 = p1.LocatePointAtDistance(angle + 180, bottomLeftExtension);
			}
			return new Line2D<T>(p1, p2);
		}

		/// <summary>
		/// Return the end of this line that is closest to the point provided
		/// </summary>
		/// <param name="rhs">The point to find the closest end to</param>
		/// <returns>The point for the end of the line that is closest to the point</returns>
		public Point2D<T> ClosestEnd(Point2D<T> rhs)
		{
			T distance1 = Ends[0].DistanceTo(rhs);
			T distance2 = Ends[1].DistanceTo(rhs);
			if ((dynamic) distance1 < distance2)
				return Ends[0];
			return Ends[1];
		}

		/// <summary>
		/// Return the end of this line that is closest to the
		/// other line
		/// </summary>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public Point2D<T> ClosestEnd(Line2D<T> rhs)
		{
			T distance1 = Ends[0].DistanceTo(rhs.Ends[0]);
			T distance2 = Ends[1].DistanceTo(rhs.Ends[0]);
			T distance3 = Ends[0].DistanceTo(rhs.Ends[1]);
			T distance4 = Ends[1].DistanceTo(rhs.Ends[1]);
			if (((dynamic)distance1 < distance2)
				&& ((dynamic)distance1 < distance3)
				&& ((dynamic)distance1 < distance4))
				return Ends[0];
			if (((dynamic)distance2 < distance3)
				&& ((dynamic)distance2 < distance4))
				return Ends[1];
			if (((dynamic)distance3 < distance4))
				return Ends[0];
			return Ends[1];
		}


	}
	
}
