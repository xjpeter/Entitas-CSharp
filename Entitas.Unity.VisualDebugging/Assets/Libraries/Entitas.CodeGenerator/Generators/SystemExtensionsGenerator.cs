using System;
using System.Linq;
using Entitas.CodeGenerator;

namespace Entitas.CodeGenerator {
    public class SystemExtensionsGenerator : ISystemCodeGenerator {

        public CodeGenFile[] Generate(Type[] systems) {
            var noTypes = new Type[0];
            return systems
                    .Where(type => type.GetConstructor(noTypes) != null)
                    .Select(type => {
                        var fileContent = new CSharpFileBuilder();
                        fileContent.AddNamespace("Entitas")
                            .AddClass("Pool").AddModifier(AccessModifiers.Public).AddModifier(Modifiers.Partial)
                                .AddMethod("Create" + type.Name, "return this.CreateSystem<" + type + ">();")
                                    .AddModifier(AccessModifiers.Public).SetReturnType(typeof(ISystem));

                        return new CodeGenFile {
                            fileName = type + CodeGenerator.CLASS_SUFFIX,
                            fileContent = fileContent.ToString().ToUnixLineEndings()
                        };
                    }).ToArray();
        }
    }
}
