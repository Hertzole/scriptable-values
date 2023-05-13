#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableDouble" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableDouble : AssetReferenceT<ScriptableDouble>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableDouble" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableDouble(string guid) : base(guid) { }
	}
}
#endif
