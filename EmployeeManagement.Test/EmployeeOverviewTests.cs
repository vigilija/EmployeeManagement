

using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManagement.Test
{
    public class EmployeeOverviewTests
    {
        [Fact]
        public async Task Index_GetAction_MustReturnViewResult()
        {
            //Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(m => m.FetchInternalEmployeesAsync())
                .ReturnsAsync(new List<InternalEmployee>()
                {
                    new InternalEmployee("Daiva", "Petrauskaite", 2, 3000, false, 2),
                    new InternalEmployee("Tomas", "Petrauskas", 3, 3400, true, 1),
                    new InternalEmployee("Petras", "Petraitis", 3, 4000, false, 3)
                });
            var employeeOverviewController = new EmployeeOverviewController(employeeServiceMock.Object, null);

            //Act
            var result = await employeeOverviewController.Index();

            //Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
