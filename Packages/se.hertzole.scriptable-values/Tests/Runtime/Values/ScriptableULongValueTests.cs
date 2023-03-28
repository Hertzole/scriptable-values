using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableULongValueTests : ScriptableValueTest<ScriptableULong, ulong>
	{
		protected override ulong MakeDifferentValue(ulong value)
		{
			return (ulong) (value - 1);
		}
	}
}
