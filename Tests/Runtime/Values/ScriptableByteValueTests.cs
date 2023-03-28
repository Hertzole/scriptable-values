using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableByteValueTests : ScriptableValueTest<ScriptableByte, byte>
	{
		protected override byte MakeDifferentValue(byte value)
		{
			return (byte) (value - 1);
		}
	}
}
