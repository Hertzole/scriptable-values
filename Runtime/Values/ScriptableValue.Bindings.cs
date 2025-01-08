#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues
{
	partial class ScriptableValue : IDataSourceViewHashProvider, INotifyBindablePropertyChanged
	{
		private event EventHandler<BindablePropertyChangedEventArgs> OnPropertyChanged;

		event EventHandler<BindablePropertyChangedEventArgs> INotifyBindablePropertyChanged.propertyChanged
		{
			add { OnPropertyChanged += value; }
			remove { OnPropertyChanged -= value; }
		}

		protected virtual long GetViewHashCode()
		{
			unchecked
			{
				long hash = 17;
				hash = hash * 23 + isReadOnly.GetHashCode();
				hash = hash * 23 + resetValueOnStart.GetHashCode();
				hash = hash * 23 + setEqualityCheck.GetHashCode();
				return hash;
			}
		}

		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			OnPropertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(propertyName));
		}

		long IDataSourceViewHashProvider.GetViewHashCode()
		{
			return GetViewHashCode();
		}
	}

	partial class ScriptableValue<T>
	{
		protected override long GetViewHashCode()
		{
			unchecked
			{
				long hash = 17;
				hash = hash * 23 + base.GetViewHashCode();
				hash = hash * 23 + EqualityComparer<T>.Default.GetHashCode(Value);
				hash = hash * 23 + EqualityComparer<T>.Default.GetHashCode(PreviousValue);
				hash = hash * 23 + EqualityComparer<T>.Default.GetHashCode(DefaultValue);
				return hash;
			}
		}
	}
}
#endif