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
 * @(#) ReflectionTypeFilters.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AU.Com.BushLife.Filters
{
	/// <summary>
	///
	/// </summary>
	public class ReflectionTypeFilters
	{
		public static bool TypeNamespaceFilter(Type theType, object criteria)
		{
			return theType.Namespace.Equals(criteria);
		}
	}
}
