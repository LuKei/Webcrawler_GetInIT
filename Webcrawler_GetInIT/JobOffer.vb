Public Class JobOffer

    Private _Id As Integer
    Private _OfferTitle As String
    Private _Company As String
    Private _CoreAreas As List(Of String)
    Private _FieldsOfStudy As List(Of String)
    Private _Degrees As List(Of String)
    Private _Locations As List(Of String)
    Private _NiceToKnow As String
    Private _Description As String
    Private _URL As String
    Private _HTML As String
    Private _Timestamp As DateTime

#Region "Properties"
    Public Property OfferTitle As String
        Get
            Return _OfferTitle
        End Get
        Set(value As String)
            _OfferTitle = value
        End Set
    End Property

    Public Property Company As String
        Get
            Return _Company
        End Get
        Set(value As String)
            _Company = value
        End Set
    End Property

    Public Property CoreAreas As List(Of String)
        Get
            Return _CoreAreas
        End Get
        Set(value As List(Of String))
            _CoreAreas = value
        End Set
    End Property

    Public Property FieldsOfStudy As List(Of String)
        Get
            Return _FieldsOfStudy
        End Get
        Set(value As List(Of String))
            _FieldsOfStudy = value
        End Set
    End Property

    Public Property Degrees As List(Of String)
        Get
            Return _Degrees
        End Get
        Set(value As List(Of String))
            _Degrees = value
        End Set
    End Property

    Public Property Locations As List(Of String)
        Get
            Return _Locations
        End Get
        Set(value As List(Of String))
            _Locations = value
        End Set
    End Property

    Public Property URL As String
        Get
            Return _URL
        End Get
        Set(value As String)
            _URL = value
        End Set
    End Property

    Public Property NiceToKnow As String
        Get
            Return _NiceToKnow
        End Get
        Set(value As String)
            _NiceToKnow = value
        End Set
    End Property

    Public Property Description As String
        Get
            Return _Description
        End Get
        Set(value As String)
            _Description = value
        End Set
    End Property

    Property HTML As String
        Get
            Return _HTML
        End Get
        Set(value As String)
            _HTML = value
        End Set
    End Property

    Public Property Timestamp As DateTime
        Get
            Return _Timestamp
        End Get
        Set(value As DateTime)
            _Timestamp = value
        End Set
    End Property

    Public Property Id As Integer
        Get
            Return _Id
        End Get
        Set(value As Integer)
            _Id = value
        End Set
    End Property
#End Region

    Public Sub New()
        Me.CoreAreas = New List(Of String)
        Me.FieldsOfStudy = New List(Of String)
        Me.Degrees = New List(Of String)
        Me.Locations = New List(Of String)
    End Sub



    Public Sub New(OfferTitle As String, Company As String, CoreAreas As List(Of String), FieldsOfStudy As List(Of String),
                   Degrees As List(Of String), Locations As List(Of String), NiceToKnow As String, Description As String, URL As String, HTML As String)
        Me.OfferTitle = OfferTitle
        Me.Company = Company
        Me.CoreAreas = CoreAreas
        Me.FieldsOfStudy = FieldsOfStudy
        Me.Degrees = Degrees
        Me.Locations = Locations
        Me.NiceToKnow = NiceToKnow
        Me.Description = Description
        Me.URL = URL
        Me.HTML = HTML
    End Sub

    Public Function getCoreAreasAsString() As String

        Dim areas As String = ""
        If Me.CoreAreas.Count > 0 Then
            For Each area In Me.CoreAreas
                areas += area & ", "
            Next
            areas = areas.Remove(areas.Length - 2)
        End If
        Return areas

    End Function

    Public Function getFieldsOfStudyAsString() As String

        Dim fields As String = ""
        If Me.FieldsOfStudy.Count > 0 Then
            For Each field In Me.FieldsOfStudy
                fields += field & ", "
            Next
            fields = fields.Remove(fields.Length - 2)
        End If
        Return fields

    End Function

    Public Function getDegreesAsString() As String

        Dim degrees As String = ""
        If Me.Degrees.Count > 0 Then
            For Each degree In Me.Degrees
                degrees += degree & ", "
            Next
            degrees = degrees.Remove(degrees.Length - 2)
        End If
        Return degrees

    End Function


    Public Function getLocationsAsString() As String

        Dim locations As String = ""
        If Me.Locations.Count > 0 Then
            For Each location In Me.Locations
                locations += location & ", "
            Next
            locations = locations.Remove(locations.Length - 2)
        End If
        Return locations

    End Function
End Class
