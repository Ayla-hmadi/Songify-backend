using Microsoft.EntityFrameworkCore;
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
        public string? UserId { get; set; }
        
    }
}
