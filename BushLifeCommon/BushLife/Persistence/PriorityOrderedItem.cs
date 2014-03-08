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
 * @(#) PriorityOrderedItem.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Persistence
{
	/// <summary>
	/// A class for priority ordered items
	/// </summary>
	public abstract class PriorityOrderedItem
	{
		/// <summary>
		/// The priority value.  The ordering of the priority (ie. the value) will
		/// be dependant on the implementer of the derived class
		/// </summary>
		public int Priority { get; set; }
	}
}
