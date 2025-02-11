﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

using AU.Com.BushLife.Utils;
using NUnit.Framework;

namespace AU.Com.BushLife.Utils
{
    [TestFixture]
    public class EmailClientTest
    {
        [Test]
        public void EmailClientSendTest()
        {
            EmailClient client = new EmailClient();
            client.FromAddress = new MailAddress("noreply@bl_cs_common.bushlife.com.au", "Bush Life Common Test Module", Encoding.UTF8);
            MailMessage message = client.CreateMessage("Bush Life Common Email Test", "This is the body of a message\n\nRegards\nFrank Adcock\n");
            client.SendEmail(message, new MailAddress("frank@bushlife.com.au","Frank Adcock", Encoding.UTF8));
        }

        [Test]
        public void EmailClientSendWithAttachmentTest()
        {
            EmailClient client = new EmailClient();
            client.FromAddress = new MailAddress("noreply@bl_cs_common.bushlife.com.au", "Bush Life Common Test Module", Encoding.UTF8);
            MailMessage message = client.CreateMessage("Bush Life Common Email Test", "This is the body of a message\n\nRegards\nFrank Adcock\n", 
                client.CreateAttachment(new FileStream(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log4net.config"), FileMode.Open), "Attachment 1", EmailClient.ApplicationXML));

            client.SendEmail(message, new MailAddress("frank@bushlife.com.au", "Frank Adcock", Encoding.UTF8), new MailAddress("spider@bushlife.com.au", "Bush Life Spider", Encoding.UTF8));

        }

        [Test]
        public void EmailClientSendWithGzipAttachmentTest()
        {
            EmailClient client = new EmailClient();
            client.FromAddress = new MailAddress("noreply@bl_cs_common.bushlife.com.au", "Bush Life Common Test Module", Encoding.UTF8);
            using (var stream = new FileStream(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "BushLifeCommonTest.dll.config"), FileMode.Open))
            {
                MailMessage message = client.CreateMessage("Bush Life Common Email Test", "This is the body of a message\n\nRegards\nFrank Adcock\n",
                    client.CreateGzipAttachment(stream, "Attachment 1", EmailClient.ApplicationGzip));

                client.SendEmail(message, new MailAddress("frank@bushlife.com.au", "Frank Adcock", Encoding.UTF8), new MailAddress("spider@bushlife.com.au", "Bush Life Spider", Encoding.UTF8));
            }
        }
    }
}
