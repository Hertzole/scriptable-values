using System;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.ValueReferences
{
	public sealed class ValueReferenceByteTests : BaseValueReferenceTest<ScriptableByte, byte>
	{
		protected override byte MakeDifferentValue(byte value)
		{
			return (byte) (value - 1);
		}
	}
}
