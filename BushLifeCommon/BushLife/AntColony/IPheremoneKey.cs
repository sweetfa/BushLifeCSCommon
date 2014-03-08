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
 * @(#) IPheremoneKey.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.AntColony
{
	/// <summary>
	/// Pheremone key type
	/// </summary>
	public interface IPheremoneKey
	{
		/// <summary>
		/// The cost of this traversal.  Higher values are
		/// indicative of a badly considered traversal
		/// whilst preference is given to lower scored
		/// traversals.
		/// </summary>
		decimal Score { get; set; }

	}
}
