using System.ComponentModel.DataAnnotations;

namespace Songify
{
    public class Favorites
    {
        [Key]
        public string Username { get; set; } = string.Empty;
        public string Songname { get; set; } = string.Empty;
        public bool Fav { get; set; } = false;
    }
}
