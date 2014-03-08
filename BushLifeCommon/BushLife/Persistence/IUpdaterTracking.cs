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
 * @(#) IUpdaterTracking.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Persistence
{
	/// <summary>
	/// An interface to add to a persistent store table to 
	/// track the last update of a row in a table
	/// </summary>
	public interface IUpdaterTracking
	{
		/// <summary>
		/// The date and time the update occurred
		/// </summary>
		Nullable<DateTime> UpdatedAt { get; set; }
		/// <summary>
		/// The name of the user who performed the update
		/// </summary>
		string UpdatedBy { get; set; }
	}
}
