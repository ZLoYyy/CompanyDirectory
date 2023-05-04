using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyDirectory.Server.Entities.Base
{
    public class NameEntity : BaseEntity
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Caption { get; set; } = null!;
    }
}
