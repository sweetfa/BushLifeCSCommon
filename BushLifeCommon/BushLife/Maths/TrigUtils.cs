// <copyright file="TrigUtils.cs" company="Bush Life Pty Limited">
// Copyright (c) 2012 Bush Life Pty Limited. All rights reserved.
// </copyright>

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
