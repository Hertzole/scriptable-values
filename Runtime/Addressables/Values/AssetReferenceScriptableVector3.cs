#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableVector3" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableVector3 : AssetReferenceT<ScriptableVector3>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableVector3" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableVector3(string guid) : base(guid) { }
	}
}
#endif
