using NewsAnalyzer.Repository.Abstractions;

namespace NlpService.Data.Abstractions;

public interface INlpUnitOfWork : IUnitOfWork
{
    INamedEntityRepository NamedEntityRepository { get; }
    INamedEntityFormRepository NamedEntityFormRepository { get; }
    INewsRepository NewsRepository { get; }
}
