using System;
using UnityEditor;

namespace AuroraPunks.ScriptableValues
{
	[CustomPropertyDrawer(typeof(ScriptableList<>))]
	public sealed class ScriptableListDrawer : BaseScriptableDrawer
	{
		protected override Type[] GetTypes()
		{
			return new Type[1] { fieldInfo.FieldType.GenericTypeArguments[0] };
		}
	}
}