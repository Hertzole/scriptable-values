using System;
using System.Reflection;
using NUnit.Framework;

namespace AuroraPunks.ScriptableValues.Tests
{
	public abstract class ScriptableValueTest<TType, TValue> : BaseScriptableValueTest<TType, TValue> where TType : ScriptableValue<TValue>
	{
		private static TValue[] values;

		public static TValue[] StaticsValue
		{
			get
			{
				if (values == null)
				{
					Type baseType = typeof(BaseTest);
					FieldInfo[] fields = baseType.GetFields(BindingFlags.Static | BindingFlags.Public);

					foreach (FieldInfo field in fields)
					{
						// We don't care about non-array fields
						if (!field.FieldType.IsArray)
						{
							continue;
						}

						if (field.FieldType.GetElementType() == typeof(TValue))
						{
							values = (TValue[]) field.GetValue(null);
						}
					}
				}

				return values;
			}
		}

		[Test]
		public void SetValue([ValueSource(nameof(StaticsValue))] TValue value)
		{
			TestSetValue(value, MakeDifferentValue(value));
		}

		[Test]
		public void SetValue_WithoutNotify([ValueSource(nameof(StaticsValue))] TValue value)
		{
			TestSetValue_WithoutNotify(value, MakeDifferentValue(value));
		}

		[Test]
		public void SetValue_OnValidate([ValueSource(nameof(bools))] bool equalsCheck, [ValueSource(nameof(StaticsValue))] TValue value)
		{
			TestSetValue_OnValidate(equalsCheck, value, MakeDifferentValue(value));
		}

		[Test]
		public void SetValue_ReadOnly([ValueSource(nameof(StaticsValue))] TValue value)
		{
			TestSetValue_ReadOnly(value, MakeDifferentValue(value));
		}

		[Test]
		public void SetValue_SameValue([ValueSource(nameof(StaticsValue))] TValue value)
		{
			TestSetValue_SameValue(value, value);
		}

		[Test]
		public void SetValue_SameValue_NoEqualsCheck([ValueSource(nameof(StaticsValue))] TValue value)
		{
			TestSetValue_SameValue_NoEqualsCheck(value, value);
		}

		protected abstract TValue MakeDifferentValue(TValue value);
	}
}