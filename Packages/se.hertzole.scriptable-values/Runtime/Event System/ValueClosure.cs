#nullable enable

using System;
using System.Text;
using Hertzole.ScriptableValues.Helpers;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Assertions;

namespace Hertzole.ScriptableValues
{
	internal readonly struct ValueClosure<T> : IEquatable<ValueClosure<T>>
	{
		public readonly Action<T, T, object?> action;
		private readonly object? context;

		public ValueClosure(Delegate action, object? context)
		{
			ThrowHelper.ThrowIfNull(action, nameof(action));

			this.action = UnsafeUtility.As<Delegate, Action<T, T, object?>>(ref action);
			this.context = context;

			Assert.IsNotNull(this.action, $"Can't convert delegate {action}");
		}

		public void Invoke(T oldValue, T newValue)
		{
			action.Invoke(oldValue, newValue, context);
		}

		public bool Equals(ValueClosure<T> other)
		{
			return action.Method.Equals(other.action.Method);
		}

		public override bool Equals(object? obj)
		{
			return obj is ValueClosure<T> other && Equals(other);
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
			StringBuilder builder = new StringBuilder(128);
			builder.Append("ValueClosure<");
			builder.Append(typeof(T).Name);
			builder.Append(">(");
			builder.Append(action.Method);
			builder.Append(", ");
			builder.Append(context ?? "null context");
			builder.Append(")");
			return builder.ToString();
		}

		public static bool operator ==(ValueClosure<T> left, ValueClosure<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ValueClosure<T> left, ValueClosure<T> right)
		{
			return !left.Equals(right);
		}
	}
}