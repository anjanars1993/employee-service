using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeDataAccess;

namespace EmployeeService
{
    public class EmployeeSecurity
    {
        public static bool Login(string UserName, string Password)
        {
            using (sampleEntities entities = new sampleEntities())
            {
                return entities.Users.Any(u => u.Username.Equals(UserName, StringComparison.OrdinalIgnoreCase) &&
                    (u.Password.Equals(Password)));
            }
        }
    }
}