using Microsoft.EntityFrameworkCore;
using server.Data.Context;
using server.Data.Entities;
using server.Dtos.Tag;

namespace server.Services.TagService
{
    public class TagService : ITagService
    {
        private readonly CardDb _cardDb;
        public TagService(CardDb cardDb)
        {
            _cardDb = cardDb;
        }
        public async Task<bool> AddTag( TagCreateRequest tagCreateRequest )
        {
            var tag = new Tag
            {
                Name = tagCreateRequest.Name,
                Description = tagCreateRequest.Description,
                UserId = tagCreateRequest.UserId,
            };

            var check = await _cardDb.Tags.AddAsync(tag);
            if(check == null) return false;
            await _cardDb.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTag( int id )
        {
            var tag = await _cardDb.Tags.FirstOrDefaultAsync(x => x.Id == id);

            var success =  _cardDb.Tags.Remove(tag);
            if(success == null) return false;
            await _cardDb.SaveChangesAsync();
            return true;    
        }

        public async Task<bool> UpdateTag(TagUpdateRequest tagUpdateRequest )
        {
            var tag = await _cardDb.Tags.FirstOrDefaultAsync(x => x.Id == tagUpdateRequest.Id);

            if(tag == null) return false;
            
            tag.Name = tagUpdateRequest.Name;
            tag.Description = tagUpdateRequest.Description;

            await _cardDb.SaveChangesAsync();
            return true;
        }

        public async Task<Tag> GetById( int Id )
        {
            var tag = await _cardDb.Tags.FirstOrDefaultAsync(x => x.Id == Id);
            return tag;
        }

        public async Task<List<Tag>> GetTag()
        {
            var tags = await _cardDb.Tags.ToListAsync();
            if(tags != null) return tags;
            return null;
        }

        public  List<Tag> GetAllTagsByUserIdAndPage( Guid Id, int numpage )
        {
            int totalTagsOnPage = 12;

            List<Tag> tags = _cardDb.Tags.ToList().FindAll(t => t.UserId == Id);

            var tagsByNumpage = tags.Skip((numpage - 1) * totalTagsOnPage).Take(totalTagsOnPage);

            return tagsByNumpage.ToList();
        }
            
        public int GetTotalPage( Guid Id )
        {
            List<Tag> tags = _cardDb.Tags.ToList().FindAll(t => t.UserId == Id);
            int totalTagsOnPage = 12;
            int allTags = tags.Count();

            if(allTags % totalTagsOnPage !=0 )
            {
                return allTags / totalTagsOnPage + 1;
            } else 
            {
                return allTags / totalTagsOnPage;
            }   

           
        }


        public List<Tag> GetAllTagsByUserId( Guid Id )
        {
            List<Tag> tags =  _cardDb.Tags.ToList().FindAll(t => t.UserId == Id);
            

            return tags;
        }
    }
}
