// <copyright file="PropertyKeyInterface.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

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
