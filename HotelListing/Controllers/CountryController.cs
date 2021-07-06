using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")] // attribute routing
    [ApiController]
    public class CountryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(
            IUnitOfWork unitOfWork, 
            ILogger<CountryController> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork
                    .Countries
                    .GetAll();
                var results = _mapper.Map<List<CountryDTO>>(countries);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }

        [HttpGet("{id:int}", Name ="GetCountry")] // Added name so sibling methods can call it with CreateAtRoute
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _unitOfWork
                    .Countries
                    .Get(
                        expression: c => c.Id == id, 
                        includes: new List<string>() { "Hotels" }
                    );
                var results = _mapper.Map<CountryDTO>(country);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountry)}");
                return StatusCode(500, "Internal Server Error. Please try again later.");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCountry([FromBody] CountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = _mapper.Map<Country>(countryDTO);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetCountry", new { id = country.Id }, country); // calls GetCountry with Id param
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred in {nameof(CreateCountry)}");
                return StatusCode(500, "Internal Server Error. Please try again later");
            }
        }


        [Authorize(Roles = "Administrator")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.Id == id);
                if(country == null)
                {
                    _logger.LogError($"Invalid POST attempt in {nameof(UpdateCountry)}");
                    return BadRequest("Invalid data submitted: Id");
                }

                _mapper.Map(countryDTO, country);
                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred in {nameof(UpdateCountry)}");
                return StatusCode(500, "Internal Server Error. Please try again later");
            }
        }

    }
}
