Public Class SharedImage
    '* A Class for handling reference-counted Image objects in VB.NET

    Private _image As Image
    Private _refCount As Integer = 0

    Public Sub New(img As Image)
        If img Is Nothing Then Throw New ArgumentNullException("img")
        _image = img
    End Sub

    Public Function Acquire() As Image
        _refCount += 1
        Return _image
    End Function

    Public Sub Release()
        _refCount -= 1
        If _refCount < 1 Then
            _image.Dispose()
            _image = Nothing
        End If
    End Sub

    Public ReadOnly Property Image As Image
        Get
            Return _image
        End Get
    End Property

    Public ReadOnly Property IsDisposed As Boolean
        Get
            Return _image Is Nothing
        End Get
    End Property
End Class
