using CompanyDirectory.Server.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CompanyDirectory.Server.Entities
{
    public class Division : NameEntity
    {
        /// <summary>
        /// Список сотрудников
        /// </summary>
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public virtual ICollection<Post> Posts { get; } = new List<Post>();

        public List<Company> Companies { get; } = new List<Company>();
    }
}
