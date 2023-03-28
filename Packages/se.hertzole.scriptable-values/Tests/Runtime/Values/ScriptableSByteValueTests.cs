using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableSByteValueTests : ScriptableValueTest<ScriptableSByte, sbyte>
	{
		protected override sbyte MakeDifferentValue(sbyte value)
		{
			return (sbyte) (value - 1);
		}
	}
}
