using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using log4net;

namespace AU.Com.BushLife.Aspects
{
    [TestFixture]
    public class Log4NetLoggerBasicTest
    {
        private static ILog Logger = LogManager.GetLogger(typeof(Log4NetLoggerBasicTest).FullName);

        [Test]
        public void Log4NetBasicTest()
        {
            Logger.Info("This is a test message");
        }
    }
}
