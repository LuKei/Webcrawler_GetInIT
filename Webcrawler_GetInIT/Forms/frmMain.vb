Public Class frmMain



    Public Property filename As String
    Public Property crawler As Crawler
    Private currentSitemap As Sitemap
    Private Db As DatabaseAccess



    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim myFrm As New frmStart()
        myFrm.ShowDialog(Me)

        Db = New DatabaseAccess(filename)
        crawler = New Crawler(Db, Me)


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

    Private Sub RefreshcombSitemapToCompare()

        comboSitemapToCompare.Items.Clear()

        Dim sitemaps As List(Of Sitemap) = Db.getSitemaps()
        For Each sitemap In sitemaps
            'TODO: Item richtig einfügen
            comboSitemapToCompare.Items.Add(sitemap)
        Next

    End Sub


    Public Sub RefreshGrid()
        'TODO: aktuelle joboffers mit letzten jobOffers vergleichen
        RefreshcombSitemapToCompare()
        comboSitemapToCompare.SelectedIndex = comboSitemapToCompare.Items.Count - 1
    End Sub
End Class
