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
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using AU.Com.BushLife.Spatial.TwoD;
using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Spatial.ThreeD
{
	/// <summary>
	/// A representation of a point in three dimensional space
	/// </summary>
	/// <typeparam name="T">The type of an individual co-ordinate</typeparam>
	[DebuggerDisplay("[{X},{Y},{Z}]")]
	public class Point3D<T> : IComparable<Point3D<T>>, ICloneable
	{
		/// <summary>
		/// X axis co-ordinate value
		/// </summary>
		public T X { get; protected set; }
		/// <summary>
		/// Y axis co-ordinate value
		/// </summary>
		public T Y { get; protected set; }
		/// <summary>
		/// Z axis co-ordinate value
		/// </summary>
		public T Z { get; protected set; }

		#region Constructors
		/// <summary>
		/// Constructor taking individual point references
		/// </summary>
		/// <param name="x">X axis offset</param>
		/// <param name="y">Y axis offset</param>
		/// <param name="z">Z axis offset</param>
		public Point3D(T x, T y, T z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Create a 3D point from a 2D point and a Z co-ordinate
		/// </summary>
		/// <param name="point">The two dimensional point</param>
		/// <param name="z">The z co-ordinate</param>
		public Point3D(Point2D<T> point, T z)
		{
			X = point.X;
			Y = point.Y;
			Z = z;
		}
		#endregion

		/// <summary>
		/// Calculate the distance between two points.  May have an issue on origin axis
		/// </summary>
		/// <param name="other">The other point</param>
		/// <returns>The distance in the specified units of the points</returns>
		public T DistanceTo(Point3D<T> other)
		{
			dynamic x = Math.Abs((dynamic)X - other.X);
			dynamic y = Math.Abs((dynamic)Y - other.Y);
			dynamic z = Math.Abs((dynamic)Z - other.Z);
			return (T)Math.Sqrt(x * x + y * y + z * z);
		}

		/// <summary>
		/// Shift a point by the specified adjustment amounts
		/// <para>This method is immutable</para>
		/// </summary>
		/// <param name="xAdjust">The amount to adjust in the X axis</param>
		/// <param name="yAdjust">The amount to adjust in the Y axis</param>
		/// <param name="zAdjust">The amount to adjust in the Z axis</param>
		/// <returns>A new point at the adjusted position</returns>
		public Point3D<T> Shift(T xAdjust, T yAdjust, T zAdjust)
		{
			return new Point3D<T>(
				(dynamic)X + xAdjust,
				(dynamic)Y + yAdjust,
				(dynamic)Z + zAdjust);
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
			if (!(obj is Point3D<T>))
			{
				return false;
			}
			return this.Equals((Point3D<T>)obj);
		}

		/// <summary>
		/// <para>Compare this class field values to another instance</para>
		/// <para>Compare each of the fields of this instance with the supplied instance to ensure that each field contains the same value</para>
		/// </summary>
		/// <param name="rhs">The object to compare against</param>
		/// <returns>true if this object equals the other object</returns>
		public bool Equals(Point3D<T> rhs)
		{
			if (rhs == null)
				return false;
			return X.Equals(rhs.X)
				&& Y.Equals(rhs.Y)
				&& Z.Equals(rhs.Z);
		}

		/// <summary>
		/// Generate a hashcode for this object
		/// </summary>
		/// <returns>A hashcode</returns>
		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(X, Y, Z);
		}
		#endregion

		/// <summary>
		/// Output a string representation of the contents of this class
		/// </summary>
		/// <returns>A string representing each of the points in the class</returns>
		public override string ToString()
		{
			return "[" + X + "," + Y + "," + Z + "]";
		}


		#region IComparable<Point3D<T>> Members

		public int CompareTo(Point3D<T> other)
		{
			// negate if one of the other axis is less than this
			if ((dynamic) other.X < X || (dynamic) other.Y < Y || (dynamic) other.Z < Z)
				return -(int)(object)this.DistanceTo(other);
			return (int) (object) this.DistanceTo(other);
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			return new Point3D<T>(X, Y, Z);
		}

		#endregion

		/// <summary>
		/// Calculate the cross product of this point with another point
		/// </summary>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public Point3D<T> CrossProduct(Point3D<T> rhs)
		{
			return CrossProduct(this, rhs);
		}

		/// <summary>
		/// Convert a three dimensional point to a two dimensional point
		/// <para>This method is immutable</para>
		/// </summary>
		/// <returns>The threeD point less the Z axis</returns>
		public TwoD.Point2D<T> ToPoint2D()
		{
			return new TwoD.Point2D<T>(X, Y);
		}

		public static Point3D<T> operator +(Point3D<T> lhs, Point3D<T> rhs)
		{
			return new Point3D<T>(
				(dynamic)lhs.X + rhs.X,
				(dynamic)lhs.Y + rhs.Y,
				(dynamic)lhs.Z + rhs.Z);
		}
		public static Point3D<T> operator -(Point3D<T> lhs, Point3D<T> rhs)
		{
			return new Point3D<T>(
				(dynamic)lhs.X - rhs.X,
				(dynamic)lhs.Y - rhs.Y,
				(dynamic)lhs.Z - rhs.Z);
		}
		public static Point3D<T> operator *(Point3D<T> lhs, Point3D<T> rhs)
		{
			return new Point3D<T>(
				(dynamic)lhs.X * rhs.X,
				(dynamic)lhs.Y * rhs.Y,
				(dynamic)lhs.Z * rhs.Z);
		}
		public static Point3D<T> operator /(Point3D<T> lhs, Point3D<T> rhs)
		{
			return new Point3D<T>(
				(dynamic)lhs.X / rhs.X,
				(dynamic)lhs.Y / rhs.Y,
				(dynamic)lhs.Z / rhs.Z);
		}
		public static Point3D<T> operator /(Point3D<T> lhs, T divisor)
		{
			return new Point3D<T>(
				(dynamic)lhs.X / divisor,
				(dynamic)lhs.Y / divisor,
				(dynamic)lhs.Z / divisor);
		}
		public static Point3D<T> CrossProduct(Point3D<T> lhs, Point3D<T> rhs)
		{
			return new Point3D<T>(
				(dynamic) lhs.Y * rhs.Z - (dynamic) rhs.Y * lhs.Z, 
				(dynamic) lhs.Z * rhs.X - (dynamic) rhs.Z * lhs.X,
				(dynamic) lhs.X * rhs.Y - (dynamic) rhs.X * lhs.Y);
			//return Coord(b.y() * c.z() - c.y() * b.z(), b.z() * c.x() - c.z() * b.x(), b.x() * c.y() - c.x() * b.y());
		}

		/// <summary>
		/// The dot product is the multiplication of each of the axis values summed together
		/// </summary>
		/// <param name="lhs">The first line</param>
		/// <param name="rhs">The second line</param>
		/// <returns>The multiplicated points summed together</returns>
		public static double DotProduct(Point3D<T> lhs, Point3D<T> rhs)
		{
			return ((dynamic) lhs.X * rhs.X + (dynamic)lhs.Y * rhs.Y + (dynamic)lhs.Z * rhs.Z);
			//return u.x() * v.x() + u.y() * v.y() + u.z() * v.z();
		}
		public static double Normalise(Point3D<T> lhs)
		{
			return Math.Sqrt(Normalise2(lhs));
		}
		public static double Normalise2(Point3D<T> lhs)
		{
			return ((dynamic)lhs.X * lhs.X + (dynamic)lhs.Y * lhs.Y + (dynamic)lhs.Z * lhs.Z);
		}
	}
}
