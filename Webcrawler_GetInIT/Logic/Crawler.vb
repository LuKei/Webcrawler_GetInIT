Imports System.Threading
Imports System.Net
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports Sgml

Public Class Crawler

    Private Db As DatabaseAccess
    Private form As frmMain
    Private crawlThread As Thread
    Private quit As Boolean = False
    Private ReadOnly sitemapLink As String = "https://www.get-in-it.de/sitemap.xml"

    Public Sub New(Db As DatabaseAccess, form As frmMain)

        Me.Db = Db
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
                    Dim jobOfferUrl As String
                    Dim mySitemapString As String = myReader.ReadToEnd()
                    myResponse.Close()

                    'Sitemap String in Datenbank speichern
                    Dim sitemapId As Integer
                    sitemapId = myDb.AddSitemap(New Sitemap(mySitemapString))
                    If sitemapId < 0 Then
                        myForm.BeginInvoke(New AddInfoTextCallback(AddressOf myForm.AddInfoText), New Object() {"Fehler beim Laden der Sitemap"})
                        Return
                    End If

                    myForm.BeginInvoke(New AddInfoTextCallback(AddressOf myForm.AddInfoText), New Object() {"Sitemap geladen"})


                    Dim myXmlReader As XmlReader = XmlReader.Create(New StringReader(mySitemapString))
                    Dim mySgmlReader As SgmlReader = New SgmlReader()
                    mySgmlReader.DocType = "HTML"
                    mySgmlReader.WhitespaceHandling = WhitespaceHandling.All
                    Dim myXmlDocument As XmlDocument
                    Dim myXPathNavigator As XPathNavigator
                    Dim items As XPathNodeIterator
                    Dim lastNodeName As String = ""
                    Dim isJobOffer As Boolean = False
                    Dim jobOffers As New List(Of JobOffer)
                    Dim jobOffer As JobOffer
                    Dim jobOfferHtmlString As String
                    Do While myXmlReader.Read()
                        If quit Then
                            'Crawling abbrechen
                            quit = False
                            myXmlReader.Close()
                            mySgmlReader.Close()
                            myForm.BeginInvoke(New AddInfoTextCallback(AddressOf myForm.AddInfoText), New Object() {"Crawling abgebrochen"})
                            myForm.BeginInvoke(New QuitCrawlingCallback(AddressOf myForm.QuitCrawlingFinished))
                            Return
                        End If

                        If myXmlReader.NodeType = XmlNodeType.Element Then
                            lastNodeName = myXmlReader.Name
                        ElseIf myXmlReader.NodeType = Xml.XmlNodeType.Text Then
                            Select Case lastNodeName
                                Case "loc"
                                    If myXmlReader.Value.Contains("https://www.get-in-it.de/it-einstiegsprogramme/p") Then
                                        jobOffer = New JobOffer()
                                        'HTML der Jobangebotsseite als String holen
                                        jobOfferUrl = myXmlReader.Value
                                        myRequest = HttpWebRequest.Create(jobOfferUrl)
                                        myResponse = myRequest.GetResponse()
                                        myStream = myResponse.GetResponseStream()
                                        myReader = New StreamReader(myStream)
                                        jobOfferHtmlString = myReader.ReadToEnd()
                                        myResponse.Close()
                                        'Ein Xml-Document mit Hilfe des SgmlReaders erstellen, um qualitative Mängel des HTML-Files, z.B. nichtgeschlossene Tags, zu beheben.
                                        mySgmlReader.InputStream = New StringReader(jobOfferHtmlString)
                                        myXmlDocument = New XmlDocument()
                                        myXmlDocument.Load(mySgmlReader)
                                        'Mit dem XPathNavigator durch das HTML/XML-Dokument navigieren und die gesuchten Attribute heraussuchen
                                        myXPathNavigator = New XPathDocument(New StringReader(myXmlDocument.OuterXml)).CreateNavigator()
                                        'Titel auslesen
                                        items = myXPathNavigator.Select("//div[@id=""pageContainer""]/div[@class=""fancyPageHeader overlap""]/div[@class=""pageRow""]/div[@class=""teaserBox invert""]/h1")
                                        If items.Count < 1 Then
                                            items = myXPathNavigator.Select("//div[@id=""pageContainer""]/div[@class=""pageRow""]/div[@class=""teaserBox""]/h1")
                                        End If
                                        jobOffer.OfferTitle = items(0).Value
                                        'Unternehmensname auslesen
                                        items = myXPathNavigator.Select("//div[@id=""pageContainer""]/div[@class=""fancyPageHeader overlap""]/div[@class=""pageRow""]/div[@class=""teaserBox invert""]/p[@class=""subheading""]/span/a[@href]")
                                        If items.Count < 1 Then
                                            items = myXPathNavigator.Select("//div[@id=""pageContainer""]/div[@class=""pageRow""]/div[@class=""teaserBox""]/p[@class=""subheading""]/span/a[@href]")
                                        End If
                                        jobOffer.Company = items(0).Value
                                        jobOffer.Company = jobOffer.Company.Trim()
                                        'NiceToKnow auslesen
                                        items = myXPathNavigator.Select("//div[@class=""further-information""]/ul/li")
                                        For Each item In items
                                            jobOffer.NiceToKnow += item.value & vbCrLf
                                        Next
                                        jobOffer.NiceToKnow = If(Not jobOffer.NiceToKnow Is Nothing, jobOffer.NiceToKnow.Trim(), Nothing)

                                        'Schwerpunkte auslesen
                                        items = myXPathNavigator.Select("//div[@class=""scoop thematic-priorities""]/ul/li")
                                        For Each item In items
                                            jobOffer.CoreAreas.Add(item.Value)
                                        Next
                                        'Gewünschte Studienfächer auslesen
                                        items = myXPathNavigator.Select("//div[@class=""scoop study-subjects""]/ul/li")
                                        For Each item In items
                                            jobOffer.FieldsOfStudy.Add(item.Value)
                                        Next
                                        'Abschlüsse auslesen
                                        items = myXPathNavigator.Select("//div[@class=""scoop degree""]/ul/li")
                                        For Each item In items
                                            jobOffer.Degrees.Add(item.Value)
                                        Next
                                        'Einsatzorte auslesen
                                        items = myXPathNavigator.Select("//div[@class=""scoop locations""]/ul/li")
                                        For Each item In items
                                            jobOffer.Locations.Add(item.Value)
                                        Next
                                        'Stellenbeschreibung auslesen (als HTML)
                                        jobOffer.Description = myXmlDocument.SelectSingleNode("//div[@id=""job_description""]").InnerXml

                                        jobOffer.URL = jobOfferUrl
                                        jobOffer.Id = CInt(jobOfferUrl.Replace("https://www.get-in-it.de/it-einstiegsprogramme/p", ""))
                                        jobOffer.HTML = jobOfferHtmlString



                                        isJobOffer = True
                                    Else
                                        isJobOffer = False
                                    End If
                                Case "lastmod"
                                    'JobOffer mit Zeitstempel aus der Sitemap versehen
                                    If isJobOffer Then
                                        jobOffer.Timestamp = myXmlReader.Value
                                    End If
                                Case "priority"
                                    'TODO: Ist die priority relevant? Was gibt diese an???
                                    If isJobOffer Then
                                        jobOffers.Add(jobOffer)
                                        isJobOffer = False
                                        myForm.BeginInvoke(New AddInfoTextCallback(AddressOf myForm.AddInfoText), New Object() {"Jobangebot mit der Id " & jobOffer.Id & " erfasst"})
                                    End If
                            End Select
                        End If
                    Loop


                    'Alle JobOffers in die Datenbank einfügen
                    myDb.AddJobOffers(jobOffers, sitemapId)
                    myForm.BeginInvoke(New AddInfoTextCallback(AddressOf myForm.AddInfoText), New Object() {jobOffers.Count & " Jobangebote in der Datenbank gespeichert"})




                    myForm.BeginInvoke(New AddInfoTextCallback(AddressOf myForm.AddInfoText), New Object() {"Crawling beendet"})


                    myXmlReader.Close()
                    mySgmlReader.Close()

                    myForm.BeginInvoke(New RefreshGridCallback(AddressOf myForm.RefreshGrid))


                End Sub

        crawlThread = New Thread(f)
        crawlThread.Start(New Object() {Db, form})

    End Sub

    Delegate Sub AddInfoTextCallback([text] As String)

    Delegate Sub RefreshGridCallback()

    Delegate Sub QuitCrawlingCallback()

    Public Sub QuitCrawling()

        quit = True

    End Sub
End Class
