Public Class frmMain

    Private Db As DatabaseAccess

    Public Property filename As String



    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim myFrm As New frmStart()
        myFrm.ShowDialog(Me)

        Db = New DatabaseAccess(filename)

    End Sub


End Class
