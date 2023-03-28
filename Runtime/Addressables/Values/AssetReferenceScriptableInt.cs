#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableInt" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableInt : AssetReferenceT<ScriptableInt>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableInt" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableInt(string guid) : base(guid) { }
	}
}
#endif
