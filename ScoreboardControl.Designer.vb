<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScoreboardControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lblTeamLocRight = New System.Windows.Forms.Label()
        Me.lblTeamNameRight = New System.Windows.Forms.Label()
        Me.lblTeamNameLeft = New System.Windows.Forms.Label()
        Me.lblTeamLocLeft = New System.Windows.Forms.Label()
        Me.lblScoreLeft = New System.Windows.Forms.Label()
        Me.lblScoreRight = New System.Windows.Forms.Label()
        Me.FadeTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'lblTeamLocRight
        '
        Me.lblTeamLocRight.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblTeamLocRight.BackColor = System.Drawing.Color.Maroon
        Me.lblTeamLocRight.Font = New System.Drawing.Font("Roboto Slab", 33.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTeamLocRight.ForeColor = System.Drawing.Color.White
        Me.lblTeamLocRight.Location = New System.Drawing.Point(843, 53)
        Me.lblTeamLocRight.Name = "lblTeamLocRight"
        Me.lblTeamLocRight.Size = New System.Drawing.Size(412, 65)
        Me.lblTeamLocRight.TabIndex = 0
        Me.lblTeamLocRight.Text = "Right Team City" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.lblTeamLocRight.TextAlign = System.Drawing.ContentAlignment.BottomRight
        Me.lblTeamLocRight.UseCompatibleTextRendering = True
        '
        'lblTeamNameRight
        '
        Me.lblTeamNameRight.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblTeamNameRight.BackColor = System.Drawing.Color.Maroon
        Me.lblTeamNameRight.Font = New System.Drawing.Font("Roboto Slab", 33.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTeamNameRight.ForeColor = System.Drawing.Color.White
        Me.lblTeamNameRight.Location = New System.Drawing.Point(843, 118)
        Me.lblTeamNameRight.Name = "lblTeamNameRight"
        Me.lblTeamNameRight.Size = New System.Drawing.Size(412, 65)
        Me.lblTeamNameRight.TabIndex = 1
        Me.lblTeamNameRight.Text = "Right Team Name"
        Me.lblTeamNameRight.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblTeamNameRight.UseCompatibleTextRendering = True
        '
        'lblTeamNameLeft
        '
        Me.lblTeamNameLeft.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblTeamNameLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
        Me.lblTeamNameLeft.Font = New System.Drawing.Font("Roboto Slab", 33.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTeamNameLeft.ForeColor = System.Drawing.Color.White
        Me.lblTeamNameLeft.Location = New System.Drawing.Point(25, 118)
        Me.lblTeamNameLeft.Name = "lblTeamNameLeft"
        Me.lblTeamNameLeft.Size = New System.Drawing.Size(412, 65)
        Me.lblTeamNameLeft.TabIndex = 3
        Me.lblTeamNameLeft.Text = "Left Team Name"
        Me.lblTeamNameLeft.UseCompatibleTextRendering = True
        '
        'lblTeamLocLeft
        '
        Me.lblTeamLocLeft.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblTeamLocLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
        Me.lblTeamLocLeft.Font = New System.Drawing.Font("Roboto Slab", 33.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTeamLocLeft.ForeColor = System.Drawing.Color.White
        Me.lblTeamLocLeft.Location = New System.Drawing.Point(25, 53)
        Me.lblTeamLocLeft.Name = "lblTeamLocLeft"
        Me.lblTeamLocLeft.Size = New System.Drawing.Size(412, 65)
        Me.lblTeamLocLeft.TabIndex = 2
        Me.lblTeamLocLeft.Text = "Left Team City" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.lblTeamLocLeft.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.lblTeamLocLeft.UseCompatibleTextRendering = True
        '
        'lblScoreLeft
        '
        Me.lblScoreLeft.Font = New System.Drawing.Font("Roboto Slab", 220.0!, System.Drawing.FontStyle.Bold)
        Me.lblScoreLeft.ForeColor = System.Drawing.Color.White
        Me.lblScoreLeft.Location = New System.Drawing.Point(-13, 284)
        Me.lblScoreLeft.Name = "lblScoreLeft"
        Me.lblScoreLeft.Size = New System.Drawing.Size(602, 416)
        Me.lblScoreLeft.TabIndex = 4
        Me.lblScoreLeft.Text = "768"
        Me.lblScoreLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblScoreLeft.UseCompatibleTextRendering = True
        '
        'lblScoreRight
        '
        Me.lblScoreRight.Font = New System.Drawing.Font("Roboto Slab", 220.0!, System.Drawing.FontStyle.Bold)
        Me.lblScoreRight.ForeColor = System.Drawing.Color.White
        Me.lblScoreRight.Location = New System.Drawing.Point(691, 284)
        Me.lblScoreRight.Name = "lblScoreRight"
        Me.lblScoreRight.Size = New System.Drawing.Size(602, 416)
        Me.lblScoreRight.TabIndex = 5
        Me.lblScoreRight.Text = "768"
        Me.lblScoreRight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblScoreRight.UseCompatibleTextRendering = True
        '
        'FadeTimer
        '
        Me.FadeTimer.Interval = 60
        '
        'Scoreboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.BackgroundImage = Global.JANIS.My.Resources.Resources.ScoreTemplate
        Me.Controls.Add(Me.lblScoreRight)
        Me.Controls.Add(Me.lblScoreLeft)
        Me.Controls.Add(Me.lblTeamNameLeft)
        Me.Controls.Add(Me.lblTeamLocLeft)
        Me.Controls.Add(Me.lblTeamNameRight)
        Me.Controls.Add(Me.lblTeamLocRight)
        Me.Name = "Scoreboard"
        Me.Size = New System.Drawing.Size(1280, 720)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTeamLocRight As System.Windows.Forms.Label
    Friend WithEvents lblTeamNameRight As System.Windows.Forms.Label
    Friend WithEvents lblTeamNameLeft As System.Windows.Forms.Label
    Friend WithEvents lblTeamLocLeft As System.Windows.Forms.Label
    Friend WithEvents lblScoreLeft As System.Windows.Forms.Label
    Friend WithEvents lblScoreRight As System.Windows.Forms.Label
    Friend WithEvents FadeTimer As System.Windows.Forms.Timer

End Class
