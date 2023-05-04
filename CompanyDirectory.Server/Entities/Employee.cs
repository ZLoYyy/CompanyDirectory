using CompanyDirectory.Server.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CompanyDirectory.Server.Entities
{
    public class Employee : BaseEntity
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// Отчество
        /// </summary>
        public string SecondName { get; set; } = string.Empty;
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateofBorn { get; set; }
        /// <summary>
        /// Дата трудоустройства
        /// </summary>
        public DateTime DateWorkBegin { get; set; }
        /// <summary>
        /// Зарплата
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        /// <summary>
        /// Текущая компания
        /// </summary>
        public Company CurrentCompany { get; set; } = null!;
        /// <summary>
        /// Текущее подразделение
        /// </summary>
        public Division CurrentDivision { get; set; } = null!;
        /// <summary>
        /// Текущая должность
        /// </summary>
        public Post CurrentPost { get; set; } = null!;
        /// <summary>
        /// Является руководителем подразделения
        /// </summary>
        public bool IsDirector { get; set; } = false;

        //public List<DivisionEmploye> DivisionEmployes { get; } = new List<DivisionEmploye>();
    }
}
