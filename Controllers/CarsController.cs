using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PlatformService.Models;
using PlatformService.Services.Car;

namespace PlatformService.Controllers
{
        [ApiController]
        [Route("[controller]")]
        public class CarsController : ControllerBase
        {
            private readonly ICarService _carService;

            public CarsController(ICarService carService)
            {
                _carService = carService;
            }

            [HttpPost]
            public async Task<IActionResult> Create(Car car)
            {
                var id = await _carService.Create(car);

                return new JsonResult(id.ToString());
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> Get(string id)
            {
                var car = await _carService.Get(ObjectId.Parse(id));

                return new JsonResult(car);
            }

            [HttpGet]
            public async Task<IActionResult> Get()
            {
                var cars = await _carService.Get();

                return new JsonResult(cars);
            }

            [HttpGet("ByMake/{make}")]
            public async Task<IActionResult> GetByMake(string make)
            {
                var cars = await _carService.GetByMake(make);

                return new JsonResult(cars);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(string id, Car car)
            {
                var result = await _carService.Update(ObjectId.Parse(id), car);

                return new JsonResult(result);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(string id)
            {
                var result = await _carService.Delete(ObjectId.Parse(id));

                return new JsonResult(result);
            }
        }
}