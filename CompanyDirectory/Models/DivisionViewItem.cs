using CompanyDirectory.Server.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyDirectory.Models
{
    internal class DivisionViewItem
    {
        public string Caption { get; set; }
        /// <summary>
        /// Список сотрудников
        /// </summary>
        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();
    }
}
