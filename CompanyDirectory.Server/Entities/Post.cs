using CompanyDirectory.Server.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyDirectory.Server.Entities
{
    public class Post : NameEntity
    {
        public List<Division> Divisions { get; } = new List<Division>();
    }
}
