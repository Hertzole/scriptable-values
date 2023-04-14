using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="Color" />
	///     that allows you to reference a <see cref="ScriptableValue{Color}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class ColorReference : ValueReference<Color> { }
}
