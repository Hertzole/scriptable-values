#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Constraints;
using Assert = UnityEngine.Assertions.Assert;
using AssertionException = UnityEngine.Assertions.AssertionException;
using Is = UnityEngine.TestTools.Constraints.Is;

namespace Hertzole.ScriptableValues.Tests
{
	public partial class ScriptableListTests : BaseCollectionTest
	{
		private TestScriptableList list = null!;

		protected override void OnSetup()
		{
			list = CreateInstance<TestScriptableList>();
			list.name = "Instance";
		}

		[Test]
		public void ToArray()
		{
			// Arrange
			int[] items = GetShuffledArray<int>();
			list.AddRange(items);

			// Act
			int[] array = list.ToArray();

			// Assert
			AssertArraysAreEqual(items, array);
		}

		[Test]
		public void TrimExcess()
		{
			// Arrange
			list.EnsureCapacity(100);
			list.Add(5);
			list.Add(2);
			list.Add(1);

			// Act
			list.TrimExcess();

			// Assert
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(3, list.Capacity);
		}

		[Test]
		public void TrueForAll()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			Assert.IsTrue(list.TrueForAll(i => i > 0));
		}

		[Test]
		public void TryFind()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			Assert.IsTrue(list.TryFind(i => i == 2, out int result));
			Assert.AreEqual(2, result);
		}

		[Test]
		public void TryFind_None()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			Assert.IsFalse(list.TryFind(i => i == 4, out int result));
			Assert.AreEqual(0, result);
		}

		[Test]
		public void GetGenericEnumerator()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			// This generates garbage due to boxing.
			NUnit.Framework.Assert.That(() =>
			{
				IEnumerator<int> enumerator = ((IList<int>) list).GetEnumerator();
				enumerator.Dispose();
			}, Is.AllocatingGCMemory());

			IEnumerator<int> enumerator = ((IList<int>) list).GetEnumerator();

			try
			{
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(5, enumerator.Current);

				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(2, enumerator.Current);

				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(1, enumerator.Current);

				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(3, enumerator.Current);

				Assert.IsFalse(enumerator.MoveNext());
			}
			catch (AssertionException) { }
			finally
			{
				enumerator.Dispose();
			}
		}

		[Test]
		public void GetNonGenericEnumerator()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			// This generates garbage due to boxing.
			NUnit.Framework.Assert.That(() =>
			{
				IEnumerator enumerator = ((IList) list).GetEnumerator();
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}, Is.AllocatingGCMemory());

			IEnumerator enumerator = ((IList) list).GetEnumerator();

			try
			{
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(5, enumerator.Current);

				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(2, enumerator.Current);

				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(1, enumerator.Current);

				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(3, enumerator.Current);

				Assert.IsFalse(enumerator.MoveNext());
			}
			catch (AssertionException) { }
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}

		[Test]
		public void GetEnumerator()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			// This does not generate garbage due to the pure struct.
			NUnit.Framework.Assert.That(() =>
			{
				List<int>.Enumerator enumerator = list.GetEnumerator();
				enumerator.Dispose();
			}, NUnit.Framework.Is.Not.AllocatingGCMemory());

			List<int>.Enumerator enumerator = list.GetEnumerator();

			try
			{
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(5, enumerator.Current);

				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(2, enumerator.Current);

				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(1, enumerator.Current);

				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(3, enumerator.Current);

				Assert.IsFalse(enumerator.MoveNext());
			}
			catch (AssertionException) { }
			finally
			{
				enumerator.Dispose();
			}
		}

		[Test]
		public void ForEach_NoAlloc()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			// This does not generate garbage due to the pure struct.
			NUnit.Framework.Assert.That(() =>
			{
				int sum = 0;

				foreach (int i in list)
				{
					sum += i;
				}
			}, NUnit.Framework.Is.Not.AllocatingGCMemory());
		}

		[Test]
		public void Contains()
		{
			list.Add(1);
			list.Add(2);
			list.Add(3);

			Assert.IsTrue(list.Contains(2));
			Assert.IsFalse(list.Contains(4));
		}

		[Test]
		public void Contains_Object()
		{
			list.Add(1);
			list.Add(2);
			list.Add(3);

			IList l = list;

			Assert.IsTrue(l.Contains(2));
			Assert.IsFalse(l.Contains(4));
		}

		[Test]
		public void IndexOf()
		{
			list.Add(1);
			list.Add(2);
			list.Add(3);

			Assert.AreEqual(1, list.IndexOf(2));
			Assert.AreEqual(-1, list.IndexOf(4));
		}

		[Test]
		public void IndexOf_Object()
		{
			list.Add(1);
			list.Add(2);
			list.Add(3);

			IList l = list;

			Assert.AreEqual(1, l.IndexOf(2));
			Assert.AreEqual(-1, l.IndexOf(4));
		}

		[Test]
		public void IndexOf_Object_Invalid()
		{
			list.Add(1);
			list.Add(2);
			list.Add(3);

			IList l = list;

			LogAssert.Expect(LogType.Error, "System.Int32 is not assignable from System.String.");

			Assert.AreEqual(-1, l.IndexOf("invalid"));
		}

		[Test]
		public void CopyTo_Index()
		{
			list.Add(1);
			list.Add(2);
			list.Add(3);

			int[] array = new int[3];
			list.CopyTo(array, 0);

			Assert.AreEqual(1, array[0]);
			Assert.AreEqual(2, array[1]);
			Assert.AreEqual(3, array[2]);
		}

		[Test]
		public void CopyTo()
		{
			list.Add(1);
			list.Add(2);
			list.Add(3);

			int[] array = new int[3];
			list.CopyTo(array);

			Assert.AreEqual(1, array[0]);
			Assert.AreEqual(2, array[1]);
			Assert.AreEqual(3, array[2]);
		}

		[Test]
		public void CopyTo_Object()
		{
			list.Add(1);
			list.Add(2);
			list.Add(3);

			IList l = list;

			object[] array = new object[3];
			l.CopyTo(array, 0);

			Assert.AreEqual(1, array[0]);
			Assert.AreEqual(2, array[1]);
			Assert.AreEqual(3, array[2]);
		}

		[Test]
		public void IsFixedSizeWhenReadOnly()
		{
			list.IsReadOnly = true;
			Assert.IsTrue(((IList) list).IsFixedSize);
		}

		[Test]
		public void IsFixedSizeWhenNotReadOnly()
		{
			list.IsReadOnly = false;
			Assert.IsFalse(((IList) list).IsFixedSize);
		}

		[Test]
		public void IsReadOnlyWhenReadOnly()
		{
			list.IsReadOnly = true;
			Assert.IsTrue(((IList) list).IsReadOnly);
		}

		[Test]
		public void IsReadOnlyWhenNotReadOnly()
		{
			list.IsReadOnly = false;
			Assert.IsFalse(((IList) list).IsReadOnly);
		}

		[Test]
		public void IsSynchronized()
		{
			Assert.IsFalse(((IList) list).IsSynchronized);
		}

		[Test]
		public void SyncRoot()
		{
			Assert.AreEqual(list, ((IList) list).SyncRoot);
		}

		[Test]
		public void Exists_True()
		{
			list.AddRange(Enumerable.Range(0, 100));
			Assert.IsTrue(list.Exists(x => x == 22));
		}

		[Test]
		public void Exists_False()
		{
			list.AddRange(Enumerable.Range(0, 100));
			Assert.IsFalse(list.Exists(x => x == 200));
		}

		[Test]
		public void Find_Success()
		{
			list.AddRange(Enumerable.Range(0, 100));
			Assert.AreEqual(22, list.Find(x => x == 22));
		}

		[Test]
		public void Find_Failure()
		{
			list.AddRange(Enumerable.Range(0, 100));
			Assert.AreEqual(0, list.Find(x => x == 200));
		}

		[Test]
		public void EnsureCapacity()
		{
			list.EnsureCapacity(100);
			Assert.AreEqual(100, list.Capacity);

			list.EnsureCapacity(50);
			Assert.AreEqual(100, list.Capacity);
		}

		[Test]
		public void RegisterChangedCallback_InvokesCallback([Values] EventType eventType)
		{
			// Arrange
			InvokeCountContext context = new InvokeCountContext();
			switch (eventType)
			{
				case EventType.Event:
					list.OnCollectionChanged += OnCallback;
					break;
				case EventType.Register:
					list.RegisterChangedListener(OnCallback);
					break;
				case EventType.RegisterWithContext:
					list.RegisterChangedListener(OnStaticCallback, context);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}

			// Act
			list.Add(1); // This will cause the event to be invoked.

			// Assert
			Assert.AreEqual(1, context.invokeCount, $"The callback was not invoked for {eventType}.");
			return;

			void OnCallback(CollectionChangedArgs<int> e)
			{
				context.invokeCount++;
			}

			static void OnStaticCallback(CollectionChangedArgs<int> e, InvokeCountContext context)
			{
				context.invokeCount++;
			}
		}

		[Test]
		public void UnregisterChangedCallback_InvokesCallback([Values] EventType eventType)
		{
			// Arrange
			InvokeCountContext context = new InvokeCountContext();
			switch (eventType)
			{
				case EventType.Event:
					list.OnCollectionChanged += OnCallback;
					list.OnCollectionChanged -= OnCallback;
					break;
				case EventType.Register:
					list.RegisterChangedListener(OnCallback);
					list.UnregisterChangedListener(OnCallback);
					break;
				case EventType.RegisterWithContext:
					list.RegisterChangedListener(OnStaticCallback, context);
					list.UnregisterChangedListener<InvokeCountContext>(OnStaticCallback);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}

			// Act
			list.Add(1); // This will cause the event to be invoked.

			// Assert
			Assert.AreEqual(0, context.invokeCount, $"The callback was invoked for {eventType}.");
			return;

			void OnCallback(CollectionChangedArgs<int> e)
			{
				context.invokeCount++;
			}

			static void OnStaticCallback(CollectionChangedArgs<int> e, InvokeCountContext context)
			{
				context.invokeCount++;
			}
		}

		[Test]
		public void RegisterCollectionChanged_InvokesEvent()
		{
			// Arrange
			InvokeCountContext context = new InvokeCountContext();
			((INotifyCollectionChanged) list).CollectionChanged += OnCallback;

			// Act
			list.Add(1); // This will cause the event to be invoked.

			// Assert
			Assert.AreEqual(1, context.invokeCount, "The callback was not invoked.");
			return;

			void OnCallback(object sender, NotifyCollectionChangedEventArgs e)
			{
				context.invokeCount++;
			}
		}

		[Test]
		public void UnregisterCollectionChanged_InvokesEvent()
		{
			// Arrange
			InvokeCountContext context = new InvokeCountContext();
			((INotifyCollectionChanged) list).CollectionChanged += OnCallback;
			((INotifyCollectionChanged) list).CollectionChanged -= OnCallback;

			// Act
			list.Add(1); // This will cause the event to be invoked.

			// Assert
			Assert.AreEqual(0, context.invokeCount, "The callback was invoked.");
			return;

			void OnCallback(object sender, NotifyCollectionChangedEventArgs e)
			{
				context.invokeCount++;
			}
		}
	}
}