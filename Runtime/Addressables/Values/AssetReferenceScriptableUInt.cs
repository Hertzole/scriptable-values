#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableUInt" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableUInt : AssetReferenceT<ScriptableUInt>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableUInt" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableUInt(string guid) : base(guid) { }
	}
}
#endif
