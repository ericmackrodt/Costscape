using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Text;
using SQLiteNetExtensionsAsync.Extensions;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.Threading.Tasks;
using Costscape.Models;
using System.Collections.ObjectModel;

namespace Costscape.Common
{
    public class DataManager : IDataManager
    {
        private SQLiteAsyncConnection _connection;

        public DataManager()
        {
            var connectionWithLock = new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString("database.sqlite", true));
            _connection = new SQLiteAsyncConnection(() => connectionWithLock);
        }

        public async Task InitializeDatabase()
        {
            await _connection.CreateTableAsync<Budget>();
            await _connection.CreateTableAsync<BudgetSection>();
            await _connection.CreateTableAsync<BudgetItem>();
        }

        public async Task<Budget> CreateNewBudget(string name)
        {
            var budget = new Budget();
            budget.Name = name;
            await _connection.InsertAsync(budget);
            return budget;
        }

        public async Task GetBudgetSections(Budget budget)
        {
            await _connection.GetChildrenAsync(budget, true);
        }

        public async Task<IEnumerable<Budget>> GetAllBudgets()
        {
            return await _connection.Table<Budget>().ToListAsync();
        }

        public async Task<BudgetSection> AddSectionToBudget(Budget budget, BudgetSection section)
        {
            section.Budget = budget;
            section.BudgetID = budget.BudgetID;

            await _connection.InsertAsync(section);

            if (budget.Sections == null)
                budget.Sections = new List<BudgetSection>();

            budget.Sections.Add(section);
            return section;
        }

        public async Task<BudgetItem> AddItemToSection(BudgetSection section, BudgetItem item)
        {
            item.Section = section;
            item.BudgetSectionID = section.BudgetSectionID;

            await _connection.InsertAsync(item);

            if (section.Items == null)
                section.Items = new ObservableCollection<BudgetItem>();

            section.Items.Insert(0, item);
            
            return item;
        }

        public async Task ChangeItemSection(BudgetItem item, BudgetSection newSection, BudgetSection oldSection)
        {
            item.Section = newSection;
            item.BudgetSectionID = newSection.BudgetSectionID;

            await _connection.UpdateAsync(item);

            if (oldSection.Items.Contains(item))
                oldSection.Items.Remove(item);

            if (newSection.Items == null)
                newSection.Items = new ObservableCollection<BudgetItem>();

            newSection.Items.Add(item);
        }

        public async Task UpdateObject(object obj)
        {
            await _connection.UpdateAsync(obj);
        }

        public async Task InsertObject(object obj)
        {
            await _connection.InsertAsync(obj);
        }

        public async Task RemoveObject(object obj)
        {
            await _connection.DeleteAsync(obj);
        }        
    }
}
