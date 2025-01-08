using System.Runtime.CompilerServices;
#if SCRIPTABLE_VALUES_PROPERTIES
using Unity.Properties;
#endif

[assembly: InternalsVisibleTo("Hertzole.ScriptableValues.Editor")]
[assembly: InternalsVisibleTo("Hertzole.ScriptableValues.Tests")]
[assembly: InternalsVisibleTo("Hertzole.ScriptableValues.Tests.Editor")]

#if SCRIPTABLE_VALUES_PROPERTIES
[assembly: GeneratePropertyBagsForAssembly]
#endif