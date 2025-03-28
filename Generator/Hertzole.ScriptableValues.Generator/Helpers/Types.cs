namespace Hertzole.ScriptableValues.Generator;

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
	public const string MARKER_ATTRIBUTE = "Hertzole.ScriptableValues.GenerateScriptableCallbacksAttribute";
	public const string GLOBAL_MARKER_ATTRIBUTE = "global::" + MARKER_ATTRIBUTE;
	public const string GENERATE_VALUE_CALLBACK_ATTRIBUTE = "Hertzole.ScriptableValues.GenerateValueCallbackAttribute";
	public const string GLOBAL_GENERATE_VALUE_CALLBACK_ATTRIBUTE = "global::" + GENERATE_VALUE_CALLBACK_ATTRIBUTE;
	public const string GENERATE_EVENT_CALLBACK_ATTRIBUTE = "Hertzole.ScriptableValues.GenerateEventCallbackAttribute";
	public const string GLOBAL_GENERATE_EVENT_CALLBACK_ATTRIBUTE = "global::" + GENERATE_EVENT_CALLBACK_ATTRIBUTE;
	public const string GENERATE_POOL_CALLBACK_ATTRIBUTE = "Hertzole.ScriptableValues.GeneratePoolCallbackAttribute";
	public const string GLOBAL_GENERATE_POOL_CALLBACK_ATTRIBUTE = "global::" + GENERATE_POOL_CALLBACK_ATTRIBUTE;
	public const string GENERATE_COLLECTION_CALLBACK_ATTRIBUTE = "Hertzole.ScriptableValues.GenerateCollectionCallbackAttribute";
	public const string GLOBAL_GENERATE_COLLECTION_CALLBACK_ATTRIBUTE = "global::" + GENERATE_COLLECTION_CALLBACK_ATTRIBUTE;
}