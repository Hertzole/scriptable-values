#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableByte" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableByte : AssetReferenceT<ScriptableByte>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableByte" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableByte(string guid) : base(guid) { }
	}
}
#endif
