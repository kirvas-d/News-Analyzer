namespace NewsAnalyzer.Core.Models;

public class NamedEntity
{
    public Guid Id { get; init; }

    public string Value { get; init; }

    public IReadOnlyList<NamedEntiryForm>? namedEntiryForms { get; init; }
}
