using System;
using System.Collections.Generic;
using System.Linq;

using Disposable.Common.Extensions;

namespace Dilemma.IntegrationTest.ServiceLevel.Domains
{
    internal class ObjectDictionary
    {
        private static ObjectDictionary instance = null;
        
        public Dictionary<ObjectType, Dictionary<string, object>> Dictionary;
        
        private ObjectDictionary()
        {
            Dictionary = EnumExtensions.GetValues<ObjectType>()
                .ToDictionary(x => x, x => new Dictionary<string, object>());
        }

        public static void Reset()
        {
            instance = null;
        }

        private static ObjectDictionary Instance
        {
            get
            {
                return instance ?? (instance = new ObjectDictionary());
            }
        }

        public static T Get<T>(ObjectType objectType, string reference, Func<T> providerFunc = null)
        {
            var subDictionary = Instance.Dictionary[objectType];
            object result;

            if (!subDictionary.TryGetValue(reference, out result))
            {
                if (providerFunc == null)
                {
                    throw new InvalidOperationException(String.Format("{0} could not be found and no provider was passed", reference));
                }

                result = providerFunc.Invoke();
                subDictionary[reference] = result;
            }

            return (T)result;
        }
    }
}