using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StockFlow.Desktop.Converters;

public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException("NullToVisibilityConverter only supports one-way binding.");
}
