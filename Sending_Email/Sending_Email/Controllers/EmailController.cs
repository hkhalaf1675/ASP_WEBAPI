using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sending_Email.Services;

namespace Sending_Email.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMailingService mailingService;

        public EmailController(IMailingService _mailingService)
        {
            mailingService = _mailingService;
        }

        [HttpGet]
        public async Task<IActionResult> SendEmail(string email)
        {
            await mailingService.SendEmailAsync(email, "Reset Password", $"Your Code is ");

            return Ok("send successfully");

        }
    }
}
