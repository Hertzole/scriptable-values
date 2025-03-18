#nullable enable

using System;
using System.Collections;
using System.Collections.Specialized;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void Add_AddsItem()
		{
			// Arrange
			int value = GetRandomNumber();

			// Act
			list.Add(value);

			// Assert
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(value, list[0]);
		}

		[Test]
		public void Add_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int value = GetRandomNumber();
			CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			list.Add(value);

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked());
			Assert.AreEqual(value, args.NewItems.Span[0]);
			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
		}

		[Test]
		public void Add_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int value = GetRandomNumber();
			CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list);

			// Act
			list.Add(value);

			// Assert
			NotifyCollectionChangedEventArgs args = tracker.NotifyCollectionChangedArgs;
			Assert.IsNotNull(args);
			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
			Assert.AreEqual(1, args.NewItems.Count);
			Assert.AreEqual(value, args.NewItems[0]);
			Assert.AreEqual(0, args.NewStartingIndex);
		}

		[Test]
		public void Add_ReadOnly_ThrowsReadOnlyException_DoesNotInvokeEvents([Values] EventType eventType)
		{
			// Arrange
			IsReadOnly = true;
			int value = GetRandomNumber();

			// Assert
			AssertThrowsReadOnlyExceptionAndNotInvoked(list, eventType, x => x.Add(value));
		}

		[Test]
		public void Add_Object_AddsItem()
		{
			// Arrange
			int value = GetRandomNumber();

			// Act
			int result = ((IList) list).Add(value);

			// Assert
			Assert.AreEqual(0, result); // Make sure the result is the index of the added item, which is 0.
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(value, list[0]);
		}

		[Test]
		public void Add_Object_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int value = GetRandomNumber();
			using CollectionEventTracker<int> tracker = new CollectionEventTracker<int>(list, eventType);

			// Act
			((IList) list).Add(value);

			// Assert
			CollectionChangedArgs<int> args = tracker.CollectionChangedArgs;
			Assert.IsTrue(tracker.HasBeenInvoked(), "The event has not been invoked.");
			Assert.AreEqual(value, args.NewItems.Span[0]);
			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
		}

		[Test]
		public void Add_Object_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int value = GetRandomNumber();
			NotifyCollectionChangedEventArgs? args = null;
			((INotifyCollectionChanged) list).CollectionChanged += OnCollectionChanged;

			// Act
			((IList) list).Add(value);

			// Assert
			Assert.IsNotNull(args);
			Assert.AreEqual(NotifyCollectionChangedAction.Add, args!.Action);
			Assert.AreEqual(1, args.NewItems.Count);
			Assert.AreEqual(value, args.NewItems[0]);
			Assert.AreEqual(0, args.NewStartingIndex);
			return;

			void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				args = e;
			}
		}

		[Test]
		public void Add_Object_ReadOnly_ThrowsReadOnlyException_DoesNotInvokeEvents([Values] EventType eventType)
		{
			// Arrange
			IsReadOnly = true;
			int value = GetRandomNumber();

			// Assert
			AssertThrowsReadOnlyExceptionAndNotInvoked(list, eventType, x => ((IList) x).Add(value));
		}

		[Test]
		public void Add_Object_InvalidType_ThrowsArgumentException()
		{
			// Arrange
			object value = new object();

			// Assert
			AssertThrows<ArgumentException>(() => ((IList) list).Add(value));
		}

		[Test]
		public void Add_Object_Null_ThrowsArgumentNullException()
		{
			// Assert
			AssertThrows<ArgumentNullException>(() => ((IList) list).Add(null));
		}
	}
}