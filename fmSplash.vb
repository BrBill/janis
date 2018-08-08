Public Class fmSplash
    Inherits System.Windows.Forms.Form

    Class ApplicationInformation
        Public Title As String = "JANIS"
        Public MajorVersion As Integer = 3
        Public MinorVersion As Integer = 2
        Public Iteration As Integer = 1
        Public ProductName As String = "SINGLE SCREEN"
        Public Copyright As String = "2004-2018"
    End Class


#Region " Windows Form Designer generated code "

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblAuthor As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents lblCopyright As System.Windows.Forms.Label
    Friend WithEvents lblVersionInfo As System.Windows.Forms.Label
    Friend WithEvents lblAppDesc As System.Windows.Forms.Label
    Friend WithEvents lblAppTitle As System.Windows.Forms.Label

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fmSplash))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblAuthor = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblCopyright = New System.Windows.Forms.Label()
        Me.lblVersionInfo = New System.Windows.Forms.Label()
        Me.lblAppDesc = New System.Windows.Forms.Label()
        Me.lblAppTitle = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 20)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(232, 232)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'lblAuthor
        '
        Me.lblAuthor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAuthor.Location = New System.Drawing.Point(267, 170)
        Me.lblAuthor.Name = "lblAuthor"
        Me.lblAuthor.Size = New System.Drawing.Size(209, 16)
        Me.lblAuthor.TabIndex = 12
        Me.lblAuthor.Text = "by Bill Cernansky"
        Me.lblAuthor.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblStatus
        '
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Green
        Me.lblStatus.Location = New System.Drawing.Point(250, 224)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(226, 28)
        Me.lblStatus.TabIndex = 11
        Me.lblStatus.Text = "Status: Initializing..."
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCopyright
        '
        Me.lblCopyright.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCopyright.Location = New System.Drawing.Point(267, 145)
        Me.lblCopyright.Name = "lblCopyright"
        Me.lblCopyright.Size = New System.Drawing.Size(209, 16)
        Me.lblCopyright.TabIndex = 10
        Me.lblCopyright.Text = "Copyright 2004-2018"
        Me.lblCopyright.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblVersionInfo
        '
        Me.lblVersionInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersionInfo.Location = New System.Drawing.Point(279, 96)
        Me.lblVersionInfo.Name = "lblVersionInfo"
        Me.lblVersionInfo.Size = New System.Drawing.Size(201, 25)
        Me.lblVersionInfo.TabIndex = 9
        Me.lblVersionInfo.Text = "Version {0}.{1}.{2} {3}"
        Me.lblVersionInfo.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblAppDesc
        '
        Me.lblAppDesc.AutoSize = True
        Me.lblAppDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAppDesc.Location = New System.Drawing.Point(343, 66)
        Me.lblAppDesc.Name = "lblAppDesc"
        Me.lblAppDesc.Size = New System.Drawing.Size(133, 16)
        Me.lblAppDesc.TabIndex = 8
        Me.lblAppDesc.Text = "The Improv Assistant"
        Me.lblAppDesc.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblAppTitle
        '
        Me.lblAppTitle.AutoSize = True
        Me.lblAppTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAppTitle.Location = New System.Drawing.Point(367, 20)
        Me.lblAppTitle.Name = "lblAppTitle"
        Me.lblAppTitle.Size = New System.Drawing.Size(113, 37)
        Me.lblAppTitle.TabIndex = 7
        Me.lblAppTitle.Text = "JANIS"
        Me.lblAppTitle.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'fmSplash
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(492, 273)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblAuthor)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblCopyright)
        Me.Controls.Add(Me.lblVersionInfo)
        Me.Controls.Add(Me.lblAppDesc)
        Me.Controls.Add(Me.lblAppTitle)
        Me.Controls.Add(Me.PictureBox1)
        Me.Cursor = System.Windows.Forms.Cursors.AppStarting
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Location = New System.Drawing.Point(300, 300)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fmSplash"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Starting JANIS..."
        Me.TopMost = True
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Private T As Timer
    Public bOwnerFinishedLoading As Boolean
    Private AppInfo As New ApplicationInformation()

    'constructors
    'the default constructor has been removed from " Windows Form Designer generated code "
    'First constructor: only show the splash while the form is loading
    Public Sub New(ByVal Owner As Form)
        MyBase.New()
        Me.Owner = Owner
        'add an eventhandler for the activated event of the calling
        'form. If the load event was trapped, the splash screen
        'would not be visible while the loading of the calling
        'form takes place. (Activated event fires after the load
        'event has finished)
        AddHandler Owner.Activated, AddressOf Me.Done
        'InitializeComponent is needed to initialize the splash forms controls
        InitializeComponent()
        Me.Show()
        'call doevents to ensure the splash is shown, before the
        'initialization of the main form continues
        Application.DoEvents()
    End Sub

    '2nd constructor: show the splash for a minimum amount of time.
    'If the form loading takes longer, the splash is shown while the
    'form is loading, but it's always at least shown the number of 
    'seconds given.
    Public Sub New(ByVal Owner As Form, ByVal TimeToShow As Integer)
        'call first constructor
        Me.New(Owner)
        'initialize timer object
        T = New Timer()
        T.Interval = TimeToShow * 1000
        T.Enabled = True
        AddHandler T.Tick, AddressOf Me.Done
        '* AddHandler Me.Owner.VisibleChanged, AddressOf HideOwner
    End Sub

    'this sub hides the main form, if a minimum time to
    'show the splash has been given and the loading is complete
    'before that time has elapsed
    'since hiding the main form would cause the application to exit,
    'the form is made transparent
    Private Sub HideOwner(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim F As Form = CType(sender, Form)
        F.Opacity = 0
    End Sub

    'this sub fires when the loading of the calling form is complete
    'If a minimum show time has been given, it also fires when that
    'time has elapsed
    Private Sub Done(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If sender Is Me.Owner Then
            'owner has finished loading
            'set flag
            bOwnerFinishedLoading = True
            'remove the handler to ensure this sub will not fire again 
            'for the activated event.
            RemoveHandler Owner.Activated, AddressOf Me.Done
            'if there is no time set, or that time has elapsed already,
            'the splash can be closed
            If T Is Nothing Then
                Finish()
            End If
        ElseIf bOwnerFinishedLoading Then
            'timer ticked, owner has finished initializing
            Finish()
        Else
            'timer has ticked, but owner hasn't finished initialzing
            'yet, wait for owner to finish.
            'set the timer object to nothing, so when the owning form
            'has finished loading, it will know it can close the splash
            '* If Not Owner Is Nothing Then RemoveHandler Owner.VisibleChanged, AddressOf HideOwner
            T.Enabled = False
            T = Nothing
        End If
    End Sub

    'this sub is called when either the loading is completed or
    'the time has elapsed, depending on the settings
    'This method can also be called from the calling form to 
    'close the splash before these events occur
    Public Sub Finish()
        If Not T Is Nothing Then T.Dispose()
        If Me.Owner IsNot Nothing Then
            Me.Owner.Opacity = 100
        End If
        Me.Close()
    End Sub

    Private Sub fmSplashScreen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        'Application title
        Me.lblAppTitle.Text = Me.AppInfo.Title

        'Format the version information using the text set into the Version control at design time as the
        '  formatting string.  This allows for effective localization if desired.
        '  Build and revision information could be included by using the following code and changing the 
        '  Version control's designtime text to "Version {0}.{1:00}.{2}.{3}" or something similar.  See
        '  String.Format() in Help for more information.
        '
        '    Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build, My.Application.Info.Version.Revision)

        Me.lblVersionInfo.Text = System.String.Format(Me.lblVersionInfo.Text, Me.AppInfo.MajorVersion, Me.AppInfo.MinorVersion, Me.AppInfo.Iteration, Me.AppInfo.ProductName)
        'Copyright info
        Me.lblCopyright.Text = "Copyright " + Me.AppInfo.Copyright
        System.Windows.Forms.Application.DoEvents()
    End Sub

    Public Sub SetStatus(ByVal MyText As String)
        Me.lblStatus.Text = "Status: " + MyText
        System.Windows.Forms.Application.DoEvents()
    End Sub
End Class
