using System;
using System.Collections.Generic;
using Xunit;
using System.IO.Abstractions.TestingHelpers;
using ExpensesCalculator.Models;
using ExpensesCalculator.Managers;

namespace ExpensesCalculatorTests
{
    public class ExpenseManagerTest
    {
        [Fact]
        public void TestExpenseLoadFromFile()
        {
            var mockFileSystem = new MockFileSystem();
            var mockInputFile = new MockFileData("JONAS 2013-02-20 333.21 SEK\nANTANAS 2017-05-10 300.00 USD");
            mockFileSystem.AddDirectory(@"C:\temp");
            mockFileSystem.AddFile(@"C:\temp\expenses.txt", mockInputFile);

            List<Expense> expected = new List<Expense>()
            {
                new Expense()
                { 
                    EmployeeName = "JONAS",
                    Date = new DateTime(2013, 02, 20),
                    Amount = 333.21M,
                    Currency = "SEK"
                },
                new Expense()
                {
                    EmployeeName = "ANTANAS",
                    Date = new DateTime(2017, 05, 10),
                    Amount = 300.00M,
                    Currency = "USD"
                }
            };

            ExpenseManager expenseManager = new ExpenseManager(mockFileSystem);
            List<Expense> actual = expenseManager.LoadExpensesFromFile(@"C:\temp\expenses.txt");

            Assert.Equal(expected, actual);
        }
    }
}
