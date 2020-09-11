using System;
using System.Globalization;
using System.Windows.Data;

namespace TreeViewControlWpfApp.TreeViewControl {
    public class ObjectToTypeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value?.GetType();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            // I don't think you'll need this
            throw new Exception("Can't convert back");
    }
}