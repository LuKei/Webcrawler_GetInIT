Public Class Sitemap

    Private _Id As Integer
    Private _Sourcecode As String
    Private _Timestamp As DateTime

#Region "Properties"
    Public Property Id As Integer
        Get
            Return _Id
        End Get
        Set(value As Integer)
            _Id = value
        End Set
    End Property

    Public Property Sourcecode As String
        Get
            Return _Sourcecode
        End Get
        Set(value As String)
            _Sourcecode = value
        End Set
    End Property

    Public Property Timestamp As Date
        Get
            Return _Timestamp
        End Get
        Set(value As Date)
            _Timestamp = value
        End Set
    End Property
#End Region

    Public Sub New(Sourcecode As String)
        Me.Sourcecode = Sourcecode
    End Sub

End Class
