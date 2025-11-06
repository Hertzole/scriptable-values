using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     Specify a tooltip for a field in the inspector.
    /// </summary>
    /// <remarks>The tooltip will not be included in any builds.</remarks>
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    [ExcludeFromCodeCoverage]
    internal sealed class EditorTooltipAttribute : TooltipAttribute
    {
        /// <summary>
        ///     Create a new tooltip attribute.
        /// </summary>
        /// <param name="tooltip">The tooltip text.</param>
        public EditorTooltipAttribute(string tooltip) : base(tooltip) { }
    }
}