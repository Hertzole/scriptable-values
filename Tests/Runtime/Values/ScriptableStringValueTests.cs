using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableStringValueTests : ScriptableValueTest<ScriptableString, string>
	{
		protected override string MakeDifferentValue(string value)
		{
			return value + "1";
		}
	}
}
