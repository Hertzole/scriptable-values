#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableChar" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableChar : AssetReferenceT<ScriptableChar>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableChar" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableChar(string guid) : base(guid) { }
	}
}
#endif
