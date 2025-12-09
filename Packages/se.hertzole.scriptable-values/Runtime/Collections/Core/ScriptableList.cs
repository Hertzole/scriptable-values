#nullable enable

using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Assertions;
#if SCRIPTABLE_VALUES_PROPERTIES
using Unity.Properties;
#endif

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     Base class for all <see cref="ScriptableList{T}" />.
    /// </summary>
    /// <remarks>You should probably inherit from <see cref="ScriptableList{T}" /> instead.</remarks>
#if UNITY_EDITOR
    [HelpURL(Documentation.SCRIPTABLE_LIST_URL)]
#endif
    public abstract class ScriptableList : RuntimeScriptableObject, ICanBeReadOnly, INotifyCollectionChanged
    {
        public static readonly PropertyChangingEventArgs clearOnStartChangingArgs = new PropertyChangingEventArgs(nameof(ClearOnStart));
        public static readonly PropertyChangedEventArgs clearOnStartChangedArgs = new PropertyChangedEventArgs(nameof(ClearOnStart));

        public static readonly PropertyChangingEventArgs isReadOnlyChangingArgs = new PropertyChangingEventArgs(nameof(IsReadOnly));
        public static readonly PropertyChangedEventArgs isReadOnlyChangedArgs = new PropertyChangedEventArgs(nameof(IsReadOnly));

        public static readonly PropertyChangingEventArgs setEqualityCheckChangingArgs = new PropertyChangingEventArgs(nameof(SetEqualityCheck));
        public static readonly PropertyChangedEventArgs setEqualityCheckChangedArgs = new PropertyChangedEventArgs(nameof(SetEqualityCheck));

        public static readonly PropertyChangingEventArgs countChangingArgs = new PropertyChangingEventArgs(nameof(Count));
        public static readonly PropertyChangedEventArgs countChangedArgs = new PropertyChangedEventArgs(nameof(Count));

        public static readonly PropertyChangingEventArgs capacityChangingArgs = new PropertyChangingEventArgs(nameof(Capacity));
        public static readonly PropertyChangedEventArgs capacityChangedArgs = new PropertyChangedEventArgs(nameof(Capacity));

        /// <summary>
        ///     If <c>true</c>, an equality check will be run before setting an item through the indexer to make sure the new
        ///     object is
        ///     not the same as the old one.
        /// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
        [CreateProperty]
#endif
        public abstract bool SetEqualityCheck { get; set; }

        /// <summary>
        ///     If <c>true</c>, the <see cref="ScriptableList{T}" /> will be cleared on play mode start/game boot.
        /// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
        [CreateProperty]
#endif
        public abstract bool ClearOnStart { get; set; }

        /// <summary>
        ///     Gets the number of elements contained in the <see cref="ScriptableList{T}" />.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="ScriptableList{T}" />.</returns>
#if SCRIPTABLE_VALUES_PROPERTIES
#if SCRIPTABLE_VALUES_PROPERTIES_SUPPORTS_READONLY
        [CreateProperty(ReadOnly = true)]
#else
        [CreateProperty]
#endif // SCRIPTABLE_VALUES_PROPERTIES_SUPPORTS_READONLY
#endif // SCRIPTABLE_VALUES_PROPERTIES
        public abstract int Count { get; protected set; }

        /// <summary>
        ///     Gets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
        [CreateProperty]
#endif
        public abstract int Capacity { get; set; }

        /// <summary>
        ///     If read only, the <see cref="ScriptableList{T}" /> cannot be changed at runtime and won't be cleared on start.
        /// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
        [CreateProperty]
#endif
        public abstract bool IsReadOnly { get; set; }

        protected event NotifyCollectionChangedEventHandler? OnInternalCollectionChanged;

        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add { OnInternalCollectionChanged += value; }
            remove { OnInternalCollectionChanged -= value; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsCollectionChangedNull()
        {
            return OnInternalCollectionChanged == null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InvokeCollectionChangedBase(NotifyCollectionChangedEventArgs args)
        {
            Assert.IsNotNull(OnInternalCollectionChanged, "Need to check for null before invoking the event using " + nameof(IsCollectionChangedNull) + ".");

            // The event should already be checked for null here.
            OnInternalCollectionChanged!.Invoke(this, args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ClearBaseSubscribers()
        {
            OnInternalCollectionChanged = null;
        }

        /// <inheritdoc />
        protected override void WarnIfLeftOverSubscribers()
        {
            base.WarnIfLeftOverSubscribers();
            EventHelper.WarnIfLeftOverSubscribers(OnInternalCollectionChanged, "INotifyCollectionChanged.CollectionChanged", this);
        }

        /// <summary>
        ///     Removes all elements from the <see cref="ScriptableList{T}" />.
        /// </summary>
        /// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
        public abstract void Clear();

        /// <summary>
        ///     Ensures that the list has at least the specified capacity.
        /// </summary>
        /// <param name="capacity">The minimum capacity to ensure.</param>
        public abstract void EnsureCapacity(int capacity);

        /// <summary>
        ///     Reverses the order of the elements in the entire <see cref="ScriptableList{T}" />.
        /// </summary>
        /// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
        public abstract void Reverse();

        /// <summary>
        ///     Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">The starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        /// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
        public abstract void Reverse(int index, int count);

        /// <summary>
        ///     Sets the capacity to the actual number of elements in the <see cref="ScriptableList{T}" />, if that number is less
        ///     than a threshold value.
        /// </summary>
        public abstract void TrimExcess();
    }
}