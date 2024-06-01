using Microsoft.AspNetCore.Mvc;
using VendingMachine.Server.Autentification;
using VendingMachine.Server.Resources;

namespace VendingMachine.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IChecker<string> _passwordChecker;
        private readonly ILoader _fileLoader;

        public HomeController(IChecker<string> passwordChecker, ILoader fileLoader)
        {
            _passwordChecker = passwordChecker;
            _fileLoader = fileLoader;
        }

        [HttpGet("CheckPassword", Name = "checkPassword")]
        public bool CheckPassword(string password)
        {
            return _passwordChecker.Check(password);
        }

        [HttpGet("GetIcon", Name = "getIcon")]
        public IActionResult GetIcon(int nominal)
        {
            return _fileLoader.TryLoad($"./Images/Coin{nominal}.jpg", "image/jpeg");
        }
    }
}
