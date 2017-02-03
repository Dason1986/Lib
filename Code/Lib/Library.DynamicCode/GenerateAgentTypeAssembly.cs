using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;

namespace Library.DynamicCode
{
    public class GenerateAgentTypeAssembly : GenerateAssembly
    {
        public GenerateAgentTypeAssembly(Type interfacetype, Type genericType, Assembly intefaceAssembly, params Assembly[] refAssemblies)
        {
            _interfaceType = interfacetype;
            _refAssemblies = refAssemblies;
            _delegateType = genericType;
            _intefaceAssembly = intefaceAssembly;
            _delegateAssembly = genericType.Assembly;
        }

        protected Assembly currentAssembly { get; set; }
        private readonly Type _interfaceType;
        private readonly Type _delegateType;
        private readonly Assembly _intefaceAssembly;
        private readonly Assembly _delegateAssembly;
        private readonly Assembly[] _refAssemblies;

        public override Assembly Generate()
        {
            var types =
                _intefaceAssembly.GetTypes()
                    .Where(n => _interfaceType.IsAssignableFrom(n) && n.IsInterface && n != _interfaceType)
                    .ToArray();
            currentAssembly = CodeDomProvider(types);
            return currentAssembly;
        }

        private Assembly CodeDomProvider(Type[] types)
        {
            AddReferencedAssemblies(_delegateAssembly);
            foreach (var item in _refAssemblies)
            {
                AddReferencedAssemblies(item);
            }
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
            if (!_delegateType.IsClass) throw new Exception();
            var exportProperties =
                interfaceType.GetInterfaces()
                    .SelectMany(n => n.GetProperties())
                    .Where(n => n.GetCustomAttributes(typeof(ExportAttribute), true).Length > 0).ToArray();

            // 类声明

            var clsdcl = new GenerateClassCodeType(interfaceType.Name.Substring(1));

            clsdcl.AddBaseTypes(new CodeTypeReference(_delegateType), new CodeTypeReference(interfaceType));
            clsdcl.CreateConstructors(_delegateType);
            foreach (var method in interfaceType.GetMethods())
            {
                CreateMethod(method, clsdcl.CodeType, exportProperties);
            }
            return clsdcl.CodeType;
        }

        private Type FindCrateClass(Type interfaceType)
        {
            var types = _refAssemblies.SelectMany(n => n.GetTypes())
                 .Where(n => interfaceType.IsAssignableFrom(n) && n.IsClass && !n.IsAbstract)
               ;
            return types.FirstOrDefault();
        }

        private void CreateMethod(MethodInfo method, CodeTypeDeclaration clsdcl, PropertyInfo[] exportProperties)
        {
            var classType = FindCrateClass(method.ReturnType);
            if (classType == null) return;
            CodeMemberMethod pry = new CodeMemberMethod();
            pry.Name = method.Name;
            pry.Attributes = MemberAttributes.Public;
            pry.ReturnType = new CodeTypeReference(method.ReturnType);
            clsdcl.Members.Add(pry);
            var constructor = classType.GetConstructors().FirstOrDefault();
            var constructorParameters = constructor.GetParameters();
            CodeExpression[] paramsExpressions = new CodeExpression[constructorParameters.Length];
            for (int i = 0; i < constructorParameters.Length; i++)
            {
                var parmtype = constructorParameters[i];
                var exoprtProterty = exportProperties.FirstOrDefault(n => n.PropertyType.IsAssignableFrom(parmtype.ParameterType));
                if (exoprtProterty == null)
                {
                    paramsExpressions[i] = new CodePrimitiveExpression(null);
                }
                else
                {
                    var castParam = GenerateHelper.CastProperty(exoprtProterty.Name, parmtype.ParameterType);
                    paramsExpressions[i] = castParam;
                }
            }
            var objCreateExpression = new CodeObjectCreateExpression(classType, paramsExpressions.Length == 0 ? null : paramsExpressions);
            var objname = "_" + method.Name;

            CodeVariableDeclarationStatement variableDeclaration = new CodeVariableDeclarationStatement(
    method.ReturnType,
   objname,
    objCreateExpression);
            pry.Statements.Add(variableDeclaration);
            var methodReturn = new CodeMethodReturnStatement(new CodeVariableReferenceExpression(objname));
            pry.Statements.Add(methodReturn);
        }
    }
}