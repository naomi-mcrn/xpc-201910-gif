Imports System.ComponentModel

Public Class XPCLogo
    'Implements INotifyPropertyChanged

    'Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    'Public Shared ReadOnly ColorProperty As DependencyProperty = DependencyProperty.Register("Color", GetType(Color), GetType(XPCLogo),
    '                                                                                         New FrameworkPropertyMetadata(Color.FromArgb(&HFF, &H19, &H74, &H9A), New PropertyChangedCallback(AddressOf OnColorChanged)))


    'Private _Color As Color = Color.FromArgb(&HFF, &H19, &H74, &H9A)

    Private myModel As XPCLogoModel

    Public Sub New()

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        myModel = New XPCLogoModel
        Me.DataContext = myModel
    End Sub

    Public Property Color As Color
        Get
            Return myModel.Color
        End Get
        Set(value As Color)
            myModel.Color = value
        End Set
    End Property

    Public Property RingColor As Color
        Get
            Return myModel.RingColor
        End Get
        Set(value As Color)
            myModel.RingColor = value
        End Set
    End Property

    'Private Shared Sub OnColorChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)

    'End Sub
End Class


