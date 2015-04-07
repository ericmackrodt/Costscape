using Broadcaster;
using MVVMBasic;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Costscape.Models
{
    [Table("BudgetItems")]
    public class BudgetItem : ObservableModel
    {
        [Ignore]
        public bool Updated { get; set; }

        private int _budgetItemID;
        [PrimaryKey, AutoIncrement]
        public int BudgetItemID
        {
            get { return _budgetItemID; }
            set
            {
                _budgetItemID = value;
                NotifyChanged();
            }
        }

        private string _title;
        [NotNull]
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    Updated = true;
                    NotifyChanged();
                }
            }
        }

        private decimal _value;
        [NotNull]
        public decimal Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    Updated = true;
                    NotifyChanged();
                }
            }
        }

        [ForeignKey(typeof(BudgetSection))]
        public int BudgetSectionID { get; set; }

        [ManyToOne]
        public BudgetSection Section { get; set; }
    }
}
