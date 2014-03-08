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
 * @(#) MathsUtils.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Utils
{
	/// <summary>
	/// Mathematical function assistances
	/// </summary>
	public static class MathsUtils
	{
		/// <summary>
		/// Convert degrees to Radians
		/// </summary>
		/// <param name="degrees">Degrees specified in degrees</param>
		/// <returns>Radian equivalent of degree</returns>
		public static double ToRadians(this double degrees)
		{
			return degrees * Math.PI / 180;
		}

		/// <summary>
		/// Convert radians to degrees
		/// </summary>
		/// <param name="radians">Radians specified in radians</param>
		/// <returns>Degree equivalent of radian parameter</returns>
		public static double ToDegrees(this double radians)
		{
			return radians * 180 / Math.PI;
		}

		/// <summary>
		/// Convert the Radians value to degrees
		/// </summary>
		/// <param name="radians">The radian value to convert</param>
		/// <returns>The degree equivalent of the radian amount</returns>
		public static decimal ToDegrees(this decimal radians)
		{
			return radians * 180 / Convert.ToDecimal(Math.PI);
		}

		/// <summary>
		/// Convert the degree value to radians
		/// </summary>
		/// <param name="degrees">The degree value to convert</param>
		/// <returns>The radian equivalent of the degree value</returns>
		public static decimal ToRadians(this decimal degrees)
		{
			return degrees * Convert.ToDecimal(Math.PI) / 180;
		}

	}
}
