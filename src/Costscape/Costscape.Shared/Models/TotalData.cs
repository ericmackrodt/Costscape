using Costscape.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Costscape.Models
{
    public class TotalData : ObservableObject
    {
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
            }
        }
    }
}
