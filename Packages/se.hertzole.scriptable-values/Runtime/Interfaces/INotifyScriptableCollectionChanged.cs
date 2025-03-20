#nullable enable

namespace Hertzole.ScriptableValues
{
	public interface INotifyScriptableCollectionChanged<T>
	{
		event CollectionChangedEventHandler<T> OnCollectionChanged;

		void RegisterChangedListener(CollectionChangedEventHandler<T> callback);

		void RegisterChangedListener<TContext>(CollectionChangedWithContextEventHandler<T, TContext> callback, TContext context);
		
		void UnregisterChangedListener(CollectionChangedEventHandler<T> callback);
		
		void UnregisterChangedListener<TContext>(CollectionChangedWithContextEventHandler<T, TContext> callback);
	}
}