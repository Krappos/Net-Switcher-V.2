Imports System.Windows.Interop
Imports System.Runtime.InteropServices

Public Class Refresher
    Private delayTimer As System.Windows.Threading.DispatcherTimer
    Private lastEventTime As DateTime = DateTime.MinValue
    Public Event StatusChanged()

    Private Const WM_DEVICECHANGE As Integer = &H219
    Private Const DBT_DEVICEARRIVAL As Integer = &H8000
    Private Const DBT_DEVICEREMOVECOMPLETE As Integer = &H8004
    Private Const DBT_DEVNODES_CHANGED As Integer = &H7
    Private _hwndSource As HwndSource

    Public Sub New()
        delayTimer = New System.Windows.Threading.DispatcherTimer()
        delayTimer.Interval = TimeSpan.FromSeconds(10)
        AddHandler delayTimer.Tick, Sub(s, e)
                                        delayTimer.Stop()
                                        RaiseEvent StatusChanged()
                                    End Sub
    End Sub

    Public Sub StartMonitoring(window As Window)
        Dim helper As New WindowInteropHelper(window)
        Dim handle As IntPtr = helper.Handle
        _hwndSource = HwndSource.FromHwnd(handle)
        _hwndSource.AddHook(AddressOf WndProc)
    End Sub

    Private Function WndProc(hwnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr, ByRef handled As Boolean) As IntPtr
        If msg = WM_DEVICECHANGE Then
            Dim action As Integer = wParam.ToInt32()
            If action = DBT_DEVICEREMOVECOMPLETE OrElse action = DBT_DEVICEARRIVAL OrElse action = DBT_DEVNODES_CHANGED Then

                If (DateTime.Now - lastEventTime).TotalSeconds < 1 Then
                    Return IntPtr.Zero
                End If

                lastEventTime = DateTime.Now
                delayTimer.Stop()
                delayTimer.Start()
            End If
        End If
        Return IntPtr.Zero
    End Function

    Public Sub StopMonitoring()
        delayTimer?.Stop()
        _hwndSource?.RemoveHook(AddressOf WndProc)
    End Sub
End Class