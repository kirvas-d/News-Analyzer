using AngleSharp.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Core.Abstractions;
using NewsAnalyzer.Core.Models;
using NewsAnalyzer.Infrastructure.EfCoreRepository.Mapping;
using NewsAnalyzer.Infrastructure.EfCoreRepository.Models;
using System.Linq.Expressions;

namespace NewsAnalyzer.Infrastructure.EfCoreRepository;

//EfCoreAsyncRepository<NamedEntityForm, Guid>,
public class NamedEntityFormEfCoreAsyncRepository : INamedEntityFormAsyncRepository
{
    private readonly IMapper _mapper;
    private readonly NamedEntityDbContext _context;

    public NamedEntityFormEfCoreAsyncRepository(NamedEntityDbContext dbContext)
    {
        _context = dbContext;

        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<NamedEntityFormDbEntitiesProfile>();
        });

        _mapper = mapperConfiguration.CreateMapper();
    }

    public async Task AddAsync(NamedEntityForm entity)
    {
        var dbEntity = _mapper.Map<NamedEntityFormDbEntity>(entity);

        foreach (var newsIdEntity in dbEntity.NewsIds)
        {
            _context.ChangeTracker.TrackGraph(
            dbEntity, node =>
            {
                var newsId = node.Entry.Property(nameof(newsIdEntity.NewsId)).CurrentValue;
                var entityType = node.Entry.Metadata;

                var existingEntity = node.Entry.Context.ChangeTracker.Entries()
                    .FirstOrDefault(
                        e => Equals(e.Metadata, entityType)
                             && Equals(nameof(newsIdEntity.NewsId), newsId));

                if (existingEntity == null)
                { 
                   node.Entry.State = EntityState.Modified;
                }
                else
                {
                    Console.WriteLine($"Discarding duplicate {entityType.DisplayName()} entity with key value {keyValue}");
                }
            });
        }

        await _context.NamedEntityFormDbEntities.AddAsync(dbEntity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<NamedEntityForm> entities)
    {
        var dbEntities = _mapper.Map<NamedEntityFormDbEntity[]>(entities);
        await _context.NamedEntityFormDbEntities.AddRangeAsync(dbEntities);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAllAsync()
    {
        return await _context.NamedEntityFormDbEntities.CountAsync();
    }

    public Task<NamedEntityForm?> FirstOrDefaultAsync(Expression<Func<NamedEntityForm, bool>> predicate)
    {
        throw new NotImplementedException();
        //return base.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<NamedEntityForm>> GetAllAsync()
    {
        var dbEntities = await _context.NamedEntityFormDbEntities.ToListAsync();
        return _mapper.Map<NamedEntityForm[]>(dbEntities);
    }

    public async Task<NamedEntityForm?> GetByIdAsync(Guid id)
    {
        var namedEntityDb = await _context.NamedEntityFormDbEntities.FindAsync(id);
        return _mapper.Map<NamedEntityForm>(namedEntityDb);
    }

    public async Task<IEnumerable<NamedEntityForm>?> GetByValueAsync(IEnumerable<string> values) 
    {
        var namedEntityDb = await _context.NamedEntityFormDbEntities.FirstOrDefaultAsync(entity => values.Contains(entity.Value));
        return _mapper.Map<NamedEntityForm[]>(namedEntityDb);
    }

    public Task<IEnumerable<NamedEntityForm>> GetWhereAsync(Expression<Func<NamedEntityForm, bool>> predicate)
    {
        throw new NotImplementedException();
        //return base.GetWhereAsync(predicate);
    }

    public Task RemoveAsync(NamedEntityForm entity)
    {
        throw new NotImplementedException();
        //return base.RemoveAsync(entity);
    }

    public async Task UpdateAsync(NamedEntityForm entity)
    {
        //var dbEntity = _mapper.Map<NamedEntityFormDbEntity>(entity);
        var dbEntity = await _context.NamedEntityFormDbEntities.FindAsync(entity.Id);
        //if (entity.NamedEntity != null)
        //{
        //    await _context.Set<NamedEntity>().AddAsync(entity.NamedEntity);

        //}
        foreach (var newsId in entity.NewsIds) 
        {
            if (!dbEntity.NewsIds.Select(n => n.NewsId).Contains(newsId)) 
            {
                var newsIdDbEntity = new NewsIdDbEntity() { NewsId = newsId };
                await _context.NewsIdDbEntyties.AddAsync(newsIdDbEntity);
                dbEntity.NewsIds.Add(newsIdDbEntity);
            }
        }

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        //return base.UpdateAsync(entity);
    }
}
