#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableVector2Event" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableVector2Event : AssetReferenceT<ScriptableVector2Event>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableVector2Event" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableVector2Event(string guid) : base(guid) { }
	}
}
#endif
