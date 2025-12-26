Imports System.Collections.ObjectModel
Imports Wpf.Ui.Controls
Imports netSwitcherBackend
Imports System.IO


Class MainWindow
    Inherits FluentWindow


    Public Property Items As New ObservableCollection(Of NetworkInfo)()

    Dim loader As New NewtorkLoader()
    Dim informacia As New NetworkInfo()
    Dim file As New ProxyWorker()
    Dim activeState As Boolean

    Public Sub New()
        InitializeComponent()

        firstBoot()



        Me.DataContext = Me
    End Sub

    Private Sub CheckBoxController(sender As Object, e As RoutedEventArgs) Handles CheckBox1.Unchecked, CheckBox1.Checked

        If CheckBox1.IsChecked = True Then
            Tb_script.IsEnabled = True
            Tb_script.TextDecorations = Nothing
        Else
            Tb_script.IsEnabled = False
            Tb_script.TextDecorations = TextDecorations.Strikethrough
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
        Tb_script.Text = file.ReadScript()
        loadNetworkInfo()
        CheckBox1.IsChecked = True



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


'pridat bluetooth  icon 
'upravit proxy font a textbox 
'pridat logiku checkboxu a textboxu na script
'pridanie kniznice na zapis scriptu do systemu
