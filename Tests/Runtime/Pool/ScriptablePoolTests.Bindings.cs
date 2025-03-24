#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptablePoolTests<TType, TValue>
	{
		[Test]
		public void Get_ChangesHashCode()
		{
			AssertHashCodeChanged(pool, i => i.Get());
		}

		[Test]
		public void Release_ChangesHashCode()
		{
			TValue obj = pool.Get();
			AssertHashCodeChanged(pool, i => i.Release(obj));
		}

		[Test]
		public void Clear_ChangesHashCode()
		{
			pool.Release(pool.Get());
			AssertHashCodeChanged(pool, i => i.Clear());
		}
	}
}
#endif