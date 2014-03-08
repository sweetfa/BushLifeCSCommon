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
 * @(#) EmptyLocus.cs
 */

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
