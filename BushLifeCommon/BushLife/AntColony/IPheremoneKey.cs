// <copyright file="IPheremoneKey.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

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
