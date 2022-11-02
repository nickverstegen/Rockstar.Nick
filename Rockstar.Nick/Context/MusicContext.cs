using Microsoft.EntityFrameworkCore;
using Rockstar.Nick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rockstar.Nick.Context
{
  public class MusicContext : DbContext
  {
    public MusicContext(DbContextOptions options): base (options)
    {

    }

    public DbSet<Artist> Artist { get; set; }
    public DbSet<Song> Song { get; set; }
  }
}
