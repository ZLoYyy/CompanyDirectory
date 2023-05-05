using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyDirectory.Models
{
    internal class ReportEmployeeList
    {
        public string CompanyCaption { get; set; }
        public string EmployeeFIO { get; set; }
        public string DivisionCaption { get; set; }
        public int Experience { get; set; }
        public int Age { get; set; }
    }
}
