using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;


using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Concessionária.Data;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;
using Concessionária.Entities;
using Azure.Core;
using Concessionária.Migrations;


namespace Concessionária.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly DataBaseContext _context;
       
        public  CarsController(DataBaseContext context) 
        {
            _context = context;
        }




        private static readonly List<Car> Cars = [
            new Car{}
            ];


            
        [HttpGet("GetAllCars")]
        public async Task<ActionResult<Car>> GetAllcars()
        {
           var Cars = await  _context.Cars.ToListAsync();

            return Ok(Cars);
        }

        [HttpGet("getonecar{id}")]

        public async Task<ActionResult> Getonecar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)

                return NotFound("this car doesn't exist in our Db");


            return Ok(car);
        }

        [HttpPost]

        public async Task<ActionResult<List<Car>>> AddCar(Car car)
        {
            _context.Cars.Add(car);
           
            await _context.SaveChangesAsync();
            
            return Ok(await _context.Cars.ToListAsync());

        }

        [HttpGet("Filtering")]
        
        public async Task<ActionResult<IEnumerable<Car>>> FilteringCar([FromQuery] string name, [FromQuery] string brand, [FromQuery] int? year, [FromQuery] decimal? price, [FromQuery] bool solded)
        {
            IQueryable<Car> query = _context.Cars;

            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name.Contains(name));

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(c => c.Brand.Contains(brand));

            if (year.HasValue)
                query = query.Where(c => c.Year == year);

            if (price.HasValue)
                query = query.Where(c => c.Price <= price);
            if (solded.Equals(true))
            {
                query = query.Where(c => c.Solded == true);
            }

            var filteredCars = await query.ToListAsync();

            return Ok(filteredCars);
        }

        [HttpPut("sell{id}")]
        public async Task<ActionResult<Car>> SellCar(int id, decimal discount, DateTime saleDate)
        {
            var car = await _context.Cars.FindAsync(id);
            

            if (car == null)
            {
                return BadRequest("Car not found.");
            }

            if (car.Solded)
            {
                return BadRequest("Car is already sold.");
            }
     
            if (discount > 0)
            {
                car.Discount = discount;
                car.Price -= discount; // Apply the discount to the sale price
            }
            
            car.Solded = true;
            car.SaleDate = saleDate;

            await _context.SaveChangesAsync();
            
            return Ok($"O carro foi marcado como vendido");  
        }
        
        [HttpGet("sales-statistics")]
        public IActionResult GetSalesStatistics(DateTime startDate, DateTime endDate)
        {
            var totalCarsSold = _context.Cars.Count(c => c.Solded && c.SaleDate >= startDate && c.SaleDate <= endDate);
            var totalRevenue = _context.Cars.Where(c => c.Solded && c.SaleDate >= startDate && c.SaleDate <= endDate)
                                            .Sum(c => (c.Price - c.Discount));

            return Ok(new
            {
                TotalCarsSold = totalCarsSold,
                TotalRevenue = totalRevenue
            });
        }

        [HttpPut]
        public async Task<ActionResult<List<Car>>> EditCar(int id, Car request)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)

                return NotFound("This car doesn't exist");

            car.Name = request.Name;
            car.Year = request.Year;
            car.Brand = request.Brand;
            car.Price = request.Price;
            car.Discount = request.Discount;
            car.SaleDate = request.SaleDate;
            

            await _context.SaveChangesAsync();

            return Ok(await _context.Cars.ToListAsync());
        }

        [HttpGet("cars")]
        public IActionResult GetCars(int page = 1, int pageSize = 5)
        {
            IQueryable<Car> query = _context.Cars;

            // Paginação
            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Limitar os resultados à página solicitada
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var cars = query.ToList();

            return Ok(new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                Page = page,
                PageSize = pageSize,
                Cars = cars
            });
        }

            [HttpDelete]

            public async Task<ActionResult<List<Car>>> DeleteCar(int id)

            {
                var car = await _context.Cars.FindAsync(id);
                if (car == null)
                    return NotFound("This car doesn`t exist");
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();

                return Ok(Cars);
            }

    }
    
}
