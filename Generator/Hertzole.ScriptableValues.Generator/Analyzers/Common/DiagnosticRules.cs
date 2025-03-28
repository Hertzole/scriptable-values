using Microsoft.CodeAnalysis;

namespace Hertzole.ScriptableValues.Generator;

internal static class DiagnosticRules
{
	private static readonly LocalizableResourceString noCallbackImplementationTitle = new LocalizableResourceString(
		nameof(Resources.HSV0001Title), Resources.ResourceManager, typeof(Resources));
	private static readonly LocalizableResourceString noCallbackImplementationMessage = new LocalizableResourceString(
		nameof(Resources.HSV0001MessageFormat), Resources.ResourceManager, typeof(Resources));

	private static readonly LocalizableResourceString noMarkerAttributeTiler = new LocalizableResourceString(
		nameof(Resources.HSV0002Title), Resources.ResourceManager, typeof(Resources));
	private static readonly LocalizableResourceString noMarkerAttributeMessage = new LocalizableResourceString(
		nameof(Resources.HSV0002Message), Resources.ResourceManager, typeof(Resources));

	private static readonly LocalizableResourceString typeNotSupportedForCallbackAttributeTitle = new LocalizableResourceString(
		nameof(Resources.HSV0003Title), Resources.ResourceManager, typeof(Resources));
	private static readonly LocalizableResourceString typeNotSupportedForCallbackAttributeMessage = new LocalizableResourceString(
		nameof(Resources.HSV0003MessageFormat), Resources.ResourceManager, typeof(Resources));

	public static readonly DiagnosticDescriptor NoCallbackImplementation = new DiagnosticDescriptor(
		DiagnosticIdentifiers.NoCallbackImplementation,
		noCallbackImplementationTitle,
		noCallbackImplementationMessage,
		DiagnosticCategories.Usage,
		DiagnosticSeverity.Error,
		true);

	public static readonly DiagnosticDescriptor NoMarkerAttribute = new DiagnosticDescriptor(
		DiagnosticIdentifiers.NoMarkerAttribute,
		noMarkerAttributeTiler,
		noMarkerAttributeMessage,
		DiagnosticCategories.Usage,
		DiagnosticSeverity.Warning,
		true);

	public static readonly DiagnosticDescriptor TypeNotSupportedForCallbackAttribute = new DiagnosticDescriptor(
		DiagnosticIdentifiers.TypeNotSupportedForCallbackAttribute,
		typeNotSupportedForCallbackAttributeTitle,
		typeNotSupportedForCallbackAttributeMessage,
		DiagnosticCategories.Usage,
		DiagnosticSeverity.Error,
		true);
}