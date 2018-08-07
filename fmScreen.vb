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
    Friend WithEvents lblScoreLeft As System.Windows.Forms.Label
    Friend WithEvents lblScoreRight As System.Windows.Forms.Label
    Friend WithEvents lblTeamNameLeft As System.Windows.Forms.Label
    Friend WithEvents lblTeamNameRight As System.Windows.Forms.Label
    Friend WithEvents lblCountdown As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.picGraphic = New System.Windows.Forms.PictureBox()
        Me.lblMsg = New System.Windows.Forms.Label()
        Me.lblScoreLeft = New System.Windows.Forms.Label()
        Me.lblTeamNameLeft = New System.Windows.Forms.Label()
        Me.lblScoreRight = New System.Windows.Forms.Label()
        Me.lblTeamNameRight = New System.Windows.Forms.Label()
        Me.lblCountdown = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'picGraphic
        '
        Me.picGraphic.Name = "picGraphic"
        Me.picGraphic.Size = New System.Drawing.Size(800, 600)
        Me.picGraphic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picGraphic.TabIndex = 0
        Me.picGraphic.TabStop = False
        '
        'lblMsg
        '
        Me.lblMsg.BackColor = System.Drawing.Color.Transparent
        Me.lblMsg.Font = New System.Drawing.Font("Arial", 123.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMsg.Location = New System.Drawing.Point(0, 40)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMsg.Size = New System.Drawing.Size(800, 560)
        Me.lblMsg.TabIndex = 1
        Me.lblMsg.Text = "Welcome to JANIS"
        Me.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblScoreLeft
        '
        Me.lblScoreLeft.BackColor = System.Drawing.Color.Transparent
        Me.lblScoreLeft.Font = New System.Drawing.Font("Arial", 160.0!, System.Drawing.FontStyle.Bold)
        Me.lblScoreLeft.Location = New System.Drawing.Point(0, 100)
        Me.lblScoreLeft.Name = "lblScoreLeft"
        Me.lblScoreLeft.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblScoreLeft.Size = New System.Drawing.Size(400, 500)
        Me.lblScoreLeft.TabIndex = 3
        Me.lblScoreLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblScoreLeft.Visible = False
        '
        'lblTeamNameLeft
        '
        Me.lblTeamNameLeft.BackColor = System.Drawing.Color.Transparent
        Me.lblTeamNameLeft.Font = New System.Drawing.Font("Arial Black", 30.0!, System.Drawing.FontStyle.Bold)
        Me.lblTeamNameLeft.Name = "lblTeamNameLeft"
        Me.lblTeamNameLeft.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTeamNameLeft.Size = New System.Drawing.Size(400, 150)
        Me.lblTeamNameLeft.TabIndex = 3
        Me.lblTeamNameLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblTeamNameLeft.Visible = False
        '
        'lblScoreRight
        '
        Me.lblScoreRight.BackColor = System.Drawing.Color.Maroon
        Me.lblScoreRight.Font = New System.Drawing.Font("Arial", 160.0!, System.Drawing.FontStyle.Bold)
        Me.lblScoreRight.Location = New System.Drawing.Point(400, 100)
        Me.lblScoreRight.Name = "lblScoreRight"
        Me.lblScoreRight.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblScoreRight.Size = New System.Drawing.Size(400, 500)
        Me.lblScoreRight.TabIndex = 4
        Me.lblScoreRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblScoreRight.Visible = False
        '
        'lblTeamNameRight
        '
        Me.lblTeamNameRight.BackColor = System.Drawing.Color.Maroon
        Me.lblTeamNameRight.Font = New System.Drawing.Font("Arial Black", 30.0!, System.Drawing.FontStyle.Bold)
        Me.lblTeamNameRight.Location = New System.Drawing.Point(400, 0)
        Me.lblTeamNameRight.Name = "lblTeamNameRight"
        Me.lblTeamNameRight.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTeamNameRight.Size = New System.Drawing.Size(400, 150)
        Me.lblTeamNameRight.TabIndex = 5
        Me.lblTeamNameRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblTeamNameRight.Visible = False
        '
        'lblCountdown
        '
        Me.lblCountdown.BackColor = System.Drawing.Color.Black
        Me.lblCountdown.Font = New System.Drawing.Font("Arial Black", 70.0!, System.Drawing.FontStyle.Bold)
        Me.lblCountdown.Location = New System.Drawing.Point(0, 480)
        Me.lblCountdown.Name = "lblCountdown"
        Me.lblCountdown.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCountdown.Size = New System.Drawing.Size(800, 120)
        Me.lblCountdown.TabIndex = 6
        Me.lblCountdown.Text = "00:00:00"
        Me.lblCountdown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblCountdown.Visible = False
        '
        'fmScreen
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(192, Byte))
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblCountdown, Me.lblTeamNameLeft, Me.lblMsg, Me.picGraphic, Me.lblScoreLeft, Me.lblTeamNameRight, Me.lblScoreRight})
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Location = New System.Drawing.Point(800, 0)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(800, 600)
        Me.MinimizeBox = False
        Me.Name = "fmScreen"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Function MyLeft()
        Return Me.Left
    End Function

    Public Sub fmScreen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picGraphic.Click

    End Sub
End Class
