using Costscape.Models;
using MVVMBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Costscape.ViewModels
{
    public class BudgetViewModel : BaseViewModel
    {
        private ObservableCollection<BudgetSection> _items;
        public ObservableCollection<BudgetSection> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                NotifyChanged();
            }
        }

        public BudgetViewModel()
        {
            Items = new ObservableCollection<BudgetSection>();
            var section1 = new BudgetSection();
            section1.Name = "Current Money";
            section1.Add(new BudgetItem() { Title = "Item 1", Value = 100000.1m, ValueConverted = 1.30m });
            section1.Add(new BudgetItem() { Title = "Item 2", Value = 100000.1m, ValueConverted = 1.30m });
            section1.Add(new BudgetItem() { Title = "Item 3", Value = 100000.1m, ValueConverted = 1.30m });
            section1.Add(new BudgetItem() { Title = "Item 3", Value = 100000.1m, ValueConverted = 1.30m });
            section1.Add(new BudgetItem() { Title = "Item 3", Value = 100000.1m, ValueConverted = 1.30m });
            section1.Add(new BudgetItem() { Title = "Item 3", Value = 100000.1m, ValueConverted = 1.30m });
            section1.Add(new BudgetItem() { Title = "Item 3", Value = 100000.1m, ValueConverted = 1.30m });
            section1.Add(new BudgetItem() { Title = "Item 3", Value = 100000.1m, ValueConverted = 1.30m });
            section1.Add(new BudgetItem() { Title = "Item 3", Value = 100000.1m, ValueConverted = 1.30m });
            section1.Add(new BudgetItem() { Title = "Item 3", Value = 100000.1m, ValueConverted = 1.30m });
            section1.Add(new BudgetItem() { Title = "Item 3", Value = 100000.1m, ValueConverted = 1.30m });
            section1.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            Items.Add(section1);
            var section2 = new BudgetSection();
            section2.Name = "Future Money";
            section2.Add(new BudgetItem() { Title = "Item 1", Value = 100000.1m, ValueConverted = 1.30m });
            section2.Add(new BudgetItem() { Title = "Item 2", Value = 100000.1m, ValueConverted = 1.30m });
            section2.Add(new BudgetItem() { Title = "Item 3", Value = 100000.1m, ValueConverted = 1.30m });
            section2.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section2.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section2.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section2.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section2.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section2.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section2.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section2.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            Items.Add(section2);
            var section3 = new BudgetSection();
            section3.Name = "Whatever Money";
            section3.Add(new BudgetItem() { Title = "Item 1", Value = 100000.1m, ValueConverted = 1.30m });
            section3.Add(new BudgetItem() { Title = "Item 2", Value = 100000.1m, ValueConverted = 1.30m });
            section3.Add(new BudgetItem() { Title = "Item 3", Value = 100000.1m, ValueConverted = 1.30m });
            section3.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section3.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section3.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section3.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section3.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section3.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section3.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            section3.Add(new BudgetItem() { Title = "Item 4", Value = 100000.1m, ValueConverted = 1.30m });
            Items.Add(section3);
        }
    }
}
