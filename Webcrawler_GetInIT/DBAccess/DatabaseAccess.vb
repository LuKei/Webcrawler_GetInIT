
Public Class DatabaseAccess

    Private connectionString As String
    Private connection As OleDb.OleDbConnection

    Public Property filename As String

    Public Sub New(filename As String)

        Me.filename = filename
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filename & ";Persist Security Info=False;"

        Dim conn As OleDb.OleDbConnection = getConnection()

        Try
            Dim cmd As New OleDb.OleDbCommand("CREATE DATABASE IF NOT EXISTS MyDb", conn)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            'Do nothing
        End Try

        Try
            Dim cmd As New OleDb.OleDbCommand("CREATE TABLE IF NOT EXISTS JobOffer(Id INTEGER PRIMARY KEY NOT NULL, OfferTitle VARCHAR(100), Company VARCHAR(100), CoreAreas VARCHAR(100))
                           FieldsOfStudy VARCHAR(100), Degrees VARCHAR(100), Locations VARCHAR(150), NiceToKnow VARCHAR(2000), Description VARCHAR(10000),
                           ULR VARCHAR(100), HTML VARCHAR(200000))", conn)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            'Do nothing
        End Try


    End Sub

    Public Sub AddJobOffers(jobs As List(Of JobOffer))

        'TODO


    End Sub

    Private Function getConnection() As OleDb.OleDbConnection
        If connection Is Nothing Then
            connection = New OleDb.OleDbConnection(connectionString)
        End If

        If connection.State = ConnectionState.Closed Then
            connection.Open()
        End If

        Return connection
    End Function


End Class
