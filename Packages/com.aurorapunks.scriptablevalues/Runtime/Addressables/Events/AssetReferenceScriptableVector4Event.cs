#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableVector4Event" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableVector4Event : AssetReferenceT<ScriptableVector4Event>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableVector4Event" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableVector4Event(string guid) : base(guid) { }
	}
}
#endif
