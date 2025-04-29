#nullable enable

namespace Hertzole.ScriptableValues
{
	public interface INotifyScriptableCollectionChanged<T>
	{
		event CollectionChangedEventHandler<T>? OnCollectionChanged;
	}
}