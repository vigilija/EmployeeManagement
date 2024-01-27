using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    internal class DataDrivenEmployeeServiceTests : IClassFixture<EmployeeServiceFixture>
    {
        private readonly EmployeeServiceFixture _employeeServiceFixture;

        public DataDrivenEmployeeServiceTests(EmployeeServiceFixture employeeServiceFixture)
        {
            _employeeServiceFixture = employeeServiceFixture;
        }
        [Fact]
        public async Task GiveRaise_MinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBeTrue()
        {
            //Arrange
            var internalEmployee = new InternalEmployee("Tadas", "Ivanauskas", 5, 3000, false, 1);

            //Act
            await _employeeServiceFixture
                .EmployeeService
                .GiveRaiseAsync(internalEmployee, 100);

            //Assert
            Assert.True(internalEmployee.MinimumRaiseGiven);
        }

        [Fact]
        public async Task GivenRaise_MoreThanMinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBeFalse()
        {
            //Arrange
            var internalEmployee = new InternalEmployee(
                "Brooklyn", "Cannon", 5, 3000, false, 1);

            //Act
            await _employeeServiceFixture.EmployeeService
                .GiveRaiseAsync (internalEmployee, 200);

            //Assert
            Assert.False(internalEmployee.MinimumRaiseGiven);
        }
    }
}
