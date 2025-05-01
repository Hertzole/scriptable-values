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
		nameof(Resources.HSV0100Title), Resources.ResourceManager, typeof(Resources));
	private static readonly LocalizableResourceString typeNotSupportedForCallbackAttributeMessage = new LocalizableResourceString(
		nameof(Resources.HSV0100MessageFormat), Resources.ResourceManager, typeof(Resources));

	private static readonly LocalizableResourceString typeNotSupportedTitle = new LocalizableResourceString(
		nameof(Resources.HSV0101Title), Resources.ResourceManager, typeof(Resources));
	private static readonly LocalizableResourceString typeNotSupportedMessage = new LocalizableResourceString(
		nameof(Resources.HSV0101MessageFormat), Resources.ResourceManager, typeof(Resources));

	private static readonly LocalizableResourceString incorrectAttributeUsageTitle = new LocalizableResourceString(
		nameof(Resources.HSV0102Title), Resources.ResourceManager, typeof(Resources));
	private static readonly LocalizableResourceString incorrectAttributeUsageMessage = new LocalizableResourceString(
		nameof(Resources.HSV0102MessageFormat), Resources.ResourceManager, typeof(Resources));

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

	public static readonly DiagnosticDescriptor TypeNotSupported = new DiagnosticDescriptor(
		DiagnosticIdentifiers.TypeNotSupported,
		typeNotSupportedTitle,
		typeNotSupportedMessage,
		DiagnosticCategories.Usage,
		DiagnosticSeverity.Error,
		true);

	public static readonly DiagnosticDescriptor IncorrectAttributeUsage = new DiagnosticDescriptor(
		DiagnosticIdentifiers.IncorrectAttributeUsage,
		incorrectAttributeUsageTitle,
		incorrectAttributeUsageMessage,
		DiagnosticCategories.Usage,
		DiagnosticSeverity.Error,
		true);
}