#if UNITY_2022_3_OR_NEWER // Only 2022.3 and newer versions support incremental generators.
using System;
using System.Diagnostics.CodeAnalysis;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     Mark a field or property to generate a value change callback. The target must derive from
    ///     <see cref="ScriptableValue{T}" />.
    /// </summary>
    /// <remarks>
    ///     The containing type must be marked with <see cref="GenerateScriptableCallbacksAttribute" /> in order to
    ///     generate any callbacks.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    [ExcludeFromCodeCoverage]
    public sealed class GenerateValueCallbackAttribute : Attribute
    {
        /// <summary>
        ///     Can be used to set a custom name for the generated callback method. If not set, the default naming convention will
        ///     be used.
        /// </summary>
        public string CallbackName { get; set; } = string.Empty;

        /// <summary>
        ///     Constructs a new <see cref="GenerateValueCallbackAttribute" /> with the default
        ///     <see cref="ValueCallbackType.Changed" />.
        /// </summary>
        public GenerateValueCallbackAttribute() : this(ValueCallbackType.Changed) { }

        /// <summary>
        ///     Constructs a new <see cref="GenerateValueCallbackAttribute" /> with the specified <see cref="ValueCallbackType" />.
        /// </summary>
        /// <param name="type">Sets how the callback will be generated.</param>
        public GenerateValueCallbackAttribute(ValueCallbackType type) { }
    }

    /// <summary>
    ///     Sets how a value change callback will be generated.
    /// </summary>
    public enum ValueCallbackType
    {
        /// <summary>
        ///     The callback will target <see cref="ScriptableValue{T}.OnValueChanging" />.
        /// </summary>
        Changing = 0,
        /// <summary>
        ///     The callback will target <see cref="ScriptableValue{T}.OnValueChanged" />.
        /// </summary>
        Changed = 1
    }
}
#endif