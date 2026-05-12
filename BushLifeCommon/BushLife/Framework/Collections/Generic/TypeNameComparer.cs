// <copyright file="TypeNameComparer.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Framework.Collections.Generic
{
	/// <summary>
	/// IComparer implementation for a type using the Type.FullName string
	/// as the comparison object
	/// </summary>
	public class TypeNameComparer : IComparer<Type>
	{
		#region IComparer<Type> Members

		public int Compare(Type x, Type y)
		{
			return x.FullName.CompareTo(y.FullName);
		}

		#endregion
	}
}
