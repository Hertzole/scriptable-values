#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableUShortEvent" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableUShortEvent : AssetReferenceT<ScriptableUShortEvent>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableUShortEvent" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableUShortEvent(string guid) : base(guid) { }
	}
}
#endif
