using EmployeeManagement.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    public class CourseTests
    {
        [Fact]
        public void CourseConstructor_ConstructCourse_IsNewMustBeTrue()
        {
            //nothing to Arrange

            //Act
            var course = new Course("Disaster Manegment 101");

            //Assert
            Assert.True(course.IsNew);
        }
    }
}
