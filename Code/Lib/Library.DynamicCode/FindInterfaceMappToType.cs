using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Library.DynamicCode
{
    public class FindInterfaceMappToType
    {
        private readonly Type _interfacType;
        private readonly Assembly _intefaceAssembly;
        private readonly Assembly _maptoAssembly;

        public FindInterfaceMappToType(Type interfacType, Assembly intefaceAssembly, Assembly maptoAssembly)
        {
            _interfacType = interfacType;
            _intefaceAssembly = intefaceAssembly;
            _maptoAssembly = maptoAssembly;
        }

        public IDictionary<Type, Type[]> Find()
        {
            IDictionary<Type, Type[]> dictionary = new Dictionary<Type, Type[]>();
            var types =
              _intefaceAssembly.GetTypes()
                  .Where(n => _interfacType.IsAssignableFrom(n) && n.IsInterface && n != _interfacType)
                  .ToArray();
            var mapptos =
                _maptoAssembly.GetTypes()
                    .Where(
                        n => _interfacType.IsAssignableFrom(n) && n.IsClass && !n.IsAbstract && n != _interfacType)
                    .ToArray();
            foreach (var type in types)
            {
                if (type.IsGenericType) continue;

                var maptoType = mapptos.Where(n => type.IsAssignableFrom(n) && n.IsClass && !n.IsAbstract).ToArray();

                dictionary.Add(type, maptoType.Length == 0 ? null : maptoType);
            }
            return dictionary;
        }
    }
}