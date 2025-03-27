using Microsoft.CodeAnalysis;

namespace Hertzole.ScriptableValues.Generator;

internal static class DiagnosticRules
{
	private static readonly LocalizableResourceString noCallbackImplementationTitle = new LocalizableResourceString(
		nameof(Resources.HSV0001Title), Resources.ResourceManager, typeof(Resources));
	private static readonly LocalizableResourceString noCallbackImplementationMessage = new LocalizableResourceString(
		nameof(Resources.HSV0001MessageFormat), Resources.ResourceManager, typeof(Resources));

	public static readonly DiagnosticDescriptor NoCallbackImplementation = new DiagnosticDescriptor(
		DiagnosticIdentifiers.NoCallbackImplementation,
		noCallbackImplementationTitle,
		noCallbackImplementationMessage,
		DiagnosticCategories.Usage,
		DiagnosticSeverity.Error,
		true);
}