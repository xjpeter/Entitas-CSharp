using System;
using System.Collections.Generic;

namespace Entitas.CodeGenerator {
    public static class AccessModifiers {
        public const string Public = "public";
        public const string Private = "private";
        public const string Internal = "internal";
        public const string Protected = "protected";
    }

    public static class Modifiers {
        public const string Abstract = "abstract";
        public const string Async = "async";
        public const string Const = "const";
        public const string Event = "event";
        public const string Extern = "extern";
        public const string New = "new";
        public const string Override = "override";
        public const string Partial = "partial";
        public const string Readonly = "readonly";
        public const string Sealed = "sealed";
        public const string Static = "static";
        public const string Unsafe = "unsafe";
        public const string Virtual = "virtual";
        public const string Volatile = "volatile";
    }

    public static class MethodParameterKeyword {
        public const string Params = "params";
        public const string Ref = "ref";
        public const string Out = "out";
    }

    public class NamespaceDescription {
        public string name { get { return _name; } }
        public ClassDescription[] classDescriptions { get { return _classDescriptions.ToArray(); } }

        readonly string _name;
        readonly List<ClassDescription> _classDescriptions;

        public NamespaceDescription(string name) {
            _name = name;
            _classDescriptions = new List<ClassDescription>();
        }

        public ClassDescription AddClass(string name) {
            var classDescription = new ClassDescription(name);
            _classDescriptions.Add(classDescription);
            return classDescription;
        }
    }

    public class ClassDescription {
        public string name { get { return _name; } }
        public string[] modifiers { get { return _modifiers.ToArray(); } }
        public Type baseClass { get { return _baseClass; } }
        public List<Type> interfaces { get { return _interfaces; } }
        public ConstructorDescription[] constructorDescriptions { get { return _constructorDescriptions.ToArray(); } }
        public PropertyDescription[] propertyDescriptions { get { return _propertyDescriptions.ToArray(); } }
        public FieldDescription[] fieldDescriptions { get { return _fieldDescriptions.ToArray(); } }
        public MethodDescription[] methodDescriptions { get { return _methodDescriptions.ToArray(); } }

        readonly string _name;
        readonly List<string> _modifiers;
        Type _baseClass;
        readonly List<Type> _interfaces;
        readonly List<ConstructorDescription> _constructorDescriptions;
        readonly List<PropertyDescription> _propertyDescriptions;
        readonly List<FieldDescription> _fieldDescriptions;
        readonly List<MethodDescription> _methodDescriptions;

        public ClassDescription(string name) {
            _name = name;
            _modifiers = new List<string>();
            _interfaces = new List<Type>();
            _constructorDescriptions = new List<ConstructorDescription>();
            _propertyDescriptions = new List<PropertyDescription>();
            _fieldDescriptions = new List<FieldDescription>();
            _methodDescriptions = new List<MethodDescription>();
        }

        public ClassDescription AddModifier(string modifier) {
            _modifiers.Add(modifier);
            return this;
        }

        public ClassDescription SetBaseClass(Type type) {
            _baseClass = type;
            return this;
        }

        public ClassDescription AddInterface(Type type) {
            _interfaces.Add(type);
            return this;
        }

        public ConstructorDescription AddConstructor(string body = "") {
            var constructorDescription = new ConstructorDescription(_name, body);
            _constructorDescriptions.Add(constructorDescription);
            return constructorDescription;
        }

        public PropertyDescription AddProperty(Type type, string name) {
            var propertyDescription = new PropertyDescription(type, name);
            _propertyDescriptions.Add(propertyDescription);
            return propertyDescription;
        }

        public FieldDescription AddField(Type type, string name) {
            var fieldDescription = new FieldDescription(type, name);
            _fieldDescriptions.Add(fieldDescription);
            return fieldDescription;
        }

        public MethodDescription AddMethod(string name, string body) {
            var methodDescription = new MethodDescription(name, body);
            _methodDescriptions.Add(methodDescription);
            return methodDescription;
        }
    }

    public class ConstructorDescription {
        public string name { get { return _name; } }
        public string body { get { return _body; } }
        public string[] modifiers { get { return _modifiers.ToArray(); } }
        public MethodParameterDescription[] parameters { get { return _parameters.ToArray(); } }
        public string baseCall { get { return _baseCall; } }

        readonly string _name;
        readonly string _body;
        readonly List<string> _modifiers;
        readonly List<MethodParameterDescription> _parameters;
        string _baseCall;

        public ConstructorDescription(string name, string body) {
            _name = name;
            _body = body;
            _modifiers = new List<string>();
            _parameters = new List<MethodParameterDescription>();
        }

        public ConstructorDescription AddModifier(string modifier) {
            _modifiers.Add(modifier);
            return this;
        }

        public ConstructorDescription AddParameter(MethodParameterDescription parameter) {
            _parameters.Add(parameter);
            return this;
        }

        public void CallBase(string arguments) {
            _baseCall = arguments;
        }
    }

    public class PropertyDescription {
        public Type type { get { return _type; } }
        public string name { get { return _name; } }
        public string[] modifiers { get { return _modifiers.ToArray(); } }
        public string getter { get { return _getter; } }
        public string setter { get { return _setter; } }

        readonly Type _type;
        readonly string _name;
        readonly List<string> _modifiers;
        string _getter;
        string _setter;

        public PropertyDescription(Type type, string name) {
            _type = type;
            _name = name;
            _modifiers = new List<string>();
        }

        public PropertyDescription AddModifier(string modifier) {
            _modifiers.Add(modifier);
            return this;
        }

        public PropertyDescription SetGetter(string body) {
            _getter = body;
            return this;
        }

        public PropertyDescription SetSetter(string body) {
            _setter = body;
            return this;
        }
    }

    public class FieldDescription {
        public Type type { get { return _type; } }
        public string name { get { return _name; } }
        public string[] modifiers { get { return _modifiers.ToArray(); } }
        public string value { get { return _value; } }

        readonly Type _type;
        readonly string _name;
        readonly List<string> _modifiers;
        string _value;

        public FieldDescription(Type type, string name) {
            _type = type;
            _name = name;
            _modifiers = new List<string>();
        }

        public FieldDescription AddModifier(string modifier) {
            _modifiers.Add(modifier);
            return this;
        }

        public FieldDescription SetValue(string value) {
            _value = value;
            return this;
        }
    }

    public class MethodDescription {
        public string name { get { return _name; } }
        public string body { get { return _body; } }
        public string[] modifiers { get { return _modifiers.ToArray(); } }
        public Type returnType { get { return _returnType; } }
        public MethodParameterDescription[] parameters { get { return _parameters.ToArray(); } }

        readonly string _name;
        readonly string _body;
        readonly List<string> _modifiers;
        Type _returnType;
        readonly List<MethodParameterDescription> _parameters;

        public MethodDescription(string name, string body) {
            _name = name;
            _body = body;
            _modifiers = new List<string>();
            _parameters = new List<MethodParameterDescription>();
        }

        public MethodDescription AddModifier(string modifier) {
            _modifiers.Add(modifier);
            return this;
        }

        public MethodDescription SetReturnType(Type type) {
            _returnType = type;
            return this;
        }

        public MethodDescription AddParameter(MethodParameterDescription parameter) {
            _parameters.Add(parameter);
            return this;
        }
    }

    public class MethodParameterDescription {
        public Type type { get { return _type; } }
        public string name { get { return _name; } }
        public string keyword { get { return _keyword; } }
        public string defaultValue { get { return _defaultValue; } }

        readonly Type _type;
        readonly string _name;
        string _keyword;
        string _defaultValue;

        public MethodParameterDescription(Type type, string name) {
            _type = type;
            _name = name;
        }

        public MethodParameterDescription SetKeyword(string keyword) {
            _keyword = keyword;
            return this;
        }

        public MethodParameterDescription SetDefaultValue(string defaultValue) {
            _defaultValue = defaultValue;
            return this;
        }
    }
}