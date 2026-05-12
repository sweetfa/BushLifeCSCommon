// <copyright file="PriorityOrderedItem.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

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
