// <copyright file="ICommandContext.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Patterns
{
	/// <summary>
	/// An interface to a context to provide additional information as required
	/// </summary>
	public interface ICommandContext
	{
		/// <summary>
		/// The current processing step being exectuted (i.e. the command pattern class type)
		/// </summary>
		Type CurrentProcessingStep { get; set; }
	}
}
