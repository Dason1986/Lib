using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Library.DynamicCode
{
    public static class GenerateHelper
    {
        #region Cast

        public static CodeCastExpression CastVariable(string variableName, Type casType)
        {
            CodeVariableReferenceExpression vexp = new CodeVariableReferenceExpression();
            vexp.VariableName = variableName;
            CodeTypeReference tref = new CodeTypeReference(casType);
            CodeCastExpression cexp = new CodeCastExpression();
            cexp.Expression = vexp;
            cexp.TargetType = tref;
            return cexp;
        }

        public static CodeCastExpression CastField(string variableName, Type casType)
        {
            CodeFieldReferenceExpression vexp = new CodeFieldReferenceExpression();
            vexp.FieldName = variableName;
            CodeTypeReference tref = new CodeTypeReference(casType);
            CodeCastExpression cexp = new CodeCastExpression();
            cexp.Expression = vexp;
            cexp.TargetType = tref;
            return cexp;
        }

        public static CodeCastExpression CastProperty(string variableName, Type casType)
        {
            CodePropertyReferenceExpression vexp = new CodePropertyReferenceExpression();
            vexp.PropertyName = variableName;

            CodeCastExpression cexp = new CodeCastExpression(casType, vexp);

            return cexp;
        }

        #endregion Cast
    }

    public abstract class GenerateAssembly
    {
        protected GenerateAssembly()
        {
            AssemblyVersion = new Version(1, 0);
            compilerParameters.GenerateExecutable = false;
            compilerParameters.TreatWarningsAsErrors = false;
            ID = Guid.NewGuid();
            Copyright = "Copyright Dason©  2015";
            Title = "Generate Assembly";
            RefAssemblies = new List<Assembly>();
        }

        protected List<Assembly> RefAssemblies { get; set; }
        protected Assembly CurrentAssembly { get; set; }
        public Version AssemblyVersion { get; set; }
        public string CodeText { get; protected set; }
        public string Copyright { get; set; }
        public string Title { get; set; }
        public Guid ID { get; set; }
        private readonly CodeDomProvider provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("cs");
        private readonly CompilerParameters compilerParameters = new CompilerParameters();

        private readonly CodeCompileUnit unit = new CodeCompileUnit();

        protected CodeAttributeDeclaration AssemlyAttribute(Type attributeType, object value)
        {
            var attribute = new CodeAttributeDeclaration(new CodeTypeReference(attributeType));
            attribute.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(value)));
            unit.AssemblyCustomAttributes.Add(attribute);
            return attribute;
        }

        protected CodeNamespace AddCodeNamespace(string nameSpace)
        {
            CodeNamespace nspace = new CodeNamespace(nameSpace);

            unit.Namespaces.Add(nspace);
            return nspace;
        }

        protected void AddReferencedAssemblies(Assembly assembly)
        {
            foreach (var tmpassembly in assembly.GetReferencedAssemblies())
            {
                var path = Assembly.Load(tmpassembly).Location;
                if (compilerParameters.ReferencedAssemblies.Contains(path)) continue;
                compilerParameters.ReferencedAssemblies.Add(path);
                unit.ReferencedAssemblies.Add(path);
                if (RefAssemblies.Any(n => n.Location == assembly.Location)) continue;
                RefAssemblies.Add(assembly);
            }

            if (!compilerParameters.ReferencedAssemblies.Contains(assembly.Location))
            {
                compilerParameters.ReferencedAssemblies.Add(assembly.Location);
                unit.ReferencedAssemblies.Add(assembly.Location);
            }

            if (RefAssemblies.All(n => n.Location != assembly.Location))
                RefAssemblies.Add(assembly);
        }

        protected CompilerResults GenerateResults()
        {
            AssemlyAttribute(typeof(AssemblyFileVersionAttribute), AssemblyVersion.ToString());
            AssemlyAttribute(typeof(AssemblyVersionAttribute), AssemblyVersion.ToString());
            AssemlyAttribute(typeof(GuidAttribute), ID.ToString());
            AssemlyAttribute(typeof(AssemblyCopyrightAttribute), Copyright);
            AssemlyAttribute(typeof(AssemblyTitleAttribute), Title);
            var res = provider.CompileAssemblyFromDom(compilerParameters, unit);
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            provider.GenerateCodeFromCompileUnit(unit, writer, null);
            CodeText = builder.ToString();
            return res;
        }

        public abstract Assembly Generate();

        public T GetService<T>(params object[] args)
        {
            var basetype = typeof(T);
            var type = RefAssemblies.SelectMany(n => n.GetTypes()).FirstOrDefault(n => basetype.IsAssignableFrom(n) && n.IsClass && !n.IsAbstract) ??
                            CurrentAssembly.GetTypes().FirstOrDefault(n => basetype.IsAssignableFrom(n) && n.IsClass && !n.IsAbstract);

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