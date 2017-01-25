using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.DynamicCode
{
    public abstract class GenerateAssembly
    {
        protected GenerateAssembly()
        {
            AssemblyVersion = new Version(1, 0);
            compilerParameters.GenerateExecutable = false;
            compilerParameters.TreatWarningsAsErrors = false;
        }

        public Version AssemblyVersion { get; set; }
        public string CodeText { get; private set; }
        public string[] Errors { get; private set; }
        protected CodeCompileUnit CodeUnit { get { return unit; } }
        private readonly CodeDomProvider provider = CodeDomProvider.CreateProvider("cs");
        private readonly CompilerParameters compilerParameters = new CompilerParameters();

        private readonly CodeCompileUnit unit = new CodeCompileUnit();

        protected virtual CodeAttributeDeclaration AssemlyAttribute(Type attributeType, object value)
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
                compilerParameters.ReferencedAssemblies.Add(path);
                unit.ReferencedAssemblies.Add(path);
            }
            compilerParameters.ReferencedAssemblies.Add(assembly.Location);
            unit.ReferencedAssemblies.Add(assembly.Location);
        }

        protected CompilerResults GenerateResults()
        {
            var res = provider.CompileAssemblyFromDom(compilerParameters, unit);
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            provider.GenerateCodeFromCompileUnit(unit, writer, null);
            CodeText = builder.ToString();
            if (res.Errors.HasErrors)
            {
                Errors = res.Errors.OfType<CompilerError>().Select(n => n.ErrorText).ToArray();
            }
            return res;
        }

        public abstract Assembly Generate();
    }
}