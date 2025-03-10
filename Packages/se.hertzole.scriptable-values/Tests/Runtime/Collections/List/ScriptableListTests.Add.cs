#nullable enable

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
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
			InvokeCountContext context = new InvokeCountContext();
			context.AddArg("args", default(CollectionChangedArgs<int>));
			switch (eventType)
			{
				case EventType.Event:
					list.OnCollectionChanged += OnCollectionChanged;
					break;
				case EventType.Register:
					list.RegisterChangedListener(OnCollectionChanged);
					break;
				case EventType.RegisterWithContext:
					list.RegisterChangedListener(OnCollectionChangedWithContext, context);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}

			// Act
			list.Add(value);

			// Assert
			CollectionChangedArgs<int> args = context.GetArg<CollectionChangedArgs<int>>("args");
			Assert.AreEqual(value, args.NewItems.Span[0]);
			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
			Assert.AreEqual(1, context.invokeCount);
			return;

			void OnCollectionChanged(CollectionChangedArgs<int> e)
			{
				context.invokeCount++;
				context.SetArg("args", e);
			}

			static void OnCollectionChangedWithContext(CollectionChangedArgs<int> e, InvokeCountContext context)
			{
				context.invokeCount++;
				context.SetArg("args", e);
			}
		}

		[Test]
		public void Add_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int value = GetRandomNumber();
			NotifyCollectionChangedEventArgs? args = null;
			((INotifyCollectionChanged) list).CollectionChanged += OnCollectionChanged;

			// Act
			list.Add(value);

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
		public void Add_ReadOnly_DoesNotAddItem()
		{
			// Arrange
			IsReadOnly = true;
			int value = GetRandomNumber();

			// Assert
			AssertThrows<ReadOnlyException>(() => list.Add(value));
		}

		[Test]
		public void Add_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			IsReadOnly = true;
			int value = GetRandomNumber();
			InvokeCountContext context = new InvokeCountContext();
			context.AddArg("args", default(CollectionChangedArgs<int>));

			switch (eventType)
			{
				case EventType.Event:
					list.OnCollectionChanged += OnCollectionChanged;
					break;
				case EventType.Register:
					list.RegisterChangedListener(OnCollectionChanged);
					break;
				case EventType.RegisterWithContext:
					list.RegisterChangedListener(OnCollectionChangedWithContext, context);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}

			// Assert
			AssertThrows<ReadOnlyException>(() => list.Add(value));
			Assert.AreEqual(0, context.invokeCount);
			Assert.AreEqual(default, context.GetArg<CollectionChangedArgs<int>>("args"));
			return;

			void OnCollectionChanged(CollectionChangedArgs<int> e)
			{
				context.invokeCount++;
				context.SetArg("args", e);
			}

			static void OnCollectionChangedWithContext(CollectionChangedArgs<int> e, InvokeCountContext context)
			{
				context.invokeCount++;
				context.SetArg("args", e);
			}
		}

		[Test]
		public void Add_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			IsReadOnly = true;
			int value = GetRandomNumber();
			NotifyCollectionChangedEventArgs? args = null;
			((INotifyCollectionChanged) list).CollectionChanged += OnCollectionChanged;

			// Assert
			AssertThrows<ReadOnlyException>(() => list.Add(value));
			Assert.IsNull(args);
			return;

			void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				args = e;
			}
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
			InvokeCountContext context = new InvokeCountContext();
			context.AddArg("args", default(CollectionChangedArgs<int>));
			switch (eventType)
			{
				case EventType.Event:
					list.OnCollectionChanged += OnCollectionChanged;
					break;
				case EventType.Register:
					list.RegisterChangedListener(OnCollectionChanged);
					break;
				case EventType.RegisterWithContext:
					list.RegisterChangedListener(OnCollectionChangedWithContext, context);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}

			// Act
			((IList) list).Add(value);

			// Assert
			CollectionChangedArgs<int> args = context.GetArg<CollectionChangedArgs<int>>("args");
			Assert.AreEqual(value, args.NewItems.Span[0]);
			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
			Assert.AreEqual(1, context.invokeCount);
			return;

			void OnCollectionChanged(CollectionChangedArgs<int> e)
			{
				context.invokeCount++;
				context.SetArg("args", e);
			}

			static void OnCollectionChangedWithContext(CollectionChangedArgs<int> e, InvokeCountContext context)
			{
				context.invokeCount++;
				context.SetArg("args", e);
			}
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
		public void Add_Object_ReadOnly_DoesNotAddItem()
		{
			// Arrange
			IsReadOnly = true;
			int value = GetRandomNumber();

			// Assert
			AssertThrows<ReadOnlyException>(() => ((IList) list).Add(value));
		}

		[Test]
		public void Add_Object_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			IsReadOnly = true;
			int value = GetRandomNumber();
			InvokeCountContext context = new InvokeCountContext();
			context.AddArg("args", default(CollectionChangedArgs<int>));

			switch (eventType)
			{
				case EventType.Event:
					list.OnCollectionChanged += OnCollectionChanged;
					break;
				case EventType.Register:
					list.RegisterChangedListener(OnCollectionChanged);
					break;
				case EventType.RegisterWithContext:
					list.RegisterChangedListener(OnCollectionChangedWithContext, context);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}

			// Assert
			AssertThrows<ReadOnlyException>(() => ((IList) list).Add(value));
			Assert.AreEqual(0, context.invokeCount);
			Assert.AreEqual(default, context.GetArg<CollectionChangedArgs<int>>("args"));
			return;

			void OnCollectionChanged(CollectionChangedArgs<int> e)
			{
				context.invokeCount++;
				context.SetArg("args", e);
			}

			static void OnCollectionChangedWithContext(CollectionChangedArgs<int> e, InvokeCountContext context)
			{
				context.invokeCount++;
				context.SetArg("args", e);
			}
		}

		[Test]
		public void Add_Object_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			IsReadOnly = true;
			int value = GetRandomNumber();
			NotifyCollectionChangedEventArgs? args = null;
			((INotifyCollectionChanged) list).CollectionChanged += OnCollectionChanged;

			// Assert
			AssertThrows<ReadOnlyException>(() => ((IList) list).Add(value));
			Assert.IsNull(args);
			return;

			void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				args = e;
			}
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