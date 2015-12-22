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
 * @(#) NetworkUtils.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace AU.Com.BushLife.Utils
{
    /// <summary>
    /// Utilities associated with network interfaces
    /// </summary>
    public static class NetworkUtils
    {
        /// <summary>
        /// Get the IP Address of the current machine
        /// </summary>
        /// <returns></returns>
        public static IPAddress Ip4Address()
        {
            return Dns
               .GetHostEntry(Dns.GetHostName())
               .AddressList
               .FirstOrDefault(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }
    }
}
