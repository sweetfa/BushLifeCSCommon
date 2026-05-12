// <copyright file="IPolymorphicClone.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Framework.Clone
{
	/// <summary>
	/// Interface to support deep cloning via a top-down clone approach
	/// </summary>
	public interface IPolymorphicClone
	{
		void CopyInto<T>(T parentObject) where T : class;
	}
}
