using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Constraints;
using Assert = UnityEngine.Assertions.Assert;
using AssertionException = UnityEngine.Assertions.AssertionException;
using Is = UnityEngine.TestTools.Constraints.Is;

namespace Hertzole.ScriptableValues.Tests
{
	public partial class ScriptableDictionaryTests : BaseCollectionTest
	{
		private TestScriptableDictionary dictionary;

		public bool IsReadOnly
		{
			get { return dictionary.IsReadOnly; }
			set { dictionary.IsReadOnly = value; }
		}

		protected override void OnSetup()
		{
			dictionary = CreateInstance<TestScriptableDictionary>();
			dictionary.name = "Instance";
		}

		[Test]
		public void ContainsKey()
		{
			dictionary.Add(0, 42);
			Assert.IsTrue(dictionary.ContainsKey(0));
			Assert.IsFalse(dictionary.ContainsKey(1));
		}

		[Test]
		public void ContainsValue()
		{
			dictionary.Add(0, 42);
			Assert.IsTrue(dictionary.ContainsValue(42));
			Assert.IsFalse(dictionary.ContainsValue(43));
		}

		[Test]
		public void ContainsKeyValuePair()
		{
			dictionary.Add(0, 42);
			ICollection<KeyValuePair<int, int>> d = dictionary;
			Assert.IsTrue(d.Contains(new KeyValuePair<int, int>(0, 42)));
			Assert.IsFalse(d.Contains(new KeyValuePair<int, int>(1, 42)));
			Assert.IsFalse(d.Contains(new KeyValuePair<int, int>(0, 43)));
		}

		[Test]
		public void ContainsObject()
		{
			dictionary.Add(0, 42);
			IDictionary d = dictionary;
			Assert.IsTrue(d.Contains(0));
			Assert.IsFalse(d.Contains(1));
		}

		[Test]
		public void TryGetValue()
		{
			dictionary.Add(0, 42);
			Assert.IsTrue(dictionary.TryGetValue(0, out int value));
			Assert.AreEqual(42, value);

			Assert.IsFalse(dictionary.TryGetValue(1, out value));
			Assert.AreEqual(0, value);
		}

		[Test]
		public void TryFindKey()
		{
			dictionary.Add(2, 42);
			Assert.IsTrue(dictionary.TryFindKey(key => key == 2, out int keyValue));
			Assert.AreEqual(2, keyValue);

			Assert.IsFalse(dictionary.TryFindKey(key => key == 1, out keyValue));
			Assert.AreEqual(0, keyValue);
		}

		[Test]
		public void TryFindValue()
		{
			dictionary.Add(0, 42);
			Assert.IsTrue(dictionary.TryFindValue(value => value == 42, out int actual));
			Assert.AreEqual(42, actual);

			Assert.IsFalse(dictionary.TryFindValue(value => value == 43, out actual));
			Assert.AreEqual(0, actual);
		}

		[Test]
		public void CopyTo()
		{
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);
			KeyValuePair<int, int>[] array = new KeyValuePair<int, int>[3];
			ICollection<KeyValuePair<int, int>> d = dictionary;
			d.CopyTo(array, 0);
			Assert.AreEqual(0, array[0].Key);
			Assert.AreEqual(42, array[0].Value);
			Assert.AreEqual(1, array[1].Key);
			Assert.AreEqual(43, array[1].Value);
			Assert.AreEqual(2, array[2].Key);
			Assert.AreEqual(44, array[2].Value);
		}

		[Test]
		public void CopyTo_Object()
		{
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);
			object[] array = new object[3];

			ICollection d = dictionary;
			d.CopyTo(array, 0);

			Assert.AreEqual(3, array.Length);
		}

		[Test]
		public void TrimExcess()
		{
			// Arrange
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			// Act
			dictionary.TrimExcess();

			// Assert
			Assert.AreEqual(3, dictionary.Count);
			Assert.AreEqual(3, dictionary.keys.Count);
			Assert.AreEqual(3, dictionary.values.Count);
		}

		[Test]
		public void TrimExcess_Capacity()
		{
			// Arrange
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			// Act
			dictionary.TrimExcess(3);

			// Assert
			Assert.AreEqual(3, dictionary.Count);
			Assert.AreEqual(3, dictionary.keys.Count);
			Assert.AreEqual(3, dictionary.values.Count);
		}

		[Test]
		public void GetEnumerator_IEnumerable()
		{
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			// This generates garbage because of boxing.
			NUnit.Framework.Assert.That(() =>
			{
				IEnumerator enumerator = ((IEnumerable) dictionary).GetEnumerator();
				if (enumerator is IDisposable d)
				{
					d.Dispose();
				}
			}, Is.AllocatingGCMemory());

			IEnumerator enumerator = ((IEnumerable) dictionary).GetEnumerator();

			try
			{
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(0, ((KeyValuePair<int, int>) enumerator.Current!).Key);
				Assert.AreEqual(42, ((KeyValuePair<int, int>) enumerator.Current).Value);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(1, ((KeyValuePair<int, int>) enumerator.Current).Key);
				Assert.AreEqual(43, ((KeyValuePair<int, int>) enumerator.Current).Value);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(2, ((KeyValuePair<int, int>) enumerator.Current).Key);
				Assert.AreEqual(44, ((KeyValuePair<int, int>) enumerator.Current).Value);
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
		public void GetEnumerator_IEnumerableGeneric()
		{
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			// This generates garbage because of boxing.
			NUnit.Framework.Assert.That(() =>
			{
				IEnumerator<KeyValuePair<int, int>> enumerator = ((IEnumerable<KeyValuePair<int, int>>) dictionary).GetEnumerator();
				enumerator.Dispose();
			}, Is.AllocatingGCMemory());

			IEnumerator enumerator = ((IEnumerable) dictionary).GetEnumerator();

			try
			{
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(0, ((KeyValuePair<int, int>) enumerator.Current!).Key);
				Assert.AreEqual(42, ((KeyValuePair<int, int>) enumerator.Current).Value);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(1, ((KeyValuePair<int, int>) enumerator.Current).Key);
				Assert.AreEqual(43, ((KeyValuePair<int, int>) enumerator.Current).Value);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(2, ((KeyValuePair<int, int>) enumerator.Current).Key);
				Assert.AreEqual(44, ((KeyValuePair<int, int>) enumerator.Current).Value);
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
		public void GetEnumerator_IDictionary()
		{
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			// This generates garbage because of boxing.
			NUnit.Framework.Assert.That(() =>
			{
				IDictionaryEnumerator enumerator = ((IDictionary) dictionary).GetEnumerator();

				if (enumerator is IDisposable d)
				{
					d.Dispose();
				}
			}, Is.AllocatingGCMemory());

			IDictionaryEnumerator enumerator = ((IDictionary) dictionary).GetEnumerator();

			try
			{
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(0, enumerator.Key);
				Assert.AreEqual(42, enumerator.Value);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(1, enumerator.Key);
				Assert.AreEqual(43, enumerator.Value);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(2, enumerator.Key);
				Assert.AreEqual(44, enumerator.Value);
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
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			// This does not generate garbage because of the pure struct.
			NUnit.Framework.Assert.That(() =>
			{
				Dictionary<int, int>.Enumerator enumerator = dictionary.GetEnumerator();
				enumerator.Dispose();
			}, NUnit.Framework.Is.Not.AllocatingGCMemory());

			Dictionary<int, int>.Enumerator enumerator = dictionary.GetEnumerator();

			try
			{
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(0, enumerator.Current.Key);
				Assert.AreEqual(42, enumerator.Current.Value);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(1, enumerator.Current.Key);
				Assert.AreEqual(43, enumerator.Current.Value);
				Assert.IsTrue(enumerator.MoveNext());
				Assert.AreEqual(2, enumerator.Current.Key);
				Assert.AreEqual(44, enumerator.Current.Value);
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
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			// This does not generate garbage because of the pure struct.
			NUnit.Framework.Assert.That(() =>
			{
				int keySum = 0;
				int valueSum = 0;

				foreach (KeyValuePair<int, int> pair in dictionary)
				{
					keySum += pair.Key;
					valueSum += pair.Value;
				}
			}, NUnit.Framework.Is.Not.AllocatingGCMemory());
		}

		[Test]
		public void ForEach_Keys_NoAlloc()
		{
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			// Mostly here to just make sure Keys gets created. Or else it will caught as gc alloc in the foreach.
			Assert.AreEqual(3, dictionary.Keys.Count);

			// This does not generate garbage because of the pure struct.
			NUnit.Framework.Assert.That(() =>
			{
				int keySum = 0;

				foreach (int key in dictionary.Keys)
				{
					keySum += key;
				}
			}, NUnit.Framework.Is.Not.AllocatingGCMemory());
		}

		[Test]
		public void ForEach_Values_NoAlloc()
		{
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			// Mostly here to just make sure Values gets created. Or else it will caught as gc alloc in the foreach.
			Assert.AreEqual(3, dictionary.Values.Count);

			// This does not generate garbage because of the pure struct.
			NUnit.Framework.Assert.That(() =>
			{
				int valueSum = 0;

				foreach (int value in dictionary.Values)
				{
					valueSum += value;
				}
			}, NUnit.Framework.Is.Not.AllocatingGCMemory());
		}

		[Test]
		public void IsFixedSizeWhenReadOnly()
		{
			IsReadOnly = true;
			Assert.IsTrue(((IDictionary) dictionary).IsFixedSize);
		}

		[Test]
		public void IsFixedSizeWhenNotReadOnly()
		{
			IsReadOnly = false;
			Assert.IsFalse(((IDictionary) dictionary).IsFixedSize);
		}

		[Test]
		public void IsReadOnlyWhenReadOnly()
		{
			IsReadOnly = true;
			Assert.IsTrue(((IDictionary) dictionary).IsReadOnly);
		}

		[Test]
		public void IsReadOnlyWhenNotReadOnly()
		{
			IsReadOnly = false;
			Assert.IsFalse(((IDictionary) dictionary).IsReadOnly);
		}

		[Test]
		public void IsSynchronized()
		{
			Assert.IsFalse(((ICollection) dictionary).IsSynchronized);
		}

		[Test]
		public void SyncRoot()
		{
			Assert.AreEqual(dictionary, ((ICollection) dictionary).SyncRoot);
		}

		[Test]
		public void DoesKeysMatch()
		{
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			int index = 0;
			ICollection dictionaryKeys = ((IDictionary) dictionary).Keys;
			foreach (int key in dictionaryKeys)
			{
				Assert.AreEqual(index, key);
				index++;
			}

			Assert.AreEqual(3, index);

			index = 0;

			foreach (int key in dictionary.Keys)
			{
				Assert.AreEqual(index, key);
				index++;
			}

			Assert.AreEqual(3, index);

			index = 0;

			IEnumerable<int> readOnlyKeys = ((IReadOnlyDictionary<int, int>) dictionary).Keys;
			foreach (int key in readOnlyKeys)
			{
				Assert.AreEqual(index, key);
				index++;
			}

			Assert.AreEqual(3, index);
		}

		[Test]
		public void DoesValuesMatch()
		{
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			int index = 42;
			ICollection dictionaryValues = ((IDictionary) dictionary).Values;
			foreach (int value in dictionaryValues)
			{
				Assert.AreEqual(index, value);
				index++;
			}

			Assert.AreEqual(45, index);

			index = 42;

			foreach (int value in dictionary.Values)
			{
				Assert.AreEqual(index, value);
				index++;
			}

			Assert.AreEqual(45, index);

			index = 42;

			IEnumerable<int> readOnlyValues = ((IReadOnlyDictionary<int, int>) dictionary).Values;
			foreach (int value in readOnlyValues)
			{
				Assert.AreEqual(index, value);
				index++;
			}

			Assert.AreEqual(45, index);
		}

		[Test]
		public void Serialization_CreatesDictionary()
		{
			dictionary.dictionary.Clear();

			dictionary.keys.Add(0);
			dictionary.keys.Add(1);
			dictionary.keys.Add(2);

			dictionary.values.Add(42);
			dictionary.values.Add(43);
			dictionary.values.Add(44);

			ISerializationCallbackReceiver callbackReceiver = dictionary;

			Assert.IsNotNull(callbackReceiver);

			callbackReceiver.OnBeforeSerialize(); // Just to get the coverage up. Does nothing.

			callbackReceiver.OnAfterDeserialize();

			Assert.AreEqual(3, dictionary.Count);
			Assert.AreEqual(42, dictionary[0]);
			Assert.AreEqual(43, dictionary[1]);
			Assert.AreEqual(44, dictionary[2]);
		}

		[Test]
		public void Serialization_Invalid_KeyLengthMismatch()
		{
			dictionary.dictionary.Clear();

			dictionary.keys.Add(0);
			dictionary.keys.Add(1);
			dictionary.keys.Add(2);
			dictionary.keys.Add(3);

			dictionary.values.Add(42);
			dictionary.values.Add(43);
			dictionary.values.Add(44);

			ISerializationCallbackReceiver callbackReceiver = dictionary;

			Assert.IsNotNull(callbackReceiver);

			callbackReceiver.OnBeforeSerialize(); // Just to get the coverage up. Does nothing.

			callbackReceiver.OnAfterDeserialize();

			Assert.AreEqual(0, dictionary.Count);
		}

		[Test]
		public void Serialization_Invalid_ValueLengthMismatch()
		{
			dictionary.dictionary.Clear();

			dictionary.keys.Add(0);
			dictionary.keys.Add(1);
			dictionary.keys.Add(2);

			dictionary.values.Add(42);
			dictionary.values.Add(43);
			dictionary.values.Add(44);
			dictionary.values.Add(45);

			ISerializationCallbackReceiver callbackReceiver = dictionary;

			Assert.IsNotNull(callbackReceiver);

			callbackReceiver.OnBeforeSerialize(); // Just to get the coverage up. Does nothing.

			callbackReceiver.OnAfterDeserialize();

			Assert.AreEqual(0, dictionary.Count);
		}

		[Test]
		public void Serialization_Invalid_DuplicateKeys()
		{
			dictionary.dictionary.Clear();

			dictionary.keys.Add(0);
			dictionary.keys.Add(1);
			dictionary.keys.Add(2);
			dictionary.keys.Add(2);

			dictionary.values.Add(42);
			dictionary.values.Add(43);
			dictionary.values.Add(44);
			dictionary.values.Add(45);

			ISerializationCallbackReceiver callbackReceiver = dictionary;

			Assert.IsNotNull(callbackReceiver);

			callbackReceiver.OnBeforeSerialize(); // Just to get the coverage up. Does nothing.

			callbackReceiver.OnAfterDeserialize();

			Assert.AreEqual(0, dictionary.Count);
		}

		[Test]
		public void IsValid_Valid()
		{
			dictionary.dictionary.Clear();

			dictionary.keys.Add(0);
			dictionary.keys.Add(1);
			dictionary.keys.Add(2);

			dictionary.values.Add(42);
			dictionary.values.Add(43);
			dictionary.values.Add(44);

			Assert.IsTrue(dictionary.IsValid());
		}

		[Test]
		public void IsValid_Invalid_KeyLengthMismatch()
		{
			dictionary.dictionary.Clear();

			dictionary.keys.Add(0);
			dictionary.keys.Add(1);
			dictionary.keys.Add(2);
			dictionary.keys.Add(3);

			dictionary.values.Add(42);
			dictionary.values.Add(43);
			dictionary.values.Add(44);

			Assert.IsFalse(dictionary.IsValid());
		}

		[Test]
		public void IsValid_Invalid_ValueLengthMismatch()
		{
			dictionary.dictionary.Clear();

			dictionary.keys.Add(0);
			dictionary.keys.Add(1);
			dictionary.keys.Add(2);

			dictionary.values.Add(42);
			dictionary.values.Add(43);
			dictionary.values.Add(44);
			dictionary.values.Add(45);

			Assert.IsFalse(dictionary.IsValid());
		}

		[Test]
		public void IsValid_Invalid_DuplicateKeys()
		{
			dictionary.dictionary.Clear();

			dictionary.keys.Add(0);
			dictionary.keys.Add(1);
			dictionary.keys.Add(2);
			dictionary.keys.Add(2);

			dictionary.values.Add(42);
			dictionary.values.Add(43);
			dictionary.values.Add(44);
			dictionary.values.Add(45);

			Assert.IsFalse(dictionary.IsValid());
		}

		[Test]
		public void IsValid_Base()
		{
			TestBaseScriptableDictionary baseDic = ScriptableObject.CreateInstance<TestBaseScriptableDictionary>();

			Assert.IsFalse(baseDic.IsValid());
		}

		[Test]
		public void IsIndexValid_Valid()
		{
			dictionary.dictionary.Clear();

			dictionary.keys.Add(0);
			dictionary.keys.Add(1);
			dictionary.keys.Add(2);

			dictionary.values.Add(42);
			dictionary.values.Add(43);
			dictionary.values.Add(44);

			Assert.IsTrue(dictionary.IsIndexUnique(0));
			Assert.IsTrue(dictionary.IsIndexUnique(1));
			Assert.IsTrue(dictionary.IsIndexUnique(2));
		}

		[Test]
		public void IsIndexValid_Invalid_DuplicateKeys()
		{
			dictionary.dictionary.Clear();

			dictionary.keys.Add(0);
			dictionary.keys.Add(1);
			dictionary.keys.Add(2);
			dictionary.keys.Add(0);

			dictionary.values.Add(42);
			dictionary.values.Add(43);
			dictionary.values.Add(44);
			dictionary.values.Add(5);

			Assert.IsFalse(dictionary.IsIndexUnique(0));
		}

		[Test]
		public void IsIndexValid_Base()
		{
			TestBaseScriptableDictionary baseDic = ScriptableObject.CreateInstance<TestBaseScriptableDictionary>();

			Assert.IsFalse(baseDic.IsIndexUnique(0));
		}

		[Test]
		public void Comparer_SetNewComparer()
		{
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			Assert.AreEqual(3, dictionary.Count);

			ReverseComparer<int> comparer = new ReverseComparer<int>();

			dictionary.Comparer = comparer;

			Assert.AreEqual(comparer, dictionary.Comparer);
			Assert.AreEqual(3, dictionary.Count);

			Assert.AreEqual(42, dictionary[0]);
			Assert.AreEqual(43, dictionary[1]);
			Assert.AreEqual(44, dictionary[2]);
		}

		[Test]
		public void Comparer_SetNull()
		{
			dictionary.Add(0, 42);
			dictionary.Add(1, 43);
			dictionary.Add(2, 44);

			Assert.AreEqual(3, dictionary.Count);

			dictionary.Comparer = null;

			Assert.AreEqual(EqualityComparer<int>.Default, dictionary.Comparer);
			Assert.AreEqual(3, dictionary.Count);

			Assert.AreEqual(42, dictionary[0]);
			Assert.AreEqual(43, dictionary[1]);
			Assert.AreEqual(44, dictionary[2]);
		}

		[Test]
		public void Get_Object_ReturnsResult()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);

			// Act
			object result = ((IDictionary) dictionary)[key];

			// Assert
			Assert.AreEqual(value, result);
		}

		[Test]
		public void Get_Object_InvalidKey_ReturnsDefaultValue()
		{
			// Arrange
			int key = GetRandomNumber();
			int value = GetRandomNumber();
			dictionary.Add(key, value);
			LogAssert.Expect(LogType.Error, $"{typeof(int)} is not assignable from {typeof(object)}.");

			// Act
			object result = ((IDictionary) dictionary)[new object()];

			// Assert
			Assert.AreEqual(null, result);
		}

		private class ReverseComparer<T> : IEqualityComparer<T>
		{
			public bool Equals(T x, T y)
			{
				return EqualityComparer<T>.Default.Equals(y, x);
			}

			public int GetHashCode(T obj)
			{
				return EqualityComparer<T>.Default.GetHashCode(obj);
			}
		}

		private class TestBaseScriptableDictionary : ScriptableDictionary
		{
			/// <inheritdoc />
			public override bool IsReadOnly { get; set; }
			/// <inheritdoc />
			public override bool SetEqualityCheck { get; set; }
			/// <inheritdoc />
			public override bool ClearOnStart { get; set; }
			/// <inheritdoc />
			public override int Count { get; protected set; }
		}
	}
}