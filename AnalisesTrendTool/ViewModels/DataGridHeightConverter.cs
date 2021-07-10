using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace AnalisesTrendTool.ViewModels
{
    public class DataGridHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                if (double.TryParse(parameter.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture,  out double percent))
                {
                    return (double)value * percent;
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
