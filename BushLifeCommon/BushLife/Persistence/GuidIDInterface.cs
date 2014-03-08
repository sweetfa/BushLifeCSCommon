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
 * @(#) GuidIDInterface.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Persistence
{
	/// <summary>
	/// An interface providing identity based on a GUID
	/// </summary>
	public interface GuidIDInterface : IEquatable<GuidIDInterface>, IEqualityComparer<GuidIDInterface>, ICloneable
	{
		Guid Id { get; set; }
	}
}
