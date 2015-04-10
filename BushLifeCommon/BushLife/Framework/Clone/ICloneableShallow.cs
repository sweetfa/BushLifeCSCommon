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
 * @(#) ICloneableShallow.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Framework.Clone
{
    /// <summary>
    /// Provides a typesafe interface for classes that wish to implement
    /// a shallow clone interface
    /// </summary>
    public interface ICloneableShallow<T>
        where T : ICloneableShallow<T>
    {
        /// <summary>
        /// Typesafe implementation of a clone which provides a non-recursive copy
        /// of members of the class
        /// </summary>
        /// <returns>The shallow cloned item</returns>
        T CloneShallow();
    }
}
