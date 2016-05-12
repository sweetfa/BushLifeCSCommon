﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AU.Com.BushLife.Aspects
{
	[TestClass]
    [DeploymentItem("log4net.Config")]
	public class Log4NetLoggerAspectTest
	{
        private string FilePath { get; set; }

        [Log4NetLoggerAspect(ConfigFileName = "log4net.config", LogFileName = "MySecondLogFile.log")]
        public class TestClass
        {
            public void Method1()
            {
            }

            public void Method2(Int32 arg1)
            {
            }

            public void Method3(Int32 arg1, string arg2)
            {
            }

            public void Method4()
            {
                throw new Exception("This is an exception");
            }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Log4NetLoggerAspect.Initialised = false;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            string pathname = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            FilePath = Path.Combine(pathname, "MySecondLogFile.log");
            File.Delete(FilePath);
        }

        [TestMethod]
		public void Log4NetAspectTest1()
		{
			TestClass cut = new TestClass();

			cut.Method1();
			cut.Method2(32);
			cut.Method3(567, "Hello dolly");
            try
            {
                cut.Method4();
            }
            catch (Exception)
            {
            }
            Assert.IsTrue(File.Exists(FilePath));
		}

	}
}
