﻿namespace Hertzole.ScriptableValues.Generator;

internal static class Types
{
	public const string ASSET_REFERENCE_T = "global::UnityEngine.AddressableAssets.AssetReferenceT<TObject>";
	public const string SCRIPTABLE_VALUE = "global::Hertzole.ScriptableValues.ScriptableValue<T>";
	public const string SCRIPTABLE_EVENT = "global::Hertzole.ScriptableValues.ScriptableEvent";
	public const string GENERIC_SCRIPTABLE_EVENT = "global::Hertzole.ScriptableValues.ScriptableEvent<T>";
	public const string SCRIPTABLE_POOL = "global::Hertzole.ScriptableValues.ScriptablePool<T>";
	public const string SCRIPTABLE_LIST = "global::Hertzole.ScriptableValues.ScriptableList<T>";
	public const string SCRIPTABLE_DICTIONARY = "global::Hertzole.ScriptableValues.ScriptableDictionary<TKey, TValue>";

	// Attributes
	public const string GLOBAL_MARKER_ATTRIBUTE = "global::" + MARKER_ATTRIBUTE;
	public const string MARKER_ATTRIBUTE = "Hertzole.ScriptableValues.GenerateScriptableCallbacksAttribute";
}