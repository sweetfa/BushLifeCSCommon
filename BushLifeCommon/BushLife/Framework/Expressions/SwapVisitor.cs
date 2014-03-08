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
 * 
 * @(#) SwapVisitor.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AU.Com.BushLife.Framework.Expressions
{
	/// <summary>
	/// A class to assist with swapping parameters between two expressions
	/// <para>See http://stackoverflow.com/questions/13513134/combining-two-linq-expressions </para>
	/// <para>    static void Main() {
	///        Expression<Func<Source, IEnumerable<Value>>> f1 =
	///            source => source.Values;
	///        Expression<Func<IEnumerable<Value>, bool>> f2 =
	///            vals => vals.Any(v => v.FinalValue == 3);
	///           
	///        // change the parameter 0 from f2 => f1
	///        var body = SwapVisitor.Swap(f2.Body, f2.Parameters[0], f1.Body);
	///        var lambda = Expression.Lambda<Func<Source, bool>>(body,f1.Parameters);
	///        // which results in:
	///        // source => source.Values.Any(v => (v.FinalValue == 3))
	///    }
	/// </para>
	/// </summary>
	public class SwapVisitor : ExpressionVisitor
	{
		private Expression From;
		private Expression To;

		public static Expression Swap(Expression body, Expression from, Expression to)
		{
			return new SwapVisitor(from, to).Visit(body);
		}

		private SwapVisitor(Expression from, Expression to)
		{
			this.From = from;
			this.To = to;
		}

		public override Expression Visit(Expression node)
		{
			return node == From
				? To
				: base.Visit(node);
		}
	}
}
