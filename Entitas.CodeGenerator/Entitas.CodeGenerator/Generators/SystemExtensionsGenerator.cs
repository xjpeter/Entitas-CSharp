using System;
using System.Collections.Generic;
using System.Linq;
using Entitas.CodeGenerator;

namespace Entitas.CodeGenerator {
    public class SystemExtensionsGenerator : ISystemCodeGenerator {

        const string CLASS_SUFFIX = "GeneratedExtension";

        public CodeGenFile[] Generate(Type[] systems) {
            return systems
                    .Where(type => type.GetConstructor(new Type[0]) != null)
                    .Aggregate(new List<CodeGenFile>(), (files, type) => {
                        var fileContent = new CSharpFileBuilder();
                        fileContent.AddNamespace("Entitas")
                            .AddClass("Pool").AddModifier(AccessModifiers.Public).AddModifier(Modifiers.Partial)
                                .AddMethod("Create" + type.Name, "return this.CreateSystem<" + type + ">();")
                                    .AddModifier(AccessModifiers.Public).SetReturnType(typeof(ISystem));

                        files.Add(new CodeGenFile {
                            fileName = type + CLASS_SUFFIX,
                            fileContent = fileContent.ToString().ToUnixLineEndings()
                        });

                        return files;
                    }).ToArray();
        }
    }
}
