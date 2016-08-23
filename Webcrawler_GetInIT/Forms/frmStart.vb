Public Class frmStart

    Private Sub frmStart_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnNeu_Click(sender As Object, e As EventArgs) Handles btnNeu.Click

        Dim dlg As New SaveFileDialog
        dlg.Filter = "SQLite Datenbank|*.sqlite"


        If dlg.ShowDialog = DialogResult.OK Then
            If Not String.IsNullOrEmpty(dlg.FileName) Then
                If System.IO.File.Exists(dlg.FileName) Then
                    System.IO.File.Delete(dlg.FileName)
                End If
                CType(Me.Owner, frmMain).filename = dlg.FileName
                Me.Close()
            Else
                MessageBox.Show("Bitte geben Sie einen gültigen Dateinamen an.", "Fehler", MessageBoxButtons.OK)
            End If
        End If
    End Sub

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click

        Dim dlg As New OpenFileDialog
        dlg.Filter = "SQLite Datenbank|*.sqlite"

        If dlg.ShowDialog = DialogResult.OK Then
            If Not String.IsNullOrEmpty(dlg.FileName) Then
                CType(Me.Owner, frmMain).filename = dlg.FileName
                Me.Close()
            Else
                MessageBox.Show("Bitte wählen Sie eine gültige Datei aus.", "Fehler", MessageBoxButtons.OK)
            End If
        End If
    End Sub
End Class