namespace NlpService.Data.Abstractions;

using NewsAnalyzer.Repository.Abstractions;

public interface INlpUnitOfWork : IUnitOfWork
{
    INamedEntityRepository NamedEntityRepository { get; }
    INamedEntityFormRepository NamedEntityFormRepository { get; }
    ITextRepository TextRepository { get; }
}
