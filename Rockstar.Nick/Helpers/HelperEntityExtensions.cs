using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rockstar.Nick.Helpers
{
  public static class HelperEntityExtensions
  {
    public static void Clear<T>(this DbSet<T> dbSet) where T : class
    {
      dbSet.RemoveRange(dbSet);
    }
  }
}
