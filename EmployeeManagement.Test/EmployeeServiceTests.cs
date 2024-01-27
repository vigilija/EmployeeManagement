using EmployeeManagement.Business;
using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;
using EmployeeManagement.Test.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    [Collection("EmployeeServiceCollection")]

    public class EmployeeServiceTests //: IClassFixture<EmployeeServiceFixture>
    {
        private readonly EmployeeServiceFixture _employeeServiceFixture;

        public EmployeeServiceTests(EmployeeServiceFixture employeeServiceFixture)
        {
            _employeeServiceFixture = employeeServiceFixture;
        }

        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse()
        {
            //Arrange

            var obligatoryCourse = _employeeServiceFixture
                .EmployeeManagementTestDataRepository
                .GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));

            //Act
            var internalEmployee = _employeeServiceFixture
                .EmployeeService
                .CreateInternalEmployee("Brooklyn", "Cannon");

            //Assert
            Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
        }

        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourseWithPredicet()
        {
            //Arrange

            //Act
            var internalEmployee = _employeeServiceFixture
                .EmployeeService
                .CreateInternalEmployee("Brooklyn", "Cannon");

            //Assert
            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        }

        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedSecondObligatoryCourseWithPredicet()
        {
            //Arrange

            //Act
            var internalEmployee = _employeeServiceFixture
                .EmployeeService
                .CreateInternalEmployee("Brooklyn", "Cannon");

            //Assert
            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        }

        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourses()
        {
            //Arrange
            var obligatoryCourses = _employeeServiceFixture
                .EmployeeManagementTestDataRepository
                .GetCourses(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                          Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            //Act
            var internalEmployee = _employeeServiceFixture
                .EmployeeService
                .CreateInternalEmployee("Brooklyn", "Cannon");

            //Assert
            Assert.Equal(internalEmployee.AttendedCourses, obligatoryCourses);
        }

        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustNotBeNew()
        {
            //Arrange

            //Act
            var internalEmployee = _employeeServiceFixture
                .EmployeeService
                .CreateInternalEmployee("Brooklyn", "Cannon");

            //Assert
            Assert.All(internalEmployee.AttendedCourses, course => Assert.False(course.IsNew));

        }


        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public async Task CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourses_Async()
        {
            //Arrange
            var employeeManagementTestDataRepository =
                new EmployeeManagementTestDataRepository();

            var obligatoryCourses = await _employeeServiceFixture
                .EmployeeManagementTestDataRepository
                .GetCoursesAsync(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                          Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            //Act
            var internalEmployee = await _employeeServiceFixture
                .EmployeeService
                .CreateInternalEmployeeAsync("Brooklyn", "Cannon");

            //Assert
            Assert.Equal(internalEmployee.AttendedCourses, obligatoryCourses);
        }

        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
        {
            //Arrange
            var internalEmployee = new InternalEmployee("Tadas", "Petraitis", 5, 3000, false, 1);

            //Act & Assert
            await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
                async () =>
                await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 50));
        }

        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentMustBeTriggerd()
        {
            //Arrange
            var internalEmployee = new InternalEmployee(
                "Brooklyn", "Cannon", 5, 3000, false, 1);

            //Act & Assert
            Assert.Raises<EmployeeIsAbsentEventArgs>(
                handler => _employeeServiceFixture
                .EmployeeService
                .EmployeeIsAbsent += handler,
                handler => _employeeServiceFixture
                .EmployeeService
                .EmployeeIsAbsent -= handler,
                () => _employeeServiceFixture
                .EmployeeService
                .NotifyOfAbsence(internalEmployee));
        }

        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_ReturnType")]
        public void CreateEmployee_IsExternalIsTrue_ReturnTypeMustBeExternalEmployee()
        {
            //Arrange
            var factory = new EmployeeFactory();

            //Act
            var employee = factory.CreateEmployee("Tadas", "Petrauskas", "Databricks", true);

            //Assert
            Assert.IsType<ExternalEmployee>(employee);
        }
    }
}
