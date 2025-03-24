#nullable enable

using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptablePoolTests<TType, TValue>
	{
		[Test]
		public void Create_InvokesOnPoolChanged([Values] EventType eventType)
		{
			// Arrange
			using PoolEventTracker<TValue> tracker = new PoolEventTracker<TValue>(pool, eventType, action => action == PoolAction.CreatedObject);

			// Act
			TValue value = pool.Get();

			// Assert
			Assert.IsTrue(tracker.HasBeenInvoked());

			Assert.AreEqual(PoolAction.CreatedObject, tracker.LastAction);
			Assert.AreEqual(value, tracker.LastItem);
		}
	}
}