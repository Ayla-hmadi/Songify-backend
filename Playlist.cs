using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Songify
{
    public class Playlist
    {
        [Key]
        public int PlaylistId { get; set; }
        [Required]
        public string? name { get; set; } 
        [Required]
        public List<int> songs = new List<int>();
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Playlists")]
        public User User { get; set; }
    }
}
