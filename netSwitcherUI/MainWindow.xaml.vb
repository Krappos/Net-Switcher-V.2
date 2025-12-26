Imports System.Collections.ObjectModel
Imports Wpf.Ui.Tray.Controls
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

    ' Tray ikona
    Private trayIcon As NotifyIcon


    Public Sub New()
        InitializeComponent()
        firstBoot()

        Me.DataContext = Me
    End Sub

    ' Inicializácia tray ikony
    Private Sub InitializeTrayIcon()
        Try
            trayIcon = New NotifyIcon()
            trayIcon.TooltipText = "Network Switcher"
            trayIcon.MenuOnRightClick = True
            trayIcon.FocusOnLeftClick = True

            ' OPRAVA NAČÍTANIA IKONY
            Try
                ' Použitie relatívnej cesty k Resource (uisti sa, že Build Action je Resource)
                Dim iconUri As New Uri("pack://application:,,,/Apka.ico")
                trayIcon.Icon = New BitmapImage(iconUri)
            Catch ex As Exception
                ' Ak Resource zlyhá, skúsime fallback na tvoju cestu, ale ošetrenú
                Try
                    Dim iconImage As New BitmapImage()
                    iconImage.BeginInit()
                    iconImage.UriSource = New Uri("C:\Programovanie\Net switcher 2\netSwitcherUI\Apka.ico")
                    iconImage.EndInit()
                    trayIcon.Icon = iconImage
                Catch ex2 As Exception
                    MsgBox("Ikona zlyhala úplne: " & ex2.Message)
                End Try
            End Try

            ' MENU
            Dim contextMenu As New ContextMenu()
            Dim openItem As New MenuItem() With {.Header = "Otvoriť"}
            AddHandler openItem.Click, AddressOf MenuItem_Open_Click
            contextMenu.Items.Add(openItem)

            contextMenu.Items.Add(New Separator())

            Dim exitItem As New MenuItem() With {.Header = "Ukončiť"}
            AddHandler exitItem.Click, AddressOf MenuItem_Exit_Click
            contextMenu.Items.Add(exitItem)

            trayIcon.Menu = contextMenu

            ' ZOBRAZENIE
            trayIcon.Visibility = System.Windows.Visibility.Visible
            trayIcon.Register()

        Catch ex As Exception
            MsgBox("Chyba: " & ex.Message)
        End Try
    End Sub

    'ak sa zmení status tak sa znova načíta status kariet 
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

    ' Tray menu handlers
    Private Sub MenuItem_Open_Click(sender As Object, e As RoutedEventArgs)
        Me.Show()
        Me.WindowState = WindowState.Normal
        Me.Activate()
    End Sub

    Private Sub MenuItem_Exit_Click(sender As Object, e As RoutedEventArgs)
        ' Vyčistenie tray ikony pred ukončením
        If trayIcon IsNot Nothing Then
            trayIcon.Unregister()
            trayIcon.Dispose()
        End If
        Application.Current.Shutdown()
    End Sub

    ' Skryť okno namiesto zatvorenia
    Protected Overrides Sub OnClosing(e As CancelEventArgs)
        e.Cancel = True
        Me.Hide()
        MyBase.OnClosing(e)
    End Sub

    ' Vyčistenie pri zatvorení
    Protected Overrides Sub OnClosed(e As EventArgs)
        If trayIcon IsNot Nothing Then
            trayIcon.Unregister()
            trayIcon.Dispose()
        End If
        MyBase.OnClosed(e)
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

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        InitializeTrayIcon()
    End Sub
#End Region
End Class