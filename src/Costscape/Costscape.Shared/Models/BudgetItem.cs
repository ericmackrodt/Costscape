using System;
using System.Collections.Generic;
using System.Text;

namespace Costscape.Models
{
    public class BudgetItem
    {
        public string Title { get; set; }
        public decimal Value { get; set; }
        public decimal ValueConverted { get; set; }
    }
}
