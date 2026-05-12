// <copyright file="ILockingMechanism.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Patterns
{
	/// <summary>
	///
	/// </summary>
	public interface ILockingMechanism : IDisposable
	{
		IDisposable Lock();
	}
}
