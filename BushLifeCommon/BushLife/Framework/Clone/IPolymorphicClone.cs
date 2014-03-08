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
 * @(#) IPolymorphicClone.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Framework.Clone
{
	/// <summary>
	/// Interface to support deep cloning via a top-down clone approach
	/// </summary>
	public interface IPolymorphicClone
	{
		void CopyInto<T>(T parentObject) where T : class;
	}
}
