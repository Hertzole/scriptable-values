using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableCharValueTests : ScriptableValueTest<ScriptableChar, Char>
	{
		protected override Char MakeDifferentValue(Char value)
		{
			return (Char) (value - 1);
		}
	}
}
