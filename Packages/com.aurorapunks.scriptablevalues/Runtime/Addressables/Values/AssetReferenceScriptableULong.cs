#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
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
		public AssetReferenceScriptableULong(string guid) : base(guid) { }
	}
}
#endif
