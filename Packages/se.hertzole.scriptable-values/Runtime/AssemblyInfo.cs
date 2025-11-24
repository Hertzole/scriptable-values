using System.Runtime.CompilerServices;
#if SCRIPTABLE_VALUES_PROPERTIES
using Unity.Properties;
#endif // SCRIPTABLE_VALUES_PROPERTIES

#if UNITY_EDITOR
[assembly: InternalsVisibleTo("Hertzole.ScriptableValues.Editor")]
#endif // UNITY_EDITOR
#if UNITY_INCLUDE_TESTS
[assembly: InternalsVisibleTo("Hertzole.ScriptableValues.Tests")]
[assembly: InternalsVisibleTo("Hertzole.ScriptableValues.Tests.Editor")]
#endif // UNITY_INCLUDE_TESTS

#if SCRIPTABLE_VALUES_PROPERTIES
[assembly: GeneratePropertyBagsForAssembly]
#endif // SCRIPTABLE_VALUES_PROPERTIES