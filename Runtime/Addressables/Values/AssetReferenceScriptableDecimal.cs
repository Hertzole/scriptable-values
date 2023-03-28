#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableDecimal" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableDecimal : AssetReferenceT<ScriptableDecimal>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableDecimal" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableDecimal(string guid) : base(guid) { }
	}
}
#endif
