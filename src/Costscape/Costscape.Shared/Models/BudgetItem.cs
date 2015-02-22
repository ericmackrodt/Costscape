using Broadcaster;
using MVVMBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Costscape.Models
{
    public class BudgetItem : BaseViewModel
    {
        private readonly Action<BudgetItem> _valueChanged;

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyChanged();
            }
        }

        private decimal _value;
        public decimal Value
        {
            get { return _value; }
            set
            {
                _value = value;
                NotifyChanged();
                _valueChanged(this);
            }
        }

        private decimal _valueConverted;
        public decimal ValueConverted
        {
            get { return _valueConverted; }
            set
            {
                _valueConverted = value;
                NotifyChanged();
                _valueChanged(this);
            }
        }

        public BudgetItem(Action<BudgetItem> valueChangedCallback)
        {
            _valueChanged = valueChangedCallback;
        }
    }
}
