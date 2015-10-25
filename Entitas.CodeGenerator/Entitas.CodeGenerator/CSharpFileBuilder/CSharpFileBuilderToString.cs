using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Entitas.CodeGenerator {
    public partial class CSharpFileBuilder {

        public override string ToString() {
            var usings = getUsings();
            var namespaces = getNamespaces();

            return usings + namespaces;
        }

        string getUsings() {
            if (_usings.Count == 0) {
                return string.Empty;
            }
        
            const string FORMAT = "using {0};\n";
            return _usings.Aggregate(string.Empty, (result, u) => 
                result + string.Format(FORMAT, u)) + "\n";
        }

        string getNamespaces() {
            const string FORMAT = @"namespace {0}";
            var namespaces = _namespaceDescriptions.Aggregate(new List<string>(), (list, nsd) => {
                var classes = getClasses(nsd);
                if (!string.IsNullOrEmpty(nsd.name)) {
                    classes = codeBlock(string.Format(FORMAT, nsd.name), classes);
                }
                if (!string.IsNullOrEmpty(classes)) {
                    list.Add(classes);
                }
                return list;
            }).ToArray();

            return string.Join("\n\n", namespaces);
        }

        static string getClasses(NamespaceDescription nsd) {
            const string FORMAT = @"{0}class {1}";
            var classes = nsd.classDescriptions.Aggregate(new List<string>(), (list, cd) => {
                var properties = getProperties(cd);
                var fields = getFields(cd);
                var methods = getMethods(cd);

                var body = properties;
                if (!string.IsNullOrEmpty(body) && !string.IsNullOrEmpty(fields)) {
                    body += "\n\n";
                }
                body += fields;

                if (!string.IsNullOrEmpty(body) && !string.IsNullOrEmpty(methods)) {
                    body += "\n\n";
                }
                body += methods;

                list.Add(codeBlock(string.Format(FORMAT, getModifiers(cd.modifiers), cd.name), body));
                return list;
            }).ToArray();

            return string.Join("\n\n", classes);
        }

        static string getProperties(ClassDescription cd) {
            if (cd.propertyDescriptions.Length == 0) {
                return string.Empty;
            }

            const string FORMAT_DEFAULT = @"{0}{1} {2} {{ get; set; }}";
            const string FORMAT = @"{0}{1} {2}";

            var properties = cd.propertyDescriptions.Aggregate(new List<string>(), (list, pd) => {
                var type = TypeGenerator.Generate(pd.type);
                if (!string.IsNullOrEmpty(pd.getter)) {
                    if (!string.IsNullOrEmpty(pd.setter)) {
                        list.Add(codeBlock(string.Format(FORMAT, getModifiers(pd.modifiers), type, pd.name),
                            codeBlock("get", pd.getter) + "\n" + codeBlock("set", pd.setter)));
                    } else {
                        list.Add(codeBlock(string.Format(FORMAT, getModifiers(pd.modifiers), type, pd.name),
                            codeBlock("get", pd.getter)));
                    }
                } else if (!string.IsNullOrEmpty(pd.setter)) {
                    list.Add(codeBlock(string.Format(FORMAT, getModifiers(pd.modifiers), type, pd.name),
                        codeBlock("set", pd.setter)));
                } else {
                    list.Add(string.Format(FORMAT_DEFAULT, getModifiers(pd.modifiers), type, pd.name));
                }

                return list;
            }).ToArray();

            return string.Join("\n", properties);
        }

        static string getFields(ClassDescription cd) {
            if (cd.fieldDescriptions.Length == 0) {
                return string.Empty;
            }

            const string FORMAT = @"{0}{1} {2};";
            const string FORMAT_WITH_VALUE = @"{0}{1} {2} = {3};";
            var fields = cd.fieldDescriptions.Aggregate(new List<string>(), (list, fd) => {
                var type = TypeGenerator.Generate(fd.type);
                if (string.IsNullOrEmpty(fd.value)) {
                    list.Add(string.Format(FORMAT, getModifiers(fd.modifiers), type, fd.name));
                } else {
                    list.Add(string.Format(FORMAT_WITH_VALUE, getModifiers(fd.modifiers), type, fd.name, fd.value));
                }

                return list;
            }).ToArray();

            return string.Join("\n", fields);
        }

        static string getMethods(ClassDescription cd) {
            if (cd.methodDescriptions.Length == 0) {
                return string.Empty;
            }

            const string FORMAT = @"{0}{1} {2}({3})";
            var methods = cd.methodDescriptions.Aggregate(new List<string>(), (list, md) => {
                var returnType = md.returnType == null
                    ? TypeGenerator.Generate(typeof(void))
                    : TypeGenerator.Generate(md.returnType);

                list.Add(codeBlock(string.Format(FORMAT, getModifiers(md.modifiers), returnType, md.name, getParameters(md)), md.body));
                return list;
            }).ToArray();

            return string.Join("\n\n", methods);
        }

        static string getParameters(MethodDescription md) {
            if (md.parameters == null || md.parameters.Length == 0) {
                return string.Empty;
            }

            const string FORMAT = @"{0}{1} {2}";
            const string FORMAT_DEFAULT_VALUE = @"{0}{1} {2} = {3}";
            var methods = md.parameters.Aggregate(new List<string>(), (list, p) => {
                var keyword = string.IsNullOrEmpty(p.keyword)
                    ? string.Empty
                    : p.keyword + " ";

                if (string.IsNullOrEmpty(p.defaultValue)) {
                    list.Add(string.Format(FORMAT, keyword, TypeGenerator.Generate(p.type), p.name));
                } else {
                    list.Add(string.Format(FORMAT_DEFAULT_VALUE, keyword, TypeGenerator.Generate(p.type), p.name, p.defaultValue));
                }
                
                return list;
            }).ToArray();

            return string.Join(", ", methods);
        }

        static string codeBlock(string name, string body) {
            const string EMPTY_BLOCK_FORMAT = "{0} {{\n}}";
            const string BLOCK_FORMAT = "{0} {{\n{1}\n}}";
            if (!string.IsNullOrEmpty(body)) {
                body = indentNewLines(body, 1);
            }

            return string.IsNullOrEmpty(body)
                ? string.Format(EMPTY_BLOCK_FORMAT, name)
                : string.Format(BLOCK_FORMAT, name, body);
        }

        static string indentNewLines(string text, int indentLevel) {
            const string INDENT = "    ";

            var indentedText = new StringBuilder();
            using (var reader = new StringReader(text)) {
                var line = reader.ReadLine();
                while (line != null) {
                    var aLine = line;
                    line = reader.ReadLine();

                    if (aLine != string.Empty) {
                        indentedText.Append(INDENT);
                        indentedText.Append(aLine);
                    }

                    if (line != null) {
                        indentedText.Append("\n");
                    }
                }
            }

            return indentedText.ToString();
        }

        static string getModifiers(string[] modifiers) {
            return modifiers == null
                ? string.Empty
                : modifiers.Aggregate(string.Empty, (result, am) => result + am + " ");
        }
    }
}