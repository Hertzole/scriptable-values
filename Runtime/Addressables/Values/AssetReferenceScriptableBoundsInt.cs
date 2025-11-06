#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     <see cref="ScriptableBoundsInt" /> only asset reference.
    /// </summary>
    [Serializable]
    public sealed class AssetReferenceScriptableBoundsInt : AssetReferenceT<ScriptableBoundsInt>
    {
        /// <summary>
        ///     Constructs a new reference to a <see cref="AssetReferenceScriptableBoundsInt" />.
        /// </summary>
        /// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
        [ExcludeFromCoverage]
#endif
        public AssetReferenceScriptableBoundsInt(string guid) : base(guid) { }
    }
}
#endif