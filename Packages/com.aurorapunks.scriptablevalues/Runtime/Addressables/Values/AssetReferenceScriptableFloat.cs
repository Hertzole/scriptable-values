#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableFloat" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableFloat : AssetReferenceT<ScriptableFloat>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableFloat" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableFloat(string guid) : base(guid) { }
	}
}
#endif
