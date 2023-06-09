using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Api.Entities
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public bool Shared { get; set; }

        [NotMapped]
        public bool Favorite { get; set; }
    }


}