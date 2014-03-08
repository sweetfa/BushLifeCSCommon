/**
 * Copyright (C) 2012 Bush Life Pty Limited
 * 
 * All rights reserved.  No unauthorised copying or redistribution without the prior written 
 * consent of the management of Bush Life Pty Limited.
 * 
 * www.bushlife.com.au
 * sales@bushlife.com.au
 * 
 * PO Box 865, Redcliffe, QLD, 4020, Australia
 * 
 * 
 * @(#) MatrixKey.cs
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Framework.Matrix
{
	/// <summary>
	/// Key used to access a value in a two dimensional matrix
	/// </summary>
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class MatrixKey<HType, VType>
	{
		public MatrixKey(HType horizontalKey, VType verticalKey)
		{
			HorizontalKey = horizontalKey;
			VerticalKey = verticalKey;
		}

		/// <summary>
		/// The index entry for the horizontal axis of the matrix
		/// </summary>
		public HType HorizontalKey { get; set; }

		/// <summary>
		/// The index entry for the vertical axis of the matrix
		/// </summary>
		public VType VerticalKey { get; set; }

		public override bool Equals(object obj)
		{
			MatrixKey<HType, VType> rhs = obj as MatrixKey<HType, VType>;
			if (rhs == null)
				return false;

			return HorizontalKey.Equals(rhs.HorizontalKey)
				&& VerticalKey.Equals(rhs.VerticalKey);
		}

		public override int GetHashCode()
		{
			return LanguageUtils.RSHash(HorizontalKey, VerticalKey);
		}

		#region Debugger Display
		internal string DebuggerDisplay
		{
			get
			{
				return string.Format("{0} [{1}:{2}]", this.GetType().Name, HorizontalKey, VerticalKey);
			}
		}
		#endregion

	}
}
