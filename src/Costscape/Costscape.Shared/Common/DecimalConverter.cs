﻿using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using System.Linq;

namespace Costscape.Common
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (string.IsNullOrWhiteSpace((string)value) || ((string)value).Any(o => !char.IsDigit(o)))
                return 0;

            return decimal.Parse((string)value);
        }
    }
}