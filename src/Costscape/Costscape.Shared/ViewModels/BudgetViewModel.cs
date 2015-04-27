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
using Broadcaster;
using Costscape.Common.BroadcasterEvents;

namespace Costscape.ViewModels
{
    public class BudgetViewModel : BaseViewModel
    {
        public event EventHandler NewBudgetSectionCreated;
        public event EventHandler NewBudgetItemCreated;
        public event EventHandler BudgetItemEdited;

        private IDataManager _dataManager;
        private IBroadcaster _broadcaster;
        private Budget _currentBudget;
        private bool _isItemSectionChangeSet;

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

        private Dictionary<BudgetSection, BudgetItem> _itemSectionChangeList;
        public Dictionary<BudgetSection, BudgetItem> ItemSectionChangeList
        {
            get { return _itemSectionChangeList; }
            set
            {
                _itemSectionChangeList = value;
                NotifyChanged();
            }
        }

        public Budget CurrentBudget
        {
            get { return _currentBudget; }
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

        private BudgetItem _newBudgetItem;
        public BudgetItem NewBudgetItem
        {
            get
            {
                if (_newBudgetItem == null)
                    _newBudgetItem = new BudgetItem();

                return _newBudgetItem;
            }
            set
            {
                _newBudgetItem = value;
                NotifyChanged();
            }
        }

        private BudgetItem _selectedBudgetItem;
        public BudgetItem SelectedBudgetItem
        {
            get { return _selectedBudgetItem; }
            set
            {
                _selectedBudgetItem = value;
                NotifyChanged();
            }
        }

        private BudgetSection _selectedBudgetSection;
        public BudgetSection SelectedBudgetSection
        {
            get { return _selectedBudgetSection; }
            set
            {
                _selectedBudgetSection = value;
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
            set
            {
                _totalCollection = value;
                NotifyChanged();
            }
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

        private ICommand _updateItemCommand;
        public ICommand UpdateItemCommand
        {
            get { return _updateItemCommand; }
        }

        private ICommand _removeItemCommand;
        public ICommand RemoveItemCommand
        {
            get { return _removeItemCommand; }
        }

        private ICommand _setupItemSectionChangeCommand;
        public ICommand SetupItemSectionChangeCommand
        {
            get { return _setupItemSectionChangeCommand; }
        }

        private ICommand _changeItemSectionCommand;
        public ICommand ChangeItemSectionCommand
        {
            get { return _changeItemSectionCommand; }
        }

        public BudgetViewModel(IDataManager dataManager, IBroadcaster broadcaster)
        {
            _dataManager = dataManager;
            _broadcaster = broadcaster;

            _addNewItemCommand = new RelayCommandAsync<BudgetSection>(AddNewItem);
            _addNewSectionCommand = new RelayCommandAsync(AddNewSection);
            _valueUpdatedCommand = new RelayCommand(ValueUpdated);
            _updateItemCommand = new RelayCommandAsync(UpdateItem);
            _removeItemCommand = new RelayCommandAsync<BudgetItem>(RemoveItem);
            _setupItemSectionChangeCommand = new RelayCommand<BudgetItem>(SetupItemSectionChange);
            _changeItemSectionCommand = new RelayCommandAsync<KeyValuePair<BudgetSection, BudgetItem>>(ChangeItemSection);
        }

        public async override Task LoadData(object arg)
        {
            _currentBudget = arg as Budget;

            _currentBudget.LastUpdated = DateTime.Now;
            await _dataManager.UpdateObject(_currentBudget);
            _broadcaster.Event<BudgetUpdatedEvent>().Broadcast(_currentBudget);

            await _dataManager.GetBudgetSections(_currentBudget);

            if (_currentBudget.Sections != null)
                BudgetSections = new ObservableCollection<BudgetSection>(_currentBudget.Sections);

            TotalCollection = new ObservableCollection<TotalData>();

            if (_currentBudget.HasInitialBudget)
            {
                InitialBudget = new TotalData() { Title = "Initial Budget", Value = _currentBudget.InitialBudget, ValueConverted = (decimal)(_currentBudget.InitialBudget / 2.20m) };
                TotalCollection.Add(InitialBudget);

                MoneyLeft = new TotalData() { Title = "Money Left" };
                TotalCollection.Add(MoneyLeft);
            }
            
            Totals = new TotalData() { Title = "Total" };            
            TotalCollection.Add(Totals);
            Recalculate();
        }

        private void ValueUpdated(object obj)
        {
            NewBudgetItem.Value = Math.Round(NewBudgetItem.Value, 2);
        }

        private async Task AddNewSection(object obj)
        {
            if (BudgetSections == null)
                BudgetSections = new ObservableCollection<BudgetSection>();

            var section = NewBudgetSection;
            NewBudgetSection = null;
            section = await _dataManager.AddSectionToBudget(_currentBudget, section);
            
            BudgetSections.Add(section);

            if (NewBudgetSectionCreated != null)
                NewBudgetSectionCreated(this, new EventArgs());
        }

        private async Task AddNewItem(BudgetSection obj)
        {
            var item = NewBudgetItem;
            var section = SelectedBudgetSection;
            NewBudgetItem = null;
            SelectedBudgetSection = null;
            await _dataManager.AddItemToSection(section, item);
            Recalculate();

            if (NewBudgetItemCreated != null)
                NewBudgetItemCreated(this, new EventArgs());
        }

        private async Task UpdateItem(object arg)
        {
            var item = SelectedBudgetItem;
            SelectedBudgetItem = null;
            await _dataManager.UpdateObject(item);
            Recalculate();
            if (BudgetItemEdited != null)
                BudgetItemEdited(this, new EventArgs());
        }

        private async Task RemoveItem(BudgetItem arg)
        {
            await _dataManager.RemoveObject(arg);
            var section = BudgetSections.First(o => o.BudgetSectionID == arg.BudgetSectionID);
            section.Items.Remove(arg);
            Recalculate();
            if (BudgetItemEdited != null)
                BudgetItemEdited(this, new EventArgs());
        }

        private void SetupItemSectionChange(BudgetItem arg)
        {
            ItemSectionChangeList = BudgetSections.ToDictionary(k => k, v => arg);
            _isItemSectionChangeSet = true;
        }

        private async Task ChangeItemSection(KeyValuePair<BudgetSection, BudgetItem> arg)
        {
            if (!_isItemSectionChangeSet)
                throw new Exception("Item section change is not set!");

            var oldSection = BudgetSections.FirstOrDefault(o => o.BudgetSectionID == arg.Value.BudgetSectionID);
            await _dataManager.ChangeItemSection(arg.Value, arg.Key, oldSection);
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

            if (_currentBudget.HasInitialBudget)
                MoneyLeft.Value = InitialBudget.Value + total;

            Totals.Value = total;
        }
    }
}
