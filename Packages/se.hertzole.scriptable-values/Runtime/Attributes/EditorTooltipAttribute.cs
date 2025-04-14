using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Hertzole.ScriptableValues
{
	[AttributeUsage(AttributeTargets.Field)]
	[Conditional("UNITY_EDITOR")]
	[ExcludeFromCodeCoverage]
	internal sealed class EditorTooltipAttribute : TooltipAttribute
	{
		public EditorTooltipAttribute(string tooltip) : base(tooltip) { }
	}
}