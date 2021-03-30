using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPService
{
    public class Employee
    {
        //Constructor of the class Employee
        public Employee(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        // Atribbutes getters and setters
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
