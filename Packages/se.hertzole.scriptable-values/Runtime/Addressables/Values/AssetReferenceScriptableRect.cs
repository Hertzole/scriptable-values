#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableRect" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableRect : AssetReferenceT<ScriptableRect>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableRect" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableRect(string guid) : base(guid) { }
	}
}
#endif
