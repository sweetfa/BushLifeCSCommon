/**
 * Copyright (C) 2014 Bush Life Pty Limited
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
 * @(#) DateExtensions.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Utils
{
    /// <summary>
    /// Extension methods for playing with dates
    /// </summary>
    public static class DateExtensions
    {
        /// <summary>
        /// Calculate the last day of the next month relative to the supplied parameter
        /// </summary>
        /// <param name="now">The point from whence to get the next month from</param>
        /// <returns>The last day of the next month relative to the now paramter</returns>
        public static DateTime LastDayOfNextMonth(this DateTime now)
        {
            return new DateTime(now.Year, now.Month, 1).AddMonths(2).AddDays(-1);
        }

        /// <summary>
        /// Calculate the last day of the the month relative to the supplied parameter
        /// </summary>
        /// <param name="now">The point from whence to get the month from</param>
        /// <returns>The last day of the month relative to the now paramter</returns>
        public static DateTime LastDayOfMonth(this DateTime now)
        {
            return new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        }
    }
}
