using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthProjWebApi.Models;

namespace AuthProjWebApi.Controllers
{

    public class MainController : ControllerBase
    {
        protected User? AuthUser
        {
            get
            {
                User? user = null;
                var currentUser = HttpContext.User;
                if (currentUser != null && currentUser.HasClaim(c => c.Type == "Id"))
                {
                    user = new User();
                    user.Id = int.Parse(currentUser.Claims.FirstOrDefault(c=>c.Type=="Id").Value);
                }
                return user;
            }
        }
    }
}
