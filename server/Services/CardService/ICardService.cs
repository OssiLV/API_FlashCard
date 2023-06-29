using server.Data.Entities;
using server.Dtos.Card;

namespace server.Services.CardService
{
    public interface ICardService
    {
        Task<Card> GetById( int Id );

        Task<List<Card>> GetCard();

        int GetTotalPage( int Id );

        List<Card> GetAllCardsByTagId( int id, int numpage );

        Task<bool> AddCard( CardCreateRequest cardRequest );

        Task<bool> DeleteTCard( int id );

        Task<bool> UpdateCard( CardUpdateRequest cardUpdateRequest );

        Task<bool> UpdateStatusCard( CardUpdate_Status cardUpdate_Status );
    }
}