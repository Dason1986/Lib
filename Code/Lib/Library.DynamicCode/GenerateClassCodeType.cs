using System;
using System.CodeDom;
using System.Reflection;

namespace Library.DynamicCode
{
    internal class GenerateClassCodeType
    {
        public GenerateClassCodeType(string className)
        {
            if (string.IsNullOrEmpty(className))
                throw new ArgumentException("Value cannot be null or empty.", nameof(className));
            CodeType = new CodeTypeDeclaration(className)
            {
                IsClass = true
            };
        }

        public CodeTypeDeclaration CodeType { get; private set; }
        public bool CheckArgs { get; set; }

        public void CreateConstructors(Type delegateType)
        {
            var flg = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;


            foreach (var constructor in delegateType.GetConstructors(flg))
            {
                CodeConstructor codeConstructor = new CodeConstructor() { Attributes = MemberAttributes.Public };

                CodeType.Members.Add(codeConstructor);
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
        }

        public void AddBaseTypes(params CodeTypeReference[] codeTypeReference)
        {
            if (codeTypeReference == null) throw new ArgumentNullException(nameof(codeTypeReference));
            CodeType.BaseTypes.AddRange(codeTypeReference);
        }
    }
}