#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableColor32" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableColor32 : AssetReferenceT<ScriptableColor32>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableColor32" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableColor32(string guid) : base(guid) { }
	}
}
#endif
