using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using TypeMock.ArrangeActAssert;

namespace AU.Com.BushLife.Aspects
{
    [TestFixture]
	[Isolated]
	public class ChangeCursorAttributeTest
	{
		[Test]
		[Isolated]
		public void ChangeCursorAttributeTest1()
		{
			#region Setup Test Data
			TestClass cut = Isolate.Fake.Instance<TestClass>(Members.MustSpecifyReturnValues);
			Isolate.WhenCalled(() => cut.MyMethod()).CallOriginal();
			#endregion

			#region Execute test
			cut.MyMethod();
			#endregion

			#region Validate Results
			Cursor waitCursor = Cursors.WaitCursor;
			Cursor defaultCursor = Cursors.Default;
			Isolate.Verify.WasCalledWithExactArguments(() => cut.Cursor = waitCursor);
			Isolate.Verify.WasCalledWithExactArguments(() => cut.Cursor = defaultCursor);
			#endregion
		}

		[Test]
		[Isolated]
		public void ChangeCursorAttributeTest2()
		{
			#region Setup Test Data
			TestClass cut = Isolate.Fake.Instance<TestClass>(Members.MustSpecifyReturnValues);
			#endregion

			#region Execute test
			cut.ExtensionMethod();
			#endregion

			#region Validate Results
			Cursor waitCursor = Cursors.WaitCursor;
			Cursor defaultCursor = Cursors.Default;
			Isolate.Verify.WasCalledWithExactArguments(() => cut.Cursor = waitCursor);
			Isolate.Verify.WasCalledWithExactArguments(() => cut.Cursor = defaultCursor);
			#endregion
		}
	}

	public static class ExtensionClass
	{
		[ChangeCursor(CursorPropertyName = "Cursor", NewCursorTypeName = "WaitCursor")]
		public static bool ExtensionMethod(this TestClass myclass)
		{
			return false;
		}
	}

	public class TestClass : Form
	{
		[ChangeCursor(CursorPropertyName = "Cursor", NewCursorTypeName = "WaitCursor")]
		public bool MyMethod()
		{
			return false;
		}
	}

}
