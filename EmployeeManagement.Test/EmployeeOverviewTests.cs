

using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManagement.Test
{
    public class EmployeeOverviewTests
    {
        private EmployeeOverviewController _employeeOverviewController;

        public EmployeeOverviewTests()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(m => m.FetchInternalEmployeesAsync())
                .ReturnsAsync(new List<InternalEmployee>()
                {
                    new InternalEmployee("Daiva", "Petrauskaite", 2, 3000, false, 2),
                    new InternalEmployee("Tomas", "Petrauskas", 3, 3400, true, 1),
                    new InternalEmployee("Petras", "Petraitis", 3, 4000, false, 3)
                });
             _employeeOverviewController = new EmployeeOverviewController(employeeServiceMock.Object, null);
        }

        [Fact]
        public async Task Index_GetAction_MustReturnViewResult()
        {
            //Arrange
           

            //Act
            var result = await _employeeOverviewController.Index();

            //Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Index_GetAction_MustReturnEmployeeOverviewViewModelAsViewModelType()
        {
            //Arrange
            //Act
            var result = await _employeeOverviewController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsType<EmployeeOverviewViewModel>(viewResult.Model);

            //Assert.IsType<EmployeeOverviewViewModel>(((ViewResult)result).Model);
        }
        [Fact]
        public async Task Index_GetAction_MustReturnNumberOfInputtedInternalEmployees()
        {
            //Arrange

            //Act
            var result = await _employeeOverviewController.Index();

            //Assert
            Assert.Equal(3,
                ((EmployeeOverviewViewModel)((ViewResult)result).Model)
                .InternalEmployees.Count);

        }
    }
}
