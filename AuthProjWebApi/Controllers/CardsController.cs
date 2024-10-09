using AuthProjWebApi.Models;
using AuthProjWebApi.Packages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
        public IActionResult GetCards([FromQuery] string name = "", [FromQuery] string occupation = "")
        {

            List<Card> cards = new List<Card>();

            cards = package.GetCards();
            if (!string.IsNullOrEmpty(name))
            {
                cards = cards.Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(occupation))
            {
                cards = cards.Where(c => c.Occupation.Contains(occupation, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return StatusCode(StatusCodes.Status200OK, cards);
        }
        [Authorize(Roles ="Admin")]

        [HttpPost]

        public IActionResult SaveCard(Card card)
        {

                package.SaveCard(card);
                return StatusCode(StatusCodes.Status200OK);

        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteCard(int id)
        {
                package.DeleteCard(id);
                return StatusCode(StatusCodes.Status200OK);
        }
       
        [HttpGet("{id}")]

        public IActionResult GetCardbyId(int id) 
        {

                Card card;
                card = package.GetCardbyId(id);
                return StatusCode(StatusCodes.Status200OK, card);

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateCard(int id,[FromBody] Card card)
        {

            try
            {
                package.UpdateCard(card);
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

    }
}


