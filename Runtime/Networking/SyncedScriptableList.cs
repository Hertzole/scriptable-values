#if (FISHNET || SCRIPTABLE_VALUES_NGO)
#define SCRIPTABLE_VALUES_NETWORKING
#endif

#if SCRIPTABLE_VALUES_NETWORKING
using System;

namespace Hertzole.ScriptableValues
{
	public sealed partial class SyncedScriptableList<T> : IDisposable
	{
		private ScriptableList<T> targetList;

		public void Initialize(ScriptableList<T> list)
		{
			targetList = list;

			targetList.OnAdded += OnAdded;
			targetList.OnRemoved += OnRemoved;
			targetList.OnInserted += OnInserted;
			targetList.OnSet += OnSet;
			targetList.OnCleared += OnCleared;

			OnInitialized();
		}

		partial void OnAdded(T item);

		partial void OnRemoved(int index, T item);

		partial void OnInserted(int index, T item);

		partial void OnSet(int index, T oldItem, T newItem);

		partial void OnCleared();

		partial void OnInitialized();

		private void DisposeTargetList()
		{
			targetList.OnAdded -= OnAdded;
			targetList.OnRemoved -= OnRemoved;
			targetList.OnInserted -= OnInserted;
			targetList.OnSet -= OnSet;
			targetList.OnCleared -= OnCleared;
		}
	}
}
#endif