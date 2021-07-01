using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<ApiUser> _userManager;
        //private readonly SignInManager<ApiUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<ApiUser> userManager,
            //SignInManager<ApiUser> signInManager,
            ILogger<AccountController> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            //_signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attempt for {userDTO.Email}");
            if (!ModelState.IsValid) // this is how you validate forms in asp.net core - based on prop attributes
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = user.Email;
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return BadRequest(ModelState);
                }

                await _userManager.AddToRolesAsync(user, userDTO.Roles);

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        //{
        //    _logger.LogInformation($"Login Attempt for {loginUserDTO.Email}");
        //    if (!ModelState.IsValid) // this is how you validate forms in asp.net core - based on prop attributes
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(loginUserDTO.Email, loginUserDTO.Password, false, false);

        //        if (!result.Succeeded)
        //        {
        //            return Unauthorized(loginUserDTO);
        //        }

        //        return Accepted();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
        //        return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
        //    }
        //}
    }
}
