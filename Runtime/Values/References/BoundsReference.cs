using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="Bounds" />
	///     that allows you to reference a <see cref="ScriptableValue{Bounds}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class BoundsReference : ValueReference<Bounds> { }
}
