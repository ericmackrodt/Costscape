using Costscape.Common;
using Costscape.Models;
using MVVMBasic;
using MVVMBasic.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Costscape.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public event EventHandler<Budget> BudgetCreated;

        private IDataManager _dataManager;

        private ObservableCollection<Budget> _budgets;
        public ObservableCollection<Budget> Budgets
        {
            get { return _budgets; }
            set
            {
                _budgets = value;
                NotifyChanged();
            }
        }

        private Budget _newBudget;
        public Budget NewBudget
        {
            get
            {
                if (_newBudget == null)
                    _newBudget = new Budget();

                return _newBudget;
            }
            set
            {
                _newBudget = value;
                NotifyChanged();
            }
        }

        private ICommand _addNewBudgetCommand;
        public ICommand AddNewBudgetCommand
        {
            get { return _addNewBudgetCommand; }
        }

        public MainViewModel(IDataManager dataManager)
        {
            _dataManager = dataManager;

            _addNewBudgetCommand = new AsyncRelayCommand(AddNewBudget);
        }

        public override async Task LoadData(object arg)
        {
            await _dataManager.InitializeDatabase();
            var budgets = await _dataManager.GetAllBudgets();
            Budgets = new ObservableCollection<Budget>(budgets);
        }

        private async Task AddNewBudget(object arg)
        {
            var budget = NewBudget;
            NewBudget = null;
            await _dataManager.InsertObject(budget);
            Budgets.Insert(0, budget);

            if (BudgetCreated != null)
                BudgetCreated(this, budget);
        }
    }
}
