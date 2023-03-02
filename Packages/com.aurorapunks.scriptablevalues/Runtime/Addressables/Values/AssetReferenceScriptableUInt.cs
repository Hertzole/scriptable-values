#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
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
		public AssetReferenceScriptableUInt(string guid) : base(guid) { }
	}
}
#endif
