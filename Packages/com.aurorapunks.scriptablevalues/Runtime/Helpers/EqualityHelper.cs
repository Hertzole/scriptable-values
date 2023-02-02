using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Helpers
{
	/// <summary>
	///     Helper methods for ScriptableValues.
	/// </summary>
	public static class EqualityHelper
	{
		/// <summary>
		///     Helper method for checking if two types are the same.
		///     <para>This takes Unity objects into consideration.</para>
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <typeparam name="T">The type of the object.</typeparam>
		/// <returns>True if the specified objects are equal; otherwise, false.</returns>
		public static bool Equals<T>(T x, T y)
		{
			// Do special checking on Unity objects.
			if (typeof(T).IsSubclassOf(typeof(Object)) || typeof(T) == typeof(Object))
			{
				Object xUnityObject = x as Object;
				Object yUnityObject = y as Object;

				return xUnityObject == yUnityObject;
			}

			return EqualityComparer<T>.Default.Equals(x, y);
		}

		/// <summary>
		///     Helper method for checking if an object is null.
		///     <para>This takes Unity objects into consideration.</para>
		/// </summary>
		/// <param name="obj">The object to check.</param>
		/// <typeparam name="T">The type of the object.</typeparam>
		/// <returns>True if the object is null; otherwise false.</returns>
		public static bool IsNull<T>(T obj)
		{
			// Do special checking on Unity objects.
			if (typeof(T).IsSubclassOf(typeof(Object)) || typeof(T) == typeof(Object))
			{
				Object unityObject = obj as Object;
				return unityObject == null;
			}

			return obj == null;
		}

		/// <summary>
		///     Helper method to check if the given object is the same type as the provided generic type.
		/// </summary>
		/// <param name="value">The object value to check.</param>
		/// <param name="newValue">The value as the generic value.</param>
		/// <typeparam name="TType">The type to match.</typeparam>
		/// <returns>True if the value is the same type; otherwise false.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSameType<TType>(object value, out TType newValue)
		{
			if (value is TType newValueT)
			{
				newValue = newValueT;
				return true;
			}

#if DEBUG
			Debug.LogError($"{typeof(TType)} is not assignable from {value.GetType()}.");
#endif
			newValue = default;
			return false;
		}
	}
}