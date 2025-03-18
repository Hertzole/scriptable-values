#nullable enable

using System;
using System.Buffers;
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
		public static void WarnIfLeftOverSubscribers<T>(T action, string parameterName, Object? targetObject = null)
		{
#if DEBUG
			if (typeof(T).IsSubclassOf(typeof(Delegate)) && action != null)
			{
				Delegate del = (Delegate) (object) action;

				CreateWarning(del.GetInvocationList().AsSpan(), parameterName, targetObject);
			}
			else if (action is IDelegateList eventList && eventList.ListenersCount > 0)
			{
				CreateWarning(eventList.GetDelegates(), parameterName, targetObject);
			}
#endif
		}

#if DEBUG
		private static void CreateWarning(ReadOnlySpan<Delegate> delegates, string parameterName, Object? targetObject)
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

		private static void WriteDelegates(StringBuilder sb, in ReadOnlySpan<Delegate> delegates)
		{
			for (int i = 0; i < delegates.Length; i++)
			{
				sb.Append(delegates[i].Method.DeclaringType);
				sb.Append('.');
				sb.Append(delegates[i].Method.Name);

				WriteParameters(sb, delegates[i]);

				if (i < delegates.Length - 1)
				{
					sb.AppendLine();
				}
			}
		}

		private static void WriteParameters(StringBuilder sb, Delegate del)
		{
			ParameterInfo[] parameters = del.Method.GetParameters();
			if (parameters.Length > 0)
			{
				sb.Append('(');

				for (int i = 0; i < parameters.Length; i++)
				{
					sb.Append(parameters[i].Name);
					if (i < parameters.Length - 1)
					{
						sb.Append(", ");
					}
				}

				sb.Append(')');
			}
		}
#endif

		/// <summary>
		///     Helper method to get the listeners from a pooled list of closures as a span.
		/// </summary>
		internal static ReadOnlySpan<Delegate> GetListeners<T>(PooledList<T> list) where T : struct, IStructClosure
		{
			Delegate[] listeners = ArrayPool<Delegate>.Shared.Rent(list.Count);

			try
			{
				for (int i = 0; i < list.Count; i++)
				{
					listeners[i] = list[i].GetAction();
				}

				return new ReadOnlySpan<Delegate>(listeners, 0, list.Count);
			}
			finally
			{
				//TODO: Try to clear the array before returning it.
				// Currently causes consumers of the span to get nulls.
				ArrayPool<Delegate>.Shared.Return(listeners, false);
			}
		}
	}
}