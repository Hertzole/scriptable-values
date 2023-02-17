using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public partial class ResetValuesTests
	{
		[Test]
		public void ScriptableBool_ResetValue()
		{
			ScriptableValueReset<ScriptableBool, bool>(false, true);
		}

		[Test]
		public void ScriptableByte_ResetValue()
		{
			ScriptableValueReset<ScriptableByte, byte>(0, 1);
		}

		[Test]
		public void ScriptableSByte_ResetValue()
		{
			ScriptableValueReset<ScriptableSByte, sbyte>(0, 1);
		}

		[Test]
		public void ScriptableShort_ResetValue()
		{
			ScriptableValueReset<ScriptableShort, short>(0, 1);
		}

		[Test]
		public void ScriptableUShort_ResetValue()
		{
			ScriptableValueReset<ScriptableUShort, ushort>(0, 1);
		}

		[Test]
		public void ScriptableInt_ResetValue()
		{
			ScriptableValueReset<ScriptableInt, int>(0, 1);
		}

		[Test]
		public void ScriptableUInt_ResetValue()
		{
			ScriptableValueReset<ScriptableUInt, uint>(0, 1);
		}

		[Test]
		public void ScriptableLong_ResetValue()
		{
			ScriptableValueReset<ScriptableLong, long>(0, 1);
		}

		[Test]
		public void ScriptableULong_ResetValue()
		{
			ScriptableValueReset<ScriptableULong, ulong>(0, 1);
		}

		[Test]
		public void ScriptableFloat_ResetValue()
		{
			ScriptableValueReset<ScriptableFloat, float>(-11.1f, 12.2f);
		}

		[Test]
		public void ScriptableDouble_ResetValue()
		{
			ScriptableValueReset<ScriptableDouble, double>(-11.1, 12.2);
		}

		[Test]
		public void ScriptableDecimal_ResetValue()
		{
			ScriptableValueReset<ScriptableDecimal, decimal>(-11.1m, 12.2m);
		}

		[Test]
		public void ScriptableString_ResetValue()
		{
			ScriptableValueReset<ScriptableString, string>("First Value", "Second Value");
		}

		private void ScriptableValueReset<TType, TValue>(TValue initialValue, TValue newValue) where TType : ScriptableValue<TValue>
		{
			TType instance = CreateInstance<TType>();

			instance.Value = initialValue;
			instance.DefaultValue = initialValue;

			Assert.AreEqual(initialValue, instance.Value);

			instance.Value = newValue;

			Assert.AreEqual(newValue, instance.Value);

			instance.Test_OnStart();

			Assert.AreEqual(initialValue, instance.Value);
		}
	}
}