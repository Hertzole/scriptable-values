#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableBoundsEvent" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableBoundsEvent : AssetReferenceT<ScriptableBoundsEvent>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableBoundsEvent" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableBoundsEvent(string guid) : base(guid) { }
	}
}
#endif
