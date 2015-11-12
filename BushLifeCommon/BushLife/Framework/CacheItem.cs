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
 * @(#) CacheItem.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NOES.Common
{
    /// <summary>
    /// Provides a simple cache for an item that expires after a set period
    /// </summary>
    public class CacheItem<T>
    {
        /// <summary>
        /// The number of seconds that the cached object can live for
        /// </summary>
        private int TimeToLive { get; set; }
        /// <summary>
        /// When the cache item was last populated
        /// </summary>
        private DateTime WhenCachePopulated { get; set; }
        /// <summary>
        /// An action to refresh the cached item after expiry
        /// </summary>
        private Func<T> LoadDelegate { get; set; }

        /// <summary>
        /// Constructor to create the inital cache entry
        /// </summary>
        /// <param name="ttlSeconds">The number of seconds that the cache item can be valid for</param>
        /// <param name="loadAction">An action to perform to refresh the cache</param>
        public CacheItem(int ttlSeconds, Func<T> loadAction)
        {
            this.TimeToLive = ttlSeconds;
            this.WhenCachePopulated = DateTime.Now.AddSeconds(0 - TimeToLive);
            this.LoadDelegate = loadAction;
        }

        /// <summary>
        /// The item held in the cache
        /// </summary>
        public T Item
        {
            get
            {
                if (WhenCachePopulated.AddSeconds(TimeToLive) <= DateTime.Now)
                {
                    Item = LoadDelegate();
                }
                return mItem;
            }
            set
            {
                mItem = value;
                this.WhenCachePopulated = DateTime.Now;
            }
        }
        private T mItem = default(T);
    }
}
