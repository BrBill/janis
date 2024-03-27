Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Threading

'Algorithm based on this C code.
'Scroll to the bottom of the page:-
'https://lodev.org/cgtutor/floodfill.html

'*****************************************************************************************
'Note:
'I use the terms "span" and "scanline" in following comments. They are NOT the same thing.
'A scanline is a single row of pixels in the image.
'A span is a set or horizontal pixels of the same colour. A scanline CAN be a span but it could also
'contain multiple spans.
'*****************************************************************************************

Public Class Gfxfast
    Public Shared Sub FloodFill(ByVal image As Bitmap, ByVal x As Integer, ByVal y As Integer, ByVal new_color As Color)

        Dim pixels As Integer() = GetPixels(image)
        Dim newClr As Integer = new_color.ToArgb()
        Dim oldClr As Integer = pixels(x + y * image.Width)
        Dim spanAbove, spanBelow As Boolean
        Dim width, height As Integer

        If newClr = oldClr Then Return

        'Very important optimization.        
        'If we instead read height and width values from the bitmap itself
        'from within the loops it will tank the performance hard.
        width = image.Width
        height = image.Height

        'Dim stack As New Stack(Of Point)(4096)
        Dim stack As New Stack(4096)

        stack.Push(New Point(x, y))

        Do Until stack.Count = 0
            Dim p As Point = stack.Pop()
            Dim x1 As Integer = p.X

            'Find the leftmost pixel of the current span that 
            'is in need of filling. This is where we start filling
            'from
            While x1 >= 0 AndAlso pixels(x1 + p.Y * width) = oldClr
                x1 -= 1
            End While

            x1 += 1

            spanAbove = False
            spanBelow = False

            While x1 < width AndAlso pixels(x1 + p.Y * width) = oldClr

                'Start filling
                pixels(x1 + p.Y * width) = newClr

                'Check above the current pixel for a fillable pixel
                If Not spanAbove AndAlso p.Y > 0 AndAlso pixels(x1 + (p.Y - 1) * width) = oldClr Then

                    stack.Push(New Point(x1, p.Y - 1))
                    spanAbove = True

                    'If we encounter an unfillable pixel above we set spanAbove to False to allow
                    'us to add another span on this same scanline if we eventually encounter
                    'a fillable pixel. This is what allows us to fill around islands and other
                    'complex arrangements
                ElseIf spanAbove AndAlso p.Y > 0 AndAlso pixels(x1 + (p.Y - 1) * width) <> oldClr Then

                    spanAbove = False

                End If

                'Check below the current pixel for a fillable pixel
                If Not spanBelow AndAlso p.Y < height - 1 AndAlso pixels(x1 + (p.Y + 1) * width) = oldClr Then

                    stack.Push(New Point(x1, p.Y + 1))
                    spanBelow = True

                    'If we encounter an unfillable pixel below we set spanBelow to False to allow
                    'us to add another span on this same scanline if we eventually encounter
                    'a fillable pixel. This is what allows us to fill around islands and other
                    'complex arrangements
                ElseIf spanBelow AndAlso p.Y < height - 1 AndAlso pixels(x1 + (p.Y + 1) * width) <> oldClr Then

                    spanBelow = False

                End If

                x1 += 1
            End While
        Loop

        'Write the changed pixels back to the image
        WritePixels(image, pixels)
    End Sub

    Private Shared Function GetPixels(ByVal bm As Bitmap) As Integer()

        Dim bmData As BitmapData = bm.LockBits(New Rectangle(0, 0, bm.Width, bm.Height),
                                               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb)

        'For best performance we treat the image as an array of 32 bit Integers
        Dim pixels As Integer() = New Integer((bm.Width * bm.Height) - 1) {}

        Marshal.Copy(bmData.Scan0, pixels, 0, pixels.Length)

        bm.UnlockBits(bmData)

        Return pixels
    End Function

    Private Shared Sub WritePixels(ByVal bm As Bitmap, ByVal pixels As Integer())

        Dim bmData As BitmapData = bm.LockBits(New Rectangle(0, 0, bm.Width, bm.Height),
                             ImageLockMode.ReadWrite,
                             PixelFormat.Format32bppArgb)

        Marshal.Copy(pixels, 0, bmData.Scan0, pixels.Length)

        bm.UnlockBits(bmData)
    End Sub

End Class
