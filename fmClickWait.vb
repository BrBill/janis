Public Class fmClickWait
    Inherits System.Windows.Forms.Form
    Dim MyOwner As JANIS.fmMain

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal Owner As Form)
        MyBase.New()
        Me.MyOwner = Owner

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fmClickWait))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(94, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(591, 125)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "To stop the Whammy! random selector, click the mouse" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " in this window, or hit any" & _
    " key." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If you click somewhere else, I can't be held responsible for what happe" & _
    "ns."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fmClickWait
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(10, 24)
        Me.BackColor = System.Drawing.Color.Yellow
        Me.ClientSize = New System.Drawing.Size(764, 538)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.AppStarting
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fmClickWait"
        Me.Text = "WHAMMY! - Waiting for user input"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub fmClickWait_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(MyOwner.Left + ((MyOwner.Width - Me.Width) / 2), MyOwner.Top + ((MyOwner.Height - Me.Height)))
        Me.CenterMouseOnForm()
        Me.Focus()
        Me.BringToFront()
    End Sub
    Private Sub fmClickWait_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        MyOwner.StopSlideShow()
    End Sub

    Private Sub fmClickWait_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        e.Handled = True
        Me.Close()
    End Sub
    Private Sub fmClickWait_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown, Label1.MouseDown
        Me.Close()
    End Sub

    Private Sub fmClickWait_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        '* Prevent the mouse from leaving this form.
        Me.Focus()
        Me.CenterMouseOnForm()
    End Sub

    Private Sub CenterMouseOnForm()
        '* This probably doesn't work in Windows newer than XP.
        System.Windows.Forms.Cursor.Position = New Point(Me.Left + (Me.Width / 2), Me.Top + (Me.Height / 2))
    End Sub

    Private Sub fmClickWait_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        Me.Focus()
        Me.CenterMouseOnForm()
        Me.BringToFront()
    End Sub

End Class
