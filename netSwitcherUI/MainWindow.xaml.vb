Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports netSwitcherBackend

Class MainWindow
    Inherits FluentWindow


    Public Property Items As New ObservableCollection(Of NetworkInfo)()
    Public Property proxyInfo As New ProxyInfo()

#Region "Instances & variables "

    Dim trayIcon As Wpf.Ui.Tray.Controls.NotifyIcon

    Dim loader As New NewtorkLoader()
    Dim inform As New NetworkInfo()
    Dim proxy As New proxyController()
    Dim config As New ConfigControler()
    Dim proxka As New ProxyRegister()

    Dim activeState As Boolean

#End Region

    'constructor
    Public Sub New()

        Me.Height = config.getHeight
        Me.Width = config.getWidth

        InitializeComponent()



        Me.DataContext = Me


        If config.getInitialization = "false" Then
            'ak je zapnuta tak ju len natiahne a ak je vypnuta tak ju zapne natiahne a vypne 

            'zobrazí proxy okno
            'proxka.ShowDialog()


        End If

    End Sub

#Region "View methods"

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded


        trayIcon = TryCast(Me.Resources("MyTrayIcon"), Wpf.Ui.Tray.Controls.NotifyIcon)
        TrayInfoControler()
        trayIcon.DataContext = Me.proxyInfo
        trayIcon.Register()


        firstBoot()

        Me.Top = config.getPosY
        Me.Left = config.getPosX

    End Sub

    Protected Overrides Sub OnClosing(e As CancelEventArgs)

        config.setHeight(Me.ActualHeight)
        config.setWidth(Me.ActualWidth)

        config.setPosX(Me.Left)
        config.setPosY(Me.Top)
        e.Cancel = True

        Me.Hide()

        Me.ShowInTaskbar = False

        MyBase.OnClosing(e)


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
        TrayInfoControler()
    End Sub

    Private Sub Tb_script_KeyDown(sender As Object, e As KeyEventArgs) Handles Tb_script.KeyDown
        If e.Key = Key.Enter Then
            Tb_scriptController()
        End If

    End Sub

#End Region


    Private Sub appClose(sender As Object, e As RoutedEventArgs)
        If trayIcon IsNot Nothing Then
            trayIcon.Unregister()
        End If
        Application.Current.Shutdown()
    End Sub


#Region "private methods"

    Private Sub TrayInfoControler()

        If proxy.IsScriptAllowed Then
            proxyInfo.farbaStavu = "green"
            proxyInfo.spravaStavu = "Zapnutá"
        Else

            proxyInfo.farbaStavu = "red"
            proxyInfo.spravaStavu = "Vypnutá"
        End If


        refreshnes()
    End Sub

    Public Sub refreshnes()
        trayIcon.DataContext = Nothing
        trayIcon.DataContext = Me.proxyInfo
    End Sub
    Public Sub refresh()
        Items.Clear()
        loadNetworkInfo()
    End Sub
    Private Sub firstBoot()

        'vytvorenie súboru pre proxy Script
        'overenie vytvorenia súboru pri náhodnom odstranení file.CreateFiles()

        If String.IsNullOrEmpty(proxy.FirstBoot) Then
            'nacítanie hodnoty zo súboru


        Else
            'zapisanie hodnoty do súboru

            config.setScript(proxy.FirstBoot)
        End If

        ' Tb_script.Text = file.ReadScript()
        Tb_script.Text = config.getScript()

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
        ElseIf Tb_script.Text = config.getScript() Then
            MsgBox("vaša proxy adresa sa nezmenila")
        Else
            MsgBox("vasa proxy je uspesne zmenená")
            config.setScript(Tb_script.Text)
            proxy.EnableProxyScript()
        End If
    End Sub

#End Region

End Class

