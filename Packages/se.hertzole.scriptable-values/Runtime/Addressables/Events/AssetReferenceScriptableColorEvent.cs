#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableColorEvent" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableColorEvent : AssetReferenceT<ScriptableColorEvent>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableColorEvent" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
		public AssetReferenceScriptableColorEvent(string guid) : base(guid) { }
	}
}
#endif
