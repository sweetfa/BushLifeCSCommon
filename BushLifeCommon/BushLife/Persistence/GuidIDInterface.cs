// <copyright file="GuidIDInterface.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

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
