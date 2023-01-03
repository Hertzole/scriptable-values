using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace AuroraPunks.ScriptableValues.Tests
{
	public class ScriptableEventTests : BaseTest
	{
		// Bool

		[Test]
		public void Invoke_ScriptableBool()
		{
			InvokeWithoutSender<ScriptableBoolEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableBool()
		{
			InvokeWithSender<ScriptableBoolEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableBool([ValueSource(nameof(bools))] bool value)
		{
			InvokeWithArgAndSender<ScriptableBoolEvent, bool>(value);
		}

		// Byte

		[Test]
		public void Invoke_ScriptableByte()
		{
			InvokeWithoutSender<ScriptableByteEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableByte()
		{
			InvokeWithSender<ScriptableByteEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableByte([ValueSource(nameof(bytes))] byte value)
		{
			InvokeWithArgAndSender<ScriptableByteEvent, byte>(value);
		}

		// SByte

		[Test]
		public void Invoke_ScriptableSByte()
		{
			InvokeWithoutSender<ScriptableSByteEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableSByte()
		{
			InvokeWithSender<ScriptableSByteEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableSByte([ValueSource(nameof(sbytes))] sbyte value)
		{
			InvokeWithArgAndSender<ScriptableSByteEvent, sbyte>(value);
		}

		// Short

		[Test]
		public void Invoke_ScriptableShort()
		{
			InvokeWithoutSender<ScriptableShortEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableShort()
		{
			InvokeWithSender<ScriptableShortEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableShort([ValueSource(nameof(shorts))] short value)
		{
			InvokeWithArgAndSender<ScriptableShortEvent, short>(value);
		}

		// UShort

		[Test]
		public void Invoke_ScriptableUShort()
		{
			InvokeWithoutSender<ScriptableUShortEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableUShort()
		{
			InvokeWithSender<ScriptableUShortEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableUShort([ValueSource(nameof(ushorts))] ushort value)
		{
			InvokeWithArgAndSender<ScriptableUShortEvent, ushort>(value);
		}

		// Int

		[Test]
		public void Invoke_ScriptableInt()
		{
			InvokeWithoutSender<ScriptableIntEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableInt()
		{
			InvokeWithSender<ScriptableIntEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableInt([ValueSource(nameof(ints))] int value)
		{
			InvokeWithArgAndSender<ScriptableIntEvent, int>(value);
		}

		// UInt

		[Test]
		public void Invoke_ScriptableUInt()
		{
			InvokeWithoutSender<ScriptableUIntEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableUInt()
		{
			InvokeWithSender<ScriptableUIntEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableUInt([ValueSource(nameof(uints))] uint value)
		{
			InvokeWithArgAndSender<ScriptableUIntEvent, uint>(value);
		}

		// Long

		[Test]
		public void Invoke_ScriptableLong()
		{
			InvokeWithoutSender<ScriptableLongEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableLong()
		{
			InvokeWithSender<ScriptableLongEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableLong([ValueSource(nameof(longs))] long value)
		{
			InvokeWithArgAndSender<ScriptableLongEvent, long>(value);
		}

		// ULong

		[Test]
		public void Invoke_ScriptableULong()
		{
			InvokeWithoutSender<ScriptableULongEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableULong()
		{
			InvokeWithSender<ScriptableULongEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableULong([ValueSource(nameof(ulongs))] ulong value)
		{
			InvokeWithArgAndSender<ScriptableULongEvent, ulong>(value);
		}

		// Float

		[Test]
		public void Invoke_ScriptableFloat()
		{
			InvokeWithoutSender<ScriptableFloatEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableFloat()
		{
			InvokeWithSender<ScriptableFloatEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableFloat([ValueSource(nameof(floats))] float value)
		{
			InvokeWithArgAndSender<ScriptableFloatEvent, float>(value);
		}

		// Double

		[Test]
		public void Invoke_ScriptableDouble()
		{
			InvokeWithoutSender<ScriptableDoubleEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableDouble()
		{
			InvokeWithSender<ScriptableDoubleEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableDouble([ValueSource(nameof(doubles))] double value)
		{
			InvokeWithArgAndSender<ScriptableDoubleEvent, double>(value);
		}

		// Decimal

		[Test]
		public void Invoke_ScriptableDecimal()
		{
			InvokeWithoutSender<ScriptableDecimalEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableDecimal()
		{
			InvokeWithSender<ScriptableDecimalEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableDecimal([ValueSource(nameof(decimals))] decimal value)
		{
			InvokeWithArgAndSender<ScriptableDecimalEvent, decimal>(value);
		}

		// String

		[Test]
		public void Invoke_ScriptableString()
		{
			InvokeWithoutSender<ScriptableStringEvent>();
		}

		[Test]
		public void Invoke_WithSender_ScriptableString()
		{
			InvokeWithSender<ScriptableStringEvent>();
		}

		[Test]
		public void Invoke_WithSenderAndArg_ScriptableString([ValueSource(nameof(strings))] string value)
		{
			InvokeWithArgAndSender<ScriptableStringEvent, string>(value);
		}

		private void InvokeWithoutSender<TType>() where TType : ScriptableEvent
		{
			TType instance = CreateInstance<TType>();
			instance.ResetValues();

			bool eventInvoked = false;

			instance.OnInvoked += (eventSender, eventArgs) => { eventInvoked = true; };

			instance.Invoke();

			Assert.IsTrue(eventInvoked, "OnInvoked should be invoked.");
		}

		private void InvokeWithSender<TType>() where TType : ScriptableEvent
		{
			TType instance = CreateInstance<TType>();
			instance.ResetValues();

			GameObject sender = CreateGameObject("sender");

			bool eventInvoked = false;

			instance.OnInvoked += (eventSender, eventArgs) =>
			{
				Assert.AreEqual(sender, eventSender);

				eventInvoked = true;
			};

			instance.Invoke(sender);

			Assert.IsTrue(eventInvoked, "OnInvoked should be invoked.");
		}

		private void InvokeWithArgAndSender<TType, TValue>(TValue args) where TType : ScriptableEvent<TValue>
		{
			TType instance = CreateInstance<TType>();
			instance.ResetValues();

			GameObject sender = CreateGameObject("sender");

			bool eventInvoked = false;

			instance.OnInvoked += (eventSender, eventArgs) =>
			{
				Assert.AreEqual(args, eventArgs);
				Assert.AreEqual(sender, eventSender);

				eventInvoked = true;
			};

			instance.Invoke(sender, args);

			Assert.IsTrue(eventInvoked, "OnInvoked should be invoked.");
		}
	}
}