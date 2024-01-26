﻿using EmployeeManagement.Business;
using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse()
        {
            //Arrange
            var employeeManagementTestDataRepository = 
                new EmployeeManagementTestDataRepository();

            var employeeService = new EmployeeService(
                employeeManagementTestDataRepository,
                new EmployeeFactory());

            var obligatoryCourse = employeeManagementTestDataRepository
                .GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));

            //Act
            var internalEmployee = employeeService
                .CreateInternalEmployee("Brooklyn", "Cannon");

            //Assert
            Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourseWithPredicet()
        {
            //Arrange
            var employeeManagementTestDataRepository =
                new EmployeeManagementTestDataRepository();

            var employeeService = new EmployeeService(
                employeeManagementTestDataRepository,
                new EmployeeFactory());

            //Act
            var internalEmployee = employeeService
                .CreateInternalEmployee("Brooklyn", "Cannon");

            //Assert
            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedSecondObligatoryCourseWithPredicet()
        {
            //Arrange
            var employeeManagementTestDataRepository =
                new EmployeeManagementTestDataRepository();

            var employeeService = new EmployeeService(
                employeeManagementTestDataRepository,
                new EmployeeFactory());

            //Act
            var internalEmployee = employeeService
                .CreateInternalEmployee("Brooklyn", "Cannon");

            //Assert
            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourses()
        {
            //Arrange
            var employeeManagementTestDataRepository =
                new EmployeeManagementTestDataRepository();

            var employeeService = new EmployeeService(
                employeeManagementTestDataRepository,
                new EmployeeFactory());

            var obligatoryCourses = employeeManagementTestDataRepository
              .GetCourses(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                          Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            //Act
            var internalEmployee = employeeService
                .CreateInternalEmployee("Brooklyn", "Cannon");

            //Assert
            Assert.Equal(internalEmployee.AttendedCourses, obligatoryCourses);
        }

        [Fact]
        public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustNotBeNew()
        {
            //Arrange
            var employeeManagementTestDataRepository =
                new EmployeeManagementTestDataRepository();

            var employeeService = new EmployeeService(
                employeeManagementTestDataRepository,
                new EmployeeFactory());

            //Act
            var internalEmployee = employeeService
                .CreateInternalEmployee("Brooklyn", "Cannon");

            //Assert
            Assert.All(internalEmployee.AttendedCourses, course => Assert.False(course.IsNew));
 
        }


        [Fact]
        public async Task CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourses_Async()
        {
            //Arrange
            var employeeManagementTestDataRepository =
                new EmployeeManagementTestDataRepository();

            var employeeService = new EmployeeService(
                employeeManagementTestDataRepository,
                new EmployeeFactory());

            var obligatoryCourses = await employeeManagementTestDataRepository
              .GetCoursesAsync(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                          Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

            //Act
            var internalEmployee = await employeeService
                .CreateInternalEmployeeAsync("Brooklyn", "Cannon");

            //Assert
            Assert.Equal(internalEmployee.AttendedCourses, obligatoryCourses);
        }

        [Fact]
        public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
        {
            //Arrange
            var employeeService = new EmployeeService(
                new EmployeeManagementTestDataRepository(),
                new EmployeeFactory());

            var internalEmployee = new InternalEmployee("Tadas", "Petraitis", 5, 3000, false, 1);

            //Act & Assert
            await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
                async () =>
                await employeeService.GiveRaiseAsync(internalEmployee, 50));
        }

        [Fact]
        public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentMustBeTriggerd()
        {
            //Arrange
            var employeeService = new EmployeeService(
                new EmployeeManagementTestDataRepository(),
                new EmployeeFactory());
            var internalEmployee = new InternalEmployee(
                "Brooklyn", "Cannon", 5, 3000, false, 1);

            //Act & Assert
            Assert.Raises<EmployeeIsAbsentEventArgs>(
                handler => employeeService.EmployeeIsAbsent +=handler,
                handler => employeeService.EmployeeIsAbsent -=handler,
                () => employeeService.NotifyOfAbsence(internalEmployee));
        }

        [Fact]
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
