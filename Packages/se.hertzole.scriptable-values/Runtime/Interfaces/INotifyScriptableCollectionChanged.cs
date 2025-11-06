#nullable enable

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     Interface for objects that can be notified when a collection changes.
    /// </summary>
    /// <typeparam name="T">The type in the collection.</typeparam>
    public interface INotifyScriptableCollectionChanged<T>
    {
        event CollectionChangedEventHandler<T>? OnCollectionChanged;
    }
}