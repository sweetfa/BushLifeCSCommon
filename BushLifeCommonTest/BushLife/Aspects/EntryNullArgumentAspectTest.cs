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
	public class EntryNullArgumentAspectTest
	{
		public MyTestClass testClass1 { get; set; }

		public class MyTestClass
		{
			public string MyString { get; set; }

			[EntryNullArgumentCheckAspect(PropertyName = "MyString")]
			public void SomeMethod()
			{
			}
		}


		[Test]
		public void EntryNullArgumentAspectValidTest()
		{
			testClass1 = new MyTestClass();
			testClass1.MyString = "Hello";
			testClass1.SomeMethod();
		}

		[Test,ExpectedArgumentNullException]
		public void EntryNullArgumentAspectNullTest()
		{
			testClass1 = new MyTestClass();
			testClass1.MyString = null;
			testClass1.SomeMethod();
		}
	}
}
