// <copyright file="AuthenticationException.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Authentication
{
	/// <summary>
	/// Exception class for any authentication exception
	/// </summary>
	public class AuthenticationException : Exception
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public AuthenticationException()
			: base()
		{
		}

		/// <summary>
		/// Constructor containing a reason for the failure
		/// </summary>
		/// <param name="Message">The message indicating the reason for the failure</param>
		public AuthenticationException(string Message)
			: base(Message)
		{
		}
	}
}
