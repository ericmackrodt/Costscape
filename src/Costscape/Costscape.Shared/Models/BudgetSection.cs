using Costscape.Models;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Linq;
using Broadcaster;
using Costscape.Common.Enums;

namespace Costscape.Models
{
    public class BudgetSection : ObservableCollection<BudgetItem>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        public string Name
        {
	        get { return _name;}
            set
            {
                _name = value;
                NotifyChanged();
            }
        }

        private decimal _total;
        public decimal Total
        {
            get { return _total; }
            set
            {
                _total = value;
                NotifyChanged();
            }
        }

        private decimal _totalConverted;
        public decimal TotalConverted
        {
            get { return _totalConverted; }
            set
            {
                _totalConverted = value;
                NotifyChanged();
            }
        }

        private BudgetSectionType _sectionType;
        public BudgetSectionType SectionType
        {
            get { return _sectionType; }
            set
            {
                _sectionType = value;
                NotifyChanged();
            }
        }

        public void RecalculateTotals()
        {
            Total = this.Sum(o => o.Value);
            TotalConverted = this.Sum(o => o.ValueConverted);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            RecalculateTotals();
        }

        private void NotifyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
