namespace Paraminter.Semantic.Attributes.Named.Koalemos.Commands;

using Microsoft.CodeAnalysis;

using System.Collections.Generic;

/// <summary>Represents data used to associate semantic named attribute arguments.</summary>
public interface IAssociateSemanticAttributeNamedData
{
    /// <summary>The names of named attribute parameters and the associated arguments.</summary>
    public abstract IReadOnlyList<KeyValuePair<string, TypedConstant>> Associations { get; }
}
