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

        btnStartCrawling.Enabled = False
        btnQuitCrawling.Enabled = True
        crawler.StartCrawling()

    End Sub

    Private Sub btnQuitCrawling_Click(sender As Object, e As EventArgs) Handles btnQuitCrawling.Click

        crawler.QuitCrawling()

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

            Dim compareJobOffers As List(Of JobOffer)
            Dim currentjobOffers As List(Of JobOffer)
            Dim table As New DataTable()
            Dim checkedIds As New List(Of Integer)

            currentjobOffers = Db.GetJobOffers(currentSitemap)
            compareJobOffers = Db.GetJobOffers(comboSitemapToCompare.SelectedItem)

            With table.Columns
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
                .Add("Style", GetType(String))
            End With

            gridJobOffers.DataSource = table
            gridJobOffers.Columns.Item("Style").Visible = False

            'Eigentliches vergleichen und table füllen:
            For i As Integer = 0 To currentjobOffers.Count - 1
                Dim currentJobOffer As JobOffer = currentjobOffers.Item(i)
                Dim compareJobOffer As JobOffer = (From r As JobOffer In compareJobOffers Where r.Id = currentJobOffer.Id).FirstOrDefault()
                table.Rows.Add({currentJobOffer.Id, currentJobOffer.OfferTitle, currentJobOffer.Company, currentJobOffer.getCoreAreasAsString,
                                       currentJobOffer.getFieldsOfStudyAsString, currentJobOffer.getDegreesAsString, currentJobOffer.getLocationsAsString,
                                       currentJobOffer.NiceToKnow, currentJobOffer.Description, currentJobOffer.URL, ""}.ToArray)
                Dim currentRow As DataRow = table.Rows(table.Rows.Count - 1)

                If compareJobOffer Is Nothing Then
                    'currentJobOffer ist ein JobOffer, welches nicht in compareJobOfferTable enthalten ist.
                    'Es handelt sich also um ein neues JobOffer und wird als solches markiert.
                    currentRow.Item("Style") = "0"
                Else
                    'currentJobOffer ist ein JobOffer, welches bereits in compareOfferTable enthalten ist.
                    'Es müssen also alle properties verglichen werden.
                    If Not currentJobOffer.OfferTitle.Equals(compareJobOffer.OfferTitle) Then
                        'Zelle als geändert markieren
                        currentRow.Item("Style") = currentRow.Item("Style") & "," & "1"
                    End If
                    If Not currentJobOffer.Company.Equals(compareJobOffer.Company) Then
                        'Zelle als geändert markieren
                        currentRow.Item("Style") = currentRow.Item("Style") & "," & "2"
                    End If
                    For j As Integer = 0 To currentJobOffer.CoreAreas.Count - 1
                        If currentJobOffer.CoreAreas.Count <> compareJobOffer.CoreAreas.Count OrElse Not currentJobOffer.CoreAreas(j).Equals(compareJobOffer.CoreAreas(j)) Then
                            'Zelle als geändert markieren
                            currentRow.Item("Style") = currentRow.Item("Style") & "," & "3"
                            Exit For
                        End If
                    Next
                    For k As Integer = 0 To currentJobOffer.FieldsOfStudy.Count - 1
                        If currentJobOffer.FieldsOfStudy.Count <> compareJobOffer.FieldsOfStudy.Count OrElse Not currentJobOffer.FieldsOfStudy(k).Equals(compareJobOffer.FieldsOfStudy(k)) Then
                            'Zelle als geändert markieren
                            currentRow.Item("Style") = currentRow.Item("Style") & "," & "4"
                            Exit For
                        End If
                    Next
                    For l As Integer = 0 To currentJobOffer.Degrees.Count - 1
                        If currentJobOffer.Degrees.Count <> compareJobOffer.Degrees.Count OrElse Not currentJobOffer.Degrees(l).Equals(compareJobOffer.Degrees(l)) Then
                            'Zelle als geändert markieren
                            currentRow.Item("Style") = currentRow.Item("Style") & "," & "5"
                            Exit For
                        End If
                    Next
                    For m As Integer = 0 To currentJobOffer.Locations.Count - 1
                        If currentJobOffer.Locations.Count <> compareJobOffer.Locations.Count OrElse Not currentJobOffer.Locations(m).Equals(compareJobOffer.Locations(m)) Then
                            'Zelle als geändert markieren
                            currentRow.Item("Style") = currentRow.Item("Style") & "," & "6"
                            Exit For
                        End If
                    Next
                    If Not currentJobOffer.NiceToKnow.Equals(compareJobOffer.NiceToKnow) Then
                        'Zelle als geändert markieren
                        currentRow.Item("Style") = currentRow.Item("Style") & "," & "7"
                    End If
                    If Not currentJobOffer.Description.Equals(compareJobOffer.Description) Then
                        'Zelle als geändert markieren
                        currentRow.Item("Style") = currentRow.Item("Style") & "," & "8"
                    End If
                    If Not currentJobOffer.URL.Equals(compareJobOffer.URL) Then
                        'Zelle als geändert markieren
                        currentRow.Item("Style") = currentRow.Item("Style") & "," & "9"
                    End If

                End If
                checkedIds.Add(currentJobOffer.Id)
            Next

            'TODO: schauen, welche jobOffers weggefallen sind (mit checkedIds)


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

    Public Sub QuitCrawlingFinished()

        btnStartCrawling.Enabled = True
        btnQuitCrawling.Enabled = False

    End Sub

    Private Sub gridJobOffers_CellFormatting(ByVal sender As Object, ByVal e As DataGridViewCellFormattingEventArgs) Handles gridJobOffers.CellFormatting

        If CStr(gridJobOffers.Rows(e.RowIndex).Cells("Style").Value) = "0" OrElse CStr(gridJobOffers.Rows(e.RowIndex).Cells("Style").Value).Split(",").Contains(CStr(e.ColumnIndex)) Then

            e.CellStyle.BackColor = Color.Green

        End If

    End Sub


End Class
