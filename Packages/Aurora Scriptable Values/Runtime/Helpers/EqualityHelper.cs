using System.Collections.Generic;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Helpers
{
	internal static class EqualityHelper
	{
		public static bool Equals<T>(T left, T right)
		{
			// Do special checking on Unity objects.
			if (typeof(T) == typeof(Object))
			{
				Object leftUnityObject = left as Object;
				Object rightUnityObject = right as Object;

				return leftUnityObject == rightUnityObject;
			}

			return EqualityComparer<T>.Default.Equals(left, right);
		}
	}
}