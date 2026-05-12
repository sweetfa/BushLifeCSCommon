// <copyright file="ComboBoxLoader.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AU.Com.BushLife.Utils
{
	/// <summary>
	/// Class to provide assistance for separation of concern
	/// over contents of combo box where the
	/// displayed value does not match the ToString 
	/// support.
	/// </summary>
	/// <typeparam name="T">The type of the value the combo box supports</typeparam>
	[DebuggerDisplay("ComboBoxLoader {Display} {Value.ToString()}")]
	public class ComboBoxLoader<T>
	{
		/// <summary>
		/// The value to display in the combo box
		/// </summary>
		public string Display { get; set; }

		/// <summary>
		/// The actual object associated with the combo box item
		/// </summary>
		public T Value { get; set; }
	}
}
