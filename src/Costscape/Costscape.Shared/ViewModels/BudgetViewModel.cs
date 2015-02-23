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

namespace Costscape.ViewModels
{
    public class BudgetViewModel : BaseViewModel
    {
        public event EventHandler NewBudgetSectionCreated;

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

        public BudgetViewModel()
        {
            _addNewItemCommand = new RelayCommand<BudgetSection>(AddNewItem);
            _addNewSectionCommand = new RelayCommand(AddNewSection);

            InitialBudget = new TotalData() { Title = "Initial Budget", Value = 5000, ValueConverted = (decimal)(5000.0 / 2.20) };
            MoneyLeft = new TotalData() { Title = "Money Left" };
            Totals = new TotalData() { Title = "Total" };
            TotalCollection = new ObservableCollection<TotalData>();
            TotalCollection.Add(InitialBudget);
            TotalCollection.Add(MoneyLeft);
            TotalCollection.Add(Totals);
        }

        private void AddNewSection(object obj)
        {
            if (BudgetSections == null)
                BudgetSections = new ObservableCollection<BudgetSection>();

            NewBudgetSection.SectionType = BudgetSectionType.Credit;
            BudgetSections.Add(NewBudgetSection);

            if (NewBudgetSectionCreated != null)
                NewBudgetSectionCreated(this, new EventArgs());
        }

        private void AddNewItem(BudgetSection obj)
        {
            obj.Add(new BudgetItem(OnValueChanged) { Title = "Item", Value = 0, ValueConverted = 0 });
            Recalculate();
        }

        private void OnValueChanged(BudgetItem obj)
        {
            Recalculate();
        } 

        private void Recalculate()
        {
            foreach (var section in BudgetSections)
            {
                section.RecalculateTotals();
            }

            var total = BudgetSections.Sum(o => o.SectionType == BudgetSectionType.Debit ? o.Total * -1 : o.Total);
            var totalConverted = BudgetSections.Sum(o => o.SectionType == BudgetSectionType.Debit ? o.TotalConverted * -1 : o.TotalConverted);
            MoneyLeft.Value = InitialBudget.Value - total;
            MoneyLeft.ValueConverted = InitialBudget.ValueConverted - totalConverted;
            Totals.Value = total;
            Totals.ValueConverted = totalConverted;
        }
    }
}
