using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Helpers
{
	internal static class EqualityHelper
	{
		public static bool Equals<T>(T left, T right)
		{
			// Do special checking on Unity objects.
			if (typeof(T).IsSubclassOf(typeof(Object)) || typeof(T) == typeof(Object))
			{
				Object leftUnityObject = left as Object;
				Object rightUnityObject = right as Object;

				return leftUnityObject == rightUnityObject;
			}

			return EqualityComparer<T>.Default.Equals(left, right);
		}

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