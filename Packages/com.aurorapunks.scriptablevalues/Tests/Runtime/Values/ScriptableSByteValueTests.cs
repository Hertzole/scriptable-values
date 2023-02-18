using System;
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace AuroraPunks.ScriptableValues.Tests.Values
{
	public class ScriptableSByteValueTests : ScriptableValueTest<ScriptableSByte, sbyte>
	{
		protected override sbyte MakeDifferentValue(sbyte value)
		{
			return (sbyte) (value - 1);
		}
	}
}
