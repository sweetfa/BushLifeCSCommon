/**
 * Copyright (C) 2015 Bush Life Pty Limited
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
 * @(#) InitialiseProgressEventArgs.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Framework
{
    /// <summary>
    /// Event arguments for an initialisation of a progress bar as an event
    /// </summary>
    public class InitialiseProgressEventArgs
    {
        /// <summary>
        /// The number of steps on the progress bar
        /// </summary>
        public int ElementCount { get; set; }
    }
}
