using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableBoolValueTests : ScriptableValueTest<ScriptableBool, bool>
	{
		protected override bool MakeDifferentValue(bool value)
		{
			return !value;
		}
	}
}
