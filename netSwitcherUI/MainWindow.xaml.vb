Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Windows.Interop
Imports netSwitcherBackend

Class MainWindow
    Inherits FluentWindow


    Public Property Items As New ObservableCollection(Of NetworkInfo)()
#Region "Instances & variables "

    Dim trayIcon As Wpf.Ui.Tray.Controls.NotifyIcon
    Dim loader As New NewtorkLoader()
    Dim inform As New NetworkInfo()
    Dim file As New FileWorker()
    Dim activeState As Boolean
    Dim proxy As New proxyController()

#End Region

    'constructor
    Public Sub New()

        InitializeComponent()

        firstBoot()

        Me.DataContext = Me


    End Sub

#Region "View methods"
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        trayIcon = TryCast(Me.Resources("MyTrayIcon"), Wpf.Ui.Tray.Controls.NotifyIcon)
        trayIcon.Register()

    End Sub

    Protected Overrides Sub OnClosing(e As CancelEventArgs)
        e.Cancel = True

        Me.Hide()

        Me.ShowInTaskbar = False

        MyBase.OnClosing(e)

        'zapisanie pozície okna 
        'zapisanie pozicie okna


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

#End Region


    Private Sub refresh()
        Items.Clear()
        loadNetworkInfo()
    End Sub

    Private Sub appClose(sender As Object, e As RoutedEventArgs)
        If trayIcon IsNot Nothing Then
            trayIcon.Unregister()
        End If
        Application.Current.Shutdown()
    End Sub




#Region "private methods"



    Private Sub firstBoot()

        'vytvorenie súboru pre proxy Script
        file.CreateFiles()

        If String.IsNullOrEmpty(proxy.FirstBoot) Then
            'nacítanie hodnoty zo súboru



        Else
            'zapisanie hodnoty do súboru
            file.WriteScript(proxy.FirstBoot)

        End If

        Tb_script.Text = file.ReadScript()

        StartupInit()
        loadNetworkInfo()


    End Sub

    Private Sub StartupInit()


        If proxy.IsScriptAllowed Then

            CheckBox1.IsChecked = True
            Tb_script.IsEnabled = True
            Tb_script.TextDecorations = Nothing

        Else
            CheckBox1.IsChecked = False
            Tb_script.IsEnabled = False
            Tb_script.TextDecorations = TextDecorations.Strikethrough

        End If


    End Sub

    'nacitanie dostupnych sieti do userControlera

    Private Sub loadNetworkInfo()
        For Each sset In loader.GetNetWorkCards()
            Items.Add(sset)
        Next

    End Sub

    Private Sub Tb_scriptController()
        If String.IsNullOrWhiteSpace(Tb_script.Text) Then
            MsgBox("prosím zapíšte svoju adresu")
        ElseIf Tb_script.Text = file.ReadScript() Then
            MsgBox("vaša proxy adresa sa nezmenila")
        Else
            MsgBox("vasa proxy je uspesne zmenená")
            file.WriteScript(Tb_script.Text)
            proxy.EnableProxyScript()
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

