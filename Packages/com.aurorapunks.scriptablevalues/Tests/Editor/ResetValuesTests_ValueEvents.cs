using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public partial class ResetValuesTests
	{
		[Test]
		public void ScriptableBool_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableBool, bool>();
		}

		[Test]
		public void ScriptableByte_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableByte, byte>();
		}

		[Test]
		public void ScriptableSByte_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableSByte, sbyte>();
		}

		[Test]
		public void ScriptableShort_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableShort, short>();
		}

		[Test]
		public void ScriptableUShort_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableUShort, ushort>();
		}

		[Test]
		public void ScriptableInt_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableInt, int>();
		}

		[Test]
		public void ScriptableUInt_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableUInt, uint>();
		}

		[Test]
		public void ScriptableLong_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableLong, long>();
		}

		[Test]
		public void ScriptableULong_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableULong, ulong>();
		}

		[Test]
		public void ScriptableFloat_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableFloat, float>();
		}

		[Test]
		public void ScriptableDouble_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableDouble, double>();
		}

		[Test]
		public void ScriptableDecimal_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableDecimal, decimal>();
		}

		[Test]
		public void ScriptableString_ResetEvents()
		{
			ScriptableValueResetEvent<ScriptableString, string>();
		}
		
		private static void ScriptableValueResetEvent<TType, TValue>() where TType : ScriptableValue<TValue>
		{
			TType instance = ScriptableObject.CreateInstance<TType>();

			instance.OnValueChanging += (_, _) => { };
			instance.OnValueChanged += (_, _) => { };

			Assert.IsTrue(instance.ValueChangingHasSubscribers);
			Assert.IsTrue(instance.ValueChangedHasSubscribers);

			instance.ResetValues();

			Assert.IsFalse(instance.ValueChangingHasSubscribers);
			Assert.IsFalse(instance.ValueChangedHasSubscribers);
		}
	}
}