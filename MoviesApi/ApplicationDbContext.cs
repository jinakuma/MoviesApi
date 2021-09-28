using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Entities;

namespace MoviesApi
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) :base(options)
        {
            
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }

    }
}
