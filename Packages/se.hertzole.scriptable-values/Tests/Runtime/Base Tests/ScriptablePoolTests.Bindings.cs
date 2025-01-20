#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptablePoolTests
	{
		[Test]
		public void Get_NotifiesPropertyChanged_CountAll()
		{
			AssertNotifyPropertyChangedCalled<TestScriptableObjectPool>(nameof(ScriptablePool<object>.CountAll), instance => instance.Get());
		}

		[Test]
		public void Get_NotifiesPropertyChanged_CountActive()
		{
			AssertNotifyPropertyChangedCalled<TestScriptableObjectPool>(nameof(ScriptablePool<object>.CountActive), instance => instance.Get());
		}

		[Test]
		public void Get_ChangesHashCode()
		{
			AssertHashCodeChanged<TestScriptableObjectPool>(instance => instance.Get());
		}

		[Test]
		public void Return_NotifiesPropertyChanged_CountInactive()
		{
			TestScriptableObjectPool instance = CreateInstance<TestScriptableObjectPool>();
			PoolableScriptableObject obj = instance.Get();
			AssertNotifyPropertyChangedCalled(instance, nameof(ScriptablePool<object>.CountInactive), i => i.Return(obj));
		}

		[Test]
		public void Return_NotifiesPropertyChanged_CountActive()
		{
			TestScriptableObjectPool instance = CreateInstance<TestScriptableObjectPool>();
			PoolableScriptableObject obj = instance.Get();
			AssertNotifyPropertyChangedCalled(instance, nameof(ScriptablePool<object>.CountActive), i => i.Return(obj));
		}

		[Test]
		public void Return_ChangesHashCode()
		{
			TestScriptableObjectPool instance = CreateInstance<TestScriptableObjectPool>();
			PoolableScriptableObject obj = instance.Get();
			AssertHashCodeChanged(instance, i => i.Return(obj));
		}

		[Test]
		public void Clear_NotifiesPropertyChanged_CountActive()
		{
			TestScriptableObjectPool instance = CreateInstance<TestScriptableObjectPool>();
			instance.Get();
			AssertNotifyPropertyChangedCalled(instance, nameof(ScriptablePool<object>.CountActive), i => i.Clear());
		}

		[Test]
		public void Clear_NotifiesPropertyChanged_CountAll()
		{
			TestScriptableObjectPool instance = CreateInstance<TestScriptableObjectPool>();
			instance.Get();
			AssertNotifyPropertyChangedCalled(instance, nameof(ScriptablePool<object>.CountAll), i => i.Clear());
		}

		[Test]
		public void Clear_ChangesHashCode()
		{
			TestScriptableObjectPool instance = CreateInstance<TestScriptableObjectPool>();
			instance.Get();
			AssertHashCodeChanged(instance, i => i.Clear());
		}
	}
}
#endif