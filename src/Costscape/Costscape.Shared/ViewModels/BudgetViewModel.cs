using Costscape.Models;
using MVVMBasic;
using MVVMBasic.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Linq;

namespace Costscape.ViewModels
{
    public class BudgetViewModel : BaseViewModel
    {
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
        
        private ICommand _addNewItemCommand;
        public ICommand AddNewItemCommand
        {
            get { return _addNewItemCommand; }
        }

        public BudgetViewModel()
        {
            _addNewItemCommand = new RelayCommand(AddNewItem);
        }

        private void AddNewItem(object obj)
        {
            if (BudgetSections == null)
                BudgetSections = new ObservableCollection<BudgetSection>();

            if (!BudgetSections.Any())
                BudgetSections.Add(new BudgetSection() { Name = "Section" });

            BudgetSections[0].Add(new BudgetItem(OnValueChanged) { Title = "Item", Value = 0, ValueConverted = 0 });
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
        }
    }
}
