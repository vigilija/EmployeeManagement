using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.DataAccess.Services;
using EmployeeManagement.Services.Test;
using Moq;

namespace EmployeeManagement.Test
{
    public class moqTests
    {
        [Fact]
        public void FetchInternalEmployye_EmployeeFetched_SuggestedBonusMustBeCalculated()
        {
            //Arrange
            var employeeManagementTestRepository =
                new EmployeeManagementTestDataRepository();
            //var employeeFactory = new EmployeeFactory();

            var employeeMoqFactory = new Mock<EmployeeFactory>();
            employeeMoqFactory.Setup(m =>
            m.CreateEmployee(
                It.IsAny<string>(),
                It.Is<string>(v => v.Contains('a')),
                null,
                false))
                .Returns(new InternalEmployee("Tadas", "Petrauskas", 5, 2500, false, 1));

            var employService = new EmployeeService(
                employeeManagementTestRepository,
                employeeMoqFactory.Object);

            decimal suggestedBonus = 1000;

            //Act
            //  var employee = employService.FetchInternalEmployee(
            //      Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"));

            var employee = employService.CreateInternalEmployee("Tadas", "Petrauskas");

            //Assert
            Assert.Equal(suggestedBonus, employee.SuggestedBonus);
        }

        [Fact]
        public void FetchInternalEmployye_EmployeeFetched_SuggestedBonusMustBeCalculatedI()
        {
            //Arrange
            var employeeManagementTestDataRepositoryMock =
                new Mock<IEmployeeManagementRepository>();

            employeeManagementTestDataRepositoryMock
                .Setup(m => m.GetInternalEmployee(It.IsAny<Guid>()))
                .Returns(new InternalEmployee("Tadas", "Tadauskas", 2, 2500, false, 2)
                {
                    AttendedCourses = new List<Course>() {
                        new Course("A course"), new Course("Another course")
                    }
                });

            var employeeMoqFactory = new Mock<EmployeeFactory>();

            var employService = new EmployeeService(
                employeeManagementTestDataRepositoryMock.Object,
                employeeMoqFactory.Object);

            //Act
            var employee = employService.FetchInternalEmployee(
               Guid.Empty);

            //Assert
            Assert.Equal(400, employee.SuggestedBonus);
        }
    }
}
