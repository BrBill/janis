Namespace JANIS
    Public Class fmScreen
        Inherits System.Windows.Forms.Form

        Private ScoreboardBitMap As Bitmap = DirectCast(Global.JANIS.My.Resources.ScoreTemplate.Clone, Bitmap)
        Private ScoreboardColorAnchorLeft As Point = New Point(70, 110)    ' These are the scoreboard background locations that
        Private ScoreboardColorAnchorRight As Point = New Point(1210, 110) ' get flood-filled when team color changes
        Private LeftTeamColor As System.Drawing.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
        Private RightTeamColor As System.Drawing.Color = System.Drawing.Color.Maroon
        Private SCORE_TEXT_COLOR As System.Drawing.Color = System.Drawing.Color.White   '* If this gets changed a lot of score fading stuff will break
        Private LeftFadeIncrements() As Integer = {0, 16, 16, 5}     '* A R G B (for base default blue)   If any of these had to decrease to reach the goal color,
        Private RightFadeIncrements() As Integer = {0, 8, 16, 16}    '* A R G B (for base default red)    the numbers would be negative, but that would break AddColorIncrement()

        Const MAX_SCORE As Integer = 999
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
        Friend WithEvents FadeTimer As System.Windows.Forms.Timer
        Friend WithEvents AxMediaPlayer As AxWMPLib.AxWindowsMediaPlayer
        Friend WithEvents picGraphic As System.Windows.Forms.PictureBox
        Friend WithEvents lblMsg As gLabel.gLabel
        Friend WithEvents lblCountdown As System.Windows.Forms.Label
        Friend WithEvents lblTeamLocLeft As System.Windows.Forms.Label
        Friend WithEvents lblTeamNameLeft As System.Windows.Forms.Label
        Friend WithEvents lblScoreLeft As System.Windows.Forms.Label
        Friend WithEvents lblScoreRight As System.Windows.Forms.Label
        Friend WithEvents lblTeamLocRight As System.Windows.Forms.Label
        Friend WithEvents lblTeamNameRight As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fmScreen))
            Me.lblMsg = New gLabel.gLabel()
            Me.lblCountdown = New System.Windows.Forms.Label()
            Me.lblTeamLocLeft = New System.Windows.Forms.Label()
            Me.lblTeamNameLeft = New System.Windows.Forms.Label()
            Me.lblScoreLeft = New System.Windows.Forms.Label()
            Me.lblScoreRight = New System.Windows.Forms.Label()
            Me.lblTeamLocRight = New System.Windows.Forms.Label()
            Me.lblTeamNameRight = New System.Windows.Forms.Label()
            Me.FadeTimer = New System.Windows.Forms.Timer(Me.components)
            Me.picGraphic = New System.Windows.Forms.PictureBox()
            Me.AxMediaPlayer = New AxWMPLib.AxWindowsMediaPlayer()
            CType(Me.picGraphic, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.AxMediaPlayer, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblMsg
            '
            Me.lblMsg.BackColor = System.Drawing.Color.Transparent
            Me.lblMsg.Font = New System.Drawing.Font("Arial", 185.0!, System.Drawing.FontStyle.Bold)
            Me.lblMsg.ForeColor = System.Drawing.Color.White
            Me.lblMsg.GlowState = False
            Me.lblMsg.Location = New System.Drawing.Point(0, 0)
            Me.lblMsg.Name = "lblMsg"
            Me.lblMsg.ShadowColor = System.Drawing.Color.Black
            Me.lblMsg.ShadowOffset = New System.Drawing.Point(3, 3)
            Me.lblMsg.ShadowState = True
            Me.lblMsg.Size = New System.Drawing.Size(1920, 1080)
            Me.lblMsg.TabIndex = 1
            Me.lblMsg.Text = "Welcome to JANIS"
            '
            'lblCountdown
            '
            Me.lblCountdown.BackColor = System.Drawing.Color.FromArgb(CType(CType(36, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(36, Byte), Integer))
            Me.lblCountdown.Font = New System.Drawing.Font("Arial Black", 70.0!, System.Drawing.FontStyle.Bold)
            Me.lblCountdown.Location = New System.Drawing.Point(0, 960)
            Me.lblCountdown.Name = "lblCountdown"
            Me.lblCountdown.Size = New System.Drawing.Size(1920, 120)
            Me.lblCountdown.TabIndex = 7
            Me.lblCountdown.Text = "00:00:00"
            Me.lblCountdown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            Me.lblCountdown.Visible = False
            '
            'lblTeamLocLeft
            '
            Me.lblTeamLocLeft.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lblTeamLocLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.lblTeamLocLeft.Font = New System.Drawing.Font("Roboto Slab", 69.0!, System.Drawing.FontStyle.Bold)
            Me.lblTeamLocLeft.ForeColor = System.Drawing.Color.White
            Me.lblTeamLocLeft.Location = New System.Drawing.Point(37, 63)
            Me.lblTeamLocLeft.Name = "lblTeamLocLeft"
            Me.lblTeamLocLeft.Size = New System.Drawing.Size(681, 115)
            Me.lblTeamLocLeft.TabIndex = 8
            Me.lblTeamLocLeft.Text = "Team City" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            Me.lblTeamLocLeft.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            Me.lblTeamLocLeft.UseCompatibleTextRendering = True
            '
            'lblTeamNameLeft
            '
            Me.lblTeamNameLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.lblTeamNameLeft.Font = New System.Drawing.Font("Roboto Slab", 69.0!, System.Drawing.FontStyle.Bold)
            Me.lblTeamNameLeft.ForeColor = System.Drawing.Color.White
            Me.lblTeamNameLeft.Location = New System.Drawing.Point(37, 163)
            Me.lblTeamNameLeft.Name = "lblTeamNameLeft"
            Me.lblTeamNameLeft.Size = New System.Drawing.Size(681, 115)
            Me.lblTeamNameLeft.TabIndex = 9
            Me.lblTeamNameLeft.Text = "Team Name"
            Me.lblTeamNameLeft.UseCompatibleTextRendering = True
            '
            'lblScoreLeft
            '
            Me.lblScoreLeft.BackColor = System.Drawing.Color.Black
            Me.lblScoreLeft.Font = New System.Drawing.Font("Roboto Slab", 329.625!, System.Drawing.FontStyle.Bold)
            Me.lblScoreLeft.ForeColor = System.Drawing.Color.White
            Me.lblScoreLeft.Location = New System.Drawing.Point(-45, 426)
            Me.lblScoreLeft.Name = "lblScoreLeft"
            Me.lblScoreLeft.Size = New System.Drawing.Size(930, 624)
            Me.lblScoreLeft.TabIndex = 10
            Me.lblScoreLeft.Text = "000"
            Me.lblScoreLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.lblScoreLeft.UseCompatibleTextRendering = True
            '
            'lblScoreRight
            '
            Me.lblScoreRight.BackColor = System.Drawing.Color.Black
            Me.lblScoreRight.Font = New System.Drawing.Font("Roboto Slab", 329.625!, System.Drawing.FontStyle.Bold)
            Me.lblScoreRight.ForeColor = System.Drawing.Color.White
            Me.lblScoreRight.Location = New System.Drawing.Point(1035, 426)
            Me.lblScoreRight.Name = "lblScoreRight"
            Me.lblScoreRight.Size = New System.Drawing.Size(930, 624)
            Me.lblScoreRight.TabIndex = 11
            Me.lblScoreRight.Text = "768"
            Me.lblScoreRight.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.lblScoreRight.UseCompatibleTextRendering = True
            '
            'lblTeamLocRight
            '
            Me.lblTeamLocRight.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTeamLocRight.BackColor = System.Drawing.Color.Maroon
            Me.lblTeamLocRight.Font = New System.Drawing.Font("Roboto Slab", 69.0!, System.Drawing.FontStyle.Bold)
            Me.lblTeamLocRight.ForeColor = System.Drawing.Color.White
            Me.lblTeamLocRight.Location = New System.Drawing.Point(1201, 63)
            Me.lblTeamLocRight.Name = "lblTeamLocRight"
            Me.lblTeamLocRight.Size = New System.Drawing.Size(681, 115)
            Me.lblTeamLocRight.TabIndex = 12
            Me.lblTeamLocRight.Text = "Team City" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            Me.lblTeamLocRight.TextAlign = System.Drawing.ContentAlignment.BottomRight
            Me.lblTeamLocRight.UseCompatibleTextRendering = True
            '
            'lblTeamNameRight
            '
            Me.lblTeamNameRight.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTeamNameRight.BackColor = System.Drawing.Color.Maroon
            Me.lblTeamNameRight.Font = New System.Drawing.Font("Roboto Slab", 69.0!, System.Drawing.FontStyle.Bold)
            Me.lblTeamNameRight.ForeColor = System.Drawing.Color.White
            Me.lblTeamNameRight.Location = New System.Drawing.Point(1201, 163)
            Me.lblTeamNameRight.Name = "lblTeamNameRight"
            Me.lblTeamNameRight.Size = New System.Drawing.Size(681, 115)
            Me.lblTeamNameRight.TabIndex = 13
            Me.lblTeamNameRight.Text = "Team Name"
            Me.lblTeamNameRight.TextAlign = System.Drawing.ContentAlignment.TopRight
            Me.lblTeamNameRight.UseCompatibleTextRendering = True
            '
            'FadeTimer
            '
            Me.FadeTimer.Interval = 70
            '
            'picGraphic
            '
            Me.picGraphic.Image = Global.JANIS.My.Resources.Resources.ScoreTemplate
            Me.picGraphic.InitialImage = Nothing
            Me.picGraphic.Location = New System.Drawing.Point(0, 0)
            Me.picGraphic.Name = "picGraphic"
            Me.picGraphic.Size = New System.Drawing.Size(1920, 1080)
            Me.picGraphic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.picGraphic.TabIndex = 0
            Me.picGraphic.TabStop = False
            '
            'AxMediaPlayer
            '
            Me.AxMediaPlayer.Enabled = True
            Me.AxMediaPlayer.Location = New System.Drawing.Point(0, 0)
            Me.AxMediaPlayer.MaximumSize = New System.Drawing.Size(1920, 1080)
            Me.AxMediaPlayer.MinimumSize = New System.Drawing.Size(1920, 1080)
            Me.AxMediaPlayer.Name = "AxMediaPlayer"
            Me.AxMediaPlayer.OcxState = CType(resources.GetObject("AxMediaPlayer.OcxState"), System.Windows.Forms.AxHost.State)
            Me.AxMediaPlayer.Size = New System.Drawing.Size(1920, 1080)
            Me.AxMediaPlayer.TabIndex = 14
            Me.AxMediaPlayer.TabStop = False
            '
            'fmScreen
            '
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(1920, 1080)
            Me.Controls.Add(Me.lblTeamNameRight)
            Me.Controls.Add(Me.lblTeamLocRight)
            Me.Controls.Add(Me.lblScoreRight)
            Me.Controls.Add(Me.lblScoreLeft)
            Me.Controls.Add(Me.lblTeamNameLeft)
            Me.Controls.Add(Me.lblTeamLocLeft)
            Me.Controls.Add(Me.lblMsg)
            Me.Controls.Add(Me.picGraphic)
            Me.Controls.Add(Me.lblCountdown)
            Me.Controls.Add(Me.AxMediaPlayer)
            Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
            Me.ForeColor = System.Drawing.Color.White
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Location = New System.Drawing.Point(1920, 0)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "fmScreen"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
            Me.TopMost = True
            CType(Me.picGraphic, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.AxMediaPlayer, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Public Sub fmScreen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Me.FadeTimer.Stop()     '* this shouldn't be running but it seems to launch at start
            Me.AxMediaPlayer.Hide()
            Me.AxMediaPlayer.uiMode = "none"

            Me.lblScoreLeft.Font = CustomFont.GetInstance(Me.lblScoreLeft.Font.Size, FontStyle.Bold)
            Me.lblScoreRight.Font = Me.lblScoreLeft.Font
            Me.lblTeamNameLeft.Font = CustomFont.GetInstance(Me.lblTeamNameLeft.Font.Size, FontStyle.Bold)
            Me.lblTeamNameRight.Font = Me.lblTeamNameLeft.Font
            Me.lblTeamLocLeft.Font = Me.lblTeamNameLeft.Font
            Me.lblTeamLocRight.Font = Me.lblTeamNameLeft.Font
            '* Let's get all the elements stacked in the right order.
            ' Me.AxMediaPlayer.BringToFront()  No need to do this, because we want it in back.
            Me.picGraphic.BringToFront()
            Me.lblMsg.BringToFront()
            Me.lblTeamLocLeft.BringToFront()
            Me.lblTeamLocRight.BringToFront()
            Me.lblTeamNameLeft.BringToFront()
            Me.lblTeamNameRight.BringToFront()
            Me.lblScoreLeft.BringToFront()
            Me.lblScoreRight.BringToFront()
            Me.lblCountdown.BringToFront()

            With Me.AxMediaPlayer
                .Ctlenabled = False
                .uiMode = "none"
                .fullScreen = False
                .stretchToFit = True
                With .settings
                    .mute = False
                    .autoStart = True
                    .invokeURLs = False
                    .playCount = 1
                    .volume = 100
                End With
            End With

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

        Public Sub SetLeft(leftpos As Integer)
            Me.Left = leftpos
        End Sub
        Public Sub SetTop(toppos As Integer)
            Me.Top = toppos
        End Sub

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
            '* Adjust the size of the form and all its controls by the supplied ratio. This makes it possible for Test Mode to work with no second monitor.
            '* Hack: resizing the height and width does WEIRD things to the team name field locations, so save them and use them later.

            With Me
                Dim NameTop As Integer = .lblTeamNameLeft.Top
                Dim LocTop As Integer = .lblTeamLocLeft.Top
                Dim RightLocLeft As Integer = .lblTeamLocRight.Left
                Dim RightTeamLeft As Integer = .lblTeamNameRight.Left
                .Height = CInt(.Height / sRatio)
                .Width = CInt(.Width / sRatio)
                .lblScoreLeft.Left = CInt(.lblScoreLeft.Left / sRatio)
                .lblScoreLeft.Top = CInt(.lblScoreLeft.Top / sRatio)
                .lblScoreLeft.Height = CInt(.lblScoreLeft.Height / sRatio)
                .lblScoreLeft.Width = CInt(.lblScoreLeft.Width / sRatio)
                .lblScoreLeft.Font = New Font(.lblScoreLeft.Font.FontFamily, CSng(Val(.lblScoreLeft.Font.Size) / sRatio), .lblScoreLeft.Font.Style)
                .lblScoreRight.Left = CInt(.lblScoreRight.Left / sRatio)
                .lblScoreRight.Top = CInt(.lblScoreRight.Top / sRatio)
                .lblScoreRight.Height = CInt(.lblScoreRight.Height / sRatio)
                .lblScoreRight.Width = CInt(.lblScoreRight.Width / sRatio)
                .lblScoreRight.Font = .lblScoreLeft.Font
                .lblTeamNameLeft.Left = CInt(.lblTeamNameLeft.Left / sRatio)
                .lblTeamNameLeft.Top = CInt(NameTop / sRatio)
                .lblTeamNameLeft.Height = CInt(.lblTeamNameLeft.Height / sRatio)
                .lblTeamNameLeft.Width = CInt(.lblTeamNameLeft.Width / sRatio)
                .lblTeamNameLeft.Font = New Font(.lblTeamNameLeft.Font.FontFamily, CSng(Val(.lblTeamNameLeft.Font.Size) / sRatio), .lblTeamNameLeft.Font.Style)
                .lblTeamNameRight.Left = CInt(RightTeamLeft / sRatio)
                .lblTeamNameRight.Top = .lblTeamNameLeft.Top
                .lblTeamNameRight.Height = CInt(.lblTeamNameRight.Height / sRatio)
                .lblTeamNameRight.Width = CInt(.lblTeamNameRight.Width / sRatio)
                .lblTeamNameRight.Font = .lblTeamNameLeft.Font
                .lblTeamLocLeft.Left = CInt(.lblTeamLocLeft.Left / sRatio)
                .lblTeamLocLeft.Top = CInt(LocTop / sRatio)
                .lblTeamLocLeft.Height = CInt(.lblTeamLocLeft.Height / sRatio)
                .lblTeamLocLeft.Width = CInt(.lblTeamLocLeft.Width / sRatio)
                .lblTeamLocLeft.Font = .lblTeamNameLeft.Font
                .lblTeamLocRight.Left = CInt(RightLocLeft / sRatio)
                .lblTeamLocRight.Top = .lblTeamLocLeft.Top
                .lblTeamLocRight.Height = CInt(.lblTeamLocRight.Height / sRatio)
                .lblTeamLocRight.Width = CInt(.lblTeamLocRight.Width / sRatio)
                .lblTeamLocRight.Font = .lblTeamNameLeft.Font
                .lblMsg.Left = CInt(.lblMsg.Left / sRatio)
                .lblMsg.Top = CInt(.lblMsg.Top / sRatio)
                .lblMsg.Height = CInt(.lblMsg.Height / sRatio)
                .lblMsg.Width = CInt(.lblMsg.Width / sRatio)
                .lblMsg.Font = New Font(.lblMsg.Font.Name, CSng(Val(.lblMsg.Font.Size) / sRatio), .lblMsg.Font.Style)
                .lblCountdown.Left = CInt(.lblCountdown.Left / sRatio)
                .lblCountdown.Top = CInt(.lblCountdown.Top / sRatio)
                .lblCountdown.Height = CInt(.lblCountdown.Height / sRatio)
                .lblCountdown.Width = CInt(.lblCountdown.Width / sRatio)
                .lblCountdown.Font = New Font(.lblCountdown.Font.Name, CSng(Val(.lblCountdown.Font.Size) / sRatio), .lblCountdown.Font.Style)
                .picGraphic.Left = CInt(.picGraphic.Left / sRatio)
                .picGraphic.Top = CInt(.picGraphic.Top / sRatio)
                .picGraphic.Height = CInt(.picGraphic.Height / sRatio)
                .picGraphic.Width = CInt(.picGraphic.Width / sRatio)
                .AxMediaPlayer.Left = .picGraphic.Left
                .AxMediaPlayer.Top = .picGraphic.Top
                .AxMediaPlayer.Height = .picGraphic.Height
                .AxMediaPlayer.Width = .picGraphic.Width
            End With
        End Sub

        Public Sub Blackout()
            '* Black out the screen and turn off visible stuff

            Me.BackColor = System.Drawing.Color.Black
            Me.StopVideo()
            Me.picGraphic.Hide()
            Me.DisposeCurrentGraphicImage()
            Me.lblMsg.Hide()
            Me.lblTeamLocLeft.Hide()
            Me.lblTeamLocRight.Hide()
            Me.lblTeamNameLeft.Hide()
            Me.lblTeamNameRight.Hide()
            Me.lblScoreLeft.Hide()
            Me.lblScoreRight.Hide()
        End Sub

        Public Function CaptureScreenImage() As Bitmap
            Dim bmp As New Bitmap(Me.Width, Me.Height)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.CopyFromScreen(Me.Location, Point.Empty, Me.Size)
            End Using
            Return bmp
        End Function
        Public Sub SetTextShadows(ByVal UseShadows As Boolean)
            Me.lblMsg.ShadowState = UseShadows
        End Sub

        Public Sub ShowText(ByVal txt As String, ByVal BackColor As System.Drawing.Color, ByVal fontsize As Single)
            Me.StopVideo()
            Me.lblTeamLocLeft.Hide()
            Me.lblTeamLocRight.Hide()
            Me.lblTeamNameLeft.Hide()
            Me.lblTeamNameRight.Hide()
            Me.lblScoreLeft.Hide()
            Me.lblScoreRight.Hide()
            Me.picGraphic.Hide()
            Me.DisposeCurrentGraphicImage()

            Me.lblMsg.Font = New Font(Me.lblMsg.Font.Name, fontsize, Me.lblMsg.Font.Style)
            Me.lblMsg.BackColor = BackColor
            Me.lblMsg.Text = txt
            Me.lblMsg.Show()
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
            Me.picGraphic.Hide()
            Me.DisposeCurrentGraphicImage()

            Me.StopVideo()
            Me.lblMsg.Hide()
            '* Must clone the score bitmap; otherwise disposing it later will dispose the referenced original (BAD).
            Me.picGraphic.Image = DirectCast(Me.ScoreboardBitMap.Clone(), Image)
            Me.picGraphic.Show()

            Me.lblTeamLocLeft.Show()
            Me.lblTeamNameLeft.Show()
            Me.lblTeamLocRight.Show()
            Me.lblTeamNameRight.Show()

            If Me.lblScoreLeft.Text <> scrLeft Then
                Me.lblScoreLeft.Text = Me.Limited_Score(scrLeft)
                FadeBuff(Me.lblScoreLeft, LeftTeamColor)
            End If
            If Me.lblScoreRight.Text <> scrRight Then
                Me.lblScoreRight.Text = Me.Limited_Score(scrRight)
                FadeBuff(Me.lblScoreRight, RightTeamColor)
            End If
            Me.lblScoreLeft.Show()
            Me.lblScoreRight.Show()
        End Sub

        Public Sub ShowImage(ByVal Img As Image)
            If Img Is Nothing Then Exit Sub

            Me.BackColor = System.Drawing.Color.Black
            Me.StopVideo()
            Me.lblTeamLocLeft.Hide()
            Me.lblTeamLocRight.Hide()
            Me.lblTeamNameLeft.Hide()
            Me.lblTeamNameRight.Hide()
            Me.lblScoreLeft.Hide()
            Me.lblScoreRight.Hide()
            Me.lblMsg.Hide()
            Me.DisposeCurrentGraphicImage()

            Me.picGraphic.Image = New Bitmap(Img)   '* Own copy; caller manages the original
            Me.picGraphic.Show()
        End Sub

        Public Function LaunchVideo(ByVal fnam As String) As String
            '* Returns an error message if there was a problem, otherwise returns Nothing
            '* NEVER modify the mute or volume settings here. They were set before we got here and that's what we want.

            Static FirstInvoke As Boolean = True '* the first time a video is played, it takes a few milliseconds to start
            Dim resultMessage As String = Nothing
            Me.StopVideo()
            Me.lblTeamLocLeft.Hide()
            Me.lblTeamLocRight.Hide()
            Me.lblTeamNameLeft.Hide()
            Me.lblTeamNameRight.Hide()
            Me.lblScoreLeft.Hide()
            Me.lblScoreRight.Hide()
            Me.lblMsg.Hide()
            Me.picGraphic.Hide()
            Me.AxMediaPlayer.Show()


            Me.AxMediaPlayer.URL = fnam
            If FirstInvoke Then
                '* The first time a video is launched, it can take a few milliseconds to start. Only need to do this once (I think).
                System.Threading.Thread.Sleep(100)
                Application.DoEvents()
                FirstInvoke = False
            End If

            Try
                Me.AxMediaPlayer.Ctlcontrols.play()
                ' Application.DoEvents()
            Catch ex As Exception
                resultMessage = "Error playing video file '" & fnam & "':" & vbCrLf & ex.Message ' & vbCrLf
                Me.StopVideo()
            End Try

            Return resultMessage
        End Function
        Public Function IsVideoPlaying() As Boolean
            Return (Me.AxMediaPlayer.playState = WMPLib.WMPPlayState.wmppsPlaying)
            ' Or Me.AxMediaPlayer.playState = WMPLib.WMPPlayState.wmppsTransitioning)
        End Function

        Public Sub SetVideoMute(ByVal newMuteSetting As Boolean)
            '* NEVER CHANGE the AxMediaPlayer.settings.mute property. It's hella broken. Once it's set to True, it STAYS muted regardless of further settings changes.
            '* Always manage it with volume.
            If newMuteSetting Then
                Me.AxMediaPlayer.settings.volume = 0
            Else
                Me.AxMediaPlayer.settings.volume = 100
            End If
        End Sub
        Public Function GetVideoMute() As Boolean
            Return (Me.AxMediaPlayer.settings.volume.Equals(0))
        End Function

        Public Sub PauseVideo()
            Me.AxMediaPlayer.Ctlcontrols.pause()
        End Sub
        Public Function ResumeVideo() As String
            '* Returns an error message if there was a problem, otherwise returns Nothing
            Dim resultMessage As String = Nothing

            '* Can only resume if it's currently paused
            If Me.AxMediaPlayer.playState <> WMPLib.WMPPlayState.wmppsPaused Then
                Return resultMessage
            End If

            Me.lblTeamLocLeft.Hide()
            Me.lblTeamLocRight.Hide()
            Me.lblTeamNameLeft.Hide()
            Me.lblTeamNameRight.Hide()
            Me.lblScoreLeft.Hide()
            Me.lblScoreRight.Hide()
            Me.lblMsg.Hide()
            Me.picGraphic.Hide()
            Me.AxMediaPlayer.Show()

            Try
                Me.AxMediaPlayer.Ctlcontrols.play()
            Catch ex As Exception
                resultMessage = "Error playing video file:" & vbCrLf & ex.Message ' & vbCrLf
                Me.StopVideo()
            End Try
            Return resultMessage
        End Function
        Public Sub StopVideo()
            Me.AxMediaPlayer.Hide()
            Me.AxMediaPlayer.Ctlcontrols.stop()
            Me.AxMediaPlayer.close()
        End Sub

        Public Sub ShowCountdownText(ByVal CountdownText As String, ByVal BackColor As System.Drawing.Color, ByVal CountdownVisible As Boolean)
            '* Change the size of the message window to accomodate the countdown timer
            Me.lblCountdown.BringToFront()
            Dim msgHeight As Integer = CInt(Me.lblMsg.Tag)
            If CountdownVisible Then
                Me.lblMsg.Height = msgHeight - Me.lblCountdown.Height
            Else
                Me.lblMsg.Height = msgHeight
            End If

            Me.lblCountdown.BackColor = BackColor
            Me.lblCountdown.Visible = CountdownVisible
            Me.lblCountdown.Text = CountdownText
        End Sub


#Region "Private Functions and Subs"
        Private Sub DisposeCurrentGraphicImage()
            If Me.picGraphic.Image IsNot Nothing Then
                Me.picGraphic.Image.Dispose()
                Me.picGraphic.Image = Nothing
            End If
        End Sub

        Private Function Limited_Score(ByVal score As String) As String
            '* If the score is too big or too small, it will be too wide to display
            If score = "" Then Return ""
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

        Private Sub FadeBuff(ByVal lbl As System.Windows.Forms.Label, ByVal buffcolor As System.Drawing.Color)
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
End Namespace