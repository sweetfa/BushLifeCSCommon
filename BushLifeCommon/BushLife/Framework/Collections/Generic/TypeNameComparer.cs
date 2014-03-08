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
 * @(#) TypeNameComparer.cs
 */

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
