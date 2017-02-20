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
        private readonly Assembly[] _maptoAssemblies;

        public FindInterfaceMappToType(Type interfacType, Assembly intefaceAssembly, Assembly[] maptoAssemblies)
        {
            if (interfacType == null) throw new ArgumentNullException(nameof(interfacType));
            if (intefaceAssembly == null) throw new ArgumentNullException(nameof(intefaceAssembly));
            if (maptoAssemblies == null) throw new ArgumentNullException(nameof(maptoAssemblies));
            _interfacType = interfacType;
            _intefaceAssembly = intefaceAssembly;
            _maptoAssemblies = maptoAssemblies;
        }

        public IDictionary<Type, Type[]> Find()
        {
            IDictionary<Type, Type[]> dictionary = new Dictionary<Type, Type[]>();
            var types = _intefaceAssembly.GetTypes().Where(n =>
                   _interfacType.IsAssignableFrom(n) && n.IsInterface && n != _interfacType).OrderBy(n => n.Name)
                  .ToArray();
            foreach (var maptoAssembly in _maptoAssemblies)
            {
                var mapptos = maptoAssembly.GetTypes().Where(n =>
                  _interfacType.IsAssignableFrom(n) && n.IsClass && !n.IsAbstract && n != _interfacType)
                 .ToArray();
                foreach (var type in types)
                {
                    if (type.IsGenericType) continue;

                    var maptoType = mapptos.Where(n => type.IsAssignableFrom(n) && n.IsClass && !n.IsAbstract).ToArray();

                    dictionary.Add(type, maptoType.Length == 0 ? null : maptoType);
                }
            }

            return dictionary;
        }
    }
}