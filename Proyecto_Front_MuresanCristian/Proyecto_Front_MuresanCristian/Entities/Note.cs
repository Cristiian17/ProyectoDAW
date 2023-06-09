namespace Proyecto_Front_MuresanCristian.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Shared { get; set; }
    }


}