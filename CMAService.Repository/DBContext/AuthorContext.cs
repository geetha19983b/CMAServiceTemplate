
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace CMAService.Repository
{
    public class AuthorContext : DbContext
    {
        public AuthorContext(DbContextOptions<AuthorContext> options)
          : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }

    }
}
