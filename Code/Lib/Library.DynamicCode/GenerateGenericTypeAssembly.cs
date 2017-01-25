using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;

namespace Library.DynamicCode
{

    public class GenerateGenericTypeAssembly : GenerateAssembly
    {
        private readonly Type _interfacType;
        private readonly Type _delegateType;
        private readonly Assembly _intefaceAssembly;
        private readonly Assembly _delegateAssembly;

        public GenerateGenericTypeAssembly(Type interfacType, Type genericType, Assembly intefaceAssembly)
        {
            _interfacType = interfacType;
            _delegateType = genericType;
            _intefaceAssembly = intefaceAssembly;
            _delegateAssembly = genericType.Assembly;
        }

        public bool CheckArgs { get; set; }

        public override Assembly Generate()
        {
            var implTypes = _delegateAssembly.GetTypes()
                 .Where(n => _interfacType.IsAssignableFrom(n) && n.IsClass && !n.IsAbstract)
                 .ToArray();
            var types =
                _intefaceAssembly.GetTypes()
                    .Where(n => n.GetMethods().Length == 0 && _interfacType.IsAssignableFrom(n) && n.IsInterface && n != _interfacType && !n.IsGenericType && !implTypes.Any(n.IsAssignableFrom))
                    .ToArray();
            currentAssembly = CodeDomProvider(types);
            return currentAssembly;
        }

        protected Assembly currentAssembly { get; set; }

        private Assembly CodeDomProvider(Type[] types)
        {
            if (!_delegateType.IsGenericType) return null;

            AssemlyAttribute(typeof(AssemblyFileVersionAttribute), AssemblyVersion.ToString());
            AssemlyAttribute(typeof(AssemblyVersionAttribute), AssemblyVersion.ToString());
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
            CodeTypeDeclaration clsdcl = new CodeTypeDeclaration
            {
                IsClass = true,
                Name = interfaceType.Name.Substring(1)
            };
            clsdcl.BaseTypes.Add(new CodeTypeReference(newType));
            clsdcl.BaseTypes.Add(new CodeTypeReference(interfaceType));

            foreach (var constructor in _delegateType.GetConstructors())
            {
                CodeConstructor codeConstructor = new CodeConstructor() { Attributes = MemberAttributes.Public };

                clsdcl.Members.Add(codeConstructor);
                foreach (var parameter in constructor.GetParameters())
                {
                    codeConstructor.Parameters.Add(new CodeParameterDeclarationExpression(parameter.ParameterType,
                        parameter.Name));

                    codeConstructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression(parameter.Name));
                    if (!CheckArgs || !parameter.ParameterType.IsClass) continue;

                    CodeConditionStatement codst = new CodeConditionStatement();
                    CodeExpression rigth;
                    //if (parameter.ParameterType.IsValueType)
                    //{
                    //    rigth = new CodePrimitiveExpression(Activator.CreateInstance(parameter.ParameterType));
                    //}
                    //else
                    {
                        rigth = new CodePrimitiveExpression(null);
                    }

                    // 设置判断条件
                    codst.Condition = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression(parameter.Name),
                        CodeBinaryOperatorType.ValueEquality, rigth);

                    CodeThrowExceptionStatement ts =
                        new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(ArgumentNullException),
                            new CodePrimitiveExpression(parameter.Name)));
                    codst.TrueStatements.Add(ts);
                    codeConstructor.Statements.Add(codst);
                }
            }
            return clsdcl;
        }

        public T GetService<T>(params object[] args)
        {
            var basetype = typeof(T);
            var type =
                currentAssembly.GetTypes()
                    .FirstOrDefault(n => basetype.IsAssignableFrom(n) && n.IsClass && !n.IsAbstract);
            if (type == null) throw new Exception();
            ConstructorInfo constructor;
            if (args == null) constructor = type.GetConstructors().First(n => n.GetParameters().Length == 0);
            else
            {
                constructor = type.GetConstructors().First(n =>
                {
                    var parm = n.GetParameters();
                    if (parm.Length != args.Length) return false;
                    for (int i = 0; i < parm.Length; i++)
                    {
                        var ptype = parm[i];
                        var arg = args[i];
                        if (ptype.ParameterType.IsValueType && arg == null)
                        {
                            return false;
                        }
                        if (ptype.ParameterType.IsClass)
                        {
                            if (arg != null && !ptype.ParameterType.IsInstanceOfType(arg)) return false;
                        }
                    }

                    return true;
                }
                );
            }
            var obj = constructor.Invoke(args);
            return (T)obj;
        }
    }
}