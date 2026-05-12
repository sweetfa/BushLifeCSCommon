// <copyright file="UpdateTrackerCollectionAttribute.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

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
