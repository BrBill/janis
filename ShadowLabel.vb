Public Class ShadowLabel
    Inherits System.Windows.Forms.Label

    Public Property ShadowState As Boolean = True
    Public Property ShadowColor As System.Drawing.Color = System.Drawing.Color.Black
    Public Property ShadowOffset As System.Drawing.Point = New System.Drawing.Point(3, 3)

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias

        Dim sf As New System.Drawing.StringFormat()
        sf.FormatFlags = System.Drawing.StringFormatFlags.LineLimit
        If Not Me.UseMnemonic Then
            sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None
        Else
            sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show
        End If

        Select Case Me.TextAlign
            Case System.Drawing.ContentAlignment.TopLeft
                sf.Alignment = System.Drawing.StringAlignment.Near
                sf.LineAlignment = System.Drawing.StringAlignment.Near
            Case System.Drawing.ContentAlignment.TopCenter
                sf.Alignment = System.Drawing.StringAlignment.Center
                sf.LineAlignment = System.Drawing.StringAlignment.Near
            Case System.Drawing.ContentAlignment.TopRight
                sf.Alignment = System.Drawing.StringAlignment.Far
                sf.LineAlignment = System.Drawing.StringAlignment.Near
            Case System.Drawing.ContentAlignment.MiddleLeft
                sf.Alignment = System.Drawing.StringAlignment.Near
                sf.LineAlignment = System.Drawing.StringAlignment.Center
            Case System.Drawing.ContentAlignment.MiddleRight
                sf.Alignment = System.Drawing.StringAlignment.Far
                sf.LineAlignment = System.Drawing.StringAlignment.Center
            Case System.Drawing.ContentAlignment.MiddleCenter
                sf.Alignment = System.Drawing.StringAlignment.Center
                sf.LineAlignment = System.Drawing.StringAlignment.Center
            Case System.Drawing.ContentAlignment.BottomLeft
                sf.Alignment = System.Drawing.StringAlignment.Near
                sf.LineAlignment = System.Drawing.StringAlignment.Far
            Case System.Drawing.ContentAlignment.BottomCenter
                sf.Alignment = System.Drawing.StringAlignment.Center
                sf.LineAlignment = System.Drawing.StringAlignment.Far
            Case System.Drawing.ContentAlignment.BottomRight
                sf.Alignment = System.Drawing.StringAlignment.Far
                sf.LineAlignment = System.Drawing.StringAlignment.Far
        End Select

        If ShadowState Then
            Dim shadowRect As New System.Drawing.Rectangle(
        Me.ClientRectangle.X + ShadowOffset.X,
        Me.ClientRectangle.Y + ShadowOffset.Y,
        Me.ClientRectangle.Width,
        Me.ClientRectangle.Height)

            Using shadowBrush As New System.Drawing.SolidBrush(ShadowColor)
                e.Graphics.DrawString(Me.Text, Me.Font, shadowBrush, shadowRect, sf)
            End Using
            Using foreBrush As New System.Drawing.SolidBrush(Me.ForeColor)
                e.Graphics.DrawString(Me.Text, Me.Font, foreBrush, Me.ClientRectangle, sf)
            End Using
        Else
            MyBase.OnPaint(e)
        End If

        sf.Dispose()
    End Sub
End Class
