#if SCRIPTABLE_VALUES_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     <see cref="ScriptableSByte" /> only asset reference.
	/// </summary>
	[Serializable]
	public sealed class AssetReferenceScriptableSByte : AssetReferenceT<ScriptableSByte>
	{
		/// <summary>
		///     Constructs a new reference to a <see cref="AssetReferenceScriptableSByte" />.
		/// </summary>
		/// <param name="guid">The object guid.</param>
#if UNITY_EDITOR || UNITY_INCLUDE_TESTS
		[UnityEngine.TestTools.ExcludeFromCoverage]
#endif
		public AssetReferenceScriptableSByte(string guid) : base(guid) { }
	}
}
#endif
