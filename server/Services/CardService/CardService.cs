using Microsoft.EntityFrameworkCore;
using server.Data.Context;
using server.Data.Entities;
using server.Dtos.Card;

namespace server.Services.CardService
{
    public class CardService : ICardService
    {
        private readonly CardDb _cardDb;

        public CardService( CardDb cardDb )
        {
            _cardDb = cardDb;
        }

        public async Task<bool> AddCard( CardCreateRequest cardRequest )
        {
            var card = new Card
            {
                Title = cardRequest.Title,
                Translate = cardRequest.Translate,
                TagId = cardRequest.TagId,
            };

            var check = await _cardDb.Cards.AddAsync(card);
            if( check == null ) return false;
            await _cardDb.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTCard( int id )
        {
            var card = await _cardDb.Cards.FirstOrDefaultAsync(x => x.Id == id);

            var success = _cardDb.Cards.Remove(card);
            if( success == null ) return false;
            await _cardDb.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCard( CardUpdateRequest cardUpdateRequest )
        {
            var card = await _cardDb.Cards.FirstOrDefaultAsync(x => x.Id == cardUpdateRequest.Id);

            if( card == null ) return false;

            card.Title = cardUpdateRequest.Title;
            card.Translate = cardUpdateRequest.Translate;

            await _cardDb.SaveChangesAsync();
            return true;
        }

        public async Task<Card> GetById( int Id )
        {
            var card = await _cardDb.Cards.FirstOrDefaultAsync(c => c.Id == Id);
            return card;
        }

        public async Task<List<Card>> GetCard()
        {
            var cards = await _cardDb.Cards.ToListAsync();
            return cards;
        }

        public List<Card> GetAllCardsByTagId( int id, int numpage )
        {
            int totalTagsOnPage = 12;

            var cards = _cardDb.Cards.ToList().FindAll(c => c.TagId == id);

            var cardsByNumpage = cards.Skip((numpage - 1) * totalTagsOnPage).Take(totalTagsOnPage);

            return cardsByNumpage.ToList();
        }

        public int GetTotalPage( int id )
        {
            int totalTagsOnPage = 12;
            var cards = _cardDb.Cards.ToList().FindAll(c => c.TagId == id);
            int allCards = cards.Count();

            if( allCards % totalTagsOnPage != 0 )
            {
                return allCards / totalTagsOnPage + 1;
            }
            else
            {
                return allCards / totalTagsOnPage;
            }
        }

        public async Task<bool> UpdateStatusCard( CardUpdate_Status cardUpdate_Status )
        {
            var card = await _cardDb.Cards.FindAsync(cardUpdate_Status.Id);

            if( card == null ) return false;

            card.Status = cardUpdate_Status.Status;
            await _cardDb.SaveChangesAsync();
            return true;
        }
    }
}