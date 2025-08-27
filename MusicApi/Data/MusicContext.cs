using Microsoft.EntityFrameworkCore;
using MusicApi.Models;

namespace MusicApi.Data
{
    public class MusicContext : DbContext
    {
        public MusicContext(DbContextOptions<MusicContext> opts) : base (opts) 
        {

        }
        public DbSet <Music> Musics { get; set; }
    }
}
