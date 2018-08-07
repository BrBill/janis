Public Class fmMain
    Inherits System.Windows.Forms.Form

    '***************
    '* System Constants
    '***************
    Const MAX_THINGS As Integer = 9
    Const EOL As String = Chr(13) + Chr(10)
    Const FontMult As Single = 123 / 41  ' This is the size ratio of fonts in the display vs. in the textbox
    Const ROOT_SUPPORT_DIR As String = "C:\Program Files\Multiple_Display"

    Dim LS As fmScreen      '* The left team screen
    Dim RS As fmScreen      '* The right team screen
    Dim LeftDefaultColor As System.Drawing.Color
    Dim RightDefaultColor As System.Drawing.Color
    Dim ThingSubs(MAX_THINGS) As String             '* Substitutions for 5 Things
    Dim SlidesActive As Boolean = False             '* Keep track of whether Slideshow is running

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
    Friend WithEvents btnBlackout As System.Windows.Forms.Button
    Friend WithEvents tbLeftTeam As System.Windows.Forms.TextBox
    Friend WithEvents tbRightTeam As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbLeftScore As System.Windows.Forms.TextBox
    Friend WithEvents tbRightScore As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnScoreLeft As System.Windows.Forms.Button
    Friend WithEvents btnScoreRight As System.Windows.Forms.Button
    Friend WithEvents btnScoreBoth As System.Windows.Forms.Button
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents menuSubtract1Left As System.Windows.Forms.MenuItem
    Friend WithEvents menuAdd5Left As System.Windows.Forms.MenuItem
    Friend WithEvents menuSubtract5Left As System.Windows.Forms.MenuItem
    Friend WithEvents menuAdd1Right As System.Windows.Forms.MenuItem
    Friend WithEvents menuSubtract1Right As System.Windows.Forms.MenuItem
    Friend WithEvents menuAdd5Right As System.Windows.Forms.MenuItem
    Friend WithEvents menuSubtract5Right As System.Windows.Forms.MenuItem
    Friend WithEvents picLeft As System.Windows.Forms.PictureBox
    Friend WithEvents picRight As System.Windows.Forms.PictureBox
    Friend WithEvents btnPictureLeft As System.Windows.Forms.Button
    Friend WithEvents btnPictureRight As System.Windows.Forms.Button
    Friend WithEvents btnPictureBoth As System.Windows.Forms.Button
    Friend WithEvents btnLeftScoreColor As System.Windows.Forms.Button
    Friend WithEvents btnRightScoreColor As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tbLeftFontSize As System.Windows.Forms.TextBox
    Friend WithEvents grpLeftColors As System.Windows.Forms.GroupBox
    Friend WithEvents btnLeftTextColor As System.Windows.Forms.Button
    Friend WithEvents pnlLeftColor6 As System.Windows.Forms.Panel
    Friend WithEvents pnlLeftColor5 As System.Windows.Forms.Panel
    Friend WithEvents pnlLeftColor4 As System.Windows.Forms.Panel
    Friend WithEvents pnlLeftColor3 As System.Windows.Forms.Panel
    Friend WithEvents pnlLeftColor2 As System.Windows.Forms.Panel
    Friend WithEvents pnlLeftColor1 As System.Windows.Forms.Panel
    Friend WithEvents tbRightFontSize As System.Windows.Forms.TextBox
    Friend WithEvents grpRightColors As System.Windows.Forms.GroupBox
    Friend WithEvents btnRightTextColor As System.Windows.Forms.Button
    Friend WithEvents pnlRightColor6 As System.Windows.Forms.Panel
    Friend WithEvents pnlRightColor5 As System.Windows.Forms.Panel
    Friend WithEvents pnlRightColor4 As System.Windows.Forms.Panel
    Friend WithEvents pnlRightColor3 As System.Windows.Forms.Panel
    Friend WithEvents pnlRightColor2 As System.Windows.Forms.Panel
    Friend WithEvents pnlRightColor1 As System.Windows.Forms.Panel
    Friend WithEvents tbRightText As System.Windows.Forms.TextBox
    Friend WithEvents tbLeftText As System.Windows.Forms.TextBox
    Friend WithEvents lblCurrentThing As System.Windows.Forms.Label
    Friend WithEvents btnListRight As System.Windows.Forms.Button
    Friend WithEvents btnListBoth As System.Windows.Forms.Button
    Friend WithEvents btnListLeft As System.Windows.Forms.Button
    Friend WithEvents btnShowThingRight As System.Windows.Forms.Button
    Friend WithEvents btnShowThingBoth As System.Windows.Forms.Button
    Friend WithEvents btnShowThingLeft As System.Windows.Forms.Button
    Friend WithEvents tbSubstitutions As System.Windows.Forms.TextBox
    Friend WithEvents tbNewThing As System.Windows.Forms.TextBox
    Friend WithEvents grpThingsColor As System.Windows.Forms.GroupBox
    Friend WithEvents radioThingColorLeft As System.Windows.Forms.RadioButton
    Friend WithEvents radioThingColorRight As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tpScreenText As System.Windows.Forms.TabPage
    Friend WithEvents tp5Things As System.Windows.Forms.TabPage
    Friend WithEvents tpSlides As System.Windows.Forms.TabPage
    Friend WithEvents tpAbout As System.Windows.Forms.TabPage
    Friend WithEvents btnAddThing As System.Windows.Forms.Button
    Friend WithEvents btnRemoveThing As System.Windows.Forms.Button
    Friend WithEvents clbThings As System.Windows.Forms.CheckedListBox
    Friend WithEvents lbSlideList As System.Windows.Forms.ListBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnThingDown As System.Windows.Forms.Button
    Friend WithEvents btnThingUp As System.Windows.Forms.Button
    Friend WithEvents btnClearThings As System.Windows.Forms.Button
    Friend WithEvents btnRemoveSlides As System.Windows.Forms.Button
    Friend WithEvents btnAddSlide As System.Windows.Forms.Button
    Friend WithEvents FileListBox1 As Microsoft.VisualBasic.Compatibility.VB6.FileListBox
    Friend WithEvents btnSaveSlides As System.Windows.Forms.Button
    Friend WithEvents btnLoadSlides As System.Windows.Forms.Button
    Friend WithEvents btnSlideDown As System.Windows.Forms.Button
    Friend WithEvents btnSlideUp As System.Windows.Forms.Button
    Friend WithEvents btnStopSlides As System.Windows.Forms.Button
    Friend WithEvents btnLastSlide As System.Windows.Forms.Button
    Friend WithEvents btnPlaySlides As System.Windows.Forms.Button
    Friend WithEvents btnFirstSlide As System.Windows.Forms.Button
    Friend WithEvents picSlidePreview As System.Windows.Forms.PictureBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents nudDelay As System.Windows.Forms.NumericUpDown
    Friend WithEvents FolderTree1 As HyperCoder.Win.FileSystemControls.FolderTree
    Friend WithEvents menuDummy As System.Windows.Forms.MenuItem
    Friend WithEvents menuAdd1Left As System.Windows.Forms.MenuItem
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents btnClearSlideList As System.Windows.Forms.Button
    Friend WithEvents btnLeftTextBoth As System.Windows.Forms.Button
    Friend WithEvents btnShowLeftText As System.Windows.Forms.Button
    Friend WithEvents btnRightTextBoth As System.Windows.Forms.Button
    Friend WithEvents btnShowRightText As System.Windows.Forms.Button
    Friend WithEvents btnDocLoadBoth As System.Windows.Forms.Button
    Friend WithEvents btnDocLoadRight As System.Windows.Forms.Button
    Friend WithEvents btnDocLoadLeft As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnBlackout = New System.Windows.Forms.Button()
        Me.tbLeftTeam = New System.Windows.Forms.TextBox()
        Me.tbRightTeam = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbLeftScore = New System.Windows.Forms.TextBox()
        Me.tbRightScore = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnScoreLeft = New System.Windows.Forms.Button()
        Me.btnScoreRight = New System.Windows.Forms.Button()
        Me.btnScoreBoth = New System.Windows.Forms.Button()
        Me.MainMenu1 = New System.Windows.Forms.MainMenu()
        Me.menuDummy = New System.Windows.Forms.MenuItem()
        Me.menuAdd1Left = New System.Windows.Forms.MenuItem()
        Me.menuSubtract1Left = New System.Windows.Forms.MenuItem()
        Me.menuAdd5Left = New System.Windows.Forms.MenuItem()
        Me.menuSubtract5Left = New System.Windows.Forms.MenuItem()
        Me.menuAdd1Right = New System.Windows.Forms.MenuItem()
        Me.menuSubtract1Right = New System.Windows.Forms.MenuItem()
        Me.menuAdd5Right = New System.Windows.Forms.MenuItem()
        Me.menuSubtract5Right = New System.Windows.Forms.MenuItem()
        Me.picLeft = New System.Windows.Forms.PictureBox()
        Me.picRight = New System.Windows.Forms.PictureBox()
        Me.btnPictureLeft = New System.Windows.Forms.Button()
        Me.btnPictureRight = New System.Windows.Forms.Button()
        Me.btnPictureBoth = New System.Windows.Forms.Button()
        Me.btnLeftScoreColor = New System.Windows.Forms.Button()
        Me.btnRightScoreColor = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tpScreenText = New System.Windows.Forms.TabPage()
        Me.btnDocLoadBoth = New System.Windows.Forms.Button()
        Me.btnDocLoadRight = New System.Windows.Forms.Button()
        Me.btnDocLoadLeft = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btnLeftTextBoth = New System.Windows.Forms.Button()
        Me.btnShowLeftText = New System.Windows.Forms.Button()
        Me.tbLeftText = New System.Windows.Forms.TextBox()
        Me.tbRightFontSize = New System.Windows.Forms.TextBox()
        Me.grpRightColors = New System.Windows.Forms.GroupBox()
        Me.btnRightTextColor = New System.Windows.Forms.Button()
        Me.pnlRightColor6 = New System.Windows.Forms.Panel()
        Me.pnlRightColor5 = New System.Windows.Forms.Panel()
        Me.pnlRightColor4 = New System.Windows.Forms.Panel()
        Me.pnlRightColor3 = New System.Windows.Forms.Panel()
        Me.pnlRightColor2 = New System.Windows.Forms.Panel()
        Me.pnlRightColor1 = New System.Windows.Forms.Panel()
        Me.btnRightTextBoth = New System.Windows.Forms.Button()
        Me.btnShowRightText = New System.Windows.Forms.Button()
        Me.tbRightText = New System.Windows.Forms.TextBox()
        Me.tbLeftFontSize = New System.Windows.Forms.TextBox()
        Me.grpLeftColors = New System.Windows.Forms.GroupBox()
        Me.btnLeftTextColor = New System.Windows.Forms.Button()
        Me.pnlLeftColor6 = New System.Windows.Forms.Panel()
        Me.pnlLeftColor5 = New System.Windows.Forms.Panel()
        Me.pnlLeftColor4 = New System.Windows.Forms.Panel()
        Me.pnlLeftColor3 = New System.Windows.Forms.Panel()
        Me.pnlLeftColor2 = New System.Windows.Forms.Panel()
        Me.pnlLeftColor1 = New System.Windows.Forms.Panel()
        Me.tp5Things = New System.Windows.Forms.TabPage()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.clbThings = New System.Windows.Forms.CheckedListBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.grpThingsColor = New System.Windows.Forms.GroupBox()
        Me.radioThingColorRight = New System.Windows.Forms.RadioButton()
        Me.radioThingColorLeft = New System.Windows.Forms.RadioButton()
        Me.btnClearThings = New System.Windows.Forms.Button()
        Me.btnRemoveThing = New System.Windows.Forms.Button()
        Me.lblCurrentThing = New System.Windows.Forms.Label()
        Me.btnListRight = New System.Windows.Forms.Button()
        Me.btnListBoth = New System.Windows.Forms.Button()
        Me.btnListLeft = New System.Windows.Forms.Button()
        Me.btnShowThingRight = New System.Windows.Forms.Button()
        Me.btnShowThingBoth = New System.Windows.Forms.Button()
        Me.btnShowThingLeft = New System.Windows.Forms.Button()
        Me.tbSubstitutions = New System.Windows.Forms.TextBox()
        Me.btnAddThing = New System.Windows.Forms.Button()
        Me.tbNewThing = New System.Windows.Forms.TextBox()
        Me.btnThingDown = New System.Windows.Forms.Button()
        Me.btnThingUp = New System.Windows.Forms.Button()
        Me.tpSlides = New System.Windows.Forms.TabPage()
        Me.btnClearSlideList = New System.Windows.Forms.Button()
        Me.FolderTree1 = New HyperCoder.Win.FileSystemControls.FolderTree()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.nudDelay = New System.Windows.Forms.NumericUpDown()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnSaveSlides = New System.Windows.Forms.Button()
        Me.btnLoadSlides = New System.Windows.Forms.Button()
        Me.FileListBox1 = New Microsoft.VisualBasic.Compatibility.VB6.FileListBox()
        Me.btnRemoveSlides = New System.Windows.Forms.Button()
        Me.btnAddSlide = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnSlideDown = New System.Windows.Forms.Button()
        Me.btnSlideUp = New System.Windows.Forms.Button()
        Me.btnStopSlides = New System.Windows.Forms.Button()
        Me.btnLastSlide = New System.Windows.Forms.Button()
        Me.btnPlaySlides = New System.Windows.Forms.Button()
        Me.btnFirstSlide = New System.Windows.Forms.Button()
        Me.lbSlideList = New System.Windows.Forms.ListBox()
        Me.picSlidePreview = New System.Windows.Forms.PictureBox()
        Me.tpAbout = New System.Windows.Forms.TabPage()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.TabControl1.SuspendLayout()
        Me.tpScreenText.SuspendLayout()
        Me.grpRightColors.SuspendLayout()
        Me.grpLeftColors.SuspendLayout()
        Me.tp5Things.SuspendLayout()
        Me.grpThingsColor.SuspendLayout()
        Me.tpSlides.SuspendLayout()
        CType(Me.nudDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpAbout.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnBlackout
        '
        Me.btnBlackout.BackColor = System.Drawing.SystemColors.Control
        Me.btnBlackout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBlackout.ForeColor = System.Drawing.SystemColors.WindowText
        Me.btnBlackout.Location = New System.Drawing.Point(332, 88)
        Me.btnBlackout.Name = "btnBlackout"
        Me.btnBlackout.Size = New System.Drawing.Size(108, 40)
        Me.btnBlackout.TabIndex = 13
        Me.btnBlackout.Text = "BLACKOUT"
        '
        'tbLeftTeam
        '
        Me.tbLeftTeam.BackColor = System.Drawing.SystemColors.Window
        Me.tbLeftTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tbLeftTeam.Location = New System.Drawing.Point(88, 8)
        Me.tbLeftTeam.MaxLength = 18
        Me.tbLeftTeam.Name = "tbLeftTeam"
        Me.tbLeftTeam.Size = New System.Drawing.Size(136, 20)
        Me.tbLeftTeam.TabIndex = 1
        Me.tbLeftTeam.Text = ""
        Me.tbLeftTeam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbRightTeam
        '
        Me.tbRightTeam.BackColor = System.Drawing.SystemColors.Window
        Me.tbRightTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tbRightTeam.Location = New System.Drawing.Point(544, 8)
        Me.tbRightTeam.MaxLength = 18
        Me.tbRightTeam.Name = "tbRightTeam"
        Me.tbRightTeam.Size = New System.Drawing.Size(136, 20)
        Me.tbRightTeam.TabIndex = 8
        Me.tbRightTeam.Text = ""
        Me.tbRightTeam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(4, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Team Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(688, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 24)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Team Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbLeftScore
        '
        Me.tbLeftScore.BackColor = System.Drawing.Color.Blue
        Me.tbLeftScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLeftScore.ForeColor = System.Drawing.Color.White
        Me.tbLeftScore.Location = New System.Drawing.Point(88, 40)
        Me.tbLeftScore.MaxLength = 3
        Me.tbLeftScore.Name = "tbLeftScore"
        Me.tbLeftScore.Size = New System.Drawing.Size(44, 22)
        Me.tbLeftScore.TabIndex = 3
        Me.tbLeftScore.Text = "0"
        Me.tbLeftScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbRightScore
        '
        Me.tbRightScore.BackColor = System.Drawing.Color.FromArgb(CType(176, Byte), CType(0, Byte), CType(0, Byte))
        Me.tbRightScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbRightScore.ForeColor = System.Drawing.Color.White
        Me.tbRightScore.Location = New System.Drawing.Point(636, 40)
        Me.tbRightScore.MaxLength = 3
        Me.tbRightScore.Name = "tbRightScore"
        Me.tbRightScore.Size = New System.Drawing.Size(44, 22)
        Me.tbRightScore.TabIndex = 11
        Me.tbRightScore.Text = "0"
        Me.tbRightScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(28, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 24)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Score"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(688, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 24)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Score"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnScoreLeft
        '
        Me.btnScoreLeft.Location = New System.Drawing.Point(236, 16)
        Me.btnScoreLeft.Name = "btnScoreLeft"
        Me.btnScoreLeft.Size = New System.Drawing.Size(76, 40)
        Me.btnScoreLeft.TabIndex = 5
        Me.btnScoreLeft.Text = "SCORE LEFT"
        '
        'btnScoreRight
        '
        Me.btnScoreRight.Location = New System.Drawing.Point(456, 16)
        Me.btnScoreRight.Name = "btnScoreRight"
        Me.btnScoreRight.Size = New System.Drawing.Size(76, 40)
        Me.btnScoreRight.TabIndex = 7
        Me.btnScoreRight.Text = "SCORE RIGHT"
        '
        'btnScoreBoth
        '
        Me.btnScoreBoth.Location = New System.Drawing.Point(344, 16)
        Me.btnScoreBoth.Name = "btnScoreBoth"
        Me.btnScoreBoth.Size = New System.Drawing.Size(84, 40)
        Me.btnScoreBoth.TabIndex = 6
        Me.btnScoreBoth.Text = "SCORE BOTH"
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDummy})
        '
        'menuDummy
        '
        Me.menuDummy.Index = 0
        Me.menuDummy.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuAdd1Left, Me.menuSubtract1Left, Me.menuAdd5Left, Me.menuSubtract5Left, Me.menuAdd1Right, Me.menuSubtract1Right, Me.menuAdd5Right, Me.menuSubtract5Right})
        Me.menuDummy.Text = "Dummy"
        Me.menuDummy.Visible = False
        '
        'menuAdd1Left
        '
        Me.menuAdd1Left.Index = 0
        Me.menuAdd1Left.Shortcut = System.Windows.Forms.Shortcut.F1
        Me.menuAdd1Left.Text = "+1 Left"
        '
        'menuSubtract1Left
        '
        Me.menuSubtract1Left.Index = 1
        Me.menuSubtract1Left.Shortcut = System.Windows.Forms.Shortcut.F2
        Me.menuSubtract1Left.Text = "-1 Left"
        '
        'menuAdd5Left
        '
        Me.menuAdd5Left.Index = 2
        Me.menuAdd5Left.Shortcut = System.Windows.Forms.Shortcut.F3
        Me.menuAdd5Left.Text = "+5 Left"
        '
        'menuSubtract5Left
        '
        Me.menuSubtract5Left.Index = 3
        Me.menuSubtract5Left.Shortcut = System.Windows.Forms.Shortcut.F4
        Me.menuSubtract5Left.Text = "-5 Left"
        '
        'menuAdd1Right
        '
        Me.menuAdd1Right.Index = 4
        Me.menuAdd1Right.Shortcut = System.Windows.Forms.Shortcut.F5
        Me.menuAdd1Right.Text = "+1 Right"
        '
        'menuSubtract1Right
        '
        Me.menuSubtract1Right.Index = 5
        Me.menuSubtract1Right.Shortcut = System.Windows.Forms.Shortcut.F6
        Me.menuSubtract1Right.Text = "-1 Right"
        '
        'menuAdd5Right
        '
        Me.menuAdd5Right.Index = 6
        Me.menuAdd5Right.Shortcut = System.Windows.Forms.Shortcut.F7
        Me.menuAdd5Right.Text = "+5 Right"
        '
        'menuSubtract5Right
        '
        Me.menuSubtract5Right.Index = 7
        Me.menuSubtract5Right.Shortcut = System.Windows.Forms.Shortcut.F8
        Me.menuSubtract5Right.Text = "-5 Right"
        '
        'picLeft
        '
        Me.picLeft.BackColor = System.Drawing.Color.Transparent
        Me.picLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picLeft.Location = New System.Drawing.Point(28, 80)
        Me.picLeft.Name = "picLeft"
        Me.picLeft.Size = New System.Drawing.Size(160, 120)
        Me.picLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLeft.TabIndex = 28
        Me.picLeft.TabStop = False
        '
        'picRight
        '
        Me.picRight.BackColor = System.Drawing.Color.Transparent
        Me.picRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picRight.Location = New System.Drawing.Point(580, 80)
        Me.picRight.Name = "picRight"
        Me.picRight.Size = New System.Drawing.Size(160, 120)
        Me.picRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picRight.TabIndex = 29
        Me.picRight.TabStop = False
        '
        'btnPictureLeft
        '
        Me.btnPictureLeft.Location = New System.Drawing.Point(216, 156)
        Me.btnPictureLeft.Name = "btnPictureLeft"
        Me.btnPictureLeft.Size = New System.Drawing.Size(84, 40)
        Me.btnPictureLeft.TabIndex = 14
        Me.btnPictureLeft.Text = "PICTURE LEFT"
        '
        'btnPictureRight
        '
        Me.btnPictureRight.Location = New System.Drawing.Point(468, 156)
        Me.btnPictureRight.Name = "btnPictureRight"
        Me.btnPictureRight.Size = New System.Drawing.Size(84, 40)
        Me.btnPictureRight.TabIndex = 16
        Me.btnPictureRight.Text = "PICTURE RIGHT"
        '
        'btnPictureBoth
        '
        Me.btnPictureBoth.Location = New System.Drawing.Point(340, 156)
        Me.btnPictureBoth.Name = "btnPictureBoth"
        Me.btnPictureBoth.Size = New System.Drawing.Size(92, 40)
        Me.btnPictureBoth.TabIndex = 15
        Me.btnPictureBoth.Text = "PICTURE BOTH"
        '
        'btnLeftScoreColor
        '
        Me.btnLeftScoreColor.Location = New System.Drawing.Point(144, 36)
        Me.btnLeftScoreColor.Name = "btnLeftScoreColor"
        Me.btnLeftScoreColor.Size = New System.Drawing.Size(68, 24)
        Me.btnLeftScoreColor.TabIndex = 4
        Me.btnLeftScoreColor.Text = "Color..."
        '
        'btnRightScoreColor
        '
        Me.btnRightScoreColor.Location = New System.Drawing.Point(556, 36)
        Me.btnRightScoreColor.Name = "btnRightScoreColor"
        Me.btnRightScoreColor.Size = New System.Drawing.Size(68, 24)
        Me.btnRightScoreColor.TabIndex = 10
        Me.btnRightScoreColor.Text = "Color..."
        '
        'TabControl1
        '
        Me.TabControl1.Controls.AddRange(New System.Windows.Forms.Control() {Me.tpScreenText, Me.tp5Things, Me.tpSlides, Me.tpAbout})
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.ItemSize = New System.Drawing.Size(192, 21)
        Me.TabControl1.Location = New System.Drawing.Point(0, 216)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(772, 360)
        Me.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.TabControl1.TabIndex = 17
        '
        'tpScreenText
        '
        Me.tpScreenText.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnDocLoadBoth, Me.btnDocLoadRight, Me.btnDocLoadLeft, Me.Label11, Me.Label10, Me.btnLeftTextBoth, Me.btnShowLeftText, Me.tbLeftText, Me.tbRightFontSize, Me.grpRightColors, Me.btnRightTextBoth, Me.btnShowRightText, Me.tbRightText, Me.tbLeftFontSize, Me.grpLeftColors})
        Me.tpScreenText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tpScreenText.Location = New System.Drawing.Point(4, 25)
        Me.tpScreenText.Name = "tpScreenText"
        Me.tpScreenText.Size = New System.Drawing.Size(764, 331)
        Me.tpScreenText.TabIndex = 0
        Me.tpScreenText.Text = "Screen Text"
        '
        'btnDocLoadBoth
        '
        Me.btnDocLoadBoth.Location = New System.Drawing.Point(328, 276)
        Me.btnDocLoadBoth.Name = "btnDocLoadBoth"
        Me.btnDocLoadBoth.Size = New System.Drawing.Size(108, 40)
        Me.btnDocLoadBoth.TabIndex = 43
        Me.btnDocLoadBoth.Tag = "Both"
        Me.btnDocLoadBoth.Text = "DOCUMENT LOAD BOTH"
        '
        'btnDocLoadRight
        '
        Me.btnDocLoadRight.Location = New System.Drawing.Point(520, 300)
        Me.btnDocLoadRight.Name = "btnDocLoadRight"
        Me.btnDocLoadRight.Size = New System.Drawing.Size(156, 24)
        Me.btnDocLoadRight.TabIndex = 46
        Me.btnDocLoadRight.Tag = "Right"
        Me.btnDocLoadRight.Text = "DOCUMENT LOAD RIGHT"
        '
        'btnDocLoadLeft
        '
        Me.btnDocLoadLeft.Location = New System.Drawing.Point(88, 300)
        Me.btnDocLoadLeft.Name = "btnDocLoadLeft"
        Me.btnDocLoadLeft.Size = New System.Drawing.Size(156, 24)
        Me.btnDocLoadLeft.TabIndex = 42
        Me.btnDocLoadLeft.Tag = "Left"
        Me.btnDocLoadLeft.Text = "DOCUMENT LOAD LEFT"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(456, 4)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(52, 20)
        Me.Label11.TabIndex = 28
        Me.Label11.Text = "Font Size"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(20, 4)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(52, 20)
        Me.Label10.TabIndex = 18
        Me.Label10.Text = "Font Size"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'btnLeftTextBoth
        '
        Me.btnLeftTextBoth.Location = New System.Drawing.Point(176, 268)
        Me.btnLeftTextBoth.Name = "btnLeftTextBoth"
        Me.btnLeftTextBoth.Size = New System.Drawing.Size(96, 24)
        Me.btnLeftTextBoth.TabIndex = 41
        Me.btnLeftTextBoth.Text = "SHOW BOTH"
        '
        'btnShowLeftText
        '
        Me.btnShowLeftText.Location = New System.Drawing.Point(60, 268)
        Me.btnShowLeftText.Name = "btnShowLeftText"
        Me.btnShowLeftText.Size = New System.Drawing.Size(96, 24)
        Me.btnShowLeftText.TabIndex = 40
        Me.btnShowLeftText.Text = "SHOW LEFT"
        '
        'tbLeftText
        '
        Me.tbLeftText.BackColor = System.Drawing.Color.Blue
        Me.tbLeftText.Font = New System.Drawing.Font("Arial", 20.0!, System.Drawing.FontStyle.Bold)
        Me.tbLeftText.ForeColor = System.Drawing.Color.White
        Me.tbLeftText.Location = New System.Drawing.Point(24, 52)
        Me.tbLeftText.Multiline = True
        Me.tbLeftText.Name = "tbLeftText"
        Me.tbLeftText.Size = New System.Drawing.Size(280, 210)
        Me.tbLeftText.TabIndex = 38
        Me.tbLeftText.Text = "JANIS" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "left"
        Me.tbLeftText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbRightFontSize
        '
        Me.tbRightFontSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbRightFontSize.Location = New System.Drawing.Point(460, 24)
        Me.tbRightFontSize.MaxLength = 3
        Me.tbRightFontSize.Name = "tbRightFontSize"
        Me.tbRightFontSize.Size = New System.Drawing.Size(44, 21)
        Me.tbRightFontSize.TabIndex = 29
        Me.tbRightFontSize.Text = "60"
        Me.tbRightFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'grpRightColors
        '
        Me.grpRightColors.BackColor = System.Drawing.Color.Transparent
        Me.grpRightColors.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnRightTextColor, Me.pnlRightColor6, Me.pnlRightColor5, Me.pnlRightColor4, Me.pnlRightColor3, Me.pnlRightColor2, Me.pnlRightColor1})
        Me.grpRightColors.Location = New System.Drawing.Point(512, 12)
        Me.grpRightColors.Name = "grpRightColors"
        Me.grpRightColors.Size = New System.Drawing.Size(228, 40)
        Me.grpRightColors.TabIndex = 30
        Me.grpRightColors.TabStop = False
        Me.grpRightColors.Text = "Background"
        '
        'btnRightTextColor
        '
        Me.btnRightTextColor.Location = New System.Drawing.Point(152, 12)
        Me.btnRightTextColor.Name = "btnRightTextColor"
        Me.btnRightTextColor.Size = New System.Drawing.Size(68, 24)
        Me.btnRightTextColor.TabIndex = 37
        Me.btnRightTextColor.Text = "Choose..."
        '
        'pnlRightColor6
        '
        Me.pnlRightColor6.BackColor = System.Drawing.Color.Black
        Me.pnlRightColor6.Location = New System.Drawing.Point(128, 16)
        Me.pnlRightColor6.Name = "pnlRightColor6"
        Me.pnlRightColor6.Size = New System.Drawing.Size(16, 16)
        Me.pnlRightColor6.TabIndex = 36
        Me.pnlRightColor6.TabStop = True
        '
        'pnlRightColor5
        '
        Me.pnlRightColor5.BackColor = System.Drawing.Color.DarkGreen
        Me.pnlRightColor5.Location = New System.Drawing.Point(104, 16)
        Me.pnlRightColor5.Name = "pnlRightColor5"
        Me.pnlRightColor5.Size = New System.Drawing.Size(16, 16)
        Me.pnlRightColor5.TabIndex = 35
        Me.pnlRightColor5.TabStop = True
        '
        'pnlRightColor4
        '
        Me.pnlRightColor4.BackColor = System.Drawing.Color.FromArgb(CType(98, Byte), CType(67, Byte), CType(114, Byte))
        Me.pnlRightColor4.Location = New System.Drawing.Point(80, 16)
        Me.pnlRightColor4.Name = "pnlRightColor4"
        Me.pnlRightColor4.Size = New System.Drawing.Size(16, 16)
        Me.pnlRightColor4.TabIndex = 34
        Me.pnlRightColor4.TabStop = True
        '
        'pnlRightColor3
        '
        Me.pnlRightColor3.BackColor = System.Drawing.Color.SaddleBrown
        Me.pnlRightColor3.Location = New System.Drawing.Point(56, 16)
        Me.pnlRightColor3.Name = "pnlRightColor3"
        Me.pnlRightColor3.Size = New System.Drawing.Size(16, 16)
        Me.pnlRightColor3.TabIndex = 33
        Me.pnlRightColor3.TabStop = True
        '
        'pnlRightColor2
        '
        Me.pnlRightColor2.BackColor = System.Drawing.Color.FromArgb(CType(176, Byte), CType(0, Byte), CType(0, Byte))
        Me.pnlRightColor2.Location = New System.Drawing.Point(32, 16)
        Me.pnlRightColor2.Name = "pnlRightColor2"
        Me.pnlRightColor2.Size = New System.Drawing.Size(16, 16)
        Me.pnlRightColor2.TabIndex = 32
        Me.pnlRightColor2.TabStop = True
        '
        'pnlRightColor1
        '
        Me.pnlRightColor1.BackColor = System.Drawing.Color.Blue
        Me.pnlRightColor1.Location = New System.Drawing.Point(8, 16)
        Me.pnlRightColor1.Name = "pnlRightColor1"
        Me.pnlRightColor1.Size = New System.Drawing.Size(16, 16)
        Me.pnlRightColor1.TabIndex = 31
        Me.pnlRightColor1.TabStop = True
        '
        'btnRightTextBoth
        '
        Me.btnRightTextBoth.Location = New System.Drawing.Point(608, 268)
        Me.btnRightTextBoth.Name = "btnRightTextBoth"
        Me.btnRightTextBoth.Size = New System.Drawing.Size(96, 24)
        Me.btnRightTextBoth.TabIndex = 45
        Me.btnRightTextBoth.Text = "SHOW BOTH"
        '
        'btnShowRightText
        '
        Me.btnShowRightText.Location = New System.Drawing.Point(492, 268)
        Me.btnShowRightText.Name = "btnShowRightText"
        Me.btnShowRightText.Size = New System.Drawing.Size(96, 24)
        Me.btnShowRightText.TabIndex = 44
        Me.btnShowRightText.Text = "SHOW RIGHT"
        '
        'tbRightText
        '
        Me.tbRightText.BackColor = System.Drawing.Color.FromArgb(CType(176, Byte), CType(0, Byte), CType(0, Byte))
        Me.tbRightText.Font = New System.Drawing.Font("Arial", 20.0!, System.Drawing.FontStyle.Bold)
        Me.tbRightText.ForeColor = System.Drawing.Color.White
        Me.tbRightText.Location = New System.Drawing.Point(460, 52)
        Me.tbRightText.Multiline = True
        Me.tbRightText.Name = "tbRightText"
        Me.tbRightText.Size = New System.Drawing.Size(280, 210)
        Me.tbRightText.TabIndex = 39
        Me.tbRightText.Text = "JANIS" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "right"
        Me.tbRightText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbLeftFontSize
        '
        Me.tbLeftFontSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLeftFontSize.Location = New System.Drawing.Point(24, 24)
        Me.tbLeftFontSize.MaxLength = 3
        Me.tbLeftFontSize.Name = "tbLeftFontSize"
        Me.tbLeftFontSize.Size = New System.Drawing.Size(44, 21)
        Me.tbLeftFontSize.TabIndex = 19
        Me.tbLeftFontSize.Text = "60"
        Me.tbLeftFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'grpLeftColors
        '
        Me.grpLeftColors.BackColor = System.Drawing.Color.Transparent
        Me.grpLeftColors.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnLeftTextColor, Me.pnlLeftColor6, Me.pnlLeftColor5, Me.pnlLeftColor4, Me.pnlLeftColor3, Me.pnlLeftColor2, Me.pnlLeftColor1})
        Me.grpLeftColors.Location = New System.Drawing.Point(76, 12)
        Me.grpLeftColors.Name = "grpLeftColors"
        Me.grpLeftColors.Size = New System.Drawing.Size(228, 40)
        Me.grpLeftColors.TabIndex = 20
        Me.grpLeftColors.TabStop = False
        Me.grpLeftColors.Text = "Background"
        '
        'btnLeftTextColor
        '
        Me.btnLeftTextColor.Location = New System.Drawing.Point(152, 12)
        Me.btnLeftTextColor.Name = "btnLeftTextColor"
        Me.btnLeftTextColor.Size = New System.Drawing.Size(68, 24)
        Me.btnLeftTextColor.TabIndex = 27
        Me.btnLeftTextColor.Text = "Choose..."
        '
        'pnlLeftColor6
        '
        Me.pnlLeftColor6.BackColor = System.Drawing.Color.Black
        Me.pnlLeftColor6.Location = New System.Drawing.Point(128, 16)
        Me.pnlLeftColor6.Name = "pnlLeftColor6"
        Me.pnlLeftColor6.Size = New System.Drawing.Size(16, 16)
        Me.pnlLeftColor6.TabIndex = 26
        Me.pnlLeftColor6.TabStop = True
        '
        'pnlLeftColor5
        '
        Me.pnlLeftColor5.BackColor = System.Drawing.Color.DarkGreen
        Me.pnlLeftColor5.Location = New System.Drawing.Point(104, 16)
        Me.pnlLeftColor5.Name = "pnlLeftColor5"
        Me.pnlLeftColor5.Size = New System.Drawing.Size(16, 16)
        Me.pnlLeftColor5.TabIndex = 25
        Me.pnlLeftColor5.TabStop = True
        '
        'pnlLeftColor4
        '
        Me.pnlLeftColor4.BackColor = System.Drawing.Color.FromArgb(CType(98, Byte), CType(67, Byte), CType(114, Byte))
        Me.pnlLeftColor4.Location = New System.Drawing.Point(80, 16)
        Me.pnlLeftColor4.Name = "pnlLeftColor4"
        Me.pnlLeftColor4.Size = New System.Drawing.Size(16, 16)
        Me.pnlLeftColor4.TabIndex = 24
        Me.pnlLeftColor4.TabStop = True
        '
        'pnlLeftColor3
        '
        Me.pnlLeftColor3.BackColor = System.Drawing.Color.FromArgb(CType(128, Byte), CType(64, Byte), CType(0, Byte))
        Me.pnlLeftColor3.Location = New System.Drawing.Point(56, 16)
        Me.pnlLeftColor3.Name = "pnlLeftColor3"
        Me.pnlLeftColor3.Size = New System.Drawing.Size(16, 16)
        Me.pnlLeftColor3.TabIndex = 23
        Me.pnlLeftColor3.TabStop = True
        '
        'pnlLeftColor2
        '
        Me.pnlLeftColor2.BackColor = System.Drawing.Color.FromArgb(CType(176, Byte), CType(0, Byte), CType(0, Byte))
        Me.pnlLeftColor2.Location = New System.Drawing.Point(32, 16)
        Me.pnlLeftColor2.Name = "pnlLeftColor2"
        Me.pnlLeftColor2.Size = New System.Drawing.Size(16, 16)
        Me.pnlLeftColor2.TabIndex = 22
        Me.pnlLeftColor2.TabStop = True
        '
        'pnlLeftColor1
        '
        Me.pnlLeftColor1.BackColor = System.Drawing.Color.Blue
        Me.pnlLeftColor1.Location = New System.Drawing.Point(8, 16)
        Me.pnlLeftColor1.Name = "pnlLeftColor1"
        Me.pnlLeftColor1.Size = New System.Drawing.Size(16, 16)
        Me.pnlLeftColor1.TabIndex = 21
        Me.pnlLeftColor1.TabStop = True
        '
        'tp5Things
        '
        Me.tp5Things.Controls.AddRange(New System.Windows.Forms.Control() {Me.Label12, Me.clbThings, Me.Label5, Me.grpThingsColor, Me.btnClearThings, Me.btnRemoveThing, Me.lblCurrentThing, Me.btnListRight, Me.btnListBoth, Me.btnListLeft, Me.btnShowThingRight, Me.btnShowThingBoth, Me.btnShowThingLeft, Me.tbSubstitutions, Me.btnAddThing, Me.tbNewThing, Me.btnThingDown, Me.btnThingUp})
        Me.tp5Things.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tp5Things.Location = New System.Drawing.Point(4, 25)
        Me.tp5Things.Name = "tp5Things"
        Me.tp5Things.Size = New System.Drawing.Size(764, 331)
        Me.tp5Things.TabIndex = 1
        Me.tp5Things.Text = "5/6 Things"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(68, 40)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(212, 16)
        Me.Label12.TabIndex = 47
        Me.Label12.Text = "Name of Thing to Add"
        '
        'clbThings
        '
        Me.clbThings.BackColor = System.Drawing.Color.Blue
        Me.clbThings.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clbThings.ForeColor = System.Drawing.Color.White
        Me.clbThings.Location = New System.Drawing.Point(68, 96)
        Me.clbThings.Name = "clbThings"
        Me.clbThings.Size = New System.Drawing.Size(212, 148)
        Me.clbThings.TabIndex = 50
        Me.clbThings.ThreeDCheckBoxes = True
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 160)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 20)
        Me.Label5.TabIndex = 52
        Me.Label5.Text = "ORDER"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpThingsColor
        '
        Me.grpThingsColor.BackColor = System.Drawing.Color.Transparent
        Me.grpThingsColor.Controls.AddRange(New System.Windows.Forms.Control() {Me.radioThingColorRight, Me.radioThingColorLeft})
        Me.grpThingsColor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpThingsColor.Location = New System.Drawing.Point(408, 36)
        Me.grpThingsColor.Name = "grpThingsColor"
        Me.grpThingsColor.Size = New System.Drawing.Size(332, 48)
        Me.grpThingsColor.TabIndex = 59
        Me.grpThingsColor.TabStop = False
        Me.grpThingsColor.Text = "Select Currently Playing Team"
        '
        'radioThingColorRight
        '
        Me.radioThingColorRight.BackColor = System.Drawing.Color.FromArgb(CType(176, Byte), CType(0, Byte), CType(0, Byte))
        Me.radioThingColorRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radioThingColorRight.ForeColor = System.Drawing.Color.White
        Me.radioThingColorRight.Location = New System.Drawing.Point(172, 16)
        Me.radioThingColorRight.Name = "radioThingColorRight"
        Me.radioThingColorRight.Size = New System.Drawing.Size(156, 28)
        Me.radioThingColorRight.TabIndex = 61
        Me.radioThingColorRight.Text = "Right"
        Me.radioThingColorRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'radioThingColorLeft
        '
        Me.radioThingColorLeft.BackColor = System.Drawing.Color.Blue
        Me.radioThingColorLeft.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.radioThingColorLeft.Checked = True
        Me.radioThingColorLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radioThingColorLeft.ForeColor = System.Drawing.Color.White
        Me.radioThingColorLeft.Location = New System.Drawing.Point(4, 16)
        Me.radioThingColorLeft.Name = "radioThingColorLeft"
        Me.radioThingColorLeft.Size = New System.Drawing.Size(156, 28)
        Me.radioThingColorLeft.TabIndex = 60
        Me.radioThingColorLeft.TabStop = True
        Me.radioThingColorLeft.Text = "Left"
        Me.radioThingColorLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnClearThings
        '
        Me.btnClearThings.Location = New System.Drawing.Point(296, 192)
        Me.btnClearThings.Name = "btnClearThings"
        Me.btnClearThings.Size = New System.Drawing.Size(80, 32)
        Me.btnClearThings.TabIndex = 55
        Me.btnClearThings.Text = "Clear List"
        '
        'btnRemoveThing
        '
        Me.btnRemoveThing.Location = New System.Drawing.Point(296, 120)
        Me.btnRemoveThing.Name = "btnRemoveThing"
        Me.btnRemoveThing.Size = New System.Drawing.Size(80, 40)
        Me.btnRemoveThing.TabIndex = 54
        Me.btnRemoveThing.Text = "Remove Selected"
        '
        'lblCurrentThing
        '
        Me.lblCurrentThing.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCurrentThing.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentThing.Location = New System.Drawing.Point(464, 108)
        Me.lblCurrentThing.Name = "lblCurrentThing"
        Me.lblCurrentThing.Size = New System.Drawing.Size(232, 24)
        Me.lblCurrentThing.TabIndex = 62
        Me.lblCurrentThing.Text = "Substitutions:"
        Me.lblCurrentThing.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'btnListRight
        '
        Me.btnListRight.Location = New System.Drawing.Point(232, 268)
        Me.btnListRight.Name = "btnListRight"
        Me.btnListRight.Size = New System.Drawing.Size(88, 32)
        Me.btnListRight.TabIndex = 58
        Me.btnListRight.Text = "List Right"
        '
        'btnListBoth
        '
        Me.btnListBoth.Location = New System.Drawing.Point(120, 268)
        Me.btnListBoth.Name = "btnListBoth"
        Me.btnListBoth.Size = New System.Drawing.Size(88, 32)
        Me.btnListBoth.TabIndex = 57
        Me.btnListBoth.Text = "List Both"
        '
        'btnListLeft
        '
        Me.btnListLeft.Location = New System.Drawing.Point(8, 268)
        Me.btnListLeft.Name = "btnListLeft"
        Me.btnListLeft.Size = New System.Drawing.Size(88, 32)
        Me.btnListLeft.TabIndex = 56
        Me.btnListLeft.Text = "List Left"
        '
        'btnShowThingRight
        '
        Me.btnShowThingRight.Location = New System.Drawing.Point(648, 268)
        Me.btnShowThingRight.Name = "btnShowThingRight"
        Me.btnShowThingRight.Size = New System.Drawing.Size(88, 32)
        Me.btnShowThingRight.TabIndex = 66
        Me.btnShowThingRight.Text = "Show Right"
        '
        'btnShowThingBoth
        '
        Me.btnShowThingBoth.Location = New System.Drawing.Point(536, 268)
        Me.btnShowThingBoth.Name = "btnShowThingBoth"
        Me.btnShowThingBoth.Size = New System.Drawing.Size(88, 32)
        Me.btnShowThingBoth.TabIndex = 65
        Me.btnShowThingBoth.Text = "Show Both"
        '
        'btnShowThingLeft
        '
        Me.btnShowThingLeft.Location = New System.Drawing.Point(424, 268)
        Me.btnShowThingLeft.Name = "btnShowThingLeft"
        Me.btnShowThingLeft.Size = New System.Drawing.Size(88, 32)
        Me.btnShowThingLeft.TabIndex = 64
        Me.btnShowThingLeft.Text = "Show Left"
        '
        'tbSubstitutions
        '
        Me.tbSubstitutions.BackColor = System.Drawing.Color.Blue
        Me.tbSubstitutions.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbSubstitutions.ForeColor = System.Drawing.Color.White
        Me.tbSubstitutions.Location = New System.Drawing.Point(464, 132)
        Me.tbSubstitutions.Multiline = True
        Me.tbSubstitutions.Name = "tbSubstitutions"
        Me.tbSubstitutions.Size = New System.Drawing.Size(232, 112)
        Me.tbSubstitutions.TabIndex = 63
        Me.tbSubstitutions.Text = ""
        Me.tbSubstitutions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnAddThing
        '
        Me.btnAddThing.Location = New System.Drawing.Point(296, 48)
        Me.btnAddThing.Name = "btnAddThing"
        Me.btnAddThing.Size = New System.Drawing.Size(76, 32)
        Me.btnAddThing.TabIndex = 49
        Me.btnAddThing.Text = "Add Thing"
        '
        'tbNewThing
        '
        Me.tbNewThing.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbNewThing.Location = New System.Drawing.Point(68, 56)
        Me.tbNewThing.Name = "tbNewThing"
        Me.tbNewThing.Size = New System.Drawing.Size(212, 23)
        Me.tbNewThing.TabIndex = 48
        Me.tbNewThing.Text = ""
        '
        'btnThingDown
        '
        Me.btnThingDown.Font = New System.Drawing.Font("Webdings", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnThingDown.Location = New System.Drawing.Point(16, 192)
        Me.btnThingDown.Name = "btnThingDown"
        Me.btnThingDown.Size = New System.Drawing.Size(40, 32)
        Me.btnThingDown.TabIndex = 53
        Me.btnThingDown.Text = "6"
        '
        'btnThingUp
        '
        Me.btnThingUp.Font = New System.Drawing.Font("Webdings", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnThingUp.Location = New System.Drawing.Point(16, 116)
        Me.btnThingUp.Name = "btnThingUp"
        Me.btnThingUp.Size = New System.Drawing.Size(40, 32)
        Me.btnThingUp.TabIndex = 51
        Me.btnThingUp.Text = "5"
        '
        'tpSlides
        '
        Me.tpSlides.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnClearSlideList, Me.FolderTree1, Me.Label9, Me.nudDelay, Me.Label8, Me.Label7, Me.btnSaveSlides, Me.btnLoadSlides, Me.FileListBox1, Me.btnRemoveSlides, Me.btnAddSlide, Me.Label6, Me.btnSlideDown, Me.btnSlideUp, Me.btnStopSlides, Me.btnLastSlide, Me.btnPlaySlides, Me.btnFirstSlide, Me.lbSlideList, Me.picSlidePreview})
        Me.tpSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tpSlides.Location = New System.Drawing.Point(4, 25)
        Me.tpSlides.Name = "tpSlides"
        Me.tpSlides.Size = New System.Drawing.Size(764, 331)
        Me.tpSlides.TabIndex = 2
        Me.tpSlides.Text = "Slide Show"
        '
        'btnClearSlideList
        '
        Me.btnClearSlideList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearSlideList.Location = New System.Drawing.Point(416, 204)
        Me.btnClearSlideList.Name = "btnClearSlideList"
        Me.btnClearSlideList.Size = New System.Drawing.Size(64, 28)
        Me.btnClearSlideList.TabIndex = 77
        Me.btnClearSlideList.Text = "Clear List"
        '
        'FolderTree1
        '
        Me.FolderTree1.Cursor = System.Windows.Forms.Cursors.Default
        Me.FolderTree1.FullRowSelect = True
        Me.FolderTree1.HideSelection = False
        Me.FolderTree1.IconSize = HyperCoder.Win.FileSystemControls.FolderTree.IconSize2Display.Small
        Me.FolderTree1.ImageIndex = -1
        Me.FolderTree1.IncludeFiles = False
        Me.FolderTree1.Location = New System.Drawing.Point(4, 4)
        Me.FolderTree1.Name = "FolderTree1"
        Me.FolderTree1.RootFolder = "Desktop"
        Me.FolderTree1.SelectedImageIndex = -1
        Me.FolderTree1.ShowHiddenItems = False
        Me.FolderTree1.ShowRootLines = False
        Me.FolderTree1.ShowSystemItems = False
        Me.FolderTree1.Size = New System.Drawing.Size(192, 320)
        Me.FolderTree1.TabIndex = 67
        Me.FolderTree1.Text = "FolderTree"
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(504, 300)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(56, 24)
        Me.Label9.TabIndex = 80
        Me.Label9.Text = "Seconds:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudDelay
        '
        Me.nudDelay.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudDelay.Location = New System.Drawing.Point(564, 300)
        Me.nudDelay.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
        Me.nudDelay.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudDelay.Name = "nudDelay"
        Me.nudDelay.Size = New System.Drawing.Size(56, 26)
        Me.nudDelay.TabIndex = 81
        Me.nudDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.nudDelay.Value = New Decimal(New Integer() {8, 0, 0, 0})
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(532, 4)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(224, 16)
        Me.Label8.TabIndex = 71
        Me.Label8.Text = "Slide List"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Location = New System.Drawing.Point(200, 4)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 16)
        Me.Label7.TabIndex = 67
        Me.Label7.Text = "Preview"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnSaveSlides
        '
        Me.btnSaveSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveSlides.Location = New System.Drawing.Point(400, 296)
        Me.btnSaveSlides.Name = "btnSaveSlides"
        Me.btnSaveSlides.Size = New System.Drawing.Size(96, 28)
        Me.btnSaveSlides.TabIndex = 79
        Me.btnSaveSlides.Text = "Save Slideshow"
        '
        'btnLoadSlides
        '
        Me.btnLoadSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadSlides.Location = New System.Drawing.Point(400, 260)
        Me.btnLoadSlides.Name = "btnLoadSlides"
        Me.btnLoadSlides.Size = New System.Drawing.Size(96, 28)
        Me.btnLoadSlides.TabIndex = 78
        Me.btnLoadSlides.Text = "Load Slideshow"
        '
        'FileListBox1
        '
        Me.FileListBox1.Location = New System.Drawing.Point(200, 112)
        Me.FileListBox1.Name = "FileListBox1"
        Me.FileListBox1.Pattern = "*.jpg;*.gif;*.bmp;*.wmf"
        Me.FileListBox1.Size = New System.Drawing.Size(184, 212)
        Me.FileListBox1.TabIndex = 69
        '
        'btnRemoveSlides
        '
        Me.btnRemoveSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveSlides.Location = New System.Drawing.Point(412, 148)
        Me.btnRemoveSlides.Name = "btnRemoveSlides"
        Me.btnRemoveSlides.Size = New System.Drawing.Size(72, 36)
        Me.btnRemoveSlides.TabIndex = 76
        Me.btnRemoveSlides.Text = "Remove Selected"
        '
        'btnAddSlide
        '
        Me.btnAddSlide.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddSlide.Location = New System.Drawing.Point(324, 36)
        Me.btnAddSlide.Name = "btnAddSlide"
        Me.btnAddSlide.Size = New System.Drawing.Size(60, 48)
        Me.btnAddSlide.TabIndex = 70
        Me.btnAddSlide.Text = "Add To List"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(420, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 15)
        Me.Label6.TabIndex = 74
        Me.Label6.Text = "ORDER"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnSlideDown
        '
        Me.btnSlideDown.Font = New System.Drawing.Font("Webdings", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSlideDown.Location = New System.Drawing.Point(432, 80)
        Me.btnSlideDown.Name = "btnSlideDown"
        Me.btnSlideDown.Size = New System.Drawing.Size(32, 28)
        Me.btnSlideDown.TabIndex = 75
        Me.btnSlideDown.Text = "6"
        '
        'btnSlideUp
        '
        Me.btnSlideUp.Font = New System.Drawing.Font("Webdings", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSlideUp.Location = New System.Drawing.Point(432, 28)
        Me.btnSlideUp.Name = "btnSlideUp"
        Me.btnSlideUp.Size = New System.Drawing.Size(32, 28)
        Me.btnSlideUp.TabIndex = 73
        Me.btnSlideUp.Text = "5"
        '
        'btnStopSlides
        '
        Me.btnStopSlides.Font = New System.Drawing.Font("Webdings", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStopSlides.ForeColor = System.Drawing.Color.Red
        Me.btnStopSlides.Location = New System.Drawing.Point(696, 300)
        Me.btnStopSlides.Name = "btnStopSlides"
        Me.btnStopSlides.Size = New System.Drawing.Size(28, 28)
        Me.btnStopSlides.TabIndex = 84
        Me.btnStopSlides.Text = "<"
        '
        'btnLastSlide
        '
        Me.btnLastSlide.Font = New System.Drawing.Font("Webdings", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLastSlide.Location = New System.Drawing.Point(732, 300)
        Me.btnLastSlide.Name = "btnLastSlide"
        Me.btnLastSlide.Size = New System.Drawing.Size(28, 28)
        Me.btnLastSlide.TabIndex = 85
        Me.btnLastSlide.Text = ":"
        '
        'btnPlaySlides
        '
        Me.btnPlaySlides.Font = New System.Drawing.Font("Webdings", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPlaySlides.ForeColor = System.Drawing.Color.Green
        Me.btnPlaySlides.Location = New System.Drawing.Point(660, 300)
        Me.btnPlaySlides.Name = "btnPlaySlides"
        Me.btnPlaySlides.Size = New System.Drawing.Size(28, 28)
        Me.btnPlaySlides.TabIndex = 83
        Me.btnPlaySlides.Text = "4"
        '
        'btnFirstSlide
        '
        Me.btnFirstSlide.Font = New System.Drawing.Font("Webdings", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFirstSlide.Location = New System.Drawing.Point(624, 300)
        Me.btnFirstSlide.Name = "btnFirstSlide"
        Me.btnFirstSlide.Size = New System.Drawing.Size(28, 28)
        Me.btnFirstSlide.TabIndex = 82
        Me.btnFirstSlide.Text = "9"
        '
        'lbSlideList
        '
        Me.lbSlideList.HorizontalScrollbar = True
        Me.lbSlideList.Location = New System.Drawing.Point(508, 20)
        Me.lbSlideList.Name = "lbSlideList"
        Me.lbSlideList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lbSlideList.Size = New System.Drawing.Size(252, 277)
        Me.lbSlideList.TabIndex = 72
        '
        'picSlidePreview
        '
        Me.picSlidePreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picSlidePreview.Location = New System.Drawing.Point(200, 20)
        Me.picSlidePreview.Name = "picSlidePreview"
        Me.picSlidePreview.Size = New System.Drawing.Size(120, 90)
        Me.picSlidePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picSlidePreview.TabIndex = 4
        Me.picSlidePreview.TabStop = False
        '
        'tpAbout
        '
        Me.tpAbout.BackColor = System.Drawing.SystemColors.Window
        Me.tpAbout.Controls.AddRange(New System.Windows.Forms.Control() {Me.TextBox2, Me.TextBox1})
        Me.tpAbout.Location = New System.Drawing.Point(4, 25)
        Me.tpAbout.Name = "tpAbout"
        Me.tpAbout.Size = New System.Drawing.Size(764, 331)
        Me.tpAbout.TabIndex = 3
        Me.tpAbout.Text = "About JANIS"
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Font = New System.Drawing.Font("Comic Sans MS", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.TextBox2.Location = New System.Drawing.Point(8, 120)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(752, 208)
        Me.TextBox2.TabIndex = 87
        Me.TextBox2.Text = "I humbly present this program as a gift to ComedySportz Portland, as" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "a token of " & _
        "my thanks for giving me so much enjoyment and fulfillment." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "This program may b" & _
        "e used with explicit permission of the author, Bill Cernansky." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Special thanks" & _
        " to:" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Jay and MaryAnn Rambo, the ultimate bug spotters" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Jamie Montgomery, who he" & _
        "lped me conceive most of the new ideas presented herein." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Andrew Berkowitz, for " & _
        "helping me with the rest of the new ideas" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Patrick Short, who trusts me with his" & _
        " prized computer equipment" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Portland Brewery, for MacTarnahan's Scottish Ale, wh" & _
        "ich refreshed me during intense bug fighting."
        Me.TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Comic Sans MS", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.TextBox1.Location = New System.Drawing.Point(8, 8)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(752, 104)
        Me.TextBox1.TabIndex = 86
        Me.TextBox1.Text = "ComedySportz JANIS" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "(Just Another Nice Improv Scorekeeper)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "version 1.04   Releas" & _
        "ed August 31, 2003" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "by Bill Cernansky"
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Timer1
        '
        Me.Timer1.Interval = 8000
        '
        'fmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(772, 553)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnBlackout, Me.TabControl1, Me.btnRightScoreColor, Me.btnLeftScoreColor, Me.btnPictureBoth, Me.btnPictureRight, Me.btnPictureLeft, Me.picRight, Me.picLeft, Me.btnScoreBoth, Me.btnScoreRight, Me.btnScoreLeft, Me.Label4, Me.Label3, Me.tbRightScore, Me.tbLeftScore, Me.Label2, Me.Label1, Me.tbRightTeam, Me.tbLeftTeam})
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Location = New System.Drawing.Point(20, 0)
        Me.Menu = Me.MainMenu1
        Me.Name = "fmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "CSz JANIS"
        Me.TabControl1.ResumeLayout(False)
        Me.tpScreenText.ResumeLayout(False)
        Me.grpRightColors.ResumeLayout(False)
        Me.grpLeftColors.ResumeLayout(False)
        Me.tp5Things.ResumeLayout(False)
        Me.grpThingsColor.ResumeLayout(False)
        Me.tpSlides.ResumeLayout(False)
        CType(Me.nudDelay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpAbout.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LeftDefaultColor = Me.tbLeftText.BackColor
        RightDefaultColor = Me.tbRightText.BackColor
        radioThingColorLeft.BackColor = LeftDefaultColor
        radioThingColorRight.BackColor = RightDefaultColor
        Me.lblCurrentThing.Visible = False
        Me.tbSubstitutions.Visible = False
        Me.btnShowThingLeft.Visible = False
        Me.btnShowThingRight.Visible = False
        Me.btnShowThingBoth.Visible = False

        Me.LS = New fmScreen()
        Me.RS = New fmScreen()

        '* Here's the wacky way you change font sizes in VB.NET. Piece of crap.
        Me.tbLeftText.Font = New Font(Me.tbLeftText.Font.Name, CSng(Val(Me.tbLeftFontSize.Text) / FontMult), Me.tbLeftText.Font.Style)
        Me.tbRightText.Font = New Font(Me.tbRightText.Font.Name, CSng(Val(Me.tbRightFontSize.Text) / FontMult), Me.tbRightText.Font.Style)

        DisplayTextScreen(Me.LS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, Me.tbLeftText.Font.Size)
        DisplayTextScreen(Me.RS, Me.tbRightText.Text, Me.tbRightText.BackColor, Me.tbRightText.Font.Size)

        Me.LS.Left = 800  'Don't really need to set this, but I'm doing it anyway
        Me.RS.Left = 1600 'Because we copied another object that uses 800
        Me.LS.Show()
        Me.RS.Show()

        '* The FolderTree control can't be selected programmatically
        '* (what the hell was the guy thinking), but we can expand it down to the
        '* directory we want to see at the start. However, since we can't select it,
        '* the FileListBox's path will be empty.
        Me.FTreeAutoExpand_C("Program Files\Multiple_Display")
        Me.FileListBox1.Path = ""
    End Sub

    Private Sub btnScoreLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScoreLeft.Click
        Me.picLeft.Image = Nothing
        DisplayScore(Me.LS, Me.tbLeftTeam.Text, Me.tbLeftScore.Text, tbLeftScore.BackColor)
    End Sub
    Private Sub btnScoreRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScoreRight.Click
        Me.picRight.Image = Nothing
        DisplayScore(Me.RS, Me.tbRightTeam.Text, Me.tbRightScore.Text, Me.tbRightScore.BackColor)
    End Sub
    Private Sub btnScoreBoth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScoreBoth.Click
        DisplayBothScores()  ' Clears the local pic images itself.
    End Sub
    Private Sub btnLeftScoreColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeftScoreColor.Click
        tbLeftScore.BackColor = PickColor(sender.Left, sender.Top, tbLeftScore.BackColor)
        radioThingColorLeft.BackColor = tbLeftScore.BackColor
        If radioThingColorLeft.Checked Then
            clbThings.BackColor = radioThingColorLeft.BackColor
            tbSubstitutions.BackColor = radioThingColorLeft.BackColor
        End If
    End Sub
    Private Sub btnRightScoreColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRightScoreColor.Click
        tbRightScore.BackColor = PickColor(sender.Left - 100, sender.Top, tbRightScore.BackColor)
        radioThingColorRight.BackColor = tbRightScore.BackColor
        If radioThingColorRight.Checked Then
            clbThings.BackColor = tbRightScore.BackColor
            tbSubstitutions.BackColor = tbRightScore.BackColor
        End If
    End Sub

    Private Sub btnShowLeftText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowLeftText.Click
        DisplayTextScreen(Me.LS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, Me.tbLeftText.Font.Size)
    End Sub
    Private Sub btnShowRightText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowRightText.Click
        DisplayTextScreen(Me.RS, Me.tbRightText.Text, Me.tbRightText.BackColor, Me.tbRightText.Font.Size)
    End Sub
    Private Sub btnLeftTextBoth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeftTextBoth.Click
        DisplayTextScreen(Me.LS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, Me.tbLeftText.Font.Size)
        DisplayTextScreen(Me.RS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, Me.tbLeftText.Font.Size)
    End Sub
    Private Sub btnRightTextBoth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRightTextBoth.Click
        DisplayTextScreen(Me.LS, Me.tbRightText.Text, Me.tbRightText.BackColor, Me.tbRightText.Font.Size)
        DisplayTextScreen(Me.RS, Me.tbRightText.Text, Me.tbRightText.BackColor, Me.tbRightText.Font.Size)
    End Sub

    Private Sub btnDocLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDocLoadLeft.Click, btnDocLoadRight.Click, btnDocLoadBoth.Click
        Dim Doc As String = LoadDoc()
        If sender.Tag <> "Left" Then  '* This will work for Right and Both
            Me.tbRightText.Text = Doc
        End If
        If sender.Tag <> "Right" Then '* This will work for Left and Both
            Me.tbLeftText.Text = Doc
        End If
        AllScreensToFront()
    End Sub

    Private Function LoadDoc() As String
        Dim Doc As String = ""
        Dim FileErr As Boolean = False
        Dim of As New OpenFileDialog()
        of.Filter = "Text File (*.TXT)|*.TXT"
        of.InitialDirectory = ROOT_SUPPORT_DIR
        If of.ShowDialog(Me) = DialogResult.OK Then
            Dim fn As Integer = FreeFile()
            Try
                FileOpen(fn, of.FileName, OpenMode.Input)
            Catch e As Exception
                FileErr = True
                MessageBox.Show("An error occurred opening file '" + of.FileName + "'.", "File Error")
            End Try
            If Not FileErr Then
                Doc = InputString(fn, LOF(fn))
                FileClose(fn)
            End If
        End If
        of.Dispose()
        Return Doc
    End Function

    Private Sub pnlLeftColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlLeftColor1.Click, pnlLeftColor2.Click, pnlLeftColor3.Click, pnlLeftColor4.Click, pnlLeftColor5.Click, pnlLeftColor6.Click
        Me.tbLeftText.BackColor = sender.BackColor
    End Sub
    Private Sub pnlRightColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlRightColor1.Click, pnlRightColor2.Click, pnlRightColor3.Click, pnlRightColor4.Click, pnlRightColor5.Click, pnlRightColor6.Click
        Me.tbRightText.BackColor = sender.BackColor
    End Sub
    Private Sub btnLeftTextColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeftTextColor.Click
        tbLeftText.BackColor = PickColor(50, 300, tbLeftText.BackColor)
    End Sub
    Private Sub btnRightTextColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRightTextColor.Click
        tbRightText.BackColor = PickColor(450, 300, tbRightText.BackColor)
        'tbRightText.BackColor = ChooseColor(tbRightText.BackColor)
    End Sub

    Private Sub btnBlackout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlackout.Click
        '* First, stop the slideshow if it's running.
        StopSlideShow()

        '* Shut them down, Artoo! Shut them all down!
        Me.LS.BackColor = System.Drawing.Color.Black
        Me.RS.BackColor = System.Drawing.Color.Black
        Me.LS.picGraphic.Visible = False
        Me.RS.picGraphic.Visible = False
        Me.picLeft.Image = Nothing
        Me.picRight.Image = Nothing
        Me.LS.lblMsg.Visible = False
        Me.RS.lblMsg.Visible = False
        Me.LS.lblScore.Visible = False
        Me.RS.lblScore.Visible = False
        Me.LS.lblTeamName.Visible = False
        Me.RS.lblTeamName.Visible = False

    End Sub

    Private Sub tbFontSize_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbLeftFontSize.KeyUp, tbRightFontSize.KeyUp
        Dim fsiz As Single
        'tbLeftText.Text = "KeyCode: " + CStr(e.KeyCode) + Chr(13) + "KeyData: " + CStr(e.KeyData) + Chr(13) + "KeyValue: " + CStr(e.KeyValue)

        '*** FIRST, zero sizes or smaller = unhandled exception. Don't allow.
        If (sender.Text = "") Then
            fsiz = 1
        Else
            fsiz = Val(sender.Text)
        End If
        If fsiz < 1 Then fsiz = 1
        If sender.Name = "tbLeftFontSize" Then
            Me.tbLeftText.Font = New Font(Me.tbLeftText.Font.Name, (fsiz / FontMult), Me.tbLeftText.Font.Style)
        Else  ' must be tbRightFontSize
            Me.tbRightText.Font = New Font(Me.tbRightText.Font.Name, (fsiz / FontMult), Me.tbRightText.Font.Style)
        End If
    End Sub
    Private Sub tbFontSize_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbLeftFontSize.KeyPress, tbRightFontSize.KeyPress
        ' Chr(8) is BackSpace
        If (e.KeyChar <> Chr(8)) And ((e.KeyChar < "0") Or (e.KeyChar > "9")) Then
            'tbLeftText.Text = CStr(Asc(e.KeyChar))
            e.Handled = True
        End If
    End Sub
    Private Sub tbScore_KeyPress(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles tbLeftScore.KeyPress, tbRightScore.KeyPress
        ' Chr(8) is BackSpace
        If (e.KeyChar <> Chr(8)) And (e.KeyChar <> "-") And ((e.KeyChar < "0") Or (e.KeyChar > "9")) Then
            'tbLeftText.Text = CStr(Asc(e.KeyChar))
            e.Handled = True
        End If
    End Sub

    Private Sub DisplayTextScreen(ByVal Scr As fmScreen, ByVal s As String, ByVal hue As Color, ByVal fontsize As Single)
        '* Translate ampersands into double ampersands to make them appear.
        Dim i As Integer
        Dim translation As String
        For i = 1 To Len(s)
            Dim onechar As String
            onechar = Mid(s, i, 1)
            '* double it up if it's an ampersand (&)
            translation = translation + onechar
            If onechar = "&" Then translation = translation + onechar
        Next

        '* Stop the slideshow if it's running.
        StopSlideShow()

        '* Blank out the corresponding graphics preview
        If Scr Is Me.LS Then
            Me.picLeft.Image = Nothing
        ElseIf Scr Is Me.RS Then
            Me.picRight.Image = Nothing
        End If

        Scr.lblScore.Visible = False
        Scr.lblTeamName.Visible = False
        Scr.picGraphic.Visible = False
        Scr.lblMsg.Font = New Font(Scr.lblMsg.Font.Name, CSng(FontMult * fontsize), Scr.lblMsg.Font.Style)
        Scr.lblMsg.Visible = True
        Scr.lblMsg.Text = translation
        Scr.BackColor = hue
    End Sub

    Private Sub DisplayScore(ByVal Scr As fmScreen, ByVal Team As String, ByVal Score As String, ByVal BackColor As System.Drawing.Color)
        '* First, stop the slideshow if it's running.
        StopSlideShow()

        Scr.BackColor = BackColor
        Scr.lblMsg.Visible = False
        Scr.picGraphic.Visible = False
        Scr.lblScore.Text = Score
        Scr.lblTeamName.Text = Team
        Scr.lblScore.Visible = True
        Scr.lblTeamName.Visible = True
    End Sub

    Private Sub DisplayImage(ByVal Scr As fmScreen, ByVal Img As Image)
        Scr.lblScore.Visible = False
        Scr.lblTeamName.Visible = False
        Scr.lblMsg.Visible = False
        Scr.picGraphic.Image = Img
        Scr.picGraphic.Visible = True
    End Sub

    Private Sub DisplayBothImages(ByVal fnam As String)
        Dim img As Image
        Try
            img = Image.FromFile(fnam)
        Catch ex As Exception
            img = Nothing
        End Try
        Me.picLeft.Image = img
        Me.picRight.Image = img
        DisplayImage(Me.LS, img)
        DisplayImage(Me.RS, img)
    End Sub

    Private Sub DisplayBothScores()
        Me.picLeft.Image = Nothing
        DisplayScore(Me.LS, Me.tbLeftTeam.Text, Me.tbLeftScore.Text, Me.tbLeftScore.BackColor)
        Me.picRight.Image = Nothing
        DisplayScore(Me.RS, Me.tbRightTeam.Text, Me.tbRightScore.Text, Me.tbRightScore.BackColor)
    End Sub

    Private Sub AddScore(ByVal Side As String, ByVal Points As Integer)
        Dim score As Integer
        Dim tbSource As TextBox
        If Side = "Left" Then
            tbSource = tbLeftScore
        Else
            tbSource = tbRightScore
        End If
        score = Val(tbSource.Text) + Points
        If score > 999 Then score = 999
        If score < -99 Then score = -99
        tbSource.Text = CStr(score)
        DisplayBothScores()
    End Sub

    Private Sub menuChangeScore(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuAdd1Left.Click, menuSubtract1Left.Click, menuAdd5Left.Click, menuSubtract5Left.Click, menuAdd1Right.Click, menuSubtract1Right.Click, menuAdd5Right.Click, menuSubtract5Right.Click
        'This is the function that does function-key mod of the score.
        'The function keys are bound to this (invisible) main menu object group
        Dim MenuInfo() As String = Split(sender.Text)
        Dim Points As Integer = Val(MenuInfo(0))
        AddScore(MenuInfo(1), Points)
    End Sub

    Private Function PickColor(ByVal X As Integer, ByVal Y As Integer, ByVal hue As Color) As Color
        Dim choose As New fmColorDialog()
        choose.pnlSelected.BackColor = hue
        choose.Location = New Point(X, Y)
        If choose.ShowDialog(Me) = DialogResult.OK Then
            hue = choose.pnlSelected.BackColor
        End If
        choose.Dispose()
        Return hue
    End Function

    '    Public Function ChooseColor(ByVal CurrentColor) As Color
    '        Dim Result As Color
    '
    '        Dim colorBox As ColorDialog = New ColorDialog()
    '        ' Sets the initial color select to the current text color,
    '        ' so that if the user cancels out, the original color is restored.
    '        colorBox.Color = CurrentColor
    '        colorBox.ShowDialog()
    '        Dim s As String = colorBox.Color.ToString()
    '
    '        'covert this string to another string that ColorConverter knows
    '        s = s.Split(New Char() {"["c, "]"c})(1)
    '        Dim sa() As String = s.Split(New Char() {"="c, ","c})
    '        If sa.GetLength(0) > 6 Then
    '            s = sa(1) + "," + sa(3) + "," + sa(5) + "," + sa(7)
    '        End If
    '
    '        ' now s holds a string that ColorConverter can understand
    '        Dim col As New ColorConverter()
    '        ' I have added a little code for exceptions
    '        Try
    '            ' If the color can not be converted, show an error => catch
    '            Result = col.ConvertFromString(s) 'some test
    '        Catch ex As Exception
    '            Result = CurrentColor
    '        End Try
    '
    '        Return Result
    '    End Function

    Private Sub AllScreensToFront()
        Me.LS.Select()
        Me.RS.Select()
        Me.Select()
    End Sub

    Private Sub btnPicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPictureBoth.Click, btnPictureLeft.Click, btnPictureRight.Click
        '* First, stop the slideshow if it's running.
        StopSlideShow()

        Dim of As New OpenFileDialog()
        of.Filter = "Image Files(*.BMP;*.GIF;*.JPG;*.WMF)|*.BMP;*.GIF;*.JPG;*.WMF"
        of.InitialDirectory = ROOT_SUPPORT_DIR
        If of.ShowDialog(Me) = DialogResult.OK Then
            Dim img As Image = Image.FromFile(of.FileName)
            If sender.Name <> "btnPictureRight" Then
                Me.picLeft.Image = img
                DisplayImage(Me.LS, img)
            End If
            If sender.Name <> "btnPictureLeft" Then
                Me.picRight.Image = img
                DisplayImage(Me.RS, img)
            End If
        End If
        of.Dispose()        '* We made a new one, we have to dispose of it.
        AllScreensToFront()
    End Sub

    Public Function AskIfSure(ByVal prompt As String) As Boolean
        If MessageBox.Show(prompt, "Are You Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly) = DialogResult.Yes Then
            Return True
        End If
        Return False
    End Function

    '=================================================================================================
    '* BEGIN 5 THINGS STUFF

    Private Sub btnAddThing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddThing.Click
        If Me.clbThings.Items.Count < MAX_THINGS Then
            Me.clbThings.Items.Add(Me.tbNewThing.Text)
            Me.tbNewThing.ResetText()
        Else
            MessageBox.Show("The list has already reached its max number of Things (" + CStr(MAX_THINGS) + "). If you need to add an item, remove another item first.", "Too many Things", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
        End If
        Me.tbNewThing.Focus()
        Me.clbThings.SelectedIndex = -1
    End Sub
    Private Sub btnClearThings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearThings.Click
        If AskIfSure("Are you sure you want to remove all the Things from the list?") Then
            Dim i As Integer
            For i = 0 To MAX_THINGS
                ThingSubs(i) = ""
            Next
            Me.clbThings.Items.Clear()
            Me.lblCurrentThing.Visible = False
            Me.tbSubstitutions.Visible = False
            Me.btnShowThingLeft.Visible = False
            Me.btnShowThingRight.Visible = False
            Me.btnShowThingBoth.Visible = False
            Me.tbNewThing.Focus()
        End If
        'Me.Select()
    End Sub
    Private Sub btnRemoveThing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveThing.Click
        If (Me.clbThings.SelectedIndex >= 0) Then
            Dim prompt As String = "Are you sure you want to remove Thing: '" + Me.clbThings.SelectedItem + "' ?"
            If AskIfSure(prompt) Then
                Dim i As Integer
                For i = Me.clbThings.SelectedIndex To (Me.clbThings.Items.Count - 1)
                    ThingSubs(i) = ThingSubs(i + 1)
                Next
                Me.clbThings.Items.RemoveAt(Me.clbThings.SelectedIndex())
            End If
        End If
    End Sub

    Private Sub clbThings_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clbThings.SelectedIndexChanged
        If sender.SelectedIndex >= 0 Then
            Me.lblCurrentThing.Text = sender.SelectedItem
            Me.tbSubstitutions.Text = ThingSubs(sender.SelectedIndex)
            Me.lblCurrentThing.Visible = True
            Me.tbSubstitutions.Visible = True
            Me.btnShowThingLeft.Visible = True
            Me.btnShowThingRight.Visible = True
            Me.btnShowThingBoth.Visible = True
            Me.clbThings.Refresh()
            Me.tbSubstitutions.Select(Me.tbSubstitutions.TextLength, 0)  '* deselect all text here
            Me.tbSubstitutions.Focus()
        Else
            Me.lblCurrentThing.Visible = False
            Me.tbSubstitutions.Visible = False
            Me.btnShowThingLeft.Visible = False
            Me.btnShowThingRight.Visible = False
            Me.btnShowThingBoth.Visible = False
            Me.tbNewThing.Focus()
        End If
    End Sub

    Private Sub btnThingUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnThingUp.Click
        Dim sel As Integer = Me.clbThings.SelectedIndex
        If sel > 0 Then
            Dim chk As Boolean = Me.clbThings.CheckedIndices.Contains(sel)
            Dim s As String = ThingSubs(sel - 1)
            ThingSubs(sel - 1) = ThingSubs(sel)
            ThingSubs(sel) = s

            Me.clbThings.Items.Insert(sel - 1, Me.clbThings.SelectedItem)
            If chk Then Me.clbThings.SetItemCheckState(sel - 1, CheckState.Checked)
            Me.clbThings.Items.RemoveAt(sel + 1)
            Me.clbThings.SelectedIndex = sel - 1
        End If
    End Sub
    Private Sub btnThingDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnThingDown.Click
        Dim sel As Integer = Me.clbThings.SelectedIndex
        If (sel < (Me.clbThings.Items.Count - 1)) And (sel >= 0) Then
            Dim chk As Boolean = Me.clbThings.CheckedIndices.Contains(sel)
            Dim s As String = ThingSubs(sel)
            ThingSubs(sel) = ThingSubs(sel + 1)
            ThingSubs(sel + 1) = s

            Me.clbThings.Items.Insert(sel + 2, Me.clbThings.Items.Item(sel))
            If chk Then Me.clbThings.SetItemCheckState(sel + 2, CheckState.Checked)
            Me.clbThings.Items.RemoveAt(sel)
            Me.clbThings.SelectedIndex = sel + 1
        End If
    End Sub

    Private Sub tbSubstitutions_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbSubstitutions.Leave
        ThingSubs(Me.clbThings.SelectedIndex) = sender.Text
    End Sub

    Private Sub btnList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnListLeft.Click, btnListRight.Click, btnListBoth.Click
        Dim i As Integer
        Dim s As String
        For i = 0 To (Me.clbThings.Items.Count - 1)
            If i > 0 Then s = s + EOL
            If Me.clbThings.CheckedIndices.Contains(i) Then s = s + ChrW(&H25BA)
            s = s + CStr(i + 1) + ". " + Me.clbThings.Items.Item(i)
        Next
        If sender.name <> "btnListRight" Then DisplayTextScreen(Me.LS, s, Me.clbThings.BackColor, 16)
        If sender.name <> "btnListLeft" Then DisplayTextScreen(Me.RS, s, Me.clbThings.BackColor, 16)
    End Sub
    Private Sub btnShowThing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowThingLeft.Click, btnShowThingRight.Click, btnShowThingBoth.Click
        Dim s As String
        s = Me.lblCurrentThing.Text + EOL + EOL + Me.tbSubstitutions.Text
        If sender.name <> "btnShowThingRight" Then DisplayTextScreen(Me.LS, s, clbThings.BackColor, 16)
        If sender.name <> "btnShowThingLeft" Then DisplayTextScreen(Me.RS, s, clbThings.BackColor, 16)
    End Sub
    Private Sub radioThingColor_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioThingColorLeft.CheckedChanged, radioThingColorRight.CheckedChanged
        Me.clbThings.BackColor = sender.BackColor
        Me.tbSubstitutions.BackColor = sender.BackColor
    End Sub

    Private Sub tbNewThing_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNewThing.Enter
        Me.AcceptButton = Me.btnAddThing
    End Sub
    Private Sub tbNewThing_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNewThing.Leave
        Me.AcceptButton = Nothing
    End Sub

    '=================================================================================================
    '* BEGIN SLIDESHOW STUFF

    Private Sub FTreeAutoExpand_C(ByVal fpath As String)
        '* This subroutine takes the path given, expands the foldertree nodes down
        '* the path as far as the subdirectories keep matching, from left to right
        '* in the backslash-separated string.
        '* The drive is assumed to be C:, so drive info should NOT be included!
        Dim idx As Integer = fpath.IndexOf("\")
        Dim nextchunk As String
        Dim n1 As System.Windows.Forms.TreeNode = Me.FolderTree1.Nodes(0).Nodes(1).Nodes(1)
        Dim n2 As System.Windows.Forms.TreeNode

        Me.FolderTree1.Nodes(0).Nodes(1).Expand()
        Me.FolderTree1.Nodes(0).Nodes(1).Nodes(1).Expand()

        While fpath.Length > 0
            Dim found As Boolean = False
            nextchunk = Microsoft.VisualBasic.Left(fpath, idx)
            For Each n2 In n1.Nodes
                If n2.Text = nextchunk Then
                    n2.Expand()
                    n1 = n2
                    found = True
                    Exit For
                End If
            Next
            If found Then
                fpath = Mid(fpath, idx + 2)
                idx = fpath.IndexOf("\")
                If idx < 1 Then idx = fpath.Length
            Else
                fpath = ""
            End If
        End While
        '* n1 still contains the name of the last thing we expanded. Activate it!
        '* Me.FolderTree1.SelectItem(n1.Index)
    End Sub


    Private Sub AddToSlideList(ByVal fnams As System.Windows.Forms.ListBox.SelectedObjectCollection)
        'Yup.
        Dim fobj As Object
        For Each fobj In fnams
            Me.lbSlideList.Items.Add(Me.FileListBox1.Path + "\" + fobj)
        Next
    End Sub
    Private Sub btnRemoveSlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveSlides.Click
        ' Only think about doing anything if at least one slide is selected.
        If Me.lbSlideList.SelectedIndices.Count > 0 Then
            Dim i As Integer
            For i = (Me.lbSlideList.SelectedIndices.Count - 1) To 0 Step -1
                Dim idx As Integer = Me.lbSlideList.Items.IndexOf(Me.lbSlideList.SelectedItems.Item(i))
                If (idx < (Me.lbSlideList.Items.Count)) And (idx >= 0) Then
                    Me.lbSlideList.Items.RemoveAt(idx)
                End If
            Next
        End If
        If Me.lbSlideList.Items.Count < 1 Then   '* stop the timing cycle
            StopSlideShow()
        End If
    End Sub
    Private Sub btnAddSlide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSlide.Click
        AddToSlideList(Me.FileListBox1.SelectedItems)
    End Sub
    Private Sub btnClearSlideList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearSlideList.Click
        If Me.lbSlideList.Items.Count < 1 Then Return
        If Not Me.AskIfSure("Clear all slides from the list?") Then Return
        StopSlideShow()
        Me.lbSlideList.Items.Clear()     '** Empty the list first
    End Sub
    Private Sub FileListBox1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles FileListBox1.DoubleClick
        AddToSlideList(sender.SelectedItems)
    End Sub
    Private Sub FileListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FileListBox1.SelectedIndexChanged
        If Me.FileListBox1.SelectedItems.Count = 1 Then
            Try
                Me.picSlidePreview.Image = Image.FromFile(Me.FileListBox1.Path + "\" + Me.FileListBox1.SelectedItem)
            Catch
            End Try
        Else
            Me.picSlidePreview.Image = Nothing
        End If
    End Sub
    Private Sub FolderTree1_AfterSelect(ByVal e As HyperCoder.Win.FileSystemControls.FolderTree.AfterSelectEventArgs) Handles FolderTree1.AfterSelect
        Static PrevSelect As String
        If Me.FolderTree1.SelectedNode.FilePath <> PrevSelect Then
            If Me.FolderTree1.SelectedNode.Type = HyperCoder.Win.FileSystemControls.FolderTree.NodeTypes.Folder Then
                Me.FileListBox1.Path = Me.FolderTree1.SelectedNode.FilePath
            Else
                Me.FileListBox1.Path = ""
            End If
            PrevSelect = Me.FolderTree1.SelectedNode.FilePath
            'FolderTree1.
            Me.picSlidePreview.Image = Nothing
        End If
    End Sub

    Private Sub btnSlideUp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSlideUp.Click
        ' Only think about doing anything if at least one slide is selected.
        If Me.lbSlideList.SelectedIndices.Count > 0 Then
            ' Only actually try to do anything if the very first slide isn't one of the chosen ones.
            If Me.lbSlideList.SelectedIndices(0) > 0 Then
                Dim i As Integer
                For i = 0 To (Me.lbSlideList.SelectedIndices.Count - 1)
                    Dim idx As Integer = Me.lbSlideList.SelectedIndices(i)
                    'Items.IndexOf(Me.lbSlideList.SelectedItems.Item(i))
                    Me.lbSlideList.Items.Insert(idx - 1, Me.lbSlideList.SelectedItems.Item(i))
                    Me.lbSlideList.Items.RemoveAt(idx + 1)
                    Me.lbSlideList.SetSelected(idx - 1, True)
                Next
            End If
        End If
    End Sub
    Private Sub btnSlideDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSlideDown.Click
        ' Only think about doing anything if at least one slide is selected.
        If Me.lbSlideList.SelectedIndices.Count > 0 Then
            ' Only actually try to do anything if the very last slide isn't one of the chosen ones.
            Dim LastIndex As Integer = Me.lbSlideList.SelectedIndices(Me.lbSlideList.SelectedIndices.Count - 1)
            If LastIndex < (Me.lbSlideList.Items.Count - 1) Then
                Dim i As Integer
                For i = (Me.lbSlideList.SelectedIndices.Count - 1) To 0 Step -1
                    Dim idx As Integer = Me.lbSlideList.SelectedIndices(i)
                    If (idx < (Me.lbSlideList.Items.Count - 1)) And (idx >= 0) Then
                        Me.lbSlideList.Items.Insert(idx + 2, Me.lbSlideList.Items.Item(idx))
                        Me.lbSlideList.Items.RemoveAt(idx)
                        Me.lbSlideList.SetSelected(idx + 1, True)
                    End If
                Next
            End If
        End If
    End Sub
    Private Sub btnLoadSlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadSlides.Click
        If Me.lbSlideList.Items.Count > 0 Then
            'There are already slides in the list - ask if we want to continue and overwrite
            If Not Me.AskIfSure("Replace the current Slideshow list?") Then Return
        End If
        Dim of As New OpenFileDialog()
        of.Filter = "JANIS Slideshow(*.JSL)|*.JSL"
        of.InitialDirectory = ROOT_SUPPORT_DIR + "\SlideShows"
        If of.ShowDialog(Me) = DialogResult.OK Then
            Me.lbSlideList.Items.Clear()     '** Empty the list first
            Dim fn As Integer = FreeFile()
            Dim FileErr As Boolean
            Try
                FileOpen(fn, of.FileName, OpenMode.Input)
            Catch ex As Exception
                FileErr = True
                MessageBox.Show("An error occurred opening file '" + of.FileName + "'.", "File Error")
            End Try
            If Not FileErr Then
                Dim s As String
                While Not EOF(fn)
                    Input(fn, s)
                    Me.lbSlideList.Items.Add(s)
                End While
                FileClose(fn)
            End If
        End If
        of.Dispose()
        AllScreensToFront()
    End Sub
    Private Sub btnSaveSlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSlides.Click
        If Me.lbSlideList.Items.Count = 0 Then
            MessageBox.Show("There are no items in the SlideShow yet.", "Nothing to Save", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
            Return
        End If
        Dim sf As New SaveFileDialog()
        sf.Filter = "JANIS Slideshow(*.JSL)|*.JSL"
        sf.InitialDirectory = ROOT_SUPPORT_DIR + "\SlideShows"
        If sf.ShowDialog(Me) = DialogResult.OK Then
            Dim fn As Integer = FreeFile()
            FileOpen(fn, sf.FileName, OpenMode.Output)
            Dim i As Integer
            For i = 0 To (Me.lbSlideList.Items.Count - 1)
                PrintLine(fn, Me.lbSlideList.Items.Item(i))
            Next
            FileClose(fn)
        End If
        sf.Dispose()
        AllScreensToFront()
    End Sub

    Private Sub SetTimerInterval()
        Me.Timer1.Interval = Me.nudDelay.Value * 1000
    End Sub
    Private Sub StartTimer()
        SetTimerInterval()
        Me.Timer1.Start()
    End Sub
    Private Sub StopSlideShow()
        Me.Timer1.Stop()
        SlidesActive = False
        Me.lbSlideList.SelectionMode = SelectionMode.MultiExtended
    End Sub
    Private Sub nudDelay_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDelay.ValueChanged
        SetTimerInterval()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Me.lbSlideList.Items.Count < 1 Then
            StopSlideShow()
            Return
        End If
        If Me.lbSlideList.SelectedIndex < (Me.lbSlideList.Items.Count - 1) Then
            Me.lbSlideList.SelectedIndex = Me.lbSlideList.SelectedIndex + 1
        Else
            Me.lbSlideList.SelectedIndex = 0
        End If
        DisplayBothImages(Me.lbSlideList.SelectedItem)
    End Sub

    Private Sub btnPlaySlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlaySlides.Click
        If SlidesActive Then Return
        If Me.lbSlideList.Items.Count < 1 Then Return
        Me.lbSlideList.SelectionMode = SelectionMode.One
        Me.lbSlideList.SelectedIndex = 0
        DisplayBothImages(Me.lbSlideList.SelectedItem)
        SlidesActive = True
        StartTimer()
    End Sub
    Private Sub btnStopSlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStopSlides.Click
        StopSlideShow()
    End Sub
    Private Sub btnChangeSlide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirstSlide.Click, btnLastSlide.Click
        If Not SlidesActive Then Return
        Me.Timer1.Stop()   '* temporary stoppage
        Select Case sender.Name
            Case "btnFirstSlide"
                Me.lbSlideList.SelectedIndex = 0
            Case "btnLastSlide"
                Me.lbSlideList.SelectedIndex = Me.lbSlideList.Items.Count - 1
        End Select
        DisplayBothImages(Me.lbSlideList.SelectedItem)
        StartTimer()
    End Sub

    '=================================================================================================

End Class
