using System;
using UnityEditor;

namespace AuroraPunks.ScriptableValues
{
	[CustomPropertyDrawer(typeof(ScriptablePool<>), true)]
	public sealed class ScriptablePoolDrawer : BaseScriptableDrawer
	{
		protected override Type[] GetTypes()
		{
			return new Type[1] { fieldInfo.FieldType.GenericTypeArguments[0] };
		}
	}
}