#nullable enable

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;
using Random = UnityEngine.Random;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void Insert_InsertsItem()
		{
			// Arrange
			int value = GetRandomNumber();
			int index = Random.Range(20, 40);
			for (int i = 0; i < 50; i++)
			{
				list.Add(i);
			}

			// Act
			list.Insert(index, value);

			// Assert
			Assert.AreEqual(value, list[index]);
			Assert.AreEqual(51, list.Count);
		}

		[Test]
		public void Insert_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int value = GetRandomNumber();
			int index = Random.Range(20, 40);
			for (int i = 0; i < 50; i++)
			{
				list.Add(i);
			}

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
			list.Insert(index, value);

			// Assert
			CollectionChangedArgs<int> args = context.GetArg<CollectionChangedArgs<int>>("args");
			Assert.AreEqual(1, args.NewItems.Length);
			Assert.AreEqual(value, args.NewItems.Span[0]);
			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
			Assert.AreEqual(index, args.NewIndex);
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
		public void Insert_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int value = GetRandomNumber();
			int index = Random.Range(20, 40);
			for (int i = 0; i < 50; i++)
			{
				list.Add(i);
			}

			NotifyCollectionChangedEventArgs? args = null;
			((INotifyCollectionChanged) list).CollectionChanged += OnCollectionChanged;

			// Act
			list.Insert(index, value);

			// Assert
			Assert.IsNotNull(args);
			Assert.AreEqual(NotifyCollectionChangedAction.Add, args!.Action);
			Assert.AreEqual(value, args.NewItems[0]);
			Assert.AreEqual(index, args.NewStartingIndex);
			Assert.AreEqual(1, args.NewItems.Count);
			return;

			void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				args = e;
			}
		}

		[Test]
		public void Insert_ReadOnly_ThrowsReadOnlyException()
		{
			// Arrange
			list.IsReadOnly = true;

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => list.Insert(0, GetRandomNumber()));
		}

		[Test]
		public void Insert_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			list.IsReadOnly = true;

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

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => list.Insert(0, GetRandomNumber()));
			Assert.AreEqual(0, context.invokeCount);
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
		public void Insert_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			list.IsReadOnly = true;

			NotifyCollectionChangedEventArgs? args = null;
			((INotifyCollectionChanged) list).CollectionChanged += OnCollectionChanged;

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => list.Insert(0, GetRandomNumber()));
			Assert.IsNull(args);
			return;

			void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				args = e;
			}
		}

		[Test]
		public void Insert_OutOfBounds_ThrowsArgumentOutOfRangeException([Values(-1, 1)] int index)
		{
			// Act & Assert
			AssertThrows<ArgumentOutOfRangeException>(() => list.Insert(index, GetRandomNumber()));
		}

		[Test]
		public void Insert_Object_InsertsItem()
		{
			// Arrange
			int value = GetRandomNumber();
			int index = Random.Range(20, 40);
			for (int i = 0; i < 50; i++)
			{
				list.Add(value);
			}

			// Act
			((IList) list).Insert(index, value);

			// Assert
			Assert.AreEqual(51, list.Count);
			Assert.AreEqual(value, list[index]);
		}

		[Test]
		public void Insert_Object_InvokesCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			int value = GetRandomNumber();
			int index = Random.Range(20, 40);
			for (int i = 0; i < 50; i++)
			{
				list.Add(i);
			}

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
			((IList) list).Insert(index, value);

			// Assert
			CollectionChangedArgs<int> args = context.GetArg<CollectionChangedArgs<int>>("args");
			Assert.AreEqual(1, args.NewItems.Length);
			Assert.AreEqual(value, args.NewItems.Span[0]);
			Assert.AreEqual(NotifyCollectionChangedAction.Add, args.Action);
			Assert.AreEqual(index, args.NewIndex);
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
		public void Insert_Object_InvokesINotifyCollectionChanged()
		{
			// Arrange
			int value = GetRandomNumber();
			int index = Random.Range(20, 40);
			for (int i = 0; i < 50; i++)
			{
				list.Add(i);
			}

			NotifyCollectionChangedEventArgs? args = null;
			((INotifyCollectionChanged) list).CollectionChanged += OnCollectionChanged;

			// Act
			((IList) list).Insert(index, value);

			// Assert
			Assert.IsNotNull(args);
			Assert.AreEqual(NotifyCollectionChangedAction.Add, args!.Action);
			Assert.AreEqual(value, args.NewItems[0]);
			Assert.AreEqual(index, args.NewStartingIndex);
			Assert.AreEqual(1, args.NewItems.Count);
			return;

			void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				args = e;
			}
		}

		[Test]
		public void Insert_Object_ReadOnly_ThrowsReadOnlyException()
		{
			// Arrange
			list.IsReadOnly = true;

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => ((IList) list).Insert(0, GetRandomNumber()));
		}

		[Test]
		public void Insert_Object_ReadOnly_DoesNotInvokeCollectionChanged([Values] EventType eventType)
		{
			// Arrange
			list.IsReadOnly = true;

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

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => ((IList) list).Insert(0, GetRandomNumber()));
			Assert.AreEqual(0, context.invokeCount);
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
		public void Insert_Object_ReadOnly_DoesNotInvokeINotifyCollectionChanged()
		{
			// Arrange
			list.IsReadOnly = true;

			NotifyCollectionChangedEventArgs? args = null;
			((INotifyCollectionChanged) list).CollectionChanged += OnCollectionChanged;

			// Act & Assert
			AssertThrows<ReadOnlyException>(() => ((IList) list).Insert(0, GetRandomNumber()));
			Assert.IsNull(args);
			return;

			void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				args = e;
			}
		}

		[Test]
		public void Insert_Object_OutOfBounds_ThrowsArgumentOutOfRangeException([Values(-1, 1)] int index)
		{
			// Act & Assert
			AssertThrows<ArgumentOutOfRangeException>(() => ((IList) list).Insert(index, GetRandomNumber()));
		}

		[Test]
		public void Insert_Object_InvalidType_ThrowsArgumentException()
		{
			// Arrange
			object value = new object();

			// Act & Assert
			AssertThrows<ArgumentException>(() => ((IList) list).Insert(0, value));
		}

		[Test]
		public void Insert_Object_Null_ThrowsArgumentNullException()
		{
			// Act & Assert
			AssertThrows<ArgumentNullException>(() => ((IList) list).Insert(0, null));
		}
	}
}