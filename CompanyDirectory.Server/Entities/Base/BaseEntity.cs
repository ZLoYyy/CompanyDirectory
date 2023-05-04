using CompanyDirectory.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyDirectory.Server.Entities.Base
{
    public abstract class BaseEntity : IEntity
    {
        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }
    }
}
