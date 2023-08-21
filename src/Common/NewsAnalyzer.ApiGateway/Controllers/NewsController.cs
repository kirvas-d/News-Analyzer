using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAnalyzer.Application.NewsService;
using static NewsAnalyzer.Application.NewsService.ApplicationNews;

namespace NewsAnalyzer.ApiGateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController : ControllerBase
{
    private readonly ApplicationNewsClient _applicationNewsClient;

    public NewsController(ApplicationNewsClient applicationNewsClient)
    {
        _applicationNewsClient = applicationNewsClient;
    }

    [HttpGet]
    [Authorize]
    [Route("GetNewsById")]
    public async Task<ActionResult<NewsResponse>> GetNewsById([FromQuery] Guid newsId)
    {
        var news = await _applicationNewsClient.GetNewsAsync(new NewsRequest { Id = newsId.ToString() });
        if (news != null)
            return Ok(news);

        return NotFound();
    }

}
