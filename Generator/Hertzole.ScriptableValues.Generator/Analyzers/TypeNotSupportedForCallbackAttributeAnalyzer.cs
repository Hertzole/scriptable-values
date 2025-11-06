using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hertzole.ScriptableValues.Generator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class TypeNotSupportedForCallbackAttributeAnalyzer : DiagnosticAnalyzer
{
    private static readonly ImmutableArray<DiagnosticDescriptor> supportedDiagnostics =
        ImmutableArray.Create(DiagnosticRules.TypeNotSupportedForCallbackAttribute);

    private static readonly SyntaxKind[] supportedSyntaxKinds = [SyntaxKind.Attribute];

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    {
        get { return supportedDiagnostics; }
    }

    /// <inheritdoc />
    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        context.RegisterSyntaxNodeAction(Action, supportedSyntaxKinds);
    }

    private static void Action(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not AttributeSyntax attributeSyntax)
        {
            return;
        }

        context.CancellationToken.ThrowIfCancellationRequested();

        INamedTypeSymbol? attributeSymbol = attributeSyntax.GetAttributeSymbol(context.SemanticModel, context.CancellationToken);
        if (attributeSymbol == null)
        {
            return;
        }

        if (!attributeSymbol.TryGetCallbackAttribute(out CallbackType callbackType))
        {
            // This is not a callback attribute, so we don't need to do anything.
            return;
        }

        // Get member declaration
        MemberDeclarationSyntax? memberDeclaration = attributeSyntax.FirstAncestorOrSelf<MemberDeclarationSyntax>();
        if (memberDeclaration == null)
        {
            return;
        }

        // Get wrapper
        if (!memberDeclaration.TryGetFieldOrPropertyWrapper(context.SemanticModel, out ImmutableArray<FieldOrPropertyWrapper> wrappers) ||
            wrappers.IsDefaultOrEmpty)
        {
            return;
        }

        TypeDeclarationSyntax? containingTypeDeclaration = memberDeclaration.FirstAncestorOrSelf<TypeDeclarationSyntax>();
        if (containingTypeDeclaration == null)
        {
            return;
        }

        // Make sure the containing type has the marker attribute
        if (context.SemanticModel.GetDeclaredSymbol(containingTypeDeclaration) is not INamedTypeSymbol containingTypeSymbol ||
            !containingTypeSymbol.HasAttribute(Types.GLOBAL_MARKER_ATTRIBUTE))
        {
            return;
        }

        FieldOrPropertyWrapper wrapper = wrappers.ItemRef(0);
        if (!ScriptableValueHelper.TryGetScriptableType(context.SemanticModel, (INamedTypeSymbol) wrapper.Type, out ScriptableType wrapperType, out _))
        {
            return;
        }

        // The correct callback attribute was used.
        if (ScriptableValueHelper.IsSupportedType(in callbackType, in wrapperType))
        {
            return;
        }

        string correctAttributeName = GetCorrectAttributeName(wrapperType);

        ImmutableDictionary<string, string?>.Builder propertiesBuilder = ImmutableDictionary.CreateBuilder<string, string?>();
        propertiesBuilder.Add("correctAttributeName", correctAttributeName);

        context.ReportDiagnostic(Diagnostic.Create(DiagnosticRules.TypeNotSupportedForCallbackAttribute, attributeSyntax.GetLocation(),
            propertiesBuilder.ToImmutable(), attributeSymbol.Name, correctAttributeName));
    }

    private static string GetCorrectAttributeName(in ScriptableType scriptableType)
    {
        switch (scriptableType)
        {
            case ScriptableType.Event:
            case ScriptableType.GenericEvent:
                return "GenerateEventCallbackAttribute";
            case ScriptableType.Value:
                return "GenerateValueCallbackAttribute";
            case ScriptableType.Pool:
                return "GeneratePoolCallbackAttribute";
            case ScriptableType.List:
            case ScriptableType.Dictionary:
                return "GenerateCollectionCallbackAttribute";
            default:
                return "UNKNOWN";
        }
    }
}