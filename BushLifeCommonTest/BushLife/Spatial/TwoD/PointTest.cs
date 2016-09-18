using System;
using System.Collections.Generic;
using System.Text;

namespace AU.Com.BushLife.Spatial.TwoD
{
    using MbUnit.Framework;
    using NUnit.Framework;

    [TestFixture]
    public class PointTest
    {
        [Test]
        public void TestEquals()
        {
            Point2D<Int16> point1 = new Point2D<Int16>(12, 15);
            Point2D<Int16> point2 = new Point2D<Int16>(15, 12);
            Point2D<Int16> point3 = new Point2D<Int16>(12, 15);
            Point2D<Int32> point4 = new Point2D<Int32>(12, 15);

            Assert.AreEqual(false, point1.Equals(null));

            Assert.AreEqual(false, point1.Equals(point2));
            Assert.AreEqual(false, point2.Equals(point1));

            Assert.AreEqual(true, point1.Equals(point3));
            Assert.AreEqual(true, point3.Equals(point1));

            Assert.AreEqual(false, point2.Equals(point3));
            Assert.AreEqual(false, point3.Equals(point2));

            Assert.AreEqual(false, point1.Equals(point4));
            Assert.AreEqual(false, point4.Equals(point1));

            Assert.AreEqual(true, point1.Equals(point1));
            Assert.AreEqual(true, point4.Equals(point4));
        }

        private static IEnumerable<object[]> TestDataProvider
        {
            get
            {
                yield return new object[] { new Point2D<Int32>(0, 0), new Point2D<Int32>(3, 4), 5 };
                yield return new object[] { new Point2D<Int32>(0, 0), new Point2D<Int32>(4, 3), 5 };
                yield return new object[] { new Point2D<Int32>(4, 0), new Point2D<Int32>(4, 4), 4 };
                yield return new object[] { new Point2D<Int32>(0, 4), new Point2D<Int32>(3, 4), 3 };
            }
        }

        [Test, TestCaseSource("TestDataProvider")]
        public void TestDistanceTo(Point2D<Int32> point1, Point2D<Int32> point2, Int32 result)
        {
            Assert.AreEqual(result, point1.DistanceTo(point2));
            Assert.AreEqual(result, point2.DistanceTo(point1));
        }

		public class RotateTestData
		{
			public Point2D<Int32> OriginalPoint { get; set; }
			public Point2D<Int32> PivotPoint { get; set; }
			public double RotationDegrees { get; set; }
			public Point2D<Int32> ExpectedPoint { get; set; }
		}

		private static IEnumerable<RotateTestData> RotationTestProvider
		{
			get
			{
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(0, 0),
					ExpectedPoint = new Point2D<int>(0, 0),
					PivotPoint = new Point2D<int>(0, 0),
					RotationDegrees = 90
				};
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(3, 4),
					ExpectedPoint = new Point2D<int>(-4, 3),
					PivotPoint = new Point2D<int>(0, 0),
					RotationDegrees = 90
				};
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(4, 4),
					ExpectedPoint = new Point2D<int>(0, 6),
					PivotPoint = new Point2D<int>(0, 0),
					RotationDegrees = 45
				};
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(4, 4),
					ExpectedPoint = new Point2D<int>(4, 4),
					PivotPoint = new Point2D<int>(0, 0),
					RotationDegrees = 360
				};
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(4, 4),
					ExpectedPoint = new Point2D<int>(0, 6),
					PivotPoint = new Point2D<int>(0, 0),
					RotationDegrees = 405
				};
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(-3, -4),
					ExpectedPoint = new Point2D<int>(4, -3),
					PivotPoint = new Point2D<int>(0, 0),
					RotationDegrees = 90
				};
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(4, 3),
					ExpectedPoint = new Point2D<int>(5, 2),
					PivotPoint = new Point2D<int>(6, 4),
					RotationDegrees = 30
				};
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(22934, 7157),
					ExpectedPoint = new Point2D<int>(3668, 26423),
					PivotPoint = new Point2D<int>(3668, 7157),
					RotationDegrees = 90
				};
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(22934, 7157),
					ExpectedPoint = new Point2D<int>(-15558, 7157),
					PivotPoint = new Point2D<int>(3688, 7157),
					RotationDegrees = 180
				};
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(22934, 7157),
					ExpectedPoint = new Point2D<int>(3688, -12089),
					PivotPoint = new Point2D<int>(3688, 7157),
					RotationDegrees = 270
				};
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(16778, 7157),
					ExpectedPoint = new Point2D<int>(3688, 20247),
					PivotPoint = new Point2D<int>(3688, 7157),
					RotationDegrees = 90
				};
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(16778, 7157),
					ExpectedPoint = new Point2D<int>(-9402, 7157),
					PivotPoint = new Point2D<int>(3688, 7157),
					RotationDegrees = 180
				};
				yield return new RotateTestData()
				{
					OriginalPoint = new Point2D<int>(16778, 7157),
					ExpectedPoint = new Point2D<int>(3688, -5933),
					PivotPoint = new Point2D<int>(3688, 7157),
					RotationDegrees = 270
				};
			}
		}

		[Test, TestCaseSource("RotationTestProvider")]
		public void TestRotate(RotateTestData data)
		{
			Assert.AreEqual(data.ExpectedPoint, data.OriginalPoint.Rotate(data.PivotPoint, data.RotationDegrees));
		}

		private static IEnumerable<object[]> LocatePointAtDistanceTestProvider
		{
			get
			{
				yield return new object[] { new Point2D<Int32>(0, 0), 0, 10, new Point2D<Int32>(10, 0) };
				yield return new object[] { new Point2D<Int32>(0, 0), 90, 10, new Point2D<Int32>(0, 10) };
				yield return new object[] { new Point2D<Int32>(0, 0), 45, 10, new Point2D<Int32>(7, 7) };
				yield return new object[] { new Point2D<Int32>(0, 0), 180, 10, new Point2D<Int32>(-10, 0) };
				yield return new object[] { new Point2D<Int32>(0, 0), 270, 10, new Point2D<Int32>(0, -10) };
				yield return new object[] { new Point2D<Int32>(0, 0), 225, 10, new Point2D<Int32>(-7, -7) };
				yield return new object[] { new Point2D<Int32>(0, 0), 135, 10, new Point2D<Int32>(-7, 7) };
				yield return new object[] { new Point2D<Int32>(0, 0), 315, 10, new Point2D<Int32>(7, -7) };
				yield return new object[] { new Point2D<Int32>(10, 10), 0, 10, new Point2D<Int32>(20, 10) };
				yield return new object[] { new Point2D<Int32>(10, 0), 0, 10, new Point2D<Int32>(20, 0) };
				yield return new object[] { new Point2D<Int32>(0, 10), 0, 10, new Point2D<Int32>(10, 10) };
				yield return new object[] { new Point2D<Int32>(10, 10), 45, 10, new Point2D<Int32>(17, 17) };
				yield return new object[] { new Point2D<Int32>(10, 0), 45, 10, new Point2D<Int32>(17, 7) };
				yield return new object[] { new Point2D<Int32>(0, 10), 45, 10, new Point2D<Int32>(7, 17) };
				yield return new object[] { new Point2D<Int32>(10, 10), 225, 10, new Point2D<Int32>(3, 3) };
				yield return new object[] { new Point2D<Int32>(10, 0), 225, 10, new Point2D<Int32>(3, -7) };
				yield return new object[] { new Point2D<Int32>(0, 10), 225, 10, new Point2D<Int32>(-7, 3) };
			}
		}

		[Test, TestCaseSource("LocatePointAtDistanceTestProvider")]
		public void LocatePointAtDistanceTest(Point2D<Int32> originalPoint, double angle, int distance, Point2D<Int32> expectedPoint)
		{
			Assert.AreEqual(expectedPoint, originalPoint.LocatePointAtDistance(angle, distance));
		}
    }
}
