Imports System.Windows.Interop
Imports netSwitcherBackend

Public Class ProxyRegister


    Dim proxy As New proxyController()
    Dim config As New ConfigControler()
    Dim dialogWin As New DialogWindow()
    Public Sub New()
        InitializeComponent()

    End Sub

    Private Async Sub Button_Click(sender As Object, e As RoutedEventArgs)
        proxy.EnableProxyScript()

        Await Task.Delay(2000)

        Dim registrovanaProxy As String = proxy.FirstBoot()

        If Not String.IsNullOrEmpty(registrovanaProxy) Then
            config.setScript(registrovanaProxy)
            MsgBox("Úspešne načítané z registrov: " & registrovanaProxy)
        Else
            MsgBox("V registroch sa nenašla žiadna AutoConfigURL adresa!")
        End If

        Me.Close()

    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)

        Me.Close()
    End Sub
End Class
