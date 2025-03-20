using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using UnityEngine.Assertions;

namespace Hertzole.ScriptableValues.Tests
{
	public abstract class BaseCollectionTest : BaseRuntimeTest
	{
		protected static void AssertDoesNotInvokeCollectionChanged<T>(ScriptableList<T> list,
			EventType eventType,
			Action<ScriptableList<T>> action,
			bool handleReadOnly = false)
		{
			AssertDoesNotInvokeCollectionChangedInternal<ScriptableList<T>, T>(list, eventType, action, handleReadOnly);
		}

		protected static void AssertDoesNotInvokeCollectionChanged<TKey, TValue>(ScriptableDictionary<TKey, TValue> list,
			EventType eventType,
			Action<ScriptableDictionary<TKey, TValue>> action,
			bool handleReadOnly = false)
		{
			AssertDoesNotInvokeCollectionChangedInternal<ScriptableDictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>(list, eventType, action,
				handleReadOnly);
		}

		private static void AssertDoesNotInvokeCollectionChangedInternal<TType, TValue>(TType list,
			EventType eventType,
			Action<TType> action,
			bool handleReadOnly = false) where TType : INotifyScriptableCollectionChanged<TValue>
		{
			// Arrange
			using CollectionEventTracker<TValue> tracker = new CollectionEventTracker<TValue>(list, eventType);

			// Act
			if (handleReadOnly)
			{
				try
				{
					action.Invoke(list);
				}
				catch (ReadOnlyException)
				{
					// Does nothing.
				}
			}
			else
			{
				action.Invoke(list);
			}

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}

		protected static void AssertDoesNotInvokeINotifyCollectionChanged<T>(ScriptableList<T> list,
			Action<ScriptableList<T>> action,
			bool handleReadOnly = false)
		{
			AssertDoesNotInvokeINotifyCollectionChangedInternal<ScriptableList<T>, T>(list, action, handleReadOnly);
		}

		protected static void AssertDoesNotInvokeINotifyCollectionChanged<TKey, TValue>(ScriptableDictionary<TKey, TValue> list,
			Action<ScriptableDictionary<TKey, TValue>> action,
			bool handleReadOnly = false)
		{
			AssertDoesNotInvokeINotifyCollectionChangedInternal<ScriptableDictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>(list, action,
				handleReadOnly);
		}

		private static void AssertDoesNotInvokeINotifyCollectionChangedInternal<TType, TValue>(TType list,
			Action<TType> action,
			bool handleReadOnly = false) where TType : INotifyCollectionChanged
		{
			// Arrange
			using CollectionEventTracker<TValue> tracker = new CollectionEventTracker<TValue>(list);

			// Act
			if (handleReadOnly)
			{
				try
				{
					action.Invoke(list);
				}
				catch (ReadOnlyException)
				{
					// Does nothing.
				}
			}
			else
			{
				action.Invoke(list);
			}

			// Assert
			Assert.IsFalse(tracker.HasBeenInvoked(), "The event has been invoked.");
		}
	}
}