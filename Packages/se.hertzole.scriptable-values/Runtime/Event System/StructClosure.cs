#nullable enable

using System;
using System.Text;
using Hertzole.ScriptableValues.Helpers;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Assertions;

namespace Hertzole.ScriptableValues
{
	internal readonly struct StructClosure<TValue1, TValue2> : IEquatable<StructClosure<TValue1, TValue2>>, IStructClosure
	{
		private readonly Action<TValue1, TValue2, object?> action;
		private readonly object? context;

		public StructClosure(Delegate action, object? context)
		{
			ThrowHelper.ThrowIfNull(action, nameof(action));

			this.action = UnsafeUtility.As<Delegate, Action<TValue1, TValue2, object?>>(ref action);
			this.context = context;

			Assert.IsNotNull(this.action, $"Can't convert delegate {action}");
		}

		public void Invoke(TValue1 oldValue, TValue2 newValue)
		{
			action.Invoke(oldValue, newValue, context);
		}

		public Delegate GetAction()
		{
			return action;
		}

		public bool Equals(StructClosure<TValue1, TValue2> other)
		{
			return action.Method.Equals(other.action.Method);
		}

		public override bool Equals(object? obj)
		{
			return obj is StructClosure<TValue1, TValue2> other && Equals(other);
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
			builder.Append("StructClosure<");
			builder.Append(typeof(TValue1).Name);
			builder.Append(", ");
			builder.Append(typeof(TValue2).Name);
			builder.Append(">(");
			builder.Append(action.Method);
			builder.Append(", ");
			builder.Append(context ?? "null context");
			builder.Append(")");
			return builder.ToString();
		}

		public static bool operator ==(StructClosure<TValue1, TValue2> left, StructClosure<TValue1, TValue2> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(StructClosure<TValue1, TValue2> left, StructClosure<TValue1, TValue2> right)
		{
			return !left.Equals(right);
		}
	}
}