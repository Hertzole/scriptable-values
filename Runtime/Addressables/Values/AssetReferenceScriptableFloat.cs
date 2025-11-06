#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     <see cref="ScriptableFloat" /> only asset reference.
    /// </summary>
    [Serializable]
    public sealed class AssetReferenceScriptableFloat : AssetReferenceT<ScriptableFloat>
    {
        /// <summary>
        ///     Constructs a new reference to a <see cref="AssetReferenceScriptableFloat" />.
        /// </summary>
        /// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
        [ExcludeFromCoverage]
#endif
        public AssetReferenceScriptableFloat(string guid) : base(guid) { }
    }
}
#endif