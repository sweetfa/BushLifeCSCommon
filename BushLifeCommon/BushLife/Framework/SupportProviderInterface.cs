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
 * @(#) SupportProviderInterface.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Mail;
using System.Net.Mime;
using System.DirectoryServices.AccountManagement;

using log4net;
using log4net.Appender;

using AU.Com.BushLife.Aspects;
using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Framework
{
    /// <summary>
    /// Class to provide a mechanism for lodging support requests via email 
    /// with associated log files.
    /// <para>The assumption is a log4net logfile appender is in place
    /// and the file name that the logfile appender uses is the logfile
    /// that is picked up and attached to the email as an attachment</para>
    /// </summary>
    public static class SupportProviderInterface
    {
        /// <summary>
        /// Log a support request with the support provider, including the log file
        /// and supplied problem description
        /// </summary>
        /// <param name="supportEmailAddress">The email address to send the support request to</param>
        /// <param name="logfileAppenderName">The name of the log file appender as configured in the log4net configuration for the application</param>
        /// <param name="problemDescription">The user described problem description</param>
        /// <param name="stepsToReproduce">The user described steps to reproduce the issue</param>
        /// <param name="allFiles">Flag indicating if all log files should be extracted</param>
        public static void LogSupportRequest(string supportEmailAddress, string logfileAppenderName, string problemDescription, string stepsToReproduce, bool allFiles)
        {
            var emailClient = new EmailClient();

            emailClient.FromAddress = new MailAddress(CurrentUserEmailAddress(), CurrentUserDisplayName());

            var messageBody = string.Format("Problem Description:\n\n{0}\n\n\nSteps to Reproduce:\n\n{1}\n\n", problemDescription, stepsToReproduce);

            MailMessage message = emailClient.CreateMessage("NOES Support Request from " + System.Environment.MachineName, messageBody);

            var files = GetLogFiles(logfileAppenderName, allFiles);
            foreach (var logfilePath in files)
            {
                var logfileName = Path.GetFileName(logfilePath);
                using (var stream = new FileStream(logfilePath, FileMode.Open))
                {
                    Attachment logfile = emailClient.CreateGzipAttachment(stream, logfileName + ".gz", EmailClient.ApplicationGzip);
                    message.Attachments.Add(logfile);
                }
            }

            var supportAddress = new MailAddress(supportEmailAddress);
            emailClient.SendEmail(message, supportAddress);
        }

        /// <summary>
        /// Get log files, all or just the latest depending on the state of the allFiles flag
        /// </summary>
        /// <param name="logfileAppenderName">The log4net log file appender to extract the path from</param>
        /// <param name="allFiles">True if all log files are to be retrieved, false if only the most recent</param>
        /// <returns>The list of log file names</returns>
        private static ICollection<String> GetLogFiles(string logfileAppenderName, bool allFiles)
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

        /// <summary>
        /// Get the current user email address
        /// </summary>
        /// <returns></returns>
        private static string CurrentUserEmailAddress()
        {
            var userPrincipal = UserPrincipal.Current;
            if (userPrincipal != null)
            {
                if (userPrincipal.EmailAddress != null && userPrincipal.EmailAddress.Length > 0)
                    return userPrincipal.EmailAddress;
            }
            var username = WinUtils.GetCurrentUserNameWithoutDomain();
            var domain = IPGlobalProperties.GetIPGlobalProperties();
            return string.Format("{0}@{1}.{2}", username, domain.HostName, domain.DomainName);
        }

        /// <summary>
        /// Get the current user display name
        /// </summary>
        /// <returns></returns>
        private static string CurrentUserDisplayName()
        {
            return WinUtils.GetCurrentUserNameWithoutDomain();
        }

        /// <summary>
        /// Get the path of the log file
        /// <para>TODO: Move this into a set of utilities for logger management</para>
        /// </summary>
        /// <param name="logfileAppenderName">The name of the log file appender 
        /// as specified in the log4net configuration file for the calling application</param>
        /// <returns>The full UNC path name to the log file</returns>
        private static string GetLogFilePath(string logfileAppenderName)
        {
            var appenders = LogManager.GetRepository(Assembly.GetCallingAssembly()).GetAppenders();
            var appender = appenders.Where(l => l.Name == logfileAppenderName).Single();
            var rollingFileAppender = appender as RollingFileAppender;
            return rollingFileAppender.File;
        }
    }
}

