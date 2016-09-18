using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AU.Com.BushLife.Spatial.TwoD
{
	[TestFixture]
	public class RectangleTest
	{
		private static IEnumerable<object[]> ContainsTestData
		{
			get
			{
				#region Polygon Vertices Test
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(100,0),
						new Point2D<Int32>(0,100),
						new Point2D<Int32>(100,100)
						), 
					new Point2D<Int32>(0,0), 
					true 
				};
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(100,0),
						new Point2D<Int32>(0,100),
						new Point2D<Int32>(100,100)
						), 
					new Point2D<Int32>(0,100), 
					true 
				};
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(100,0),
						new Point2D<Int32>(0,100),
						new Point2D<Int32>(100,100)
						), 
					new Point2D<Int32>(100,0), 
					true 
				};
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(100,0),
						new Point2D<Int32>(0,100),
						new Point2D<Int32>(100,100)
						), 
					new Point2D<Int32>(100,100), 
					true 
				};
				#endregion

				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(100,0),
						new Point2D<Int32>(0,100),
						new Point2D<Int32>(100,100)
						), 
					new Point2D<Int32>(1,1), 
					true 
				};
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(100,0),
						new Point2D<Int32>(0,100),
						new Point2D<Int32>(100,100)
						), 
					new Point2D<Int32>(99,99), 
					true 
				};
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(100,0),
						new Point2D<Int32>(0,100),
						new Point2D<Int32>(100,100)
						), 
					new Point2D<Int32>(1,99), 
					true 
				};
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(100,0),
						new Point2D<Int32>(0,100),
						new Point2D<Int32>(100,100)
						), 
					new Point2D<Int32>(99,1), 
					true 
				};
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(100,0),
						new Point2D<Int32>(0,100),
						new Point2D<Int32>(100,100)
						), 
					new Point2D<Int32>(-1,-1), 
					false 
				};
				#region NonOrthagonal Rectangles
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(50,50),
						new Point2D<Int32>(-50,50),
						new Point2D<Int32>(0,100)
						), 
					new Point2D<Int32>(-1,-1), 
					false 
				};
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(50,50),
						new Point2D<Int32>(-50,50),
						new Point2D<Int32>(0,100)
						), 
					new Point2D<Int32>(0,25), 
					true 
				};
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(50,50),
						new Point2D<Int32>(-50,50),
						new Point2D<Int32>(0,100)
						), 
					new Point2D<Int32>(25,0), 
					false 
				};
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(50,50),
						new Point2D<Int32>(-50,50),
						new Point2D<Int32>(0,100)
						), 
					new Point2D<Int32>(26,26), 
					true 
				};
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(50,50),
						new Point2D<Int32>(-50,50),
						new Point2D<Int32>(0,100)
						), 
					new Point2D<Int32>(18,30), 
					true 
				};
				yield return new object[] 
				{ 
					new Rectangle<Int32>(
						new Point2D<Int32>(0,0),
						new Point2D<Int32>(50,50),
						new Point2D<Int32>(-50,50),
						new Point2D<Int32>(0,100)
						), 
					new Point2D<Int32>(30,18), 
					false 
				};
				#endregion
			}
		}

		[Test]
		[TestCaseSource("ContainsTestData")]
		public void RectangleContainsPointsTest(Rectangle<Int32> box, Point2D<Int32> point, bool expectedResult)
		{
			Assert.AreEqual(expectedResult, box.Contains(point));
		}
	}
}
