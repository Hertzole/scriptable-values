using System;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     A <see cref="ValueReference{TValue}" /> with a type of <see cref="long" />
    ///     that allows you to reference a <see cref="ScriptableValue{Int64}" /> or a constant value.
    /// </summary>
    [Serializable]
    public sealed class LongReference : ValueReference<long> { }
}