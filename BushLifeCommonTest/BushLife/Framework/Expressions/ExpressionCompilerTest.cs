using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Framework.Expressions
{
	[TestFixture]
	public class ExpressionCompilerTest
	{
		private IEnumerable<object[]> DataProvider
		{
			get
			{
				#region Test 1: Func<Int32, Int32>
				yield return new object[] { typeof(Int32), typeof(Int32),  "i => (i + i) * 10 / 5 % 7", 101, 5 };
				#endregion
				#region Test 2: Func<Int32, Int32>
				yield return new object[] { typeof(Int32), typeof(Int32), "i => (i + i) * 10 / 5 % 7", 12, 6 };
				#endregion
				#region Test 3: Func<string, Int32>
				yield return new object[] { typeof(string), typeof(Int32), "i => Int32.Parse(i)", "18", 18 };
				#endregion
			}
		}

		[Test]
		[Factory("DataProvider")]
		public void ExpressionCompilerCompilationTest<Tin,Tout>(string expression, Tin funcArgument, Tout expected)
		{
			ExpressionCompiler<Func<Tin, Tout>> compiler = new ExpressionCompiler<Func<Tin, Tout>>();
			Func<Tin, Tout> func = compiler.Compile(expression);
			Assert.AreEqual(expected, func(funcArgument));
		}
	}
}
