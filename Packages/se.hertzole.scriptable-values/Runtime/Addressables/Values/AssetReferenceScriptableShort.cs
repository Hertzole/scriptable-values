#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableShort" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableShort : AssetReferenceT<ScriptableShort>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableShort" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableShort(string guid) : base(guid) { }
	}
}
#endif
