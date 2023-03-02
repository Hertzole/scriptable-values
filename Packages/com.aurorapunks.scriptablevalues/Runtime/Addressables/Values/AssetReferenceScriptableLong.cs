#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableLong" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableLong : AssetReferenceT<ScriptableLong>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableLong" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableLong(string guid) : base(guid) { }
	}
}
#endif
