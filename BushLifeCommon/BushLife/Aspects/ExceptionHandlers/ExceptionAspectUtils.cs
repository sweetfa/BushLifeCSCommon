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
 * @(#) ExceptionAspectUtils.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Exceptions;

namespace AU.Com.BushLife.Aspects.ExceptionHandlers
{
	/// <summary>
	/// A set of common utilities used by exception aspects
	/// </summary>
	public static class ExceptionAspectUtils
	{
		/// <summary>
		/// Determine if this exception has already been handled by an instance of this aspect
		/// <para>For it to be true the exception type must be AlreadyHandledException and the
		/// aspect handler type of the exception must be this aspect</para>
		/// </summary>
		/// <param name="exception">The exception to check</param>
		/// <param name="aspectHandlerType">The type of the aspect calling this method</param>
		/// <returns>true if this exception has already been handled, false otherwise</returns>
		internal static bool IsAlreadyHandled(this Exception exception, Type aspectHandlerType)
		{
			AlreadyHandledException ex = exception as AlreadyHandledException;
			if (ex != null)
			{
				if (ex.AspectHandlerType.Equals(aspectHandlerType))
					return true;
			}
			return false;
		}
	}
}
