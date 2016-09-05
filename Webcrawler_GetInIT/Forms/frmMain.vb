Public Class frmMain



    Public Property filename As String
    Public Property crawler As Crawler
    Private currentSitemap As Sitemap
    Private sitemapToCompare As Sitemap
    Private Db As DatabaseAccess



    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim myFrm As New frmStart()
        myFrm.ShowDialog(Me)

        Db = New DatabaseAccess(filename)
        crawler = New Crawler(Db, Me)

        RefreshGrid()

    End Sub

    Private Sub btnStartCrawling_Click(sender As Object, e As EventArgs) Handles btnStartCrawling.Click

        crawler.StartCrawling()

    End Sub

    Private Sub btnPauseCrawling_Click(sender As Object, e As EventArgs) Handles btnPauseCrawling.Click

        crawler.PauseCrawling()

    End Sub


    Public Sub AddInfoText(text As String)
        tbInfo.Text += vbCrLf & DateTime.Now.ToLongTimeString & ": " & text
    End Sub

    Private Sub tbInfo_TextChanged(sender As Object, e As EventArgs) Handles tbInfo.TextChanged
        'Aktuelle caret position auf Ende des Textes setzen
        tbInfo.SelectionStart = tbInfo.Text.Length
        'Zum caret scrollen
        tbInfo.ScrollToCaret()
    End Sub

    Private Sub RefreshcomboSitemapToCompare()

        comboSitemapToCompare.Items.Clear()

        Dim sitemaps As List(Of Sitemap) = Db.getSitemaps()
        'Alle Sitemaps, außer die neuste einfügen
        For i As Integer = 0 To sitemaps.Count - 2
            comboSitemapToCompare.Items.Add(sitemaps(i))
        Next

    End Sub


    Public Sub RefreshGrid()

        RefreshcomboSitemapToCompare()
        currentSitemap = Db.getLastSitemap()
        If comboSitemapToCompare.Items.Count > 0 AndAlso Not currentSitemap Is Nothing Then
            If comboSitemapToCompare.SelectedItem Is Nothing Then
                comboSitemapToCompare.SelectedIndex = 0
            End If

            'Die neusten JobOffers mit den zu der in der comboBox ausgewählten Sitemap gehörenden Joboffers vergleichen

            'DataTables füllen:
            Dim compareJobOfferTable As New DataTable
            Dim currentjobOfferTable As DataTable

            With compareJobOfferTable.Columns
                .Add("Id", GetType(Integer))
                .Add("OfferTitle", GetType(String))
                .Add("Company", GetType(String))
                .Add("CoreAreas", GetType(String))
                .Add("FieldsOfStudy", GetType(String))
                .Add("Degrees", GetType(String))
                .Add("Locations", GetType(String))
                .Add("NiceToKnow", GetType(String))
                .Add("Description", GetType(String))
                .Add("URL", GetType(String))
                .Add("HTML", GetType(String))
            End With

            For Each job In Db.getJobOffers(currentSitemap)
                With compareJobOfferTable.Rows
                    .Add({job.Id, job.OfferTitle, job.Company, job.CoreAreas, job.FieldsOfStudy, job.Degrees, job.Locations,
                         job.NiceToKnow, job.Description, job.URL, job.HTML})
                End With
            Next



            currentjobOfferTable = compareJobOfferTable.Clone()

            For Each job In Db.getJobOffers(comboSitemapToCompare.SelectedItem)
                With currentjobOfferTable.Rows
                    .Add({job.Id, job.OfferTitle, job.Company, job.CoreAreas, job.FieldsOfStudy, job.Degrees, job.Locations,
                         job.NiceToKnow, job.Description, job.URL, job.HTML})
                End With
            Next


            'TODO:
            'Eigentliches vergleichen:



        End If


    End Sub

    Private Sub frmMain_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        Db.Close()
    End Sub

    Private Sub comboSitemapToCompare_SelectedIndexChanged(sender As Object, e As EventArgs) Handles comboSitemapToCompare.SelectedIndexChanged

        If comboSitemapToCompare.Items.Count > 0 Then
            sitemapToCompare = CType(comboSitemapToCompare.SelectedItem, Sitemap)
        End If

    End Sub
End Class
