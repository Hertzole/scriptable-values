#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableRectInt" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableRectInt : AssetReferenceT<ScriptableRectInt>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableRectInt" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableRectInt(string guid) : base(guid) { }
	}
}
#endif
