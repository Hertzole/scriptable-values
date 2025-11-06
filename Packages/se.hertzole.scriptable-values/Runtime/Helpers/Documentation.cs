#if UNITY_EDITOR
namespace Hertzole.ScriptableValues
{
    internal static class Documentation
    {
        private const string BASE_URL = "https://hertzole.github.io/scriptable-values/";
        private const string TYPE_URL = BASE_URL + "types/";
        private const string GUIDES_URL = BASE_URL + "guides/";

        public const string SCRIPTABLE_VALUE_URL = TYPE_URL + "scriptable-value";
        public const string SCRIPTABLE_EVENT_URL = TYPE_URL + "scriptable-event";
        public const string SCRIPTABLE_EVENT_GENERIC_URL = SCRIPTABLE_EVENT_URL;
        public const string SCRIPTABLE_POOL_URL = TYPE_URL + "scriptable-pool";
        public const string SCRIPTABLE_LIST_URL = TYPE_URL + "scriptable-list";
        public const string SCRIPTABLE_DICTIONARY_URL = TYPE_URL + "scriptable-dictionary";

        public const string CREATING_CUSTOM_TYPES_URL = GUIDES_URL + "creating-custom-types";
    }
}
#endif