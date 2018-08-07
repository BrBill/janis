Public Class fmDirDialog
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
    Friend WithEvents FolderTree1 As HyperCoder.Win.FileSystemControls.FolderTree
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(fmDirDialog))
        Me.FolderTree1 = New HyperCoder.Win.FileSystemControls.FolderTree()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'FolderTree1
        '
        Me.FolderTree1.Cursor = System.Windows.Forms.Cursors.Default
        Me.FolderTree1.FullRowSelect = True
        Me.FolderTree1.HideSelection = False
        Me.FolderTree1.IconSize = HyperCoder.Win.FileSystemControls.FolderTree.IconSize2Display.Small
        Me.FolderTree1.ImageIndex = -1
        Me.FolderTree1.IncludeFiles = False
        Me.FolderTree1.Location = New System.Drawing.Point(8, 8)
        Me.FolderTree1.Name = "FolderTree1"
        Me.FolderTree1.RootFolder = "Desktop"
        Me.FolderTree1.SelectedImageIndex = -1
        Me.FolderTree1.ShowHiddenItems = False
        Me.FolderTree1.ShowSystemItems = False
        Me.FolderTree1.Size = New System.Drawing.Size(344, 248)
        Me.FolderTree1.TabIndex = 0
        Me.FolderTree1.Text = "FolderTree"
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(248, 264)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(104, 32)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(8, 264)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(104, 32)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        '
        'fmDirDialog
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(360, 301)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnCancel, Me.btnOK, Me.FolderTree1})
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(368, 328)
        Me.Name = "fmDirDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Directory"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub fmDirDialog_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        '* When we resize the dialog box, resize the items within it.
        Me.btnOK.Top = Me.Height - Me.btnOK.Height - 32
        Me.btnCancel.Top = Me.btnOK.Top
        Me.FolderTree1.Height = Me.Height - 80
        Me.FolderTree1.Width = Me.Width - 24
    End Sub

End Class
