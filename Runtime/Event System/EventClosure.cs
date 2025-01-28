#nullable enable

using System;
using Hertzole.ScriptableValues.Helpers;
using Unity.Collections.LowLevel.Unsafe;

namespace Hertzole.ScriptableValues
{
	internal readonly struct EventClosure<T> : IEquatable<EventClosure<T>>, IStructClosure
	{
		public readonly Action<object, T, object?> action;
		private readonly object? context;

		public EventClosure(Delegate action, object? context)
		{
			ThrowHelper.ThrowIfNull(action, nameof(action));

			this.action = UnsafeUtility.As<Delegate, Action<object, T, object?>>(ref action);
			this.context = context;
		}

		public void Invoke(in object sender, in T value)
		{
			action.Invoke(sender, value, context);
		}

		public bool Equals(EventClosure<T> other)
		{
			return action.Method.Equals(other.action.Method);
		}

		public override bool Equals(object? obj)
		{
			return obj is EventClosure<T> other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return action.Method.GetHashCode() * 397;
			}
		}

		public override string ToString()
		{
			return $"EventClosure<{typeof(T).Name}>({action.Method}, {context ?? "null context"})";
		}

		public Delegate GetAction()
		{
			return action;
		}

		public static bool operator ==(EventClosure<T> left, EventClosure<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(EventClosure<T> left, EventClosure<T> right)
		{
			return !left.Equals(right);
		}
	}
}