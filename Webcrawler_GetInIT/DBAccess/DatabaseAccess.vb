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

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS JobOffer(Id INTEGER NOT NULL, OfferTitle VARCHAR(255) NOT NULL, Company VARCHAR(255) NOT NULL, NiceToKnow TEXT, Description TEXT NOT NULL,
                           URL VARCHAR(255) NOT NULL, HTML TEXT NOT NULL, Timestamp TEXT NOT NULL, SitemapId INTEGER NOT NULL, PRIMARY KEY(Id, SitemapId),
                           FOREIGN KEY(SitemapId) REFERENCES Sitemap(Id))"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS CoreAreas(JobOfferId INTEGER NOT NULL, SitemapId INTEGER NOT NULL, CoreArea VARCHAR(255) NOT NULL, PRIMARY KEY(JobOfferId, SitemapId, CoreArea), FOREIGN KEY(JobOfferId, SitemapId) REFERENCES Sitemap(JobOfferId, SitemapId))"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS FieldsOfStudy(JobOfferId INTEGER NOT NULL, SitemapId INTEGER NOT NULL, FieldOfStudy VARCHAR(255) NOT NULL, PRIMARY KEY(JobOfferId, SitemapId, FieldOfStudy), FOREIGN KEY(JobOfferId, SitemapId) REFERENCES Sitemap(JobOfferId, SitemapId))"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS Degrees(JobOfferId INTEGER NOT NULL, SitemapId INTEGER NOT NULL, Degree VARCHAR(255) NOT NULL, PRIMARY KEY(JobOfferId, SitemapId, Degree), FOREIGN KEY(JobOfferId, SitemapId) REFERENCES Sitemap(JobOfferId, SitemapId))"
            cmd.ExecuteNonQuery()

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS Locations(JobOfferId INTEGER NOT NULL,  SitemapId INTEGER NOT NULL, Location VARCHAR(255) NOT NULL, PRIMARY KEY(JobOfferId, SitemapId, Location), FOREIGN KEY(JobOfferId, SitemapId) REFERENCES Sitemap(JobOfferId, SitemapId))"
            cmd.ExecuteNonQuery()

        Catch ex As SQLiteException
            MsgBox(ex.ToString)
        End Try

        Me.CommitChanges()

    End Sub

    Public Sub AddJobOffers(jobs As List(Of JobOffer))

        Dim conn As SQLiteConnection = GetConnection()
        Try
            Dim cmd As New SQLiteCommand("INSERT INTO JobOffer(Id, OfferTitle, Company, NiceToKnow, Description, URL, HTML, Timestamp, SitemapId) 
                                          VALUES(@Id, @OfferTitle, @Company, @NiceToKnow, @Description, @URL, @HTML, @Timestamp, @SitemapId)", conn)

            Dim cmd2 As New SQLiteCommand(conn)
            For Each job In jobs
                cmd.Parameters.AddWithValue("@Id", job.Id)
                cmd.Parameters.AddWithValue("@OfferTitle", job.OfferTitle)
                cmd.Parameters.AddWithValue("@Company", job.Company)
                For Each coreArea In job.CoreAreas
                    cmd2.CommandText = "INSERT INTO CoreAreas(JobOfferId, SitemapId, CoreArea) VALUES(@JobOfferId, @SitemapId, @CoreArea)"
                    cmd2.Parameters.AddWithValue("@JobOfferId", job.Id)
                    cmd2.Parameters.AddWithValue("@SitemapId", job.Sitemap.Id)
                    cmd2.Parameters.AddWithValue("@CoreArea", coreArea)
                    cmd2.ExecuteNonQuery()
                Next
                For Each fieldOfStudy In job.FieldsOfStudy
                    cmd2.CommandText = "INSERT INTO FieldsOfStudy(JobOfferId, SitemapId, FieldOfStudy) VALUES(@JobOfferId, @SitemapId, @FieldOfStudy)"
                    cmd2.Parameters.Clear()
                    cmd2.Parameters.AddWithValue("@JobOfferId", job.Id)
                    cmd2.Parameters.AddWithValue("@SitemapId", job.Sitemap.Id)
                    cmd2.Parameters.AddWithValue("@FieldOfStudy", fieldOfStudy)
                    cmd2.ExecuteNonQuery()
                Next
                For Each degree In job.Degrees
                    cmd2.CommandText = "INSERT INTO Degrees(JobOfferId, SitemapId, Degree) VALUES(@JobOfferId, @SitemapId, @Degree)"
                    cmd2.Parameters.Clear()
                    cmd2.Parameters.AddWithValue("@JobOfferId", job.Id)
                    cmd2.Parameters.AddWithValue("@SitemapId", job.Sitemap.Id)
                    cmd2.Parameters.AddWithValue("@Degree", degree)
                    cmd2.ExecuteNonQuery()
                Next
                For Each location In job.Locations
                    cmd2.CommandText = "INSERT INTO Locations(JobOfferId, SitemapId, Location) VALUES(@JobOfferId, @SitemapId, @Location)"
                    cmd2.Parameters.Clear()
                    cmd2.Parameters.AddWithValue("@JobOfferId", job.Id)
                    cmd2.Parameters.AddWithValue("@SitemapId", job.Sitemap.Id)
                    cmd2.Parameters.AddWithValue("@Location", location)
                    cmd2.ExecuteNonQuery()
                Next
                cmd.Parameters.AddWithValue("@NiceToKnow", If(job.NiceToKnow Is Nothing, "", job.NiceToKnow))
                cmd.Parameters.AddWithValue("@Description", job.Description)
                cmd.Parameters.AddWithValue("@URL", job.URL)
                cmd.Parameters.AddWithValue("@HTML", job.HTML)
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

        Dim conn As SQLiteConnection = GetConnection()
        Dim SitemapId As Integer = Sitemap.Id
        Dim jobOffers As New List(Of JobOffer)
        Try
            Dim jobOffer As JobOffer

            Dim jobOfferTable As New DataTable()
            Dim coreAreaTable As New DataTable()
            Dim fieldsOfStudyTable As New DataTable()
            Dim degreesTable As New DataTable()
            Dim locationsTable As New DataTable()

            Dim adapter As New SQLiteDataAdapter("SELECT * FROM JobOffer WHERE SitemapId = @SitemapId ORDER BY Id ASC", conn)
            adapter.SelectCommand.Parameters.AddWithValue("@SitemapId", SitemapId)
            adapter.Fill(jobOfferTable)

            adapter.SelectCommand.CommandText = "SELECT * FROM CoreAreas WHERE SitemapId = @SitemapId"
            adapter.Fill(coreAreaTable)

            adapter.SelectCommand.CommandText = "SELECT * FROM FieldsOfStudy WHERE SitemapId = @SitemapId"
            adapter.Fill(fieldsOfStudyTable)

            adapter.SelectCommand.CommandText = "SELECT * FROM Degrees WHERE SitemapId = @SitemapId"
            adapter.Fill(degreesTable)

            adapter.SelectCommand.CommandText = "SELECT * FROM Locations WHERE SitemapId = @SitemapId"
            adapter.Fill(locationsTable)

            For Each row In jobOfferTable.Rows
                jobOffer = New JobOffer()
                With jobOffer
                    .CoreAreas = New List(Of String)
                    .FieldsOfStudy = New List(Of String)
                    .Degrees = New List(Of String)
                    .Locations = New List(Of String)

                    .Id = row("Id")
                    .OfferTitle = row("OfferTitle")
                    .Company = row("Company")
                    .CoreAreas = (From row2 As DataRow In coreAreaTable.Rows Where row2.Item("JobOfferId") = jobOffer.Id Select CStr(row2.Item("CoreArea"))).ToList
                    .FieldsOfStudy = (From row2 As DataRow In fieldsOfStudyTable.Rows Where row2.Item("JobOfferId") = jobOffer.Id Select CStr(row2.Item("FieldOfStudy"))).ToList
                    .Degrees = (From row2 As DataRow In degreesTable.Rows Where row2.Item("JobOfferId") = jobOffer.Id Select CStr(row2.Item("Degree"))).ToList
                    .Locations = (From row2 As DataRow In locationsTable.Rows Where row2.Item("JobOfferId") = jobOffer.Id Select CStr(row2.Item("Location"))).ToList
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
