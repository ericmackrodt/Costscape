using Costscape.Common.Enums;
using MVVMBasic;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Costscape.Models
{
    [Table("Budgets")]
    public class Budget : ObservableModel
    {
        private int _budgetID;
        [PrimaryKey, AutoIncrement]
        public int BudgetID
        {
            get { return _budgetID; }
            set
            {
                _budgetID = value;
                NotifyChanged();
            }
        }

        private string _name;
        [NotNull]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyChanged();
            }
        }

        private DateTime _lastUpdated;
        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
            set
            {
                _lastUpdated = value;
                NotifyChanged();
            }
        }

        private bool _hasInitialBudget;
        [NotNull, Default(true, false)]
        public bool HasInitialBudget
        {
            get { return _hasInitialBudget; }
            set
            {
                _hasInitialBudget = value;
                NotifyChanged();
            }
        }

        private decimal _initialBudget;
        public decimal InitialBudget
        {
            get { return _initialBudget; }
            set
            {
                _initialBudget = value;
                NotifyChanged();
            }
        }

        private BudgetBaseCurrency _baseCurrency;
        [NotNull, Default(value: BudgetBaseCurrency.USD)]
        public BudgetBaseCurrency BaseCurrency
        {
            get { return _baseCurrency; }
            set
            {
                _baseCurrency = value;
                NotifyChanged();
            }
        }

        [OneToMany(CascadeOperations=CascadeOperation.All)]
        public List<BudgetSection> Sections { get; set; }
    }
}
