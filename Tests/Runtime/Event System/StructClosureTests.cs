using System;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	public class StructClosureTests : BaseRuntimeTest
	{
		[Test]
		public void Constructor_Validations()
		{
			AssertThrows<ArgumentNullException>(() => new StructClosure<Action>(null!, null));
		}

		[Test]
		public void GetAction_ReturnsAction()
		{
			// Arrange
			Action<int, int> action = Action;
			StructClosure<Action<int, int>> closure = new StructClosure<Action<int, int>>(action, null);

			// Act
			Delegate del = closure.GetAction();

			// Assert
			Assert.AreEqual(action.Method, del.Method);
			return;

			void Action(int value1, int value2) { }
		}

		[Test]
		public void Equals_SameAction_ReturnsTrue()
		{
			// Arrange
			Action<int, int> action = Action;
			StructClosure<Action<int, int>> closure1 = new StructClosure<Action<int, int>>(action, null);
			StructClosure<Action<int, int>> closure2 = new StructClosure<Action<int, int>>(action, null);

			// Act
			bool equals = closure1.Equals(closure2);

			// Assert
			Assert.IsTrue(equals);
			return;

			void Action(int value1, int value2) { }
		}

		[Test]
		public void Equals_SameAction_DifferentContext_ReturnsTrue()
		{
			// Arrange
			Action<int, int> action = Action;
			StructClosure<Action<int, int>> closure1 = new StructClosure<Action<int, int>>(action, new object());
			StructClosure<Action<int, int>> closure2 = new StructClosure<Action<int, int>>(action, new object());

			// Act
			bool equals = closure1.Equals(closure2);

			// Assert
			Assert.IsTrue(equals);
			return;

			void Action(int value1, int value2) { }
		}

		[Test]
		public void Equals_DifferentAction_ReturnsFalse()
		{
			// Arrange
			Action<int, int> action1 = Action1;
			Action<int, int> action2 = Action2;
			StructClosure<Action<int, int>> closure1 = new StructClosure<Action<int, int>>(action1, null);
			StructClosure<Action<int, int>> closure2 = new StructClosure<Action<int, int>>(action2, null);

			// Act
			bool equals = closure1.Equals(closure2);

			// Assert
			Assert.IsFalse(equals);
			return;

			void Action1(int value1, int value2) { }
			void Action2(int value1, int value2) { }
		}

		[Test]
		public void Equals_Object_ReturnsTrue()
		{
			// Arrange
			Action<int, int> action = Action;
			StructClosure<Action<int, int>> closure1 = new StructClosure<Action<int, int>>(action, null);
			object closure2 = new StructClosure<Action<int, int>>(action, null);

			// Act
			bool equals = closure1.Equals(closure2);

			// Assert
			Assert.IsTrue(equals);
			return;

			void Action(int value1, int value2) { }
		}

		[Test]
		public void Equals_Object_NotStructClosure_ReturnsFalse()
		{
			// Arrange
			Action<int, int> action = Action;
			StructClosure<Action<int, int>> closure1 = new StructClosure<Action<int, int>>(action, null);
			object closure2 = new object();

			// Act
			bool equals = closure1.Equals(closure2);

			// Assert
			Assert.IsFalse(equals);
			return;

			void Action(int value1, int value2) { }
		}

		[Test]
		public void EqualsOperator_SameAction_ReturnsTrue()
		{
			// Arrange
			Action<int, int> action = Action;
			StructClosure<Action<int, int>> closure1 = new StructClosure<Action<int, int>>(action, null);
			StructClosure<Action<int, int>> closure2 = new StructClosure<Action<int, int>>(action, null);

			// Act
			bool equals = closure1 == closure2;

			// Assert
			Assert.IsTrue(equals);
			return;

			void Action(int value1, int value2) { }
		}

		[Test]
		public void EqualsOperator_DifferentAction_ReturnsFalse()
		{
			// Arrange
			Action<int, int> action1 = Action1;
			Action<int, int> action2 = Action2;
			StructClosure<Action<int, int>> closure1 = new StructClosure<Action<int, int>>(action1, null);
			StructClosure<Action<int, int>> closure2 = new StructClosure<Action<int, int>>(action2, null);

			// Act
			bool equals = closure1 == closure2;

			// Assert
			Assert.IsFalse(equals);
			return;

			void Action1(int value1, int value2) { }
			void Action2(int value1, int value2) { }
		}

		[Test]
		public void NotEqualsOperator_SameAction_ReturnsFalse()
		{
			// Arrange
			Action<int, int> action = Action;
			StructClosure<Action<int, int>> closure1 = new StructClosure<Action<int, int>>(action, null);
			StructClosure<Action<int, int>> closure2 = new StructClosure<Action<int, int>>(action, null);

			// Act
			bool equals = closure1 != closure2;

			// Assert
			Assert.IsFalse(equals);
			return;

			void Action(int value1, int value2) { }
		}

		[Test]
		public void NotEqualsOperator_DifferentAction_ReturnsTrue()
		{
			// Arrange
			Action<int, int> action1 = Action1;
			Action<int, int> action2 = Action2;
			StructClosure<Action<int, int>> closure1 = new StructClosure<Action<int, int>>(action1, null);
			StructClosure<Action<int, int>> closure2 = new StructClosure<Action<int, int>>(action2, null);

			// Act
			bool equals = closure1 != closure2;

			// Assert
			Assert.IsTrue(equals);
			return;

			void Action1(int value1, int value2) { }
			void Action2(int value1, int value2) { }
		}

		[Test]
		public void GetHashCode_SameAction_ReturnsSame()
		{
			// Arrange
			Action<int, int> action = Action;
			StructClosure<Action<int, int>> closure1 = new StructClosure<Action<int, int>>(action, null);
			StructClosure<Action<int, int>> closure2 = new StructClosure<Action<int, int>>(action, null);

			// Act
			int hash1 = closure1.GetHashCode();
			int hash2 = closure2.GetHashCode();

			// Assert
			Assert.AreEqual(hash1, hash2);
			return;

			void Action(int value1, int value2) { }
		}

		[Test]
		public void GetHashCode_SameAction_DifferentContext_ReturnsSame()
		{
			// Arrange
			Action<int, int> action = Action;
			StructClosure<Action<int, int>> closure1 = new StructClosure<Action<int, int>>(action, new object());
			StructClosure<Action<int, int>> closure2 = new StructClosure<Action<int, int>>(action, new object());

			// Act
			int hash1 = closure1.GetHashCode();
			int hash2 = closure2.GetHashCode();

			// Assert
			Assert.AreEqual(hash1, hash2);
			return;

			void Action(int value1, int value2) { }
		}

		[Test]
		public void GetHashCode_DifferentAction_ReturnsDifferent()
		{
			// Arrange
			Action<int, int> action1 = Action1;
			Action<int, int> action2 = Action2;
			StructClosure<Action<int, int>> closure1 = new StructClosure<Action<int, int>>(action1, null);
			StructClosure<Action<int, int>> closure2 = new StructClosure<Action<int, int>>(action2, null);

			// Act
			int hash1 = closure1.GetHashCode();
			int hash2 = closure2.GetHashCode();

			// Assert
			Assert.AreNotEqual(hash1, hash2);
			return;

			void Action1(int value1, int value2) { }
			void Action2(int value1, int value2) { }
		}

		[Test]
		public void ToString_ReturnsCorrectString()
		{
			// Arrange
			Action<int, int> action = Action;
			StructClosure<Action<int, int>> closure = new StructClosure<Action<int, int>>(action, null);

			// Act
			string str = closure.ToString();

			// Assert
			Assert.AreEqual($"StructClosure<Action`2>({action.Method.Name})", str);
			return;

			void Action(int value1, int value2) { }
		}

		[Test]
		public void ToString_WithContext_ReturnsCorrectString()
		{
			// Arrange
			Action<int, int, InvokeCountContext> action = Action;
			StructClosure<Action<int, int, InvokeCountContext>> closure =
				new StructClosure<Action<int, int, InvokeCountContext>>(action, new InvokeCountContext());

			// Act
			string str = closure.ToString();

			// Assert
			Assert.AreEqual($"StructClosure<Action`3>({action.Method.Name}, Hertzole.ScriptableValues.Tests.InvokeCountContext)", str);
			return;

			void Action(int value1, int value2, InvokeCountContext ctx) { }
		}
	}
}