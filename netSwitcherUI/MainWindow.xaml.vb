Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.IO
Imports System.Net.Mime.MediaTypeNames
Imports netSwitcherBackend
Imports Wpf.Ui.Controls


Class MainWindow
    Inherits FluentWindow

    Public Property Items As New ObservableCollection(Of NetworkInfo)()

    Dim loader As New NewtorkLoader()
    Dim inform As New NetworkInfo()
    Dim file As New ProxyWorker()
    Dim activeState As Boolean
    Dim proxy As New proxyController()

    Public Sub New()
        InitializeComponent()
        firstBoot()

        Me.DataContext = Me
    End Sub
    Public Sub refresh()
        Items.Clear()
        loadNetworkInfo()
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
        StartupInit()
        loadNetworkInfo()
        Tb_script.Text = file.ReadScript()

    End Sub

    Private Sub loadNetworkInfo()

        'nacitanie dostupnych sieti do userControlera
        For Each sset In loader.GetNetWorkCards()
            Items.Add(sset)
        Next

    End Sub

    Private Sub Tb_scriptController()
        If String.IsNullOrWhiteSpace(Tb_script.Text) Then
            MsgBox("prosím zapíšte svoju adresu")
        Else

            MsgBox("vasa proxy je uspesne zmenená")
            file.WriteScript(Tb_script.Text)

        End If
    End Sub

    Private Sub StartupInit()
        If loader.IsScriptAllowed Then

            CheckBox1.IsChecked = True
            Tb_script.IsEnabled = True
            Tb_script.TextDecorations = Nothing

        Else
            CheckBox1.IsChecked = False
            Tb_script.IsEnabled = False
            Tb_script.TextDecorations = TextDecorations.Strikethrough

        End If


    End Sub

#End Region


End Class

'nacitavanie sieti do zoznamu funguje <3
'upravit proxy font a textbox <3
'pridanie kniznice na zapis scriptu do systemu <3
'nejaky proces zadržuje vypnutie aplikácie  <3
'pridanie logiky na aktivnu siet <3
'pridat logiku checkboxu a textboxu na script <
'ak je tb_script text rovnaky ako file.readscript vypíše sa že nič sa nezmenilo <3

'pridat bluetooth  icon 


