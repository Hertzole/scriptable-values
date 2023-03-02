#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableSByteEvent" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableSByteEvent : AssetReferenceT<ScriptableSByteEvent>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableSByteEvent" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableSByteEvent(string guid) : base(guid) { }
	}
}
#endif
