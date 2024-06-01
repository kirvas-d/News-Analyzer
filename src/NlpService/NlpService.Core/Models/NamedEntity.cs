namespace NlpService.Core.Models;

public class NamedEntity
{
    public Guid Id { get; init; }

    public string Value { get; init; } = string.Empty;

    public IReadOnlyList<NamedEntityForm>? NamedEntityForms { get; init; }
}
