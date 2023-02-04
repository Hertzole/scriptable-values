using System;
using UnityEditor;

namespace AuroraPunks.ScriptableValues
{
	[CustomPropertyDrawer(typeof(ScriptableEvent<>), true)]
	public sealed class ScriptableEventDrawer : BaseScriptableDrawer
	{
		protected override Type[] GetTypes()
		{
			return new Type[1] { fieldInfo.FieldType.GenericTypeArguments[0] };
		}
	}
}