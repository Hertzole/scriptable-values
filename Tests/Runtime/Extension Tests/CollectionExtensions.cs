using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	public class CollectionExtensions : BaseRuntimeTest
	{
		public static IEnumerable GetNonEnumeratedCountCases
		{
			get
			{
				yield return new TestCaseData(new List<int> { 1, 2, 3, 4, 5 }, 5).SetName("List<T> with 5 elements");
				yield return new TestCaseData(new List<int>(), 0).SetName("List<T> with 0 elements");
				yield return new TestCaseData(new List<int>(new[] { 1, 2, 3, 4, 5 }).AsReadOnly(), 5).SetName("ReadOnlyCollection<T> with 5 elements");
				yield return new TestCaseData(new List<int>().AsReadOnly(), 0).SetName("ReadOnlyCollection<T> with 0 elements");
				yield return new TestCaseData(new[] { 1, 2, 3, 4, 5 }, 5).SetName("Array with 5 elements");
				yield return new TestCaseData(Array.Empty<int>(), 0).SetName("Array with 0 elements");
				yield return new TestCaseData(new HashSet<int> { 1, 2, 3, 4, 5 }, 5).SetName("HashSet<T> with 5 elements");
				yield return new TestCaseData(new HashSet<int>(), 0).SetName("HashSet<T> with 0 elements");
				yield return new TestCaseData(new Queue<int>(new[] { 1, 2, 3, 4, 5 }), 5).SetName("Queue<T> with 5 elements");
				yield return new TestCaseData(new Queue<int>(), 0).SetName("Queue<T> with 0 elements");
			}
		}

		[Test]
		[TestCase(1)]
		[TestCase(10)]
		[TestCase(500)]
		public void TryEnsureCapacity_GenericList_Success(int capacity)
		{
			// Arrange
			List<int> list = new List<int>();

			// Act
			TryEnsureCapacityTest<List<int>, int>(list, capacity);
		}

		[Test]
		[TestCase(1)]
		[TestCase(10)]
		[TestCase(500)]
		public void TryEnsureCapacity_ScriptableList_Success(int capacity)
		{
			// Arrange
			TestScriptableList list = CreateInstance<TestScriptableList>();

			// Act
			TryEnsureCapacityTest<ScriptableList<int>, int>(list, capacity);
		}

		private static void TryEnsureCapacityTest<TList, TValue>(TList list, int capacity) where TList : IList<TValue>
		{
			// Act
			bool success = list.TryEnsureCapacity(capacity);

			// Assert
			Assert.That(success, Is.True, "EnsureCapacity should succeed.");
			switch (list)
			{
				case List<TValue> genericList:
					Assert.That(genericList.Capacity, Is.AtLeast(capacity));
					break;
				case ScriptableList<TValue> scriptableList:
					Assert.That(scriptableList.Capacity, Is.AtLeast(capacity));
					break;
				default:
					Assert.Fail("List type is not supported.");
					break;
			}
		}

		[Test]
		public void TryEnsureCapacity_Failure()
		{
			// Arrange
			IList<int> list = new ArraySegment<int>(); // ArraySegment does not support capacity.
			int capacity = 100;

			// Act
			bool success = list.TryEnsureCapacity(capacity);

			// Assert
			Assert.That(success, Is.False, "EnsureCapacity should fail.");
		}

		[Test]
		[TestCaseSource(nameof(GetNonEnumeratedCountCases))]
		public void TryGetNonEnumeratedCount_Success(IEnumerable<int> enumerable, int expectedCount)
		{
			// Act
			bool success = enumerable.TryGetNonEnumeratedCount(out int count);

			// Assert
			Assert.That(success, Is.True, "TryGetNonEnumeratedCount should succeed.");
			Assert.That(count, Is.EqualTo(expectedCount), "Count should be equal to the expected count.");
		}

		[Test]
		public void TryGetNonEnumeratedCount_Failure()
		{
			// Arrange
			IEnumerable<int> enumerable = new Enumerable<int>(new[] { 1, 2, 3, 4, 5 });

			// Act
			bool success = enumerable.TryGetNonEnumeratedCount(out int count);

			// Assert
			Assert.That(success, Is.False, "TryGetNonEnumeratedCount should fail.");
			Assert.That(count, Is.EqualTo(0), "Count should be 0 when TryGetNonEnumeratedCount fails.");
		}

		private class Enumerable<T> : IEnumerable<T>
		{
			private readonly T[] items;

			public Enumerable(T[] items)
			{
				this.items = items;
			}

			public IEnumerator<T> GetEnumerator()
			{
				foreach (T item in items)
				{
					yield return item;
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}