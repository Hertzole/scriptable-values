#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableVector2" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableVector2 : AssetReferenceT<ScriptableVector2>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableVector2" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableVector2(string guid) : base(guid) { }
	}
}
#endif
