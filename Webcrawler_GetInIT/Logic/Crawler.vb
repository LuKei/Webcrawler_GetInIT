Imports System.Threading

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

        Dim f = Sub(argument As Object)

                    Dim myDb As DatabaseAccess = CType(argument(0), DatabaseAccess)
                    Dim myForm As frmMain = CType(argument(1), frmMain)




                End Sub

        crawlThread = New Thread(f)
        crawlThread.Start(New Object() {Db, form})

    End Sub

    Public Sub PauseCrawling()



    End Sub
End Class
