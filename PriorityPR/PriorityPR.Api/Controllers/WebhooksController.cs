using Microsoft.AspNetCore.Mvc;

namespace PriorityPR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WebhooksController
{
    [HttpPost("Incoming/Github/PullRequests")]
    public async Task PullRequest()
    {
        // TODO:
    }
}