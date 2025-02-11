﻿using System;
using System.Collections.Generic;
using System.Text;
using AU.Com.BushLife.Aspects;
using System.IO;
using log4net;
using NUnit.Framework;

namespace AU.Com.BushLife.Utils
{
    [TestFixture]
    [Log4NetLoggerAspect(ConfigFileName = "log4net.config", LogFileName = "BushLife.log", AspectPriority = 2, ApplyToStateMachine = false)]
    public class Log4NetExtensionsTest
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Log4NetExtensionsTest));

        #region GetLogFilePathTest
        private static IEnumerable<object[]> GetLogFilePathTestData
        {
            get
            {
                yield return new object[]
                {
                    "LogFileAppender",
                    "BushLife.log"
                };
            }
        }

        [Test]
        [TestCaseSource("GetLogFilePathTestData")]
        public void GetLogFilePathTest(string logfileAppenderName, string expectedName)
        {
            Assert.AreEqual(expectedName, Path.GetFileName(Log4NetExtensions.GetLogFilePath(logfileAppenderName)));
        }
        #endregion
        
        #region GetLogFilesTest
        private static IEnumerable<object[]> GetLogFilesTestData
        {
            get
            {
                yield return new object[]
                {
                    new Action(BasicSetup),
                    "LogFileAppender",
                    true,
                    1
                };
                yield return new object[]
                {
                    new Action(BasicSetup),
                    "LogFileAppender",
                    false,
                    1
                };
                yield return new object[]
                {
                    new Action(RolloverSetup),
                    "LogFileAppender",
                    true,
                    2
                };
                yield return new object[]
                {
                    new Action(RolloverSetup),
                    "LogFileAppender",
                    false,
                    1
                };
            }
        }

        private static void BasicSetup()
        {
            Log.Info("Trigger creation of initial log file");
        }

        private static void RolloverSetup()
        {
            for (int i = 0; i < 8000; i++)
                Log.InfoFormat("Filling up the log file with a number of messages {0}", i);
        }

        [SetUp]
        public void Setup()
        {
            var path = Log4NetExtensions.GetLogFilePath("LogFileAppender");
            var files = Log4NetExtensions.GetLogFiles("LogFileAppender", true);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        [Test]
        [TestCaseSource("GetLogFilesTestData")]
        public void GetLogFilesTest(Action initAction, string logfileAppenderName, bool allfiles, int expectedCount)
        {
            initAction();
            var result = Log4NetExtensions.GetLogFiles(logfileAppenderName, allfiles);
            foreach (var file in result)
            {
                Log.Info(file);
            }
            Assert.AreEqual(expectedCount, result.Count);
        }
        #endregion
    }
}
