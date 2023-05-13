#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableEvent" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableEvent : AssetReferenceT<ScriptableEvent>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableEvent" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableEvent(string guid) : base(guid) { }
	}
}
#endif
