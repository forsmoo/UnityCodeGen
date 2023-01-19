using System;
using System.CodeDom;
using System.Reflection;
using UnityEngine;

namespace CodeGeneration
{
    public class ClassInterpolatorGenerator
    {
        public ClassInterpolatorGenerator()
        {
        }


        public void GenerateInterpolator(Type originalType, string sourceFile)
        {
            var gen = new CodeUnitGenerator("Lirp");
            gen.NamespaceImports.Add("UnityEngine");

            var interpolatorClass = new ClassGenerator(originalType.Name + "Interpolator", MemberAttributes.Public | MemberAttributes.Static).SetIsSealed(false);
            interpolatorClass.MakeStatic();

            FieldInfo[] fields = originalType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            var copyMethod = new MethodGenerator("Copy");
            copyMethod.MakeStatic();
            copyMethod.AddParameter("this " + originalType.Name, "to");
            copyMethod.AddParameter(originalType, "from");

            var statementBuilder = new StatementBuilder();
            foreach (FieldInfo field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(IgnoreGenerateInterpolator)))
                {
                    if (field.FieldType.IsArray)
                    {
                    }
                    else
                    {
                        statementBuilder.AddSnippet("to." + field.Name + "=" + "from." + field.Name);
                    }

                }
            }
            copyMethod.AddStatement(statementBuilder);

            interpolatorClass.AddMethod(copyMethod);

            var lerpMethod = new MethodGenerator("Lerp");
            lerpMethod.MakeStatic();

            lerpMethod.AddParameter("this " + originalType.Name, "target");
            lerpMethod.AddParameter(originalType, "start");
            lerpMethod.AddParameter(originalType, "end");
            lerpMethod.AddParameter(typeof(float), "t");

            statementBuilder = new StatementBuilder();

            foreach (FieldInfo field in fields)
            {
                var angleLerp = Attribute.IsDefined(field, typeof(AngularLerpAttribute));
                if (!Attribute.IsDefined(field, typeof(IgnoreGenerateInterpolator)))
                {
                    if (field.FieldType.IsArray)
                    {
                        //var arrayElementType = field.FieldType.GetElementType();
                    }
                    else
                    {
                        statementBuilder.AddSnippet(GenerateInterpolateCode(field.FieldType, "target", field.Name, angleLerp));

                    }
                }
            }

            lerpMethod.AddStatement(statementBuilder);
            interpolatorClass.AddMethod(lerpMethod);

            gen.AddType(interpolatorClass);
            var ccu = gen.GenerateCompileUnit();

            CodeFileWriter writer = new CodeFileWriter();
            writer.Write(ccu, sourceFile);
        }

        static string GenerateInterpolateCode(Type type, string targetName, string fieldName, bool angleLerp)
        {
            if (angleLerp)
            {
                if (type == typeof(float))
                    return targetName + "." + fieldName + " = Mathf.LerpAngle(start." + fieldName + "," + "end." + fieldName + ",t)";
                if (type == typeof(Vector3))
                    return targetName + "." + fieldName + " = Mathf.LerpAngles(start." + fieldName + "," + "end." + fieldName + ",t)";
            }
            else
            {
                if (type == typeof(float))
                    return targetName + "." + fieldName + " = Mathf.Lerp(start." + fieldName + "," + "end." + fieldName + ",t)";
                if (type == typeof(Vector2))
                    return targetName + "." + fieldName + " = Vector2.Lerp(start." + fieldName + "," + "end." + fieldName + ",t)";
                if (type == typeof(Vector3))
                    return targetName + "." + fieldName + " = Vector3.Lerp(start." + fieldName + "," + "end." + fieldName + ",t)";
                if (type == typeof(Quaternion))
                    return targetName + "." + fieldName + " = Quaternion.Lerp(start." + fieldName + "," + "end." + fieldName + ",t)";
            }

            return "//" + type.ToString() + " " + fieldName;
        }

        static string GenerateInterpolateCodeNewMath(Type type, string targetName, string fieldName, bool angleLerp)
        {
            if (angleLerp)
            {
                if (type == typeof(float))
                    return targetName + "." + fieldName + " = mathfx.LerpAngle(start." + fieldName + "," + "end." + fieldName + ",t)";
                if (type == typeof(Vector3))
                    return targetName + "." + fieldName + " = mathfx.LerpAngles(start." + fieldName + "," + "end." + fieldName + ",t)";
            }
            else
            {
                if (type == typeof(float))
                    return targetName + "." + fieldName + " = math.lerp(start." + fieldName + "," + "end." + fieldName + ",t)";
                if (type == typeof(Vector2))
                    return targetName + "." + fieldName + " = math.lerp(start." + fieldName + "," + "end." + fieldName + ",t)";
                if (type == typeof(Vector3))
                    return targetName + "." + fieldName + " = math.lerp(start." + fieldName + "," + "end." + fieldName + ",t)";
                if (type == typeof(Quaternion))
                    return targetName + "." + fieldName + " = math.nlerp(start." + fieldName + "," + "end." + fieldName + ",t)";
            }

            return "//" + type.ToString() + " " + fieldName;
        }


    }

}