using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NewsService.Core.Abstractions;

namespace NewsAnalyzer.Application.NewsService.Services;

public class ApplicationNewsService : ApplicationNews.ApplicationNewsBase
{
    private readonly INewsAsyncRepository _newsAsyncRepository;

    public ApplicationNewsService(INewsAsyncRepository newsAsyncRepository) 
    {
        _newsAsyncRepository = newsAsyncRepository;
    }

    public override async Task<NewsResponse> GetNews(NewsRequest newsRequest, ServerCallContext context) 
    {
        var news = await _newsAsyncRepository.GetByIdAsync(Guid.Parse(newsRequest.Id));
        return new NewsResponse
        {
            Id = news.Id.ToString(),
            SourceName = news.SourceName,
            Title = news.Title,
            Text = news.Text,
            PublishDate = Timestamp.FromDateTime(news.PublishDate)
        };
    }
}