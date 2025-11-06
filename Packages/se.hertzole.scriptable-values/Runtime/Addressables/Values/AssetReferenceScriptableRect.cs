#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     <see cref="ScriptableRect" /> only asset reference.
    /// </summary>
    [Serializable]
    public sealed class AssetReferenceScriptableRect : AssetReferenceT<ScriptableRect>
    {
        /// <summary>
        ///     Constructs a new reference to a <see cref="AssetReferenceScriptableRect" />.
        /// </summary>
        /// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
        [ExcludeFromCoverage]
#endif
        public AssetReferenceScriptableRect(string guid) : base(guid) { }
    }
}
#endif