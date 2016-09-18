/**
 * Copyright (C) 2012 Bush Life Pty Limited
 * 
 * All rights reserved.  No unauthorised copying or redistribution without the prior written 
 * consent of the management of Bush Life Pty Limited.
 * 
 * www.bushlife.com.au
 * sales@bushlife.com.au
 * 
 * PO Box 865, Redcliffe, QLD, 4020, Australia
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NUnit.Framework;

namespace AU.Com.BushLife.Patterns
{
    [TestFixture]
    public class ChainOfCommandTest
    {
        public class MyContext : ICommandContext
        {
            public MyContext()
            {
                accessed = new List<string>();
            }

            public List<string> accessed { get; set; }
			#region ICommandContext Members
			/// <summary>
			/// The current processing step
			/// </summary>
			public Type CurrentProcessingStep { get; set; }
			#endregion
		}

        public class Command1 : ICommand<MyContext>
        {
            public IResult Execute(MyContext context)
            {
                if (context != null)
                {
                    context.accessed.Add(this.GetType().Name);
                }
                return IResult.Successful;
            }
        }

        public class Command2 : ICommand<MyContext>
        {
            public IResult Execute(MyContext context)
            {
                if (context != null)
                {
                    context.accessed.Add(this.GetType().Name);
                }
                return IResult.Successful;
            }
        }

        public class Command3 : ICommand<MyContext>
        {
            public IResult Execute(MyContext context)
            {
                if (context != null)
                {
                    context.accessed.Add(this.GetType().Name);
                }
                return IResult.Failed;
            }
        }

        // Tests destined for success
        private static IEnumerable<object[]> DataProvider1
        {
            get
            {
                List<ICommand<MyContext>> list0 = new List<ICommand<MyContext>>();

                List<ICommand<MyContext>> list1 = new List<ICommand<MyContext>>();
                list1.Add(new Command1());

                List<ICommand<MyContext>> list2 = new List<ICommand<MyContext>>();
                list2.Add(new Command1());
                list2.Add(new Command2());

                yield return new object[] { list0, 0 };
                yield return new object[] { list1, 1 };
                yield return new object[] { list2, 2 };
            }
        }

        // Tests destined for failure
        private static IEnumerable<object[]> DataProvider2
        {
            get
            {
                List<ICommand<MyContext>> list0 = new List<ICommand<MyContext>>();
                list0.Add(new Command3());

                List<ICommand<MyContext>> list1 = new List<ICommand<MyContext>>();
                list1.Add(new Command3());
                list1.Add(new Command1());

                List<ICommand<MyContext>> list2 = new List<ICommand<MyContext>>();
                list2.Add(new Command1());
                list2.Add(new Command2());
                list2.Add(new Command3());

                List<ICommand<MyContext>> list3 = new List<ICommand<MyContext>>();
                list3.Add(new Command1());
                list3.Add(new Command2());
                list3.Add(new Command3());
                list3.Add(new Command1());

                yield return new object[] { list0, 1 };
                yield return new object[] { list1, 1 };
                yield return new object[] { list2, 3 };
                yield return new object[] { list3, 3 };
            }
        }

        [Test, TestCaseSource("DataProvider1")]
        public void TestChainOfCommandWorksNullContext(List<ICommand<MyContext>> commands, int visitedCount)
        {
            ChainOfCommand<MyContext> c = new ChainOfCommand<MyContext>(commands);
            Assert.AreEqual(IResult.Successful, c.Execute(null));
        }

        [Test, TestCaseSource("DataProvider1")]
        public void TestChainOfCommandWorks(List<ICommand<MyContext>> commands, int visitedCount)
        {
            MyContext context = new MyContext();
            ChainOfCommand<MyContext> c = new ChainOfCommand<MyContext>(commands);
            Assert.AreEqual(IResult.Successful, c.Execute(context));
            Assert.AreEqual(visitedCount, context.accessed.Count);
        }

        [Test, TestCaseSource("DataProvider2")]
        public void TestChainOfCommandFailsNullContext(List<ICommand<MyContext>> commands, int visitedCount)
        {
            ChainOfCommand<MyContext> c = new ChainOfCommand<MyContext>(commands);
            Assert.AreEqual(IResult.Failed, c.Execute(null));
        }

        [Test, TestCaseSource("DataProvider2")]
        public void TestChainOfCommandFails(List<ICommand<MyContext>> commands, int visitedCount)
        {
            MyContext context = new MyContext();
            ChainOfCommand<MyContext> c = new ChainOfCommand<MyContext>(commands);
            Assert.AreEqual(IResult.Failed, c.Execute(context));
            Assert.AreEqual(visitedCount, context.accessed.Count);
        }

    }
}
