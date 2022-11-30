namespace AuroraPunks.ScriptableValues
{
	public interface IPoolable
	{
		void OnUnpooled();

		void OnPooled();
	}
}