using System.Collections.Generic;
using System.Linq;

namespace Entitas.CodeGenerator {
    public class PoolAttributeGenerator : IPoolCodeGenerator {

        public CodeGenFile[] Generate(string[] poolNames) {
            return poolNames.Aggregate(new List<CodeGenFile>(), (files, poolName) => {
                files.Add(new CodeGenFile {
                    fileName = poolName + "Attribute",
                    fileContent = generatePoolAttributes(poolName).ToUnixLineEndings()
                });
                return files;
            }).ToArray();
        }

        static string generatePoolAttributes(string poolName) {
            var fileContent = new CSharpFileBuilder();
            var className = poolName + "Attribute";
            fileContent.NoNamespace()
                .AddClass(className).AddModifier(AccessModifiers.Public)
                    .SetBaseClass(typeof(PoolAttribute))
                    .AddConstructor()
                        .AddModifier(AccessModifiers.Public)
                        .CallBase("\"" + poolName + "\"");

            return fileContent.ToString();
        }
    }
}

