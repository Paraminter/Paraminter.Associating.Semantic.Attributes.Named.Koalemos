namespace Paraminter.Semantic.Attributes.Named.Koalemos.Models;

using Microsoft.CodeAnalysis;

using Paraminter.Models;

using System.Collections.Generic;

/// <summary>Represents data used to associate all semantic named attribute arguments with parameters.</summary>
public interface IAssociateAllSemanticAttributeNamedArgumentsData
    : IAssociateAllArgumentsData
{
    /// <summary>The names of named attribute parameters and the associated arguments.</summary>
    public abstract IReadOnlyList<KeyValuePair<string, TypedConstant>> Associations { get; }
}
