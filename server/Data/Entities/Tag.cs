namespace server.Data.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }

        public List<Card> Cards { get; set; }
        public AppUser AppUser { get; set; }
    }
}
