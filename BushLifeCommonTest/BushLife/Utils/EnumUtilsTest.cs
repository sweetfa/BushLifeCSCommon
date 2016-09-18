using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AU.Com.BushLife.Utils
{
	[TestFixture]
	public class EnumUtilsTest
	{
		public enum MyEnum
		{
			Value1,
			Value2,
			Value3,
			Value4,
			Value5
		}

		[Test]
		public void ConvertTest()
		{
			IList<Int32> testValues = new List<Int32>() { 2, 4, 3, 1, 3 };
			IList<MyEnum> resultValues = new List<MyEnum>() { MyEnum.Value3, MyEnum.Value5, MyEnum.Value4, MyEnum.Value2, MyEnum.Value4 };

			IList<MyEnum> result = EnumUtils.Convert<MyEnum, Int32>(testValues);

			CollectionAssert.AreEqual(resultValues, result);
		}
	}
}
