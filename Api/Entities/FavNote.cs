using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities
{
    [Table("fav_notes")]
    public class FavNote
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("note_id")]
        [Column("note_id")]
        public int NoteId { get; set; }

        [ForeignKey("user_id")]
        [Column("user_id")]
        public int UserId { get; set; }
    }
}
