#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
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
		public AssetReferenceScriptableChar(string guid) : base(guid) { }
	}
}
#endif
