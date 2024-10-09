using AuthProjWebApi.Auth;
using AuthProjWebApi.Models;
using AuthProjWebApi.Packages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthProjWebApi.Controllers

{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : MainController
    {
        IPKG_USERS package;
        
        private readonly IJwtManager jwtManager;

        public UsersController(IPKG_USERS package, IJwtManager jwtmanager)
        {
            this.package = package;
            this.jwtManager = jwtmanager;
        }

        List<User> users;


        [HttpGet("{name}")]
       
        public IActionResult get_user_name(string name) 
        {
            User user;
            try
            {
                user = package.get_user_name(name);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(user);

        }

        [HttpGet]
        
        public IActionResult get_users()
        {
            List<User> users = new List<User>();
            users = package.get_users();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult add_user(User user)
        {

            package.add_user(user);
            return Ok();

        }

        [HttpPost]
      
        public IActionResult Authenticate(Login loginData)
        {
           Token? token = null;
            User? user = null;
            try
            {
                user = package.authenticate(loginData);

                if (user == null) return Unauthorized("Incorrect username or password");

                token = jwtManager.GetToken(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(token);
        }

    }
}
