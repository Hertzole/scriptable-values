using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests.Editor
{
	public class BaseEditorTest : BaseTest
	{
		[SetUp]
		public void Setup()
		{
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