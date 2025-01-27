using System;
using System.Diagnostics;
using Object = UnityEngine.Object;
#if DEBUG
using System.Reflection;
using System.Text;
using Debug = UnityEngine.Debug;
#endif

namespace Hertzole.ScriptableValues.Helpers
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
		public static void WarnIfLeftOverSubscribers<T>(T action, string parameterName, Object targetObject = null)
		{
#if DEBUG
			if (typeof(T).IsSubclassOf(typeof(Delegate)) && action != null)
			{
				if (action is not Delegate del)
				{
					throw new ArgumentException("The given action is not a delegate.");
				}

				CreateWarning(del.GetInvocationList().AsSpan(), parameterName, targetObject);
			}
			else if (action is IEventList eventList && eventList.ListenersCount > 0)
			{
				CreateWarning(eventList.GetListeners(), parameterName, targetObject);
			}
#endif
		}

#if DEBUG
		private static void CreateWarning(ReadOnlySpan<Delegate> delegates, string parameterName, Object targetObject)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"{parameterName}");
			if (targetObject != null)
			{
				sb.Append($" in object {targetObject.name} ({targetObject.GetType().FullName})");
			}

			sb.AppendLine(" has some left over subscribers:");
			WriteDelegates(sb, delegates);

			Debug.LogWarning(sb.ToString(), targetObject);
		}

		private static void WriteDelegates(StringBuilder sb, ReadOnlySpan<Delegate> delegates)
		{
			foreach (Delegate del in delegates)
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