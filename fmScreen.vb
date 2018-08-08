Public Class fmScreen
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents picGraphic As System.Windows.Forms.PictureBox
    Friend WithEvents lblMain As gLabel.gLabel
    Friend WithEvents lblCountdown As System.Windows.Forms.Label
    Friend WithEvents lblScoreLeft As gLabel.gLabel
    Friend WithEvents lblTeamNameLeft As gLabel.gLabel
    Friend WithEvents lblTeamNameRight As gLabel.gLabel
    Friend WithEvents lblScoreRight As gLabel.gLabel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim CBlendItems1 As gLabel.cBlendItems = New gLabel.cBlendItems()
        Dim CBlendItems2 As gLabel.cBlendItems = New gLabel.cBlendItems()
        Me.picGraphic = New System.Windows.Forms.PictureBox()
        Me.lblMain = New gLabel.gLabel()
        Me.lblTeamNameLeft = New gLabel.gLabel()
        Me.lblCountdown = New System.Windows.Forms.Label()
        Me.lblScoreLeft = New gLabel.gLabel()
        Me.lblTeamNameRight = New gLabel.gLabel()
        Me.lblScoreRight = New gLabel.gLabel()
        CType(Me.picGraphic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picGraphic
        '
        Me.picGraphic.Location = New System.Drawing.Point(0, 0)
        Me.picGraphic.Name = "picGraphic"
        Me.picGraphic.Size = New System.Drawing.Size(1280, 720)
        Me.picGraphic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picGraphic.TabIndex = 0
        Me.picGraphic.TabStop = False
        '
        'lblMain
        '
        Me.lblMain.BackColor = System.Drawing.Color.Transparent
        Me.lblMain.Font = New System.Drawing.Font("Arial", 123.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMain.ForeColor = System.Drawing.Color.White
        Me.lblMain.GlowState = False
        Me.lblMain.Location = New System.Drawing.Point(0, 0)
        Me.lblMain.Name = "lblMain"
        Me.lblMain.ShadowColor = System.Drawing.Color.Black
        Me.lblMain.ShadowState = True
        Me.lblMain.Size = New System.Drawing.Size(1280, 720)
        Me.lblMain.TabIndex = 1
        Me.lblMain.Text = "Welcome to JANIS"
        '
        'lblTeamNameLeft
        '
        Me.lblTeamNameLeft.BackColor = System.Drawing.Color.Transparent
        Me.lblTeamNameLeft.Font = New System.Drawing.Font("Arial", 54.0!, System.Drawing.FontStyle.Bold)
        Me.lblTeamNameLeft.ForeColor = System.Drawing.Color.White
        Me.lblTeamNameLeft.GlowState = False
        Me.lblTeamNameLeft.Location = New System.Drawing.Point(0, 0)
        Me.lblTeamNameLeft.Name = "lblTeamNameLeft"
        Me.lblTeamNameLeft.ShadowColor = System.Drawing.Color.Black
        Me.lblTeamNameLeft.ShadowOffset = New System.Drawing.Point(4, 4)
        Me.lblTeamNameLeft.ShadowState = True
        Me.lblTeamNameLeft.Size = New System.Drawing.Size(640, 150)
        Me.lblTeamNameLeft.TabIndex = 0
        Me.lblTeamNameLeft.Visible = False
        '
        'lblCountdown
        '
        Me.lblCountdown.BackColor = System.Drawing.Color.Black
        Me.lblCountdown.Font = New System.Drawing.Font("Arial Black", 70.0!, System.Drawing.FontStyle.Bold)
        Me.lblCountdown.Location = New System.Drawing.Point(0, 600)
        Me.lblCountdown.Name = "lblCountdown"
        Me.lblCountdown.Size = New System.Drawing.Size(1280, 120)
        Me.lblCountdown.TabIndex = 7
        Me.lblCountdown.Text = "00:00:00"
        Me.lblCountdown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblCountdown.Visible = False
        '
        'lblScoreLeft
        '
        Me.lblScoreLeft.BackColor = System.Drawing.Color.Transparent
        Me.lblScoreLeft.Feather = 95
        Me.lblScoreLeft.FeatherState = False
        Me.lblScoreLeft.FillType = gLabel.gLabel.eFillType.GradientLinear
        Me.lblScoreLeft.Font = New System.Drawing.Font("Arial", 240.0!, System.Drawing.FontStyle.Bold)
        Me.lblScoreLeft.ForeColor = System.Drawing.Color.White
        CBlendItems1.iColor = New System.Drawing.Color() {System.Drawing.Color.FromArgb(CType(CType(161, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(255, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(161, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(255, Byte), Integer))}
        CBlendItems1.iPoint = New Single() {0.0!, 0.5!, 1.0!}
        Me.lblScoreLeft.ForeColorBlend = CBlendItems1
        Me.lblScoreLeft.GlowState = False
        Me.lblScoreLeft.Location = New System.Drawing.Point(0, 150)
        Me.lblScoreLeft.Name = "lblScoreLeft"
        Me.lblScoreLeft.ShadowColor = System.Drawing.Color.Black
        Me.lblScoreLeft.ShadowOffset = New System.Drawing.Point(7, 7)
        Me.lblScoreLeft.ShadowState = True
        Me.lblScoreLeft.Size = New System.Drawing.Size(640, 570)
        Me.lblScoreLeft.TabIndex = 1
        Me.lblScoreLeft.Text = "368"
        Me.lblScoreLeft.TextWordWrap = False
        Me.lblScoreLeft.Visible = False
        '
        'lblTeamNameRight
        '
        Me.lblTeamNameRight.BackColor = System.Drawing.Color.Maroon
        Me.lblTeamNameRight.Font = New System.Drawing.Font("Arial", 54.0!, System.Drawing.FontStyle.Bold)
        Me.lblTeamNameRight.ForeColor = System.Drawing.Color.White
        Me.lblTeamNameRight.GlowState = False
        Me.lblTeamNameRight.Location = New System.Drawing.Point(640, 0)
        Me.lblTeamNameRight.Name = "lblTeamNameRight"
        Me.lblTeamNameRight.ShadowColor = System.Drawing.Color.Black
        Me.lblTeamNameRight.ShadowOffset = New System.Drawing.Point(4, 4)
        Me.lblTeamNameRight.ShadowState = True
        Me.lblTeamNameRight.Size = New System.Drawing.Size(640, 150)
        Me.lblTeamNameRight.TabIndex = 2
        Me.lblTeamNameRight.Visible = False
        '
        'lblScoreRight
        '
        Me.lblScoreRight.BackColor = System.Drawing.Color.Maroon
        Me.lblScoreRight.Feather = 95
        Me.lblScoreRight.FeatherState = False
        Me.lblScoreRight.FillType = gLabel.gLabel.eFillType.GradientLinear
        Me.lblScoreRight.Font = New System.Drawing.Font("Arial", 240.0!, System.Drawing.FontStyle.Bold)
        Me.lblScoreRight.ForeColor = System.Drawing.Color.White
        CBlendItems2.iColor = New System.Drawing.Color() {System.Drawing.Color.FromArgb(CType(CType(161, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(255, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(161, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(255, Byte), Integer))}
        CBlendItems2.iPoint = New Single() {0.0!, 0.5!, 1.0!}
        Me.lblScoreRight.ForeColorBlend = CBlendItems2
        Me.lblScoreRight.GlowColor = System.Drawing.Color.Orchid
        Me.lblScoreRight.GlowState = False
        Me.lblScoreRight.Location = New System.Drawing.Point(640, 150)
        Me.lblScoreRight.Name = "lblScoreRight"
        Me.lblScoreRight.ShadowColor = System.Drawing.Color.Black
        Me.lblScoreRight.ShadowOffset = New System.Drawing.Point(7, 7)
        Me.lblScoreRight.ShadowState = True
        Me.lblScoreRight.Size = New System.Drawing.Size(640, 570)
        Me.lblScoreRight.TabIndex = 3
        Me.lblScoreRight.Text = "836"
        Me.lblScoreRight.TextWordWrap = False
        Me.lblScoreRight.Visible = False
        '
        'fmScreen
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1280, 720)
        Me.Controls.Add(Me.lblScoreRight)
        Me.Controls.Add(Me.lblTeamNameRight)
        Me.Controls.Add(Me.lblScoreLeft)
        Me.Controls.Add(Me.lblCountdown)
        Me.Controls.Add(Me.lblTeamNameLeft)
        Me.Controls.Add(Me.lblMain)
        Me.Controls.Add(Me.picGraphic)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Location = New System.Drawing.Point(1280, 0)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fmScreen"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.TopMost = True
        CType(Me.picGraphic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub fmScreen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '* Make sure the countdown timer is never behind any other controls.
        Me.lblCountdown.BringToFront()
    End Sub

    Public Sub fmScreen_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.MouseEnter, lblScoreLeft.MouseEnter, picGraphic.MouseEnter, lblTeamNameLeft.MouseEnter, lblMain.MouseEnter
        '* Prevent the cursor from moving into the fmScreen. If it does, move it to the
        '* immediate left of this form (keep Y coord). This works unbelievably well.
        System.Windows.Forms.Cursor.Position = New Point(Me.Left - 1, MousePosition.Y)
    End Sub

    Public Function MyLeft() As Integer
        '* Shareable function for public return of leftmost coordinate of this form.
        Return Me.Left
    End Function

    Public Sub Blackout()
        '* Black out the screen and turn off visible stuff

        Me.BackColor = System.Drawing.Color.Black
        Me.picGraphic.Visible = False
        Me.picGraphic.ImageLocation = ""
        Me.lblMain.Visible = False
        Me.lblScoreLeft.Visible = False
        Me.lblTeamNameLeft.Visible = False
        Me.lblScoreRight.Visible = False
        Me.lblTeamNameRight.Visible = False

    End Sub

    Public Sub ShowText(ByVal txt As String, ByVal BackColor As System.Drawing.Color, ByVal fontsize As Integer)
        Me.lblScoreLeft.Visible = False
        Me.lblTeamNameLeft.Visible = False
        Me.lblScoreRight.Visible = False
        Me.lblTeamNameRight.Visible = False
        Me.picGraphic.Visible = False
        Me.picGraphic.ImageLocation = ""
        Me.lblMain.Font = New Font(Me.lblMain.Font.Name, fontsize, Me.lblMain.Font.Style)
        'Me.lblScoreLeft.ShadowColor = Me.ColorMixer(BackColor, System.Drawing.Color.Black)
        Me.lblMain.BackColor = BackColor
        Me.lblMain.Text = txt
        Me.lblMain.Visible = True
    End Sub

    Public Sub SetSideScore(ByVal Side As String, ByVal Score As String, ByVal Team As String, ByVal BackColor As System.Drawing.Color)
        Dim scoreCBlendItems As gLabel.cBlendItems = New gLabel.cBlendItems()
        '* Mix the background, foreground colors together with a gray to make a good blending shade for the score
        Dim edgeBlendColor As System.Drawing.Color = Me.ColorMixer(BackColor, System.Drawing.Color.White, System.Drawing.Color.FromArgb(&HFF777777))

        ' Set the text gradient for the score
        scoreCBlendItems.iColor = New System.Drawing.Color() {edgeBlendColor, System.Drawing.Color.White, edgeBlendColor}
        scoreCBlendItems.iPoint = New Single() {0.0!, 0.5!, 1.0!}

        If Side <> "Right" Then
            Me.lblTeamNameLeft.BackColor = BackColor
            Me.lblScoreLeft.BackColor = BackColor
            Me.lblScoreLeft.Text = Score
            Me.lblTeamNameLeft.Text = Team
            Me.lblScoreLeft.ForeColorBlend = scoreCBlendItems
            'Me.lblScore.ShadowColor = System.Drawing.Color.FromArgb(System.Drawing.Color.Black.ToArgb And &H77FFFFFF) 'Transparent
            Me.lblScoreLeft.ShadowColor = Me.ColorMixer(BackColor, System.Drawing.Color.Black)
            Me.lblTeamNameLeft.ShadowColor = Me.lblScoreLeft.ShadowColor
        Else
            Me.lblTeamNameRight.BackColor = BackColor
            Me.lblScoreRight.BackColor = BackColor
            Me.lblScoreRight.Text = Score
            Me.lblTeamNameRight.Text = Team
            Me.lblScoreRight.ForeColorBlend = scoreCBlendItems
            'Me.lblScore.ShadowColor = System.Drawing.Color.FromArgb(System.Drawing.Color.Black.ToArgb And &H77FFFFFF) 'Transparent
            Me.lblScoreRight.ShadowColor = Me.ColorMixer(BackColor, System.Drawing.Color.Black)
            Me.lblTeamNameRight.ShadowColor = Me.lblScoreRight.ShadowColor
        End If
    End Sub

    Public Sub ShowScore()
        Me.lblMain.Visible = False
        Me.picGraphic.Visible = False
        Me.picGraphic.ImageLocation = ""

        Me.lblScoreLeft.Visible = True
        Me.lblTeamNameLeft.Visible = True
        Me.lblScoreRight.Visible = True
        Me.lblTeamNameRight.Visible = True
    End Sub

    Private Function ColorMixer(ByVal ParamArray colours() As System.Drawing.Color) As System.Drawing.Color
        ' Find the correct edge blend color in Argb between the two passed-in colors and mix in a little gray for extra contrast, to be used for Scores.
        Dim resultColor As Integer = &HFF000000        '* It's opaque, that much we know.
        Dim R As Integer = 0, G As Integer = 0, B As Integer = 0
        Dim myArgb As Integer

        If colours.Length = 0 Then Return System.Drawing.Color.Black '* If this ever happens, it's CALLER error.

        ' These are averages. Split into R G B and average them, then put them back into Argb.
        For i As Integer = 0 To UBound(colours, 1)
            myArgb = colours(i).ToArgb
            R += (myArgb And &HFF0000)
            G += (myArgb And &HFF00)
            B += (myArgb And &HFF)
        Next

        ' The following rigamarole throws away remainders. This is a good thing, because otherwise we get unexpected tints.
        ' This way we keep the color components isolated from each other.
        R = (R / colours.Length) And &HFF0000
        G = (G / colours.Length) And &HFF00
        B = (B / colours.Length) And &HFF
        resultColor += R + G + B
        Return System.Drawing.Color.FromArgb(resultColor)
    End Function

    Public Sub ShowImage(ByRef Img As Image, ByVal Expand As Boolean)
        If Img Is Nothing Then Exit Sub

        Me.BackColor = System.Drawing.Color.Black
        Me.lblScoreLeft.Visible = False
        Me.lblTeamNameLeft.Visible = False
        Me.lblScoreRight.Visible = False
        Me.lblTeamNameRight.Visible = False
        Me.lblMain.Visible = False

        If Expand Then
            Me.picGraphic.SizeMode = PictureBoxSizeMode.StretchImage
        Else
            Me.picGraphic.SizeMode = PictureBoxSizeMode.Zoom
        End If

        Me.picGraphic.Image = Img
        Me.picGraphic.Visible = True
    End Sub

    'Public Sub ShowURLImage(ByRef url As String)
    '    If url = "" Then Exit Sub

    '    Me.BackColor = System.Drawing.Color.Black
    '    Me.lblScore.Visible = False
    '    Me.lblTeamName.Visible = False
    '    Me.lblTimer.Visible = False

    '    '* when loading URL, we always maintain size ratio because we don't know pic info.
    '    Me.picGraphic.SizeMode = PictureBoxSizeMode.Zoom

    '    Try
    '        Me.picGraphic.Load(url)
    '        Me.picGraphic.Visible = True
    '    Catch ex As Exception
    '    End Try

    'End Sub

    Public Sub ShowCountdownText(ByVal CountdownText As String, ByVal BackColor As System.Drawing.Color, ByVal CountdownVisible As Boolean)
        '* Change the size of the message window to accomodate the countdown timer
        If CountdownVisible Then
            Me.lblMain.Height = Me.lblMain.Tag - Me.lblCountdown.Height
        Else
            Me.lblMain.Height = Me.lblMain.Tag
        End If

        Me.lblCountdown.BackColor = BackColor
        Me.lblCountdown.Visible = CountdownVisible
        Me.lblCountdown.Text = CountdownText
    End Sub

End Class
