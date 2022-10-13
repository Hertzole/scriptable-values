using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AuroraPunks.ScriptableValues.Helpers
{
	internal static class EventHelper
	{
		[Conditional("DEBUG")]
		internal static void WarnIfLeftOverSubscribers<T>(T action, string parameterName, ScriptableObject targetObject = null) where T : Delegate
		{
#if DEBUG
			if (action != null)
			{
				Delegate[] addedDelegates = action.GetInvocationList();

				StringBuilder sb = new StringBuilder();
				sb.Append($"{parameterName}");
				if (targetObject != null)
				{
					sb.Append($" in object {targetObject.name} ({targetObject.GetType().FullName}) ");
				}

				sb.AppendLine(" has some left over subscribers:");
				for (int i = 0; i < addedDelegates.Length; i++)
				{
					sb.Append($"{addedDelegates[i].Method.DeclaringType}.{addedDelegates[i].Method.Name}");
					ParameterInfo[] parameters = addedDelegates[i].Method.GetParameters();
					if (parameters.Length > 0)
					{
						sb.Append("(");
						for (int j = 0; j < parameters.Length; j++)
						{
							sb.Append(parameters[j].Name);
							if (j < parameters.Length - 1)
							{
								sb.Append(", ");
							}
						}

						sb.Append(")");
					}

					sb.AppendLine();
				}

				Debug.LogWarning(sb.ToString(), targetObject);
			}
#endif
		}
	}
}