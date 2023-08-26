using NlpService.Data.Abstractions;

namespace NlpService.Data;

public class NlpUnitOfWork : INlpUnitOfWork
{
    private readonly NamedEntityDbContext _context;
    private readonly INamedEntityRepository _namedEntityRepository;
    private readonly INamedEntityFormRepository _namedEntityFormRepository;
    private readonly INewsRepository _newsRepository;

    public INamedEntityRepository NamedEntityRepository => _namedEntityRepository;

    public INamedEntityFormRepository NamedEntityFormRepository => _namedEntityFormRepository;

    public INewsRepository NewsRepository => _newsRepository;

    public NlpUnitOfWork(NamedEntityDbContext namedEntityDbContext)
    {
        _context = namedEntityDbContext;
        _namedEntityRepository = new NamedEntityRepository(namedEntityDbContext);
        _namedEntityFormRepository = new NamedEntityFormRepository(namedEntityDbContext);
        _newsRepository = new NewsRepository(namedEntityDbContext);
    }
    public void Dispose()
    {
        _context.Dispose();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
