#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using System;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

namespace Hertzole.ScriptableValues
{
	partial class RuntimeScriptableObject : IDataSourceViewHashProvider, INotifyBindablePropertyChanged
	{
		private event EventHandler<BindablePropertyChangedEventArgs> OnPropertyChanged;

		event EventHandler<BindablePropertyChangedEventArgs> INotifyBindablePropertyChanged.propertyChanged
		{
			add { OnPropertyChanged += value; }
			remove { OnPropertyChanged -= value; }
		}

		protected virtual long GetViewHashCode()
		{
			return GetHashCode();
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
}
#endif