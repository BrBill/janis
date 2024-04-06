Public Class fmScreen
    Inherits System.Windows.Forms.Form

    Private ScoreboardBitMap As Bitmap = Global.JANIS.My.Resources.ScoreTemplate.Clone
    Private ScoreboardColorAnchorLeft As Point = New Point(70, 110)    ' These are the scoreboard background locations that
    Private ScoreboardColorAnchorRight As Point = New Point(1210, 110) ' get flood-filled when team color changes
    Private LeftTeamColor As System.Drawing.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
    Private RightTeamColor As System.Drawing.Color = System.Drawing.Color.Maroon
    Private SCORE_TEXT_COLOR As System.Drawing.Color = System.Drawing.Color.White   '* If this gets changed a lot of score fading stuff will break
    Private LeftFadeIncrements() As Integer = {0, 16, 16, 5}     '* A R G B (for base default blue)   If any of these had to decrease to reach the goal color,
    Private RightFadeIncrements() As Integer = {0, 8, 16, 16}    '* A R G B (for base default red)    the numbers would be negative, but that would break AddColorIncrement()

    Const MAX_SCORE As Integer = 999
    Friend WithEvents FadeTimer As System.Windows.Forms.Timer
    Const MIN_SCORE As Integer = -99

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
    Friend WithEvents GLabel1 As gLabel.gLabel
    Friend WithEvents lblTeamLocLeft As System.Windows.Forms.Label
    Friend WithEvents lblTeamNameLeft As System.Windows.Forms.Label
    Friend WithEvents lblScoreLeft As System.Windows.Forms.Label
    Friend WithEvents lblScoreRight As System.Windows.Forms.Label
    Friend WithEvents lblTeamLocRight As System.Windows.Forms.Label
    Friend WithEvents lblTeamNameRight As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lblMsg = New gLabel.gLabel()
        Me.lblCountdown = New System.Windows.Forms.Label()
        Me.picGraphic = New System.Windows.Forms.PictureBox()
        Me.lblTeamLocLeft = New System.Windows.Forms.Label()
        Me.lblTeamNameLeft = New System.Windows.Forms.Label()
        Me.lblScoreLeft = New System.Windows.Forms.Label()
        Me.lblScoreRight = New System.Windows.Forms.Label()
        Me.lblTeamLocRight = New System.Windows.Forms.Label()
        Me.lblTeamNameRight = New System.Windows.Forms.Label()
        Me.FadeTimer = New System.Windows.Forms.Timer(Me.components)
        CType(Me.picGraphic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.lblMsg.ShadowOffset = New System.Drawing.Point(3, 3)
        Me.lblMsg.ShadowState = True
        Me.lblMsg.Size = New System.Drawing.Size(1280, 720)
        Me.lblMsg.TabIndex = 1
        Me.lblMsg.Text = "Welcome to JANIS"
        '
        'lblCountdown
        '
        Me.lblCountdown.BackColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(48, Byte), Integer), CType(CType(48, Byte), Integer))
        Me.lblCountdown.Font = New System.Drawing.Font("Arial Black", 70.0!, System.Drawing.FontStyle.Bold)
        Me.lblCountdown.Location = New System.Drawing.Point(0, 600)
        Me.lblCountdown.Name = "lblCountdown"
        Me.lblCountdown.Size = New System.Drawing.Size(1280, 120)
        Me.lblCountdown.TabIndex = 7
        Me.lblCountdown.Text = "00:00:00"
        Me.lblCountdown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblCountdown.Visible = False
        '
        'picGraphic
        '
        Me.picGraphic.Image = Global.JANIS.My.Resources.Resources.ScoreTemplate
        Me.picGraphic.InitialImage = Nothing
        Me.picGraphic.Location = New System.Drawing.Point(0, 0)
        Me.picGraphic.Name = "picGraphic"
        Me.picGraphic.Size = New System.Drawing.Size(1280, 720)
        Me.picGraphic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picGraphic.TabIndex = 0
        Me.picGraphic.TabStop = False
        '
        'lblTeamLocLeft
        '
        Me.lblTeamLocLeft.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTeamLocLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
        Me.lblTeamLocLeft.Font = New System.Drawing.Font("Roboto Slab", 46.0!, System.Drawing.FontStyle.Bold)
        Me.lblTeamLocLeft.ForeColor = System.Drawing.Color.White
        Me.lblTeamLocLeft.Location = New System.Drawing.Point(25, 41)
        Me.lblTeamLocLeft.Name = "lblTeamLocLeft"
        Me.lblTeamLocLeft.Size = New System.Drawing.Size(454, 77)
        Me.lblTeamLocLeft.TabIndex = 8
        Me.lblTeamLocLeft.Text = "Team City" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.lblTeamLocLeft.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.lblTeamLocLeft.UseCompatibleTextRendering = True
        '
        'lblTeamNameLeft
        '
        Me.lblTeamNameLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
        Me.lblTeamNameLeft.Font = New System.Drawing.Font("Roboto Slab", 46.0!, System.Drawing.FontStyle.Bold)
        Me.lblTeamNameLeft.ForeColor = System.Drawing.Color.White
        Me.lblTeamNameLeft.Location = New System.Drawing.Point(25, 109)
        Me.lblTeamNameLeft.Name = "lblTeamNameLeft"
        Me.lblTeamNameLeft.Size = New System.Drawing.Size(454, 77)
        Me.lblTeamNameLeft.TabIndex = 9
        Me.lblTeamNameLeft.Text = "Team Name"
        Me.lblTeamNameLeft.UseCompatibleTextRendering = True
        '
        'lblScoreLeft
        '
        Me.lblScoreLeft.BackColor = System.Drawing.Color.Black
        Me.lblScoreLeft.Font = New System.Drawing.Font("Roboto Slab", 219.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScoreLeft.ForeColor = System.Drawing.Color.White
        Me.lblScoreLeft.Location = New System.Drawing.Point(-30, 284)
        Me.lblScoreLeft.Name = "lblScoreLeft"
        Me.lblScoreLeft.Size = New System.Drawing.Size(620, 416)
        Me.lblScoreLeft.TabIndex = 10
        Me.lblScoreLeft.Text = "000"
        Me.lblScoreLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblScoreLeft.UseCompatibleTextRendering = True
        '
        'lblScoreRight
        '
        Me.lblScoreRight.BackColor = System.Drawing.Color.Black
        Me.lblScoreRight.Font = New System.Drawing.Font("Roboto Slab", 219.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScoreRight.ForeColor = System.Drawing.Color.White
        Me.lblScoreRight.Location = New System.Drawing.Point(690, 284)
        Me.lblScoreRight.Name = "lblScoreRight"
        Me.lblScoreRight.Size = New System.Drawing.Size(620, 416)
        Me.lblScoreRight.TabIndex = 11
        Me.lblScoreRight.Text = "768"
        Me.lblScoreRight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblScoreRight.UseCompatibleTextRendering = True
        '
        'lblTeamLocRight
        '
        Me.lblTeamLocRight.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTeamLocRight.BackColor = System.Drawing.Color.Maroon
        Me.lblTeamLocRight.Font = New System.Drawing.Font("Roboto Slab", 46.0!, System.Drawing.FontStyle.Bold)
        Me.lblTeamLocRight.ForeColor = System.Drawing.Color.White
        Me.lblTeamLocRight.Location = New System.Drawing.Point(801, 41)
        Me.lblTeamLocRight.Name = "lblTeamLocRight"
        Me.lblTeamLocRight.Size = New System.Drawing.Size(454, 77)
        Me.lblTeamLocRight.TabIndex = 12
        Me.lblTeamLocRight.Text = "Team City" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.lblTeamLocRight.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblTeamLocRight.UseCompatibleTextRendering = True
        '
        'lblTeamNameRight
        '
        Me.lblTeamNameRight.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTeamNameRight.BackColor = System.Drawing.Color.Maroon
        Me.lblTeamNameRight.Font = New System.Drawing.Font("Roboto Slab", 46.0!, System.Drawing.FontStyle.Bold)
        Me.lblTeamNameRight.ForeColor = System.Drawing.Color.White
        Me.lblTeamNameRight.Location = New System.Drawing.Point(801, 109)
        Me.lblTeamNameRight.Name = "lblTeamNameRight"
        Me.lblTeamNameRight.Size = New System.Drawing.Size(454, 77)
        Me.lblTeamNameRight.TabIndex = 13
        Me.lblTeamNameRight.Text = "Team Name"
        Me.lblTeamNameRight.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblTeamNameRight.UseCompatibleTextRendering = True
        '
        'FadeTimer
        '
        Me.FadeTimer.Interval = 70
        '
        'fmScreen
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1280, 720)
        Me.Controls.Add(Me.lblTeamNameRight)
        Me.Controls.Add(Me.lblTeamLocRight)
        Me.Controls.Add(Me.lblScoreRight)
        Me.Controls.Add(Me.lblScoreLeft)
        Me.Controls.Add(Me.lblTeamNameLeft)
        Me.Controls.Add(Me.lblTeamLocLeft)
        Me.Controls.Add(Me.lblCountdown)
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
        Me.lblScoreLeft.Font = CustomFont.GetInstance(Me.lblScoreLeft.Font.Size, FontStyle.Bold)
        Me.lblScoreRight.Font = Me.lblScoreLeft.Font
        Me.lblTeamNameLeft.Font = CustomFont.GetInstance(Me.lblTeamNameLeft.Font.Size, FontStyle.Bold)
        Me.lblTeamNameRight.Font = Me.lblTeamNameLeft.Font
        Me.lblTeamLocLeft.Font = Me.lblTeamNameLeft.Font
        Me.lblTeamLocRight.Font = Me.lblTeamNameLeft.Font
        '* Let's get all the elements stacked in the right order.
        ' Me.picGraphic.BringToFront()  No need to do this, because we want it in back.
        Me.lblMsg.BringToFront()
        Me.lblTeamLocLeft.BringToFront()
        Me.lblTeamLocRight.BringToFront()
        Me.lblTeamNameLeft.BringToFront()
        Me.lblTeamNameRight.BringToFront()
        Me.lblScoreLeft.BringToFront()
        Me.lblScoreRight.BringToFront()
        Me.lblCountdown.BringToFront()
    End Sub

    Public Sub fmScreen_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.MouseEnter, picGraphic.MouseEnter, lblMsg.MouseEnter
        '* Prevent the cursor from moving into the fmScreen. If it does, move it to the
        '* immediate left of this form (keep Y coord). This used to work in XP and VB 2005. I don't think it works now.
        System.Windows.Forms.Cursor.Position = New Point(Me.Left - 1, MousePosition.Y)
    End Sub

    Public Function MyLeft() As Integer
        '* Shareable function for public return of leftmost coordinate of this form.
        Return Me.Left
    End Function
    Public Sub SetTeamColor(teamside As String, newcolor As Color)
        If teamside = "Left" Then
            Me.LeftTeamColor = newcolor
            Gfxfast.FloodFill(Me.ScoreboardBitMap, Me.ScoreboardColorAnchorLeft.X, Me.ScoreboardColorAnchorLeft.Y, newcolor)
            Me.SetScoreFadeIncrements(LeftFadeIncrements, newcolor)
        Else
            Me.RightTeamColor = newcolor
            Gfxfast.FloodFill(Me.ScoreboardBitMap, Me.ScoreboardColorAnchorRight.X, Me.ScoreboardColorAnchorRight.Y, newcolor)
            Me.SetScoreFadeIncrements(RightFadeIncrements, newcolor)
        End If
        If Me.lblTeamLocLeft.Visible Then
            Me.ShowScore(Me.lblScoreLeft.Text, Me.lblTeamLocLeft.Text, Me.lblTeamNameLeft.Text, Me.lblScoreRight.Text, Me.lblTeamLocRight.Text, Me.lblTeamNameRight.Text)
        End If
    End Sub

    Public Sub AdjustSize(ByVal sRatio As Integer)
        '* Hack: resizing the height and width does WEIRD things to the team name field locations, so save them and use them later.

        With Me
            Dim NameTop As Integer = .lblTeamNameLeft.Top
            Dim LocTop As Integer = .lblTeamLocLeft.Top
            Dim RightLocLeft As Integer = .lblTeamLocRight.Left
            Dim RightTeamLeft As Integer = .lblTeamNameRight.Left
            .Height = .Height / sRatio
            .Width = .Width / sRatio
            .lblScoreLeft.Left = .lblScoreLeft.Left / sRatio
            .lblScoreLeft.Top = .lblScoreLeft.Top / sRatio
            .lblScoreLeft.Height = .lblScoreLeft.Height / sRatio
            .lblScoreLeft.Width = .lblScoreLeft.Width / sRatio
            .lblScoreLeft.Font = New Font(.lblScoreLeft.Font.FontFamily, CSng(Val(.lblScoreLeft.Font.Size) / sRatio), .lblScoreLeft.Font.Style)
            .lblScoreRight.Left = .lblScoreRight.Left / sRatio
            .lblScoreRight.Top = .lblScoreRight.Top / sRatio
            .lblScoreRight.Height = .lblScoreRight.Height / sRatio
            .lblScoreRight.Width = .lblScoreRight.Width / sRatio
            .lblScoreRight.Font = .lblScoreLeft.Font
            .lblTeamNameLeft.Left = .lblTeamNameLeft.Left / sRatio
            .lblTeamNameLeft.Top = NameTop / sRatio
            .lblTeamNameLeft.Height = .lblTeamNameLeft.Height / sRatio
            .lblTeamNameLeft.Width = .lblTeamNameLeft.Width / sRatio
            .lblTeamNameLeft.Font = New Font(.lblTeamNameLeft.Font.FontFamily, CSng(Val(.lblTeamNameLeft.Font.Size) / sRatio), .lblTeamNameLeft.Font.Style)
            .lblTeamNameRight.Left = RightTeamLeft / sRatio
            .lblTeamNameRight.Top = .lblTeamNameLeft.Top
            .lblTeamNameRight.Height = .lblTeamNameRight.Height / sRatio
            .lblTeamNameRight.Width = .lblTeamNameRight.Width / sRatio
            .lblTeamNameRight.Font = .lblTeamNameLeft.Font
            .lblTeamLocLeft.Left = .lblTeamLocLeft.Left / sRatio
            .lblTeamLocLeft.Top = LocTop / sRatio
            .lblTeamLocLeft.Height = .lblTeamLocLeft.Height / sRatio
            .lblTeamLocLeft.Width = .lblTeamLocLeft.Width / sRatio
            .lblTeamLocLeft.Font = .lblTeamNameLeft.Font
            .lblTeamLocRight.Left = RightLocLeft / sRatio
            .lblTeamLocRight.Top = .lblTeamLocLeft.Top
            .lblTeamLocRight.Height = .lblTeamLocRight.Height / sRatio
            .lblTeamLocRight.Width = .lblTeamLocRight.Width / sRatio
            .lblTeamLocRight.Font = .lblTeamNameLeft.Font
            .lblMsg.Left = .lblMsg.Left / sRatio
            .lblMsg.Top = .lblMsg.Top / sRatio
            .lblMsg.Height = .lblMsg.Height / sRatio
            .lblMsg.Width = .lblMsg.Width / sRatio
            .lblMsg.Font = New Font(.lblMsg.Font.Name, CSng(Val(.lblMsg.Font.Size) / sRatio), .lblMsg.Font.Style)
            .lblCountdown.Left = .lblCountdown.Left / sRatio
            .lblCountdown.Top = .lblCountdown.Top / sRatio
            .lblCountdown.Height = .lblCountdown.Height / sRatio
            .lblCountdown.Width = .lblCountdown.Width / sRatio
            .lblCountdown.Font = New Font(.lblCountdown.Font.Name, CSng(Val(.lblCountdown.Font.Size) / sRatio), .lblCountdown.Font.Style)
            .picGraphic.Left = .picGraphic.Left / sRatio
            .picGraphic.Top = .picGraphic.Top / sRatio
            .picGraphic.Height = .picGraphic.Height / sRatio
            .picGraphic.Width = .picGraphic.Width / sRatio
        End With
    End Sub

    Public Sub Blackout()
        '* Black out the screen and turn off visible stuff

        Me.BackColor = System.Drawing.Color.Black
        Me.picGraphic.Visible = False
        Me.picGraphic.ImageLocation = ""
        Me.lblMsg.Visible = False
        Me.lblTeamLocLeft.Visible = False
        Me.lblTeamLocRight.Visible = False
        Me.lblTeamNameLeft.Visible = False
        Me.lblTeamNameRight.Visible = False
        Me.lblScoreLeft.Visible = False
        Me.lblScoreRight.Visible = False
    End Sub

    Public Sub SetTextShadows(ByVal UseShadows As Boolean)
        Me.lblMsg.ShadowState = UseShadows
    End Sub

    Public Sub ShowText(ByVal txt As String, ByVal BackColor As System.Drawing.Color, ByVal fontsize As Integer)
        Me.lblTeamLocLeft.Visible = False
        Me.lblTeamLocRight.Visible = False
        Me.lblTeamNameLeft.Visible = False
        Me.lblTeamNameRight.Visible = False
        Me.lblScoreLeft.Visible = False
        Me.lblScoreRight.Visible = False
        Me.picGraphic.Visible = False
        Me.picGraphic.ImageLocation = ""
        Me.lblMsg.Font = New Font(Me.lblMsg.Font.Name, fontsize, Me.lblMsg.Font.Style)
        Me.lblMsg.BackColor = BackColor
        Me.lblMsg.Text = txt
        Me.lblMsg.Visible = True
    End Sub

    Public Sub ShowScore(ByVal scrLeft As String, ByVal locLeft As String, ByVal nameLeft As String, ByVal scrRight As String, ByVal locRight As String, ByVal nameRight As String)

        Me.lblTeamLocLeft.BackColor = LeftTeamColor
        Me.lblTeamNameLeft.BackColor = LeftTeamColor
        Me.lblTeamLocLeft.Text = locLeft
        Me.lblTeamNameLeft.Text = nameLeft
        Me.lblTeamLocRight.BackColor = RightTeamColor
        Me.lblTeamNameRight.BackColor = RightTeamColor
        Me.lblTeamLocRight.Text = locRight
        Me.lblTeamNameRight.Text = nameRight

        Me.lblMsg.Visible = False
        Me.picGraphic.Image = Me.ScoreboardBitMap
        Me.picGraphic.Visible = True

        Me.lblTeamLocLeft.Visible = True
        Me.lblTeamNameLeft.Visible = True
        Me.lblTeamLocRight.Visible = True
        Me.lblTeamNameRight.Visible = True

        If Me.lblScoreLeft.Text <> scrLeft Then
            Me.lblScoreLeft.Text = Me.Limited_Score(scrLeft)
            FadeBuff(Me.lblScoreLeft, LeftTeamColor)
        End If
        If Me.lblScoreRight.Text <> scrRight Then
            Me.lblScoreRight.Text = Me.Limited_Score(scrRight)
            FadeBuff(Me.lblScoreRight, RightTeamColor)
        End If
        Me.lblScoreLeft.Visible = True
        Me.lblScoreRight.Visible = True
    End Sub

    Public Sub ShowImage(ByRef Img As Image, ByVal Expand As Boolean)
        If Img Is Nothing Then Exit Sub

        Me.BackColor = System.Drawing.Color.Black
        Me.lblTeamLocLeft.Visible = False
        Me.lblTeamLocRight.Visible = False
        Me.lblTeamNameLeft.Visible = False
        Me.lblTeamNameRight.Visible = False
        Me.lblScoreLeft.Visible = False
        Me.lblScoreRight.Visible = False
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
        Me.lblCountdown.BringToFront()
        If CountdownVisible Then
            Me.lblMsg.Height = Me.lblMsg.Tag - Me.lblCountdown.Height
        Else
            Me.lblMsg.Height = Me.lblMsg.Tag
        End If

        Me.lblCountdown.BackColor = BackColor
        Me.lblCountdown.Visible = CountdownVisible
        Me.lblCountdown.Text = CountdownText
    End Sub


#Region "Private Functions and Subs"
    Private Function Limited_Score(ByVal score As String) As Integer
        '* If the score is too big or too small, it will be too wide to display
        Return Math.Min(Math.Max(Convert.ToInt32(score), MIN_SCORE), MAX_SCORE).ToString
    End Function

    Private Sub SetScoreFadeIncrements(ByRef FadeIncrements As Integer(), ByVal fromcolor As System.Drawing.Color)
        '* this ABSOLUTELY depends on the default score text color being full white
        '* Lossy integer division so add 1 to each increment -- AddColorIncrement() will make sure they never exceed maximum.
        Dim timerTicks As Integer = 8
        FadeIncrements(0) = (SCORE_TEXT_COLOR.A - fromcolor.A) \ timerTicks + 1    '* Backslash is for integer division
        FadeIncrements(1) = (SCORE_TEXT_COLOR.R - fromcolor.R) \ timerTicks + 1
        FadeIncrements(2) = (SCORE_TEXT_COLOR.G - fromcolor.G) \ timerTicks + 1
        FadeIncrements(3) = (SCORE_TEXT_COLOR.B - fromcolor.B) \ timerTicks + 1
    End Sub
    Private Function AddColorIncrement(ByVal color As System.Drawing.Color, ByVal increments() As Integer) As System.Drawing.Color
        '* this ABSOLUTELY depends on the default score text color being full white
        Dim a As Integer = Math.Min(increments(0) + color.A, SCORE_TEXT_COLOR.A)
        Dim r As Integer = Math.Min(increments(1) + color.R, SCORE_TEXT_COLOR.R)
        Dim g As Integer = Math.Min(increments(2) + color.G, SCORE_TEXT_COLOR.G)
        Dim b As Integer = Math.Min(increments(3) + color.B, SCORE_TEXT_COLOR.B)

        Return System.Drawing.Color.FromArgb(a, r, g, b)
    End Function

    Private Sub FadeBuff(ByRef lbl As System.Windows.Forms.Label, ByVal buffcolor As System.Drawing.Color)
        lbl.ForeColor = buffcolor
        If Not Me.FadeTimer.Enabled Then Me.FadeTimer.Start()
    End Sub

    Private Sub FadeTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FadeTimer.Tick
        Dim changed As Boolean = False
        If Not Me.lblScoreLeft.ForeColor.Equals(SCORE_TEXT_COLOR) Then
            Me.lblScoreLeft.ForeColor = Me.AddColorIncrement(Me.lblScoreLeft.ForeColor, LeftFadeIncrements)
            changed = True
        End If
        If Not Me.lblScoreRight.ForeColor.Equals(SCORE_TEXT_COLOR) Then
            Me.lblScoreRight.ForeColor = Me.AddColorIncrement(Me.lblScoreRight.ForeColor, RightFadeIncrements)
            changed = True
        End If
        If Not changed Then Me.FadeTimer.Stop()
    End Sub

#End Region
End Class
