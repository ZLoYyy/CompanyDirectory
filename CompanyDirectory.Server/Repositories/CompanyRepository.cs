using CompanyDirectory.Server.Context;
using CompanyDirectory.Server.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyDirectory.Server.Repositories
{
    internal class CompanyRepository : DbRepository<Company>
    {
        public CompanyRepository(CompanyDirectoryDBContext db) : base(db) { } 

        public override IQueryable<Company> Items => base.Items
            .Include(item => item.Divisions);

    }
}
