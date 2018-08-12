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
    Friend WithEvents lblMsg As gLabel.gLabel
    Friend WithEvents lblCountdown As System.Windows.Forms.Label
    Friend WithEvents lblScore As gLabel.gLabel
    Friend WithEvents GLabel1 As gLabel.gLabel
    Friend WithEvents lblTeamName As gLabel.gLabel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim CBlendItems1 As gLabel.cBlendItems = New gLabel.cBlendItems()
        Me.picGraphic = New System.Windows.Forms.PictureBox()
        Me.lblMsg = New gLabel.gLabel()
        Me.lblTeamName = New gLabel.gLabel()
        Me.lblCountdown = New System.Windows.Forms.Label()
        Me.lblScore = New gLabel.gLabel()
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
        'lblMsg
        '
        Me.lblMsg.BackColor = System.Drawing.Color.Transparent
        Me.lblMsg.Font = New System.Drawing.Font("Arial", 123.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMsg.ForeColor = System.Drawing.Color.White
        Me.lblMsg.GlowState = False
        Me.lblMsg.Location = New System.Drawing.Point(0, 0)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.ShadowColor = System.Drawing.Color.Black
        Me.lblMsg.ShadowState = True
        Me.lblMsg.Size = New System.Drawing.Size(1280, 720)
        Me.lblMsg.TabIndex = 1
        Me.lblMsg.Text = "Welcome to JANIS"
        '
        'lblTeamName
        '
        Me.lblTeamName.BackColor = System.Drawing.Color.Transparent
        Me.lblTeamName.Font = New System.Drawing.Font("Arial", 54.0!, System.Drawing.FontStyle.Bold)
        Me.lblTeamName.ForeColor = System.Drawing.Color.White
        Me.lblTeamName.GlowState = False
        Me.lblTeamName.Location = New System.Drawing.Point(0, 0)
        Me.lblTeamName.Name = "lblTeamName"
        Me.lblTeamName.ShadowColor = System.Drawing.Color.Black
        Me.lblTeamName.ShadowOffset = New System.Drawing.Point(4, 4)
        Me.lblTeamName.ShadowState = True
        Me.lblTeamName.Size = New System.Drawing.Size(1280, 150)
        Me.lblTeamName.TabIndex = 3
        Me.lblTeamName.Visible = False
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
        'lblScore
        '
        Me.lblScore.BackColor = System.Drawing.Color.Transparent
        Me.lblScore.Feather = 95
        Me.lblScore.FeatherState = False
        Me.lblScore.FillType = gLabel.gLabel.eFillType.GradientLinear
        Me.lblScore.Font = New System.Drawing.Font("Arial", 360.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScore.ForeColor = System.Drawing.Color.White
        CBlendItems1.iColor = New System.Drawing.Color() {System.Drawing.Color.FromArgb(CType(CType(161, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(255, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(161, Byte), Integer), CType(CType(161, Byte), Integer), CType(CType(255, Byte), Integer))}
        CBlendItems1.iPoint = New Single() {0.0!, 0.5!, 1.0!}
        Me.lblScore.ForeColorBlend = CBlendItems1
        Me.lblScore.GlowState = False
        Me.lblScore.Location = New System.Drawing.Point(0, 150)
        Me.lblScore.Name = "lblScore"
        Me.lblScore.ShadowColor = System.Drawing.Color.Black
        Me.lblScore.ShadowOffset = New System.Drawing.Point(10, 10)
        Me.lblScore.ShadowState = True
        Me.lblScore.Size = New System.Drawing.Size(1280, 570)
        Me.lblScore.TabIndex = 2
        Me.lblScore.Text = "-68"
        Me.lblScore.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblScore.TextWordWrap = False
        Me.lblScore.Visible = False
        '
        'fmScreen
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1280, 720)
        Me.Controls.Add(Me.lblScore)
        Me.Controls.Add(Me.lblCountdown)
        Me.Controls.Add(Me.lblTeamName)
        Me.Controls.Add(Me.lblMsg)
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
    End Sub

    Public Sub fmScreen_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.MouseEnter, lblScore.MouseEnter, picGraphic.MouseEnter, lblTeamName.MouseEnter, lblMsg.MouseEnter
        '* Prevent the cursor from moving into the fmScreen. If it does, move it to the
        '* immediate left of this form (keep Y coord). This used to work in XP and VB 2005. I don't think it works now.
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
        Me.lblMsg.Visible = False
        Me.lblScore.Visible = False
        Me.lblTeamName.Visible = False
    End Sub

    Public Sub SetTextShadows(ByVal UseShadows As Boolean)
        Me.lblMsg.ShadowState = UseShadows
    End Sub

    Public Sub ShowText(ByVal txt As String, ByVal BackColor As System.Drawing.Color, ByVal fontsize As Integer)
        Me.lblScore.Visible = False
        Me.lblTeamName.Visible = False
        Me.picGraphic.Visible = False
        Me.picGraphic.ImageLocation = ""
        Me.lblMsg.Font = New Font(Me.lblMsg.Font.Name, fontsize, Me.lblMsg.Font.Style)
        Me.lblScore.ShadowColor = Me.ColorMixer(BackColor, System.Drawing.Color.Black)
        Me.lblMsg.BackColor = BackColor
        Me.lblMsg.Text = txt
        Me.lblMsg.Visible = True
    End Sub

    Public Sub ShowScore(ByVal Score As String, ByVal Team As String, ByVal BackColor As System.Drawing.Color)
        Dim scoreCBlendItems As gLabel.cBlendItems = New gLabel.cBlendItems()
        '* Mix the background, foreground colors together with a gray to make a good blending shade for the score
        Dim edgeBlendColor As System.Drawing.Color = Me.ColorMixer(BackColor, System.Drawing.Color.White, System.Drawing.Color.FromArgb(&HFF777777))

        Me.lblTeamName.BackColor = BackColor
        Me.lblScore.BackColor = BackColor
        Me.lblMsg.Visible = False
        Me.picGraphic.Visible = False
        Me.picGraphic.ImageLocation = ""
        Me.lblScore.Text = Score
        Me.lblTeamName.Text = Team
        '
        ' Set the text gradient for the score
        scoreCBlendItems.iColor = New System.Drawing.Color() {edgeBlendColor, System.Drawing.Color.White, edgeBlendColor}
        scoreCBlendItems.iPoint = New Single() {0.0!, 0.5!, 1.0!}
        Me.lblScore.ForeColorBlend = scoreCBlendItems
        'Me.lblScore.ShadowColor = System.Drawing.Color.FromArgb(System.Drawing.Color.Black.ToArgb And &H77FFFFFF) 'Transparent
        Me.lblScore.ShadowColor = Me.ColorMixer(BackColor, System.Drawing.Color.Black)
        Me.lblTeamName.ShadowColor = Me.lblScore.ShadowColor
        Me.lblScore.Visible = True
        Me.lblTeamName.Visible = True
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
        Me.lblScore.Visible = False
        Me.lblTeamName.Visible = False
        Me.lblMsg.Visible = False

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
    '    Me.lblMsg.Visible = False

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
            Me.lblMsg.Height = Me.lblMsg.Tag - Me.lblCountdown.Height
        Else
            Me.lblMsg.Height = Me.lblMsg.Tag
        End If

        Me.lblCountdown.BackColor = BackColor
        Me.lblCountdown.Visible = CountdownVisible
        Me.lblCountdown.Text = CountdownText
    End Sub

End Class
