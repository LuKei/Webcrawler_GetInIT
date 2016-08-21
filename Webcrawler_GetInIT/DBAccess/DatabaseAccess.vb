
Public Class DatabaseAccess

    Private connectionString As String
    Private connection As OleDb.OleDbConnection

    Public Property filename As String

    Public Sub New(filename As String)

        Me.filename = filename

        'Access File erzeugen
        Dim cat As New ADOX.Catalog()
        Dim creationString As String = "Provider=Microsoft.ACE.OLEDB.15.0;Data Source=" & filename
        cat.Create(creationString)

        connectionString = creationString & ";Persist Security Info=False;"
        Dim conn As OleDb.OleDbConnection = getConnection()

        'Try
        '    Dim cmd As New OleDb.OleDbCommand("CREATE DATABASE IF NOT EXISTS MyDb", conn)
        '    cmd.ExecuteNonQuery()
        'Catch ex As Exception
        '    'Do nothing
        'End Try

        Try
            Dim cmd As New OleDb.OleDbCommand("CREATE TABLE JobOffer(Id INTEGER PRIMARY KEY NOT NULL, OfferTitle VARCHAR(255), Company VARCHAR(255), CoreAreas VARCHAR(255),
                           FieldsOfStudy VARCHAR(255), Degrees VARCHAR(255), Locations VARCHAR(255), NiceToKnow LONGTEXT, Description LONGTEXT,
                           URL VARCHAR(255), HTML LONGTEXT)", conn)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            'Do nothing
        End Try


    End Sub

    Public Sub AddJobOffers(jobs As List(Of JobOffer))

        'TODO


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
