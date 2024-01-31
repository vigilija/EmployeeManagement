

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
        private readonly InternalEmployee _firsEmployee;

        public EmployeeOverviewTests()
        {
            _firsEmployee = new InternalEmployee("Daiva", "Petrauskaite", 2, 3000, false, 2)
            {
                Id = Guid.Parse("bfdd0acd-d314-48d5-a7ad-0e94dfdd9155"),
                SuggestedBonus = 400
            };

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(m => m.FetchInternalEmployeesAsync())
                .ReturnsAsync(new List<InternalEmployee>()
                {
                    _firsEmployee,
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
        [Fact]
        public async Task Index_GetAction_ReturnsViewResultWithInternalEmployees()
        {
            //Arrange

            //Act
            var result = await _employeeOverviewController.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<EmployeeOverviewViewModel>(viewResult.Model);
            Assert.Equal(3, viewModel.InternalEmployees.Count);

            var firstEmployee = viewModel.InternalEmployees[0];
            Assert.Equal(_firsEmployee.Id, firstEmployee.Id);
            Assert.Equal(_firsEmployee.FirstName, firstEmployee.FirstName);
            Assert.Equal(_firsEmployee.LastName, firstEmployee.LastName);
            Assert.Equal(_firsEmployee.Salary, firstEmployee.Salary);
            Assert.Equal(_firsEmployee.SuggestedBonus, firstEmployee.SuggestedBonus);
            Assert.Equal(_firsEmployee.YearsInService, firstEmployee.YearsInService);
        }
    }
}
