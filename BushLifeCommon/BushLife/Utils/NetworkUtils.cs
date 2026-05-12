// <copyright file="NetworkUtils.cs" company="Bush Life Pty Limited">
// Copyright (c) 2014 Bush Life Pty Limited. All rights reserved.
// </copyright>

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
