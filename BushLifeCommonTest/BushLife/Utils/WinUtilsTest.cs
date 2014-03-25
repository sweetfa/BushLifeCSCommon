using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

using TypeMock.ArrangeActAssert;

namespace AU.Com.BushLife.Utils
{
	[TestFixture]
	public class WinUtilsTest
	{
		[Test]
		public void GetCurrentUserNameTest()
		{
			Assert.AreEqual(WindowsIdentity.GetCurrent().Name, WinUtils.GetCurrentUserName());
		}

		[Test]
		public void GetCurrentUserNameWithoutDomainTest()
		{
			string[] splits = WindowsIdentity.GetCurrent().Name.Split('\\');
			Assert.IsTrue(splits.Length > 1);
			Assert.AreEqual(splits[splits.Length - 1], WinUtils.GetCurrentUserNameWithoutDomain());
		}
	}
}
