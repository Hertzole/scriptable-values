#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableColor32Event" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableColor32Event : AssetReferenceT<ScriptableColor32Event>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableColor32Event" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableColor32Event(string guid) : base(guid) { }
	}
}
#endif
