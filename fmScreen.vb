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
    Friend WithEvents lblTeamName As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.picGraphic = New System.Windows.Forms.PictureBox()
        Me.lblMsg = New System.Windows.Forms.Label()
        Me.lblScore = New System.Windows.Forms.Label()
        Me.lblTeamName = New System.Windows.Forms.Label()
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
        'lblScore
        '
        Me.lblScore.BackColor = System.Drawing.Color.Transparent
        Me.lblScore.Font = New System.Drawing.Font("Arial", 280.0!, System.Drawing.FontStyle.Bold)
        Me.lblScore.Location = New System.Drawing.Point(0, 100)
        Me.lblScore.Name = "lblScore"
        Me.lblScore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblScore.Size = New System.Drawing.Size(800, 700)
        Me.lblScore.TabIndex = 3
        Me.lblScore.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblScore.Visible = False
        '
        'lblTeamName
        '
        Me.lblTeamName.BackColor = System.Drawing.Color.Transparent
        Me.lblTeamName.Font = New System.Drawing.Font("Arial", 54.0!, System.Drawing.FontStyle.Bold)
        Me.lblTeamName.Name = "lblTeamName"
        Me.lblTeamName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTeamName.Size = New System.Drawing.Size(800, 150)
        Me.lblTeamName.TabIndex = 3
        Me.lblTeamName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblTeamName.Visible = False
        '
        'fmScreen
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(192, Byte))
        Me.ClientSize = New System.Drawing.Size(800, 600)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblTeamName, Me.lblMsg, Me.picGraphic, Me.lblScore})
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
