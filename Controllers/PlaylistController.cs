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
    
        [HttpGet]
        public ActionResult<string> GetAll(String user)
        {
            IEnumerable<Playlist> Playlists = ;
            return Ok(userName);
        }

        public async Task<ActionResult<User>> Register(Playlist request)
        {
            playlist.Id = request.Id;
            playlist.playlist.Add(request.AddSong())
            playlists.Add(playlist);
            _context.playlist.Add(playlist);
            await _context.SaveChangesAsync();

            return Ok(playlist);
        }
    }
}
