Imports System.Globalization

Public Class RatioConverter
    Implements IValueConverter

    Public Property Ratio As Double = 1.0

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        Return CDbl(value) * Ratio
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return CDbl(value) / Ratio
    End Function
End Class
