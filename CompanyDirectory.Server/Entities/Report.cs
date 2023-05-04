using CompanyDirectory.Server.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyDirectory.Server.Entities
{
    public class Report : NameEntity
    {
        public string Tag { get; set; } = null!;
    }
}
