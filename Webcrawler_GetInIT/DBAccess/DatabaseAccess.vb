Imports System.IO
Imports System.Data.SQLite

Public Class DatabaseAccess

    Private connectionString As String
    Private connection As SQLiteConnection
    Private transaction As SQLiteTransaction

    Public Property Filename As String

    Public Sub New(Filename As String)

        Me.Filename = Filename
        connectionString = "Data Source=" & Filename & ";Version=3"



        Dim conn As SQLiteConnection = GetConnection()
        transaction = conn.BeginTransaction()

        Try
            Dim cmd As New SQLiteCommand("CREATE TABLE IF NOT EXISTS Sitemap(Id INTEGER PRIMARY KEY AUTOINCREMENT, HTML TEXT NOT NULL, Timestamp TEXT DEFAULT (datetime('now', 'localtime')))", conn)
            cmd.ExecuteNonQuery()

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS JobOffer(Id INTEGER NOT NULL, OfferTitle VARCHAR(255) NOT NULL, Company VARCHAR(255) NOT NULL, CoreAreas VARCHAR(255) NOT NULL,
                           FieldsOfStudy VARCHAR(255) NOT NULL, Degrees VARCHAR(255) NOT NULL, Locations VARCHAR(255) NOT NULL, NiceToKnow TEXT, Description TEXT NOT NULL,
                           URL VARCHAR(255) NOT NULL, HTML TEXT NOT NULL, Timestamp TEXT NOT NULL, SitemapId INTEGER NOT NULL, PRIMARY KEY(Id, SitemapId),
                           FOREIGN KEY(SitemapId) REFERENCES Sitemap(Id))"
            cmd.ExecuteNonQuery()
        Catch ex As SQLiteException
            MsgBox(ex.ToString)
        End Try

        Me.CommitChanges()

    End Sub

    Public Sub AddJobOffers(jobs As List(Of JobOffer))

        Dim conn As SQLiteConnection = GetConnection()
        Try
            Dim cmd As New SQLiteCommand("INSERT INTO JobOffer(Id, OfferTitle, Company, CoreAreas, FieldsOfStudy, Degrees, Locations, NiceToKnow, Description, URL, HTML, Timestamp, SitemapId) 
                                          VALUES(@Id, @OfferTitle, @Company, @CoreAreas, @FieldsOfStudy, @Degrees, @Locations, @NiceToKnow, @Description, @URL, @HTML, @Timestamp, @SitemapId)", conn)
            For Each job In jobs
                cmd.Parameters.AddWithValue("@Id", job.Id)
                cmd.Parameters.AddWithValue("@OfferTitle", job.OfferTitle)
                cmd.Parameters.AddWithValue("@Company", job.Company)
                'Dim timestampString As String = ""
                cmd.Parameters.AddWithValue("@CoreAreas", job.getCoreAreasAsString)
                cmd.Parameters.AddWithValue("@FieldsOfStudy", job.getFieldsOfStudyAsString)
                cmd.Parameters.AddWithValue("@Degrees", job.getDegreesAsString)
                cmd.Parameters.AddWithValue("@Locations", job.getLocationsAsString)
                cmd.Parameters.AddWithValue("@NiceToKnow", If(job.NiceToKnow Is Nothing, "", job.NiceToKnow))
                cmd.Parameters.AddWithValue("@Description", job.Description)
                cmd.Parameters.AddWithValue("@URL", job.URL)
                cmd.Parameters.AddWithValue("@HTML", job.HTML)
                'timestampString = job.Timestamp.Year & "-" & job.Timestamp.Month & "-" & job.Timestamp.Day _
                '                 & " " & job.Timestamp.Hour & ":" & job.Timestamp.Minute & ":" & job.Timestamp.Second
                cmd.Parameters.AddWithValue("@Timestamp", job.Timestamp.ToString("yyyy-MM-dd HH:mm:ss", Globalization.DateTimeFormatInfo.InvariantInfo))
                cmd.Parameters.AddWithValue("@SitemapId", job.Sitemap.Id)
                cmd.ExecuteNonQuery()
            Next
        Catch ex As SQLiteException
            MessageBox.Show(ex.ToString)
            Throw ex
        End Try


    End Sub

    Public Function AddSitemap(Sitemap As Sitemap) As Integer

        Dim conn As SQLiteConnection = GetConnection()
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


    Public Function GetJobOffers(Sitemap As Sitemap) As List(Of JobOffer)

        'TODO: wird Timestamp gebraucht?
        Dim conn As SQLiteConnection = GetConnection()
        Dim SitemapId As Integer = Sitemap.Id
        Dim jobOffers As New List(Of JobOffer)
        Try
            Dim adapter As New SQLiteDataAdapter("SELECT * FROM JobOffer WHERE SitemapId = @SitemapId ORDER BY Id ASC", conn)
            Dim table As New DataTable()
            Dim jobOffer As JobOffer
            adapter.SelectCommand.Parameters.AddWithValue("@SitemapId", SitemapId)
            adapter.Fill(table)
            For Each row In table.Rows
                jobOffer = New JobOffer()
                jobOffer.CoreAreas = New List(Of String)
                jobOffer.FieldsOfStudy = New List(Of String)
                jobOffer.Degrees = New List(Of String)
                jobOffer.Locations = New List(Of String)
                With jobOffer
                    .Id = row("Id")
                    .OfferTitle = row("OfferTitle")
                    .Company = row("Company")
                    For Each area In CStr(row("CoreAreas")).Split(", ")
                        jobOffer.CoreAreas.Add(area)
                    Next
                    For Each field In CStr(row("FieldsOfStudy")).Split(", ")
                        jobOffer.FieldsOfStudy.Add(field)
                    Next
                    For Each degree In CStr(row("Degrees")).Split(", ")
                        jobOffer.Degrees.Add(degree)
                    Next
                    For Each location In CStr(row("Locations")).Split(", ")
                        jobOffer.Locations.Add(location)
                    Next
                    .NiceToKnow = row("NiceToKnow")
                    .Description = row("Description")
                    .URL = row("URL")
                    .HTML = row("HTML")
                    .Timestamp = CType(DateTime.ParseExact(row("Timestamp"), "yyyy-MM-dd HH:mm:ss", Globalization.DateTimeFormatInfo.InvariantInfo), DateTime)
                End With
                jobOffer.Sitemap = Sitemap
                jobOffers.Add(jobOffer)
            Next
        Catch ex As SQLiteException
            MsgBox(ex.ToString)
            Throw ex
        End Try

        Return jobOffers

    End Function


    Public Function GetSitemaps() As List(Of Sitemap)

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

    Public Function GetLastSitemap() As Sitemap

        Dim sitemap As Sitemap
        Try
            Dim adapter As New SQLiteDataAdapter("SELECT * FROM Sitemap WHERE Id = (SELECT MAX(Id) FROM Sitemap)", getConnection())
            Dim table As New DataTable
            adapter.Fill(table)
            If table.Rows.Count > 0 Then
                sitemap = New Sitemap(CStr(table.Rows(0).Item("HTML")))
                sitemap.Id = CInt(table.Rows(0).Item("Id"))
                sitemap.Timestamp = table.Rows(0).Item("Timestamp")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

        Return sitemap
    End Function


    Private Function GetConnection() As SQLiteConnection
        If connection Is Nothing Then
            connection = New SQLiteConnection(connectionString)
        End If

        If connection.State = ConnectionState.Closed Then
            connection.Open()
        End If

        Return connection
    End Function


    Public Sub CommitChanges()

        transaction.Commit()
        transaction = GetConnection().BeginTransaction()

    End Sub


    Public Sub RollbackChanges()

        transaction.Rollback()
        transaction = GetConnection().BeginTransaction()

    End Sub

    Public Sub Close()
        connection.Close()
    End Sub


End Class
