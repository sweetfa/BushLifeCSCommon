// <copyright file="IUpdaterTracking.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

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
