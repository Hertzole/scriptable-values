using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableShortValueTests : ScriptableValueTest<ScriptableShort, short>
	{
		protected override short MakeDifferentValue(short value)
		{
			return (short) (value - 1);
		}
	}
}
