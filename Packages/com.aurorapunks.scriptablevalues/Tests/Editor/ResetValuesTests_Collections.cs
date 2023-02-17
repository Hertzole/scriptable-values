using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public partial class ResetValuesTests
	{
		[Test]
		public void Dictionary_ResetEvents()
		{
			TestScriptableDictionary instance = CreateInstance<TestScriptableDictionary>();

			bool addedWasInvoked = false;
			bool removedWasInvoked = false;
			bool clearedWasInvoked = false;
			bool setWasInvoked = false;

			instance.OnAdded += (_, _) => { addedWasInvoked = true; };
			instance.OnRemoved += (_, _) => { removedWasInvoked = true; };
			instance.OnCleared += () => { clearedWasInvoked = true; };
			instance.OnSet += (_, _, _) => { setWasInvoked = true; };

			instance.Test_OnStart();

			instance.Add(0, 1);
			instance.Remove(0);
			instance.Clear();
			instance[0] = 1;

			Assert.IsFalse(addedWasInvoked);
			Assert.IsFalse(removedWasInvoked);
			Assert.IsFalse(clearedWasInvoked);
			Assert.IsFalse(setWasInvoked);
		}

		[Test]
		public void Dictionary_ResetValues([ValueSource(nameof(bools))] bool isReadOnly, [ValueSource(nameof(bools))] bool clearOnStart)
		{
			TestScriptableDictionary instance = CreateInstance<TestScriptableDictionary>();

			instance.Add(0, 1);
			instance.Add(2, 3);
			instance.Add(4, 5);

			instance.IsReadOnly = isReadOnly;
			instance.ClearOnStart = clearOnStart;

			instance.Test_OnStart();

			if (!isReadOnly && clearOnStart)
			{
				Assert.AreEqual(0, instance.Count);
				Assert.AreEqual(0, instance.dictionary.Count);
				Assert.AreEqual(0, instance.values.Count);
				Assert.AreEqual(0, instance.keys.Count);
			}
			else
			{
				Assert.AreEqual(3, instance.Count);
				Assert.AreEqual(3, instance.dictionary.Count);
				Assert.AreEqual(3, instance.values.Count);
				Assert.AreEqual(3, instance.keys.Count);
			}
		}

		[Test]
		public void List_ResetEvents()
		{
			TestScriptableList instance = CreateInstance<TestScriptableList>();

			bool addedWasInvoked = false;
			bool removedWasInvoked = false;
			bool clearedWasInvoked = false;
			bool setWasInvoked = false;
			bool insertedWasInvoked = false;
			bool addedOrInsertedWasInvoked = false;

			instance.OnAdded += _ => { addedWasInvoked = true; };
			instance.OnRemoved += (_, _) => { removedWasInvoked = true; };
			instance.OnCleared += () => { clearedWasInvoked = true; };
			instance.OnSet += (_, _, _) => { setWasInvoked = true; };
			instance.OnInserted += (_, _) => { insertedWasInvoked = true; };
			instance.OnAddedOrInserted += (_, _) => { addedOrInsertedWasInvoked = true; };

			instance.Test_OnStart();

			instance.Add(0);
			instance[0] = 1;
			instance.Clear();
			instance.Insert(0, 42);
			instance.RemoveAt(0);

			Assert.IsFalse(addedWasInvoked);
			Assert.IsFalse(removedWasInvoked);
			Assert.IsFalse(clearedWasInvoked);
			Assert.IsFalse(setWasInvoked);
			Assert.IsFalse(insertedWasInvoked);
			Assert.IsFalse(addedOrInsertedWasInvoked);
		}
		
		[Test]
		public void List_ResetValues([ValueSource(nameof(bools))] bool isReadOnly, [ValueSource(nameof(bools))] bool clearOnStart)
		{
			TestScriptableList instance = CreateInstance<TestScriptableList>();

			instance.Add(0);
			instance.Add(1);
			instance.Add(2);

			instance.IsReadOnly = isReadOnly;
			instance.ClearOnStart = clearOnStart;

			instance.Test_OnStart();

			if (!isReadOnly && clearOnStart)
			{
				Assert.AreEqual(0, instance.Count);
				Assert.AreEqual(0, instance.list.Count);
			}
			else
			{
				Assert.AreEqual(3, instance.Count);
				Assert.AreEqual(3, instance.list.Count);
			}
		}
	}
}