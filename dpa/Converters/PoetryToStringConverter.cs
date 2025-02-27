using System;
using System.Globalization;
using Avalonia.Data.Converters;
using dpa.Library.Models;

namespace dpa.Converters;

public class PoetryToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture) =>
        value is Poetry poetry
            ? $"{poetry.Dynasty} · {poetry.Author}    {poetry.Snippet}"
            : null;

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture) =>
        throw new InvalidOperationException();
}