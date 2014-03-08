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
 * @(#) AuthenticationException.cs
 */

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
