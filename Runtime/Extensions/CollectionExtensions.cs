using System.Collections.Generic;
using Unity.Collections;

namespace Hertzole.ScriptableValues
{
    public static class CollectionExtensions
    {
        internal static bool TryEnsureCapacity<T>(this IList<T> list, int capacity)
        {
            if (list is List<T> genericList)
            {
                if (genericList.Capacity < capacity)
                {
                    genericList.Capacity = capacity;
                }

                return true;
            }

            if (list is ScriptableList<T> scriptableList)
            {
                scriptableList.EnsureCapacity(capacity);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Returns a <see cref="NativeArray{T}" /> that is a copy of this list.
        /// </summary>
        /// <remarks>This method supports both <c>struct</c> and <c>unmanaged</c> types.</remarks>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to copy.</param>
        /// <param name="allocator">The allocator to use.</param>
        /// <returns>An array that is a copy of this list.</returns>
        public static NativeArray<T> ToNativeArray<T>(this ScriptableList<T> list, Allocator allocator) where T : struct
        {
            NativeArray<T> nativeArray = new NativeArray<T>(list.Count, allocator, NativeArrayOptions.UninitializedMemory);
            for (int i = 0; i < list.Count; i++)
            {
                nativeArray[i] = list[i];
            }

            return nativeArray;
        }

#if SCRIPTABLE_VALUES_UNITY_COLLECTIONS
        /// <summary>
        ///     Returns a <see cref="NativeArray{T}" /> that is a copy of this list.
        /// </summary>
        /// <remarks>This method only supports <c>unmanaged</c> types.</remarks>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to copy.</param>
        /// <param name="allocator">The allocator to use.</param>
        /// <returns>An array that is a copy of this list.</returns>
        public static NativeArray<T> ToNativeArray<T>(this ScriptableList<T> list, AllocatorManager.AllocatorHandle allocator) where T : unmanaged
        {
            return list.list.ToNativeArray(allocator);
        }

        /// <summary>
        ///     Returns a <see cref="NativeList{T}" /> that is a copy of this list.
        /// </summary>
        /// <remarks>This method only supports <c>unmanaged</c> types.</remarks>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to copy.</param>
        /// <param name="allocator">The allocator to use.</param>
        /// <returns>A <see cref="NativeList{T}" /> that is a copy of this list.</returns>
        public static NativeList<T> ToNativeList<T>(this ScriptableList<T> list, AllocatorManager.AllocatorHandle allocator) where T : unmanaged
        {
            return list.list.ToNativeList(allocator);
        }
#endif
    }
}