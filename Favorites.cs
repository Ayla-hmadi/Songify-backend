using System.ComponentModel.DataAnnotations;

namespace Songify
{
    public class Favorites
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public List<int> playlist = new List<int>();

        public int AddSong() 
        {
            return 0;
        }
    }
}
