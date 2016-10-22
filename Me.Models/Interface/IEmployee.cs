using Me.Models.Tables;
using System;
using System.Collections.Generic;

namespace Me.Models.Interface
{
    public interface IEmployee
    {
        int EmployeeId { get; set; }
        string EmployeeNumber { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        DateTime DateRegularization { get; set; } 
        byte[] ImageEmployee { get; set; }
        Department Department { get; set; }
        Section Section { get; set; }
        Leave Leave { get; set; }
        EmployeeLeave EmployeeLeave { get; set; }
    }
}
