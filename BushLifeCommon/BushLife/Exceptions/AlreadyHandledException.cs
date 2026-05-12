// <copyright file="AlreadyHandledException.cs" company="Bush Life Pty Limited">
// Copyright (c) 2014 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Exceptions
{
	/// <summary>
	/// An exception thrown within an aspect to indicate that the inner exception has already been handled
	/// </summary>
	public class AlreadyHandledException : Exception
	{
		/// <summary>
		/// The type of aspect handler that handled this exception
		/// </summary>
		public Type AspectHandlerType { get; set; }

		public AlreadyHandledException(Type aspectType, Exception innerException)
			: base("", innerException)
		{
			AspectHandlerType = aspectType;
		}
	}
}
