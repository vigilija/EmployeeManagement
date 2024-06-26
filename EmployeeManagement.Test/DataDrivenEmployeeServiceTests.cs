﻿using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Test.Fixtures;
using EmployeeManagement.Test.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    public class DataDrivenEmployeeServiceTests : IClassFixture<EmployeeServiceFixture>
    {
        private readonly EmployeeServiceFixture _employeeServiceFixture;

        public DataDrivenEmployeeServiceTests(EmployeeServiceFixture employeeServiceFixture)
        {
            _employeeServiceFixture = employeeServiceFixture;
        }

        [Theory]
        [InlineData("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e")]
        [InlineData("37e03ca7-c730-4351-834c-b66f280cdb01")]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedSecondObligatoryCourse(Guid courseId)
        {
            //Arrange

            //Act
            var internalEmployee = _employeeServiceFixture
                .EmployeeService
                .CreateInternalEmployee("Brooklyn", "Cannon");

            //Assert
            Assert.Contains(internalEmployee.AttendedCourses,
                course => course.Id == courseId);
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
                .GiveRaiseAsync(internalEmployee, 200);

            //Assert
            Assert.False(internalEmployee.MinimumRaiseGiven);
        }

        public static IEnumerable<object[]> ExamplesTestDataForGiveRaise_WithProperty
        {
            get
            {
                return new List<object[]>
                {
                    new object[] {100, true},
                    new object[] {200, false},
                };
            }
        }

        public static IEnumerable<object[]> ExamplesTestDataForGiveRaise_WithMethod(
            int testDataInstancesToProvide)
        {
             var testData = new List<object[]>
                {
                    new object[] {100, true},
                    new object[] {200, false},
                };
            return testData.Take(testDataInstancesToProvide);
        }

        public static TheoryData<int,bool> StronglyTypedExampleTestDataForGiveRaise_WithMethod
        {
            get
            {
                return new TheoryData<int, bool>
                {
                    {100, true},
                    {200, false},
                };
            }
        }

        [Theory]
        //  [MemberData(nameof(ExamplesTestDataForGiveRaise_WithProperty))]
        //[MemberData(nameof(ExamplesTestDataForGiveRaise_WithMethod), 1, MemberType = typeof(DataDrivenEmployeeServiceTests))]
        //[ClassData(typeof(StronglyTypedEmployeeServiceTestData))]
        //[MemberData(nameof(StronglyTypedExampleTestDataForGiveRaise_WithMethod))]
        [ClassData(typeof(StronglyTypedEmployeeServiceTestData_FromFile))]

        public async Task GivenRaise_RaiseGiven_EmployeeMinimumRaiseGivenMatchesValue(
            int raiseGiven, bool expectedValueForMinimumRaiseGiven)
        {
            //Arrange
            var internalEmployee = new InternalEmployee(
                "Brooklyn", "Cannon", 5, 3000, false, 1);

            //Act
            await _employeeServiceFixture.EmployeeService
                .GiveRaiseAsync(internalEmployee, raiseGiven);

            //Assert
            Assert.Equal(expectedValueForMinimumRaiseGiven, internalEmployee.MinimumRaiseGiven);
        }
    }
}
