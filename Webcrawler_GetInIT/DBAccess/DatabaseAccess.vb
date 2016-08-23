Imports System.IO
Imports System.Data.SQLite

Public Class DatabaseAccess

    Private connectionString As String
    Private connection As SQLiteConnection

    Public Property filename As String

    Public Sub New(filename As String)

        Me.filename = filename
        connectionString = "Data Source=" & filename & ";Version=3"



        Dim conn As SQLiteConnection = getConnection()


        Try
            Dim cmd As New SQLiteCommand("CREATE TABLE IF NOT EXISTS Sitemap(Id INTEGER PRIMARY KEY, Sourcecode TEXT NOT NULL, Timestamp DATETIME DEFAULT (datetime('now', 'localtime')))", conn)
            cmd.ExecuteNonQuery()

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS JobOffer(Id INTEGER PRIMARY KEY, OfferTitle VARCHAR(255) NOT NULL, Company VARCHAR(255) NOT NULL, CoreAreas VARCHAR(255) NOT NULL,
                           FieldsOfStudy VARCHAR(255) NOT NULL, Degrees VARCHAR(255) NOT NULL, Locations VARCHAR(255) NOT NULL, NiceToKnow TEXT NOT NULL, Description TEXT NOT NULL,
                           URL VARCHAR(255) NOT NULL, SitemapId INTEGER NOT NULL,FOREIGN KEY(SitemapId) REFERENCES Sitemap(Id))"
            cmd.ExecuteNonQuery()
        Catch ex As SQLiteException
            MsgBox(ex.ToString)
        End Try


    End Sub

    Public Sub AddJobOffers(jobs As List(Of JobOffer))

        'TODO

        Dim conn As SQLiteConnection = getConnection()


    End Sub

    Private Function getConnection() As SQLiteConnection
        If connection Is Nothing Then
            connection = New SQLiteConnection(connectionString)
        End If

        If connection.State = ConnectionState.Closed Then
            connection.Open()
        End If

        Return connection
    End Function


End Class
