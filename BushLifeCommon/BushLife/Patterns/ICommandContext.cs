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
 * @(#) ICommandContext.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AU.Com.BushLife.Patterns
{
	/// <summary>
	/// An interface to a context to provide additional information as required
	/// </summary>
	public interface ICommandContext
	{
		/// <summary>
		/// The current processing step being exectuted (i.e. the command pattern class type)
		/// </summary>
		Type CurrentProcessingStep { get; set; }
	}
}
