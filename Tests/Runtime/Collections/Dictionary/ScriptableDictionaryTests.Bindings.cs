#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System.Collections;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableDictionaryTests
	{
		[Test]
		public void Add_NotifiesPropertyChanged_Count()
		{
			AssertNotifyPropertyChangedCalled(dictionary, nameof(dictionary.Count), instance => instance.Add(1, 1));
		}

		[Test]
		public void Add_ChangesHashCode()
		{
			AssertHashCodeChanged(dictionary, instance => instance.Add(1, 1));
		}

		[Test]
		public void Add_Object_NotifiesPropertyChanged_Count()
		{
			AssertNotifyPropertyChangedCalled(dictionary, nameof(dictionary.Count), instance => ((IDictionary) instance).Add(1, 1));
		}

		[Test]
		public void Add_Object_ChangesHashCode()
		{
			AssertHashCodeChanged(dictionary, instance => ((IDictionary) instance).Add(1, 1));
		}

		[Test]
		public void TryAdd_ChangesHashCode()
		{
			AssertHashCodeChanged(dictionary, instance => instance.TryAdd(1, 1));
		}

		[Test]
		public void Clear_NotifiesPropertyChanged_Count()
		{
			dictionary.Add(1, 1);
			AssertNotifyPropertyChangedCalled(dictionary, nameof(dictionary.Count), instance => instance.Clear());
		}

		[Test]
		public void Clear_ChangesHashCode()
		{
			dictionary.Add(1, 1);
			AssertHashCodeChanged(dictionary, instance => instance.Clear());
		}

		[Test]
		public void Remove_NotifiesPropertyChanged_Count()
		{
			dictionary.Add(1, 1);
			AssertNotifyPropertyChangedCalled(dictionary, nameof(dictionary.Count), instance => instance.Remove(1));
		}

		[Test]
		public void Remove_ChangesHashCode()
		{
			dictionary.Add(1, 1);
			AssertHashCodeChanged(dictionary, instance => instance.Remove(1));
		}

		[Test]
		public void Remove_Object_NotifiesPropertyChanged_Count()
		{
			dictionary.Add(1, 1);
			AssertNotifyPropertyChangedCalled(dictionary, nameof(dictionary.Count), instance => ((IDictionary) instance).Remove(1));
		}

		[Test]
		public void Remove_Object_ChangesHashCode()
		{
			dictionary.Add(1, 1);
			AssertHashCodeChanged(dictionary, instance => ((IDictionary) instance).Remove(1));
		}

		[Test]
		public void TryAdd_NotifiesPropertyChanged_Count()
		{
			AssertNotifyPropertyChangedCalled(dictionary, nameof(dictionary.Count), instance => instance.TryAdd(1, 1));
		}

		[Test]
		public void SetEqualityCheck_NotifiesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled(dictionary, nameof(dictionary.SetEqualityCheck),
				instance => instance.SetEqualityCheck = !instance.SetEqualityCheck);
		}

		[Test]
		public void SetEqualityCheck_ChangesHashCode()
		{
			AssertHashCodeChanged(dictionary, instance => instance.SetEqualityCheck = !instance.SetEqualityCheck);
		}

		[Test]
		public void ClearOnStart_NotifiesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled(dictionary, nameof(dictionary.ClearOnStart), instance => instance.ClearOnStart = !instance.ClearOnStart);
		}

		[Test]
		public void ClearOnStart_ChangesHashCode()
		{
			AssertHashCodeChanged(dictionary, instance => instance.ClearOnStart = !instance.ClearOnStart);
		}

		[Test]
		public void IsReadOnly_NotifiesPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled(dictionary, nameof(dictionary.IsReadOnly), instance => instance.IsReadOnly = !instance.IsReadOnly);
		}

		[Test]
		public void IsReadOnly_ChangesHashCode()
		{
			AssertHashCodeChanged(dictionary, instance => instance.IsReadOnly = !instance.IsReadOnly);
		}
	}
}
#endif