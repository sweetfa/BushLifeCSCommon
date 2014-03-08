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
 * @(#) StringUtils.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Utils
{
	/// <summary>
	/// Extension methods for managing the String class
	/// </summary>
	public static class StringUtils
	{
		/// <summary>
		/// Determing if a string is contained within the source string (allowing the override of case if required)
		/// </summary>
		/// <param name="source">The source string to check the contents of</param>
		/// <param name="stringToCheck">The string to locate in the source string</param>
		/// <param name="comparison">Rules to use for the comparison</param>
		/// <returns>true if the stringToCheck is found within the source</returns>
		public static bool Contains(this string source, string stringToCheck, StringComparison comparison)
		{
			return source.IndexOf(stringToCheck, 0, comparison) >= 0;
		}
	}
}
