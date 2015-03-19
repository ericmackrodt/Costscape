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
            await _connection.InsertAsync(section);

            if (budget.Sections == null)
                budget.Sections = new List<BudgetSection>();

            budget.Sections.Add(section);
            await _connection.UpdateWithChildrenAsync(budget);
            return section;
        }

        public async Task<BudgetItem> AddItemToSection(BudgetSection section, BudgetItem item)
        {
            await _connection.InsertAsync(item);

            if (section.Items == null)
                section.Items = new ObservableCollection<BudgetItem>();

            section.Items.Add(item);
            await _connection.UpdateWithChildrenAsync(section);
            return item;
        }

        public async Task UpdateObject(object obj)
        {
            await _connection.UpdateAsync(obj);
        }

        public async Task InsertObject(object obj)
        {
            await _connection.InsertAsync(obj);
        }
    }
}
