using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace AU.Com.BushLife.Spatial.TwoD
{
	[TestFixture]
	public class LineTest
	{
		private IEnumerable<object[]> ParallelTestDataProvider
		{
			get
			{
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 10), new Line2D<Int32>(0, 0, 10, 0), false };
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 0), new Line2D<Int32>(0, 0, 0, 10), false };
				yield return new object[] { new Line2D<Int32>(10, 0, 0, 0), new Line2D<Int32>(0, 10, 0, 0), false };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 0), new Line2D<Int32>(10, 0, 0, 0), false };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 10), new Line2D<Int32>(10, 0, 10, 0), true };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 10), new Line2D<Int32>(10, 0, 10, 0), true };
				yield return new object[] { new Line2D<Int32>(0, 10, 10, 10), new Line2D<Int32>(10, 0, 10, 10), false };
				yield return new object[] { new Line2D<Int32>(10, 10, 0, 0), new Line2D<Int32>(0, 0, 10, 10), true };
				yield return new object[] { new Line2D<Int32>(16974, 5831, 16974, 5231), new Line2D<Int32>(17026, 5179, 9818, 5179), false };

			}
		}

		[Test, Factory("ParallelTestDataProvider")]
		public void ParallelTest(Line2D<Int32> lhs, Line2D<Int32> rhs, bool result)
		{
			Assert.AreEqual(result, lhs.Parallel(rhs));
		}


		private IEnumerable<object[]> IntersectingTestDataProvider
		{
			get
			{
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 10), new Line2D<Int32>(0, 0, 10, 0), true };
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 0), new Line2D<Int32>(0, 0, 0, 10), true };
				yield return new object[] { new Line2D<Int32>(10, 0, 0, 0), new Line2D<Int32>(0, 10, 0, 0), true };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 0), new Line2D<Int32>(10, 0, 0, 0), true };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 10), new Line2D<Int32>(10, 0, 10, 0), false };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 10), new Line2D<Int32>(10, 0, 10, 0), false };
				yield return new object[] { new Line2D<Int32>(0, 10, 10, 10), new Line2D<Int32>(10, 0, 10, 10), true };
				yield return new object[] { new Line2D<Int32>(10, 10, 0, 0), new Line2D<Int32>(0, 0, 10, 10), true }; // coincident
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 10), new Line2D<Int32>(10, 10, 0, 0), true }; // coincident
				yield return new object[] { new Line2D<Int32>(16974, 5831, 16974, 5231), new Line2D<Int32>(17026, 5179, 9818, 5179), false };
				yield return new object[] { new Line2D<Int32>(200, 9069, 4432, 9069), new Line2D<Int32>(-1228, 9069, 350, 9069), true };
				yield return new object[] { new Line2D<Int32>(200, 9069, 4432, 9069), new Line2D<Int32>(-1228, 9069, 350, 9069), true };
				yield return new object[] { new Line2D<Int32>(10424, -2597, 10424, 4159), new Line2D<Int32>(10424,-6475,10424,-2597), true };
			}
		}

		[Test, Factory("IntersectingTestDataProvider")]
		public void IntersectingTest(Line2D<Int32> lhs, Line2D<Int32> rhs, bool result)
		{
			Assert.AreEqual(result, lhs.Intersects(rhs));
		}

		#region Intersection Test

		private IEnumerable<object[]> IntersectionTestDataProvider
		{
			get
			{
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 10), new Line2D<Int32>(0, 0, 10, 0), new PointLocus<Int32>(new Point2D<Int32>(0, 0)) };
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 0), new Line2D<Int32>(0, 0, 0, 10), new PointLocus<Int32>(new Point2D<Int32>(0, 0)) };
				yield return new object[] { new Line2D<Int32>(10, 0, 0, 0), new Line2D<Int32>(0, 10, 0, 0), new PointLocus<Int32>(new Point2D<Int32>(0, 0)) };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 0), new Line2D<Int32>(10, 0, 0, 0), new PointLocus<Int32>(new Point2D<Int32>(0, 0)) };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 10), new Line2D<Int32>(10, 0, 10, 0), new EmptyLocus<Int32>() };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 10), new Line2D<Int32>(10, 0, 10, 0), new EmptyLocus<Int32>() };
				yield return new object[] { new Line2D<Int32>(0, 10, 10, 10), new Line2D<Int32>(10, 0, 10, 10), new PointLocus<Int32>(new Point2D<Int32>(10, 10)) };
				yield return new object[] { new Line2D<Int32>(10, 10, 0, 0), new Line2D<Int32>(0, 0, 10, 10), new LineSegmentLocus<Int32>() { Segments = new List<Line2D<Int32>>() { new Line2D<Int32>(0, 0, 10, 10) } } }; // coincident
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 10), new Line2D<Int32>(10, 10, 0, 0), new LineSegmentLocus<Int32>() { Segments = new List<Line2D<Int32>>() { new Line2D<Int32>(0, 0, 10, 10) } } }; // coincident
				yield return new object[] { new Line2D<Int32>(16974, 5831, 16974, 5231), new Line2D<Int32>(17026, 5179, 9818, 5179), new EmptyLocus<Int32>() };
				yield return new object[] { new Line2D<Int32>(200, 9069, 4432, 9069), new Line2D<Int32>(-1228, 9069, 350, 9069), new LineSegmentLocus<Int32>() { Segments = new List<Line2D<Int32>>() { new Line2D<Int32>(200, 9069, 350, 9069) } } };
				yield return new object[] { new Line2D<Int32>(10424, -2597, 10424, 4159), new Line2D<Int32>(10424, -6475, 10424, -2597), new PointLocus<Int32>(new Point2D<Int32>(10424, -2597)) };
				yield return new object[] { new Line2D<Int32>(10424, -2597, 10424, 4159), new Line2D<Int32>(10424, -6475, 10424, -2598), new EmptyLocus<Int32>() };
				yield return new object[] { new Line2D<Int32>(-8035, 1959, -8035, 7449), new Line2D<Int32>(-5735, 3671, -8035, 3671), new PointLocus<Int32>(new Point2D<Int32>(-8035, 3671)) };
				yield return new object[] { new Line2D<Int32>(-5735, 3671, -8035, 3671), new Line2D<Int32>(-8035, 1959, -8035, 7449), new PointLocus<Int32>(new Point2D<Int32>(-8035, 3671)) };
				yield return new object[] { new Line2D<Int32>(-1879, 3493, -1879, 7449), new Line2D<Int32>(-4918, 2854, -1325, 6447), new PointLocus<Int32>(new Point2D<Int32>(-1879, 5893)) };
				yield return new object[] { new Line2D<Int32>(-4918, 2854, -1325, 6447), new Line2D<Int32>(-1879, 3493, -1879, 7449), new PointLocus<Int32>(new Point2D<Int32>(-1879, 5893)) };
			}
		}

		[Test, Factory("IntersectionTestDataProvider")]
		public void IntersectionTest(Line2D<Int32> lhs, Line2D<Int32> rhs, Locus<Int32> expected)
		{
			Assert.AreEqual(expected, lhs.Intersection(rhs));
		}
		#endregion


		private IEnumerable<object[]> CoincidentTestDataProvider
		{
			get
			{
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 10), new Line2D<Int32>(0, 0, 10, 0), false };
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 0), new Line2D<Int32>(0, 0, 0, 10), false };
				yield return new object[] { new Line2D<Int32>(10, 0, 0, 0), new Line2D<Int32>(0, 10, 0, 0), false };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 0), new Line2D<Int32>(10, 0, 0, 0), false };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 10), new Line2D<Int32>(10, 0, 10, 0), true };
				yield return new object[] { new Line2D<Int32>(0, 10, 10, 0), new Line2D<Int32>(10, 0, 0, 10), true };
				yield return new object[] { new Line2D<Int32>(0, 10, 10, 10), new Line2D<Int32>(10, 0, 10, 10), false };
				yield return new object[] { new Line2D<Int32>(10, 10, 0, 0), new Line2D<Int32>(0, 0, 10, 10), true };
				yield return new object[] { new Line2D<Int32>(5, 5, 7, 7), new Line2D<Int32>(0, 0, 10, 10), true };
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 10), new Line2D<Int32>(5, 5, 7, 7), true };
				yield return new object[] { new Line2D<Int32>(22934, -5857, 22934, -751), new Line2D<Int32>(22934, 4229, 22934, 4799), true };
				yield return new object[] { new Line2D<Int32>(22934, -5857, 22934, -751), new Line2D<Int32>(22934, -5650, 22934, -751), true };
				yield return new object[] { new Line2D<Int32>(200, 9069, 4432, 9069), new Line2D<Int32>(-1228, 9069, 350, 9069), true };
			}
		}

		[Test, Factory("CoincidentTestDataProvider")]
		public void CoincidentTest(Line2D<Int32> lhs, Line2D<Int32> rhs, bool result)
		{
			Assert.AreEqual(result, lhs.Coincident(rhs));
		}

		private IEnumerable<object[]> OverlapTestDataProvider
		{
			get
			{
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 10), new Line2D<Int32>(0, 0, 10, 0), new EmptyLocus<Int32>() };
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 0), new Line2D<Int32>(0, 0, 0, 10), new EmptyLocus<Int32>() };
				yield return new object[] { new Line2D<Int32>(10, 0, 0, 0), new Line2D<Int32>(0, 10, 0, 0), new EmptyLocus<Int32>() };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 0), new Line2D<Int32>(10, 0, 0, 0), new EmptyLocus<Int32>() };
				yield return new object[] { new Line2D<Int32>(0, 10, 0, 10), new Line2D<Int32>(10, 0, 10, 0), new EmptyLocus<Int32>() };
				yield return new object[] { new Line2D<Int32>(0, 10, 10, 0), new Line2D<Int32>(10, 0, 0, 10), new LineSegmentLocus<Int32>() { Segments = new List<Line2D<Int32>>() { new Line2D<Int32>(0, 10, 10, 0) } } };
				yield return new object[] { new Line2D<Int32>(0, 10, 10, 10), new Line2D<Int32>(10, 0, 10, 10), new EmptyLocus<Int32>() };
				yield return new object[] { new Line2D<Int32>(10, 10, 0, 0), new Line2D<Int32>(0, 0, 10, 10), new LineSegmentLocus<Int32>() { Segments = new List<Line2D<Int32>>() { new Line2D<Int32>(0, 0, 10, 10) } } };
				yield return new object[] { new Line2D<Int32>(5, 5, 7, 7), new Line2D<Int32>(0, 0, 10, 10), new LineSegmentLocus<Int32>() { Segments = new List<Line2D<Int32>>() { new Line2D<Int32>(5, 5, 7, 7) } } };
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 10), new Line2D<Int32>(5, 5, 7, 7), new LineSegmentLocus<Int32>() { Segments = new List<Line2D<Int32>>() { new Line2D<Int32>(5, 5, 7, 7) } } };
				yield return new object[] { new Line2D<Int32>(22934, -5857, 22934, -751), new Line2D<Int32>(22934, 4229, 22934, 4799), new EmptyLocus<Int32>() };
				yield return new object[] { new Line2D<Int32>(22934, -5857, 22934, -751), new Line2D<Int32>(22934, -5650, 22934, -751), new LineSegmentLocus<Int32>() { Segments = new List<Line2D<Int32>>() { new Line2D<Int32>(22934, -5650, 22934, -751) } } };
				yield return new object[] { new Line2D<Int32>(200, 9069, 4432, 9069), new Line2D<Int32>(-1228, 9069, 350, 9069), new LineSegmentLocus<Int32>() { Segments = new List<Line2D<Int32>>() { new Line2D<Int32>(200, 9069, 350, 9069) } } };

			}
		}

		[Test, Factory("OverlapTestDataProvider")]
		public void OverlapTest(Line2D<Int32> lhs, Line2D<Int32> rhs, Locus<Int32> result)
		{
			Assert.AreEqual(result, lhs.Overlaps(rhs));
		}

		private IEnumerable<object[]> EqualsTestDataProvider
		{
			get
			{
				yield return new object[] { new Line2D<Int32>(10, 10, 10, 10), new Line2D<Int32>(10, 10, 10, 10), true };
				yield return new object[] { new Line2D<Int32>(10, 101, 10, 10), new Line2D<Int32>(10, 101, 10, 10), true };
				yield return new object[] { new Line2D<Int32>(10, 101, 10, 10), new Line2D<Int32>(10, 10, 10, 101), true };
				yield return new object[] { new Line2D<Int32>(5, 10, 10, 10), new Line2D<Int32>(5, 10, 10, 10), true };
				yield return new object[] { new Line2D<Int32>(5, 10, 10, 10), new Line2D<Int32>(10, 10, 5, 10), true };
				yield return new object[] { new Line2D<Int32>(5, 6, 7, 8), new Line2D<Int32>(10, 10, 10, 10), false };
				yield return new object[] { new Line2D<Int32>(200, 125, -10, 10), new Line2D<Int32>(-10, 10, 200, 125), true };
				yield return new object[] { new Line2D<Int32>(200, 125, -10, 10), new Line2D<Int32>(-10, -10, 200, 125), false };
			}
		}

		[Test, Factory("EqualsTestDataProvider")]
		public void EqualsTest(Line2D<Int32> lhs, Line2D<Int32> rhs, bool result)
		{
			Assert.AreEqual(result, lhs.Equals(rhs));
		}

		[Test]
		public void CloneTest()
		{
			Line2D<Int32> line1 = new Line2D<Int32>(24, 25, 26, 27);
			Line2D<Int32> line2 = new Line2D<Int32>(24, 25, 26, 27);
			Line2D<Int32> line3 = (Line2D<Int32>)line1.Clone<Line2D<Int32>, Int32>();
			Assert.AreEqual(line1, line2);
			Assert.AreEqual(line1, line3);
			line1.Shift(7, 5);
			Assert.AreNotEqual(line1, line2);
			Assert.AreNotEqual(line1, line3);
		}

		#region Line Extension test
		private IEnumerable<object[]> LineExtensionDataProvider
		{
			get
			{
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 0), 0, 10, new Line2D<Int32>(0, 0, 20, 0) };
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 10), 0, 10, new Line2D<Int32>(0, 0, 0, 20) };
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 0), 10, 0, new Line2D<Int32>(-10, 0, 10, 0) };
				yield return new object[] { new Line2D<Int32>(10, 0, 10, 10), 10, 10, new Line2D<Int32>(10, -10, 10, 20) };
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 10), 10, 0, new Line2D<Int32>(0, -10, 0, 10) };
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 0), 10, 10, new Line2D<Int32>(-10, 0, 20, 0) };
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 10), 10, 10, new Line2D<Int32>(0, -10, 0, 20) };
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 10), 0, 10, new Line2D<Int32>(0, 0, 17, 17) };
				yield return new object[] { new Line2D<Int32>(0, 0, 10, 10), 10, 10, new Line2D<Int32>(-7, -7, 17, 17) };
				yield return new object[] { new Line2D<Int32>(50, 20, 700, 6400), 250, 150, new Line2D<Int32>(25, -229, 715, 6549) };
				yield return new object[] { new Line2D<Int32>(-1078, 1991, -1078, 7391), 150, 150, new Line2D<Int32>(-1078, 1841, -1078, 7541) };
				yield return new object[] { new Line2D<Int32>(-1338, -6397, -1338, -2797), 150, 150, new Line2D<Int32>(-1338, -6547, -1338, -2647) };
				yield return new object[] { new Line2D<Int32>(-1338, -2797, -1338, -6397), 150, 150, new Line2D<Int32>(-1338, -2647, -1338, -6547) };
				yield return new object[] { new Line2D<Int32>(-3636, 5056, -6946, 5056), 150, 150, new Line2D<Int32>(-3486, 5056, -7096, 5056) };
				yield return new object[] { new Line2D<Int32>(5056, -3636, 5056, -6946), 150, 150, new Line2D<Int32>(5056, -3486, 5056, -7096) };
			}
		}

		[Test, Factory("LineExtensionDataProvider")]
		public void ExtendLineTest(Line2D<Int32> originalLine, Int32 extension1, Int32 extension2, Line2D<Int32> expectedLine)
		{
			Assert.AreEqual(expectedLine, originalLine.Extend( extension1, extension2));
		}
		#endregion

		private IEnumerable<object[]> IsPerpendicularDataProvider
		{
			get
			{
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 100), new Line2D<Int32>(0, 0, 100, 0), true };
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 101), new Line2D<Int32>(0, 0, 100, 0), true };
			}
		}


		[Test,Factory("IsPerpendicularDataProvider")]
		public void IsPerpendicularTest(Line2D<Int32> line1, Line2D<Int32> line2, bool expectedResult)
		{
			Assert.AreEqual(expectedResult, line1.IsPerpendicular(line2));
			Assert.AreEqual(expectedResult, line2.IsPerpendicular(line1));
		}

		#region AngleTo Test
		private IEnumerable<object[]> ToAngleDataProvider
		{
			get
			{
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 100), new Line2D<Int32>(0, 0, 100, 0), 90.0 };
				yield return new object[] { new Line2D<Int32>(0, 100, 0, 0), new Line2D<Int32>(100, 0, 0, 0), 90.0 };
				yield return new object[] { new Line2D<Int32>(0, 100, 0, 0), new Line2D<Int32>(0, 0, 0, 100), 180.0 };
				yield return new object[] { new Line2D<Int32>(100, 100, 0, 0), new Line2D<Int32>(0, 0, 100, 100), 180.0 };
				yield return new object[] { new Line2D<Int32>(110, 110, 10, 10), new Line2D<Int32>(10, 10, 110, 110), 180.0 };
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 100), new Line2D<Int32>(0, 0, 100, 100), 45 };
				yield return new object[] { new Line2D<Int32>(0, 0, 0, 100), new Line2D<Int32>(10, 10, 100, 100), 45 };
			}
		}

		[Test,Factory("ToAngleDataProvider")]
		public void AngleToTest(Line2D<Int32> line1, Line2D<Int32> line2, double expectedAngle)
		{
			Assert.AreEqual(expectedAngle, line1.AngleTo(line2));
		}
		#endregion


	}
}
