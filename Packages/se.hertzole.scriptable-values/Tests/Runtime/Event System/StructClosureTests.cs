using System;
using NUnit.Framework;

namespace Hertzole.ScriptableValues.Tests
{
	//TODO: StructClosureTests
	public class StructClosureTests
	{
		// [Test]
		// public void Constructor_NullAction_ThrowsException()
		// {
		// 	Assert.Throws<ArgumentNullException>(() => new StructClosure<int, int>(null, null));
		// }
		//
		// [Test]
		// public void Invoke_WithoutContext_ActionIsInvoked()
		// {
		// 	// Arrange
		// 	bool invoked = false;
		// 	Action<int, int> action = Action;
		// 	StructClosure<int, int> closure = new StructClosure<int, int>(action, null);
		//
		// 	// Act
		// 	closure.Invoke(69, 420);
		//
		// 	// Assert
		// 	Assert.IsTrue(invoked);
		// 	return;
		//
		// 	void Action(int value1, int value2)
		// 	{
		// 		invoked = true;
		// 		Assert.AreEqual(69, value1);
		// 		Assert.AreEqual(420, value2);
		// 	}
		// }
		//
		// [Test]
		// public void Invoke_WithContext_ActionIsInvoked()
		// {
		// 	// Arrange
		// 	InvokeCountContext context = new InvokeCountContext();
		// 	Action<int, int, InvokeCountContext> action = Action;
		// 	StructClosure<int, int> closure = new StructClosure<int, int>(action, context);
		//
		// 	// Act
		// 	closure.Invoke(69, 420);
		//
		// 	// Assert
		// 	Assert.AreEqual(1, context.invokeCount);
		// 	return;
		//
		// 	void Action(int value1, int value2, InvokeCountContext ctx)
		// 	{
		// 		ctx.invokeCount++;
		// 		Assert.AreEqual(69, value1);
		// 		Assert.AreEqual(420, value2);
		// 	}
		// }
		//
		// [Test]
		// public void GetAction_ReturnsAction()
		// {
		// 	// Arrange
		// 	Action<int, int> action = Action;
		// 	StructClosure<int, int> closure = new StructClosure<int, int>(action, null);
		//
		// 	// Act
		// 	Delegate del = closure.GetAction();
		//
		// 	// Assert
		// 	Assert.AreEqual(action.Method, del.Method);
		// 	return;
		//
		// 	void Action(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void Equals_SameAction_ReturnsTrue()
		// {
		// 	// Arrange
		// 	Action<int, int> action = Action;
		// 	StructClosure<int, int> closure1 = new StructClosure<int, int>(action, null);
		// 	StructClosure<int, int> closure2 = new StructClosure<int, int>(action, null);
		//
		// 	// Act
		// 	bool equals = closure1.Equals(closure2);
		//
		// 	// Assert
		// 	Assert.IsTrue(equals);
		// 	return;
		//
		// 	void Action(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void Equals_SameAction_DifferentContext_ReturnsTrue()
		// {
		// 	// Arrange
		// 	Action<int, int> action = Action;
		// 	StructClosure<int, int> closure1 = new StructClosure<int, int>(action, new object());
		// 	StructClosure<int, int> closure2 = new StructClosure<int, int>(action, new object());
		//
		// 	// Act
		// 	bool equals = closure1.Equals(closure2);
		//
		// 	// Assert
		// 	Assert.IsTrue(equals);
		// 	return;
		//
		// 	void Action(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void Equals_DifferentAction_ReturnsFalse()
		// {
		// 	// Arrange
		// 	Action<int, int> action1 = Action1;
		// 	Action<int, int> action2 = Action2;
		// 	StructClosure<int, int> closure1 = new StructClosure<int, int>(action1, null);
		// 	StructClosure<int, int> closure2 = new StructClosure<int, int>(action2, null);
		//
		// 	// Act
		// 	bool equals = closure1.Equals(closure2);
		//
		// 	// Assert
		// 	Assert.IsFalse(equals);
		// 	return;
		//
		// 	void Action1(int value1, int value2) { }
		// 	void Action2(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void Equals_Object_ReturnsTrue()
		// {
		// 	// Arrange
		// 	Action<int, int> action = Action;
		// 	StructClosure<int, int> closure1 = new StructClosure<int, int>(action, null);
		// 	object closure2 = new StructClosure<int, int>(action, null);
		//
		// 	// Act
		// 	bool equals = closure1.Equals(closure2);
		//
		// 	// Assert
		// 	Assert.IsTrue(equals);
		// 	return;
		//
		// 	void Action(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void Equals_Object_NotStructClosure_ReturnsFalse()
		// {
		// 	// Arrange
		// 	Action<int, int> action = Action;
		// 	StructClosure<int, int> closure1 = new StructClosure<int, int>(action, null);
		// 	object closure2 = new object();
		//
		// 	// Act
		// 	bool equals = closure1.Equals(closure2);
		//
		// 	// Assert
		// 	Assert.IsFalse(equals);
		// 	return;
		//
		// 	void Action(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void EqualsOperator_SameAction_ReturnsTrue()
		// {
		// 	// Arrange
		// 	Action<int, int> action = Action;
		// 	StructClosure<int, int> closure1 = new StructClosure<int, int>(action, null);
		// 	StructClosure<int, int> closure2 = new StructClosure<int, int>(action, null);
		//
		// 	// Act
		// 	bool equals = closure1 == closure2;
		//
		// 	// Assert
		// 	Assert.IsTrue(equals);
		// 	return;
		//
		// 	void Action(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void EqualsOperator_DifferentAction_ReturnsFalse()
		// {
		// 	// Arrange
		// 	Action<int, int> action1 = Action1;
		// 	Action<int, int> action2 = Action2;
		// 	StructClosure<int, int> closure1 = new StructClosure<int, int>(action1, null);
		// 	StructClosure<int, int> closure2 = new StructClosure<int, int>(action2, null);
		//
		// 	// Act
		// 	bool equals = closure1 == closure2;
		//
		// 	// Assert
		// 	Assert.IsFalse(equals);
		// 	return;
		//
		// 	void Action1(int value1, int value2) { }
		// 	void Action2(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void NotEqualsOperator_SameAction_ReturnsFalse()
		// {
		// 	// Arrange
		// 	Action<int, int> action = Action;
		// 	StructClosure<int, int> closure1 = new StructClosure<int, int>(action, null);
		// 	StructClosure<int, int> closure2 = new StructClosure<int, int>(action, null);
		//
		// 	// Act
		// 	bool equals = closure1 != closure2;
		//
		// 	// Assert
		// 	Assert.IsFalse(equals);
		// 	return;
		//
		// 	void Action(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void NotEqualsOperator_DifferentAction_ReturnsTrue()
		// {
		// 	// Arrange
		// 	Action<int, int> action1 = Action1;
		// 	Action<int, int> action2 = Action2;
		// 	StructClosure<int, int> closure1 = new StructClosure<int, int>(action1, null);
		// 	StructClosure<int, int> closure2 = new StructClosure<int, int>(action2, null);
		//
		// 	// Act
		// 	bool equals = closure1 != closure2;
		//
		// 	// Assert
		// 	Assert.IsTrue(equals);
		// 	return;
		//
		// 	void Action1(int value1, int value2) { }
		// 	void Action2(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void GetHashCode_SameAction_ReturnsSame()
		// {
		// 	// Arrange
		// 	Action<int, int> action = Action;
		// 	StructClosure<int, int> closure1 = new StructClosure<int, int>(action, null);
		// 	StructClosure<int, int> closure2 = new StructClosure<int, int>(action, null);
		//
		// 	// Act
		// 	int hash1 = closure1.GetHashCode();
		// 	int hash2 = closure2.GetHashCode();
		//
		// 	// Assert
		// 	Assert.AreEqual(hash1, hash2);
		// 	return;
		//
		// 	void Action(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void GetHashCode_SameAction_DifferentContext_ReturnsSame()
		// {
		// 	// Arrange
		// 	Action<int, int> action = Action;
		// 	StructClosure<int, int> closure1 = new StructClosure<int, int>(action, new object());
		// 	StructClosure<int, int> closure2 = new StructClosure<int, int>(action, new object());
		//
		// 	// Act
		// 	int hash1 = closure1.GetHashCode();
		// 	int hash2 = closure2.GetHashCode();
		//
		// 	// Assert
		// 	Assert.AreEqual(hash1, hash2);
		// 	return;
		//
		// 	void Action(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void GetHashCode_DifferentAction_ReturnsDifferent()
		// {
		// 	// Arrange
		// 	Action<int, int> action1 = Action1;
		// 	Action<int, int> action2 = Action2;
		// 	StructClosure<int, int> closure1 = new StructClosure<int, int>(action1, null);
		// 	StructClosure<int, int> closure2 = new StructClosure<int, int>(action2, null);
		//
		// 	// Act
		// 	int hash1 = closure1.GetHashCode();
		// 	int hash2 = closure2.GetHashCode();
		//
		// 	// Assert
		// 	Assert.AreNotEqual(hash1, hash2);
		// 	return;
		//
		// 	void Action1(int value1, int value2) { }
		// 	void Action2(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void ToString_ReturnsCorrectString()
		// {
		// 	// Arrange
		// 	Action<int, int> action = Action;
		// 	StructClosure<int, int> closure = new StructClosure<int, int>(action, null);
		//
		// 	// Act
		// 	string str = closure.ToString();
		//
		// 	// Assert
		// 	Assert.AreEqual($"StructClosure<Int32, Int32>({action.Method}, null context)", str);
		// 	return;
		//
		// 	void Action(int value1, int value2) { }
		// }
		//
		// [Test]
		// public void ToString_WithContext_ReturnsCorrectString()
		// {
		// 	// Arrange
		// 	Action<int, int, InvokeCountContext> action = Action;
		// 	StructClosure<int, int> closure = new StructClosure<int, int>(action, new InvokeCountContext());
		//
		// 	// Act
		// 	string str = closure.ToString();
		//
		// 	// Assert
		// 	Assert.AreEqual($"StructClosure<Int32, Int32>({action.Method}, Hertzole.ScriptableValues.Tests.InvokeCountContext)", str);
		// 	return;
		//
		// 	void Action(int value1, int value2, InvokeCountContext ctx) { }
		// }
	}
}