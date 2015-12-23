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
    public class SupportProviderInterface
    {
        /// <summary>
        /// Delegate defining the interface for the support progress event initialisation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void InitialiseSupportProgressEvent(object sender, InitialiseProgressEventArgs e);

        /// <summary>
        /// Event to notify the intialisation of a progress monitoring event
        /// </summary>
        public event InitialiseSupportProgressEvent InitialiseSupportProgress;

        /// <summary>
        /// Event to notify an incremental step has been completed in the progress
        /// </summary>
        public event EventHandler UpdateSupportProgress = delegate {};

        /// <summary>
        /// Log a support request with the support provider, including the log file
        /// and supplied problem description
        /// </summary>
        /// <param name="supportEmailAddress">The email address to send the support request to</param>
        /// <param name="logfileAppenderName">The name of the log file appender as configured in the log4net configuration for the application</param>
        /// <param name="problemDescription">The user described problem description</param>
        /// <param name="stepsToReproduce">The user described steps to reproduce the issue</param>
        /// <param name="allFiles">Flag indicating if all log files should be extracted</param>
        public void LogSupportRequest(string supportEmailAddress, string logfileAppenderName, string problemDescription, string stepsToReproduce, bool allFiles)
        {
            var emailClient = new EmailClient();

            emailClient.FromAddress = new MailAddress(CurrentUserEmailAddress(), CurrentUserDisplayName());

            var messageBody = string.Format("Problem Description:\n\n{0}\n\n\nSteps to Reproduce:\n\n{1}\n\n", problemDescription, stepsToReproduce);

            MailMessage message = emailClient.CreateMessage("NOES Support Request from " + System.Environment.MachineName, messageBody);

            var files = Log4NetExtensions.GetLogFiles(logfileAppenderName, allFiles);
            // Initialise any progress listeners
            if (InitialiseSupportProgress != null)
            {
                var e = new InitialiseProgressEventArgs() { ElementCount = files.Count };
                InitialiseSupportProgress(this, e);
            }
            foreach (var logfilePath in files)
            {
                var logfileName = Path.GetFileName(logfilePath);
                using (var stream = new FileStream(logfilePath, FileMode.Open))
                {
                    Attachment logfile = emailClient.CreateGzipAttachment(stream, logfileName + ".gz", EmailClient.ApplicationGzip);
                    message.Attachments.Add(logfile);
                }
                // Update any progress listeners
                if (UpdateSupportProgress != null)
                    UpdateSupportProgress(this, EventArgs.Empty);
            }

            var supportAddress = new MailAddress(supportEmailAddress);
            emailClient.SendEmail(message, supportAddress);
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

    }
}

