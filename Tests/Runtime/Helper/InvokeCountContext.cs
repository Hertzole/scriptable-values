using System.Collections.Generic;

namespace Hertzole.ScriptableValues.Tests
{
	public sealed class InvokeCountContext
	{
		public int invokeCount = 0;

		private readonly Dictionary<string, object> args = new Dictionary<string, object>();
		
		public void AddArg<T>(string key, T value)
		{
			args.Add(key, value);
		}
		
		public T GetArg<T>(string key)
		{
			return (T)args[key];
		}
	}
}