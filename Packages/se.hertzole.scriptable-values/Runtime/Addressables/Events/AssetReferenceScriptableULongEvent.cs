#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     <see cref="ScriptableULongEvent" /> only asset reference.
    /// </summary>
    [Serializable]
    public sealed class AssetReferenceScriptableULongEvent : AssetReferenceT<ScriptableULongEvent>
    {
        /// <summary>
        ///     Constructs a new reference to a <see cref="AssetReferenceScriptableULongEvent" />.
        /// </summary>
        /// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
        [ExcludeFromCoverage]
#endif
        public AssetReferenceScriptableULongEvent(string guid) : base(guid) { }
    }
}
#endif