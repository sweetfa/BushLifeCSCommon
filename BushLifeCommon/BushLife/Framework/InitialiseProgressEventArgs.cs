// <copyright file="InitialiseProgressEventArgs.cs" company="Bush Life Pty Limited">
// Copyright (c) 2015 Bush Life Pty Limited. All rights reserved.
// </copyright>

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
