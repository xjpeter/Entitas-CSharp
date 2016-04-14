//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGenerator.ComponentExtensionsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Entitas;

namespace Entitas {
    public partial class Entity {
        public AgeComponent age { get { return (AgeComponent)GetComponent(BlueprintsComponentIds.Age); } }

        public bool hasAge { get { return HasComponent(BlueprintsComponentIds.Age); } }

        public Entity AddAge(int newValue) {
            var component = CreateComponent<AgeComponent>(BlueprintsComponentIds.Age);
            component.value = newValue;
            return AddComponent(BlueprintsComponentIds.Age, component);
        }

        public Entity ReplaceAge(int newValue) {
            var component = CreateComponent<AgeComponent>(BlueprintsComponentIds.Age);
            component.value = newValue;
            ReplaceComponent(BlueprintsComponentIds.Age, component);
            return this;
        }

        public Entity RemoveAge() {
            return RemoveComponent(BlueprintsComponentIds.Age);
        }
    }
}

    public partial class BlueprintsMatcher {
        static IMatcher _matcherAge;

        public static IMatcher Age {
            get {
                if (_matcherAge == null) {
                    var matcher = (Matcher)Matcher.AllOf(BlueprintsComponentIds.Age);
                    matcher.componentNames = BlueprintsComponentIds.componentNames;
                    _matcherAge = matcher;
                }

                return _matcherAge;
            }
        }
    }