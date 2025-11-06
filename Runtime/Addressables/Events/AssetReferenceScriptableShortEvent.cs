#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     <see cref="ScriptableShortEvent" /> only asset reference.
    /// </summary>
    [Serializable]
    public sealed class AssetReferenceScriptableShortEvent : AssetReferenceT<ScriptableShortEvent>
    {
        /// <summary>
        ///     Constructs a new reference to a <see cref="AssetReferenceScriptableShortEvent" />.
        /// </summary>
        /// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
        [ExcludeFromCoverage]
#endif
        public AssetReferenceScriptableShortEvent(string guid) : base(guid) { }
    }
}
#endif