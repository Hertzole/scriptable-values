using System;
using System.Diagnostics;
using Object = UnityEngine.Object;
#if DEBUG
using System.Reflection;
using System.Text;
using Debug = UnityEngine.Debug;
#endif

namespace AuroraPunks.ScriptableValues.Helpers
{
	internal static class EventHelper
	{
		[Conditional("DEBUG")]
		internal static void WarnIfLeftOverSubscribers<T>(T action, string parameterName, Object targetObject = null) where T : Delegate
		{
#if DEBUG
			if (action != null)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append($"{parameterName}");
				if (targetObject != null)
				{
					sb.Append($" in object {targetObject.name} ({targetObject.GetType().FullName})");
				}

				sb.AppendLine(" has some left over subscribers:");
				WriteDelegates(sb, action);

				Debug.LogWarning(sb.ToString(), targetObject);
			}
#endif
		}

#if DEBUG
		private static void WriteDelegates<T>(StringBuilder sb, T action) where T : Delegate
		{
			Delegate[] addedDelegates = action.GetInvocationList();
			foreach (Delegate del in addedDelegates)
			{
				sb.Append($"{del.Method.DeclaringType}.{del.Method.Name}");
				WriteParameters(sb, del);

				sb.AppendLine();
			}
		}

		private static void WriteParameters(StringBuilder sb, Delegate del)
		{
			ParameterInfo[] parameters = del.Method.GetParameters();
			if (parameters.Length > 0)
			{
				sb.Append("(");

				for (int i = 0; i < parameters.Length; i++)
				{
					sb.Append(parameters[i].Name);
					if (i < parameters.Length - 1)
					{
						sb.Append(", ");
					}
				}

				sb.Append(")");
			}
		}
#endif
	}
}