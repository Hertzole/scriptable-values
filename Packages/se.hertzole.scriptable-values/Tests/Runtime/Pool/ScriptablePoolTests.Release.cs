#nullable enable

using System;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptablePoolTests<TType, TValue>
	{
		[Test]
		public void Release_ReturnsItem()
		{
			// Arrange
			TValue value = pool.Get();

			// Act
			pool.Release(value);

			// Assert
			Assert.AreEqual(1, pool.CountAll);
			Assert.AreEqual(0, pool.CountActive);
			Assert.AreEqual(1, pool.CountInactive);

			Assert.AreEqual(value, pool.pool.Peek());
		}

		[Test]
		public void Release_Poolable()
		{
			// Arrange
			TValue value = pool.Get();

			// Act
			pool.Release(value);
			bool isPooled = GetIsPooled(value);

			// Assert
			Assert.IsTrue(isPooled);
		}

		[Test]
		public void Release_IsActive()
		{
			if (!IsType<GameObject>() && !IsType<Camera>())
			{
				// This test only applies to GameObjects and Components.
				return;
			}

			// Arrange
			TValue value = pool.Get();

			// Act
			pool.Release(value);
			bool isActive = GetIsActive(value);

			// Assert
			Assert.IsFalse(isActive);
		}

		[Test]
		public void Release_Null_ThrowsArgumentNullException()
		{
			// Act & Assert
			AssertThrows<ArgumentNullException>(() => pool.Release(null!));
		}

		[Test]
		public void Release_InvokesOnPoolChanged([Values] EventType eventType)
		{
			// Arrange
			TValue value = pool.Get();
			using PoolEventTracker<TValue> tracker = new PoolEventTracker<TValue>(pool);

			// Act
			pool.Release(value);

			// Assert
			Assert.IsTrue(tracker.HasBeenInvoked());

			Assert.AreEqual(PoolAction.ReleasedObject, tracker.LastAction);
			Assert.AreEqual(value, tracker.LastItem);
		}

		[Test]
		public void Release_NotifiesPropertyChange_CountActive()
		{
			// Arrange
			TValue value = pool.Get();

			// Act & Assert
			AssertPropertyChangesAreInvoked(ScriptablePool.countActiveChangingEventArgs, ScriptablePool.countActiveChangedEventArgs, x => x.Release(value),
				pool);
		}

		[Test]
		public void Release_NotifiesPropertyChange_CountInactive()
		{
			// Arrange
			TValue value = pool.Get();

			// Act & Assert
			AssertPropertyChangesAreInvoked(ScriptablePool.countInactiveChangingEventArgs, ScriptablePool.countInactiveChangedEventArgs, x => x.Release(value),
				pool);
		}
	}
}