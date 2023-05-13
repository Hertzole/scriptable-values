#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableString" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableString : AssetReferenceT<ScriptableString>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableString" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableString(string guid) : base(guid) { }
	}
}
#endif
