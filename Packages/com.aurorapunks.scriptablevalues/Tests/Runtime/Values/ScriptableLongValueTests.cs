using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableLongValueTests : ScriptableValueTest<ScriptableLong, long>
	{
		protected override long MakeDifferentValue(long value)
		{
			return (long) (value - 1);
		}
	}
}
