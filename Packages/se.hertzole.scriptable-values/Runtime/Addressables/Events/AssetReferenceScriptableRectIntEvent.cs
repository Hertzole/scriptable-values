#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableRectIntEvent" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableRectIntEvent : AssetReferenceT<ScriptableRectIntEvent>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableRectIntEvent" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableRectIntEvent(string guid) : base(guid) { }
	}
}
#endif
