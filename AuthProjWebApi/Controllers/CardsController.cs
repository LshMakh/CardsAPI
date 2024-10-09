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
        [Authorize(Roles ="Admin")]

        [HttpPost]

        public IActionResult SaveCard(Card card)
        {

            try
            {
                package.SaveCard(card);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status200OK);

        }


        [Authorize(Roles = "Admin")]
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
       
        [HttpGet("{id}")]

        public IActionResult GetCardbyId(int id) 
        {

            Card card;

            try
            {
                card = package.GetCardbyId(id);
            }
            catch (Exception ex) 
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

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


