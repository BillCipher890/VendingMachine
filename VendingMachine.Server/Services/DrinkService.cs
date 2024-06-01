using Microsoft.AspNetCore.Mvc;
using VendingMachine.Server.CountChange;
using VendingMachine.Server.DrinkLogic;
using VendingMachine.Server.Models;
using VendingMachine.Server.Repositories;

namespace VendingMachine.Server.Services
{
    public class DrinkService : Controller, IDrinkService
    {
        private readonly IDrinkRepository _drinkRepository;
        private readonly ICoinRepository _coinRepository;
        private readonly ICountChange _countChange;
        private readonly ISaver _imageSaver;

        public DrinkService(IDrinkRepository drinkRepository, ICoinRepository coinRepository, ICountChange countChange, ISaver saver)
        {
            _drinkRepository = drinkRepository;
            _coinRepository = coinRepository;
            _countChange = countChange;
            _imageSaver = saver;
        }

        public async Task<IEnumerable<Drink>> GetAll()
        {
            return await _drinkRepository.GetAllAsync();
        }

        public async Task<IActionResult> UpdateDrink(Drink drink)
        {
            try
            {
                await _drinkRepository.UpdateAsync(drink);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetImage(string guid)
        {
            try
            {
                var drink = await _drinkRepository.GetAsync(guid);

                if (!System.IO.File.Exists(drink.ImagePath))
                    return NotFound();

                var imageBytes = System.IO.File.ReadAllBytes(drink.ImagePath);
                return File(imageBytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка: {ex.Message}");
            }
        }

        public async Task<IActionResult> AddDrink(Drink drink)
        {
            try
            {
                await _drinkRepository.AddAsync(drink);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка: {ex.Message}");
            }
        }

        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var form = Request.Form;
                var filePath = _imageSaver.Save(form);

                if (Guid.TryParse(form["guid"], out var guid) && guid.Equals(Guid.Empty))
                    throw new Exception("Incorrect guid!");

                var drink = await _drinkRepository.GetAsync(guid.ToString());
                drink.ImagePath = filePath;
                await _drinkRepository.UpdateAsync(drink);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteDrink(Drink drink)
        {
            try
            {
                if (System.IO.File.Exists(drink.ImagePath))
                    System.IO.File.Delete(drink.ImagePath);

                await _drinkRepository.DeleteAsync(drink);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка: {ex.Message}");
            }
        }

        public async Task<IActionResult> ClientGetDrink(PurchaseData data)
        {
            try
            {
                await _drinkRepository.UpdateAsync(data.Drink);
                var coins = await _coinRepository.GetAllAsync();
                var coinsToReturn = _countChange.GetChange(coins, data.Money, data.Drink.Cost);

                foreach (var coinResult in coinsToReturn)
                {
                    var coin = coins.FirstOrDefault(c => c.NominalValue == coinResult[1]);
                    if (coin == null)
                        return StatusCode(500, $"В системе не существует монета с номиналом {coinResult[1]}");

                    coin.Count -= coinResult[0];
                    await _coinRepository.UpdateAsync(coin);
                }

                var result = $"Сдача:{Environment.NewLine}";
                for (int i = 0; i < coinsToReturn.Length; i++)
                {
                    if (coinsToReturn[i][0] > 0)
                    {
                        result += $"{coinsToReturn[i][0]} монет(ы) номиналом {coinsToReturn[i][1]} {Environment.NewLine}";
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
