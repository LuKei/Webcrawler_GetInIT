Imports System.Threading
Imports System.Net
Imports System.IO

Public Class Crawler

    Private Db As DatabaseAccess
    Private form As frmMain
    Private crawlThread As Thread
    Private ReadOnly sitemapLink As String = "https://www.get-in-it.de/sitemap.xml"

    Public Sub New(filename As String, form As frmMain)

        Db = New DatabaseAccess(filename)
        Me.form = form

    End Sub

    Public Sub StartCrawling()

        'Methode, die das eigentliche Crawling durchführt
        Dim f = Sub(argument As Object)

                    Dim myDb As DatabaseAccess = CType(argument(0), DatabaseAccess)
                    Dim myForm As frmMain = CType(argument(1), frmMain)

                    myForm.BeginInvoke(New AddInfoTextCallback(AddressOf myForm.AddInfoText), New Object() {"Sitemap wird geladen..."})

                    'Sitemap holen
                    Dim myRequest As HttpWebRequest = HttpWebRequest.Create(sitemapLink)
                    Dim myResponse As HttpWebResponse = myRequest.GetResponse()
                    Dim myStream As Stream = myResponse.GetResponseStream
                    Dim myReader As StreamReader = New StreamReader(myStream)
                    Dim mySitemapString As String = myReader.ReadToEnd()
                    myResponse.Close()

                    myForm.BeginInvoke(New AddInfoTextCallback(AddressOf myForm.AddInfoText), New Object() {"Sitemap geladen"})








                    myForm.BeginInvoke(New AddInfoTextCallback(AddressOf myForm.AddInfoText), New Object() {"Crawling beendet"})

                End Sub

        crawlThread = New Thread(f)
        crawlThread.Start(New Object() {Db, form})

    End Sub

    Delegate Sub AddInfoTextCallback([text] As String)

    Public Sub PauseCrawling()



    End Sub
End Class
