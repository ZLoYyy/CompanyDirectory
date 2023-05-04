using CompanyDirectory.Server.Context;
using CompanyDirectory.Server.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDirectory.Data
{
    internal class DbInitializer
    {
        private readonly CompanyDirectoryDBContext _db;

        public DbInitializer(CompanyDirectoryDBContext db)
        {
            _db = db;

        }

        public async Task InitializeAsync()
        {
            await _db.Database.MigrateAsync().ConfigureAwait(false);

            if (await _db.Posts.AnyAsync()) return;

            //await InitializeCategories();
        }

        private const int __postCount = 3;

        private Post[] _posts;
        private async Task InitializeCategories()
        {

            _posts = new Post[__postCount];
            for (var i = 0; i < __postCount; i++)
                _posts[i] = new Post { Caption = $"Должность {i + 1}" };

            await _db.Posts.AddRangeAsync(_posts);
            await _db.SaveChangesAsync();
        }
    }
}
