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
 * @(#) UpdateTrackerCollectionAttribute.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Attributes
{
	/// <summary>
	/// Attribute used to signify that the field or property contains a collection of
	/// objects that implements the IUpdaterTracker interface
	/// and requires the UpdateTrackerAspect to update the fields when inserting or updating the property
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class UpdateTrackerCollectionAttribute : Attribute
	{
	}
}
