using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableCharValueTests : ScriptableValueTest<ScriptableChar, char>
	{
		protected override char MakeDifferentValue(char value)
		{
			return (char) (value - 1);
		}
	}
}
