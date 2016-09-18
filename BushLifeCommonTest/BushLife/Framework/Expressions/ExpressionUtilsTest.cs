using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

using AU.Com.BushLife.Framework.Expressions;
using NUnit.Framework;

namespace AU.Com.BushLife.Framework.Expressions
{
	[TestFixture]
	public class ExpressionUtilsTest
	{

		[Test]
		[Ignore("Not enough work gone into function to satisfy testing yet")]
		public void GetPathTest()
		{
			Assert.AreEqual("(x) => string.Compare(x,x)", ExpressionUtils.GetPath<string, Int32>(x => string.Compare(x,x)));
			Assert.AreEqual("(x) => x.Length", ExpressionUtils.GetPath<string, Int32>(x => x.Length));
			// FIXME: Assert.AreEqual<string>("(x) => x.Length", ((Expression<Func<string, Int32>>)(x => x.Length)).GetPath<string, Int32>());
		}
	}
}
