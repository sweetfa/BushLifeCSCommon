using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

using TypeMock.ArrangeActAssert;

namespace AU.Com.BushLife.Aspects
{
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
		public Cursor Cursor { get; set; }

		[ChangeCursor(CursorPropertyName = "Cursor", NewCursorTypeName = "WaitCursor")]
		public bool MyMethod()
		{
			return false;
		}
	}

	[TestFixture]
	public class ChangeCursorAttributeTest
	{
		[Test]
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
			Isolate.Verify.WasCalledWithExactArguments(() => cut.Cursor = Cursors.WaitCursor);
			Isolate.Verify.WasCalledWithExactArguments(() => cut.Cursor = Cursors.Default);
			#endregion
		}

		[Test]
		public void ChangeCursorAttributeTest2()
		{
			#region Setup Test Data
			TestClass cut = Isolate.Fake.Instance<TestClass>(Members.MustSpecifyReturnValues);
			#endregion

			#region Execute test
			cut.ExtensionMethod();
			#endregion

			#region Validate Results
			Isolate.Verify.WasCalledWithExactArguments(() => cut.Cursor = Cursors.WaitCursor);
			Isolate.Verify.WasCalledWithExactArguments(() => cut.Cursor = Cursors.Default);
			#endregion
		}
	}
}
