using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using log4net;

namespace AU.Com.BushLife.Aspects
{
    [TestClass]
    [DeploymentItem("log4net.Config")]
    public class Log4NetLoggerBasicTest
    {
        private static ILog Logger = LogManager.GetLogger(typeof(Log4NetLoggerBasicTest).FullName);

        [TestMethod]
        public void Log4NetBasicTest()
        {
            Logger.Info("This is a test message");
        }
    }
}
