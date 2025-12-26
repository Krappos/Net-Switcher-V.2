Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Net.Mime.MediaTypeNames
Imports netSwitcherBackend
Imports Wpf.Ui.Controls


Class MainWindow
    Inherits FluentWindow

    Public Property Items As New ObservableCollection(Of NetworkInfo)()

    Dim loader As New NewtorkLoader()
    Dim informacia As New NetworkInfo()
    Dim file As New ProxyWorker()
    Dim activeState As Boolean
    Dim proxy As New ProxyController()

    Public Sub New()
        InitializeComponent()
        firstBoot()

        Me.DataContext = Me
    End Sub

    Private Sub CheckBoxController(sender As Object, e As RoutedEventArgs) Handles CheckBox1.Unchecked, CheckBox1.Checked

        If CheckBox1.IsChecked = True Then
            Tb_script.IsEnabled = True
            Tb_script.TextDecorations = Nothing
            proxy.EnableProxyScript()
        Else
            Tb_script.IsEnabled = False
            Tb_script.TextDecorations = TextDecorations.Strikethrough
            proxy.DisableProxyScript()
        End If

    End Sub

    Private Sub Tb_script_KeyDown(sender As Object, e As KeyEventArgs) Handles Tb_script.KeyDown
        If e.Key = Key.Enter Then
            Tb_scriptController()
        End If
    End Sub


#Region "private methods"

    Private Sub firstBoot()
        file.CreateFiles()
        ProxyStatus()
        loadNetworkInfo()
        CheckBox1.IsChecked = True

    End Sub
    Private Sub ProxyStatus()
        If CheckBox1.IsChecked Then
            proxy.EnableProxyScript()

        Else

            proxy.DisableProxyScript()
        End If
    End Sub
    Private Sub loadNetworkInfo()

        'nacitanie dostupnych sieti do userControlera
        For Each sset In loader.getAvaliableNets()
            Items.Add(sset)
        Next

    End Sub

    Private Sub Tb_scriptController()
        file.WriteScript(Tb_script.Text)
        MsgBox("vasa proxy je uspesne zmenená")
    End Sub

#End Region


End Class

'nacitavanie sieti do zoznamu funguje <3
'upravit proxy font a textbox <3
'pridanie kniznice na zapis scriptu do systemu<3
'pridat logiku checkboxu a textboxu na script <


'pridat bluetooth  icon 

'pridanie logiky na aktivnu siet
