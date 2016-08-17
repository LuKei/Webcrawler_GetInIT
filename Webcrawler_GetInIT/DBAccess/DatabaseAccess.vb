
Public Class DatabaseAccess

    Private ReadOnly connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=(local);Persist Security Info=False;"
    Private connection As OleDb.OleDbConnection

    Public Sub New()

        Dim conn As OleDb.OleDbConnection = getConnection()
        conn.Open()

        Dim cmd As New OleDb.OleDbCommand("CREATE DATABASE IF NOT EXISTS MyDb", conn)
        cmd.ExecuteNonQuery()

        cmd.CommandText = "CREATE TABLE IF NOT EXISTS JobOffer(Id INTEGER PRIMARY KEY NOT NULL, OfferTitle VARCHAR(100), Company VARCHAR(100), CoreAreas VARCHAR(100))
                           FieldsOfStudy VARCHAR(100), Degrees VARCHAR(100), Locations VARCHAR(150), NiceToKnow VARCHAR(2000), Description VARCHAR(10000),
                           ULR VARCHAR(100), HTML VARCHAR(200000))"
        cmd.ExecuteNonQuery()

    End Sub

    Public Sub AddJobOffers(jobs As List(Of JobOffer))

        'TODO


    End Sub

    Private Function getConnection() As OleDb.OleDbConnection
        If connection Is Nothing Then
            connection = New OleDb.OleDbConnection(connectionString)
        End If
        Return connection
    End Function


End Class
