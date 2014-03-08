using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace AU.Com.BushLife.Spatial.ThreeD
{
	[TestFixture]
	public class Line3DTest
	{
		private IEnumerable<object[]> LineIntersectionDataProvider
		{
			get
			{
				yield return new object[] { new Line3D<Int32>(0, 0, 0, 100, 0, 0), new Line3D<Int32>(100, 0, 0, 200, 0, 0), true, new Point3D<Int32>(100, 0, 0) };
				yield return new object[] { new Line3D<Int32>(0, 0, 0, 100, 0, 0), new Line3D<Int32>(101, 0, 0, 200, 0, 0), false, new Point3D<Int32>(100, 0, 0) };
				yield return new object[] { new Line3D<Int32>(0, 0, 0, 0, 100, 0), new Line3D<Int32>(0, 100, 0, 0, 200, 0), true, new Point3D<Int32>(0, 100, 0) };
				yield return new object[] { new Line3D<Int32>(0, 0, 0, 100, 100, 0), new Line3D<Int32>(50, 0, 0, 50, 100, 0), true, new Point3D<Int32>(50, 50, 0) };
				yield return new object[] { new Line3D<Int32>(0, 0, 0, 100, 100, 0), new Line3D<Int32>(-20, 0, 0, 80, 100, 0), false, new Point3D<Int32>(50, 50, 0) };
				yield return new object[] { new Line3D<Int32>(168, -6475, 0, 168, 4237, 0), new Line3D<Int32>(2312, 4237, 0, 5168, 4237, 0), false, new Point3D<Int32>(50, 50, 0) };
			}
		}

		[Test,Factory("LineIntersectionDataProvider"),Disable("Function does not work so disable tests until it is working again")]
		public void Intersection3DTest(Line3D<Int32> line1, Line3D<Int32> line2, bool intersects, Point3D<Int32> intersectionPoint)
		{
			Point3D<Int32> ip;
			Assert.AreEqual(intersects, line1.Intersects(line2, out ip));
			if (intersects)
				Assert.AreEqual(intersectionPoint, ip);
		}
	}
}
