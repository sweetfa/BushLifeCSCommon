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
 * @(#) PropertyKey.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Persistence
{
	/// <summary>
	/// Base class for persistent classes using a property field as the key
	/// </summary>
	public abstract class PropertyKey<T> : PropertyKeyInterface<T> 
		where T : class
	{
		// These two methods need to be overridden to support NHibernate properly
		public override abstract int GetHashCode();
		public override abstract bool Equals(object obj);

		#region IEqualityComparer<T> Members

		public virtual bool Equals(T x, T y)
		{
			return this.Equals(y);
		}

		public virtual int GetHashCode(T obj)
		{
			return obj.GetHashCode();
		}

		#endregion

		#region ICloneable Members
		public abstract object Clone();
		#endregion
	}
}
