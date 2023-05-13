#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableColor" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableColor : AssetReferenceT<ScriptableColor>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableColor" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableColor(string guid) : base(guid) { }
	}
}
#endif
