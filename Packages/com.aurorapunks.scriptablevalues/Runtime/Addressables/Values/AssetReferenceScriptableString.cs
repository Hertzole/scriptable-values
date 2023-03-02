#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
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
		public AssetReferenceScriptableString(string guid) : base(guid) { }
	}
}
#endif
