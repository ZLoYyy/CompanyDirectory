using CompanyDirectory.Server.Context;
using CompanyDirectory.Server.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyDirectory.Server.Repositories
{
    internal class DivisionRepository : DbRepository<Division>
    {
        public DivisionRepository(CompanyDirectoryDBContext db) : base(db) { }

        public override IQueryable<Division> Items => base.Items
            .Include(item => item.Companies)
            .Include(item => item.Posts)
            .Include(item => item.Employees);
    }
}
