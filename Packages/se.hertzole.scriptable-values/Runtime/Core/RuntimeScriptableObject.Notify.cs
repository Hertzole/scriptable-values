#nullable enable

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine.Assertions;
#if SCRIPTABLE_VALUES_RUNTIME_BINDING
using Unity.Properties;
using UnityEngine.UIElements;
#endif // SCRIPTABLE_VALUES_RUNTIME_BINDING

namespace Hertzole.ScriptableValues
{
	partial class RuntimeScriptableObject :
#if SCRIPTABLE_VALUES_RUNTIME_BINDING
		IDataSourceViewHashProvider,
		INotifyBindablePropertyChanged,
#endif // SCRIPTABLE_VALUES_RUNTIME_BINDING
		INotifyPropertyChanging,
		INotifyPropertyChanged
	{
		private event PropertyChangingEventHandler? OnNotifyPropertyChanging;
		private event PropertyChangedEventHandler? OnNotifyPropertyChanged;

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
			// Check first if the event is null to avoid creating a new PropertyChangingEventArgs
			if (OnNotifyPropertyChanging != null)
			{
				NotifyPropertyChanging(new PropertyChangingEventArgs(propertyName));
			}
		}

		protected void NotifyPropertyChanging(PropertyChangingEventArgs args)
		{
			Assert.IsNotNull(args);

			OnNotifyPropertyChanging?.Invoke(this, args);
		}

		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			// Check first if the event is null to avoid creating a new PropertyChangedEventArgs
			if (OnNotifyPropertyChanged != null)
			{
				NotifyPropertyChanged(new PropertyChangedEventArgs(propertyName));
			}
			else
			{
				// If the event is null, we can still notify Unity about the property change
				NotifyUnityPropertyChanged(propertyName);
			}
		}

		protected void NotifyPropertyChanged(PropertyChangedEventArgs args)
		{
			Assert.IsNotNull(args);

			NotifyUnityPropertyChanged(args.PropertyName);
			OnNotifyPropertyChanged?.Invoke(this, args);
		}

		[Conditional("SCRIPTABLE_VALUES_RUNTIME_BINDING")]
		private void NotifyUnityPropertyChanged(string propertyName)
		{
#if SCRIPTABLE_VALUES_RUNTIME_BINDING
			OnPropertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(new BindingId(new PropertyPath(propertyName))));
#endif // SCRIPTABLE_VALUES_RUNTIME_BINDING
		}

		protected bool SetField<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
		{
			PropertyChangingEventArgs? changingEventArgs = null;
			PropertyChangedEventArgs? changedEventArgs = null;

			// Only create the event args if the event is not null
			if (OnNotifyPropertyChanging != null)
			{
				changingEventArgs = new PropertyChangingEventArgs(propertyName);
			}

			if (OnNotifyPropertyChanged != null)
			{
				changedEventArgs = new PropertyChangedEventArgs(propertyName);
			}

			return SetFieldInternal(ref field, newValue, changingEventArgs, changedEventArgs, propertyName);
		}

		protected bool SetField<T>(ref T field, T newValue, PropertyChangingEventArgs changingArgs, PropertyChangedEventArgs changedArgs)
		{
			return SetFieldInternal(ref field, newValue, changingArgs, changedArgs, changedArgs.PropertyName);
		}

		private bool SetFieldInternal<T>(ref T field,
			T newValue,
			PropertyChangingEventArgs? changingArgs,
			PropertyChangedEventArgs? changedArgs,
			string propertyName)
		{
			Assert.IsNotNull(changingArgs);
			Assert.IsNotNull(changedArgs);

			if (EqualityHelper.Equals(field, newValue))
			{
				return false;
			}

			if (changingArgs != null)
			{
				NotifyPropertyChanging(changingArgs);
			}

			field = newValue;

			if (changedArgs != null)
			{
				NotifyPropertyChanged(changedArgs);
			}
			else
			{
				NotifyUnityPropertyChanged(propertyName);
			}

			return true;
		}

		/// <summary>
		///     Warns if there are any left-over subscribers to the events.
		/// </summary>
		/// <remarks>This will only be called in the Unity editor and builds with the DEBUG flag.</remarks>
		[Conditional("DEBUG")]
		protected virtual void WarnIfLeftOverSubscribers()
		{
			EventHelper.WarnIfLeftOverSubscribers(OnNotifyPropertyChanging, "INotifyPropertyChanging.PropertyChanging", this);
			EventHelper.WarnIfLeftOverSubscribers(OnNotifyPropertyChanged, "INotifyPropertyChanged.PropertyChanged", this);
#if SCRIPTABLE_VALUES_RUNTIME_BINDING
			EventHelper.WarnIfLeftOverSubscribers(OnPropertyChanged, "INotifyBindablePropertyChanged.propertyChanged", this);
#endif // SCRIPTABLE_VALUES_RUNTIME_BINDING
		}

#if SCRIPTABLE_VALUES_RUNTIME_BINDING
		private event EventHandler<BindablePropertyChangedEventArgs>? OnPropertyChanged;

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