using System.Linq;
using Entitas.CodeGenerator;

namespace Entitas.CodeGenerator {
    public class PoolsGenerator : IPoolCodeGenerator {

        const string ALL_POOLS_GETTER = @"if (_allPools == null) {{
    _allPools = new [] {{ {0} }};
}}

return _allPools;";

        const string POOL_GETTER = @"if (_{0} == null) {{
    _{0} = new Pool({1}" + CodeGenerator.DEFAULT_INDICES_LOOKUP_TAG + @".TotalComponents);
    #if (UNITY_EDITOR)
    var poolObserver = new Entitas.Unity.VisualDebugging.PoolObserver(_{0}, {1}" + CodeGenerator.DEFAULT_INDICES_LOOKUP_TAG + @".componentNames, {1}" + CodeGenerator.DEFAULT_INDICES_LOOKUP_TAG + @".componentTypes, ""{2}Pool"");
    UnityEngine.Object.DontDestroyOnLoad(poolObserver.entitiesContainer);
    #endif
}}

return _{0};";

        public CodeGenFile[] Generate(string[] poolNames) {
            const string DEFAULT_POOL_NAME = "pool";
            var hasPools = poolNames == null || poolNames.Length == 0;

            var fileContent = new CSharpFileBuilder();
            var cd = fileContent.NoNamespace().AddClass("Pools").AddModifier(AccessModifiers.Public).AddModifier(Modifiers.Static);

            var allPoolNames = hasPools
                ? DEFAULT_POOL_NAME
                : string.Join(", ", poolNames.Select(poolName => poolName.LowercaseFirst()).ToArray());

            cd.AddProperty(typeof(Pool[]), "allPools").AddModifier(AccessModifiers.Public).AddModifier(Modifiers.Static)
                .SetGetter(string.Format(ALL_POOLS_GETTER, allPoolNames));
            cd.AddField(typeof(Pool[]), "_allPools").AddModifier(Modifiers.Static);

            if (hasPools) {
                cd.AddProperty(typeof(Pool), DEFAULT_POOL_NAME).AddModifier(AccessModifiers.Public).AddModifier(Modifiers.Static)
                    .SetGetter(string.Format(POOL_GETTER, DEFAULT_POOL_NAME, string.Empty, string.Empty));
                cd.AddField(typeof(Pool), "_" + DEFAULT_POOL_NAME).AddModifier(Modifiers.Static);
            } else {
                foreach (var poolName in poolNames) {
                    var lowerPoolName = poolName.LowercaseFirst();
                    cd.AddProperty(typeof(Pool), lowerPoolName).AddModifier(AccessModifiers.Public).AddModifier(Modifiers.Static)
                        .SetGetter(string.Format(POOL_GETTER, lowerPoolName, poolName, poolName + " "));
                    cd.AddField(typeof(Pool), "_" + lowerPoolName).AddModifier(Modifiers.Static);
                }
            }

            return new [] { new CodeGenFile {
                    fileName = "Pools",
                    fileContent = fileContent.ToString().ToUnixLineEndings()
                }
            };
        }
    }
}