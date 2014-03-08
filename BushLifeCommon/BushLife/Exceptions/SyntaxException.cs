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
 * @(#) SyntaxException.cs
 */

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
