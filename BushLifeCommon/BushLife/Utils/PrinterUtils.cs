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
 * @(#) PrinterUtils.cs
 */

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
