Imports System.Collections.ObjectModel
Imports Wpf.Ui.Controls
Imports netSwitcherBackend


Class MainWindow
    Inherits FluentWindow


    Public Property Items As New ObservableCollection(Of NetworkInfo)()

    Dim loader As New NewtorkLoader()
    Dim informacia As New NetworkInfo()

    Public Sub New()
        InitializeComponent()


        'nacitanie dostupnych sieti do userControlera
        For Each sset In loader.getAvaliableNets()
            Items.Add(sset)
        Next

        Me.DataContext = Me
    End Sub

    Private Sub CheckBox1_Unchecked(sender As Object, e As RoutedEventArgs) Handles CheckBox1.Unchecked, CheckBox1.Checked
        If CheckBox1.IsChecked = True Then
            Tb_script.TextDecorations = TextDecorations.Strikethrough
            Tb_script.IsEnabled = True

        Else
            Tb_script.TextDecorations = Nothing
            Tb_script.IsEnabled = False
        End If

    End Sub


End Class
