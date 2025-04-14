#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using Hertzole.ScriptableValues.Helpers;

namespace Hertzole.ScriptableValues
{
	internal readonly struct StructClosure<T> : IStructClosure, IEquatable<StructClosure<T>> where T : Delegate
	{
		public readonly T action;
		public readonly object? context;

		public StructClosure(T action, object? context)
		{
			ThrowHelper.ThrowIfNull(action, nameof(action));

			this.action = action;
			this.context = context;
		}

		public Delegate GetAction()
		{
			return action;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder(256);
			sb.Append("StructClosure<");
			sb.Append(typeof(T).Name);
			sb.Append(">(");
			sb.Append(action.Method.Name);

			if (context != null)
			{
				sb.Append(", ");
				sb.Append(context);
			}

			sb.Append(")");
			return sb.ToString();
		}

		public bool Equals(StructClosure<T> other)
		{
			// Only compare the method because we don't pass the context when removing callbacks.
			return action.Method.Equals(other.action.Method);
		}

		public override bool Equals(object? obj)
		{
			return obj is StructClosure<T> other && Equals(other);
		}

		public override int GetHashCode()
		{
			return EqualityComparer<T>.Default.GetHashCode(action);
		}

		public static bool operator ==(StructClosure<T> left, StructClosure<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(StructClosure<T> left, StructClosure<T> right)
		{
			return !left.Equals(right);
		}
	}
}