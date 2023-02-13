using System;
using System.Reflection;

namespace AuroraPunks.ScriptableValues.Tests
{
	internal static class TestHelper
	{
		public static T[] FindValues<T>(Type type, ref T[] existingValues)
		{
			if (existingValues == null)
			{
				T[] values = null;
				
				Type baseType = type;
				FieldInfo[] fields = baseType.GetFields(BindingFlags.Static | BindingFlags.Public);

				foreach (FieldInfo field in fields)
				{
					// We don't care about non-array fields
					if (!field.FieldType.IsArray)
					{
						continue;
					}

					if (field.FieldType.GetElementType() == typeof(T))
					{
						values = (T[]) field.GetValue(null);
					}
				}
				
				existingValues = values;
			}

			return existingValues;
		}
	}
}