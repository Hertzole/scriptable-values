#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AuroraPunks.ScriptableValues.Helpers
{
	public readonly struct StackTraceEntry : IEquatable<StackTraceEntry>
	{
		// Use bytes to use as little memory as possible.
		// It's way more efficient than using DateTime.
		// We need to be efficient because these stack traces can easily stack up to 1000s of entries.
		public readonly byte hour;
		public readonly byte minute;
		public readonly byte second;

		public readonly StackTrace trace;

		public StackTraceEntry(StackTrace trace)
		{
			this.trace = trace;
			DateTime time = DateTime.Now;
			hour = (byte) time.Hour;
			minute = (byte) time.Minute;
			second = (byte) time.Second;
		}

		public bool Equals(StackTraceEntry other)
		{
			return hour == other.hour && minute == other.minute && second == other.second && EqualityComparer<StackTrace>.Default.Equals(trace, other.trace);
		}

		public override bool Equals(object obj)
		{
			return obj is StackTraceEntry other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = hour.GetHashCode();
				hashCode = (hashCode * 397) ^ minute.GetHashCode();
				hashCode = (hashCode * 397) ^ second.GetHashCode();
				hashCode = (hashCode * 397) ^ (trace != null ? trace.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(StackTraceEntry left, StackTraceEntry right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(StackTraceEntry left, StackTraceEntry right)
		{
			return !left.Equals(right);
		}
	}
}
#endif