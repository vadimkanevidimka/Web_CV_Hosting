using AuthService.Buisness.Services.Implementations;
using AuthService.Buisness.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackgroundJobController : ControllerBase
    {
        private readonly BackgroundRefreshTokenService _jobService;
        public BackgroundJobController(IBackgroundRefreshTokenService jobService)
        {
            _jobService = (BackgroundRefreshTokenService)jobService;
        }

        [HttpGet("/Add-Deletion-Of-Unused-Tokens")]
        [AllowAnonymous]
        public async Task<IActionResult> AddDeletionOfUnusedToken()
        {
            _jobService.DeleteUnusedRefreshTokens();
            return Ok("Recurred");
        }
    }
}
