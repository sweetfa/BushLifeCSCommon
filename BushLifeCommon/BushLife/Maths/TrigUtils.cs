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
 * @(#) TrigUtils.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Maths
{
	/// <summary>
	///
	/// </summary>
	public static class TrigUtils
	{
		public static double HypotenuseLengthWithAdjacent(double degrees, Int32 adjacentLength)
		{
			return adjacentLength / Math.Cos(degrees);
		}

		public static double HypotenuseLengthWithOpposite(double degrees, Int32 oppositeLength)
		{
			return oppositeLength / Math.Sin(degrees);
		}
	}
}
