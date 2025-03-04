using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.ScriptableValues.Tests
{
	public class BaseRuntimeTest : BaseTest
	{
		[SetUp]
		public void Setup()
		{
			Assert.AreEqual(0, objects.Count);

			OnSetup();
		}

		protected virtual void OnSetup() { }

		[TearDown]
		public void TearDown()
		{
			OnTearDown();

			DestroyObjects(true);
		}

		protected virtual void OnTearDown() { }
	}
}