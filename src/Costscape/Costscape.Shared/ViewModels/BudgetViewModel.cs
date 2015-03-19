using Costscape.Models;
using MVVMBasic;
using MVVMBasic.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Linq;
using Costscape.Common.Enums;
using Costscape.Common;
using System.Threading.Tasks;

namespace Costscape.ViewModels
{
    public class BudgetViewModel : BaseViewModel
    {
        public event EventHandler NewBudgetSectionCreated;

        private IDataManager _dataManager;
        private Budget _currentBudget;

        private ObservableCollection<BudgetSection> _budgetSections;
        public ObservableCollection<BudgetSection> BudgetSections
        {
            get { return _budgetSections; }
            set
            {
                _budgetSections = value;
                NotifyChanged();
            }
        }

        public string BudgetName
        {
            get { return _currentBudget.Name; }
            set
            {
                _currentBudget.Name = value;
                NotifyChanged();
            }
        }


        private BudgetSection _newBudgetSection;
        public BudgetSection NewBudgetSection
        {
            get
            {
                if (_newBudgetSection == null)
                    _newBudgetSection = new BudgetSection();

                return _newBudgetSection;
            }
            set
            {
                _newBudgetSection = value;
                NotifyChanged();
            }
        }

        private TotalData _totals;
        public TotalData Totals
        {
            get { return _totals; }
            set
            {
                _totals = value;
                NotifyChanged();
            }
        }

        private TotalData _moneyLeft;
        public TotalData MoneyLeft
        {
            get { return _moneyLeft; }
            set
            {
                _moneyLeft = value;
                NotifyChanged();
            }
        }

        private ObservableCollection<TotalData> _totalCollection;
        public ObservableCollection<TotalData> TotalCollection
        {
            get { return _totalCollection; }
            set { _totalCollection = value; }
        }

        private TotalData _initialBudget;
        public TotalData InitialBudget
        {
            get { return _initialBudget; }
            set
            {
                _initialBudget = value;
                NotifyChanged();
            }
        }

        private ICommand _addNewItemCommand;
        public ICommand AddNewItemCommand
        {
            get { return _addNewItemCommand; }
        }

        private ICommand _addNewSectionCommand;
        public ICommand AddNewSectionCommand
        {
            get { return _addNewSectionCommand; }
        }

        private ICommand _valueUpdatedCommand;
        public ICommand ValueUpdatedCommand
        {
            get { return _valueUpdatedCommand; }
        }

        private ICommand _saveValueCommand;
        public ICommand SaveValueCommand
        {
            get { return _saveValueCommand; }
        }

        public BudgetViewModel(IDataManager dataManager)
        {
            _dataManager = dataManager;

            _addNewItemCommand = new AsyncRelayCommand<BudgetSection>(AddNewItem);
            _addNewSectionCommand = new AsyncRelayCommand(AddNewSection);
            _valueUpdatedCommand = new RelayCommand<BudgetItem>(ValueUpdated) { IsEnabled = true };
            _saveValueCommand = new AsyncRelayCommand<BudgetItem>(SaveValue);

            InitialBudget = new TotalData() { Title = "Initial Budget", Value = 5000, ValueConverted = (decimal)(5000.0 / 2.20) };
            MoneyLeft = new TotalData() { Title = "Money Left" };
            Totals = new TotalData() { Title = "Total" };
            TotalCollection = new ObservableCollection<TotalData>();
            TotalCollection.Add(InitialBudget);
            TotalCollection.Add(MoneyLeft);
            TotalCollection.Add(Totals);
        }

        public async override Task LoadData(object arg)
        {
            _currentBudget = arg as Budget;

            await _dataManager.GetBudgetSections(_currentBudget);

            if (_currentBudget.Sections != null)
                BudgetSections = new ObservableCollection<BudgetSection>(_currentBudget.Sections);

            Recalculate();
        }

        private void ValueUpdated(BudgetItem obj)
        {
            Recalculate();
        }

        private async Task SaveValue(BudgetItem arg)
        {
            if (arg.Updated)
            {
                await _dataManager.UpdateObject(arg);
                arg.Updated = false;
            }
        }

        private async Task AddNewSection(object obj)
        {
            if (BudgetSections == null)
                BudgetSections = new ObservableCollection<BudgetSection>();

            NewBudgetSection.SectionType = BudgetSectionType.Credit;
            var section = NewBudgetSection;
            NewBudgetSection = null;
            section = await _dataManager.AddSectionToBudget(_currentBudget, section);
            
            BudgetSections.Add(section);

            if (NewBudgetSectionCreated != null)
                NewBudgetSectionCreated(this, new EventArgs());
        }

        private async Task AddNewItem(BudgetSection obj)
        {
            await _dataManager.AddItemToSection(obj, new BudgetItem() { Title = "Item", Value = 0 });
            Recalculate();
        }

        private void OnValueChanged(BudgetItem obj)
        {
            Recalculate();
        } 

        private void Recalculate()
        {
            if (BudgetSections == null) return;

            foreach (var section in BudgetSections)
            {
                section.RecalculateTotals();
            }

            var total = BudgetSections.Sum(o => o.SectionType == BudgetSectionType.Debit ? o.Total * -1 : o.Total);
            MoneyLeft.Value = InitialBudget.Value - total;
            Totals.Value = total;
        }
    }
}
