namespace Hertzole.ScriptableValues.Tests
{
    public class CallbackObject : RuntimeScriptableObject
    {
        public bool onPreStartCalled;
        public bool onStartCalled;
        public bool onPreDisabledCalled;
        public bool onDisabledCalled;

        /// <inheritdoc />
        protected override void OnPreStart()
        {
            onPreStartCalled = true;
        }

        /// <inheritdoc />
        protected override void OnStart()
        {
            onStartCalled = true;
        }

        /// <inheritdoc />
        protected override void OnPreDisabled()
        {
            onPreDisabledCalled = true;
        }

        /// <inheritdoc />
        protected override void OnDisabled()
        {
            onDisabledCalled = true;
        }
    }
}