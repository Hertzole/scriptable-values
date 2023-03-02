#if AURORA_SV_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableQuaternion" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableQuaternion : AssetReferenceT<ScriptableQuaternion>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableQuaternion" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableQuaternion(string guid) : base(guid) { }
	}
}
#endif
