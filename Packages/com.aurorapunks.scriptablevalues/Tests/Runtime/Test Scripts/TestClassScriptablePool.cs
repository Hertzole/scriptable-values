namespace AuroraPunks.ScriptableValues.Tests
{
	public class TestClass : IPoolable
	{
		public bool IsPooled { get; set; }
		
		public object Data { get; set; }

		public void OnUnpooled()
		{
			IsPooled = false;
		}

		public void OnPooled()
		{
			IsPooled = true;
		}
	}

	public class TestClassScriptablePool : ScriptablePool<TestClass>
	{
		protected override TestClass CreateObject()
		{
			return new TestClass();
		}

		protected override void DestroyObject(TestClass item) { }
	}
}