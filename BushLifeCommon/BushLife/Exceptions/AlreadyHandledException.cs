/**
 * Copyright (C) 2014 Bush Life Pty Limited
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
 * @(#) AlreadyHandledException.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Exceptions
{
	/// <summary>
	/// An exception thrown within an aspect to indicate that the inner exception has already been handled
	/// </summary>
	public class AlreadyHandledException : Exception
	{
		/// <summary>
		/// The type of aspect handler that handled this exception
		/// </summary>
		public Type AspectHandlerType { get; set; }

		public AlreadyHandledException(Type aspectType, Exception innerException)
			: base("", innerException)
		{
			AspectHandlerType = aspectType;
		}
	}
}
