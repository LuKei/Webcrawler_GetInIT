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
            Dim cmd As New SQLiteCommand("CREATE TABLE IF NOT EXISTS Sitemap(Id INTEGER PRIMARY KEY AUTOINCREMENT, Sourcecode TEXT NOT NULL, Timestamp DATETIME DEFAULT (datetime('now', 'localtime')))", conn)
            cmd.ExecuteNonQuery()

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS JobOffer(Id INTEGER PRIMARY KEY AUTOINCREMENT, OfferTitle VARCHAR(255) NOT NULL, Company VARCHAR(255) NOT NULL, CoreAreas VARCHAR(255) NOT NULL,
                           FieldsOfStudy VARCHAR(255) NOT NULL, Degrees VARCHAR(255) NOT NULL, Locations VARCHAR(255) NOT NULL, NiceToKnow TEXT NOT NULL, Description TEXT NOT NULL,
                           URL VARCHAR(255) NOT NULL, Timestamp DATETIME NOT NULL, SitemapId INTEGER NOT NULL,FOREIGN KEY(SitemapId) REFERENCES Sitemap(Id))"
            cmd.ExecuteNonQuery()
        Catch ex As SQLiteException
            MsgBox(ex.ToString)
        End Try


    End Sub

    Public Sub AddJobOffers(jobs As List(Of JobOffer), sitemapId As Integer)

        'TODO

        Dim conn As SQLiteConnection = getConnection()
        Try
            Dim cmd As New SQLiteCommand("INSERT INTO JobOffer(OfferTitle, Company, CoreAreas, FieldsOfStudy, Degrees, Locations, NiceToKnow, Description, URL, Timestamp, SitemapId) 
                                          VALUES(@OfferTitle, @Company, @CoreAreas, @FieldsOfStudy, @Degrees, @Locations, @NiceToKnow, @Description, @URL, @Timestamp, @SitemapId)", conn)
            For Each job In jobs
                cmd.Parameters.AddWithValue("@OfferTitle", job.OfferTitle)
                cmd.Parameters.AddWithValue("@Company", job.Company)
                Dim areas As String = ""
                Dim fields As String = ""
                Dim degrees As String = ""
                Dim locations As String = ""
                For Each area In job.CoreAreas
                    areas += area & ", "
                Next
                For Each field In job.FieldsOfStudy
                    fields += fields & ", "
                Next
                For Each degree In job.Degrees
                    degrees += degree & " ,"
                Next
                For Each location In job.Locations
                    locations += location & " ,"
                Next
                areas.Remove(areas.Length - 3)
                fields.Remove(fields.Length - 3)
                degrees.Remove(degrees.Length - 3)
                locations.Remove(locations.Length - 3)
                cmd.Parameters.AddWithValue("@CoreAreas", areas)
                cmd.Parameters.AddWithValue("@FiledsOfStudy", fields)
                cmd.Parameters.AddWithValue("@Degrees", degrees)
                cmd.Parameters.AddWithValue("@Locations", locations)
                cmd.Parameters.AddWithValue("@NiceToKnow", job.NiceToKnow)
                cmd.Parameters.AddWithValue("@Description", job.Description)
                cmd.Parameters.AddWithValue("@URL", job.URL)
                cmd.Parameters.AddWithValue("@Timestamp", job.Timestamp.ToString)
                cmd.Parameters.AddWithValue("@SitemapId", sitemapId)
                cmd.ExecuteNonQuery()
                cmd.Parameters.Clear()
            Next
        Catch ex As SQLiteException

        End Try


    End Sub

    Public Function AddSitemap(sitemapString As String) As Integer

        Dim conn As SQLiteConnection = getConnection()
        Try
            Dim cmd As New SQLiteCommand("INSERT INTO Sitemap (Sourcecode) VALUES (@sitemapString)", conn)
            cmd.Parameters.AddWithValue("@sitemapString", sitemapString)
            cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            cmd.CommandText = "SELECT Id FROM Sitemap"
            Dim reader As SQLiteDataReader = cmd.ExecuteReader()
            Return CInt(reader.Read())
        Catch ex As SQLiteException
            MsgBox(ex.ToString)
        End Try

    End Function

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
