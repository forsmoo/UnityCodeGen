using System;
using System.CodeDom;
using System.Collections.Generic;


namespace CodeGeneration
{

    public class StatementBuilder
    {
        public List<CodeStatement> Expressions = new List<CodeStatement>();

        public static implicit operator CodeStatementCollection(StatementBuilder builder)
        {
            return new CodeStatementCollection(builder.Expressions.ToArray());
        }

        public StatementBuilder AddSnippet(string snippet)
        {
            Expressions.Add(new CodeExpressionStatement(new CodeSnippetExpression(snippet)));
            return this;
        }

        public StatementBuilder AddVariable(string type, string name)
        {
            Expressions.Add(new CodeVariableDeclarationStatement(type, name));
            return this;
        }

        public StatementBuilder AddVariable(Type type, string name)
        {
            Expressions.Add(new CodeVariableDeclarationStatement(type, name));
            return this;
        }

        public StatementBuilder AddVariable(string type, string name, object defaultValue)
        {
            Expressions.Add(new CodeVariableDeclarationStatement(type, name, new CodePrimitiveExpression(defaultValue)));
            return this;
        }

        public StatementBuilder AddVariable(Type type, string name, object defaultValue)
        {
            Expressions.Add(new CodeVariableDeclarationStatement(type, name, new CodePrimitiveExpression(defaultValue)));
            return this;
        }

        public StatementBuilder AddAssignement(ITarget to, ITarget from)
        {
            Expressions.Add(
                new CodeAssignStatement(
                    to.Expression,
                    from.Expression));
            return this;
        }

        public StatementBuilder AddCreator(FieldGenerator to, ParamBuilder parameters)
        {
            Expressions.Add(
                new CodeAssignStatement(
                    new FieldTarget(to).Expression,
                    new CodeObjectCreateExpression(to.Member.Type, parameters)));
            return this;
        }
        public StatementBuilder AddCreator(ITarget to, CodeTypeReference type, ParamBuilder parameters)
        {
            Expressions.Add(
                new CodeAssignStatement(
                    to.Expression,
                    new CodeObjectCreateExpression(type, parameters)));
            return this;
        }

        public StatementBuilder AddCreator(ITarget to, string type, ParamBuilder parameters)
        {
            Expressions.Add(
                new CodeAssignStatement(
                    to.Expression,
                    new CodeObjectCreateExpression(type, parameters)));
            return this;
        }

        public StatementBuilder AddConstructorFieldAssignement(string fieldName, string argument)
        {
            Expressions.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName),
                    new CodeArgumentReferenceExpression(argument)));
            return this;
        }

        public StatementBuilder AddConstructorPropertyAssignement(string propertyName, string argument)
        {
            Expressions.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), propertyName),
                    new CodeArgumentReferenceExpression(argument)));
            return this;
        }

        public StatementBuilder InvokeMethod(MethodGenerator methodGen, ParamBuilder parameters)
        {
            var invokeMethod = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), methodGen.Name, parameters);
            Expressions.Add(new CodeExpressionStatement(invokeMethod));
            return this;
        }

        public StatementBuilder InvokeMethod(ITarget target, MethodGenerator methodGen, ParamBuilder parameters)
        {
            var invokeMethod = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), methodGen.Name, parameters);
            Expressions.Add(
               new CodeAssignStatement(
               target.Expression,
               invokeMethod));
            return this;
        }

        public StatementBuilder InvokeMethod(ITarget target, ITarget methodSource, string methodName)
        {
            var invokeMethod = new CodeMethodInvokeExpression(methodSource.Expression, methodName);
            Expressions.Add(
                new CodeAssignStatement(
                target.Expression,
                invokeMethod));
            return this;
        }

        public StatementBuilder InvokeMethod(ITarget target, ITarget methodSource, string methodName, ParamBuilder parameters)
        {
            var invokeMethod = new CodeMethodInvokeExpression(methodSource.Expression, methodName, parameters);
            Expressions.Add(
                new CodeAssignStatement(
                target.Expression,
                invokeMethod));
            return this;
        }

        public StatementBuilder InvokeMethod(ITarget methodSource, string methodName)
        {
            var invokeMethod = new CodeMethodInvokeExpression(methodSource.Expression, methodName);
            Expressions.Add(new CodeExpressionStatement(invokeMethod));
            return this;
        }

        public StatementBuilder InvokeMethod(ITarget methodSource, string method, ParamBuilder parameters)
        {
            var invokeMethod = new CodeMethodInvokeExpression(methodSource.Expression, method, parameters);
            Expressions.Add(new CodeExpressionStatement(invokeMethod));
            return this;
        }

        public StatementBuilder InvokeMethod(ITarget methodSource, CodeMethodReferenceExpression method, ParamBuilder parameters)
        {
            var invokeMethod = new CodeMethodInvokeExpression(methodSource.Expression, method.MethodName, parameters);
            Expressions.Add(new CodeExpressionStatement(invokeMethod));
            return this;
        }

        public StatementBuilder InvokeEvent(ITarget target, CodeMemberEvent evt, ParamBuilder parameters)
        {
            var eventRef = new CodeEventReferenceExpression(new CodeThisReferenceExpression(), evt.Name);
            var compareExpression = new CodeBinaryOperatorExpression(
                eventRef,
                CodeBinaryOperatorType.IdentityInequality,
                new CodePrimitiveExpression("null"));

            var invoke = new CodeDelegateInvokeExpression(
                eventRef,
                parameters.Parameters.ToArray());

            var condition = new CodeConditionStatement(compareExpression, new CodeAssignStatement(
                target.Expression,
                invoke));

            Expressions.Add(condition);
            return this;
        }

        public StatementBuilder InvokeEvent(CodeMemberEvent evt, ParamBuilder parameters)
        {
            var eventRef = new CodeEventReferenceExpression(new CodeThisReferenceExpression(), evt.Name);
            var compareExpression = new CodeBinaryOperatorExpression(
                eventRef,
                CodeBinaryOperatorType.IdentityInequality,
                new CodePrimitiveExpression(null));

            var invoke = new CodeDelegateInvokeExpression(
                eventRef,
                parameters.Parameters.ToArray());
            //new CodeExpression[] { new CodeThisReferenceExpression(), new CodeObjectCreateExpression("System.EventArgs") });

            var condition = new CodeConditionStatement(compareExpression, new CodeExpressionStatement(invoke));

            Expressions.Add(condition);
            return this;
        }

        /*public StatementBuilder InvokeEvent(string eventName, ParamBuilder parameters)
        {
            //var invoke = new CodeDelegateInvokeExpression()
            
            var compareExpression = new CodeBinaryOperatorExpression(
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), eventName),
                CodeBinaryOperatorType.IdentityInequality,
                new CodePrimitiveExpression("null"));
            
            var invoke = new CodeDelegateInvokeExpression(
                new CodeEventReferenceExpression(
                    new CodeThisReferenceExpression(),
                    eventName),
                parameters.Parameters.ToArray());
                //new CodeExpression[] { new CodeThisReferenceExpression(), new CodeObjectCreateExpression("System.EventArgs") });

            var condition = new CodeConditionStatement(compareExpression,new CodeExpressionStatement(invoke));
            
            Expressions.Add(condition);
            return this;
        }*/

        public StatementBuilder AttachEvent(CodeMemberMethod memberMethod, ITarget eventSource, string eventName)
        {
            var handler = new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), memberMethod.Name);
            Expressions.Add(new CodeAttachEventStatement(
                eventSource.Expression,
                eventName,
                handler));
            return this;
        }

        public StatementBuilder AttachEvent(CodeMemberMethod memberMethod, ITarget eventSource, CodeMemberEvent evt)
        {
            var handler = new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), memberMethod.Name);
            Expressions.Add(new CodeAttachEventStatement(
                eventSource.Expression,
                evt.Name,
                handler));
            return this;
        }

        public StatementBuilder DetachEvent(CodeMemberMethod memberMethod, ITarget target, CodeMemberEvent evt)
        {
            var handler = new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), memberMethod.Name);
            Expressions.Add(new CodeRemoveEventStatement(
                target.Expression,
                evt.Name,
                handler));
            return this;
        }

        public StatementBuilder AddForLoop(string iteratorName, int numLoops, StatementBuilder builder)
        {
            // Declares and initializes an integer variable named testInt.
            CodeVariableDeclarationStatement iteratorVariable = new CodeVariableDeclarationStatement(typeof(int), iteratorName, new CodePrimitiveExpression(0));

            var iteratorReference = new CodeVariableReferenceExpression(iteratorName);

            // Creates a for loop that sets testInt to 0 and continues incrementing testInt by 1 each loop until testInt is not less than 10.
            CodeIterationStatement forLoop = new CodeIterationStatement(
                // initStatement parameter for pre-loop initialization.
                new CodeAssignStatement(iteratorReference, new CodePrimitiveExpression(0)),
                // testExpression parameter to test for continuation condition.
                new CodeBinaryOperatorExpression(
                    iteratorReference,
                    CodeBinaryOperatorType.LessThan,
                    new CodePrimitiveExpression(numLoops)),
                // incrementStatement parameter indicates statement to execute after each iteration.
                new CodeAssignStatement(iteratorReference, new CodeBinaryOperatorExpression(
                    iteratorReference, CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1))),
                // statements parameter contains the statements to execute during each interation of the loop.
                // Each loop iteration the value of the integer is output using the Console.WriteLine method.
                builder.Expressions.ToArray()
                );
            Expressions.Add(iteratorVariable);
            Expressions.Add(forLoop);

            /*var body = new CodeStatement[] { new CodeExpressionStatement( new CodeMethodInvokeExpression( new CodeMethodReferenceExpression(
                new CodeTypeReferenceExpression("Console"), "WriteLine" ), new CodeMethodInvokeExpression(
                new CodeVariableReferenceExpression("testInt"), "ToString" ) ) ) }
            */
            return this;
        }

        public StatementBuilder AddWhile(CodeExpression condition, StatementBuilder builder)
        {
            CodeIterationStatement codeWhile = new CodeIterationStatement();
            codeWhile.TestExpression = condition;//new CodeSnippetExpression("mt.MoveNext()");
            codeWhile.Statements.AddRange(builder);
            codeWhile.IncrementStatement = new CodeSnippetStatement("");
            codeWhile.InitStatement = new CodeSnippetStatement("");
            Expressions.Add(codeWhile);
            return this;
        }

        public StatementBuilder AddReturn(CodePrimitiveExpression expression)
        {
            Expressions.Add(new CodeMethodReturnStatement(expression));
            return this;
        }
    }

}