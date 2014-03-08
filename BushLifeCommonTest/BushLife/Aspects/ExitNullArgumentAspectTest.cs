using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

using PostSharp.Aspects;
using AU.Com.BushLife.Aspects;

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

		[Test, ExpectedArgumentNullException]
		[ExitNullArgumentCheckAspect(PropertyName = "testClass1")]
		public void ExitNullArgumentNullTest()
		{
			testClass1 = null;
		}

		[Test, ExpectedArgumentNullException]
		[ExitNullArgumentCheckAspect(PropertyName = "testClass1.MyPropertyTestClass")]
		public void ExitNullArgumentNullLevel2Test()
		{
			testClass1 = new MyTestClass();
		}
	}
}
