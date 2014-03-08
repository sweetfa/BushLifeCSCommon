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
 * @(#) Point2D.cs
 */

using System;
using System.Diagnostics;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Spatial.TwoD
{
	/// <summary>
	/// A single point in two dimensional space
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DebuggerDisplay("[{X},{Y}]")]
	public class Point2D<T> : IComparable<Point2D<T>>, ICloneable
	{
		/// <summary>
		/// The origin point in two dimensional space
		/// </summary>
		public static readonly Point2D<T> Origin = new Point2D<T>((T) (dynamic) 0, (T) (dynamic) 0);

		/// <summary>
		/// The position along the horizontal axis
		/// </summary>
        public T X { get; protected set; }

		/// <summary>
		/// The position along the vertical axis
		/// </summary>
		public T Y { get; protected set; }

		/// <summary>
		/// A simple constructor taking the co-ordinate points
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
        public Point2D(T x, T y)
        {
            X = x;
            Y = y;
        }

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="rhs">The point to copy</param>
		public Point2D(Point2D<T> rhs)
		{
			X = rhs.X;
			Y = rhs.Y;
		}

		#region ICloneable Members

		public Point2D<T> Clone()
		{
			Point2D<T> result = new Point2D<T>(X, Y);
			return result;
		}

		object ICloneable.Clone()
		{
			return Clone();
		}
		#endregion

		/// <summary>
		/// Shift the point by the specified amount.  Negative amounts adjust the point in the negative direction
		/// <para>This method is mutable</para>
		/// </summary>
		/// <param name="xAdjust">The amount to adjust the X axis by</param>
		/// <param name="yAdjust">The amount to adjust the Y axis by</param>
		/// <returns>The new point</returns>
        public Point2D<T> Shift(T xAdjust, T yAdjust)
        {
            dynamic x1 = X;
            dynamic x2 = xAdjust;
            X = x1 + x2;
            dynamic y1 = Y;
            dynamic y2 = yAdjust;
            Y = y1 + y2;
            return this;
        }

		/// <summary>
		/// Measure the distance between this point and the other point
		/// </summary>
		/// <param name="otherPoint">The point to measure to</param>
		/// <returns>An absolute value of the distance specified in the units of the point</returns>
        public T DistanceTo(Point2D<T> otherPoint)
        {
			return (T) Normalise((dynamic)(this - otherPoint));
        }

		#region Rotation
		/// <summary>
		/// Rotate this point around a pivot point for the specified rotation
		/// <para>This method is immutable</para>
		/// </summary>
		/// <param name="pivotPoint">The point to pivot around</param>
		/// <param name="rotationDegrees">The degrees to pivot.  Degrees is specified in an anti-clockwise direction</param>
		/// <returns>A new point at the rotated position relative to the pivot point</returns>
		public Point2D<T> Rotate(Point2D<T> pivotPoint, double rotationDegrees)
		{
			double radians = rotationDegrees.ToRadians();
			double sin = Math.Sin(radians);
			double cos = Math.Cos(radians);

			T x1 = (dynamic) X - pivotPoint.X;
			T y1 = (dynamic) Y - pivotPoint.Y;

			double x2 = (dynamic) x1 * cos - (dynamic) y1 * sin;
			double y2 = (dynamic) x1 * sin + (dynamic) y1 * cos;

			double x3 = Math.Round((dynamic)x2 + pivotPoint.X);
			double y3 = Math.Round((dynamic)y2 + pivotPoint.Y);

			return new Point2D<T>((T)(dynamic) x3, (T)(dynamic) y3);
		}

		#endregion

		/// <summary>
		/// Get a point a specified distance and angle away from the current point
		/// </summary>
		/// <param name="angle">The angle to traverse away from</param>
		/// <param name="distance">The distance to traverse</param>
		/// <param name="decimals">The number of decimal points to include in the result (only for float and double types)</param>
		/// <returns>A new point the specified distance and angle away from this point</returns>
		public Point2D<T> LocatePointAtDistance(double angle, T distance, int decimals = 0)
		{
			double radians = angle.ToRadians();
			double cosValue = (dynamic)distance * Math.Cos(radians);
			double sinValue = (dynamic)distance * Math.Sin(radians);
			T cValue = (T)(dynamic)Math.Round(cosValue, decimals, MidpointRounding.AwayFromZero);
			T sValue = (T)(dynamic)Math.Round(sinValue, decimals, MidpointRounding.ToEven);
			T newX = (dynamic) cValue + X;
			T newY = (dynamic) sValue + Y;
			return new Point2D<T>(newX, newY);
		}

		#region IEquatable Functions
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
            if (!(obj is Point2D<T>))
            {
                return false;
            }
            return this.Equals((Point2D<T>)obj);
        }

		/// <summary>
		/// <para>Compare this class field values to another instance</para>
		/// <para>Compare each of the fields of this instance with the supplied instance to ensure that each field contains the same value</para>
		/// </summary>
		/// <param name="rhs">The object to compare against</param>
		/// <returns>true if this object equals the other object</returns>
		public bool Equals(Point2D<T> rhs)
        {
            if (rhs == null)
                return false;
            return X.Equals(rhs.X)
                && Y.Equals(rhs.Y);
        }

		/// <summary>
		/// Generate a hashcode for this object
		/// </summary>
		/// <returns>A hashcode</returns>
		public override int GetHashCode()
        {
			return LanguageUtils.RSHash(X, Y);
        }
		#endregion

		/// <summary>
		/// Output a string representation of the contents of this class
		/// </summary>
		/// <returns>A string representing each of the points in the class</returns>
		public override string ToString()
		{
			return "[" + X + "," + Y + "]";
		}

		#region IComparable<Point2D<T>> Members

		/// <summary>
		/// Implement a comparable interface.  
		/// <para>Comparison is formed based on the distance from the origin</para>
		/// <para>A negative distance indicates that this point is less (negative X and/or negative Y from other point</para>
		/// </summary>
		/// <param name="other">The other point to check</param>
		/// <returns>Return value is the difference in distance from the origin</returns>
		public int CompareTo(Point2D<T> other)
		{
			T d1 = this.DistanceTo(Origin);
			T d2 = other.DistanceTo(Origin);
			return (dynamic) d2 - d1;
		}

		#endregion

		public static Point2D<T> operator +(Point2D<T> lhs, Point2D<T> rhs)
		{
			return new Point2D<T>(
				(dynamic)lhs.X + rhs.X,
				(dynamic)lhs.Y + rhs.Y);
		}
		public static Point2D<T> operator -(Point2D<T> lhs, Point2D<T> rhs)
		{
			return new Point2D<T>(
				(dynamic)lhs.X - rhs.X,
				(dynamic)lhs.Y - rhs.Y);
		}
		public static Point2D<T> operator *(Point2D<T> lhs, Point2D<T> rhs)
		{
			return new Point2D<T>(
				(dynamic)lhs.X * rhs.X,
				(dynamic)lhs.Y * rhs.Y);
		}
		public static Point2D<T> operator /(Point2D<T> lhs, Point2D<T> rhs)
		{
			return new Point2D<T>(
				(dynamic)lhs.X / rhs.X,
				(dynamic)lhs.Y / rhs.Y);
		}
		public static Point2D<T> operator /(Point2D<T> lhs, Int32 divisor)
		{
			return new Point2D<T>(
				(dynamic)lhs.X / divisor,
				(dynamic)lhs.Y / divisor);
		}

		/// <summary>
		/// Fetch the dot product of two points.  The multiplication of 
		/// each of the axis co-ordinates in the point.
		/// </summary>
		/// <param name="lhs">Point 1</param>
		/// <param name="rhs">Point 2</param>
		/// <returns>The dot product</returns>
		public static decimal DotProduct(Point2D<T> lhs, Point2D<T> rhs)
		{
			return Convert.ToDecimal(((dynamic)lhs.X * rhs.X + (dynamic)lhs.Y * rhs.Y));
		}

		/// <summary>
		/// Fetch the dot product of two points.  The multiplication of 
		/// each of the axis co-ordinates in the point.
		/// </summary>
		/// <param name="rhs">The other point</param>
		/// <returns>The dot product</returns>
		public decimal DotProduct(Point2D<T> rhs)
		{
			return DotProduct(this, rhs);
		}

		/// <summary>
		/// The length of a vector/point
		/// </summary>
		/// <param name="lhs">The point</param>
		/// <returns>The square root of the square of the point</returns>
		public static decimal Normalise(Point2D<T> lhs)
		{
			return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(Normalise2(lhs))));
		}

		/// <summary>
		/// Squared length of a point
		/// </summary>
		/// <param name="lhs">The point to square</param>
		/// <returns>The squared value of the point</returns>
		public static decimal Normalise2(Point2D<T> lhs)
		{
			return DotProduct(lhs, lhs);
		}


	}
	
}
