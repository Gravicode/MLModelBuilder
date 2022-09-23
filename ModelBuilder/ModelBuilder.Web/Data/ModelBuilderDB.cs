using Microsoft.EntityFrameworkCore;
using ModelBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelBuilder.Web.Data;

namespace ModelBuilder.Web.Data
{
   
    public class ModelBuilderDB : DbContext
    {
        public DbSet<MLModel> MLModels { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public string DbPath { get; }

        public ModelBuilderDB()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            DbPath = System.IO.Path.Join(path, "/model-builder");
            if (!Directory.Exists(DbPath))
                Directory.CreateDirectory(DbPath);
            DbPath = System.IO.Path.Join(DbPath, "/ml.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    
}
