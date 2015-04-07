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
using System.Linq;
using Broadcaster;
using Costscape.Common.BroadcasterEvents;

namespace Costscape.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public event EventHandler<Budget> BudgetCreated;

        private IDataManager _dataManager;
        private IBroadcaster _broadcaster;

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

        private ICommand _initialBudgetUpdateCommand;
        public ICommand InitialBudgetUpdateCommand
        {
            get { return _initialBudgetUpdateCommand; }
        }

        public MainViewModel(IDataManager dataManager, IBroadcaster broadcaster)
        {
            _dataManager = dataManager;
            _broadcaster = broadcaster;

            _addNewBudgetCommand = new RelayCommandAsync(AddNewBudget);
            _initialBudgetUpdateCommand = new RelayCommand(UpdateInitialBudgetValue) { IsEnabled = true };

            _broadcaster.Event<BudgetUpdatedEvent>().Subscribe(OnBudgetUpdated);
        }

        public override async Task LoadData(object arg)
        {
            await _dataManager.InitializeDatabase();
            var budgets = await _dataManager.GetAllBudgets();
            Budgets = new ObservableCollection<Budget>(budgets.OrderByDescending(o => o.LastUpdated));
        }

        private async Task AddNewBudget(object arg)
        {
            var budget = NewBudget;
            NewBudget = null;
            budget.LastUpdated = DateTime.Now;
            await _dataManager.InsertObject(budget);
            Budgets.Insert(0, budget);

            if (BudgetCreated != null)
                BudgetCreated(this, budget);
        }

        private void UpdateInitialBudgetValue(object obj)
        {
            if (NewBudget != null)
                NewBudget.InitialBudget = Math.Round(NewBudget.InitialBudget, 2);
        }

        private void OnBudgetUpdated(Budget obj)
        {
            Budgets = new ObservableCollection<Budget>(Budgets.OrderByDescending(o => o.LastUpdated));   
        }

        ~MainViewModel()
        {
            System.Diagnostics.Debug.WriteLine("What??");
        }
    }
}
