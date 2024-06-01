using Microsoft.AspNetCore.Mvc;
using VendingMachine.Server.Models.Coin;
using VendingMachine.Server.Repositories;

namespace VendingMachine.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoinController : Controller
    {
        private readonly ICoinRepository _repository;

        public CoinController(ICoinRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("AllCoin", Name = "allCoin")]
        public async Task<IEnumerable<Coin>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        [HttpPost("UpdateCoin", Name = "updateCoin")]
        public async Task<IActionResult> UpdateCoin(Coin coin)
        {
            await _repository.UpdateAsync(coin);
            return Ok();
        }
    }
}
