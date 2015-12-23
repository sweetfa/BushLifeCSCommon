/**
 * Copyright (C) 2014 Bush Life Pty Limited
 * 
 * All rights reserved.  No unauthorised copying or redistribution without the prior written 
 * consent of the management of Bush Life Pty Limited.
 * 
 * www.bushlife.com.au
 * sales@bushlife.com.au
 * 
 * PO Box 865, Redcliffe, QLD, 4020, Australia
 * 
 * 
 * @(#) Log4NetExtensions.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;
using log4net.Appender;
using System.IO;

namespace AU.Com.BushLife.Utils
{
    /// <summary>
    /// Set of extension functions to enhance usage for log4net related applications
    /// </summary>
    public static class Log4NetExtensions
    {
        /// <summary>
        /// Get the path of the log file based on the name of the log file appender
        /// as specified in the log4net configuration file for the calling application
        /// </summary>
        /// <param name="logfileAppenderName">The name of the log file appender 
        /// as specified in the log4net configuration file for the calling application</param>
        /// <returns>The full UNC path name to the log file</returns>
        public static string GetLogFilePath(string logfileAppenderName)
        {
            var appenders = LogManager.GetRepository(Assembly.GetCallingAssembly()).GetAppenders();
            var appender = appenders.Where(l => l.Name == logfileAppenderName).Single();
            var rollingFileAppender = appender as RollingFileAppender;
            return rollingFileAppender.File;
        }

        /// <summary>
        /// Get log files, all or just the latest depending on the state of the allFiles flag
        /// </summary>
        /// <param name="logfileAppenderName">The log4net log file appender to extract the path from</param>
        /// <param name="allFiles">True if all log files are to be retrieved, false if only the most recent</param>
        /// <returns>The list of log file names</returns>
        public static ICollection<String> GetLogFiles(string logfileAppenderName, bool allFiles)
        {
            var logfilePath = GetLogFilePath(logfileAppenderName);
            if (allFiles)
            {
                var dirname = Path.GetDirectoryName(logfilePath);
                var filename = string.Format("{0}*", Path.GetFileName(logfilePath));
                return Directory.GetFiles(dirname, filename).ToList();
            }
            else
            {
                return new List<String>() { logfilePath };
            }
        }

    }
}
