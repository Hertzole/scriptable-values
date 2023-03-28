﻿using System;
using UnityEditor;

namespace Hertzole.ScriptableValues
{
	[CustomPropertyDrawer(typeof(ScriptableValue<>), true)]
	public sealed class ScriptableValueDrawer : BaseScriptableDrawer
	{
		protected override Type[] GetTypes()
		{
			return new Type[1] { fieldInfo.FieldType.GenericTypeArguments[0] };
		}
	}
}