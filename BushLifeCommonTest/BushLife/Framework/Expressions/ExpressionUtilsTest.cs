using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;
using System.Linq.Expressions;

using AU.Com.BushLife.Framework.Expressions;

namespace AU.Com.BushLife.Framework.Expressions
{
	[TestFixture]
	public class ExpressionUtilsTest
	{

		[Test]
		[Disable("Not enough work gone into function to satisfy testing yet")]
		public void GetPathTest()
		{
			Assert.AreEqual("(x) => string.Compare(x,x)", ExpressionUtils.GetPath<string, Int32>(x => string.Compare(x,x)));
			Assert.AreEqual("(x) => x.Length", ExpressionUtils.GetPath<string, Int32>(x => x.Length));
			Assert.AreEqual<string>("(x) => x.Length", ((Expression<Func<string, Int32>>)(x => x.Length)).GetPath<string, Int32>());
		}
	}
}
