namespace Hertzole.ScriptableValues
{
    internal interface IScriptableValueCallbacks
    {
        void OnScriptableObjectPreEnable();

        void OnScriptableObjectEnable();

        void OnScriptableObjectPreDisable();

        void OnScriptableObjectDisable();
    }
}