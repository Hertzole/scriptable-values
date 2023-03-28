#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableLongEvent" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableLongEvent : AssetReferenceT<ScriptableLongEvent>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableLongEvent" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableLongEvent(string guid) : base(guid) { }
	}
}
#endif
