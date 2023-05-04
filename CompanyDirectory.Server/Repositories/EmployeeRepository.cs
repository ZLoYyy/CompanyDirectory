using CompanyDirectory.Server.Context;
using CompanyDirectory.Server.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyDirectory.Server.Repositories
{
    internal class EmployeeRepository: DbRepository<Employee>
    {
        public EmployeeRepository(CompanyDirectoryDBContext db) : base(db) { }

        public override IQueryable<Employee> Items => base.Items
            .Include(item => item.CurrentDivision)
            .Include(item => item.CurrentPost);
    }
}
