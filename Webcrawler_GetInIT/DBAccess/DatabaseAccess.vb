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
            Dim cmd As New SQLiteCommand("CREATE TABLE JobOffer(Id INTEGER PRIMARY KEY NOT NULL, OfferTitle VARCHAR(255), Company VARCHAR(255), CoreAreas VARCHAR(255),
                           FieldsOfStudy VARCHAR(255), Degrees VARCHAR(255), Locations VARCHAR(255), NiceToKnow TEXT, Description TEXT,
                           URL VARCHAR(255), HTML TEXT)", conn)
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
