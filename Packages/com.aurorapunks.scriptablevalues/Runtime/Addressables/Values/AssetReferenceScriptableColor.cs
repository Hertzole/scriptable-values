#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableColor" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableColor : AssetReferenceT<ScriptableColor>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableColor" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableColor(string guid) : base(guid) { }
	}
}
#endif
