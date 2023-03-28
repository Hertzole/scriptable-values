using System;
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.ScriptableValues.Tests.Values
{
	public class ScriptableCharValueTests : ScriptableValueTest<ScriptableChar, Char>
	{
		protected override Char MakeDifferentValue(Char value)
		{
			return (Char) (value - 1);
		}
	}
}
