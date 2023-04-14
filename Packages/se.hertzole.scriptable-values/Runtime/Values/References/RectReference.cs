using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="Rect" />
	///     that allows you to reference a <see cref="ScriptableValue{Rect}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class RectReference : ValueReference<Rect> { }
}
