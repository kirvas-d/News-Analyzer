using NlpService.Data.Abstractions;

namespace NlpService.Data;

public class NlpUnitOfWork : INlpUnitOfWork
{
    private readonly NamedEntityDbContext _context;
    private readonly INamedEntityRepository _namedEntityRepository;
    private readonly INamedEntityFormRepository _namedEntityFormRepository;
    private readonly ITextRepository _textRepository;

    public INamedEntityRepository NamedEntityRepository => _namedEntityRepository;

    public INamedEntityFormRepository NamedEntityFormRepository => _namedEntityFormRepository;

    public ITextRepository TextRepository => _textRepository;

    public NlpUnitOfWork(NamedEntityDbContext namedEntityDbContext)
    {
        _context = namedEntityDbContext;
        _namedEntityRepository = new NamedEntityRepository(namedEntityDbContext);
        _namedEntityFormRepository = new NamedEntityFormRepository(namedEntityDbContext);
        _textRepository = new TextRepository(namedEntityDbContext);
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
