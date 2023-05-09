using CompanyDirectory.Server.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyDirectory.Server.Entities
{
    public class Company : NameEntity
    {
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateCreate { get; set; }
        /// <summary>
        /// Юридический адрес
        /// </summary>
        public string LegalAddress { get; set; } = string.Empty;
        /// <summary>
        /// Список подразделений
        /// </summary>
        public virtual ICollection<Division> Divisions { get; } = new List<Division>();
        /// <summary>
        /// Список сотрудников
        /// </summary>
        public virtual List<Employee> Employees { get; } = new List<Employee>();
    }
}
