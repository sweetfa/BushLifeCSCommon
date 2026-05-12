// <copyright file="TransientAttribute.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Attributes
{
	/// <summary>
	/// Attribute used to signify that a field or property is transient
	/// and should not be persisted or serialized
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class TransientAttribute : Attribute
	{
	}
}
