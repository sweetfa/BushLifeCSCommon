using System;
using System.Collections.Generic;
using System.Text;

using PostSharp.Aspects;
using AU.Com.BushLife.Aspects;
using NUnit.Framework;

namespace BushLifeCommonTest.BushLife.Aspects
{
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

		[Test]
		public void EntryNullArgumentAspectNullTest()
		{
			testClass1 = new MyTestClass();
			testClass1.MyString = null;
            Assert.Throws<ArgumentNullException>(() => testClass1.SomeMethod());
		}
	}
}
