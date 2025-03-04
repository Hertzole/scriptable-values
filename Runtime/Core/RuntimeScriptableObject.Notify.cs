using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Hertzole.ScriptableValues.Helpers;
using Unity.Properties;
using UnityEngine.Assertions;
#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using UnityEngine.UIElements;
#endif // SCRIPTABLE_VALUES_RUNTIME_BINDING

namespace Hertzole.ScriptableValues
{
	partial class RuntimeScriptableObject :
		INotifyPropertyChanging,
		INotifyPropertyChanged,
#if SCRIPTABLE_VALUES_RUNTIME_BINDING
		IDataSourceViewHashProvider,
		INotifyBindablePropertyChanged
#endif // SCRIPTABLE_VALUES_RUNTIME_BINDING
	{
		private event PropertyChangingEventHandler OnNotifyPropertyChanging;
		private event PropertyChangedEventHandler OnNotifyPropertyChanged;

		event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
		{
			add { OnNotifyPropertyChanging += value; }
			remove { OnNotifyPropertyChanging -= value; }
		}
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add { OnNotifyPropertyChanged += value; }
			remove { OnNotifyPropertyChanged -= value; }
		}

		protected void NotifyPropertyChanging([CallerMemberName] string propertyName = "")
		{
			NotifyPropertyChanging(new PropertyChangingEventArgs(propertyName));
		}

		protected void NotifyPropertyChanging(PropertyChangingEventArgs args)
		{
			Assert.IsNotNull(args);

			OnNotifyPropertyChanging?.Invoke(this, args);
		}

		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			NotifyPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		protected void NotifyPropertyChanged(PropertyChangedEventArgs args)
		{
			Assert.IsNotNull(args);

#if SCRIPTABLE_VALUES_RUNTIME_BINDING
			OnPropertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(new BindingId(new PropertyPath(args.PropertyName))));
#endif // SCRIPTABLE_VALUES_RUNTIME_BINDING
			OnNotifyPropertyChanged?.Invoke(this, args);
		}

		protected bool SetField<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
		{
			return SetField(ref field, newValue, new PropertyChangingEventArgs(propertyName), new PropertyChangedEventArgs(propertyName));
		}

		protected bool SetField<T>(ref T field, T newValue, PropertyChangingEventArgs changingArgs, PropertyChangedEventArgs changedArgs)
		{
			Assert.IsNotNull(changingArgs);
			Assert.IsNotNull(changedArgs);

			if (EqualityHelper.Equals(field, newValue))
			{
				return false;
			}

			NotifyPropertyChanging(changingArgs);
			field = newValue;
			NotifyPropertyChanged(changedArgs);
			return true;
		}

#if SCRIPTABLE_VALUES_RUNTIME_BINDING
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

		long IDataSourceViewHashProvider.GetViewHashCode()
		{
			return GetViewHashCode();
		}
#endif // SCRIPTABLE_VALUES_RUNTIME_BINDING
	}
}