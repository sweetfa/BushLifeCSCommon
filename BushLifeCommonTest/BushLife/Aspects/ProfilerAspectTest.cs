using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace AU.Com.BushLife.Aspects
{
	[TestFixture]
	public class ProfilerAspectTest
	{
		public class TestClass
		{
			[ProfilerAspect]
			public void Method1()
			{
			}

			[ProfilerAspect]
			public void Method2(Int32 val)
			{
			}
			[ProfilerAspect]
			public void Method3(string strparam, Int32 val)
			{
			}
			[ProfilerAspect]
			public void Method4<T>(T val)
			{
			}

			[ProfilerAspect]
			public void Method5<T1, T2>(T1 val1, T2 val2)
			{
			}
		}

		[Test]
		public void ProfilerTest()
		{
			var cut = new TestClass();

			cut.Method1();
			cut.Method2(33);
			cut.Method3("Hello", 33);
			cut.Method4<Int32>(33);
			cut.Method4<string>("Hello");
			cut.Method5<Int32, string>(34, "Bye");
			cut.Method5<string, Int32>("Bye bye", 35);
		}
	}
}
