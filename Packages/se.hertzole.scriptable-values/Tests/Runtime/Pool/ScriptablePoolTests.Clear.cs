#nullable enable

using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;
using Object = UnityEngine.Object;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptablePoolTests<TType, TValue>
	{
		[UnityTest]
		public IEnumerator Clear_ClearsAll()
		{
			// Arrange
			TValue value1 = pool.Get();
			TValue value2 = pool.Get();
			pool.Release(value1); // Make sure we have an inactive object.

			// Act
			pool.Clear();

			yield return null; // Wait for the next frame to ensure all objects are cleared.

			// Assert
			Assert.AreEqual(0, pool.CountAll);
			Assert.AreEqual(0, pool.CountActive);
			Assert.AreEqual(0, pool.CountInactive);

			Assert.IsFalse(pool.activeObjects.Contains(value1));
			Assert.IsFalse(pool.activeObjects.Contains(value2));
			Assert.IsFalse(pool.pool.Contains(value1));
			Assert.IsFalse(pool.pool.Contains(value2));

			if (value1 is Object obj1)
			{
				Assert.IsNull(obj1);
			}

			if (value2 is Object obj2)
			{
				Assert.IsNull(obj2);
			}
		}

		[Test]
		public void Clear_InvokesOnPoolChanged([Values] EventType eventType)
		{
			// Arrange
			TValue value1 = pool.Get();
			pool.Get();
			pool.Release(value1); // Make sure we have an inactive object.

			using PoolEventTracker<TValue> tracker = new PoolEventTracker<TValue>(pool, eventType, action => action == PoolAction.DestroyedObject);

			// Act
			pool.Clear();

			// Assert
			Assert.IsTrue(tracker.HasBeenInvoked());

			Assert.AreEqual(PoolAction.DestroyedObject, tracker.LastAction);
		}

		[Test]
		public void Clear_NotifiesPropertyChanged_CountAll()
		{
			// Arrange
			TValue value1 = pool.Get();
			pool.Get();
			pool.Release(value1); // Make sure we have an inactive object.

			// Act & Assert
			AssertPropertyChangesAreInvoked(ScriptablePool.countAllChangingEventArgs, ScriptablePool.countAllChangedEventArgs, x => x.Clear(), pool);
		}

		[Test]
		public void Clear_NotifiesPropertyChanged_CountActive()
		{
			// Arrange
			TValue value1 = pool.Get();
			pool.Get();
			pool.Release(value1); // Make sure we have an inactive object.

			// Act & Assert
			AssertPropertyChangesAreInvoked(ScriptablePool.countActiveChangingEventArgs, ScriptablePool.countActiveChangedEventArgs, x => x.Clear(), pool);
		}

		[Test]
		public void Clear_NotifiesPropertyChanged_CountInactive()
		{
			// Arrange
			TValue value1 = pool.Get();
			pool.Get();
			pool.Release(value1); // Make sure we have an inactive object.

			// Act & Assert
			AssertPropertyChangesAreInvoked(ScriptablePool.countInactiveChangingEventArgs, ScriptablePool.countInactiveChangedEventArgs, x => x.Clear(), pool);
		}
	}
}