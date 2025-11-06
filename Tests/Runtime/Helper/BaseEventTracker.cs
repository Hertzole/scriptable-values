using System;

namespace Hertzole.ScriptableValues.Tests
{
    public abstract class BaseEventTracker : IDisposable
    {
        public abstract bool HasBeenInvoked();

        public virtual void Dispose() { }
    }
}