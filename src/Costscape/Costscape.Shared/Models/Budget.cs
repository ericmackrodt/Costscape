using MVVMBasic;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Costscape.Models
{
    [Table("Budgets")]
    public class Budget : BaseViewModel
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

        [OneToMany(CascadeOperations=CascadeOperation.All)]
        public List<BudgetSection> Sections { get; set; }
    }
}
