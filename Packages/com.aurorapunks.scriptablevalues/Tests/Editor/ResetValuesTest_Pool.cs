using NUnit.Framework;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public partial class ResetValuesTests
	{
		[Test]
		public void Pool_ResetEvents()
		{
			TestClassScriptablePool instance = CreateInstance<TestClassScriptablePool>();

			bool addedWasInvoked = false;
			bool removedWasInvoked = false;
			bool clearedWasInvoked = false;
			bool setWasInvoked = false;

			instance.OnGetObject += _ => { addedWasInvoked = true; };
			instance.OnReturnObject += _ => { removedWasInvoked = true; };
			instance.OnDestroyObject += _ => { clearedWasInvoked = true; };
			instance.OnCreateObject += _ => { setWasInvoked = true; };

			instance.Test_OnStart();

			TestClass obj = instance.Get();
			instance.Return(obj);
			instance.Clear();

			Assert.IsFalse(addedWasInvoked);
			Assert.IsFalse(removedWasInvoked);
			Assert.IsFalse(clearedWasInvoked);
			Assert.IsFalse(setWasInvoked);
		}
	}
}