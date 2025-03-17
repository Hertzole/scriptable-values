#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System.Collections;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	partial class ScriptableListTests
	{
		[Test]
		public void Add_NotifiesPropertyChanged_Count()
		{
			AssertNotifyPropertyChangedCalled(list, nameof(list.Count), instance => instance.Add(1));
		}

		[Test]
		public void Add_ChangesHashCode()
		{
			AssertHashCodeChanged(list, instance => instance.Add(1));
		}

		[Test]
		public void Add_Object_NotifiesPropertyChanged_Count()
		{
			AssertNotifyPropertyChangedCalled(list, nameof(list.Count), instance => ((IList) instance).Add(1));
		}

		[Test]
		public void Add_Object_ChangesHashCode()
		{
			AssertHashCodeChanged(list, instance => ((IList) instance).Add(1));
		}

		[Test]
		public void AddRange_NotifiesPropertyChanged_Count()
		{
			AssertNotifyPropertyChangedCalled(list, nameof(list.Count), instance => instance.AddRange(new[] { 1, 2, 3 }));
		}

		[Test]
		public void AddRange_ChangesHashCode()
		{
			AssertHashCodeChanged(list, instance => instance.AddRange(new[] { 1, 2, 3 }));
		}

		[Test]
		public void Clear_NotifiesPropertyChanged_Count()
		{
			list.Add(1);
			AssertNotifyPropertyChangedCalled(list, nameof(list.Count), instance => instance.Clear());
		}

		[Test]
		public void Clear_ChangesHashCode()
		{
			list.Add(1);
			AssertHashCodeChanged(list, instance => instance.Clear());
		}

		[Test]
		public void Insert_NotifiesPropertyChanged_Count()
		{
			AssertNotifyPropertyChangedCalled(list, nameof(list.Count), instance => instance.Insert(0, 1));
		}

		[Test]
		public void Insert_ChangesHashCode()
		{
			AssertHashCodeChanged(list, instance => instance.Insert(0, 1));
		}

		[Test]
		public void Insert_Object_NotifiesPropertyChanged_Count()
		{
			AssertNotifyPropertyChangedCalled(list, nameof(list.Count), instance => ((IList) instance).Insert(0, 1));
		}

		[Test]
		public void Insert_Object_ChangesHashCode()
		{
			AssertHashCodeChanged(list, instance => ((IList) instance).Insert(0, 1));
		}

		[Test]
		public void InsertRange_NotifiesPropertyChanged_Count()
		{
			AssertNotifyPropertyChangedCalled(list, nameof(list.Count), instance => instance.InsertRange(0, new[] { 1, 2, 3 }));
		}

		[Test]
		public void InsertRange_ChangesHashCode()
		{
			AssertHashCodeChanged(list, instance => instance.InsertRange(0, new[] { 1, 2, 3 }));
		}

		[Test]
		public void Remove_NotifiesPropertyChanged_Count()
		{
			list.Add(1);
			AssertNotifyPropertyChangedCalled(list, nameof(list.Count), instance => instance.Remove(1));
		}

		[Test]
		public void Remove_ChangesHashCode()
		{
			list.Add(1);
			AssertHashCodeChanged(list, instance => instance.Remove(1));
		}

		[Test]
		public void Remove_Object_NotifiesPropertyChanged_Count()
		{
			list.Add(1);
			AssertNotifyPropertyChangedCalled(list, nameof(list.Count), instance => ((IList) instance).Remove(1));
		}

		[Test]
		public void Remove_Object_ChangesHashCode()
		{
			list.Add(1);
			AssertHashCodeChanged(list, instance => ((IList) instance).Remove(1));
		}

		[Test]
		public void RemoveAt_NotifiesPropertyChanged_Count()
		{
			list.Add(1);
			AssertNotifyPropertyChangedCalled(list, nameof(list.Count), instance => instance.RemoveAt(0));
		}

		[Test]
		public void RemoveAt_ChangesHashCode()
		{
			list.Add(1);
			AssertHashCodeChanged(list, instance => instance.RemoveAt(0));
		}

		[Test]
		public void EnsureCapacity_NotifiesPropertyChanged_Capacity()
		{
			AssertNotifyPropertyChangedCalled(list, nameof(list.Capacity), instance => instance.EnsureCapacity(10));
		}

		[Test]
		public void EnsureCapacity_ChangesHashCode()
		{
			AssertHashCodeChanged(list, instance => instance.EnsureCapacity(10));
		}

		[Test]
		public void TrimExcess_NotifiesPropertyChanged_Capacity()
		{
			list.EnsureCapacity(10);
			AssertNotifyPropertyChangedCalled(list, nameof(list.Capacity), instance => instance.TrimExcess());
		}

		[Test]
		public void TrimExcess_ChangesHashCode()
		{
			list.EnsureCapacity(10);
			AssertHashCodeChanged(list, instance => instance.TrimExcess());
		}

		[Test]
		public void SetEqualityCheck_NotifyPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled(list, nameof(list.SetEqualityCheck), instance => instance.SetEqualityCheck = !instance.SetEqualityCheck);
		}

		[Test]
		public void SetEqualityCheck_ChangesHashCode()
		{
			AssertHashCodeChanged(list, instance => instance.SetEqualityCheck = !instance.SetEqualityCheck);
		}

		[Test]
		public void ClearOnStart_NotifyPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled(list, nameof(list.ClearOnStart), instance => instance.ClearOnStart = !instance.ClearOnStart);
		}

		[Test]
		public void ClearOnStart_ChangesHashCode()
		{
			AssertHashCodeChanged(list, instance => instance.ClearOnStart = !instance.ClearOnStart);
		}

		[Test]
		public void IsReadOnly_NotifyPropertyChanged()
		{
			AssertNotifyPropertyChangedCalled(list, nameof(list.IsReadOnly), instance => instance.IsReadOnly = !instance.IsReadOnly);
		}

		[Test]
		public void IsReadOnly_ChangesHashCode()
		{
			AssertHashCodeChanged(list, instance => instance.IsReadOnly = !instance.IsReadOnly);
		}
	}
}
#endif