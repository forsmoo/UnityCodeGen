using CodeGeneration;
using NUnit.Framework;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExpressonWriterTests
{

    [Test]
    public void TestWriteCodeUnit()
    {
        ICodeWriter writer = new DebugFileWriter();

        using (var stream = new StringWriter())
        {
            var units = new List<IUnit>();
            units.Add(new TextUnit("Code"));

            writer.Write(stream, units);
            string output = stream.ToString();

            Assert.IsTrue(output.StartsWith("Code"));
        }

        Assert.Pass();
    }

    [Test]
    public void TestWriteClassToFile()
    {
        var gen = new CodeUnitGenerator("TestCodeGen");
        gen.NamespaceImports.Add("System");

        string sourceFile = "mycodegenpath.cs";
        ClassGenerator classGen = new ClassGenerator("MyCodeDOMTestClass");

        gen.AddType(classGen);
        var ccu = gen.GenerateCompileUnit();

        CodeFileWriter writer = new CodeFileWriter();
        writer.Write(ccu, sourceFile);

        bool exists = File.Exists(sourceFile);
        Assert.IsTrue(exists);
    }

    [Test]
    public void TestGenerateClassWithImports()
    {
        var gen = new CodeUnitGenerator("TestCodeGen");
        gen.NamespaceImports.Add("System");

        ClassGenerator classGen = new ClassGenerator("MyCodeDOMTestClass");

        gen.AddType(classGen);
        var ccu = gen.GenerateCompileUnit();
        string output = StringCompiler.CompileToString(ccu);

        Assert.IsTrue(output.Length > 0);
        Assert.IsTrue(output.Contains("System"));
        Assert.IsTrue(output.Contains("TestCodeGen"));
        Assert.IsTrue(output.Contains("MyCodeDOMTestClass"));
    }

    [Test]
    public void TestGenerateClassWithMethod()
    {
        var gen = new CodeUnitGenerator("TestCodeGen");
        gen.NamespaceImports.Add("System");
        ClassGenerator classGen = new ClassGenerator("MyCodeDOMTestClass")
            .SetIsSealed(true)
            .SetIsSealed(false);
        MethodGenerator method = new MethodGenerator("DoStuff");

        classGen.AddMethod(method);
        gen.AddType(classGen);
        var ccu = gen.GenerateCompileUnit();

        var output = StringCompiler.CompileToString(ccu);

        Assert.IsFalse(output.Contains("sealed"));
        Assert.IsTrue(output.Contains("DoStuff"));
    }

    [Test]
    public void TestGenerateClassWithFieldPropertyMethod()
    {
        var gen = new CodeUnitGenerator("TestCodeGen");
        var classGen = new ClassGenerator("TestClass");
        var field = new FieldGenerator(typeof(int), "MyField");
        var property = new AutoPropertyGenerator("TestClass", "MyProp");
        var method = new MethodReturnField(field);
        method.AddStatement("Debug.Log(\"This is a string\"");
        classGen.AddAutoProperty(property);
        classGen.AddField(field);
        classGen.AddMethod(method);
        gen.AddType(classGen);
        var ccu = gen.GenerateCompileUnit();

        var output = StringCompiler.CompileToString(ccu);

        //Debug.Log(output);
        Assert.IsTrue(output.Contains("MyProp"));
        Assert.IsTrue(output.Contains("MyField"));
        Assert.IsTrue(output.Contains("GetMyField"));
    }

    [Test]
    public void TestGenerateClassWithConstructor()
    {
        var gen = new CodeUnitGenerator("TestCodeGen");
        var classGen = new ClassGenerator("TestClass");
        var field = new FieldGenerator(typeof(int), "MyField");
        var property = new AutoPropertyGenerator("TestClass", "MyProp");
        var constructor = new ConstructorGenerator()
            .AddParameter(field.FieldType, field.Name)
            .AddParameter(property.Name, property.PropertyType)
            .AddStatement(new StatementBuilder()
                .AddConstructorFieldAssignement(field.Name, field.Name)
                .AddConstructorPropertyAssignement(property.Name, property.Name));

        classGen.AddAutoProperty(property);
        classGen.AddField(field);
        classGen.AddMethod(constructor);
        gen.AddType(classGen);
        var ccu = gen.GenerateCompileUnit();

        var output = StringCompiler.CompileToString(ccu);

        Debug.Log(output);
        Assert.IsTrue(output.Contains("MyProp"));
        Assert.IsTrue(output.Contains("MyField"));
        Assert.IsTrue(output.Contains("TestClass("));
    }

    [Test]
    public void TestGenerateClassImplementation()
    {
        var gen = new CodeUnitGenerator("TestCodeGen");
        var classGen = new ClassGenerator("TestClass")
            .SetIsPartial(true)
            .SetIsAbstract(true)
            .AddBaseType("IComponent");
        var field = new FieldGenerator(typeof(int), "MyField");
        var property = new AutoPropertyGenerator("TestClass", "MyProp");
        var constructor = new ConstructorGenerator()
            .AddBaseCall("BaseArg")
            .AddParameter(field.FieldType, field.Name)
            .AddParameter(property.Name, property.PropertyType)
            .AddStatement(new StatementBuilder()
                .AddConstructorFieldAssignement(field.Name, field.Name)
                .AddConstructorPropertyAssignement(property.Name, property.Name));

        classGen.AddAutoProperty(property);
        classGen.AddField(field);
        classGen.AddMethod(constructor);
        gen.AddType(classGen);
        var ccu = gen.GenerateCompileUnit();

        var output = StringCompiler.CompileToString(ccu);

        Debug.Log(output);
        Assert.IsTrue(output.Contains("base("));
        Assert.IsTrue(output.Contains("BaseArg"));
    }

    [Test]
    public void TestGenerateDelegateEvents()
    {
        var gen = new CodeUnitGenerator("TestCodeGen");
        var classGen = new ClassGenerator("TestClass");
        var delegateGen = new DelegateGenerator("MyEventHandler")
            .AddParameter("TestClass", "myRef")
            .AddReturnType(typeof(bool));

        var eventGen = new EventGenerator("OnSomeTrigger", delegateGen.delegateType);
        classGen.AddEvent(eventGen);
        var fireEventMethod = new MethodGenerator("FireEvent")
            .AddStatement(new StatementBuilder()
                //.AddSnippetExpression("Debug.Log();DoMoreStuff();"));
                .InvokeEvent(eventGen, new ParamBuilder()
                    .AddPrimitiveExpression("new TestClass()")));
        classGen.AddMethod(fireEventMethod);

        gen.AddType(delegateGen);
        gen.AddType(classGen);

        var classSubscriber = new ClassGenerator("MySubscribeClass");
        var field = new FieldGenerator("TestClass", "eventSource");
        classSubscriber.AddField(field);

        var constructor = new ConstructorGenerator(classSubscriber.classType);
        classSubscriber.AddMethod(constructor);

        var eventHandler = new MethodGenerator("OnSomeTrigger", delegateGen)
            .AddStatement(new StatementBuilder()
                .AddSnippet("Debug.Log(\"Expression1\");")
                .AddSnippet("Debug.Log(\"Expression2\");"));

        var subscribeMethod = new MethodGenerator("AddListener")
            .AddStatement(new StatementBuilder()
                .AttachEvent(eventHandler, new FieldTarget(field), eventGen));

        classSubscriber.AddMethod(
            new MethodGenerator("Unsubscribe").AddStatement(
                new StatementBuilder()
                    .DetachEvent(eventHandler, new FieldTarget(field), eventGen)));
        classSubscriber.AddMethod(eventHandler);
        classSubscriber.AddMethod(subscribeMethod);
        gen.AddType(classSubscriber);

        var ccu = gen.GenerateCompileUnit();

        var output = StringCompiler.CompileToString(ccu);

        //Debug.Log(output);
        Assert.IsTrue(output.Contains("OnSomeTrigger"));
        Assert.IsTrue(output.Contains("FireEvent"));
        Assert.IsTrue(output.Contains("+="));
        Assert.IsTrue(output.Contains("-="));
        Assert.IsTrue(output.Contains("delegate"));
        Assert.IsTrue(output.Contains("event"));
    }

    [Test]
    public void TestGenerateInterface()
    {
        var gen = new CodeUnitGenerator("TestCodeGen");
        var interfaceGen = new InterfaceGenerator("IMyInterface");
        var method = new MethodGenerator("MyInterfaceMethod")
            .AddParameter(new CodeTypeReference(typeof(bool)), "arg");

        interfaceGen.AddMember(method.Method);
        gen.AddType(interfaceGen);
        var ccu = gen.GenerateCompileUnit();

        var output = StringCompiler.CompileToString(ccu);

        Debug.Log(output);
        Assert.IsTrue(output.Contains("interface IMyInterface"));
        Assert.IsTrue(output.Contains("void MyInterfaceMethod(bool arg);"));
    }

    [Test]
    public void TestGenerateEnum()
    {
        var gen = new CodeUnitGenerator("TestCodeGen");
        var interfaceGen = new EnumGenerator("MyEnum");
        interfaceGen.AddOption("Option1");
        interfaceGen.AddOption("Option2");
        gen.AddType(interfaceGen);
        var ccu = gen.GenerateCompileUnit();

        var output = StringCompiler.CompileToString(ccu);

        Debug.Log(output);
        Assert.IsTrue(output.Contains("enum MyEnum"));
        Assert.IsTrue(output.Contains("Option1"));
        Assert.IsTrue(output.Contains("Option2"));
    }

    [Test]
    public void TestGenerateAttributes()
    {
        var gen = new CodeUnitGenerator("TestCodeGen");

        var attributeGen = new ClassGenerator("MyAttribute")
            .AddBaseType("Attribute");

        var classGen = new ClassGenerator("TestClass");
        classGen.SetCustomAttribute("MyAttribute", new ParamBuilder());

        var field = new FieldGenerator(typeof(int), "MyField");
        field.SetCustomAttribute("MyAttribute", new ParamBuilder());
        var property = new AutoPropertyGenerator("TestClass", "MyProp");
        property.SetCustomAttribute("MyAttribute", new ParamBuilder());


        classGen.AddAutoProperty(property);
        classGen.AddField(field);
        gen.AddType(attributeGen);
        gen.AddType(classGen);
        var ccu = gen.GenerateCompileUnit();

        var output = StringCompiler.CompileToString(ccu);

        Debug.Log(output);
        Assert.IsTrue(output.Contains("MyAttribute"));
    }

    [Test]
    public void TestGenerateForLoop()
    {
        var gen = new CodeUnitGenerator("TestCodeGen");
        var classGen = new ClassGenerator("TestClass")
            .SetIsSealed(true);
        var method = new MethodGenerator("IterateStuff");
        method.AddStatement(new StatementBuilder()
            .AddVariable(typeof(string[]), "myStuff", true)
            .AddVariable(typeof(bool), "myBool", true)
            .AddVariable(typeof(int), "myInt", 1)
            .AddSnippet("//Snippet")
            .AddForLoop("i", 10, new StatementBuilder()
                .AddSnippet("Debug.Log()")
                .AddAssignement(new VariableTarget("myInt"), new VariableTarget("i"))
                .InvokeMethod(new VariableTarget("myStuff"),
                                new ClassTarget("UnityEngine"),
                                "GetAllStuff",
                                new ParamBuilder()
                                    .AddVariable("myInt"))));
        classGen.AddMethod(method);
        gen.AddType(classGen);
        var ccu = gen.GenerateCompileUnit();

        var output = StringCompiler.CompileToString(ccu);

        Debug.Log(output);
        Assert.IsTrue(output.Contains("for (i = 0"));
        Assert.IsTrue(output.Contains("(i < 10"));
        Assert.IsTrue(output.Contains("myStuff = UnityEngine.GetAllStuff("));
    }

    [Test]
    public void TestWhileLoop()
    {
        var gen = new CodeUnitGenerator("TestCodeGen");
        var classGen = new ClassGenerator("TestClass")
            .SetIsSealed(true);
        var method = new MethodGenerator("IterateStuff");
        method.AddStatement(new StatementBuilder()
            .AddVariable(typeof(string[]), "myStuff", true)
            .AddVariable(typeof(bool), "myBool", true)
            .AddWhile(new CodeSnippetExpression("iterator.MoveNext(myBool)"), new StatementBuilder()
                .AddSnippet("Debug.Log(myBool)")));

        classGen.AddMethod(method);
        gen.AddType(classGen);
        var ccu = gen.GenerateCompileUnit();

        var output = StringCompiler.CompileToString(ccu);

        Debug.Log(output);
    }

    /* This is not implmented in the CSharpProvider
    [Test]
    public void TestParseCode()
    {

        //Not implmented
        string source = @"
using System;
namespace MyTestNamespace
{
    public class MyTestClass
    {
        public class MyTestMethod()
        {
            var variable = new string();
        }
    }
}
";
        var reader = new CodeStringReader();
        var ccu = reader.Parse(source);

        bool hasNamespace = false;
        bool hasClass = false;
        bool hasMethod = false;
        bool hasMethodVariable = false;
        foreach (CodeNamespace ns in ccu.Namespaces)
        {
            if (ns.Name == "MyTestNamespace")
            {
                hasNamespace = true;
                foreach (CodeTypeDeclaration type in ns.Types)
                {
                    if (type.Name == "MyTestClass")
                    {
                        hasClass = type.IsClass;
                    }
                }
                
            }
        }

        Assert.IsTrue(hasNamespace);
        Assert.IsTrue(hasClass);
        Assert.IsTrue(hasMethod);
        Assert.IsTrue(hasMethodVariable);
    }
    */
}
