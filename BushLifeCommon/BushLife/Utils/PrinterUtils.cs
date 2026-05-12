// <copyright file="PrinterUtils.cs" company="Bush Life Pty Limited">
// Copyright (c) 2014 Bush Life Pty Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;

namespace AU.Com.BushLife.Utils
{
    /// <summary>
    /// Set of utilities for dealing with system printers
    /// </summary>
    public class PrinterUtils
    {
        /// <summary>
        /// Get the name of the default printer attached to this NOES session
        /// </summary>
        /// <returns>The default printer name</returns>
        public static string DefaultPrinterName()
        {
            return new PrinterSettings().PrinterName;
        }
    }
}
