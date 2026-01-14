Imports System.Windows.Interop
Imports System.Runtime.InteropServices

Public Class Refresher
    Public Event StatusChanged()

    Private Const WM_DEVICECHANGE As Integer = &H219
    Private _hwndSource As HwndSource

    Public Sub StartMonitoring(window As Window)
        Dim helper As New WindowInteropHelper(window)
        Dim handle As IntPtr = helper.Handle

        _hwndSource = HwndSource.FromHwnd(handle)
        _hwndSource.AddHook(AddressOf WndProc)
    End Sub

    Private Function WndProc(hwnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr, ByRef handled As Boolean) As IntPtr
        If msg = WM_DEVICECHANGE Then
            RaiseEvent StatusChanged()
        End If

        Return IntPtr.Zero
    End Function

    Public Sub StopMonitoring()
        _hwndSource?.RemoveHook(AddressOf WndProc)
    End Sub
End Class