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
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using MVVMBasic;

namespace Costscape.Models
{
    [Table("BudgetSections")]
    public class BudgetSection : BaseViewModel
    {
        private int _budgetSectionID;
        [PrimaryKey, AutoIncrement]
        public int BudgetSectionID
        {
            get { return _budgetSectionID; }
            set
            {
                _budgetSectionID = value;
                NotifyChanged();
            }
        }

        private string _name;
        [NotNull]
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
        [Ignore]
        public decimal Total
        {
            get { return _total; }
            set
            {
                _total = value;
                NotifyChanged();
            }
        }

        private BudgetSectionType _sectionType;
        [NotNull, Default(value: BudgetSectionType.Debit)]
        public BudgetSectionType SectionType
        {
            get { return _sectionType; }
            set
            {
                _sectionType = value;
                NotifyChanged();
            }
        }

        [ManyToOne]
        public Budget Budget { get; set; }
        
        [ForeignKey(typeof(Budget))]
        public int BudgetID { get; set; }

        private ObservableCollection<BudgetItem> _items;
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ObservableCollection<BudgetItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                NotifyChanged();
            }
        }

        [Ignore]
        public override bool IsBusy
        {
            get
            {
                return base.IsBusy;
            }
            set
            {
                base.IsBusy = value;
            }
        }

        [Ignore]
        public override bool IsDataLoaded
        {
            get
            {
                return base.IsDataLoaded;
            }
            set
            {
                base.IsDataLoaded = value;
            }
        }

        public BudgetSection()
        {
        }

        public void Add(BudgetItem item)
        {
            if (Items == null)
                Items = new ObservableCollection<BudgetItem>();

            Items.Add(item);
        }

        public void RecalculateTotals()
        {
            Total = Items.Sum(o => o.Value);
        }
    }
}
