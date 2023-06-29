namespace server.Data.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Translate { get; set; }
        public string Status { get; set; } = "pending";
        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}