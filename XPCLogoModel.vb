Imports System.ComponentModel

Public Class XPCLogoModel
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _Color As Color
    Private _RingColor As Color


    Public Sub New()
        Me.Color = Color.FromArgb(&HFF, &H19, &H74, &H9A)
        Me.RingColor = Color.FromArgb(255, 255, 255, 255)
    End Sub

    Public Property Color As Color
        Get
            Return _Color
        End Get
        Set(value As Color)
            _Color = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Color"))
        End Set
    End Property

    Public Property RingColor As Color
        Get
            Return _RingColor
        End Get
        Set(value As Color)
            _RingColor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RingColor"))
        End Set
    End Property

End Class