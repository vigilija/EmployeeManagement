using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Test.TestData
{
    internal class StronglyTypedEmployeeServiceTestData_FromFile : TheoryData<int, bool>
    {
        public StronglyTypedEmployeeServiceTestData_FromFile()
        {
            var testDataLines = File.ReadAllLines("TestData/EmployeeServiceTestData.csv");

            foreach (var line in testDataLines)
            {
                //split the string
                var splitString = line.Split(',');
                //parse
                if (int.TryParse(splitString[0], out int raise) && bool.TryParse(splitString[1], out bool minimumRaiseGiven))
                {
                    Add(raise, minimumRaiseGiven);
                }
            }
        }
    }
}
