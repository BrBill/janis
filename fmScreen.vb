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
    Friend WithEvents lblMsg As System.Windows.Forms.Label
    Friend WithEvents lblScore As System.Windows.Forms.Label
    Friend WithEvents lblCountdown As System.Windows.Forms.Label
    Friend WithEvents lblTeamName As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.picGraphic = New System.Windows.Forms.PictureBox
        Me.lblMsg = New System.Windows.Forms.Label
        Me.lblScore = New System.Windows.Forms.Label
        Me.lblTeamName = New System.Windows.Forms.Label
        Me.lblCountdown = New System.Windows.Forms.Label
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
        Me.lblMsg.Location = New System.Drawing.Point(0, 0)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMsg.Size = New System.Drawing.Size(1280, 720)
        Me.lblMsg.TabIndex = 1
        Me.lblMsg.Text = "Welcome to JANIS"
        Me.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblScore
        '
        Me.lblScore.BackColor = System.Drawing.Color.Transparent
        Me.lblScore.Font = New System.Drawing.Font("Arial", 360.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScore.Location = New System.Drawing.Point(0, 150)
        Me.lblScore.Name = "lblScore"
        Me.lblScore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblScore.Size = New System.Drawing.Size(1280, 570)
        Me.lblScore.TabIndex = 3
        Me.lblScore.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblScore.Visible = False
        '
        'lblTeamName
        '
        Me.lblTeamName.BackColor = System.Drawing.Color.Transparent
        Me.lblTeamName.Font = New System.Drawing.Font("Arial", 54.0!, System.Drawing.FontStyle.Bold)
        Me.lblTeamName.Location = New System.Drawing.Point(0, 0)
        Me.lblTeamName.Name = "lblTeamName"
        Me.lblTeamName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTeamName.Size = New System.Drawing.Size(1280, 150)
        Me.lblTeamName.TabIndex = 3
        Me.lblTeamName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblTeamName.Visible = False
        '
        'lblCountdown
        '
        Me.lblCountdown.BackColor = System.Drawing.Color.Black
        Me.lblCountdown.Font = New System.Drawing.Font("Arial Black", 70.0!, System.Drawing.FontStyle.Bold)
        Me.lblCountdown.Location = New System.Drawing.Point(0, 600)
        Me.lblCountdown.Name = "lblCountdown"
        Me.lblCountdown.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountdown.Size = New System.Drawing.Size(1280, 120)
        Me.lblCountdown.TabIndex = 7
        Me.lblCountdown.Text = "00:00:00"
        Me.lblCountdown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblCountdown.Visible = False
        '
        'fmScreen
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1280, 720)
        Me.Controls.Add(Me.lblCountdown)
        Me.Controls.Add(Me.lblTeamName)
        Me.Controls.Add(Me.lblMsg)
        Me.Controls.Add(Me.picGraphic)
        Me.Controls.Add(Me.lblScore)
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

    Public Sub fmScreen_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.MouseEnter, lblMsg.MouseEnter, lblScore.MouseEnter, lblTeamName.MouseEnter, picGraphic.MouseEnter
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
        Me.lblMsg.Visible = False
        Me.lblScore.Visible = False
        Me.lblTeamName.Visible = False
    End Sub

    Public Sub ShowText(ByVal txt As String, ByVal color As System.Drawing.Color, ByVal fontsize As Integer)
        Me.lblScore.Visible = False
        Me.lblTeamName.Visible = False
        Me.picGraphic.Visible = False
        Me.picGraphic.ImageLocation = ""
        Me.lblMsg.Font = New Font(Me.lblMsg.Font.Name, fontsize, Me.lblMsg.Font.Style)
        Me.lblMsg.Visible = True
        Me.lblMsg.Text = txt
        Me.BackColor = color
    End Sub

    Public Sub ShowScore(ByVal Score As String, ByVal Team As String, ByVal BackColor As System.Drawing.Color)
        Me.lblTeamName.BackColor = BackColor
        Me.lblScore.BackColor = BackColor
        Me.lblMsg.Visible = False
        Me.picGraphic.Visible = False
        Me.picGraphic.ImageLocation = ""
        Me.lblScore.Text = Score
        Me.lblTeamName.Text = Team
        Me.lblScore.Visible = True
        Me.lblTeamName.Visible = True
    End Sub

    Public Sub ShowImage(ByRef Img As Image, ByVal Expand As Boolean)
        If Img Is Nothing Then Exit Sub

        Dim monitor_width_height_ratio As Single = CSng(Me.Width) / CSng(Me.Height)
        Dim image_width_height_ratio As Single = CSng(Img.Width) / CSng(Img.Height)
        Dim ratio_compare As Single = image_width_height_ratio / monitor_width_height_ratio
        Dim new_height, new_width As Integer

        Me.BackColor = System.Drawing.Color.Black
        Me.lblScore.Visible = False
        Me.lblTeamName.Visible = False
        Me.lblMsg.Visible = False

        If Expand Or (ratio_compare < 1.15 And ratio_compare > 0.85) Then
            '* close enough to equal or in stretch mode
            new_width = Me.Width
            new_height = Me.Height
        ElseIf ratio_compare > 1 Then   '* wider/shorter
            new_width = Me.Width
            new_height = CInt(new_width / image_width_height_ratio)
        Else    '* narrower/taller
            new_height = Me.Height
            new_width = CInt(new_height * image_width_height_ratio)
        End If

        '* Float the picture in the middle if appropriate
        If new_width <> Me.picGraphic.Width Then
            Me.picGraphic.Left = (Me.Width - new_width) / 2
            Me.picGraphic.Width = new_width
        End If
        If new_height <> Me.picGraphic.Height Then
            Me.picGraphic.Top = (Me.Height - new_height) / 2
            Me.picGraphic.Height = new_height
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

    '    '* when loading URL, we always show as expanded because we don't know pic info.
    '    Me.picGraphic.Left = 0
    '    Me.picGraphic.Width = Me.Width
    '    Me.picGraphic.Top = 0
    '    Me.picGraphic.Height = Me.Height

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

        Me.lblCountdown.Visible = CountdownVisible
        Me.lblCountdown.Text = CountdownText
    End Sub

End Class
