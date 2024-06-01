namespace NlpService.Core.Abstractions;

public interface INerService
{
    IEnumerable<string> GetNamedEntityFormsFromNews(string text);
}
