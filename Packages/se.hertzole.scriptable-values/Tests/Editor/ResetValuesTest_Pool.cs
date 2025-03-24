using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests.Editor
{
	public partial class ResetValuesTests
	{
		[Test]
		public void Pool_ResetEvents()
		{
			TestClassScriptablePool instance = CreateInstance<TestClassScriptablePool>();

			bool poolChangedWasInvoked = false;

			instance.OnPoolChanged += (_, _) => { poolChangedWasInvoked = true; };

			instance.Test_OnStart();

			TestClass obj = instance.Get();
			instance.Release(obj);
			instance.Clear();

			Assert.IsFalse(poolChangedWasInvoked);
		}
	}
}