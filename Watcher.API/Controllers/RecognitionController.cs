using Microsoft.AspNetCore.Mvc;
using Watcher.API.Providers;

namespace Watcher.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecognitionController : ControllerBase
    {
        private IRecogProvider _provider;

        public RecognitionController(IRecogProvider recogProvider)
        {
            _provider = recogProvider;
        }


        [HttpPost]
        public async Task<IActionResult> PersonDetected([FromForm] IFormFile image)
        {
            using var stream = image.OpenReadStream();
            await _provider.PersonDetected(stream);
            return Ok();

        }


    }
}
