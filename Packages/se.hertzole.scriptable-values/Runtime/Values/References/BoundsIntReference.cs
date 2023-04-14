using UnityEngine;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="BoundsInt" />
	///     that allows you to reference a <see cref="ScriptableValue{BoundsInt}" /> or a constant value.
	/// </summary>
	[System.Serializable]
	public sealed class BoundsIntReference : ValueReference<BoundsInt> { }
}
