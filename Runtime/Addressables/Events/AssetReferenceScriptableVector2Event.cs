#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableVector2Event" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableVector2Event : AssetReferenceT<ScriptableVector2Event>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableVector2Event" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableVector2Event(string guid) : base(guid) { }
	}
}
#endif
