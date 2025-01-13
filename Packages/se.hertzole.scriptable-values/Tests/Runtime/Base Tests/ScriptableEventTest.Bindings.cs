#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableEventTest<TType, TValue> 
	{
		[Test]
		public void Binding_PreviousArgs_InvokesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled<TType>(nameof(ScriptableEvent<TValue>.PreviousArgs), instance =>
			{
				instance.Invoke(MakeDifferentValue<TValue>(instance.PreviousArgs));
				instance.Invoke(MakeDifferentValue<TValue>(instance.PreviousArgs)); // Do it twice to make sure previous args changes from the default value.
			});
		}

		[Test]
		public void Binding_PreviousArgs_ChangesHashCode()
		{
			AssertHashCodeChanged<TType>(instance =>
			{
				instance.Invoke(MakeDifferentValue<TValue>(instance.PreviousArgs));
				instance.Invoke(MakeDifferentValue<TValue>(instance.PreviousArgs)); // Do it twice to make sure previous args changes from the default value.
			});
		}
	}
}
#endif