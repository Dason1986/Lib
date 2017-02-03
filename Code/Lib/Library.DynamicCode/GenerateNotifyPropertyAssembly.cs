using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Library.DynamicCode
{
    public class GenerateNotifyPropertyAssembly : GenerateAssembly
    {
        public GenerateNotifyPropertyAssembly(string nameSpace)
        {
            Namespace = AddCodeNamespace(nameSpace);
            AddReferencedAssemblies(typeof(System.Type).Assembly);
            AddReferencedAssemblies(typeof(INotifyPropertyChanged).Assembly);
        }

        private readonly CodeNamespace Namespace;

        public override Assembly Generate()
        {
            CompilerResults res = GenerateResults();

            return res.Errors.Count == 0 ? res.CompiledAssembly : null;
        }

        private class GenerateNotifyPropertyEntity
        {
            public GenerateNotifyPropertyEntity(string className)
            {
                newType = new CodeTypeDeclaration(className);
                CreateClassType();

                PropertyChangedEvent();
                OnPropertyChanged();
            }

            private void CreateClassType()
            {
                newType.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(SerializableAttribute))));
                newType.BaseTypes.Add(typeof(INotifyPropertyChanged));
                newType.TypeAttributes = TypeAttributes.Public;
                newType.IsPartial = true;
            }

            private readonly CodeTypeDeclaration newType;
            public CodeTypeDeclaration EntityClass { get { return newType; } }

            protected void PropertyChangedEvent()
            {
                CodeMemberEvent PropertyChangedEvent = new CodeMemberEvent();
                PropertyChangedEvent.Name = "PropertyChanged";
                PropertyChangedEvent.Type = new CodeTypeReference(typeof(PropertyChangedEventHandler));
                PropertyChangedEvent.Attributes = MemberAttributes.Public;
                newType.Members.Add(PropertyChangedEvent);
            }

            protected void OnPropertyChanged()
            {
                CodeMemberMethod OnPropertyChanged = new CodeMemberMethod();
                OnPropertyChanged.Name = "OnPropertyChanged";
                OnPropertyChanged.Attributes = MemberAttributes.Family;
                OnPropertyChanged.Parameters.Add(new CodeParameterDeclarationExpression(
                    new CodeTypeReference(typeof(String)), "Property"));

                //Declare temp variable holding the event
                CodeVariableDeclarationStatement vardec = new CodeVariableDeclarationStatement(
                    new CodeTypeReference(typeof(PropertyChangedEventHandler)), "temp");
                vardec.InitExpression = new CodeEventReferenceExpression(
                    new CodeThisReferenceExpression(), "PropertyChanged");
                OnPropertyChanged.Statements.Add(vardec);

                //The part of the true, create the event and invoke it
                CodeObjectCreateExpression createArgs = new CodeObjectCreateExpression(
                    new CodeTypeReference(typeof(PropertyChangedEventArgs)),
                    new CodeArgumentReferenceExpression("Property"));
                CodeDelegateInvokeExpression raiseEvent = new CodeDelegateInvokeExpression(
                    new CodeVariableReferenceExpression("temp"),
                    new CodeThisReferenceExpression(), createArgs);

                //The conditino
                CodeExpression condition = new CodeBinaryOperatorExpression(
                    new CodeVariableReferenceExpression("temp"),
                    CodeBinaryOperatorType.IdentityInequality,
                    new CodePrimitiveExpression(null));

                //The if condition
                CodeConditionStatement ifTempIsNull = new CodeConditionStatement();
                ifTempIsNull.Condition = condition;
                ifTempIsNull.TrueStatements.Add(raiseEvent);
                OnPropertyChanged.Statements.Add(ifTempIsNull);
                newType.Members.Add(OnPropertyChanged);
            }

            public void AddProperty(string name, Type propertyType)
            {
                var fieldName = "_" + name.ToLower();
                CodeMemberField objField = new CodeMemberField(new CodeTypeReference(propertyType), fieldName);
                objField.Attributes = MemberAttributes.Private;
                newType.Members.Add(objField);

                CodeMemberProperty objProps = new CodeMemberProperty();
                objProps.Name = name;
                objProps.Type = new CodeTypeReference(propertyType);
                objProps.Attributes = MemberAttributes.Public;
                objProps.HasGet = true;
                objProps.HasSet = true;

                objProps.GetStatements.Add(
                    new CodeMethodReturnStatement(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(), fieldName)));

                CodeConditionStatement codst = new CodeConditionStatement();

                // 设置判断条件
                codst.Condition = new CodeBinaryOperatorExpression(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName),
                    CodeBinaryOperatorType.ValueEquality, new CodePropertySetValueReferenceExpression());

                codst.FalseStatements.Add(new CodeAssignStatement(
                            new CodeFieldReferenceExpression(
                                new CodeThisReferenceExpression(), fieldName),
                            new CodePropertySetValueReferenceExpression()));
                codst.FalseStatements.Add(
                 new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeThisReferenceExpression(), "OnPropertyChanged"),
                        new CodePrimitiveExpression(name)));
                objProps.SetStatements.Add(codst);
                newType.Members.Add(objProps);
            }
        }

        public void Add(DataTable dt)
        {
            if (dt == null) throw new ArgumentNullException(nameof(dt));
            if (string.IsNullOrEmpty(dt.TableName)) throw new ArgumentNullException(nameof(dt));
            if (Namespace.Types.OfType<CodeTypeDeclaration>().Any(nn => nn.Name == dt.TableName))
            {
            }
            GenerateNotifyPropertyEntity entity = new GenerateNotifyPropertyEntity(dt.TableName);
            foreach (DataColumn column in dt.Columns)
            {
                entity.AddProperty(column.ColumnName, column.DataType);
            }
            Namespace.Types.Add(entity.EntityClass);
        }
    }
}