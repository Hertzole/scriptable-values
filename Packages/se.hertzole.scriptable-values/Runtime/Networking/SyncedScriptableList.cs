#if (FISHNET || SCRIPTABLE_VALUES_NGO)
#define SCRIPTABLE_VALUES_NETWORKING
#endif

#if SCRIPTABLE_VALUES_NETWORKING
using System;
using Hertzole.ScriptableValues.Helpers;

namespace Hertzole.ScriptableValues
{
	public sealed partial class SyncedScriptableList<T> : IDisposable
	{
		private bool isInitialized;

		private ScriptableList<T> targetList;

		public void Initialize(ScriptableList<T> list)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));

			targetList = list;

			targetList.OnAdded += OnAdded;
			targetList.OnRemoved += OnRemoved;
			targetList.OnInserted += OnInserted;
			targetList.OnSet += OnSet;
			targetList.OnCleared += OnCleared;

			isInitialized = true;

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
			if (targetList != null)
			{
				targetList.OnAdded -= OnAdded;
				targetList.OnRemoved -= OnRemoved;
				targetList.OnInserted -= OnInserted;
				targetList.OnSet -= OnSet;
				targetList.OnCleared -= OnCleared;
				targetList = null;
			}

			isInitialized = false;
		}
	}
}
#endif