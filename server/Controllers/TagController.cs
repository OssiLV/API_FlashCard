using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Data.Context;
using server.Data.Entities;
using server.Dtos.Tag;
using server.Services.TagService;

namespace server.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost("addtag")]
        public async Task<IActionResult> AddTag( TagCreateRequest tagCreateRequest )
        {
            var result = await _tagService.AddTag(tagCreateRequest);

            if( !result ) return NotFound();
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTag( int id )
        {
            var result = await _tagService.DeleteTag(id);

            if( !result ) return NotFound();
            return Ok();
        }

        //Update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTag(TagUpdateRequest tagUpdateRequest )
        {
            var result = await _tagService.UpdateTag(tagUpdateRequest);

            if( !result ) return NotFound();
            return Ok();
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var listTags = await _tagService.GetTag();

        //    if( listTags == null ) return NotFound();

        //    return Ok(listTags);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById( int id )
        //{
        //    var tag = await _tagService.GetById(id);

        //    if( tag == null ) return NotFound();

        //    return Ok(tag);
        //}

        [HttpGet("alltags/{id}")]
        public async Task<IActionResult> GetAllTagByUserId( Guid id )
        {
            var tag = _tagService.GetAllTagsByUserId(id);

            if( tag == null ) return NotFound();

            return Ok(tag);
        }

        [HttpGet("total-page-tag/{userId}")]
        public IActionResult GetTotalPage(Guid userId )
        {
            var totalPage = _tagService.GetTotalPage(userId);   

            if(totalPage == null) return NotFound();
            return Ok(totalPage);
        }

        [HttpGet("{userId}/{numpage}")]
        public IActionResult GetAllTagByUserId(Guid userId, int numpage )
        {
            //byte[] bytes = new byte[16];
            //BitConverter.GetBytes(id).CopyTo(bytes, 0);

            var tags =  _tagService.GetAllTagsByUserIdAndPage(userId, numpage);
            if( tags == null ) return NotFound();
            return Ok(tags);
        }
    }
}
