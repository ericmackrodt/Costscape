using Costscape.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Costscape.Common
{
    public interface IDataManager
    {
        Task InitializeDatabase();
        Task<Budget> CreateNewBudget(string name);
        Task<IEnumerable<Budget>> GetAllBudgets();
        Task GetBudgetSections(Budget budget);
        Task<BudgetSection> AddSectionToBudget(Budget budget, BudgetSection section);
        Task<BudgetItem> AddItemToSection(BudgetSection section, BudgetItem item);
        Task UpdateObject(object obj);
        Task InsertObject(object obj);
    }
}
