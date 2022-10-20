using System.Diagnostics;
using AuroraPunks.ScriptableValues.Debugging;
using NUnit.Framework;

namespace AuroraPunks.ScriptableValues.Tests.Editor
{
	public class StackTraceEntryTests
	{
#pragma warning disable CS1718 // Comparison made to same variable
		[Test]
		public void Equals_WhenSameObject_ReturnsTrue()
		{
			StackTraceEntry entry = new StackTraceEntry(new StackTrace());
			Assert.IsTrue(entry.Equals(entry));
		}

		[Test]
		public void Equals_WhenDifferentObject_ReturnsFalse()
		{
			StackTraceEntry entry = new StackTraceEntry(new StackTrace());
			Assert.IsFalse(entry.Equals(new StackTraceEntry(new StackTrace())));
		}

		[Test]
		public void EqualsObject_WhenSameObject_ReturnsTrue()
		{
			StackTraceEntry entry = new StackTraceEntry(new StackTrace());
			Assert.IsTrue(entry.Equals((object) entry));
		}

		[Test]
		public void EqualsObject_WhenDifferentObject_ReturnsFalse()
		{
			StackTraceEntry entry = new StackTraceEntry(new StackTrace());
			Assert.IsFalse(entry.Equals((object) new StackTraceEntry(new StackTrace())));
		}

		[Test]
		public void GetHashCode_WhenSameObject_ReturnsSameValue()
		{
			StackTraceEntry entry = new StackTraceEntry(new StackTrace());
			Assert.AreEqual(entry.GetHashCode(), entry.GetHashCode());
		}

		[Test]
		public void GetHashCode_WhenDifferentObject_ReturnsDifferentValue()
		{
			StackTraceEntry entry = new StackTraceEntry(new StackTrace());
			Assert.AreNotEqual(entry.GetHashCode(), new StackTraceEntry(new StackTrace()).GetHashCode());
		}

		[Test]
		public void EqualsOperator_WhenSameObject_ReturnsTrue()
		{
			StackTraceEntry entry = new StackTraceEntry(new StackTrace());
			Assert.IsTrue(entry == entry);
		}

		[Test]
		public void EqualsOperator_WhenDifferentObject_ReturnsFalse()
		{
			StackTraceEntry entry = new StackTraceEntry(new StackTrace());
			Assert.IsFalse(entry == new StackTraceEntry(new StackTrace()));
		}

		[Test]
		public void NotEqualsOperator_WhenSameObject_ReturnsFalse()
		{
			StackTraceEntry entry = new StackTraceEntry(new StackTrace());
			Assert.IsFalse(entry != entry);
		}

		[Test]
		public void NotEqualsOperator_WhenDifferentObject_ReturnsTrue()
		{
			StackTraceEntry entry = new StackTraceEntry(new StackTrace());
			Assert.IsTrue(entry != new StackTraceEntry(new StackTrace()));
		}
#pragma warning restore CS1718 // Comparison made to same variable
	}
}