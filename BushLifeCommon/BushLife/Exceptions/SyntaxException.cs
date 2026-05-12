// <copyright file="SyntaxException.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Exceptions
{
	/// <summary>
	/// Exception thrown when a syntax error is present in a parsed expression
	/// </summary>
	public class SyntaxException : FormatException
	{
		public SyntaxException(string message)
			: base(message)
		{
		}
	}
}
