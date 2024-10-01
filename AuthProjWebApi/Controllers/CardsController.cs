using AuthProjWebApi.Models;
using AuthProjWebApi.Packages;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AuthProjWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class CardsController : ControllerBase
    {
        IPKG_CARD package;


        public CardsController(IPKG_CARD package)
        {
            this.package = package;
        }

        List<Card> cards;

        [HttpGet]
        public IActionResult GetCards() {

            List<Card> cards = new List<Card>();
            try
            {
                cards = package.GetCards();
            }
            catch (Exception ex) {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
            return StatusCode(StatusCodes.Status200OK, cards);
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteCard(int id)
        {
            try
            {
                package.DeleteCard(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }

    
}


