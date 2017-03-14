using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalarySearchLibrary;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void GetEmployeesTestMethod()
        {
            List<Employee> employeesList = SalarySearch.GetEmployees("Kevin", "Adams", "", "", "", "", "");
        }

        [TestMethod]
        public void GetCabinetsTestMethod()
        {
            List<string> cabinetList = SalarySearch.GetCabinets();
        }

        [TestMethod]
        public void GetDepartmentsTestMethod()
        {
            List<string> departmentList = SalarySearch.GetDepartments();
        }
    }
}
