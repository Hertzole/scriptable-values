#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableULong" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableULong : AssetReferenceT<ScriptableULong>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableULong" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableULong(string guid) : base(guid) { }
	}
}
#endif
