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
 * @(#) ExpressionUtils.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Framework.Expressions
{
	/// <summary>
	/// Class of useful utilities for using Expressions
	/// </summary>
	public static class ExpressionUtils
	{
		/// <summary>
		/// Convert the expression into a string representation of the expression
		/// <para>http://stackoverflow.com/questions/3049825/given-a-member-access-lambda-expression-convert-it-to-a-specific-string-represe</para>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="expr">The expression to extract the string representation of</param>
		/// <returns>The string representation of a function</returns>
		[Obsolete("Needs more work to correctly return a string representation of an expression")]
		public static string GetPath<T,TProperty>(this Expression<Func<T, TProperty>> expr)
		{
			var stack = new Stack<string>();

			MemberExpression me = null;
			switch (expr.Body.NodeType)
			{
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
					var ue = expr.Body as UnaryExpression;
					me = ((ue != null) ? ue.Operand : null) as MemberExpression;
					break;
				case ExpressionType.Call:
					var argList = new List<string>();
					var ce = expr.Body as MethodCallExpression;
					foreach (var arg in ce.Arguments)
					{
						argList.Add(arg.ToString());
					}
					stack.Push(string.Format("{0}({1})", ce.Method.Name, string.Join(",", argList.ToArray())));
					break;
				default:
					me = expr.Body as MemberExpression;
					break;
			}

			while (me != null)
			{
				stack.Push(me.Member.Name);
				me = me.Expression as MemberExpression;
			}

			stack.Push(string.Format("({0}) => {1}.", string.Join(",", expr.Parameters.SelectMany(x => x.Name)), expr.Parameters[0].Name));
			return string.Join(".", stack.ToArray());
		}
	}
}
