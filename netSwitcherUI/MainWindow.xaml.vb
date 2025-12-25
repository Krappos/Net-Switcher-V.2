Imports Wpf.Ui.Controls
Imports System.Collections.ObjectModel
Class MainWindow
    Inherits FluentWindow

    Public Property Items As New ObservableCollection(Of Filler)()
    Public Sub New()
        InitializeComponent()

        Items.Add(New Filler With {.Name = " Eth", .iconPath = "/WIFI.png"})
        Items.Add(New Filler With {.Name = " Eth", .iconPath = "/ETH.png"})

        Items.Add(New Filler With {.Name = " Eth"})
        Items.Add(New Filler With {.Name = " Eth"})
        Items.Add(New Filler With {.Name = " Eth"})


        Me.DataContext = Me
    End Sub


End Class
