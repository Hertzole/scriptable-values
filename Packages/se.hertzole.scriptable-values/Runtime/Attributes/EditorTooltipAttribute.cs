using System;
using System.Diagnostics;
using UnityEngine;

namespace Hertzole.ScriptableValues
{
	[AttributeUsage(AttributeTargets.Field)]
	[Conditional("UNITY_EDITOR")]
	internal sealed class EditorTooltipAttribute : TooltipAttribute
	{
		public EditorTooltipAttribute(string tooltip) : base(tooltip) { }
	}
}