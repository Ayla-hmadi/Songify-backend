namespace Songify.Services.PlaylistService
{
    public class PlaylistService : IPlaylistService
    {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public PlaylistService(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public string GetMyPlaylist()
            {
            var result = string.Empty;
                if (_httpContextAccessor.HttpContext != null)
                {
                    //result = _httpContextAccessor.HttpContext..FindFirstValue(ClaimTypes.Name);
                }
                return result;
            }
    }
}
