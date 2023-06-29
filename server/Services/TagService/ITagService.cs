using server.Data.Entities;
using server.Dtos.Tag;

namespace server.Services.TagService
{
    public interface ITagService
    {
        Task<Tag> GetById( int Id );
        Task<List<Tag>> GetTag();
        int GetTotalPage( Guid Id );
        List<Tag> GetAllTagsByUserIdAndPage(Guid Id, int numpage);
        List<Tag> GetAllTagsByUserId(Guid Id);
        Task<bool> AddTag( TagCreateRequest tagCreateRequest );
        Task<bool> UpdateTag( TagUpdateRequest tagRequest );
        Task<bool> DeleteTag( int id );

    }
}
