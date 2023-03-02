#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
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
		public AssetReferenceScriptableUShort(string guid) : base(guid) { }
	}
}
#endif
