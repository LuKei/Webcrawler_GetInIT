Public Class frmMain



    Public Property filename As String
    Public crawler As Crawler



    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim myFrm As New frmStart()
        myFrm.ShowDialog(Me)

        crawler = New Crawler(filename, Me)


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
End Class
