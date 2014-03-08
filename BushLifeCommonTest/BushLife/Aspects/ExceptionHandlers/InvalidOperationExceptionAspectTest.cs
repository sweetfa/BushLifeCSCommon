using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace AU.Com.BushLife.Aspects.ExceptionHandlers
{
	[TestFixture]
	public class InvalidOperationExceptionAspectTest
	{
		[Test]
		[ExpectedException(typeof(System.IndexOutOfRangeException),Message= "My Exception generated")]
		[InvalidOperationExceptionAspect(ThrowExceptionType=typeof(System.IndexOutOfRangeException),Message="My Exception generated")]
		public void InvalidAspectExceptionRequiredTest()
		{
			IList<Int32> items = new List<Int32>() { 1, 2, 3, 4, 5 };
			Int32 x = items.Where(i => i > 10).First();
			Assert.Fail("Should have thrown exception");
		}

		[Test]
		[InvalidOperationExceptionAspect(ThrowExceptionType = typeof(System.IndexOutOfRangeException), Message = "Exception generated")]
		public void InvalidAspectExceptionNotRequiredTest()
		{
			IList<Int32> items = new List<Int32>() { 1, 2, 3, 4, 5 };
			Int32 x = items.Where(i => i == 3).First();
		}

		[Test]
		[ExpectedInvalidOperationException]
		public void InvalidAspectExceptionControlTest()
		{
			IList<Int32> items = new List<Int32>() { 1, 2, 3, 4, 5 };
			Int32 x = items.Where(i => i > 10).First();
		}
	}
}
