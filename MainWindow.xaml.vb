Class MainWindow

    Private lstBmps As New List(Of System.Drawing.Bitmap)
    Private Sub ShotCanvas()
        Dim rtBmp As RenderTargetBitmap
        Dim bmp As System.Drawing.Bitmap
        Dim bmpRaw As System.Drawing.Imaging.BitmapData

        rtBmp = New RenderTargetBitmap(480, 270, 96, 96, PixelFormats.Pbgra32)
        rtBmp.Render(cvsStage)

        bmp = New System.Drawing.Bitmap(rtBmp.PixelWidth, rtBmp.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
        bmpRaw = bmp.LockBits(New System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, bmp.PixelFormat)
        rtBmp.CopyPixels(Int32Rect.Empty, bmpRaw.Scan0, bmpRaw.Stride * bmpRaw.Height, bmpRaw.Stride)
        bmp.UnlockBits(bmpRaw)

        lstBmps.Add(bmp)
    End Sub
    Private Sub BtnCreate_Click(sender As Object, e As RoutedEventArgs) Handles btnCreate.Click
        lstBmps.Clear()

        Dim txRng As New TextRange(rtbCode.Document.ContentStart, rtbCode.Document.ContentEnd)
        If String.IsNullOrWhiteSpace(txRng.Text) Then
            MessageBox.Show("ERROR! NO RICH TEXT!!")
            Exit Sub
        End If

        Dim rtbScrollStart As Integer = 13500
        Dim rtbScrollEnd As Integer = 14310
        Dim movieLengthMillis As Integer = 4000
        Dim frameInterval As Integer = 4 '1/100 ssec
        Dim ining As Integer = CInt(Math.Ceiling(movieLengthMillis * 0.1 / frameInterval))

        Dim bw As New System.ComponentModel.BackgroundWorker()

        AddHandler bw.DoWork, Sub(ss, ee)
                                  Dim ii As Short
                                  For i As Short = 1 To ining
                                      ii = i
                                      cvsStage.Dispatcher.Invoke(
                                      Sub()

                                          'Dim hslc = HslColor.FromRgb(Color.FromArgb(255, &H19, &H74, &H9A))
                                          'hslc.H = CInt(hslc.H + ii) Mod 360

                                          'ucXPCLogo.Color = HslColor.ToRgb(hslc)

                                          rtbCode.ScrollToVerticalOffset(rtbScrollStart + (rtbScrollEnd - rtbScrollStart) * ii / ining)

                                          txbCounter.Text = ii
                                          txbCounter.InvalidateVisual()
                                      End Sub)

                                      Threading.Thread.Sleep(10)

                                      cvsStage.Dispatcher.Invoke(
                                      Sub()
                                          'For k = 1 To 5
                                          ShotCanvas()
                                          Me.Title = lstBmps.Count
                                          '        Threading.Thread.Sleep(40)
                                          'Next
                                      End Sub)


                                  Next
                              End Sub

        AddHandler bw.RunWorkerCompleted, Sub(ss, ee)
                                              AniGIF.SaveAnimatedGif("Z:\test.gif", lstBmps.ToArray, frameInterval, 0)
                                              MessageBox.Show("DONE!")
                                              Process.Start("Z:\")
                                          End Sub




        rtbCode.ScrollToVerticalOffset(rtbScrollStart)
        bw.RunWorkerAsync()

    End Sub

    Private Sub Slider_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))

        'Dim hslc = HslColor.FromRgb(Color.FromArgb(255, &H19, &H74, &H9A))
        'hslc.H = CInt(hslc.H + e.NewValue) Mod 360

        'ucXPCLogo.Color = HslColor.ToRgb(hslc)
    End Sub

    Private Sub btnGetRTBSize_Click(sender As Object, e As RoutedEventArgs) Handles btnGetRTBSize.Click
        sldValue.Maximum = rtbCode.ExtentHeight() - rtbCode.ViewportHeight
    End Sub

    Private Sub sldValue_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles sldValue.ValueChanged
        rtbCode.ScrollToVerticalOffset(e.NewValue)
    End Sub
End Class

Public Class HslColor
    Private _h As Single
    ''' <summary>
    ''' 色相 (Hue)
    ''' </summary>
    Public Property H() As Single
        Get
            Return Me._h
        End Get
        Set(value As Single)
            Me._h = value
        End Set
    End Property

    Private _s As Single
    ''' <summary>
    ''' 彩度 (Saturation)
    ''' </summary>
    Public ReadOnly Property S() As Single
        Get
            Return Me._s
        End Get
    End Property

    Private _l As Single
    ''' <summary>
    ''' 輝度 (Lightness)
    ''' </summary>
    Public ReadOnly Property L() As Single
        Get
            Return Me._l
        End Get
    End Property

    Private Sub New(ByVal hue As Single,
                    ByVal saturation As Single,
                    ByVal lightness As Single)
        If hue < 0.0F OrElse 360.0F <= hue Then
            Throw New ArgumentException(
                "hueは0以上360未満の値です。", "hue")
        End If
        If saturation < 0.0F OrElse 1.0F < saturation Then
            Throw New ArgumentException(
                "saturationは0以上1以下の値です。", "saturation")
        End If
        If lightness < 0.0F OrElse 1.0F < lightness Then
            Throw New ArgumentException(
                "lightnessは0以上1以下の値です。", "lightness")
        End If

        Me._h = hue
        Me._s = saturation
        Me._l = lightness
    End Sub

    ''' <summary>
    ''' 指定したColorからHslColorを作成する
    ''' </summary>
    ''' <param name="rgb">Color</param>
    ''' <returns>HslColor</returns>
    Public Shared Function FromRgb(ByVal rgb As Color) As HslColor
        Dim r As Single = CSng(rgb.R) / 255.0F
        Dim g As Single = CSng(rgb.G) / 255.0F
        Dim b As Single = CSng(rgb.B) / 255.0F

        Dim max As Single = Math.Max(r, Math.Max(g, b))
        Dim min As Single = Math.Min(r, Math.Min(g, b))

        Dim lightness As Single = (max + min) / 2.0F

        Dim hue As Single, saturation As Single
        If max = min Then
            'undefined
            hue = 0.0F
            saturation = 0.0F
        Else
            Dim c As Single = max - min

            If max = r Then
                hue = (g - b) / c
            ElseIf max = g Then
                hue = (b - r) / c + 2.0F
            Else
                hue = (r - g) / c + 4.0F
            End If
            hue *= 60.0F
            If hue < 0.0F Then
                hue += 360.0F
            End If

            'saturation = c / (1.0F - Math.Abs(2.0F * lightness - 1.0F))
            If lightness < 0.5F Then
                saturation = c / (max + min)
            Else
                saturation = c / (2.0F - max - min)
            End If
        End If

        Return New HslColor(hue, saturation, lightness)
    End Function

    ''' <summary>
    ''' 指定したHslColorからColorを作成する
    ''' </summary>
    ''' <param name="hsl">HslColor</param>
    ''' <returns>Color</returns>
    Public Shared Function ToRgb(ByVal hsl As HslColor) As Color
        Dim s As Single = hsl.S
        Dim l As Single = hsl.L

        Dim r1 As Single, g1 As Single, b1 As Single
        If s = 0 Then
            r1 = l
            g1 = l
            b1 = l
        Else
            Dim h As Single = hsl.H / 60.0F
            Dim i As Integer = CInt(Math.Floor(h))
            Dim f As Single = h - i
            'Dim c As Single = (1.0F - Math.Abs(2.0F * l - 1.0F)) * s
            Dim c As Single
            If l < 0.5F Then
                c = 2.0F * s * l
            Else
                c = 2.0F * s * (1.0F - l)
            End If
            Dim m As Single = l - c / 2.0F
            Dim p As Single = c + m
            'Dim x As Single = c * (1.0F - Math.Abs(h Mod 2.0F - 1.0F))
            Dim q As Single
            ' q = x + m
            If i Mod 2 = 0 Then
                q = l + c * (f - 0.5F)
            Else
                q = l - c * (f - 0.5F)
            End If

            Select Case i
                Case 0
                    r1 = p
                    g1 = q
                    b1 = m
                    Exit Select
                Case 1
                    r1 = q
                    g1 = p
                    b1 = m
                    Exit Select
                Case 2
                    r1 = m
                    g1 = p
                    b1 = q
                    Exit Select
                Case 3
                    r1 = m
                    g1 = q
                    b1 = p
                    Exit Select
                Case 4
                    r1 = q
                    g1 = m
                    b1 = p
                    Exit Select
                Case 5
                    r1 = p
                    g1 = m
                    b1 = q
                    Exit Select
                Case Else
                    Throw New ArgumentException(
                        "色相の値が不正です。", "hsl")
            End Select
        End If

        Return Color.FromArgb(255, CInt(Math.Round(r1 * 255.0F)),
                              CInt(Math.Round(g1 * 255.0F)),
                              CInt(Math.Round(b1 * 255.0F)))
    End Function
End Class