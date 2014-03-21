using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

using TypeMock.ArrangeActAssert;

using log4net;
using AU.Com.BushLife.Utils;
using AU.Com.BushLife.Exceptions;

namespace AU.Com.BushLife.Aspects.ExceptionHandlers
{
	[TestFixture]
	public class ShowExceptionAttributeTest
	{
		private static ILog Logger = LogManager.GetLogger(typeof(ShowExceptionAttributeTest));

		[Test]
		[Isolated]
		public void ShowExceptionTestSpecificException1()
		{
			#region Set up test data
			string message = "Hello dolly\n\nSystem.ArgumentException\nSomething is wrong";
			string caption = "Exception Occurred";
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage image = MessageBoxImage.Error;

			Isolate.Fake.StaticMethods<Window>(Members.MustSpecifyReturnValues);
			Isolate.Fake.StaticMethods<System.Windows.MessageBox>(Members.ReturnRecursiveFakes);
			//Isolate.WhenCalled(() => System.Windows.MessageBox.Show((string)null, (string)null, MessageBoxButton.OK, MessageBoxImage.Error)).DoInstead((c) => DumpMessage(c, new object[] { message, caption, button, image }));
			#endregion

			#region Execute test
			try
			{
				TestClass cut = new TestClass();
				cut.MyMethod1();
			}
			catch (AlreadyHandledException)
			{
			}
			#endregion

			#region Verity results
			Isolate.Verify.WasCalledWithExactArguments(() => System.Windows.MessageBox.Show(message, caption, button, image));
			#endregion
		}

		[Test]
		[Isolated]
		public void ShowExceptionTestSpecificException2()
		{
			#region Set up test data
			string message = "Hello dolly";
			string caption = "Exception Occurred";
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage image = MessageBoxImage.Error;

			Isolate.Fake.StaticMethods<Window>(Members.MustSpecifyReturnValues);
			Isolate.Fake.StaticMethods<System.Windows.MessageBox>(Members.ReturnRecursiveFakes);
			//Isolate.WhenCalled(() => System.Windows.MessageBox.Show((string)null, (string)null, MessageBoxButton.OK, MessageBoxImage.Error)).DoInstead((c) => DumpMessage(c, new object[] { message, caption, button, image }));
			#endregion

			#region Execute test
			try
			{
				TestClass cut = new TestClass();
				cut.MyMethod2();
			}
			catch (AlreadyHandledException)
			{
			}
			#endregion

			#region Verity results
			Isolate.Verify.WasCalledWithExactArguments(() => System.Windows.MessageBox.Show(message, caption, button, image));
			#endregion
		}

		[Test]
		[Isolated]
		[ExpectedArgumentException]
		public void ShowExceptionTestSpecificException3()
		{
			#region Set up test data
			string message = "Hello dolly\n\nSystem.ArgumentException\nValue cannot be null.\r\nParameter name: Something is wrong as well";
			string caption = "Exception Occurred";
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage image = MessageBoxImage.Error;

			Isolate.Fake.StaticMethods<Window>(Members.MustSpecifyReturnValues);
			Isolate.Fake.StaticMethods<System.Windows.MessageBox>(Members.ReturnRecursiveFakes);
			//Isolate.WhenCalled(() => System.Windows.MessageBox.Show((string)null, (string)null, MessageBoxButton.OK, MessageBoxImage.Error)).DoInstead((c) => DumpMessage(c, new object[] { message, caption, button, image }));
			#endregion

			#region Execute test
			try
			{
				TestClass cut = new TestClass();
				cut.MyMethod3();
			}
			catch (ArgumentNullException)
			{
			}
			#endregion

			#region Verity results
			Isolate.Verify.WasNotCalled(() => System.Windows.MessageBox.Show(message, caption, button, image));
			#endregion
		}

		[Test]
		[Isolated]
		[ExpectedArgumentException]
		public void ShowExceptionTestSpecificException4()
		{
			#region Set up test data
			string message = "Hello dolly";
			string caption = "Exception Occurred";
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage image = MessageBoxImage.Error;

			Isolate.Fake.StaticMethods<Window>(Members.MustSpecifyReturnValues);
			Isolate.Fake.StaticMethods<System.Windows.MessageBox>(Members.ReturnRecursiveFakes);
			//Isolate.WhenCalled(() => System.Windows.MessageBox.Show((string)null, (string)null, MessageBoxButton.OK, MessageBoxImage.Error)).DoInstead((c) => DumpMessage(c, new object[] { message, caption, button, image }));
			#endregion

			#region Execute test
			try
			{
				TestClass cut = new TestClass();
				cut.MyMethod4();
			}
			catch (ArgumentNullException)
			{
			}
			#endregion

			#region Verity results
			Isolate.Verify.WasNotCalled(() => System.Windows.MessageBox.Show(message, caption, button, image));
			#endregion
		}

		[Test]
		[Isolated]
		public void ShowExceptionTestSpecificException5()
		{
			#region Set up test data
			string message = "Hello dolly 4";
			string caption = "Exception Occurred";
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage image = MessageBoxImage.Error;

			Isolate.Fake.StaticMethods<Window>(Members.MustSpecifyReturnValues);
			Isolate.Fake.StaticMethods<System.Windows.MessageBox>(Members.ReturnRecursiveFakes);
			//Isolate.WhenCalled(() => System.Windows.MessageBox.Show((string)null, (string)null, MessageBoxButton.OK, MessageBoxImage.Error)).DoInstead((c) => DumpMessage(c, new object[] { message, caption, button, image }));
			#endregion

			#region Execute test
			try
			{
				TestClass cut = new TestClass();
				cut.MyMethod7();
			}
			catch (AlreadyHandledException)
			{
			}
			#endregion

			#region Verity results
			Isolate.Verify.WasCalledWithExactArguments(() => System.Windows.MessageBox.Show(message, caption, button, image));
			#endregion
		}

		[Test]
		[Isolated]
		[ExpectedArgumentException]
		public void ShowExceptionTestSpecificException6()
		{
			#region Set up test data
			string message = "Hello dolly\n\nSystem.ArgumentException\nValue cannot be null.\r\nParameter name: Something is wrong as well";
			string caption = "Exception Occurred";
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage image = MessageBoxImage.Error;

			Isolate.Fake.StaticMethods<Window>(Members.MustSpecifyReturnValues);
			Isolate.Fake.StaticMethods<System.Windows.MessageBox>(Members.ReturnRecursiveFakes);
			//Isolate.WhenCalled(() => System.Windows.MessageBox.Show((string)null, (string)null, MessageBoxButton.OK, MessageBoxImage.Error)).DoInstead((c) => DumpMessage(c, new object[] { message, caption, button, image }));
			#endregion

			#region Execute test
			try
			{
				TestClass cut = new TestClass();
				cut.MyMethod8();
			}
			catch (AlreadyHandledException)
			{
			}
			#endregion

			#region Verity results
			Isolate.Verify.WasCalledWithExactArguments(() => System.Windows.MessageBox.Show(message, caption, button, image));
			#endregion
		}

		private MessageBoxResult DumpMessage(TypeMock.MethodCallContext context, object[] expectedValues)
		{
			StreamUtils converter = new StreamUtils(Encoding.ASCII);
			int i = 0;
			foreach (var param in context.Parameters)
			{
				Console.WriteLine(string.Format("{0}:[{1}]", param.GetType().FullName, param.ToString()));
				Console.WriteLine(string.Join(" ", converter.ConvertToBytes(param.ToString()).Select(c => c.ToString("x2"))));
				Console.WriteLine(string.Join(" ", converter.ConvertToBytes(expectedValues[i].ToString()).Select(c => c.ToString("x2"))));
				Console.WriteLine(string.Format("Comparison {0}:{1}", param == expectedValues[i], param.Equals(expectedValues[i])));
				++i;
			}
			return MessageBoxResult.OK;
		}

		[Test]
		[Isolated]
		public void ShowExceptionTestAnyException1()
		{
			#region Set up test data
			string message = "Hello dolly 2\n\nSystem.ArgumentNullException\nValue cannot be null.\r\nParameter name: Something is wrong as well";
			string caption = "Exception Occurred";
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage image = MessageBoxImage.Error;

			Isolate.Fake.StaticMethods<Window>(Members.MustSpecifyReturnValues);
			Isolate.Fake.StaticMethods<System.Windows.MessageBox>(Members.ReturnRecursiveFakes);
			//Isolate.WhenCalled(() => System.Windows.MessageBox.Show((string)null, (string)null, MessageBoxButton.OK, MessageBoxImage.Error)).DoInstead((c) => DumpMessage(c, new object[] { message, caption, button, image }));
			#endregion

			#region Execute test
			try
			{
				TestClass cut = new TestClass();
				cut.MyMethod5();
			}
			catch (ArgumentNullException)
			{
			}
			#endregion

			#region Verity results
			Isolate.Verify.WasCalledWithExactArguments(() => System.Windows.MessageBox.Show(message, caption, button, image));
			#endregion
		}

		[Test]
		[Isolated]
		public void ShowExceptionTestAnyException2()
		{
			#region Set up test data
			string message = "Hello dolly 2";
			string caption = "Exception Occurred";
			MessageBoxButton button = MessageBoxButton.OK;
			MessageBoxImage image = MessageBoxImage.Error;

			Isolate.Fake.StaticMethods<Window>(Members.MustSpecifyReturnValues);
			Isolate.Fake.StaticMethods<System.Windows.MessageBox>(Members.ReturnRecursiveFakes);
			//Isolate.WhenCalled(() => System.Windows.MessageBox.Show((string)null, (string)null, MessageBoxButton.OK, MessageBoxImage.Error)).DoInstead((c) => DumpMessage(c, new object[] { message, caption, button, image }));
			#endregion

			#region Execute test
			try
			{
				TestClass cut = new TestClass();
				cut.MyMethod6();
			}
			catch (ArgumentNullException)
			{
			}
			#endregion

			#region Verity results
			Isolate.Verify.WasCalledWithExactArguments(() => System.Windows.MessageBox.Show(message, caption, button, image));
			#endregion
		}

		[Test]
		[Isolated]
		public void TestMessageBoxTest()
		{
			#region Set up test data
			Isolate.Fake.StaticMethods<System.Windows.MessageBox>(Members.ReturnRecursiveFakes);
			#endregion

			#region Execute test
			System.Windows.MessageBox.Show("Hello dolly", "", MessageBoxButton.OK, MessageBoxImage.Error);
			#endregion

			#region Verity results
			Isolate.Verify.WasCalledWithExactArguments(() => System.Windows.MessageBox.Show("Hello dolly", "", MessageBoxButton.OK, MessageBoxImage.Error));
			#endregion
		}
	}

	public class TestClass : Form
	{
		[ShowException(Message = "Hello dolly", SpecificExceptionType = typeof(System.ArgumentException), ShowExceptionDetail = true)]
		public bool MyMethod1()
		{
			throw new ArgumentException("Something is wrong");
		}

		[ShowException(Message = "Hello dolly", SpecificExceptionType = typeof(System.ArgumentException), ShowExceptionDetail = false)]
		public bool MyMethod2()
		{
			throw new ArgumentException("Something is wrong");
		}

		[ShowException(Message = "Hello dolly", SpecificExceptionType = typeof(System.ArgumentNullException), ShowExceptionDetail = true)]
		public bool MyMethod3()
		{
			throw new ArgumentException("Something is wrong if you don't receive this exception without a message");
		}

		[ShowException(Message = "Hello dolly", SpecificExceptionType = typeof(System.ArgumentNullException), ShowExceptionDetail = false)]
		public bool MyMethod4()
		{
			throw new ArgumentException("Something is wrong if you don't receive this exception without a message");
		}

		[ShowException(Message = "Hello dolly 2", ShowExceptionDetail = true)]
		public bool MyMethod5()
		{
			throw new ArgumentNullException("Something is wrong as well");
		}

		[ShowException(Message = "Hello dolly 2", ShowExceptionDetail = false)]
		public bool MyMethod6()
		{
			throw new ArgumentNullException("Something is wrong as well");
		}

		[ShowException(Message = "Hello dolly 3", SpecificExceptionType = typeof(System.ArgumentNullException), ShowExceptionDetail = false, AspectPriority = 5)]
		[ShowException(Message = "Hello dolly 4", ShowExceptionDetail = false, AspectPriority = 6)]
		public bool MyMethod7()
		{
			throw new ArgumentNullException("Something is wrong as well");
		}

		[ShowException(Message = "Hello dolly 3", SpecificExceptionType = typeof(System.ArgumentNullException), ShowExceptionDetail = false, AspectPriority = 5)]
		[ShowException(Message = "Hello dolly 4", ShowExceptionDetail = false, AspectPriority = 6)]
		public bool MyMethod8()
		{
			throw new ArgumentException("Something is wrong as well");
		}
	}

}
