#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableBool" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableBool : AssetReferenceT<ScriptableBool>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableBool" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableBool(string guid) : base(guid) { }
	}
}
#endif
