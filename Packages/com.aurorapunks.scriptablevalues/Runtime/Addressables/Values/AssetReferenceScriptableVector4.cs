#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableVector4" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableVector4 : AssetReferenceT<ScriptableVector4>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableVector4" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableVector4(string guid) : base(guid) { }
	}
}
#endif
