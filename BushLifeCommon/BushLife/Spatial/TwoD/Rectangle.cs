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
 * @(#) Rectangle.cs
 */

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Spatial.TwoD
{
	/// <summary>
	/// A classic rectangle.  Supports rectangles that are not square to the XY axis
	/// </summary>
	/// <typeparam name="T">The type of the point storage for each point</typeparam>
	[DebuggerDisplay("[{Points[0]},{Points[1]},{Points[2]},{Points[3]}]")]
	public class Rectangle<T>
	{
		public Point2D<T>[] Points { get; set; }
		public Line2D<T>[] Vertices { get; set; }

		/// <summary>
		/// The square T area of this rectangle
		/// </summary>
		public T Area { get { return (dynamic) Vertices.Min(v => v.Length) * Vertices.Max(v => v.Length); } }

		/// <summary>
		/// Determine if the rectangle is an empty/non-existant rectangle
		/// </summary>
		public bool IsEmpty 
		{ 
			get 
			{ 
				return (dynamic) Area == 0 
					&& (dynamic) Vertices[0].Length == 0
					&& (dynamic)Vertices[1].Length == 0
					&& (dynamic)Vertices[2].Length == 0;
			}
		}

		/// <summary>
		/// Return the length of the long edge of the rectangle
		/// </summary>
		public T LongAxisLength
		{
			get
			{
				return Vertices.Max(v => v.Length);
			}
		}

		/// <summary>
		/// Return the length of the short edge of the rectangle
		/// </summary>
		public T ShortAxisLength
		{
			get
			{
				return Vertices.Min(v => v.Length);
			}
		}

		/// <summary>
		/// The minimum X co-ordinate for the rectangle
		/// </summary>
		public T MinX
		{
			get
			{
				return Points.Min(p => p.X);
			}
		}

		/// <summary>
		/// The minumum Y co-ordinate for the rectangle
		/// </summary>
		public T MinY
		{
			get
			{
				return Points.Min(p => p.Y);
			}
		}

		/// <summary>
		/// The maximum X co-ordinate for the rectangle
		/// </summary>
		public T MaxX
		{
			get
			{
				return Points.Max(p => p.X);
			}
		}

		/// <summary>
		/// The maximum Y co-ordinate for the rectangle
		/// </summary>
		public T MaxY
		{
			get
			{
				return Points.Max(p => p.Y);
			}
		}

		/// <summary>
		/// Basic constructor taking the four corners of the rectangle
		/// <para>Point[0] joins to Point[1]</para>
		/// <para>Point[1] joins to Point[2]</para>
		/// <para>Point[2] joins to Point[3]</para>
		/// <para>Point[3] joins to Point[0]</para>
		/// </summary>
		/// <param name="p1">Translates to Point[0]</param>
		/// <param name="p2">Translates to Point[1]</param>
		/// <param name="p3">Translates to Point[2]</param>
		/// <param name="p4">Translates to Point[3]</param>
		public Rectangle(Point2D<T> p1, Point2D<T> p2, Point2D<T> p3, Point2D<T> p4)
		{
			Points = new Point2D<T>[4];
			Points[0] = p1;
			Points[1] = p2;
			Points[2] = p3;
			Points[3] = p4;
			Vertices = new Line2D<T>[4];
			Vertices[0] = new Line2D<T>(p1, p2);
			Vertices[1] = new Line2D<T>(p2, p3);
			Vertices[2] = new Line2D<T>(p3, p4);
			Vertices[3] = new Line2D<T>(p4, p1);
		}

		/// <summary>
		/// Constructor for a rectangle taking the two diagonally opposite corners of the rectangle
		/// </summary>
		/// <param name="p1">diagonal opposite corner from p3</param>
		/// <param name="p3">diagonal opposite corner from p1</param>
		public Rectangle(Point2D<T> p1, Point2D<T> p3)
		{
			Point2D<T> p2 = new Point2D<T>(p1.X, p3.Y);
			Point2D<T> p4 = new Point2D<T>(p3.X, p1.Y);
			Points = new Point2D<T>[4];
			Points[0] = p1;
			Points[1] = p2;
			Points[2] = p3;
			Points[3] = p4;
			Vertices = new Line2D<T>[4];
			Vertices[0] = new Line2D<T>(p1, p2);
			Vertices[1] = new Line2D<T>(p2, p3);
			Vertices[2] = new Line2D<T>(p3, p4);
			Vertices[3] = new Line2D<T>(p4, p1);
		}

		/// <summary>
		/// Determine if two rectangles intersect
		/// <para>This does not include the condition where one
		/// rectangle is enclosed by the other</para>
		/// </summary>
		/// <param name="rhs">The other rectangle</param>
		/// <returns>true if an intersection exists between the two rectangles
		/// less than the entire size of one of the rectangles</returns>
		public bool Intersects(Rectangle<T> rhs)
		{
			foreach (Line2D<T> vertice in Vertices)
				if (rhs.Intersects(vertice))
					return true;
			return false;
		}

		/// <summary>
		/// Determine if a line intersects this rectangle
		/// <para>This does not include lines contained wholly
		/// within the rectangle</para>
		/// </summary>
		/// <param name="line">The line to check</param>
		/// <returns>True if the line intersects a vertice of this rectangle</returns>
		public bool Intersects(Line2D<T> line)
		{
			foreach (Line2D<T> vertice in Vertices)
				if (line.Intersects(vertice))
					return true;
			return false;
		}

		/// <summary>
		/// Determine the intersection of the supplied rectangle with this rectangle.
		/// <para>Returns a rectangle with 0 co-ordinates if the rectangles do not intersect</para>
		/// <para>NOTE: This function only works for orthagonal rectangles</para>
		/// </summary>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public Rectangle<T> Intersection(Rectangle<T> rhs)
		{
			if (!Intersects(rhs))
				return new Rectangle<T>(new Point2D<T>((dynamic)0, (dynamic)0), new Point2D<T>((dynamic)0, (dynamic)0));
			T[] horizontal = { Points[0].X, Points[1].X, Points[2].X, Points[3].X, rhs.Points[0].X, rhs.Points[1].X, rhs.Points[2].X, rhs.Points[3].X };
			T[] vertical = { Points[0].Y, Points[1].Y, Points[2].Y, Points[3].Y, rhs.Points[0].Y, rhs.Points[1].Y, rhs.Points[2].Y, rhs.Points[3].Y };
			Array.Sort(horizontal);
			Array.Sort(vertical);
			return new Rectangle<T>(new Point2D<T>(horizontal[3], vertical[3]), new Point2D<T>(horizontal[4], vertical[4]));
		}

		/// <summary>
		/// Determine the intersection of a line with this rectangle
		/// <para>If the line is contained within the rectangle it is not considered intersecting</para>
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public Locus<T> Intersection(Line2D<T> line)
		{
			Locus<T> result = new EmptyLocus<T>();
			foreach (Line2D<T> vertice in Vertices)
			{
				Locus<T> verticeIntersection = line.Intersection(vertice);
				if (!(verticeIntersection is EmptyLocus<T>))
					result = CombineIntersections((dynamic) result, (dynamic) verticeIntersection);
			}
			return result;
		}

		private Locus<T> CombineIntersections(EmptyLocus<T> current, PointLocus<T> addition)
		{
			return addition;
		}

		private Locus<T> CombineIntersections(EmptyLocus<T> current, LineSegmentLocus<T> addition)
		{
			return addition;
		}

		private Locus<T> CombineIntersections(EmptyLocus<T> current, MultiPointLocus<T> addition)
		{
			return addition;
		}

		private Locus<T> CombineIntersections(PointLocus<T> current, PointLocus<T> addition)
		{
			if (current.Equals(addition))
				return current;
			return new MultiPointLocus<T>(current.IntersectionPoint, addition.IntersectionPoint);
		}

		private Locus<T> CombineIntersections(PointLocus<T> current, LineSegmentLocus<T> addition)
		{
			if (ContainsPoint(addition, current))
				return addition;
			addition.Segments.Add(new Line2D<T>(current.IntersectionPoint, current.IntersectionPoint));
			return addition;
		}

		private Locus<T> CombineIntersections(PointLocus<T> current, MultiPointLocus<T> addition)
		{
			if (!ContainsPoint(addition, current))
				addition.Points.Add(current.IntersectionPoint);
			return addition;
		}

		private Locus<T> CombineIntersections(LineSegmentLocus<T> current, PointLocus<T> addition)
		{
			// If the point to be added already exists in one of the line
			// segments ignore it
			if (ContainsPoint(current, addition))
				return current;
			current.Segments.Add(new Line2D<T>(addition.IntersectionPoint, addition.IntersectionPoint));
			return current;
		}

		/// <summary>
		/// Determine if the point is contained in any of the segments of the line segment
		/// </summary>
		/// <param name="lineSegment"></param>
		/// <param name="point"></param>
		/// <returns>true if the point lands on a line segment</returns>
		private bool ContainsPoint(LineSegmentLocus<T> lineSegment, PointLocus<T> point)
		{
			foreach (Line2D<T> line in lineSegment.Segments)
			{
				if (line.Contains(point.IntersectionPoint))
					return true;
			}
			return false;
		}

		private Locus<T> CombineIntersections(LineSegmentLocus<T> current, LineSegmentLocus<T> addition)
		{
			// If the segment to be added already exists in one of the line
			// segments ignore it
			ICollection<Line2D<T>> linesToAdd = new List<Line2D<T>>();
			foreach (Line2D<T> line in current.Segments)
			{
				if (!line.Equals(addition.Segments))
				{
					linesToAdd.Add(line);
				}
			}
			if (linesToAdd.Count > 0)
				current.Segments.AddRange(linesToAdd);
			return current;
		}

		private Locus<T> CombineIntersections(LineSegmentLocus<T> current, MultiPointLocus<T> addition)
		{
			ICollection<Point2D<T>> additionalPoints = addition.Points;

			// If the points to be added already exists in one of the line
			// segments ignore it
			foreach (Line2D<T> line in current.Segments)
			{
				foreach (Point2D<T> point in addition.Points)
				{
					if (line.Contains(point))
						additionalPoints.Remove(point);
				}
			}
			additionalPoints.ForEach(p => current.Segments.Add(new Line2D<T>(p, p)));
			return current;
		}

		private Locus<T> CombineIntersections(MultiPointLocus<T> current, PointLocus<T> addition)
		{
			if (!ContainsPoint(current, addition))
				current.Points.Add(addition.IntersectionPoint);
			return current;
		}

		private bool ContainsPoint(MultiPointLocus<T> multi, PointLocus<T> point)
		{
			foreach (Point2D<T> p2 in multi.Points)
			{
				if (p2.Equals(point.IntersectionPoint))
					return true;
			}
			return false;
		}

		private Locus<T> CombineIntersections(MultiPointLocus<T> current, LineSegmentLocus<T> addition)
		{
			foreach (Point2D<T> point in current.Points)
			{
				bool found = false;
				foreach (Line2D<T> line in addition.Segments)
				{
					if (line.Contains(point))
						found = true;
				}
				if (!found)
					addition.Segments.Add(new Line2D<T>(point,point));
			}
			return addition;
		}

		private Locus<T> CombineIntersections(MultiPointLocus<T> current, MultiPointLocus<T> addition)
		{
			foreach (Point2D<T> point in addition.Points)
			{
				bool found = false;
				foreach (Point2D<T> p2 in current.Points)
				{
					if (p2.Equals(point))
						found = true;
				}
				if (!found)
					current.Points.Add(point);
			}
			return current;
		}



		/// <summary>
		/// Determine if a point exists within a polygon space
		/// <para>If the point lands on a vertice then the result is inderterminate</para>
		/// </summary>
		/// <param name="point">The point to check</param>
		/// <returns>true if the point resides in the space</returns>
		public bool Contains(Point2D<T> point)
		{
			//return PolySidesContains(point);
			return RayTrace(point);
		}

		/// <summary>
		/// Perform a trace at 90 degree angles from the provided point to see if the ray touches
		/// one of the vertices of this rectangle
		/// <para>If it touches all 4 of the vertices the point is inside the rectangle</para>
		/// </summary>
		/// <param name="point">The origin point for the ray</param>
		/// <returns>true if the ray intercepts a vertice</returns>
		private bool RayTrace(Point2D<T> point)
		{
			// Get the maximum length between any vertices
			T d1 = Points[0].DistanceTo(Points[1]);
			T d2 = Points[0].DistanceTo(Points[2]);
			T d3 = Points[0].DistanceTo(Points[3]);
			T d4 = Points[1].DistanceTo(Points[2]);
			T d5 = Points[1].DistanceTo(Points[3]);
			T d6 = Points[2].DistanceTo(Points[3]);
			T rayLength = Math.Max(Math.Max(Math.Max((dynamic)d1,(dynamic)d2),Math.Max((dynamic)d3,(dynamic)d4)),Math.Max((dynamic)d5,(dynamic)d6));

			int cnt = 0;
			for (int i = 0; i < 360; i += 90)
			{
				Point2D<T> p2 = point.LocatePointAtDistance(i, rayLength);
				Line2D<T> line = new Line2D<T>(point, p2);
				foreach (Line2D<T> vector in Vertices)
				{
					if (line.Intersects(vector))
					{
						++cnt;
						break;
					}
				}
			}
			return cnt > 0 && cnt % 4 == 0;
		}

		private bool PolySidesContains(Point2D<T> point)
		{
			Int32 polySides = Points.Count() - 1;
			int i, j = polySides - 1;
			bool oddNodes = false;

			for (i = 0; i < polySides; i++)
			{
				if (((dynamic)Points[i].Y < point.Y && (dynamic)Points[j].Y >= (dynamic)point.Y
				|| (dynamic)Points[j].Y < point.Y && (dynamic)Points[i].Y >= point.Y)
				&& ((dynamic)Points[i].X <= point.X || (dynamic)Points[j].X <= point.X))
				{
					oddNodes ^= (Points[i].X + ((dynamic)point.Y - Points[i].Y) / ((dynamic)Points[j].Y - Points[i].Y) * ((dynamic)Points[j].X - Points[i].X) < point.X);
				}
				j = i;
			}
			return oddNodes;
		}

		/// <summary>
		/// Determine if a point is encompassed completely within this rectangle.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool Encompasses(Point2D<T> point)
		{
			return RayTrace(point);
		}

		/// <summary>
		/// Determine if a line is encompassed completely within a rectangle
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public bool Encompasses(Line2D<T> line)
		{
			return RayTrace(line.Ends[0]) && RayTrace(line.Ends[1]);
		}
	}
}
