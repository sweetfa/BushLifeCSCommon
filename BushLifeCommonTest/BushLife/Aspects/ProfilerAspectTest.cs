﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AU.Com.BushLife.Aspects
{
	[TestFixture]
    [DeploymentItem("log4net.Config")]
    public class ProfilerAspectTest
	{
        [Log4NetLoggerAspect(ConfigFileName = "log4net.config", LogFileName = "MyFirstLogFile.log")]
        public class TestClass
		{
			[ProfilerAspect(AspectPriority=2)]
			public void Method1()
			{
			}

            [ProfilerAspect(AspectPriority = 2)]
			public void Method2(Int32 val)
			{
			}
            [ProfilerAspect(AspectPriority = 2)]
			public void Method3(string strparam, Int32 val)
			{
			}
            [ProfilerAspect(AspectPriority = 2)]
			public void Method4<T>(T val)
			{
			}

            [ProfilerAspect(AspectPriority = 2)]
			public void Method5<T1, T2>(T1 val1, T2 val2)
			{
			}

            [ProfilerAspect(AspectPriority = 2)]
            public T Method6<T>()
                where T: new()
            {
                return new T();
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
            cut.Method6<Int32>();
		}
	}
}
