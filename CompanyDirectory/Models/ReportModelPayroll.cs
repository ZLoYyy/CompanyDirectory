using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyDirectory.Models
{
    internal class ReportModelPayroll
    {
        public string CompanyCaption { get; set; }
        public string EmployeeFIO { get; set; }
        public string DivisionCaption { get; set; }
        public decimal Salary { get; set; }
    }
}
