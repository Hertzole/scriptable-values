#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableUShort" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableUShort : AssetReferenceT<ScriptableUShort>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableUShort" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableUShort(string guid) : base(guid) { }
	}
}
#endif
