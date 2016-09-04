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
            Dim cmd As New SQLiteCommand("CREATE TABLE IF NOT EXISTS Sitemap(Id INTEGER PRIMARY KEY AUTOINCREMENT, HTML TEXT NOT NULL, Timestamp DATETIME DEFAULT (datetime('now', 'localtime')))", conn)
            cmd.ExecuteNonQuery()

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS JobOffer(Id INTEGER NOT NULL, OfferTitle VARCHAR(255) NOT NULL, Company VARCHAR(255) NOT NULL, CoreAreas VARCHAR(255) NOT NULL,
                           FieldsOfStudy VARCHAR(255) NOT NULL, Degrees VARCHAR(255) NOT NULL, Locations VARCHAR(255) NOT NULL, NiceToKnow TEXT, Description TEXT NOT NULL,
                           URL VARCHAR(255) NOT NULL, HTML TEXT NOT NULL, Timestamp DATETIME NOT NULL, SitemapId INTEGER NOT NULL, PRIMARY KEY(Id, SitemapId),
                           FOREIGN KEY(SitemapId) REFERENCES Sitemap(Id))"
            cmd.ExecuteNonQuery()
        Catch ex As SQLiteException
            MsgBox(ex.ToString)
        End Try


    End Sub

    Public Sub AddJobOffers(jobs As List(Of JobOffer), sitemapId As Integer)

        Dim conn As SQLiteConnection = getConnection()
        Try
            Dim cmd As New SQLiteCommand("INSERT INTO JobOffer(Id, OfferTitle, Company, CoreAreas, FieldsOfStudy, Degrees, Locations, NiceToKnow, Description, URL, HTML, Timestamp, SitemapId) 
                                          VALUES(@Id, @OfferTitle, @Company, @CoreAreas, @FieldsOfStudy, @Degrees, @Locations, @NiceToKnow, @Description, @URL, @HTML, @Timestamp, @SitemapId)", conn)
            For Each job In jobs
                cmd.Parameters.AddWithValue("@Id", job.Id)
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
                    fields += field & ", "
                Next
                For Each degree In job.Degrees
                    degrees += degree & ", "
                Next
                For Each location In job.Locations
                    locations += location & ", "
                Next
                areas = areas.Remove(areas.Length - 2)
                fields = fields.Remove(fields.Length - 2)
                degrees = degrees.Remove(degrees.Length - 2)
                locations = locations.Remove(locations.Length - 2)
                cmd.Parameters.AddWithValue("@CoreAreas", areas)
                cmd.Parameters.AddWithValue("@FieldsOfStudy", fields)
                cmd.Parameters.AddWithValue("@Degrees", degrees)
                cmd.Parameters.AddWithValue("@Locations", locations)
                cmd.Parameters.AddWithValue("@NiceToKnow", If(job.NiceToKnow Is Nothing, "", job.NiceToKnow))
                cmd.Parameters.AddWithValue("@Description", job.Description)
                cmd.Parameters.AddWithValue("@URL", job.URL)
                cmd.Parameters.AddWithValue("@HTML", job.HTML)
                cmd.Parameters.AddWithValue("@Timestamp", job.Timestamp.ToString)
                cmd.Parameters.AddWithValue("@SitemapId", sitemapId)
                cmd.ExecuteNonQuery()
            Next
        Catch ex As SQLiteException
            MessageBox.Show(ex.ToString)
            Throw ex
        End Try


    End Sub

    Public Function AddSitemap(Sitemap As Sitemap) As Integer

        Dim conn As SQLiteConnection = getConnection()
        Try
            Dim cmd As New SQLiteCommand("INSERT INTO Sitemap (HTML) VALUES (@sitemapString)", conn)
            cmd.Parameters.AddWithValue("@sitemapString", Sitemap.Sourcecode)
            cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            cmd.CommandText = "SELECT last_insert_rowid()"
            Return (CInt(cmd.ExecuteScalar()))
        Catch ex As SQLiteException
            MsgBox(ex.ToString)
            Return -1
        End Try

    End Function


    Public Function getJobOffers(Sitemap As Sitemap) As List(Of JobOffer)

        Dim conn As SQLiteConnection = getConnection()
        Dim SitemapId As Integer = Sitemap.Id
        Dim jobOffers As New List(Of JobOffer)
        Try
            Dim adapter As New SQLiteDataAdapter("SELECT * FROM JobOffer WHERE SitemapId = @SitemapId", conn)
            Dim table As New DataTable()
            Dim jobOffer As JobOffer
            adapter.SelectCommand.Parameters.AddWithValue("@SitemapId", SitemapId)
            adapter.Fill(table)
            For Each row In table.Rows
                jobOffer = New JobOffer()
                With jobOffer
                    .OfferTitle = row("OfferTitle")
                    .Company = row("Company")
                    .CoreAreas = row("CoreAreas")
                    .FieldsOfStudy = row("FieldsOfStudy")
                    .Degrees = row("Degrees")
                    .Locations = row("Loactions")
                    .NiceToKnow = row("NiceToKnow")
                    .Description = row("Description")
                    .URL = row("URL")
                    .HTML = row("HTML")
                    .Timestamp = row("Timestamp")
                End With
                jobOffers.Add(jobOffer)
            Next
        Catch ex As SQLiteException
            MsgBox(ex.ToString)
            Throw ex
        End Try

        Return jobOffers

    End Function


    Public Function getSitemaps() As List(Of Sitemap)

        Dim sitemaps As New List(Of Sitemap)
        Try
            Dim adapter As New SQLiteDataAdapter("SELECT * FROM Sitemap", getConnection())
            Dim table As New DataTable()
            Dim sitemap As Sitemap
            adapter.Fill(table)
            For Each row In table.Rows
                sitemap = New Sitemap(row("HTML"))
                With sitemap
                    .Id = row("Id")
                    .Timestamp = row("Timestamp")
                End With
                sitemaps.Add(sitemap)
            Next
        Catch ex As SQLiteException
            MsgBox(ex.ToString)
            Throw ex
        End Try

        Return sitemaps

    End Function

    Public Function getLastSitemap() As Sitemap

        Dim sitemap As Sitemap
        Try
            Dim adapter As New SQLiteDataAdapter("SELECT * FROM Sitemap WHERE Id = (SELECT MAX(Id) FROM JobOffer)", getConnection())
            Dim table As New DataTable
            adapter.Fill(table)
            If table.Rows.Count > 0 Then
                sitemap = New Sitemap(CStr(table.Rows("HTML").Item(1)))
                sitemap.Id = CInt(table.Rows("Id").Item(0))
                sitemap.Timestamp = table.Rows("Timestamp").Item(0)
            End If
        Catch ex As Exception

        End Try

        Return sitemap
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
