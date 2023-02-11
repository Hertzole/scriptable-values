using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;
using AssertionException = UnityEngine.Assertions.AssertionException;

namespace AuroraPunks.ScriptableValues.Tests
{
	public class ScriptableListTests : BaseRuntimeTest
	{
		private TestScriptableList list;

		public bool IsReadOnly { get { return list.IsReadOnly; } set { list.IsReadOnly = value; } }

		protected override void OnSetup()
		{
			list = CreateInstance<TestScriptableList>();
			list.name = "Instance";
		}

		[Test]
		public void Add()
		{
			bool addEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

			list.OnAdded += i => { addEventInvoked = true; };
			list.OnAddedOrInserted += (index, item) =>
			{
				addOrInsertEventInvoked = true;
				Assert.AreEqual(0, index);
				Assert.AreEqual(1, item);
			};

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Added, type);
			};

			list.Add(1);

			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.IsTrue(addEventInvoked);
			Assert.IsTrue(addOrInsertEventInvoked);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Add_ReadOnly()
		{
			IsReadOnly = true;
			bool addEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

			list.OnAdded += i => { addEventInvoked = true; };
			list.OnAddedOrInserted += (index, item) => { addOrInsertEventInvoked = true; };
			list.OnChanged += type => { changedEventInvoked = true; };

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be added to at runtime.");

			list.Add(1);

			Assert.AreEqual(0, list.Count);
			Assert.IsFalse(addEventInvoked);
			Assert.IsFalse(addOrInsertEventInvoked);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Add_Object()
		{
			bool addEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

			list.OnAdded += i => { addEventInvoked = true; };
			list.OnAddedOrInserted += (index, item) =>
			{
				addOrInsertEventInvoked = true;
				Assert.AreEqual(0, index);
				Assert.AreEqual(1, item);
			};

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Added, type);
			};

			int result = ((IList) list).Add(1);

			// Make sure the result is the index of the added item, which is 0.
			Assert.AreEqual(0, result);
			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.IsTrue(addEventInvoked);
			Assert.IsTrue(addOrInsertEventInvoked);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Add_Object_ReadOnly()
		{
			IsReadOnly = true;
			bool addEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

			list.OnAdded += i => { addEventInvoked = true; };
			list.OnAddedOrInserted += (index, item) => { addOrInsertEventInvoked = true; };
			list.OnChanged += type => { changedEventInvoked = true; };

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be added to at runtime.");

			((IList) list).Add(1);

			Assert.AreEqual(0, list.Count);
			Assert.IsFalse(addEventInvoked);
			Assert.IsFalse(addOrInsertEventInvoked);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Add_ObjectInvalid()
		{
			LogAssert.Expect(LogType.Error, "System.Int32 is not assignable from System.String.");

			int result = ((IList) list).Add("invalid");

			Assert.AreEqual(-1, result);
		}

		[Test]
		public void AddRange_Collection()
		{
			bool addEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

			int addedCount = 0;
			int addedOrInsertedCount = 0;
			int changedCount = 0;

			list.OnAdded += i =>
			{
				addEventInvoked = true;
				addedCount++;
			};

			list.OnAddedOrInserted += (index, item) =>
			{
				addOrInsertEventInvoked = true;
				addedOrInsertedCount++;
			};

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				changedCount++;
				Assert.AreEqual(ListChangeType.Added, type);
			};

			int[] array = { 1, 2, 3 };

			list.AddRange(array);

			Assert.AreEqual(array.Length, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.IsTrue(addEventInvoked);
			Assert.IsTrue(addOrInsertEventInvoked);
			Assert.IsTrue(changedEventInvoked);
			Assert.AreEqual(array.Length, addedCount);
			Assert.AreEqual(array.Length, addedOrInsertedCount);
			Assert.AreEqual(array.Length, changedCount);
		}

		[Test]
		public void AddRange_Enumerable()
		{
			bool addEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

			int addedCount = 0;
			int addedOrInsertedCount = 0;
			int changedCount = 0;

			list.OnAdded += i =>
			{
				addEventInvoked = true;
				addedCount++;
			};

			list.OnAddedOrInserted += (index, item) =>
			{
				addOrInsertEventInvoked = true;
				addedOrInsertedCount++;
			};

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				changedCount++;
				Assert.AreEqual(ListChangeType.Added, type);
			};

			IEnumerable<int> array = Enumerable.Range(1, 3);
			int arrayLength = array.Count();

			list.AddRange(array);

			Assert.AreEqual(arrayLength, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.IsTrue(addEventInvoked);
			Assert.IsTrue(addOrInsertEventInvoked);
			Assert.IsTrue(changedEventInvoked);
			Assert.AreEqual(arrayLength, addedCount);
			Assert.AreEqual(arrayLength, addedOrInsertedCount);
			Assert.AreEqual(arrayLength, changedCount);
		}

		[Test]
		public void AddRange_Readonly()
		{
			IsReadOnly = true;
			bool addEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

			list.OnAdded += i => { addEventInvoked = true; };
			list.OnAddedOrInserted += (index, item) => { addOrInsertEventInvoked = true; };
			list.OnChanged += type => { changedEventInvoked = true; };

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be added to at runtime.");

			int[] array = { 1, 2, 3 };

			list.AddRange(array);

			Assert.AreEqual(0, list.Count);
			Assert.IsFalse(addEventInvoked);
			Assert.IsFalse(addOrInsertEventInvoked);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void AddRange_Null()
		{
			NUnit.Framework.Assert.That(() => list.AddRange(null), Throws.ArgumentNullException);
		}

		[Test]
		public void Insert()
		{
			bool insertEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

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

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Inserted, type);
			};

			list.Insert(1, 3);

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(3, list[1]);
			Assert.AreEqual(2, list[2]);
			Assert.IsTrue(insertEventInvoked);
			Assert.IsTrue(addOrInsertEventInvoked);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Insert_ReadOnly()
		{
			bool insertEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnInserted += (index, item) => { insertEventInvoked = true; };

			list.OnAddedOrInserted += (index, item) => { addOrInsertEventInvoked = true; };

			list.OnChanged += type => { changedEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be inserted to at runtime.");

			list.Insert(1, 3);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(insertEventInvoked);
			Assert.IsFalse(addOrInsertEventInvoked);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Insert_Object()
		{
			bool insertEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

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

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Inserted, type);
			};

			l.Insert(1, 3);

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(3, list[1]);
			Assert.AreEqual(2, list[2]);
			Assert.IsTrue(insertEventInvoked);
			Assert.IsTrue(addOrInsertEventInvoked);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Insert_Object_ReadOnly()
		{
			bool insertEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnInserted += (index, item) => { insertEventInvoked = true; };

			list.OnAddedOrInserted += (index, item) => { addOrInsertEventInvoked = true; };

			list.OnChanged += type => { changedEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be inserted to at runtime.");

			((IList) list).Insert(1, 3);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(insertEventInvoked);
			Assert.IsFalse(addOrInsertEventInvoked);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Insert_ObjectInvalid()
		{
			LogAssert.Expect(LogType.Error, "System.Int32 is not assignable from System.String.");

			((IList) list).Insert(0, "invalid");
		}

		[Test]
		public void InsertRange_Collection()
		{
			bool insertEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);

			int insertedCount = 0;
			int addedOrInsertedCount = 0;
			int changedCount = 0;

			list.OnInserted += (index, item) =>
			{
				insertEventInvoked = true;
				insertedCount++;
			};

			list.OnAddedOrInserted += (index, item) =>
			{
				addOrInsertEventInvoked = true;
				addedOrInsertedCount++;
			};

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				changedCount++;
				Assert.AreEqual(ListChangeType.Inserted, type);
			};

			int[] items = { 3, 4, 5 };

			list.InsertRange(1, items);

			Assert.AreEqual(5, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(3, list[1]);
			Assert.AreEqual(4, list[2]);
			Assert.AreEqual(5, list[3]);
			Assert.AreEqual(2, list[4]);
			Assert.IsTrue(insertEventInvoked);
			Assert.IsTrue(addOrInsertEventInvoked);
			Assert.IsTrue(changedEventInvoked);
			Assert.AreEqual(items.Length, insertedCount);
			Assert.AreEqual(items.Length, addedOrInsertedCount);
			Assert.AreEqual(items.Length, changedCount);
		}

		[Test]
		public void InsertRange_Enumerable()
		{
			bool insertEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);

			int insertedCount = 0;
			int addedOrInsertedCount = 0;
			int changedCount = 0;

			list.OnInserted += (index, item) =>
			{
				insertEventInvoked = true;
				insertedCount++;
			};

			list.OnAddedOrInserted += (index, item) =>
			{
				addOrInsertEventInvoked = true;
				addedOrInsertedCount++;
			};

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				changedCount++;
				Assert.AreEqual(ListChangeType.Inserted, type);
			};

			IEnumerable<int> items = Enumerable.Range(3, 3);
			int count = items.Count();

			list.InsertRange(1, items);

			Assert.AreEqual(5, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(3, list[1]);
			Assert.AreEqual(4, list[2]);
			Assert.AreEqual(5, list[3]);
			Assert.AreEqual(2, list[4]);
			Assert.IsTrue(insertEventInvoked);
			Assert.IsTrue(addOrInsertEventInvoked);
			Assert.IsTrue(changedEventInvoked);
			Assert.AreEqual(count, insertedCount);
			Assert.AreEqual(count, addedOrInsertedCount);
			Assert.AreEqual(count, changedCount);
		}

		[Test]
		public void InsertRange_ReadOnly()
		{
			bool insertEventInvoked = false;
			bool addOrInsertEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnInserted += (index, item) => { insertEventInvoked = true; };

			list.OnAddedOrInserted += (index, item) => { addOrInsertEventInvoked = true; };

			list.OnChanged += type => { changedEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be inserted to at runtime.");

			list.InsertRange(1, new[] { 3, 4, 5 });

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(insertEventInvoked);
			Assert.IsFalse(addOrInsertEventInvoked);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void InsertRange_LessThanZero_InvalidIndex()
		{
			NUnit.Framework.Assert.That(() => list.InsertRange(-1, new[] { 1, 2, 3 }), Throws.TypeOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void InsertRange_GreatherThanCount_InvalidIndex()
		{
			NUnit.Framework.Assert.That(() => list.InsertRange(1, new[] { 1, 2, 3 }), Throws.TypeOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void InsertRange_Null()
		{
			NUnit.Framework.Assert.That(() => list.InsertRange(0, null), Throws.TypeOf<ArgumentNullException>());
		}

		[Test]
		public void Set()
		{
			bool setEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(0);
			list.OnSet += (index, oldValue, newValue) =>
			{
				Assert.AreEqual(0, index);
				Assert.AreEqual(0, oldValue);
				Assert.AreEqual(1, newValue);
				setEventInvoked = true;
			};

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Replaced, type);
			};

			list[0] = 1;

			Assert.AreEqual(1, list[0]);
			Assert.IsTrue(setEventInvoked);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Set_ReadOnly()
		{
			bool setEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(0);
			list.OnSet += (index, oldValue, newValue) => { setEventInvoked = true; };
			list.OnChanged += type => { changedEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be changed at runtime.");

			list[0] = 1;

			Assert.AreEqual(0, list[0]);
			Assert.IsFalse(setEventInvoked);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Set_SameValue([ValueSource(nameof(bools))] bool setEqualityCheck)
		{
			bool setEventInvoked = false;
			bool changedEventInvoked = false;

			list.SetEqualityCheck = setEqualityCheck;

			list.Add(0);
			list.OnSet += (index, oldValue, newValue) => { setEventInvoked = true; };
			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Replaced, type);
			};

			list[0] = 0;

			Assert.AreEqual(0, list[0]);

			if (setEqualityCheck)
			{
				Assert.IsFalse(setEventInvoked);
				Assert.IsFalse(changedEventInvoked);
			}
			else
			{
				Assert.IsTrue(setEventInvoked);
				Assert.IsTrue(changedEventInvoked);
			}
		}

		[Test]
		public void Set_SameValue_ReadOnly()
		{
			bool setEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(0);
			list.OnSet += (index, oldValue, newValue) => { setEventInvoked = true; };
			list.OnChanged += type => { changedEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be changed at runtime.");

			list[0] = 0;

			Assert.AreEqual(0, list[0]);
			Assert.IsFalse(setEventInvoked);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Set_Object()
		{
			bool setEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(0);
			list.OnSet += (index, oldValue, newValue) =>
			{
				Assert.AreEqual(0, index);
				Assert.AreEqual(0, oldValue);
				Assert.AreEqual(1, newValue);
				setEventInvoked = true;
			};

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Replaced, type);
			};

			((IList) list)[0] = 1;

			Assert.AreEqual(1, ((IList) list)[0]);
			Assert.IsTrue(setEventInvoked);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Set_Object_ReadOnly()
		{
			bool setEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(0);
			list.OnSet += (index, oldValue, newValue) => { setEventInvoked = true; };
			list.OnChanged += type => { changedEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be changed at runtime.");

			((IList) list)[0] = 1;

			Assert.AreEqual(0, ((IList) list)[0]);
			Assert.IsFalse(setEventInvoked);
			Assert.IsFalse(changedEventInvoked);
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
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) =>
			{
				removeEventInvoked = true;
				Assert.AreEqual(1, index);
				Assert.AreEqual(2, item);
			};

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Removed, type);
			};

			bool removed = list.Remove(2);

			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.IsTrue(removed);
			Assert.IsTrue(removeEventInvoked);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Remove_None()
		{
			bool removeEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) => { removeEventInvoked = true; };
			list.OnChanged += type => { changedEventInvoked = true; };

			bool removed = list.Remove(3);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(removed);
			Assert.IsFalse(removeEventInvoked);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Remove_ReadOnly()
		{
			bool removeEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) => { removeEventInvoked = true; };
			list.OnChanged += type => { changedEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be removed from at runtime.");

			bool removed = list.Remove(2);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(removed);
			Assert.IsFalse(removeEventInvoked);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Remove_Object()
		{
			bool removeEventInvoked = false;
			bool changedEventInvoked = false;

			IList l = list;
			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) =>
			{
				removeEventInvoked = true;
				Assert.AreEqual(1, index);
				Assert.AreEqual(2, item);
			};

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Removed, type);
			};

			l.Remove(2);

			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.IsTrue(removeEventInvoked);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Remove_Object_ReadOnly()
		{
			bool removeEventInvoked = false;
			bool changedEventInvoked = false;

			IList l = list;
			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) => { removeEventInvoked = true; };
			list.OnChanged += type => { changedEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be removed from at runtime.");

			l.Remove(2);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(removeEventInvoked);
			Assert.IsFalse(changedEventInvoked);
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
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) =>
			{
				removeEventInvoked = true;
				Assert.AreEqual(1, index);
				Assert.AreEqual(2, item);
			};

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Removed, type);
			};

			list.RemoveAt(1);

			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.IsTrue(removeEventInvoked);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void RemoveAt_ReadOnly()
		{
			bool removeEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnRemoved += (index, item) => { removeEventInvoked = true; };
			list.OnChanged += type => { changedEventInvoked = true; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be removed from at runtime.");

			list.RemoveAt(1);

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(removeEventInvoked);
			Assert.IsFalse(changedEventInvoked);
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
			int changedEventInvoked = 0;

			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.OnRemoved += (index, item) => { removeEventInvoked++; };
			list.OnChanged += type =>
			{
				changedEventInvoked++;
				Assert.AreEqual(ListChangeType.Removed, type);
			};

			int removedCount = list.RemoveAll(i => i > 1);

			Assert.AreEqual(1, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, removeEventInvoked);
			Assert.AreEqual(2, removedCount);
			Assert.AreEqual(2, changedEventInvoked);
		}

		[Test]
		public void RemoveAll_ReadOnly()
		{
			int removeEventInvoked = 0;
			int changedEventInvoked = 0;

			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.OnRemoved += (index, item) => { removeEventInvoked++; };
			list.OnChanged += type => { changedEventInvoked++; };

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be removed from at runtime.");

			int removedCount = list.RemoveAll(i => i > 1);

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.AreEqual(0, removeEventInvoked);
			Assert.AreEqual(0, removedCount);
			Assert.AreEqual(0, changedEventInvoked);
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
		public void RemoveRange()
		{
			bool removeEventInvoked = false;
			int removeCount = 0;
			int changedEventInvoked = 0;

			list.AddRange(Enumerable.Range(0, 100));

			Assert.AreEqual(list.Count, 100);
			for (int i = 0; i < list.Count; i++)
			{
				Assert.AreEqual(i, list[i]);
			}

			list.OnRemoved += (index, item) =>
			{
				removeEventInvoked = true;
				removeCount++;
			};

			list.OnChanged += type =>
			{
				changedEventInvoked++;
				Assert.AreEqual(ListChangeType.Removed, type);
			};

			list.RemoveRange(10, 20);

			Assert.AreEqual(list.Count, 80);

			for (int i = 0; i < 20; i++)
			{
				Assert.AreEqual(30 + i, list[i + 10]);
			}

			Assert.IsTrue(removeEventInvoked);
			Assert.AreEqual(20, removeCount);
			Assert.AreEqual(20, changedEventInvoked);
		}

		[Test]
		public void RemoveRange_ReadOnly()
		{
			bool removeEventInvoked = false;
			int removeCount = 0;
			int changedEventInvoked = 0;

			list.AddRange(Enumerable.Range(0, 100));

			Assert.AreEqual(list.Count, 100);
			for (int i = 0; i < list.Count; i++)
			{
				Assert.AreEqual(i, list[i]);
			}

			list.OnRemoved += (index, item) =>
			{
				removeEventInvoked = true;
				removeCount++;
			};

			list.OnChanged += type =>
			{
				changedEventInvoked++;
				Assert.AreEqual(ListChangeType.Removed, type);
			};

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be removed from at runtime.");

			list.RemoveRange(10, 20);

			Assert.AreEqual(list.Count, 100);

			for (int i = 0; i < 100; i++)
			{
				Assert.AreEqual(i, list[i]);
			}

			Assert.IsFalse(removeEventInvoked);
			Assert.AreEqual(0, removeCount);
			Assert.AreEqual(0, changedEventInvoked);
		}

		[Test]
		public void RemoveRange_CountLessThanZero()
		{
			list.AddRange(Enumerable.Range(0, 100));
			NUnit.Framework.Assert.That(() => list.RemoveRange(0, -1), Throws.TypeOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void RemoveRange_IndexLessThanZero()
		{
			list.AddRange(Enumerable.Range(0, 100));
			NUnit.Framework.Assert.That(() => list.RemoveRange(-1, 1), Throws.TypeOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void RemoveRange_IndexPlusCountGreaterThanCount()
		{
			list.AddRange(Enumerable.Range(0, 100));
			NUnit.Framework.Assert.That(() => list.RemoveRange(50, 51), Throws.TypeOf<ArgumentException>());
		}

		[Test]
		public void RemoveRange_IndexGreaterThanCount()
		{
			list.AddRange(Enumerable.Range(0, 100));
			NUnit.Framework.Assert.That(() => list.RemoveRange(101, 1), Throws.TypeOf<ArgumentOutOfRangeException>());
		}

		[Test]
		public void Clear()
		{
			bool clearEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnCleared += () => { clearEventInvoked = true; };
			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Cleared, type);
			};

			list.Clear();

			Assert.AreEqual(0, list.Count);
			Assert.IsTrue(clearEventInvoked);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Clear_ReadOnly()
		{
			bool clearEventInvoked = false;
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.OnCleared += () => { clearEventInvoked = true; };
			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Cleared, type);
			};

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be cleared at runtime.");

			list.Clear();

			Assert.AreEqual(2, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.IsFalse(clearEventInvoked);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Reverse()
		{
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.Add(3);

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Reversed, type);
			};

			list.Reverse();

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(3, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(1, list[2]);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Reverse_ReadOnly()
		{
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.Add(3);

			IsReadOnly = true;

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Reversed, type);
			};

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be reversed at runtime.");

			list.Reverse();

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Reverse_Range()
		{
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.Add(4);

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Reversed, type);
			};

			list.Reverse(1, 2);

			Assert.AreEqual(4, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(3, list[1]);
			Assert.AreEqual(2, list[2]);
			Assert.AreEqual(4, list[3]);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Reverse_Range_ReadOnly()
		{
			bool changedEventInvoked = false;

			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.Add(4);

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Reversed, type);
			};

			IsReadOnly = true;

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be reversed at runtime.");

			list.Reverse(1, 2);

			Assert.AreEqual(4, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.AreEqual(4, list[3]);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Sort()
		{
			bool changedEventInvoked = false;

			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Sorted, type);
			};

			list.Sort();

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.AreEqual(5, list[3]);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Sort_ReadOnly()
		{
			bool changedEventInvoked = false;

			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			IsReadOnly = true;

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Sorted, type);
			};

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be sorted at runtime.");

			list.Sort();

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(5, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(1, list[2]);
			Assert.AreEqual(3, list[3]);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Sort_Comparer()
		{
			bool changedEventInvoked = false;

			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Sorted, type);
			};

			list.Sort(Comparer<int>.Default);

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.AreEqual(5, list[3]);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Sort_Comparer_ReadOnly()
		{
			bool changedEventInvoked = false;

			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			IsReadOnly = true;

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Sorted, type);
			};

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be sorted at runtime.");

			list.Sort(Comparer<int>.Default);

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(5, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(1, list[2]);
			Assert.AreEqual(3, list[3]);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Sort_IndexCount()
		{
			bool changedEventInvoked = false;

			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Sorted, type);
			};

			list.Sort(1, 2, Comparer<int>.Default);

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(5, list[0]);
			Assert.AreEqual(1, list[1]);
			Assert.AreEqual(2, list[2]);
			Assert.AreEqual(3, list[3]);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Sort_IndexCount_ReadOnly()
		{
			bool changedEventInvoked = false;

			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			IsReadOnly = true;

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Sorted, type);
			};

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be sorted at runtime.");

			list.Sort(1, 2, Comparer<int>.Default);

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(5, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(1, list[2]);
			Assert.AreEqual(3, list[3]);
			Assert.IsFalse(changedEventInvoked);
		}

		[Test]
		public void Sort_Comparison()
		{
			bool changedEventInvoked = false;

			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Sorted, type);
			};

			list.Sort((a, b) => a.CompareTo(b));

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
			Assert.AreEqual(5, list[3]);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void Sort_Comparison_ReadOnly()
		{
			bool changedEventInvoked = false;

			list.Add(5);
			list.Add(2);
			list.Add(1);
			list.Add(3);

			IsReadOnly = true;

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Sorted, type);
			};

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be sorted at runtime.");

			list.Sort((a, b) => b.CompareTo(a));

			Assert.AreEqual(4, list.Count);

			Assert.AreEqual(5, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(1, list[2]);
			Assert.AreEqual(3, list[3]);
			Assert.IsFalse(changedEventInvoked);
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
			bool changedEventInvoked = false;

			list.Add(5);
			list.Add(2);
			list.Add(1);

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Trimmed, type);
			};

			list.TrimExcess();

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(4, list.Capacity);
			Assert.IsTrue(changedEventInvoked);
		}

		[Test]
		public void TrimExcess_ReadOnly()
		{
			bool changedEventInvoked = false;

			list.Add(5);
			list.Add(2);
			list.Add(1);

			IsReadOnly = true;

			list.OnChanged += type =>
			{
				changedEventInvoked = true;
				Assert.AreEqual(ListChangeType.Trimmed, type);
			};

			LogAssert.Expect(LogType.Error, $"{list} is marked as read only and cannot be trimmed at runtime.");

			list.TrimExcess();

			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(4, list.Capacity);
			Assert.IsFalse(changedEventInvoked);
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
	}
}