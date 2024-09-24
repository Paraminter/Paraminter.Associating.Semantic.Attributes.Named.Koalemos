namespace Paraminter.Associating.Semantic.Attributes.Named.Koalemos.Models;

using Microsoft.CodeAnalysis;

using Paraminter.Associating.Models;

using System.Collections.Generic;

/// <summary>Represents data used to associate semantic named attribute arguments with parameters.</summary>
public interface IAssociateSemanticAttributeNamedArgumentsData
    : IAssociateArgumentsData
{
    /// <summary>The names of named attribute parameters and the associated arguments.</summary>
    public abstract IReadOnlyList<KeyValuePair<string, TypedConstant>> Associations { get; }
}
