using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeMock.ArrangeActAssert;
using AU.Com.BushLife.Utils;
using System.Net.Mime;
using System.Net.Mail;
using System.Collections.ObjectModel;
using AU.Com.BushLife.Aspects;
using log4net;
using NUnit.Framework;

namespace AU.Com.BushLife.Framework
{
    [Log4NetLoggerAspect(ConfigFileName = "log4net.config", LogFileName = "BushLife.log", AspectPriority = 2, ApplyToStateMachine = false)]
    [TestFixture]
    public class SupportProviderInterfaceTest
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SupportProviderInterfaceTest));

        private static IEnumerable<object[]> LogSupportRequestFileSelectionTestData
        {
            get
            {
                yield return new object[]
                {
                    "test@bushlife.com.au",
                    "LogFileAppender",
                    "Run this test",
                    "Test fails to lodge a support request",
                    true,
                    1
                };
                yield return new object[]
                {
                    "test@bushlife.com.au",
                    "LogFileAppender",
                    "Run this test",
                    "Test fails to lodge a support request",
                    false,
                    1
                };
            }
        }


        [Test]
        [Isolated]
        [TestCaseSource("LogSupportRequestFileSelectionTestData")]
        public void LogSupportRequestFileSelectionTest(string supportEmailAddress,
            string logfileAppenderName,
            string stepsToReproduce, 
            string problemDescription,
            bool allFiles,
            int expectedAdds)
        {
            #region Setup Test
            Log.InfoFormat("Test Starting");

            var attachment = Isolate.Fake.Instance<Attachment>();
            //var added = 0;

            var message = Isolate.Fake.Instance<MailMessage>(Members.MustSpecifyReturnValues, ConstructorWillBe.Called);
            //Isolate.WhenCalled(() => message.Attachments).CallOriginal();
            //Isolate.WhenCalled(() => message.Attachments.Add(attachment)).DoInstead(c => added++);

            var emailClient = Isolate.Fake.Instance<EmailClient>(Members.MustSpecifyReturnValues, ConstructorWillBe.Called);
            Isolate.Swap.AllInstances<EmailClient>().With(emailClient);
            Isolate.WhenCalled(() => emailClient.CreateMessage(null, null)).CallOriginal();
            Isolate.WhenCalled(() => emailClient.CreateAttachment(null, null, null)).CallOriginal();
            Isolate.WhenCalled(() => emailClient.CreateGzipAttachment(null, null, null)).CallOriginal();
            Isolate.WhenCalled(() => emailClient.SendEmail(null, null)).ReturnRecursiveFake();
            #endregion

            #region Execute Test
            new SupportProviderInterface().LogSupportRequest(supportEmailAddress, logfileAppenderName, problemDescription, stepsToReproduce, allFiles);
            #endregion

            #region Verify Results
            //Assert.AreEqual(expectedAdds, added);
            // TODO: Isolate.Verify.WasCalledWithAnyArguments(() => message.Attachments.Add(attachment));
            #endregion
        }
    }
}
