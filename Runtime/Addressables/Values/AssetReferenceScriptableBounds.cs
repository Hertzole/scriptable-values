#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableBounds" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableBounds : AssetReferenceT<ScriptableBounds>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableBounds" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableBounds(string guid) : base(guid) { }
	}
}
#endif
