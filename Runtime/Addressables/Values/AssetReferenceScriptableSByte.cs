#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableSByte" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableSByte : AssetReferenceT<ScriptableSByte>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableSByte" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableSByte(string guid) : base(guid) { }
	}
}
#endif
