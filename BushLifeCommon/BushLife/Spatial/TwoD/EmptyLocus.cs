// <copyright file="EmptyLocus.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AU.Com.BushLife.Spatial.TwoD
{
	/// <summary>
	/// A locus containing no points of intersection
	/// </summary>
	[DebuggerDisplay("Empty Locus")]
	public class EmptyLocus<T> : Locus<T>
	{
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (!(obj is EmptyLocus<T>))
				return false;
			return true;
		}

		public override int GetHashCode()
		{
			return 0;
		}
	}
}
