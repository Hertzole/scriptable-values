using System;
using UnityEditor;

namespace AuroraPunks.ScriptableValues
{
	[CustomPropertyDrawer(typeof(ScriptableDictionary<,>))]
	public sealed class ScriptableDictionaryDrawer : BaseScriptableDrawer
	{
		protected override Type[] GetTypes()
		{
			return new Type[2] { fieldInfo.FieldType.GenericTypeArguments[0], fieldInfo.FieldType.GenericTypeArguments[1] };
		}
	}
}