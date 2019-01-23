using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IssueTrackerTests.Helpers
{
    public static class ExtensionMethods
    {
        public static Type ShouldHaveType(this Assembly assembly, string fullyQualifiedTypeName)
        {
            var type = assembly.GetType(fullyQualifiedTypeName);
            var names = ParseFullyQualifiedTypeName(fullyQualifiedTypeName);

            Assert.True(type != null,
                $"Did you add a class named {names.ClassName} to the {names.NamespaceName} namespace?");

            return type;
        }

        public static Type ThatIsPublic(this Type type)
        {
            Assert.True(type.IsPublic,
                $"Did you define the {GetTypeName(type)} class using the 'public' access modifier?");

            return type;
        }

        public static Type AndStatic(this Type type)
        {
            Assert.True(type.IsAbstract && type.IsSealed,
                $"Did you define the {GetTypeName(type)} class as static?");

            return type;
        }

        public static Type AndNonStatic(this Type type)
        {
            Assert.True(!(type.IsAbstract && type.IsSealed),
                $"Did you define the {GetTypeName(type)} class without using the 'static' keyword?");

            return type;
        }

        public static Type AndInheritsFrom(this Type type, Type baseType)
        {
            Assert.True(type.BaseType == baseType,
                $"Does the {GetTypeName(type)} class inherit from the {GetTypeName(baseType, useFullName: true)} base class?");

            return type;
        }

        public static ConstructorInfo ShouldHaveASingleConstructor(this Type type,
            Parameter[] parameters = null, bool validateParameterNames = false)
        {
            return ShouldHaveConstructor(
                type, parameters, validateParameterNames, validateConstructorCount: true);
        }

        public static ConstructorInfo ShouldHaveConstructor(this Type type, 
            Parameter[] parameters = null, bool validateParameterNames = false, 
            bool validateConstructorCount = false)
        {
            if (parameters == null)
            {
                parameters = new Parameter[0];
            }

            var parameterTypes = parameters
                .Select(p => p.Type)
                .ToArray();

            var constructor = type.GetConstructor(
                BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.Static,
                null,
                parameterTypes,
                null);

            string userMessage = null;
            string className = GetTypeName(type);

            if (parameters.Length > 0)
            {
                var parametersDescription = GetParameterTypesDescription(parameters);
                userMessage = $"Did you define a constructor with {parametersDescription} for the {className} class?";
            }
            else
            {
                userMessage = $"Did you define a constructor (with no parameters) for the {className} class?";
            }

            Assert.True(constructor != null, userMessage);

            if (validateParameterNames)
            {
                var constructorParameters = constructor.GetParameters();

                for (int parameterIndex = 0; parameterIndex < constructorParameters.Length; parameterIndex++)
                {
                    var parameterInfo = constructorParameters[parameterIndex];
                    var parameter = parameters[parameterIndex];

                    Assert.True(parameterInfo.Name == parameter.Name,
                        $"Did you name the parameter{(parameters.Length == 1 ? string.Empty : "s")} for the constructor in the {className} class {GetParameterNamesDescription(parameters)}?");
                }
            }

            if (validateConstructorCount)
            {
                var constructors = type.GetConstructors();

                Assert.True(constructors.Length == 1, 
                    $"Did you define a single constructor for the {className}?");
            }

            return constructor;
        }

        public static ConstructorInfo ThatIsPublic(this ConstructorInfo constructorInfo)
        {
            Assert.True(constructorInfo.IsPublic,
                $"Did you define the constructor for the '{GetTypeName(constructorInfo.DeclaringType)}' class using the 'public' access modifier?");

            return constructorInfo;
        }

        public static ConstructorInfo AndStatic(this ConstructorInfo constructorInfo)
        {
            Assert.True(constructorInfo.IsStatic,
                $"Did you define the constructor for the '{GetTypeName(constructorInfo.DeclaringType)}' class as static?");

            return constructorInfo;
        }

        public static ConstructorInfo AndNonStatic(this ConstructorInfo constructorInfo)
        {
            Assert.True(!constructorInfo.IsStatic,
                $"Did you define the constructor for the '{GetTypeName(constructorInfo.DeclaringType)}' class without using the 'static' keyword?");

            return constructorInfo;
        }

        public static PropertyInfo ShouldHavePropertyOfType<TPropertyType>(this Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName, 
                BindingFlags.Instance | 
                BindingFlags.NonPublic | 
                BindingFlags.Public | 
                BindingFlags.Static);

            Assert.True(property != null,
                $"Did you define a property named '{propertyName}' in the {GetTypeName(type)} class?");

            Assert.True(property.PropertyType == typeof(TPropertyType), 
                $"Did you define the '{propertyName}' property in the {GetTypeName(type)} class to be of type '{GetTypeName(typeof(TPropertyType))}'?");

            return property;
        }

        public static PropertyInfo ThatHasPublicNonStaticGetterAndSetter(this PropertyInfo propertyInfo)
        {
            var getter = propertyInfo.GetGetMethod();
            var setter = propertyInfo.GetSetMethod();

            Assert.True(getter != null,
                $"Did you define the '{propertyInfo.Name}' property in the {GetTypeName(propertyInfo.DeclaringType)} class with a getter?");

            Assert.True(setter != null,
                $"Did you define the '{propertyInfo.Name}' property in the {GetTypeName(propertyInfo.DeclaringType)} class with a setter?");

            Assert.True(getter.IsPublic && setter.IsPublic,
                $"Did you define the '{propertyInfo.Name}' property in the {GetTypeName(propertyInfo.DeclaringType)} class using the 'public' access modifier?");

            Assert.True(!(getter.IsStatic && setter.IsStatic),
                $"Did you define the '{propertyInfo.Name}' property in the {GetTypeName(propertyInfo.DeclaringType)} class without using the 'static' keyword?");

            return propertyInfo;
        }

        public static PropertyInfo AndHasAttribute<TAttributeType>(this PropertyInfo propertyInfo,
                Func<TAttributeType, bool> validator = null, string userMessage = null)
            where TAttributeType : Attribute
        {
            var attribute = propertyInfo.GetCustomAttribute<TAttributeType>();

            var attributeName = typeof(TAttributeType).Name;
            var attributeIndex = attributeName.IndexOf("Attribute");
            if (attributeIndex != -1)
            {
                attributeName = attributeName.Substring(0, attributeIndex);
            }

            Assert.True(attribute != null, 
                $"Did you decorate the '{propertyInfo.Name}' property in the {GetTypeName(propertyInfo.DeclaringType)} class with a {attributeName} attribute?");

            if (validator != null)
            {
                Assert.True(validator(attribute), userMessage);
            }

            return propertyInfo;
        }

        public static MethodInfo ShouldHaveMethod(this Type type, string methodName, 
            Parameter[] parameters = null, bool validateParameterNames = false)
        {
            if (parameters == null)
            {
                parameters = new Parameter[0];
            }

            var parameterTypes = parameters
                .Select(p => p.Type)
                .ToArray();

            MethodInfo method = type.GetMethod(methodName,
                BindingFlags.Instance |
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.Static,
                null,
                parameterTypes,
                null);

            string userMessage = null;
            string className = GetTypeName(type);

            if (parameters.Length > 0)
            {
                var parametersDescription = GetParameterTypesDescription(parameters);
                userMessage = $"Did you define a method named '{methodName}' with {parametersDescription} in the {className} class?";
            }
            else
            {
                userMessage = $"Did you define a method named '{methodName}' (with no parameters) in the {className} class?";
            }

            Assert.True(method != null, userMessage);

            if (method != null && validateParameterNames)
            {
                var methodParameters = method.GetParameters();

                for (int parameterIndex = 0; parameterIndex < methodParameters.Length; parameterIndex++)
                {
                    var parameterInfo = methodParameters[parameterIndex];
                    var parameter = parameters[parameterIndex];

                    Assert.True(parameterInfo.Name == parameter.Name,
                        $"Did you name the parameter{(parameters.Length == 1 ? string.Empty : "s")} for the '{method.Name}' method in the {className} class {GetParameterNamesDescription(parameters)}?");
                }
            }

            return method;
        }

        public static MethodInfo ThatIsPublic(this MethodInfo methodInfo)
        {
            Assert.True(methodInfo.IsPublic,
                $"Did you define the '{methodInfo.Name}' method in the {GetTypeName(methodInfo.DeclaringType)} class using the 'public' access modifier?");

            return methodInfo;
        }

        public static MethodInfo ThatIsProtected(this MethodInfo methodInfo)
        {
            Assert.True((!methodInfo.IsPublic && !methodInfo.IsPrivate),
                $"Did you define the '{methodInfo.Name}' method in the {GetTypeName(methodInfo.DeclaringType)} class using the 'protected' access modifier?");

            return methodInfo;
        }

        public static MethodInfo ThatIsPrivate(this MethodInfo methodInfo)
        {
            Assert.True(methodInfo.IsPrivate,
                $"Did you define the '{methodInfo.Name}' method in the {GetTypeName(methodInfo.DeclaringType)} class using the 'private' access modifier?");

            return methodInfo;
        }

        public static MethodInfo AndStatic(this MethodInfo methodInfo)
        {
            Assert.True(methodInfo.IsStatic,
                $"Did you define the '{methodInfo.Name}' method in the {GetTypeName(methodInfo.DeclaringType)} class as static?");

            return methodInfo;
        }

        public static MethodInfo AndNonStatic(this MethodInfo methodInfo)
        {
            Assert.True(!methodInfo.IsStatic,
                $"Did you define the '{methodInfo.Name}' method in the '{GetTypeName(methodInfo.DeclaringType)}' class without using the 'static' keyword?");

            return methodInfo;
        }

        public static MethodInfo AndReturnsVoid(this MethodInfo methodInfo)
        {
            Assert.True(methodInfo.ReturnType == typeof(void),
                $"Did you define the '{methodInfo.Name}' method's return type in the {GetTypeName(methodInfo.DeclaringType)} class as 'void'?");

            return methodInfo;
        }

        public static MethodInfo AndReturns<TReturnType>(this MethodInfo methodInfo)
        {
            var returnType = typeof(TReturnType);

            Assert.True(methodInfo.ReturnType == returnType,
                $"Did you define the '{methodInfo.Name}' method's return type in the {GetTypeName(methodInfo.DeclaringType)} class as '{GetTypeName(returnType)}'?");

            return methodInfo;
        }

        public static ObjectWithMockedDependency<TObjectType, TMockType> VerifyAndAssert<TObjectType, TMockType>(this ObjectWithMockedDependency<TObjectType, TMockType> objectWithMockedDependency, 
            Expression<Action<TMockType>> expression, string userMessage, Times? times = null)
            where TObjectType : class
            where TMockType : class
        {
            objectWithMockedDependency.Mock.VerifyAndAssert(expression, userMessage, times);

            return objectWithMockedDependency;
        }

        public static Mock<TMockType> VerifyAndAssert<TMockType>(this Mock<TMockType> mock,
            Expression<Action<TMockType>> expression, string userMessage, Times? times = null)
            where TMockType : class
        {
            var verified = false;

            try
            {
                if (times != null)
                {
                    mock.Verify(expression, times.Value);
                }
                else
                {
                    mock.Verify(expression);
                }
                verified = true;
            }
            catch (MockException) { }

            Assert.True(verified, userMessage);

            return mock;
        }

        private static (string NamespaceName, string ClassName) ParseFullyQualifiedTypeName(string fullyQualifiedTypeName)
        {
            var lastDotIndex = fullyQualifiedTypeName.LastIndexOf('.');
            var namespaceName = fullyQualifiedTypeName.Substring(0, lastDotIndex);
            var className = fullyQualifiedTypeName.Substring(lastDotIndex + 1);

            return (namespaceName, className);
        }

        private static string GetParameterTypesDescription(Parameter[] parameters)
        {
            // Example return value: 3 parameters (using the types 'int', 'string', and 'int')

            var typeNames = parameters.Select(p => GetTypeName(p.Type)).ToList();
            var parameterTypeNames = GetCommaSeparatedText(typeNames);

            return $"{parameters.Length} parameter{(parameters.Length == 1 ? string.Empty : "s")} (using the type{(parameters.Length == 1 ? string.Empty : "s")} {parameterTypeNames})";
        }

        private static string GetParameterNamesDescription(Parameter[] parameters)
        {
            // Example return value: 'id', 'name', and 'notes'

            var names = parameters.Select(p => p.Name).ToList();
            return GetCommaSeparatedText(names);
       }
        
        private static string GetCommaSeparatedText<TItemType>(List<TItemType> values) 
        {
            var text = $"'{string.Join("', '", values)}'";

            if (values.Count > 1)
            {
                var separatorLastIndex = text.LastIndexOf("', '");
                text = text.Substring(0, separatorLastIndex) +
                    "'" + (values.Count > 2 ? "," : string.Empty) + " and '" +
                    text.Substring(separatorLastIndex + 4);
            }

            return text;
        }

        private static string GetTypeName(Type type, bool useFullName = false)
        {
            // Examples:
            // TypeName
            // TypeName<GenericType>
            // TypeName<GenericType, GenericType>

            string typeName = useFullName ? type.FullName : GetKeywordName(type.Name);

            if (type.IsGenericType)
            {
                // Remove the default generic type name.
                if (typeName.IndexOf('`') != -1)
                {
                    typeName = typeName.Substring(0, typeName.IndexOf('`'));
                }

                var genericTypeNames = type.GenericTypeArguments.Select(t => GetTypeName(t)).ToList();
                var genericTypeNamesText = string.Join(", ", genericTypeNames);

                if (typeName == "Nullable")
                {
                    typeName = genericTypeNamesText + "?";
                }
                else
                {
                    typeName += $"<{genericTypeNamesText}>";
                }
            }

            return typeName;
        }

        private static string GetKeywordName(string name)
        {
            switch (name)
            {
                case "Int32":
                    return "int";
                case "String":
                    return "string";
                default:
                    return name;
            }
        }
    }
}
