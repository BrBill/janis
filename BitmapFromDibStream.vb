Imports System.IO

Namespace JANIS
    Public Class BitmapFromDibStream
        '* JANIS uses this to allow images from browsers to be drag-dropped into JANIS image display.
        Inherits Stream
        Private dib As Stream = Nothing
        Private header As Byte() = Nothing

        Public Sub New(ByVal dib As Stream)
            Me.dib = dib
            makeHeader()
        End Sub

        Private Sub makeHeader()
            Using reader As New BinaryReader(dib, System.Text.Encoding.Default, leaveOpen:=True)
                Dim headerSize As Integer = reader.ReadInt32()
                Dim pixelSize As Integer = CInt(dib.Length) - headerSize
                Dim fileSize As Integer = 14 + headerSize + pixelSize

                'Dim bmp As New MemoryStream(14)
                'Dim writer As New BinaryWriter(bmp)

                ' Get the palette size
                '                   * The Palette size is stored as an int32 at offset 32
                '                   * Actually stored as number of colours, so multiply by 4
                '                   
                dib.Position = 32
                Dim paletteSize As Integer = 4 * reader.ReadInt32()

                ' Get the palette size from the bbp if none was specified
                If paletteSize = 0 Then
                    ' Get the bits per pixel
                    ' The bits per pixel is stored as an int16 at offset 14
                    '                     
                    dib.Position = 14
                    Dim bpp As Integer = reader.ReadInt16()

                    ' Only set the palette size if the bpp < 16
                    If bpp < 16 Then
                        paletteSize = 4 * (2 << (bpp - 1))
                    End If
                End If

                Using bmp As New MemoryStream(14)
                    Using writer As New BinaryWriter(bmp, System.Text.Encoding.Default, leaveOpen:=True)
                        writer.Write(CByte(AscW("B"c)))
                        writer.Write(CByte(AscW("M"c)))
                        writer.Write(fileSize)
                        writer.Write(CInt(0))
                        writer.Write(14 + headerSize + paletteSize)
                    End Using
                    header = bmp.GetBuffer()
                End Using
            End Using
            dib.Position = 0
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If dib IsNot Nothing Then
                    dib.Dispose()
                    dib = Nothing
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        Public Overrides Function Read(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer) As Integer

            Dim dibCount As Integer = count
            Dim dibOffset As Integer = offset - 14
            Dim result As Integer = 0
            If _position < 14 Then
                Dim headerCount As Integer = Math.Min(count + CInt(_position), 14)
                Array.Copy(header, _position, buffer, offset, headerCount)
                dibCount -= headerCount
                _position += headerCount
                result = headerCount
            End If
            If _position >= 14 Then
                result += dib.Read(buffer, offset + result, dibCount)
                _position = 14 + dib.Position
            End If
            Return CInt(result)
        End Function
        Public Overrides ReadOnly Property CanRead() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property CanSeek() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property CanWrite() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides Sub Flush()

        End Sub

        Public Overrides ReadOnly Property Length() As Long
            Get
                Return 14 + dib.Length
            End Get
        End Property

        Private _position As Long = 0
        Public Overrides Property Position() As Long
            Get
                Return _position
            End Get
            Set(ByVal value As Long)
                _position = value
                If _position > 14 Then
                    dib.Position = _position - 14
                End If
            End Set
        End Property

        Public Overrides Function Seek(ByVal offset As Long, ByVal origin As SeekOrigin) As Long
            Throw New Exception("The method or operation is not implemented.")
        End Function

        Public Overrides Sub SetLength(ByVal value As Long)
            Throw New Exception("The method or operation is not implemented.")
        End Sub

        Public Overrides Sub Write(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer)
            Throw New Exception("The method or operation is not implemented.")
        End Sub
    End Class
End Namespace