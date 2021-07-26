using ExpensesCalculator.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Text;

namespace ExpensesCalculator.Managers
{
    public class ExpenseManager
    {
        private readonly IFileSystem FileSystem;
        public ExpenseManager() : this(new FileSystem()) { }

        public ExpenseManager(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }
        public List<Expense> LoadExpensesFromFile(string filename)
        {
            List<Expense> expenses = new List<Expense>();
            string line; 

            StreamReader file = FileSystem.File.OpenText(filename);
            while ((line = file.ReadLine()) != null)
            {
                string[] row = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                expenses.Add(
                    new Expense()
                    {
                        EmployeeName = row[0],
                        Date = DateTime.Parse(row[1]),
                        Amount = decimal.Parse(row[2], CultureInfo.InvariantCulture),
                        Currency = row[3]
                    });
            }
            file.Close();

            return expenses;
        }
    }
}
