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
 * @(#) EmailClient.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.IO;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Utils
{
    /// <summary>
    /// Class to provide client interface to .NET email system
    /// <para>Add the following configuration to your app.config file</para>
    /// <code>
    /// <para>   &lt;system.net&gt;</para>
	/// <para>      &lt;mailSettings&gt;</para>
	/// <para>        &lt;smtp deliveryMethod="Network"&gt;</para>
	/// <para>          &lt;network</para>
	/// <para>            defaultCredentials="false"</para>
	/// <para>            enableSsl="false"</para>
	/// <para>            host="smtp.ho.bushlife.com.au"</para>
	/// <para>            port="25"</para>
	/// <para>          /&gt;</para>
	/// <para>        &lt;/smtp&gt;</para>
	/// <para>      &lt;/mailSettings&gt;</para>
	/// <para>    &lt;/system.net&gt;</para>
	/// </code>
    /// </summary>
    public class EmailClient : IDisposable
    {
        /// <summary>
        /// Adobe Portable Document Format (Adobe PDF)
        /// </summary>
        public static readonly ContentType ApplicationPDF = new ContentType("application/pdf");

        /// <summary>
        /// XML - Extensible Markup Language (W3C XML)
        /// </summary>
        public static readonly ContentType ApplicationXML = new ContentType("application/XML");

        /// <summary>
        /// The client to use to send the email
        /// </summary>
        private SmtpClient Client
        {
            get
            {
                if (m_Client == null)
                    m_Client = new SmtpClient();
                return m_Client;
            }
        }
        private SmtpClient m_Client = null;

        /// <summary>
        /// The address that all emails from this client are sent from
        /// </summary>
        public MailAddress FromAddress { get; set; }

        /// <summary>
        /// Send an email message to the specified address set
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="toAddresses">The set of addresses to forward the message to</param>
        /// <returns>true if the email was sent</returns>
        public bool SendEmail(MailMessage message, params MailAddress[] toAddresses)
        {
            message.To.AddRange(toAddresses);
            message.From = FromAddress;
            Client.Send(message);
            return true;
        }

        /// <summary>
        /// Create a message comprised of the specified subject and body
        /// </summary>
        /// <param name="subject">The subject line of the message</param>
        /// <param name="messageBody">The message body of the message</param>
        /// <returns>The created mail message</returns>
        public MailMessage CreateMessage(string subject, string messageBody)
        {
            MailMessage message = new MailMessage();
            message.Subject = subject;
            message.Body = messageBody;
            return message;
        }

        /// <summary>
        /// Create a message comprised of the specified subject and body
        /// and include any provided attachments as well
        /// </summary>
        /// <param name="subject">The subject line of the message</param>
        /// <param name="messageBody">The message body of the message</param>
        /// <param name="attachments">The set of attachments to attach to the message</param>
        /// <returns>The created mail message</returns>
        public MailMessage CreateMessage(string subject, string messageBody, params Attachment[] attachments)
        {
            MailMessage message = new MailMessage();
            message.Subject = subject;
            message.Body = messageBody;
            message.Attachments.AddRange(attachments);
            return message;
        }

        /// <summary>
        /// Create an attachment from the specified input stream
        /// </summary>
        /// <param name="stream">The stream the attachment is to be extracted from</param>
        /// <param name="attachmentName">The name to use on the attachment in the message</param>
        /// <param name="mimeType">The MIME type of the attachment</param>
        /// <returns>A formatted attachment</returns>
        public Attachment CreateAttachment(Stream stream, string attachmentName, ContentType mimeType)
        {
            Attachment attachment = new Attachment(stream, attachmentName, mimeType.ToString());
            return attachment;
        }

        /// <summary>
        /// Clean up when garbage collected
        /// </summary>
        public void Dispose()
        {
            if (m_Client != null)
                m_Client.Dispose();
            m_Client = null;
        }
    }
}
