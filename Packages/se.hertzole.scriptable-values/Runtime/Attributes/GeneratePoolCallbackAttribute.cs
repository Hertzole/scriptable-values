#if UNITY_2022_3_OR_NEWER // Only 2022.3 and newer versions support incremental generators.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     Mark a field or property to generate a pool change callback. The target must derive from
    ///     <see cref="ScriptablePool{T}" />.
    /// </summary>
    /// <remarks>
    ///     The containing type must be marked with <see cref="GenerateScriptableCallbacksAttribute" /> in order to
    ///     generate any callbacks.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [ExcludeFromCodeCoverage]
    public sealed class GeneratePoolCallbackAttribute : Attribute
    {
        /// <summary>
        ///     Can be used to set a custom name for the generated callback method. If not set, the default naming convention will
        ///     be used.
        /// </summary>
        public string CallbackName { get; set; } = string.Empty;
    }
}
#endif