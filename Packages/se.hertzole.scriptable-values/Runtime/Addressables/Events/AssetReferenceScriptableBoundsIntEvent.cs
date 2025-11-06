#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     <see cref="ScriptableBoundsIntEvent" /> only asset reference.
    /// </summary>
    [Serializable]
    public sealed class AssetReferenceScriptableBoundsIntEvent : AssetReferenceT<ScriptableBoundsIntEvent>
    {
        /// <summary>
        ///     Constructs a new reference to a <see cref="AssetReferenceScriptableBoundsIntEvent" />.
        /// </summary>
        /// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
        [ExcludeFromCoverage]
#endif
        public AssetReferenceScriptableBoundsIntEvent(string guid) : base(guid) { }
    }
}
#endif