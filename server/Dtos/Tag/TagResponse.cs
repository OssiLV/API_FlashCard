

using server.Data.Entities;

namespace server.Dtos.Tag
{
    public class TagResponse<T>
    {
        public int TotalSizePages { get; set; }
        public T Result { get; set; }

        public TagResponse(int TotalSizePages, T Result)
        {
            this.TotalSizePages = TotalSizePages;
            this.Result = Result;   
        }
    }
}
