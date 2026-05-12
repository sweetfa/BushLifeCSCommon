// <copyright file="ReflectionTypeFilters.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

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
