using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using System.Linq;
using System.Globalization;

namespace Costscape.Common.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var format = "{0:0" + CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator + "00}"; 

            return string.Format(format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            value = ((string)value).Replace(",", ".");

            if (string.IsNullOrWhiteSpace((string)value) || ((string)value).Any(o => !char.IsDigit(o) && o != '.') ||
                !((string)value).Any(o => char.IsDigit(o)) || ((string)value).Count(o => o == '.') > 1)
                return 0.00m;

            return Math.Round(decimal.Parse((string)value), 2);
        }
    }
}
