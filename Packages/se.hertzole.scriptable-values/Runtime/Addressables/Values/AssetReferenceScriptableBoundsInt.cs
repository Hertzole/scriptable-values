#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableBoundsInt" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableBoundsInt : AssetReferenceT<ScriptableBoundsInt>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableBoundsInt" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableBoundsInt(string guid) : base(guid) { }
	}
}
#endif
