using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Songify.Data;
using Songify.Services.PlaylistService;

namespace Songify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly DataContext _context;
        public static Playlist playlist = new Playlist();
        public static List<Playlist> playlists = new List<Playlist>();
        private readonly IConfiguration _configuration;
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IConfiguration configuration, IPlaylistService playlistService, DataContext context)
        {
            _context = context;
            _configuration = configuration;
            _playlistService = playlistService;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _playlistService.GetMyPlaylist();
            return Ok(userName);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(Playlist request)
        {
            playlist.Id = request.Id;
            playlist.playlist.Add(request.AddSong());

            playlists.Add(playlist);
            _context.playlist.Add(playlist);
            await _context.SaveChangesAsync();

            return Ok(playlist);
        }
    }
}
