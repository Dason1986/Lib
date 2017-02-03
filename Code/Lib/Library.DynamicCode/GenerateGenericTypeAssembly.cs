using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;

namespace Library.DynamicCode
{
    public class GenerateGenericTypeAssembly : GenerateAssembly
    {
        private readonly Type _interfaceType;
        private readonly Type _delegateType;
        private readonly Assembly _intefaceAssembly;
        private readonly Assembly _delegateAssembly;

        public GenerateGenericTypeAssembly(Type interfaceType, Type genericType, Assembly intefaceAssembly)
        {
            _interfaceType = interfaceType;
            _delegateType = genericType;
            _intefaceAssembly = intefaceAssembly;
            _delegateAssembly = genericType.Assembly;
        }

        public bool CheckArgs { get; set; }

        public override Assembly Generate()
        {
            var implTypes = _delegateAssembly.GetTypes()
                 .Where(n => _interfaceType.IsAssignableFrom(n) && n.IsClass && !n.IsAbstract)
                 .ToArray();
            var types =
                _intefaceAssembly.GetTypes()
                    .Where(n => n.GetMethods().Length == 0 && _interfaceType.IsAssignableFrom(n) && n.IsInterface && n != _interfaceType && !n.IsGenericType && !implTypes.Any(n.IsAssignableFrom))
                    .ToArray();
            CurrentAssembly = CodeDomProvider(types);
            return CurrentAssembly;
        }

        private Assembly CodeDomProvider(Type[] types)
        {
            if (!_delegateType.IsGenericType) throw new Exception();

            AddReferencedAssemblies(_delegateAssembly);
            // 命名空间
            CodeNamespace nspace = AddCodeNamespace(_delegateType.Namespace);

            foreach (var type in types)
            {
                var clsdcl = CrateClass(type);
                if (clsdcl == null) continue;
                nspace.Types.Add(clsdcl);
            }
            CompilerResults res = GenerateResults();

            return res.Errors.Count == 0 ? res.CompiledAssembly : null;
        }

        private CodeTypeDeclaration CrateClass(Type interfaceType)
        {
            var genericInterfaceType = interfaceType.GetInterfaces().FirstOrDefault(n => n.IsGenericType);
            if (genericInterfaceType == null) return null;

            var types = genericInterfaceType.GetGenericArguments();
            var newType = _delegateType.MakeGenericType(types);

            // 类声明
            var clsdcl = new GenerateClassCodeType(interfaceType.Name.Substring(1));

            clsdcl.AddBaseTypes(new CodeTypeReference(newType), new CodeTypeReference(interfaceType));
            clsdcl.CreateConstructors(_delegateType);

            return clsdcl.CodeType;
        }
    }
}