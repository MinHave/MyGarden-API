using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGarden_API.Services;

namespace MyGarden_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("{mailAddress}")]
        public IActionResult PostDefaultMail(string mailAddress)
        {
            Console.WriteLine(mailAddress);
            _mailService.SendNewUserEmailAsync(mailAddress, "Test username", "empty");
            return Ok();

        }
    }
}
