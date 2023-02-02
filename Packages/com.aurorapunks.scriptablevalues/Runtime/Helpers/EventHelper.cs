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
	/// <summary>
	///     Debug only helper methods for events.
	/// </summary>
	public static class EventHelper
	{
		/// <summary>
		///     Helper method that warns if the given delegate has any subscribers left.
		///     <para>This method is removed in non-debug builds, but you don't need to explicitly remove it yourself.</para>
		/// </summary>
		/// <param name="action">The delegate to check.</param>
		/// <param name="parameterName">The name of the delegate that will be used in the message.</param>
		/// <param name="targetObject">Optional target object to ping when the log is selected.</param>
		/// <typeparam name="T">The type of the delegate.</typeparam>
		[Conditional("DEBUG")]
		public static void WarnIfLeftOverSubscribers<T>(T action, string parameterName, Object targetObject = null) where T : Delegate
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