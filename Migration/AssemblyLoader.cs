using Migration.Model;
using Migration.Model.Xml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Migration
{
    internal static class AssemblyLoader
    {
        public static MainModel LoadAssembly(string exePath, string xmlPath, TransConfig transConfig)
        {
            MainModel mainModel = new MainModel();
            Node root = new RootNode { Name = "ROOT" };
            mainModel.Root = root;

            //Load XML
            var xmlDoc = LoadXml(xmlPath);
            if (xmlDoc != null)
            {
                if (xmlDoc.Members != null && xmlDoc.Members.Length > 0)
                {
                    foreach (var member in xmlDoc.Members)
                    {
                        if (!string.IsNullOrEmpty(member.Summary))
                        {
                            member.Summary = member.Summary.Trim();
                        }
                    }
                }
            }

            //LoadTypes
            var types = LoadTypes(exePath, transConfig.ReflectionOnlyLoad);

            //Process
            if (xmlDoc != null)
            {
                root.Items = Transform(types, xmlDoc.Members, transConfig);
            }
            else
            {
                root.Items = Transform(types, null, transConfig);
            }

            return mainModel;
        }

        private static Type[] LoadTypes(string path, bool reflectionOnlyLoad)
        {
            Type[] types = null;

            if (reflectionOnlyLoad)
            {
                AppDomain appDomain = AppDomain.CurrentDomain;

                appDomain.ReflectionOnlyAssemblyResolve += (sender, args) =>
                {
                    AssemblyName name = new AssemblyName(args.Name);

                    string asmToCheck = Path.GetDirectoryName(path) + "\\" + name.Name + ".dll";

                    if (File.Exists(asmToCheck))
                    {
                        return Assembly.ReflectionOnlyLoadFrom(asmToCheck);
                    }

                    return Assembly.ReflectionOnlyLoad(args.Name);
                };

                Assembly asm = Assembly.ReflectionOnlyLoadFrom(path);

                types = asm.GetTypes();
            }
            else
            {
                types = Assembly.LoadFrom(path).GetTypes();
            }

            return types;
        }

        static XmlDoc LoadXml(string xmlPath)
        {
            XmlDoc doc = null;

            if (!string.IsNullOrEmpty(xmlPath))
            {
                var element = XElement.Load(xmlPath);

                doc = LoadXml(element);
            }

            return doc;
        }

        static XmlDoc LoadXml(XElement document)
        {
            XmlDoc returnValue = new XmlDoc();

            if (document != null)
            {
                var assemblyElement = document.Element("assembly");
                if (assemblyElement != null)
                {
                    returnValue.Assembly = new XmlAssembly();
                    var nameElement = assemblyElement.Element("name");
                    if (nameElement != null)
                    {
                        returnValue.Assembly.Name = nameElement.Value;
                    }
                }

                var members = document.Element("members");
                if (members != null)
                {
                    var xmlMembers = new List<XmlMember>();

                    var items = members.Elements();
                    foreach (var member in items)
                    {
                        XmlMember xmlMember = new XmlMember();

                        //Name
                        var nameAttribute = member.Attribute("name");
                        if (nameAttribute != null)
                        {
                            xmlMember.Name = nameAttribute.Value;
                        }

                        //Summary
                        var summaryElement = member.Element("summary");
                        if (summaryElement != null)
                        {
                            if (!string.IsNullOrEmpty(summaryElement.Value))
                            {
                                xmlMember.Summary = summaryElement.Value.Trim();
                            }
                        }

                        var paramElements = member.Elements("param");
                        if (paramElements != null)
                        {
                            var xmlMemberParamList = new List<XmlMemberParam>();
                            foreach (var paramElement in paramElements)
                            {
                                var paramNameAttribute = paramElement.Attribute("name");
                                var paramValueElement = paramElement.Value;
                                if (paramNameAttribute != null && !string.IsNullOrEmpty(paramNameAttribute.Value) && !string.IsNullOrEmpty(paramValueElement))
                                {
                                    var xmlMemberParam = new XmlMemberParam();

                                    xmlMemberParam.Name = paramNameAttribute.Value;
                                    xmlMemberParam.Value = paramValueElement;

                                    xmlMemberParamList.Add(xmlMemberParam);
                                }
                            }
                            if (xmlMemberParamList.Count > 0)
                            {
                                xmlMember.Param = xmlMemberParamList.ToArray();
                            }
                        }

                        var returns = member.Element("returns");
                        if (returns != null)
                        {
                            if (!string.IsNullOrEmpty(returns.Value))
                            {
                                xmlMember.Returns = returns.Value;
                            }
                        }

                        xmlMembers.Add(xmlMember);
                    }

                    returnValue.Members = xmlMembers.ToArray();
                }

                var x = 0;
            }

            return returnValue;
        }

        /// <summary>
        /// 根据namespace聚合
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        private static ObservableCollection<Node> Transform(Type[] types, XmlMember[] xmlMembers, TransConfig transConfig)
        {
            var returnValue = new ObservableCollection<Node>();

            if (types != null && types.Length > 0)
            {
                var groups = types.ToLookup(v => v.Namespace);

                foreach (var group in groups)
                {
                    var ns = new NamespaceNode { Name = group.Key };

                    ns.Items = new ObservableCollection<Node>();

                    //Class
                    var classes = group.Where(v => v.IsClass).ToArray();
                    var classNodes = LoadClasses(classes, xmlMembers, transConfig);
                    foreach (var item in classNodes)
                    {
                        ns.Items.Add(item);
                    }

                    returnValue.Add(ns);
                }
            }

            return returnValue;
        }

        private static List<ClassNode> LoadClasses(Type[] classes, XmlMember[] xmlMembers, TransConfig transConfig)
        {
            List<ClassNode> returnValue = new List<ClassNode>();

            foreach (var item in classes)
            {
                var classNode = new ClassNode();

                classNode.Name = item.Name;
                classNode.IsAbstract = item.IsAbstract;

                var classMember = xmlMembers != null ? xmlMembers.FirstOrDefault(v => string.Format("T:{0}", item.FullName).Equals(v.Name)) : null;
                if (classMember != null)
                {
                    classNode.Summary = classMember.Summary;
                }

                //BaseType
                if (item.BaseType != typeof(object))
                {
                    classNode.BaseTypeName = item.BaseType.Name;
                    classNode.BaseTypeFullName = item.BaseType.FullName;
                }

                //Properties
                var properties = item.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                var propertyNodes = LoadProperties(properties, xmlMembers);
                if (propertyNodes != null && propertyNodes.Count > 0)
                {
                    foreach (var propertyNode in propertyNodes)
                    {
                        classNode.Items.Add(propertyNode);
                    }
                }

                //Fields
                var fields = item.GetMembers().Where(v => v.MemberType == MemberTypes.Field && v.DeclaringType == item).Select(v => (FieldInfo)v).ToArray();
                var fieldNodes = LoadFields(fields, xmlMembers, transConfig);
                if (fieldNodes != null && fieldNodes.Count > 0)
                {
                    foreach (var fieldNode in fieldNodes)
                    {
                        classNode.Items.Add(fieldNode);
                    }
                }

                //Methods
                var methods = item.GetMembers()
                    .Where(v => v.MemberType == MemberTypes.Method)
                    .Where(v => v.DeclaringType == item)
                    .Select(v => (MethodInfo)v)
                    .Where(v => v.IsPublic && !v.IsSpecialName)
                    .ToArray();
                var methodNodes = LoadMethods(methods, xmlMembers);
                if (methodNodes != null && methodNodes.Count > 0)
                {
                    foreach (var methodNode in methodNodes)
                    {
                        classNode.Items.Add(methodNode);
                    }
                }

                returnValue.Add(classNode);
            }

            return returnValue;
        }

        private static List<PropertyNode> LoadProperties(PropertyInfo[] properties, XmlMember[] xmlMembers)
        {
            List<PropertyNode> returnValue = new List<PropertyNode>();

            if (properties != null && properties.Length > 0)
            {
                foreach (var property in properties)
                {
                    var propertyNode = new PropertyNode();

                    propertyNode.Name = property.Name;
                    propertyNode.CSharpTypeName = property.PropertyType.Name;
                    propertyNode.CSharpTypeFullName = property.PropertyType.FullName;
                    propertyNode.JavaTypeName = GetJavaType(property.PropertyType);
                    propertyNode.BjscTypeName = GetBaijiTypeName(property.PropertyType);
                    propertyNode.PropertyJavaName = string.Format("{0}{1}", property.Name[0].ToString().ToLower(), property.Name.Substring(1));

                    var getMethod = property.GetGetMethod();
                    if (getMethod != null)
                    {
                        var getMethodNodes = LoadMethods(new MethodInfo[] { getMethod }, null);

                        var methodNode = getMethodNodes[0];
                        methodNode.PropertyName = property.Name;
                        methodNode.PropertyJavaName = propertyNode.PropertyJavaName;
                        methodNode.IsGet = true;

                        propertyNode.Items.Add(methodNode);
                    }

                    var setMethod = property.GetSetMethod();
                    if (setMethod != null)
                    {
                        var setMethodNodes = LoadMethods(new MethodInfo[] { setMethod }, null);

                        var methodNode = setMethodNodes[0];
                        methodNode.PropertyName = property.Name;
                        methodNode.PropertyJavaName = propertyNode.PropertyJavaName;
                        methodNode.IsSet = true;

                        propertyNode.Items.Add(setMethodNodes[0]);
                    }

                    var propertyMember = xmlMembers != null ? xmlMembers.FirstOrDefault(v => string.Format("P:{0}.{1}", property.DeclaringType.FullName, property.Name).Equals(v.Name)) : null;
                    if (propertyMember != null)
                    {
                        propertyNode.Summary = propertyMember.Summary;
                    }

                    returnValue.Add(propertyNode);
                }
            }

            return returnValue;
        }

        private static List<FieldNode> LoadFields(FieldInfo[] fields, XmlMember[] xmlMembers, TransConfig transConfig)
        {
            List<FieldNode> returnValue = new List<FieldNode>();

            if (fields != null && fields.Length > 0)
            {
                foreach (var field in fields)
                {
                    var fieldNode = new FieldNode();

                    fieldNode.Name = field.Name;
                    fieldNode.CSharpTypeName = field.FieldType.Name;
                    fieldNode.CSharpTypeFullName = field.FieldType.FullName;
                    fieldNode.JavaTypeName = GetJavaType(field.FieldType);

                    if (!transConfig.ReflectionOnlyLoad && field.IsStatic)
                    {
                        var fieldValue = field.GetValue(null);
                        switch (field.FieldType.FullName)
                        {
                            case "System.Int32":
                                fieldNode.ConstValue = fieldValue.ToString();
                                break;
                            case "System.String":
                                fieldNode.ConstValue = string.Format("\"{0}\"", fieldValue);
                                break;
                            default:
                                break;
                        }
                    }

                    var fieldMember = xmlMembers != null ? xmlMembers.FirstOrDefault(v => string.Format("F:{0}.{1}", field.DeclaringType.FullName, field.Name).Equals(v.Name)) : null;
                    if (fieldMember != null)
                    {
                        fieldNode.Summary = fieldMember.Summary;
                    }

                    returnValue.Add(fieldNode);
                }
            }

            return returnValue;
        }

        private static List<MethodNode> LoadMethods(MethodInfo[] methods, XmlMember[] xmlMembers)
        {
            List<MethodNode> returnValue = new List<MethodNode>();

            if (methods != null && methods.Length > 0)
            {
                foreach (var method in methods)
                {
                    var methodModel = new MethodNode();

                    methodModel.Name = method.Name;
                    methodModel.ReturnCSharpTypeName = method.ReturnType.Name;
                    methodModel.ReturnCSharpTypeFullName = method.ReturnType.FullName;
                    methodModel.ReturnJavaTypeName = GetJavaType(method.ReturnType);
                    methodModel.IsStatic = method.IsStatic;
                    var parameters = method.GetParameters();
                    if (parameters.Length > 0)
                    {
                        methodModel.Parameters = new List<MethodParameter>();
                        foreach (var parameter in parameters)
                        {
                            var methodParameter = new MethodParameter();

                            methodParameter.Name = parameter.Name;
                            methodParameter.CSharpTypeName = parameter.ParameterType.Name;
                            methodParameter.CSharpTypeFullName = parameter.ParameterType.FullName;
                            methodParameter.JavaTypeName = GetJavaType(parameter.ParameterType);

                            methodModel.Parameters.Add(methodParameter);
                        }
                    }

                    var xmlMemberName = string.Format("M:{0}.{1}", method.DeclaringType.FullName, method.Name);
                    if (methodModel.Parameters != null && methodModel.Parameters.Count > 0)
                    {
                        xmlMemberName = string.Concat(xmlMemberName, "(", string.Join(",", methodModel.Parameters.Select(v => v.CSharpTypeFullName)), ")");
                    }
                    var methodMember = xmlMembers != null ? xmlMembers.FirstOrDefault(v => xmlMemberName.Equals(v.Name)) : null;
                    if (methodMember != null)
                    {
                        methodModel.Summary = methodMember.Summary;
                    }

                    returnValue.Add(methodModel);
                }
            }

            return returnValue;
        }

        private static List<Node> LoadStructs(Type[] structs)
        {
            List<Node> returnValue = new List<Node>();

            return returnValue;
        }

        private static string GetJavaType(Type type)
        {
            string typeName = type.Name;
            //string typeName = type.FullName;

            if (type.IsGenericType)
            {
                typeName = string.Format("{0}<{1}>",
                    type.Name.Split(new string[] { "`" }, StringSplitOptions.RemoveEmptyEntries)[0],
                    string.Join(", ", type.GetGenericArguments().Select(v => GetJavaType(v))));
            }
            else
            {
                switch (type.FullName)
                {
                    case "System.String":
                        typeName = "String";
                        break;
                    case "System.Int32":
                        typeName = "Integer";
                        break;
                    case "System.Int64":
                        typeName = "Long";
                        break;
                    case "System.Decimal":
                    case "System.Double":
                        typeName = "Double";
                        break;
                    case "System.Boolean":
                        typeName = "boolean";
                        break;
                    case "System.DateTime":
                        typeName = "Calendar";
                        break;
                    case "System.Void":
                        typeName = "void";
                        break;
                    default:
                        //typeName = type.FullName;
                        break;
                }
            }

            return typeName;
        }

        public static string GetBaijiTypeName(Type type)
        {
            string typeName = type.Name;

            if (type.IsGenericType)
            {
                typeName = string.Format("{0}<{1}>",
                    type.Name.Split(new string[] { "`" }, StringSplitOptions.RemoveEmptyEntries)[0].ToLower(),
                    string.Join(", ", type.GetGenericArguments().Select(v => GetBaijiTypeName(v))));
            }
            else
            {
                switch (type.FullName)
                {
                    case "System.Int32":
                        typeName = "int";
                        break;
                    case "System.Int64":
                        typeName = "long";
                        break;
                    case "System.String":
                        typeName = "string";
                        break;
                    case "System.Boolean":
                        typeName = "bool";
                        break;
                    case "System.DateTime":
                        typeName = "datetime";
                        break;
                    case "System.Single":
                    case "System.Decimal":
                    case "System.Double":
                        typeName = "double";
                        break;
                    default:
                        break;
                }
            }

            return typeName;
        }
    }
}
