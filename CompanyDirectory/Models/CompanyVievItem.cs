using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyDirectory.Models
{
    internal class CompanyVievItem
    {
        public string Caption { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateCreate { get; set; }
        /// <summary>
        /// Юридический адрес
        /// </summary>
        public string LegalAddress { get; set; }
        /// <summary>
        /// Список подразделений
        /// </summary>
        public List<DivisionViewItem> Divisions { get; set; } = new List<DivisionViewItem>();
    }
}
