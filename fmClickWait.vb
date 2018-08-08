Public Class fmClickWait
    Inherits System.Windows.Forms.Form
    Dim MyOwner As fmMain

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
    Friend WithEvents pnlContainMouse As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(fmClickWait))
        Me.pnlContainMouse = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'pnlContainMouse
        '
        Me.pnlContainMouse.BackColor = System.Drawing.Color.Yellow
        Me.pnlContainMouse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlContainMouse.Location = New System.Drawing.Point(302, 225)
        Me.pnlContainMouse.Name = "pnlContainMouse"
        Me.pnlContainMouse.Size = New System.Drawing.Size(187, 108)
        Me.pnlContainMouse.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(94, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(591, 125)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "To stop the Whammy! random selector, click the mouse" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "or hit any key." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Your mou" & _
        "se will not be able to leave the yellow box until a choice has been made. Theore" & _
        "tically."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fmClickWait
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(10, 24)
        Me.ClientSize = New System.Drawing.Size(764, 538)
        Me.ControlBox = False
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.Label1, Me.pnlContainMouse})
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
        Me.CenterFormOnParent()
        Me.CenterMouseOnForm()
        Me.pnlContainMouse.Focus()
    End Sub
    Private Sub fmClickWait_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        MyOwner.StopSlideShow()
    End Sub

    Private Sub fmClickWait_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown, pnlContainMouse.KeyDown
        e.Handled = True
        Me.Close()
    End Sub
    Private Sub fmClickWait_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown, pnlContainMouse.MouseDown
        Me.Close()
    End Sub

    Private Sub fmClickWait_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MouseLeave, pnlContainMouse.MouseLeave
        '* Prevent the mouse from leaving this form.
        Me.pnlContainMouse.Focus()
        Me.CenterMouseOnForm()
    End Sub

    Private Sub CenterMouseOnForm()
        With Me.pnlContainMouse
            System.Windows.Forms.Cursor.Position = New Point(Me.Left + .Left + (.Width / 2), Me.Top + .Top + (.Height / 2))
        End With
    End Sub
    Private Sub CenterFormOnParent()
        Me.Left = MyOwner.Left + ((MyOwner.Width - Me.Width) / 2)
        Me.Top = MyOwner.Top + (MyOwner.Height - Me.Height) - 2
    End Sub

    Private Sub fmClickWait_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.LostFocus
        Me.pnlContainMouse.Focus()
        Me.CenterMouseOnForm()
        Me.BringToFront()
    End Sub
End Class
