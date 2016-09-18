using System;
using System.Collections.Generic;
using System.Text;
using PostSharp.Aspects;
using AU.Com.BushLife.Aspects;
using NUnit.Framework;

namespace BushLifeCommonTest.BushLife.Aspects
{
	[TestFixture]
	public class ExitNullArgumentAspectTest
	{
		public MyTestClass testClass1 { get; set; }

		public class MyTestClass
		{
			public string MyString { get; set; }
			public MyPropertyTestClass MyPropertyTestClass { get; set; }
		}

		public class MyPropertyTestClass
		{
			public string MyOtherString { get; set; }
			public MyPropertyOtherTestClass MyOtherTestClass { get; set; }
		}

		public class MyPropertyOtherTestClass
		{
			public string MyProperty { get; set; }
		}

		[Test]
		[ExitNullArgumentCheckAspect(PropertyName="testClass1")]
		public void ExitNullArgumentValidTest()
		{
			testClass1 = new MyTestClass();
		}

		[Test]
		[ExitNullArgumentCheckAspect(PropertyName = "testClass1.MyString")]
		public void ExitNullArgumentValidLevel2Test()
		{
			testClass1 = new MyTestClass();
			testClass1.MyString = "Hello";
		}

		[Test]
		[ExitNullArgumentCheckAspect(PropertyName = "testClass1.MyPropertyTestClass.MyOtherString")]
		public void ExitNullArgumentValidLevel3Test()
		{
			testClass1 = new MyTestClass();
			testClass1.MyString = "Hello";
			testClass1.MyPropertyTestClass = new MyPropertyTestClass();
			testClass1.MyPropertyTestClass.MyOtherString = "Hello";
		}

        [Test]
		public void ExitNullArgumentNullTest()
		{
			Assert.Throws<ArgumentNullException>(() => ExitNullMethod1());
		}

        [ExitNullArgumentCheckAspect(PropertyName = "testClass1")]
        private void ExitNullMethod1()
        {
            testClass1 = null;
        }

		[Test]
		public void ExitNullArgumentNullLevel2Test()
		{
			Assert.Throws<ArgumentNullException>(() => ExitNullMethod2());
		}

        [ExitNullArgumentCheckAspect(PropertyName = "testClass1.MyPropertyTestClass")]
        private void ExitNullMethod2()
        {
            testClass1 = new MyTestClass();
        }
	}
}
