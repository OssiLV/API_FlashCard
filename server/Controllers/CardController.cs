using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.Card;
using server.Services.CardService;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController( ICardService cardService )
        {
            _cardService = cardService;
        }

        [HttpPost("addcard")]
        public async Task<IActionResult> AddTag( CardCreateRequest cardRequest )
        {
            var result = await _cardService.AddCard(cardRequest);

            if( !result ) return NotFound();
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTag( int id )
        {
            var result = await _cardService.DeleteTCard(id);

            if( !result ) return NotFound();
            return Ok();
        }

        //Update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTag( CardUpdateRequest cardUpdateRequest )
        {
            var result = await _cardService.UpdateCard(cardUpdateRequest);

            if( !result ) return NotFound();
            return Ok();
        }

        [HttpPut("update-status-card")]
        public async Task<IActionResult> UpdateStatusCard( CardUpdate_Status cardUpdate_Status )
        {
            var check = await _cardService.UpdateStatusCard(cardUpdate_Status);

            if( !check ) return NotFound();
            return Ok(check);
        }


        [HttpGet("{tagId}/{numpage}")]
        public IActionResult GetAllCardByTagId( int tagId, int numpage )
        {
            var cards = _cardService.GetAllCardsByTagId(tagId, numpage);
            if( cards == null ) return NotFound();

            return Ok(cards);
        }

        [HttpGet("total-page-card/{tagId}")]
        public IActionResult GetTotalPage( int tagId )
        {
            var totalPage = _cardService.GetTotalPage(tagId);

            if( totalPage == null ) return NotFound();
            return Ok(totalPage);
        }

        
    }
}