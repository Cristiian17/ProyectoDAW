using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities
{
    [Table("shared_notes")]
    public class SharedNote
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Note")]
        [Column("note_id")]
        public int NoteId { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey("User")]
        [Column("shared_user_id")]
        public int SharedUserId { get; set; }
    }
}
