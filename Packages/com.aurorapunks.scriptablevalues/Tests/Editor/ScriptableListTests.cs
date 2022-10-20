using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;
using AssertionException = UnityEngine.Assertions.AssertionException;
using Object = UnityEngine.Object;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public class ScriptableListTests
	{
		private TestScriptableList list;

		public bool IsReadOnly { get { return list.IsReadOnly; } set { list.IsReadOnly = value; } }

		[UnitySetUp]
		public IEnumerator Setup()
		{
			list = ScriptableObject.CreateInstance<TestScriptableList>();
			list.name = "Instance";

			yield return new EnterPlayMode(false);

			Assert.IsTrue(Application.isPlaying);
		}

		[UnityTearDown]
		public IEnumerator Teardown()
		{
			yield return new ExitPlayMode();

			Object.DestroyImmediate(list);
		}

		[Test]
		public void Add()
		{
			bool addEventInvoked = false;
			bool addOrInsertEventInvoked = false;

			list.OnAdded += i => { addEventInvoked = true; };
			list.OnAddedOrInserted += (index, item) =>
			{
				addOrInsertEventInvoked = true;
				Assert.AreEqual(0, index);
				Assert.AreEqual(1, item);
			};

			list.Add(1);

			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.IsTrue(addEventInvoked);
			Assert.IsTrue(addOrInsertEventInvoked);
		}

		[Test]
		public void Add_ReadOnly()
		{
			IsReadOnly = true;
			bool addEventInvoked = false;
			bool addOrInsertEventInvoked = false;

			list.OnAdded += i => { addEventInvoked = true; };
			list.OnAddedOrInserted += (index, item) => { addOrInsertEventInvoked = true; };

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be added to at runtime.");

			list.Add(1);

			Assert.AreEqual(0, list.Count);
			Assert.IsFalse(addEventInvoked);
			Assert.IsFalse(addOrInsertEventInvoked);
		}

		[Test]
		public void Add_Object()
		{
			bool addEventInvoked = false;
			bool addOrInsertEventInvoked = false;

			list.OnAdded += i => { addEventInvoked = true; };
			list.OnAddedOrInserted += (index, item) =>
			{
				addOrInsertEventInvoked = true;
				Assert.AreEqual(0, index);
				Assert.AreEqual(1, item);
			};

			int result = ((IList) list).Add(1);

			// Make sure the result is the index of the added item, which is 0.
			Assert.AreEqual(0, result);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.IsTrue(addEventInvoked);
			Assert.IsTrue(addOrInsertEventInvoked);
		}

		[Test]
		public void Add_Object_ReadOnly()
		{
			IsReadOnly = true;
			bool addEventInvoked = false;
			bool addOrInsertEventInvoked = false;

			list.OnAdded += i => { addEventInvoked = true; };
			list.OnAddedOrInserted += (index, item) => { addOrInsertEventInvoked = true; };

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be added to at runtime.");

			((IList) list).Add(1);

			Assert.AreEqual(0, list.Count);
			Assert.IsFalse(addEventInvoked);
			Assert.IsFalse(addOrInsertEventInvoked);
		}

		[Test]
		public void Add_ObjectInvalid()
		{
			LogAssert.Expect(LogType.Error, "System.Int32 is not assignable from System.String.");

			int result = ((IList) list).Add("invalid");

			Assert.AreEqual(-1, result);
		}

		[Test]
		public void Insert()
		{
			bool insertEventInvoked = false;
			bool addOrInsertEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnInserted += (index, item) =>
			{
				insertEventInvoked = true;
				Assert.AreEqual(1, index);
				Assert.AreEqual(3, item);
			};

			list.OnAddedOrInserted += (index, item) =>
			{
				addOrInsertEventInvoked = true;
				Assert.AreEqual(1, index);
				Assert.AreEqual(3, item);
			};

			list.Insert(1, 3);

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(3, list[1]);
			Assert.AreEqual(2, list[2]);
			Assert.IsTrue(insertEventInvoked);
			Assert.IsTrue(addOrInsertEventInvoked);
		}

		[Test]
		public void Insert_ReadOnly()
		{
			bool insertEventInvoked = false;
			bool addOrInsertEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnInserted += (index, item) => { insertEventInvoked = true; };

			list.OnAddedOrInserted += (index, item) => { addOrInsertEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be inserted to at runtime.");

			list.Insert(1, 3);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(insertEventInvoked);
			Assert.IsFalse(addOrInsertEventInvoked);
		}

		[Test]
		public void Insert_Object()
		{
			bool insertEventInvoked = false;
			bool addOrInsertEventInvoked = false;

			IList l = list;
			list.Add(1);
			list.Add(2);
			list.OnInserted += (index, item) =>
			{
				insertEventInvoked = true;
				Assert.AreEqual(1, index);
				Assert.AreEqual(3, item);
			};

			list.OnAddedOrInserted += (index, item) =>
			{
				addOrInsertEventInvoked = true;
				Assert.AreEqual(1, index);
				Assert.AreEqual(3, item);
			};

			l.Insert(1, 3);

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(3, list[1]);
			Assert.AreEqual(2, list[2]);
			Assert.IsTrue(insertEventInvoked);
			Assert.IsTrue(addOrInsertEventInvoked);
		}

		[Test]
		public void Insert_Object_ReadOnly()
		{
			bool insertEventInvoked = false;
			bool addOrInsertEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnInserted += (index, item) => { insertEventInvoked = true; };

			list.OnAddedOrInserted += (index, item) => { addOrInsertEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be inserted to at runtime.");

			((IList) list).Insert(1, 3);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(insertEventInvoked);
			Assert.IsFalse(addOrInsertEventInvoked);
		}

		[Test]
		public void Insert_ObjectInvalid()
		{
			LogAssert.Expect(LogType.Error, "System.Int32 is not assignable from System.String.");

			((IList) list).Insert(0, "invalid");
		}

		[Test]
		public void Set()
		{
			bool setEventInvoked = false;

			list.Add(0);
			list.OnSet += (index, oldValue, newValue) =>
			{
				Assert.AreEqual(0, index);
				Assert.AreEqual(0, oldValue);
				Assert.AreEqual(1, newValue);
				setEventInvoked = true;
			};

			list[0] = 1;

			Assert.AreEqual(1, list[0]);
			Assert.IsTrue(setEventInvoked);
		}

		[Test]
		public void Set_ReadOnly()
		{
			bool setEventInvoked = false;

			list.Add(0);
			list.OnSet += (index, oldValue, newValue) => { setEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be changed at runtime.");

			list[0] = 1;

			Assert.AreEqual(0, list[0]);
			Assert.IsFalse(setEventInvoked);
		}

		[Test]
		public void Set_SameValue()
		{
			bool setEventInvoked = false;

			list.Add(0);
			list.OnSet += (index, oldValue, newValue) => { setEventInvoked = true; };

			list[0] = 0;

			Assert.AreEqual(0, list[0]);
			Assert.IsFalse(setEventInvoked);
		}

		[Test]
		public void Set_SameValue_ReadOnly()
		{
			bool setEventInvoked = false;

			list.Add(0);
			list.OnSet += (index, oldValue, newValue) => { setEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be changed at runtime.");

			list[0] = 0;

			Assert.AreEqual(0, list[0]);
			Assert.IsFalse(setEventInvoked);
		}

		[Test]
		public void Set_Object()
		{
			bool setEventInvoked = false;

			list.Add(0);
			list.OnSet += (index, oldValue, newValue) =>
			{
				Assert.AreEqual(0, index);
				Assert.AreEqual(0, oldValue);
				Assert.AreEqual(1, newValue);
				setEventInvoked = true;
			};

			((IList) list)[0] = 1;

			Assert.AreEqual(1, ((IList) list)[0]);
			Assert.IsTrue(setEventInvoked);
		}

		[Test]
		public void Set_Object_ReadOnly()
		{
			bool setEventInvoked = false;

			list.Add(0);
			list.OnSet += (index, oldValue, newValue) => { setEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be changed at runtime.");

			((IList) list)[0] = 1;

			Assert.AreEqual(0, ((IList) list)[0]);
			Assert.IsFalse(setEventInvoked);
		}

		[Test]
		public void Set_ObjectInvalid()
		{
			LogAssert.Expect(LogType.Error, "System.Int32 is not assignable from System.String.");

			((IList) list)[0] = "invalid";
		}

		[Test]
		public void Remove()
		{
			bool removeEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) =>
			{
				removeEventInvoked = true;
				Assert.AreEqual(1, index);
				Assert.AreEqual(2, item);
			};

			bool removed = list.Remove(2);

			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.IsTrue(removed);
			Assert.IsTrue(removeEventInvoked);
		}

		[Test]
		public void Remove_None()
		{
			bool removeEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) => { removeEventInvoked = true; };

			bool removed = list.Remove(3);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(removed);
			Assert.IsFalse(removeEventInvoked);
		}

		[Test]
		public void Remove_ReadOnly()
		{
			bool removeEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) => { removeEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be removed from at runtime.");

			bool removed = list.Remove(2);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(removed);
			Assert.IsFalse(removeEventInvoked);
		}

		[Test]
		public void Remove_Object()
		{
			bool removeEventInvoked = false;

			IList l = list;
			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) =>
			{
				removeEventInvoked = true;
				Assert.AreEqual(1, index);
				Assert.AreEqual(2, item);
			};

			l.Remove(2);

			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.IsTrue(removeEventInvoked);
		}

		[Test]
		public void Remove_Object_ReadOnly()
		{
			bool removeEventInvoked = false;

			IList l = list;
			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) => { removeEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be removed from at runtime.");

			l.Remove(2);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(removeEventInvoked);
		}

		[Test]
		public void Remove_ObjectInvalid()
		{
			LogAssert.Expect(LogType.Error, "System.Int32 is not assignable from System.String.");

			((IList) list).Remove("invalid");
		}

		[Test]
		public void RemoveAt()
		{
			bool removeEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) =>
			{
				removeEventInvoked = true;
				Assert.AreEqual(1, index);
				Assert.AreEqual(2, item);
			};

			list.RemoveAt(1);

			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.IsTrue(removeEventInvoked);
		}

		[Test]
		public void RemoveAt_ReadOnly()
		{
			bool removeEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) => { removeEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be removed from at runtime.");

			list.RemoveAt(1);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(removeEventInvoked);
		}

		[Test]
		public void RemoveAt_InvalidIndex()
		{
			Exception exception = null;

			try
			{
				list.RemoveAt(0);
			}
			catch (ArgumentOutOfRangeException e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);
		}

		[Test]
		public void RemoveAll()
		{
			int removeEventInvoked = 0;

			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.OnRemoved += (index, item) => { removeEventInvoked++; };

			int removedCount = list.RemoveAll(i => i > 1);

			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, removeEventInvoked);
			Assert.AreEqual(2, removedCount);
		}

		[Test]
		public void RemoveAll_ReadOnly()
		{
			int removeEventInvoked = 0;

			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.OnRemoved += (index, item) => { removeEventInvoked++; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be removed from at runtime.");

			int removedCount = list.RemoveAll(i => i > 1);

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.AreEqual(0, removeEventInvoked);
			Assert.AreEqual(0, removedCount);
		}

		[Test]
		public void RemoveAll_NullPredicate()
		{
			Exception exception = null;

			try
			{
				list.RemoveAll(null);
			}
			catch (ArgumentNullException e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);
		}

		[Test]
		public void Clear()
		{
			bool clearEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnCleared += () => { clearEventInvoked = true; };

			list.Clear();

			Assert.AreEqual(0, list.Count);
			Assert.IsTrue(clearEventInvoked);
		}

		[Test]
		public void Clear_ReadOnly()
		{
			bool clearEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnCleared += () => { clearEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be cleared at runtime.");

			list.Clear();

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(clearEventInvoked);
		}

		[Test]
		public void Reverse()
		{
			list.Add(1);
			list.Add(2);
			list.Add(3);

			list.Reverse();

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(3, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(1, list[2]);
		}

		[Test]
		public void Reverse_ReadOnly()
		{
			list.Add(1);
			list.Add(2);
			list.Add(3);

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be reversed at runtime.");

			list.Reverse();

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
		}

		[Test]
		public void Reverse_Range()
		{
			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.Add(4);

			list.Reverse(1, 2);

			Assert.AreEqual(4, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(3, list[1]);
			Assert.AreEqual(2, list[2]);
			Assert.AreEqual(4, list[3]);
		}

		[Test]
		public void Reverse_Range_ReadOnly()
		{
			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.Add(4);

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be reversed at runtime.");

			list.Reverse(1, 2);

			Assert.AreEqual(4, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.AreEqual(4, list[3]);
		}

		[Test]
		public void Sort()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			list.Sort();

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.AreEqual(5, list[3]);
		}

		[Test]
		public void Sort_ReadOnly()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be sorted at runtime.");

			list.Sort();

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(5, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(1, list[2]);
			Assert.AreEqual(3, list[3]);
		}

		[Test]
		public void Sort_Comparer()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			list.Sort(Comparer<int>.Default);

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.AreEqual(5, list[3]);
		}

		[Test]
		public void Sort_Comparer_ReadOnly()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be sorted at runtime.");

			list.Sort(Comparer<int>.Default);

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(5, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(1, list[2]);
			Assert.AreEqual(3, list[3]);
		}

		[Test]
		public void Sort_IndexCount()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			list.Sort(1, 2, Comparer<int>.Default);

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(5, list[0]);
			Assert.AreEqual(1, list[1]);
			Assert.AreEqual(2, list[2]);
			Assert.AreEqual(3, list[3]);
		}

		[Test]
		public void Sort_IndexCount_ReadOnly()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be sorted at runtime.");

			list.Sort(1, 2, Comparer<int>.Default);

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(5, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(1, list[2]);
			Assert.AreEqual(3, list[3]);
		}

		[Test]
		public void Sort_Comparison()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			list.Sort((a, b) => a.CompareTo(b));

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.AreEqual(5, list[3]);
		}

		[Test]
		public void Sort_Comparison_ReadOnly()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be sorted at runtime.");

			list.Sort((a, b) => b.CompareTo(a));

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(5, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(1, list[2]);
			Assert.AreEqual(3, list[3]);
		}

		[Test]
		public void ToArray()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			int[] array = list.ToArray();

			Assert.AreEqual(4, array.Length);

			Assert.AreEqual(5, array[0]);
			Assert.AreEqual(2, array[1]);
			Assert.AreEqual(1, array[2]);
			Assert.AreEqual(3, array[3]);
		}

		[Test]
		public void TrimExcess()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);

			list.TrimExcess();

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(4, list.Capacity);
		}

		[Test]
		public void TrimExcess_ReadOnly()
		{
			list.Add(5);
			list.Add(2);
			list.Add(1);

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, "Instance (AuroraPunks.ScriptableValues.Tests.Editor.TestScriptableList) is marked as read only and cannot be trimmed at runtime.");

			list.TrimExcess();

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(4, list.Capacity);
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

			IEnumerator enumerator = ((IList) list).GetEnumerator();

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
		public void CopyTo()
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
			IsReadOnly = true;
			Assert.IsTrue(((IList) list).IsFixedSize);
		}

		[Test]
		public void IsFixedSizeWhenNotReadOnly()
		{
			IsReadOnly = false;
			Assert.IsFalse(((IList) list).IsFixedSize);
		}

		[Test]
		public void IsReadOnlyWhenReadOnly()
		{
			IsReadOnly = true;
			Assert.IsTrue(((IList) list).IsReadOnly);
		}

		[Test]
		public void IsReadOnlyWhenNotReadOnly()
		{
			IsReadOnly = false;
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
	}
}