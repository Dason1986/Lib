using System.Collections.Generic;

namespace Library.FastReflection
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class FastReflectionCache<TKey, TValue> : IFastReflectionCache<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _mCache = new Dictionary<TKey, TValue>();
        private readonly object _mRwLock = new object();

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            TValue value;

            lock (_mRwLock)
            {
                bool cacheHit = this._mCache.TryGetValue(key, out value);

                if (cacheHit) return value;

                if (!this._mCache.TryGetValue(key, out value))
                {
                    value = this.Create(key);
                    this._mCache[key] = value;
                }
            }

            return value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract TValue Create(TKey key);
    }
}