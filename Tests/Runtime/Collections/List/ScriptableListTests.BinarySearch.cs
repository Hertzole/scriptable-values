using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Random = UnityEngine.Random;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		// I'll be honest, I don't know what these types do. But they are based on the ones from the .NET runtime source code, so I guess they'll do.
		// https://github.com/dotnet/runtime/blob/main/src/libraries/System.Collections/tests/Generic/List/List.Generic.Tests.BinarySearch.cs

		public static readonly int[] validCollectionSizes =
		{
			0, 1, 75
		};

		[Test]
		public void BinarySearch_ForEveryItemWithoutDuplicates([ValueSource(nameof(validCollectionSizes))] int count)
		{
			// Arrange
			list.AddRange(CreateList(count));
			foreach (int i in list)
			{
				while (list.Count(value => value.Equals(i)) > 1)
				{
					list.Remove(i);
				}
			}

			list.Sort();
			List<int> beforeList = list.ToList();

			// Assert
			IEnumerable<int> range = Enumerable.Range(0, list.Count);
			int index = 0;
			foreach (int i in range)
			{
				Assert.AreEqual(index, list.BinarySearch(beforeList[index]));
				Assert.AreEqual(index, list.BinarySearch(beforeList[index], Comparer<int>.Default));
				Assert.AreEqual(beforeList[index], list[index]);
				index++;
			}
		}

		[Test]
		public void BinarySearch_ForEveryItemWithDuplicates([ValueSource(nameof(validCollectionSizes))] int count)
		{
			if (count > 0)
			{
				// Arrange
				list.AddRange(CreateList(count));
				list.Add(list[0]);
				list.Sort();
				List<int> beforeList = list.ToList();

				// Assert
				IEnumerable<int> range = Enumerable.Range(0, list.Count);
				int index = 0;
				foreach (int i in range)
				{
					Assert.IsTrue(list.BinarySearch(beforeList[index]) >= 0);
					Assert.IsTrue(list.BinarySearch(beforeList[index], Comparer<int>.Default) >= 0);
					Assert.AreEqual(beforeList[index], list[index]);
					index++;
				}
			}
		}

		[Test]
		public void BinarySearch_Validations([ValueSource(nameof(validCollectionSizes))] int count)
		{
			// Arrange
			list.AddRange(CreateList(count));
			list.Sort();
			int element = Random.Range(int.MinValue, int.MaxValue);

			// Assert
			AssertThrows<ArgumentOutOfRangeException>(() => list.BinarySearch(-1, count, element, Comparer<int>.Default));
			AssertThrows<ArgumentOutOfRangeException>(() => list.BinarySearch(0, -1, element, Comparer<int>.Default));
			AssertThrows<ArgumentException>(() => list.BinarySearch(count + 1, count, element, Comparer<int>.Default));
			AssertThrows<ArgumentException>(() => list.BinarySearch(0, count + 1, element, Comparer<int>.Default));
		}

		private static IEnumerable<int> CreateList(int count)
		{
			List<int> list = new List<int>(count);

			while (list.Count < count)
			{
				int toAdd = Random.Range(int.MinValue, int.MaxValue);
				while (list.Contains(toAdd))
				{
					toAdd = Random.Range(int.MinValue, int.MaxValue);
				}

				list.Add(toAdd);
			}

			return list;
		}
	}
}