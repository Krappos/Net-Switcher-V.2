' Správne importy
Imports System.Windows.Controls
Imports Wpf.Ui.Controls

' netCard musí dediť z UserControl, nie z okna!
Public Class netCard
    Inherits UserControl

    Public Sub New()
        ' Tento riadok prepojí VB kód s tvojím XAML dizajnom
        InitializeComponent()
    End Sub



End Class