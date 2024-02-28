using Microsoft.AspNetCore.Mvc;

namespace ChitChat.Web.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult DisplayMessage()
        {
            return Ok();
        }
    }
}
