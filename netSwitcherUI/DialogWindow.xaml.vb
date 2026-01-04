Public Class DialogWindow

    Public Property messageText As String

    Public Sub New(mesageText As String)

        ' This call is required by the designer.

        Me.messageText = mesageText

        InitializeComponent()
        Me.DataContext = Me

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
End Class
