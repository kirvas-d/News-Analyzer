namespace NewsAnalyzer.Repository.Abstractions;

public interface IUnitOfWork : IDisposable
{
    void SaveChanges();
}
