Imports System.Net.NetworkInformation

Namespace JANIS
    Public Class fmScreen
        Inherits System.Windows.Forms.Form

        Public Event ScoreUpdateComplete()

        Private ScoreboardBitMap As Bitmap = DirectCast(Global.JANIS.My.Resources.ScoreTemplate.Clone, Bitmap)
        Private ScoreboardColorAnchorLeft As Point = New Point(70, 110)    ' These are the scoreboard background locations that
        Private ScoreboardColorAnchorRight As Point = New Point(1210, 110) ' get flood-filled when team color changes
        Private LeftTeamColor As System.Drawing.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
        Private RightTeamColor As System.Drawing.Color = System.Drawing.Color.Maroon
        Private SCORE_TEXT_COLOR As System.Drawing.Color = System.Drawing.Color.White   '* If this gets changed a lot of score fading stuff will break
        Private LeftFadeIncrements() As Integer = {0, 16, 16, 5}     '* A R G B (for base default blue)   If any of these had to decrease to reach the goal color,
        Private RightFadeIncrements() As Integer = {0, 8, 16, 16}    '* A R G B (for base default red)    the numbers would be negative, but that would break AddColorIncrement()

        '* LibVLC video playback
        Private _libVLC As LibVLCSharp.Shared.LibVLC
        Private _mediaPlayer As LibVLCSharp.Shared.MediaPlayer

        '* Limit these so they always fit in the display area
        Const MAX_SCORE As Integer = 999
        Const MIN_SCORE As Integer = -99

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            Me.FadeTimer.Stop()     '* this shouldn't be running but it seems to launch at start
        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
                Me.ScoreboardBitMap?.Dispose()
                Me._mediaPlayer?.Stop()
                Me._videoView?.Dispose()
                Me._mediaPlayer?.Dispose()
                Me._libVLC?.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.
        'Do not modify it using the code editor.
        Friend WithEvents FadeTimer As System.Windows.Forms.Timer
        Friend WithEvents picGraphic As System.Windows.Forms.PictureBox
        Friend WithEvents lblMsg As ShadowLabel
        Friend WithEvents lblCountdown As System.Windows.Forms.Label
        Friend WithEvents lblTeamLocLeft As System.Windows.Forms.Label
        Friend WithEvents lblTeamNameLeft As System.Windows.Forms.Label
        Friend WithEvents lblScoreLeft As System.Windows.Forms.Label
        Friend WithEvents lblScoreRight As System.Windows.Forms.Label
        Friend WithEvents lblTeamLocRight As System.Windows.Forms.Label
        Friend WithEvents lblTeamNameRight As System.Windows.Forms.Label
        Friend WithEvents _videoView As LibVLCSharp.WinForms.VideoView
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.lblMsg = New ShadowLabel()
            Me.lblCountdown = New System.Windows.Forms.Label()
            Me.lblTeamLocLeft = New System.Windows.Forms.Label()
            Me.lblTeamNameLeft = New System.Windows.Forms.Label()
            Me.lblScoreLeft = New System.Windows.Forms.Label()
            Me.lblScoreRight = New System.Windows.Forms.Label()
            Me.lblTeamLocRight = New System.Windows.Forms.Label()
            Me.lblTeamNameRight = New System.Windows.Forms.Label()
            Me.FadeTimer = New System.Windows.Forms.Timer(Me.components)
            Me.picGraphic = New System.Windows.Forms.PictureBox()
            Me._videoView = New LibVLCSharp.WinForms.VideoView()
            CType(Me.picGraphic, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me._videoView, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblMsg
            '
            Me.lblMsg.BackColor = System.Drawing.Color.Transparent
            Me.lblMsg.Font = New System.Drawing.Font("Arial", 185.0!, System.Drawing.FontStyle.Bold)
            Me.lblMsg.ForeColor = System.Drawing.Color.White
            Me.lblMsg.Location = New System.Drawing.Point(0, 0)
            Me.lblMsg.Name = "lblMsg"
            Me.lblMsg.ShadowColor = System.Drawing.Color.Black
            Me.lblMsg.ShadowOffset = New System.Drawing.Point(10, 10)
            Me.lblMsg.ShadowState = True
            Me.lblMsg.Size = New System.Drawing.Size(1920, 1080)
            Me.lblMsg.TabIndex = 1
            Me.lblMsg.Text = "Welcome to JANIS"
            Me.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
            '_videoView
            '
            Me._videoView.BackColor = System.Drawing.Color.Black
            Me._videoView.ForeColor = System.Drawing.Color.Turquoise
            Me._videoView.Location = New System.Drawing.Point(0, 0)
            Me._videoView.MediaPlayer = Nothing
            Me._videoView.Name = "_videoView"
            Me._videoView.Size = New System.Drawing.Size(1920, 1080)
            Me._videoView.TabIndex = 14
            Me._videoView.Visible = False
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
            Me.Controls.Add(Me._videoView)
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
            CType(Me._videoView, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Public Sub fmScreen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            '* Initialize LibVLC for video
            Me._libVLC = New LibVLCSharp.Shared.LibVLC()
            Me._mediaPlayer = New LibVLCSharp.Shared.MediaPlayer(Me._libVLC)
            Me._mediaPlayer.Mute = False
            Me._mediaPlayer.Volume = 100
            Me._videoView.MediaPlayer = Me._mediaPlayer

            '* Load custom fonts
            Me.lblScoreLeft.Font = CustomFont.GetInstance(Me.lblScoreLeft.Font.Size, FontStyle.Bold)
            Me.lblScoreRight.Font = Me.lblScoreLeft.Font
            Me.lblTeamNameLeft.Font = CustomFont.GetInstance(Me.lblTeamNameLeft.Font.Size, FontStyle.Bold)
            Me.lblTeamNameRight.Font = Me.lblTeamNameLeft.Font
            Me.lblTeamLocLeft.Font = Me.lblTeamNameLeft.Font
            Me.lblTeamLocRight.Font = Me.lblTeamNameLeft.Font

            Me.SetZOrder()
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
                .lblScoreLeft.Font = New Font(.lblScoreLeft.Font.FontFamily, .lblScoreLeft.Font.Size / sRatio, .lblScoreLeft.Font.Style)
                .lblScoreRight.Left = CInt(.lblScoreRight.Left / sRatio)
                .lblScoreRight.Top = CInt(.lblScoreRight.Top / sRatio)
                .lblScoreRight.Height = CInt(.lblScoreRight.Height / sRatio)
                .lblScoreRight.Width = CInt(.lblScoreRight.Width / sRatio)
                .lblScoreRight.Font = .lblScoreLeft.Font
                .lblTeamNameLeft.Left = CInt(.lblTeamNameLeft.Left / sRatio)
                .lblTeamNameLeft.Top = CInt(NameTop / sRatio)
                .lblTeamNameLeft.Height = CInt(.lblTeamNameLeft.Height / sRatio)
                .lblTeamNameLeft.Width = CInt(.lblTeamNameLeft.Width / sRatio)
                .lblTeamNameLeft.Font = New Font(.lblTeamNameLeft.Font.FontFamily, .lblTeamNameLeft.Font.Size / sRatio, .lblTeamNameLeft.Font.Style)
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
                .lblMsg.Font = New Font(.lblMsg.Font.Name, .lblMsg.Font.Size / sRatio, .lblMsg.Font.Style)
                .lblCountdown.Left = CInt(.lblCountdown.Left / sRatio)
                .lblCountdown.Top = CInt(.lblCountdown.Top / sRatio)
                .lblCountdown.Height = CInt(.lblCountdown.Height / sRatio)
                .lblCountdown.Width = CInt(.lblCountdown.Width / sRatio)
                .lblCountdown.Font = New Font(.lblCountdown.Font.Name, .lblCountdown.Font.Size / sRatio, .lblCountdown.Font.Style)
                .picGraphic.Left = CInt(.picGraphic.Left / sRatio)
                .picGraphic.Top = CInt(.picGraphic.Top / sRatio)
                .picGraphic.Height = CInt(.picGraphic.Height / sRatio)
                .picGraphic.Width = CInt(.picGraphic.Width / sRatio)
                _videoView.Left = .picGraphic.Left
                _videoView.Top = .picGraphic.Top
                _videoView.Height = .picGraphic.Height
                _videoView.Width = .picGraphic.Width
            End With
        End Sub

        Public Sub Blackout()
            '* Black out the screen and turn off visible stuff

            Me.BackColor = System.Drawing.Color.Black
            Me.StopVideo()
            Me.picGraphic.Visible = False
            Me.DisposeCurrentGraphicImage()
            Me.lblMsg.Visible = False
            Me.lblTeamLocLeft.Visible = False
            Me.lblTeamLocRight.Visible = False
            Me.lblTeamNameLeft.Visible = False
            Me.lblTeamNameRight.Visible = False
            Me.lblScoreLeft.Visible = False
            Me.lblScoreRight.Visible = False
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
            Me.lblTeamLocLeft.Visible = False
            Me.lblTeamLocRight.Visible = False
            Me.lblTeamNameLeft.Visible = False
            Me.lblTeamNameRight.Visible = False
            Me.lblScoreLeft.Visible = False
            Me.lblScoreRight.Visible = False
            Me.picGraphic.Visible = False
            Me.DisposeCurrentGraphicImage()

            Me.lblMsg.Font = New Font(Me.lblMsg.Font.Name, fontsize, Me.lblMsg.Font.Style)
            Me.lblMsg.BackColor = BackColor
            Me.lblMsg.Text = txt
            Me.lblMsg.Visible = True
        End Sub

        Public Sub ShowScore(ByVal scrLeft As String, ByVal locLeft As String, ByVal nameLeft As String, ByVal scrRight As String, ByVal locRight As String, ByVal nameRight As String)
            Dim ScoreBkgndAlreadySet As Boolean = Me.picGraphic.Image Is Me.ScoreboardBitMap
            Me.lblTeamLocLeft.BackColor = LeftTeamColor
            Me.lblTeamNameLeft.BackColor = LeftTeamColor
            Me.lblTeamLocLeft.Text = locLeft
            Me.lblTeamNameLeft.Text = nameLeft
            Me.lblTeamLocRight.BackColor = RightTeamColor
            Me.lblTeamNameRight.BackColor = RightTeamColor
            Me.lblTeamLocRight.Text = locRight
            Me.lblTeamNameRight.Text = nameRight
            If Not ScoreBkgndAlreadySet Then
                Me.picGraphic.Visible = False
                Me.DisposeCurrentGraphicImage() '* This will never dispose ScoreboardBitMap
            End If

            Me.StopVideo()
            Me.lblMsg.Visible = False

            '* If the scoreboard background is already there, skip reassignment and component
            '* redisplays so that we don't get unwanted blinking
            If Not ScoreBkgndAlreadySet Then Me.picGraphic.Image = Me.ScoreboardBitMap
            If Not Me.picGraphic.Visible = True Then Me.picGraphic.Visible = True

            If Not Me.lblTeamLocLeft.Visible = True Then Me.lblTeamLocLeft.Visible = True
            If Not Me.lblTeamNameLeft.Visible = True Then Me.lblTeamNameLeft.Visible = True
            If Not Me.lblTeamLocRight.Visible = True Then Me.lblTeamLocRight.Visible = True
            If Not Me.lblTeamNameRight.Visible = True Then Me.lblTeamNameRight.Visible = True
            SetZOrder()

            Dim fadeStarted As Boolean = False
            If Me.lblScoreLeft.Text <> scrLeft Then
                Me.lblScoreLeft.Text = Me.Limited_Score(scrLeft)
                FadeBuff(Me.lblScoreLeft, LeftTeamColor)
                fadeStarted = True
            Else
                Me.lblScoreLeft.ForeColor = SCORE_TEXT_COLOR
            End If
            If Me.lblScoreRight.Text <> scrRight Then
                Me.lblScoreRight.Text = Me.Limited_Score(scrRight)
                FadeBuff(Me.lblScoreRight, RightTeamColor)
                fadeStarted = True
            Else
                Me.lblScoreRight.ForeColor = SCORE_TEXT_COLOR
            End If
            Me.lblScoreLeft.Visible = True
            Me.lblScoreRight.Visible = True

            If Not fadeStarted Then RaiseEvent ScoreUpdateComplete()
        End Sub

        Public Sub ShowImage(ByVal Img As Image)
            If Img Is Nothing Then Exit Sub

            Me.BackColor = System.Drawing.Color.Black
            Me.StopVideo()
            Me.lblTeamLocLeft.Visible = False
            Me.lblTeamLocRight.Visible = False
            Me.lblTeamNameLeft.Visible = False
            Me.lblTeamNameRight.Visible = False
            Me.lblScoreLeft.Visible = False
            Me.lblScoreRight.Visible = False
            Me.lblMsg.Visible = False
            Me.DisposeCurrentGraphicImage()

            '* The Image has to be cloned. If it is just an object copy, animation info is lost
            Me.picGraphic.Image = DirectCast(Img.Clone(), Image)
            Me.picGraphic.Visible = True
        End Sub

        Public Function LaunchVideo(ByVal fnam As String) As String
            '* Returns an error message if there was a problem, otherwise returns Nothing
            '* NEVER modify the mute or volume settings here. They were set before we got here and that's what we want.

            'Static FirstInvoke As Boolean = True '* the first time a video is played, it takes a few milliseconds to start
            Dim resultMessage As String = Nothing
            Me.StopVideo()
            Me.lblTeamLocLeft.Visible = False
            Me.lblTeamLocRight.Visible = False
            Me.lblTeamNameLeft.Visible = False
            Me.lblTeamNameRight.Visible = False
            Me.lblScoreLeft.Visible = False
            Me.lblScoreRight.Visible = False
            Me.lblMsg.Visible = False
            Me.picGraphic.Visible = False

            Try
                Dim media As New LibVLCSharp.Shared.Media(Me._libVLC, fnam, LibVLCSharp.Shared.FromType.FromPath)
                _mediaPlayer.Play(media)
                media.Dispose()
                Me._videoView.Visible = True
            Catch ex As Exception
                resultMessage = "Error playing video file '" & fnam & "':" & vbCrLf & ex.Message
                Me.StopVideo()
            End Try

            Return resultMessage
        End Function
        Public Function IsVideoPlaying() As Boolean
            Return Me._mediaPlayer.IsPlaying
        End Function

        Public Sub SetVideoMute(ByVal newMuteSetting As Boolean)
            If Me._mediaPlayer Is Nothing Then Return
            Me._mediaPlayer.Mute = newMuteSetting
        End Sub
        Public Function GetVideoMute() As Boolean
            Return Me._mediaPlayer.Mute
        End Function

        Public Sub PauseVideo()
            Me._mediaPlayer.Pause()
        End Sub
        Public Function ResumeVideo() As String
            '* Returns an error message if there was a problem, otherwise returns Nothing
            Dim resultMessage As String = Nothing

            '* Can only resume if it's currently paused (CanPause is false)
            If Not Me._mediaPlayer.CanPause Then Return resultMessage

            Me.lblTeamLocLeft.Visible = False
            Me.lblTeamLocRight.Visible = False
            Me.lblTeamNameLeft.Visible = False
            Me.lblTeamNameRight.Visible = False
            Me.lblScoreLeft.Visible = False
            Me.lblScoreRight.Visible = False
            Me.lblMsg.Visible = False
            Me.picGraphic.Visible = False

            Try
                Me._mediaPlayer.Play()
                Me._videoView.Visible = True
            Catch ex As Exception
                resultMessage = "Error resuming video:" & vbCrLf & ex.Message
                Me.StopVideo()
            End Try

            Return resultMessage
        End Function
        Public Sub StopVideo()
            If Me._mediaPlayer Is Nothing Then Return
            Me._mediaPlayer.Stop()
            Me._videoView.Visible = False
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
        Private Sub SetZOrder()
            '* Let's get all the elements stacked in the right order.
            Me.picGraphic.SendToBack()
            Me._videoView.SendToBack()
            Me.lblMsg.BringToFront()
            Me.lblTeamLocLeft.BringToFront()
            Me.lblTeamLocRight.BringToFront()
            Me.lblTeamNameLeft.BringToFront()
            Me.lblTeamNameRight.BringToFront()
            Me.lblScoreLeft.BringToFront()
            Me.lblScoreRight.BringToFront()
            Me.lblCountdown.BringToFront()
        End Sub

        Private Sub DisposeCurrentGraphicImage()
            If Me.picGraphic.Image IsNot Nothing AndAlso Me.picGraphic.Image IsNot Me.ScoreboardBitMap Then
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
            Me.FadeTimer.Stop()
            Me.FadeTimer.Start()
        End Sub

        Private Sub FadeTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FadeTimer.Tick
            Dim changed As Boolean = False
            If Not (Me.lblScoreLeft.ForeColor.ToArgb() = SCORE_TEXT_COLOR.ToArgb()) Then
                Me.lblScoreLeft.ForeColor = Me.AddColorIncrement(Me.lblScoreLeft.ForeColor, LeftFadeIncrements)
                changed = True
            End If
            If Not (Me.lblScoreRight.ForeColor.ToArgb() = SCORE_TEXT_COLOR.ToArgb()) Then
                Me.lblScoreRight.ForeColor = Me.AddColorIncrement(Me.lblScoreRight.ForeColor, RightFadeIncrements)
                changed = True
            End If

            If Not changed Then
                Me.FadeTimer.Stop()
                RaiseEvent ScoreUpdateComplete()
            End If
        End Sub

#End Region
    End Class
End Namespace