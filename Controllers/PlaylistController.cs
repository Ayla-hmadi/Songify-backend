using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Songify.Data;
using SQLitePCL;

namespace Songify.Controllers
{
    [Route("api/PlaylistController")]
    [ApiController]
    public class PlaylistController : Controller
    {
        private readonly DataContext _context;

        public PlaylistController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("getplaylists/{user}")]
        public ActionResult GetAllUserPlaylist(String user)
        {
            IEnumerable<Playlist> list = _context.playlist.Where(m => m.UserId == user);

            return Ok(list);
        }

        [HttpGet("getsongs/{id}")]
        public ActionResult GetSongsInPlaylist(int id)
        {
            IEnumerable<PlaylistSong> list = _context.playlistSong.Where(m => m.PlaylistId == id);
            List<int> songs = new List<int>();
            
            foreach (PlaylistSong song in list)
            {
                songs.Add(song.songId);
            }
            return Ok(songs);
        }




        [HttpPost("create/{user}/{name}")]
        public ActionResult Create(String user,String name)
        {
            Playlist playlist = new Playlist();
            playlist.UserId = user;
            playlist.name = name;
            _context.playlist.Add(playlist);
            _context.SaveChanges();
            
            return Ok(playlist);
        }

        [HttpPost("AddSongToPlaylist/{PlaylistId}/{song}")]
        public ActionResult AddSongToPlaylist(int PlaylistId, int song)
        {
            PlaylistSong playlistSong = new PlaylistSong();
            playlistSong.PlaylistId= PlaylistId;
            playlistSong.songId= song;

            _context.playlistSong.Add(playlistSong);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("RemoveSongFromPlaylist/{PlaylistId}/{song}")]
        public ActionResult RemoveSongFromPlaylist(int PlaylistId, int song)
        {
            PlaylistSong playlistSong = _context.playlistSong.Where(m => m.songId== song && m.PlaylistId == PlaylistId).First();
            _context.playlistSong.Remove(playlistSong);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("RemovePlaylist/{id}")]
        public ActionResult RemovePlaylist(int id)
        {
            IEnumerable<PlaylistSong> list = _context.playlistSong.Where(m => m.PlaylistId == id);
            foreach (PlaylistSong item in list)
            {
                _context.playlistSong.Remove(item);
            }
            Playlist p = _context.playlist.Where(m => m.PlaylistId == id).First();
            _context.playlist.Remove(p);
            _context.SaveChanges();
            return Ok();

        }






    }
}
