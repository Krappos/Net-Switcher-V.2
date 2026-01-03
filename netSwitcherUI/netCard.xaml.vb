' Správne importy
Imports System.Windows.Controls
Imports Wpf.Ui.Controls
Imports netSwitcherBackend
Imports System.Threading

' netCard musí dediť z UserControl, nie z okna!
Public Class netCard
    Inherits UserControl

    Dim controller As New NewtorkLoader()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Async Sub Btn1_Click(sender As Object, e As RoutedEventArgs) Handles Btn1.Click
        controller.ToggleAdapterStatus(tx1.Text)
        Await Task.Delay(10000)

        Dim otovreneOkno = TryCast(Application.Current.MainWindow, MainWindow)
        If otovreneOkno IsNot Nothing Then
            otovreneOkno.refresh()
        End If

    End Sub
End Class