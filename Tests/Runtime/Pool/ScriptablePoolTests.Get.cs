#nullable enable

using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;
using Object = UnityEngine.Object;

namespace Hertzole.ScriptableValues.Tests
{
    partial class ScriptablePoolTests<TType, TValue>
    {
        [Test]
        public void Get_RentsItem()
        {
            // Act
            TValue result = pool.Get();

            // Act
            Assert.IsNotNull(result);

            Assert.AreEqual(1, pool.CountAll);
            Assert.AreEqual(1, pool.CountActive);
            Assert.AreEqual(0, pool.CountInactive);

            Assert.AreEqual(result, pool.activeObjects[0]);
        }

        [Test]
        public void Get_ReturnSame()
        {
            // Arrange
            TValue value = pool.Get();
            pool.Release(value);

            // Act
            TValue value2 = pool.Get();

            // Assert
            Assert.AreEqual(value, value2);
        }

        [Test]
        public void Get_Poolable()
        {
            // Act
            TValue value = pool.Get();
            bool isPooled = GetIsPooled(value);

            // Assert
            Assert.IsFalse(isPooled);
        }

        [Test]
        public void Get_IsActive()
        {
            if (!IsType<GameObject>() && !IsType<Camera>())
            {
                // This test only applies to GameObjects and Components.
                return;
            }

            // Arrange
            TValue value = pool.Get();
            bool firstActive = GetIsActive(value);

            // Act
            pool.Release(value);
            bool releaseActive = GetIsActive(value);

            // Check multiple times as objects always start active.
            pool.Get();
            bool secondActive = GetIsActive(value);

            // Assert
            Assert.IsTrue(firstActive);
            Assert.IsFalse(releaseActive);
            Assert.IsTrue(secondActive);
        }

        [Test]
        public void Get_DestroyedObjects()
        {
            if (!IsType<GameObject>() && !IsType<Camera>())
            {
                // This test only applies to GameObjects.
                return;
            }

            // Arrange
            // Get and release an object to ensure the pool is not empty.
            TValue value = pool.Get();
            pool.Release(value);

            // Act
            // Destroy the object directly in the pool to simulate a scene change, or if the object is destroyed in the editor.
            Object.DestroyImmediate(pool.pool.Peek() as Object);

            value = pool.Get();

            // Assert
            Assert.IsNotNull(value);
        }

        [Test]
        public void Get_InvokesOnPoolChanged([Values] EventType eventType)
        {
            // Arrange
            using PoolEventTracker<TValue> tracker = new PoolEventTracker<TValue>(pool, action => action == PoolAction.RentedObject);

            // Act
            TValue result = pool.Get();

            // Assert
            Assert.IsTrue(tracker.HasBeenInvoked());

            Assert.AreEqual(PoolAction.RentedObject, tracker.LastAction);
            Assert.AreEqual(result, tracker.LastItem);
        }

        [Test]
        public void Get_NotifiesPropertyChange_CountAll()
        {
            AssertPropertyChangesAreInvoked(ScriptablePool.countAllChangingEventArgs, ScriptablePool.countAllChangedEventArgs, x => x.Get(), pool);
        }

        [Test]
        public void Get_NotifiesPropertyChange_CountActive()
        {
            AssertPropertyChangesAreInvoked(ScriptablePool.countActiveChangingEventArgs, ScriptablePool.countActiveChangedEventArgs, x => x.Get(), pool);
        }
    }
}