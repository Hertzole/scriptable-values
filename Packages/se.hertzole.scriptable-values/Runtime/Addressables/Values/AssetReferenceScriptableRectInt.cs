#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     <see cref="ScriptableRectInt" /> only asset reference.
    /// </summary>
    [Serializable]
    public sealed class AssetReferenceScriptableRectInt : AssetReferenceT<ScriptableRectInt>
    {
        /// <summary>
        ///     Constructs a new reference to a <see cref="AssetReferenceScriptableRectInt" />.
        /// </summary>
        /// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
        [ExcludeFromCoverage]
#endif
        public AssetReferenceScriptableRectInt(string guid) : base(guid) { }
    }
}
#endif