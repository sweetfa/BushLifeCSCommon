using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;

namespace AU.Com.BushLife.Aspects.ExceptionHandlers
{
	[TestFixture]
	public class InvalidOperationExceptionAspectTest
	{
		[Test]
		public void InvalidAspectExceptionRequiredTest()
		{
			Assert.Throws<IndexOutOfRangeException>(() => AspectMethod1());
		}

        [InvalidOperationExceptionAspect(ThrowExceptionType = typeof(System.IndexOutOfRangeException), Message = "My Exception generated")]
        private void AspectMethod1()
        {
            IList<Int32> items = new List<Int32>() { 1, 2, 3, 4, 5 };
            items.Where(i => i > 10).First();
        }

        [Test]
		[InvalidOperationExceptionAspect(ThrowExceptionType = typeof(System.IndexOutOfRangeException), Message = "Exception generated")]
		public void InvalidAspectExceptionNotRequiredTest()
		{
			IList<Int32> items = new List<Int32>() { 1, 2, 3, 4, 5 };
			Int32 x = items.Where(i => i == 3).First();
		}

		[Test]
		public void InvalidAspectExceptionControlTest()
		{
			IList<Int32> items = new List<Int32>() { 1, 2, 3, 4, 5 };
			Assert.Throws<InvalidOperationException>(() => items.Where(i => i > 10).First());
		}
	}
}
