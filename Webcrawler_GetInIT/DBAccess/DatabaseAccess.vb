Imports System.IO
Imports System.Data.OleDb

Public Class DatabaseAccess

    Private connectionString As String
    Private connection As OleDbConnection

    Public Property filename As String

    Public Sub New(filename As String)

        Me.filename = filename

        Dim creationString As String = "Provider=Microsoft.ACE.OLEDB.15.0;Data Source=" & filename
        connectionString = creationString & ";Persist Security Info=False;"
        'Access File erzeugen, wenn noch nicht vorhanden
        If Not File.Exists(filename) Then
            Dim cat As New ADOX.Catalog()
            cat.Create(creationString)
        End If


        Dim conn As OleDbConnection = getConnection()


        Try
            Dim cmd As New OleDbCommand("CREATE TABLE JobOffer(Id INTEGER PRIMARY KEY NOT NULL, OfferTitle VARCHAR(255), Company VARCHAR(255), CoreAreas VARCHAR(255),
                           FieldsOfStudy VARCHAR(255), Degrees VARCHAR(255), Locations VARCHAR(255), NiceToKnow LONGTEXT, Description LONGTEXT,
                           URL VARCHAR(255), HTML LONGTEXT)", conn)
            cmd.ExecuteNonQuery()
        Catch ex As OleDbException
            If ex.ErrorCode = -2147217900 Then
                'Do nothing, table already exists
            Else
                Throw ex
            End If
        End Try


    End Sub

    Public Sub AddJobOffers(jobs As List(Of JobOffer))

        'TODO

        Dim conn As OleDbConnection = getConnection()


    End Sub

    Private Function getConnection() As OleDb.OleDbConnection
        If connection Is Nothing Then
            connection = New OleDb.OleDbConnection(connectionString)
        End If

        If connection.State = ConnectionState.Closed Then
            connection.Open()
        End If

        Return connection
    End Function


End Class
