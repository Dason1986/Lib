using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.HelperUtility
{
    /// <summary>
    ///
    /// </summary>
    public static class ProviderUtility
    {
        private static readonly FieldInfo providerCollectionReadOnlyField;

        static ProviderUtility()
        {
            Type t = typeof(ProviderCollection);
            providerCollectionReadOnlyField = t.GetField("_ReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="pc"></param>
        public static void AddTo(this ProviderBase provider, ProviderCollection pc)
        {
            bool prevValue = (bool)providerCollectionReadOnlyField.GetValue(pc);
            if (prevValue)
                providerCollectionReadOnlyField.SetValue(pc, false);

            pc.Add(provider);

            if (prevValue)
                providerCollectionReadOnlyField.SetValue(pc, true);
        }
    }
}