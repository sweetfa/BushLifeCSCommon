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
 * @(#) PropertyKeyInterface.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Persistence
{
	/// <summary>
	/// Interface to implement for persistent classes using class properties as keys
	/// </summary>
	public interface PropertyKeyInterface<T> : IEqualityComparer<T>, ICloneable
		where T : class
	{
	}
}
