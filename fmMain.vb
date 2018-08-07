Public Class fmMain
    Inherits System.Windows.Forms.Form

    '***************
    '* System Constants
    '***************
    Const MAX_THINGS As Integer = 9
    Const EOL As String = Chr(13) + Chr(10)
    Const ROOT_SUPPORT_DIR As String = "C:\Program Files\JANIS"
    Const PREFS_FILE As String = ROOT_SUPPORT_DIR + "\JANIS.ini"
    Const DEFAULT_SLIDESHOW_DIR As String = "\SlideShows"
    Const DEFAULT_HOTBUTTON_DIR As String = "\HotButtons"
    Const SLIDES_STOPPED As Integer = 0
    Const SLIDES_PAUSED As Integer = 1
    Const SLIDES_PLAYING As Integer = 2

    Dim TestMode As Boolean = False
    Dim DisplayModeAdjustment As Single = 1.0 ' Divide font setting by this for display. Differs for test/arena mode.
    Dim DisplayToEntryFontRatio As Single = 123 / 42  ' This is the size ratio of fonts in the display vs. in the textbox
    Dim DisplayFontRatio As Single = 15 / 44 ' This is the "should be" size ratio of display to what I once thought it was.

    Dim LS As fmScreen      '* The left team screen

    Dim ThingSubs(MAX_THINGS) As String             '* Substitutions for 5 Things
    Dim SlidesStatus As Integer = SLIDES_STOPPED    '* Keep track of whether Slideshow is running
    Dim HotButtonsChanged As Boolean                '* Have we changed the hot buttons?
    Dim CountdownSeconds As Integer


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
    Friend WithEvents btnPictureLeft As System.Windows.Forms.Button
    Friend WithEvents btnLeftScoreColor As System.Windows.Forms.Button
    Friend WithEvents btnRightScoreColor As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tbLeftFontSize As System.Windows.Forms.TextBox
    Friend WithEvents grpLeftColors As System.Windows.Forms.GroupBox
    Friend WithEvents btnChooseTextColorLeft As System.Windows.Forms.Button
    Friend WithEvents pnlTextColorLeft6 As System.Windows.Forms.Panel
    Friend WithEvents pnlTextColorLeft5 As System.Windows.Forms.Panel
    Friend WithEvents pnlTextColorLeft4 As System.Windows.Forms.Panel
    Friend WithEvents pnlTextColorLeft3 As System.Windows.Forms.Panel
    Friend WithEvents pnlTextColorLeft2 As System.Windows.Forms.Panel
    Friend WithEvents pnlTextColorLeft1 As System.Windows.Forms.Panel
    Friend WithEvents tbRightFontSize As System.Windows.Forms.TextBox
    Friend WithEvents grpRightColors As System.Windows.Forms.GroupBox
    Friend WithEvents btnChooseTextColorRight As System.Windows.Forms.Button
    Friend WithEvents pnlTextColorRight6 As System.Windows.Forms.Panel
    Friend WithEvents pnlTextColorRight5 As System.Windows.Forms.Panel
    Friend WithEvents pnlTextColorRight4 As System.Windows.Forms.Panel
    Friend WithEvents pnlTextColorRight3 As System.Windows.Forms.Panel
    Friend WithEvents pnlTextColorRight2 As System.Windows.Forms.Panel
    Friend WithEvents pnlTextColorRight1 As System.Windows.Forms.Panel
    Friend WithEvents tbRightText As System.Windows.Forms.TextBox
    Friend WithEvents tbLeftText As System.Windows.Forms.TextBox
    Friend WithEvents btnListLeft As System.Windows.Forms.Button
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
    Friend WithEvents tpHotButtons As System.Windows.Forms.TabPage
    Friend WithEvents tpPrefs As System.Windows.Forms.TabPage
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
    Friend WithEvents SlideTimer As System.Windows.Forms.Timer
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
    Friend WithEvents btnDocLoadBoth As System.Windows.Forms.Button
    Friend WithEvents btnDocLoadRight As System.Windows.Forms.Button
    Friend WithEvents btnDocLoadLeft As System.Windows.Forms.Button
    Friend WithEvents EasterEgg1 As System.Windows.Forms.MenuItem
    Friend WithEvents btnPrevSlide As System.Windows.Forms.Button
    Friend WithEvents btnNextSlide As System.Windows.Forms.Button
    Friend WithEvents btnPauseSlides As System.Windows.Forms.Button
    Friend WithEvents tbCurrentThing As System.Windows.Forms.TextBox
    Friend WithEvents btnHot1 As System.Windows.Forms.Button
    Friend WithEvents btnHot2 As System.Windows.Forms.Button
    Friend WithEvents btnHot3 As System.Windows.Forms.Button
    Friend WithEvents btnHot4 As System.Windows.Forms.Button
    Friend WithEvents btnHot5 As System.Windows.Forms.Button
    Friend WithEvents btnHot6 As System.Windows.Forms.Button
    Friend WithEvents btnHot7 As System.Windows.Forms.Button
    Friend WithEvents btnHot8 As System.Windows.Forms.Button
    Friend WithEvents btnHot9 As System.Windows.Forms.Button
    Friend WithEvents btnHot10 As System.Windows.Forms.Button
    Friend WithEvents cbHBActive As System.Windows.Forms.CheckBox
    Friend WithEvents tbHBtext3 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBtext2 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBtext1 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBtext6 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBtext5 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBtext4 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBtext10 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBtext9 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBtext8 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBtext7 As System.Windows.Forms.TextBox
    Friend WithEvents btnHBSelect1 As System.Windows.Forms.Button
    Friend WithEvents btnHBSelect2 As System.Windows.Forms.Button
    Friend WithEvents btnHBSelect4 As System.Windows.Forms.Button
    Friend WithEvents btnHBSelect3 As System.Windows.Forms.Button
    Friend WithEvents gbHB As System.Windows.Forms.GroupBox
    Friend WithEvents btnHBSelect10 As System.Windows.Forms.Button
    Friend WithEvents btnHBSelect9 As System.Windows.Forms.Button
    Friend WithEvents btnHBSelect8 As System.Windows.Forms.Button
    Friend WithEvents btnHBSelect7 As System.Windows.Forms.Button
    Friend WithEvents btnHBSelect6 As System.Windows.Forms.Button
    Friend WithEvents btnHBSelect5 As System.Windows.Forms.Button
    Friend WithEvents btnLoadHB As System.Windows.Forms.Button
    Friend WithEvents btnSaveHB As System.Windows.Forms.Button
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblHBinstructions As System.Windows.Forms.Label
    Friend WithEvents btnClearHB As System.Windows.Forms.Button
    Friend WithEvents tbHBfile1 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBfile2 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBfile4 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBfile3 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBfile6 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBfile5 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBfile8 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBfile7 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBfile10 As System.Windows.Forms.TextBox
    Friend WithEvents tbHBfile9 As System.Windows.Forms.TextBox
    Friend WithEvents btnShowRightText As System.Windows.Forms.Button
    Friend WithEvents btnShowLeftText As System.Windows.Forms.Button
    Friend WithEvents btnClearTextBoth As System.Windows.Forms.Button
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents btnClearTextRight As System.Windows.Forms.Button
    Friend WithEvents btnClearTextLeft As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents pnlPicBackLeft As System.Windows.Forms.Panel
    Friend WithEvents tbDefaultSlideShow As System.Windows.Forms.TextBox
    Friend WithEvents tbDefaultHBFile As System.Windows.Forms.TextBox
    Friend WithEvents tbDefaultImageFile As System.Windows.Forms.TextBox
    Friend WithEvents btnRevertPrefs As System.Windows.Forms.Button
    Friend WithEvents btnSavePrefs As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lblDefaultColorLeft As System.Windows.Forms.Label
    Friend WithEvents lblDefaultColorRight As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents tbDefaultFontSize As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents btnDefaultPrefs As System.Windows.Forms.Button
    Friend WithEvents grpDefaultColorsRight As System.Windows.Forms.GroupBox
    Friend WithEvents grpDefaultColorsLeft As System.Windows.Forms.GroupBox
    Friend WithEvents cbDisplayDefaultImage As System.Windows.Forms.CheckBox
    Friend WithEvents btnChooseDefaultImageDir As System.Windows.Forms.Button
    Friend WithEvents btnChooseDefaultTextColorRight As System.Windows.Forms.Button
    Friend WithEvents pnlDefaultTextColorRight6 As System.Windows.Forms.Panel
    Friend WithEvents pnlDefaultTextColorRight5 As System.Windows.Forms.Panel
    Friend WithEvents pnlDefaultTextColorRight4 As System.Windows.Forms.Panel
    Friend WithEvents pnlDefaultTextColorRight3 As System.Windows.Forms.Panel
    Friend WithEvents pnlDefaultTextColorRight2 As System.Windows.Forms.Panel
    Friend WithEvents pnlDefaultTextColorRight1 As System.Windows.Forms.Panel
    Friend WithEvents btnChooseDefaultTextColorLeft As System.Windows.Forms.Button
    Friend WithEvents pnlDefaultTextColorLeft6 As System.Windows.Forms.Panel
    Friend WithEvents pnlDefaultTextColorLeft5 As System.Windows.Forms.Panel
    Friend WithEvents pnlDefaultTextColorLeft4 As System.Windows.Forms.Panel
    Friend WithEvents pnlDefaultTextColorLeft3 As System.Windows.Forms.Panel
    Friend WithEvents pnlDefaultTextColorLeft2 As System.Windows.Forms.Panel
    Friend WithEvents pnlDefaultTextColorLeft1 As System.Windows.Forms.Panel
    Friend WithEvents cbPlaySlidesAtStart As System.Windows.Forms.CheckBox
    Friend WithEvents nudDefaultSlideDelay As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnChooseDefaultSlideShow As System.Windows.Forms.Button
    Friend WithEvents btnChooseDefaultHB As System.Windows.Forms.Button
    Friend WithEvents btnChooseDefaultImage As System.Windows.Forms.Button
    Friend WithEvents cbLoadDefaultSlides As System.Windows.Forms.CheckBox
    Friend WithEvents cbLoadDefaultHB As System.Windows.Forms.CheckBox
    Friend WithEvents tbDefaultImageDir As System.Windows.Forms.TextBox
    Friend WithEvents CountdownTimer As System.Windows.Forms.Timer
    Friend WithEvents MenuCountdownToggle As System.Windows.Forms.MenuItem
    Friend WithEvents btnStartCountdown As System.Windows.Forms.Button
    Friend WithEvents cbExpandPicLeft As System.Windows.Forms.CheckBox

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(fmMain))
        Me.btnBlackout = New System.Windows.Forms.Button()
        Me.tbLeftTeam = New System.Windows.Forms.TextBox()
        Me.tbRightTeam = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbLeftScore = New System.Windows.Forms.TextBox()
        Me.tbRightScore = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
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
        Me.EasterEgg1 = New System.Windows.Forms.MenuItem()
        Me.MenuCountdownToggle = New System.Windows.Forms.MenuItem()
        Me.picLeft = New System.Windows.Forms.PictureBox()
        Me.btnPictureLeft = New System.Windows.Forms.Button()
        Me.btnLeftScoreColor = New System.Windows.Forms.Button()
        Me.btnRightScoreColor = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tpScreenText = New System.Windows.Forms.TabPage()
        Me.btnStartCountdown = New System.Windows.Forms.Button()
        Me.btnClearTextBoth = New System.Windows.Forms.Button()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.btnClearTextRight = New System.Windows.Forms.Button()
        Me.btnClearTextLeft = New System.Windows.Forms.Button()
        Me.btnDocLoadBoth = New System.Windows.Forms.Button()
        Me.btnDocLoadRight = New System.Windows.Forms.Button()
        Me.btnDocLoadLeft = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.btnShowLeftText = New System.Windows.Forms.Button()
        Me.tbLeftText = New System.Windows.Forms.TextBox()
        Me.tbRightFontSize = New System.Windows.Forms.TextBox()
        Me.grpRightColors = New System.Windows.Forms.GroupBox()
        Me.btnChooseTextColorRight = New System.Windows.Forms.Button()
        Me.pnlTextColorRight6 = New System.Windows.Forms.Panel()
        Me.pnlTextColorRight5 = New System.Windows.Forms.Panel()
        Me.pnlTextColorRight4 = New System.Windows.Forms.Panel()
        Me.pnlTextColorRight3 = New System.Windows.Forms.Panel()
        Me.pnlTextColorRight2 = New System.Windows.Forms.Panel()
        Me.pnlTextColorRight1 = New System.Windows.Forms.Panel()
        Me.btnShowRightText = New System.Windows.Forms.Button()
        Me.tbRightText = New System.Windows.Forms.TextBox()
        Me.tbLeftFontSize = New System.Windows.Forms.TextBox()
        Me.grpLeftColors = New System.Windows.Forms.GroupBox()
        Me.btnChooseTextColorLeft = New System.Windows.Forms.Button()
        Me.pnlTextColorLeft6 = New System.Windows.Forms.Panel()
        Me.pnlTextColorLeft5 = New System.Windows.Forms.Panel()
        Me.pnlTextColorLeft4 = New System.Windows.Forms.Panel()
        Me.pnlTextColorLeft3 = New System.Windows.Forms.Panel()
        Me.pnlTextColorLeft2 = New System.Windows.Forms.Panel()
        Me.pnlTextColorLeft1 = New System.Windows.Forms.Panel()
        Me.tp5Things = New System.Windows.Forms.TabPage()
        Me.tbCurrentThing = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.clbThings = New System.Windows.Forms.CheckedListBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.grpThingsColor = New System.Windows.Forms.GroupBox()
        Me.radioThingColorRight = New System.Windows.Forms.RadioButton()
        Me.radioThingColorLeft = New System.Windows.Forms.RadioButton()
        Me.btnClearThings = New System.Windows.Forms.Button()
        Me.btnRemoveThing = New System.Windows.Forms.Button()
        Me.btnListLeft = New System.Windows.Forms.Button()
        Me.btnShowThingLeft = New System.Windows.Forms.Button()
        Me.tbSubstitutions = New System.Windows.Forms.TextBox()
        Me.btnAddThing = New System.Windows.Forms.Button()
        Me.tbNewThing = New System.Windows.Forms.TextBox()
        Me.btnThingDown = New System.Windows.Forms.Button()
        Me.btnThingUp = New System.Windows.Forms.Button()
        Me.tpHotButtons = New System.Windows.Forms.TabPage()
        Me.btnClearHB = New System.Windows.Forms.Button()
        Me.lblHBinstructions = New System.Windows.Forms.Label()
        Me.btnSaveHB = New System.Windows.Forms.Button()
        Me.btnLoadHB = New System.Windows.Forms.Button()
        Me.gbHB = New System.Windows.Forms.GroupBox()
        Me.tbHBfile10 = New System.Windows.Forms.TextBox()
        Me.tbHBfile9 = New System.Windows.Forms.TextBox()
        Me.tbHBfile8 = New System.Windows.Forms.TextBox()
        Me.tbHBfile7 = New System.Windows.Forms.TextBox()
        Me.tbHBfile6 = New System.Windows.Forms.TextBox()
        Me.tbHBfile5 = New System.Windows.Forms.TextBox()
        Me.tbHBfile4 = New System.Windows.Forms.TextBox()
        Me.tbHBfile3 = New System.Windows.Forms.TextBox()
        Me.tbHBfile2 = New System.Windows.Forms.TextBox()
        Me.tbHBfile1 = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.btnHBSelect10 = New System.Windows.Forms.Button()
        Me.btnHBSelect9 = New System.Windows.Forms.Button()
        Me.btnHBSelect8 = New System.Windows.Forms.Button()
        Me.btnHBSelect7 = New System.Windows.Forms.Button()
        Me.btnHBSelect6 = New System.Windows.Forms.Button()
        Me.btnHBSelect5 = New System.Windows.Forms.Button()
        Me.btnHBSelect4 = New System.Windows.Forms.Button()
        Me.btnHBSelect3 = New System.Windows.Forms.Button()
        Me.btnHBSelect2 = New System.Windows.Forms.Button()
        Me.btnHBSelect1 = New System.Windows.Forms.Button()
        Me.tbHBtext10 = New System.Windows.Forms.TextBox()
        Me.tbHBtext9 = New System.Windows.Forms.TextBox()
        Me.tbHBtext8 = New System.Windows.Forms.TextBox()
        Me.tbHBtext7 = New System.Windows.Forms.TextBox()
        Me.tbHBtext6 = New System.Windows.Forms.TextBox()
        Me.tbHBtext5 = New System.Windows.Forms.TextBox()
        Me.tbHBtext4 = New System.Windows.Forms.TextBox()
        Me.tbHBtext3 = New System.Windows.Forms.TextBox()
        Me.tbHBtext2 = New System.Windows.Forms.TextBox()
        Me.tbHBtext1 = New System.Windows.Forms.TextBox()
        Me.cbHBActive = New System.Windows.Forms.CheckBox()
        Me.tpSlides = New System.Windows.Forms.TabPage()
        Me.btnPauseSlides = New System.Windows.Forms.Button()
        Me.btnNextSlide = New System.Windows.Forms.Button()
        Me.btnPrevSlide = New System.Windows.Forms.Button()
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
        Me.tpPrefs = New System.Windows.Forms.TabPage()
        Me.btnDefaultPrefs = New System.Windows.Forms.Button()
        Me.tbDefaultImageDir = New System.Windows.Forms.TextBox()
        Me.btnChooseDefaultImageDir = New System.Windows.Forms.Button()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.tbDefaultFontSize = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblDefaultColorRight = New System.Windows.Forms.Label()
        Me.lblDefaultColorLeft = New System.Windows.Forms.Label()
        Me.grpDefaultColorsRight = New System.Windows.Forms.GroupBox()
        Me.btnChooseDefaultTextColorRight = New System.Windows.Forms.Button()
        Me.pnlDefaultTextColorRight6 = New System.Windows.Forms.Panel()
        Me.pnlDefaultTextColorRight5 = New System.Windows.Forms.Panel()
        Me.pnlDefaultTextColorRight4 = New System.Windows.Forms.Panel()
        Me.pnlDefaultTextColorRight3 = New System.Windows.Forms.Panel()
        Me.pnlDefaultTextColorRight2 = New System.Windows.Forms.Panel()
        Me.pnlDefaultTextColorRight1 = New System.Windows.Forms.Panel()
        Me.grpDefaultColorsLeft = New System.Windows.Forms.GroupBox()
        Me.btnChooseDefaultTextColorLeft = New System.Windows.Forms.Button()
        Me.pnlDefaultTextColorLeft6 = New System.Windows.Forms.Panel()
        Me.pnlDefaultTextColorLeft5 = New System.Windows.Forms.Panel()
        Me.pnlDefaultTextColorLeft4 = New System.Windows.Forms.Panel()
        Me.pnlDefaultTextColorLeft3 = New System.Windows.Forms.Panel()
        Me.pnlDefaultTextColorLeft2 = New System.Windows.Forms.Panel()
        Me.pnlDefaultTextColorLeft1 = New System.Windows.Forms.Panel()
        Me.btnSavePrefs = New System.Windows.Forms.Button()
        Me.btnRevertPrefs = New System.Windows.Forms.Button()
        Me.cbPlaySlidesAtStart = New System.Windows.Forms.CheckBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.nudDefaultSlideDelay = New System.Windows.Forms.NumericUpDown()
        Me.tbDefaultSlideShow = New System.Windows.Forms.TextBox()
        Me.btnChooseDefaultSlideShow = New System.Windows.Forms.Button()
        Me.tbDefaultHBFile = New System.Windows.Forms.TextBox()
        Me.btnChooseDefaultHB = New System.Windows.Forms.Button()
        Me.tbDefaultImageFile = New System.Windows.Forms.TextBox()
        Me.btnChooseDefaultImage = New System.Windows.Forms.Button()
        Me.cbDisplayDefaultImage = New System.Windows.Forms.CheckBox()
        Me.cbLoadDefaultSlides = New System.Windows.Forms.CheckBox()
        Me.cbLoadDefaultHB = New System.Windows.Forms.CheckBox()
        Me.tpAbout = New System.Windows.Forms.TabPage()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.SlideTimer = New System.Windows.Forms.Timer(Me.components)
        Me.btnHot1 = New System.Windows.Forms.Button()
        Me.btnHot2 = New System.Windows.Forms.Button()
        Me.btnHot3 = New System.Windows.Forms.Button()
        Me.btnHot4 = New System.Windows.Forms.Button()
        Me.btnHot5 = New System.Windows.Forms.Button()
        Me.btnHot6 = New System.Windows.Forms.Button()
        Me.btnHot7 = New System.Windows.Forms.Button()
        Me.btnHot8 = New System.Windows.Forms.Button()
        Me.btnHot9 = New System.Windows.Forms.Button()
        Me.btnHot10 = New System.Windows.Forms.Button()
        Me.pnlPicBackLeft = New System.Windows.Forms.Panel()
        Me.CountdownTimer = New System.Windows.Forms.Timer(Me.components)
        Me.cbExpandPicLeft = New System.Windows.Forms.CheckBox()
        Me.TabControl1.SuspendLayout()
        Me.tpScreenText.SuspendLayout()
        Me.grpRightColors.SuspendLayout()
        Me.grpLeftColors.SuspendLayout()
        Me.tp5Things.SuspendLayout()
        Me.grpThingsColor.SuspendLayout()
        Me.tpHotButtons.SuspendLayout()
        Me.gbHB.SuspendLayout()
        Me.tpSlides.SuspendLayout()
        CType(Me.nudDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpPrefs.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.grpDefaultColorsRight.SuspendLayout()
        Me.grpDefaultColorsLeft.SuspendLayout()
        CType(Me.nudDefaultSlideDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpAbout.SuspendLayout()
        Me.pnlPicBackLeft.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnBlackout
        '
        Me.btnBlackout.BackColor = System.Drawing.SystemColors.Control
        Me.btnBlackout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBlackout.ForeColor = System.Drawing.SystemColors.WindowText
        Me.btnBlackout.Location = New System.Drawing.Point(560, 100)
        Me.btnBlackout.Name = "btnBlackout"
        Me.btnBlackout.Size = New System.Drawing.Size(108, 40)
        Me.btnBlackout.TabIndex = 13
        Me.btnBlackout.Text = "&BLACKOUT"
        '
        'tbLeftTeam
        '
        Me.tbLeftTeam.BackColor = System.Drawing.SystemColors.Window
        Me.tbLeftTeam.ForeColor = System.Drawing.SystemColors.WindowText
        Me.tbLeftTeam.Location = New System.Drawing.Point(88, 4)
        Me.tbLeftTeam.MaxLength = 20
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
        Me.tbRightTeam.Location = New System.Drawing.Point(544, 4)
        Me.tbRightTeam.MaxLength = 20
        Me.tbRightTeam.Name = "tbRightTeam"
        Me.tbRightTeam.Size = New System.Drawing.Size(136, 20)
        Me.tbRightTeam.TabIndex = 8
        Me.tbRightTeam.Text = ""
        Me.tbRightTeam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(4, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Team Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(688, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 24)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Team Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbLeftScore
        '
        Me.tbLeftScore.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(176, Byte))
        Me.tbLeftScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLeftScore.ForeColor = System.Drawing.Color.White
        Me.tbLeftScore.Location = New System.Drawing.Point(88, 28)
        Me.tbLeftScore.MaxLength = 3
        Me.tbLeftScore.Name = "tbLeftScore"
        Me.tbLeftScore.Size = New System.Drawing.Size(44, 22)
        Me.tbLeftScore.TabIndex = 3
        Me.tbLeftScore.Text = "0"
        Me.tbLeftScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbRightScore
        '
        Me.tbRightScore.BackColor = System.Drawing.Color.Maroon
        Me.tbRightScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbRightScore.ForeColor = System.Drawing.Color.White
        Me.tbRightScore.Location = New System.Drawing.Point(636, 28)
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
        Me.Label3.Location = New System.Drawing.Point(28, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 24)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Score"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(688, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 24)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Score"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnScoreBoth
        '
        Me.btnScoreBoth.Location = New System.Drawing.Point(244, 8)
        Me.btnScoreBoth.Name = "btnScoreBoth"
        Me.btnScoreBoth.Size = New System.Drawing.Size(280, 40)
        Me.btnScoreBoth.TabIndex = 6
        Me.btnScoreBoth.Text = "SHOW SCORE"
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuDummy})
        '
        'menuDummy
        '
        Me.menuDummy.Index = 0
        Me.menuDummy.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuAdd1Left, Me.menuSubtract1Left, Me.menuAdd5Left, Me.menuSubtract5Left, Me.menuAdd1Right, Me.menuSubtract1Right, Me.menuAdd5Right, Me.menuSubtract5Right, Me.EasterEgg1, Me.MenuCountdownToggle})
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
        'EasterEgg1
        '
        Me.EasterEgg1.Index = 8
        Me.EasterEgg1.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftB
        Me.EasterEgg1.Text = "Bill Loves Betse!"
        '
        'MenuCountdownToggle
        '
        Me.MenuCountdownToggle.Index = 9
        Me.MenuCountdownToggle.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftT
        Me.MenuCountdownToggle.Text = "CountdownTimer"
        '
        'picLeft
        '
        Me.picLeft.BackColor = System.Drawing.Color.Transparent
        Me.picLeft.Name = "picLeft"
        Me.picLeft.Size = New System.Drawing.Size(160, 120)
        Me.picLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLeft.TabIndex = 28
        Me.picLeft.TabStop = False
        '
        'btnPictureLeft
        '
        Me.btnPictureLeft.Location = New System.Drawing.Point(236, 100)
        Me.btnPictureLeft.Name = "btnPictureLeft"
        Me.btnPictureLeft.Size = New System.Drawing.Size(84, 40)
        Me.btnPictureLeft.TabIndex = 14
        Me.btnPictureLeft.Text = "SHOW PICTURE"
        '
        'btnLeftScoreColor
        '
        Me.btnLeftScoreColor.Location = New System.Drawing.Point(144, 28)
        Me.btnLeftScoreColor.Name = "btnLeftScoreColor"
        Me.btnLeftScoreColor.Size = New System.Drawing.Size(68, 24)
        Me.btnLeftScoreColor.TabIndex = 4
        Me.btnLeftScoreColor.Text = "Color..."
        '
        'btnRightScoreColor
        '
        Me.btnRightScoreColor.Location = New System.Drawing.Point(556, 28)
        Me.btnRightScoreColor.Name = "btnRightScoreColor"
        Me.btnRightScoreColor.Size = New System.Drawing.Size(68, 24)
        Me.btnRightScoreColor.TabIndex = 10
        Me.btnRightScoreColor.Text = "Color..."
        '
        'TabControl1
        '
        Me.TabControl1.Controls.AddRange(New System.Windows.Forms.Control() {Me.tpScreenText, Me.tp5Things, Me.tpHotButtons, Me.tpSlides, Me.tpPrefs, Me.tpAbout})
        Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.HotTrack = True
        Me.TabControl1.ItemSize = New System.Drawing.Size(128, 21)
        Me.TabControl1.Location = New System.Drawing.Point(0, 216)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(772, 360)
        Me.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.TabControl1.TabIndex = 17
        '
        'tpScreenText
        '
        Me.tpScreenText.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnStartCountdown, Me.btnClearTextBoth, Me.Label15, Me.btnClearTextRight, Me.btnClearTextLeft, Me.btnDocLoadBoth, Me.btnDocLoadRight, Me.btnDocLoadLeft, Me.Label11, Me.Label10, Me.btnShowLeftText, Me.tbLeftText, Me.tbRightFontSize, Me.grpRightColors, Me.btnShowRightText, Me.tbRightText, Me.tbLeftFontSize, Me.grpLeftColors})
        Me.tpScreenText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tpScreenText.Location = New System.Drawing.Point(4, 25)
        Me.tpScreenText.Name = "tpScreenText"
        Me.tpScreenText.Size = New System.Drawing.Size(764, 331)
        Me.tpScreenText.TabIndex = 0
        Me.tpScreenText.Text = "Text Display"
        '
        'btnStartCountdown
        '
        Me.btnStartCountdown.Location = New System.Drawing.Point(332, 16)
        Me.btnStartCountdown.Name = "btnStartCountdown"
        Me.btnStartCountdown.Size = New System.Drawing.Size(100, 24)
        Me.btnStartCountdown.TabIndex = 57
        Me.btnStartCountdown.Text = "START TIMER"
        Me.btnStartCountdown.Visible = False
        '
        'btnClearTextBoth
        '
        Me.btnClearTextBoth.BackColor = System.Drawing.Color.Transparent
        Me.btnClearTextBoth.Font = New System.Drawing.Font("Wingdings", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnClearTextBoth.ForeColor = System.Drawing.SystemColors.WindowText
        Me.btnClearTextBoth.Location = New System.Drawing.Point(364, 128)
        Me.btnClearTextBoth.Name = "btnClearTextBoth"
        Me.btnClearTextBoth.Size = New System.Drawing.Size(36, 36)
        Me.btnClearTextBoth.TabIndex = 60
        Me.btnClearTextBoth.Text = "ó"
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(332, 172)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(96, 16)
        Me.Label15.TabIndex = 59
        Me.Label15.Text = "CLEAR TEXT"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnClearTextRight
        '
        Me.btnClearTextRight.BackColor = System.Drawing.Color.Transparent
        Me.btnClearTextRight.Font = New System.Drawing.Font("Wingdings", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnClearTextRight.ForeColor = System.Drawing.SystemColors.WindowText
        Me.btnClearTextRight.Location = New System.Drawing.Point(420, 128)
        Me.btnClearTextRight.Name = "btnClearTextRight"
        Me.btnClearTextRight.Size = New System.Drawing.Size(36, 36)
        Me.btnClearTextRight.TabIndex = 58
        Me.btnClearTextRight.Text = "ð"
        '
        'btnClearTextLeft
        '
        Me.btnClearTextLeft.BackColor = System.Drawing.Color.Transparent
        Me.btnClearTextLeft.Font = New System.Drawing.Font("Wingdings", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnClearTextLeft.ForeColor = System.Drawing.SystemColors.WindowText
        Me.btnClearTextLeft.Location = New System.Drawing.Point(308, 128)
        Me.btnClearTextLeft.Name = "btnClearTextLeft"
        Me.btnClearTextLeft.Size = New System.Drawing.Size(36, 36)
        Me.btnClearTextLeft.TabIndex = 57
        Me.btnClearTextLeft.Text = "ï"
        '
        'btnDocLoadBoth
        '
        Me.btnDocLoadBoth.Location = New System.Drawing.Point(304, 300)
        Me.btnDocLoadBoth.Name = "btnDocLoadBoth"
        Me.btnDocLoadBoth.Size = New System.Drawing.Size(156, 24)
        Me.btnDocLoadBoth.TabIndex = 44
        Me.btnDocLoadBoth.Tag = "Both"
        Me.btnDocLoadBoth.Text = "DOCUMENT LOAD BOTH"
        '
        'btnDocLoadRight
        '
        Me.btnDocLoadRight.Location = New System.Drawing.Point(520, 300)
        Me.btnDocLoadRight.Name = "btnDocLoadRight"
        Me.btnDocLoadRight.Size = New System.Drawing.Size(156, 24)
        Me.btnDocLoadRight.TabIndex = 47
        Me.btnDocLoadRight.Tag = "Right"
        Me.btnDocLoadRight.Text = "DOCUMENT LOAD RIGHT"
        '
        'btnDocLoadLeft
        '
        Me.btnDocLoadLeft.Location = New System.Drawing.Point(88, 300)
        Me.btnDocLoadLeft.Name = "btnDocLoadLeft"
        Me.btnDocLoadLeft.Size = New System.Drawing.Size(156, 24)
        Me.btnDocLoadLeft.TabIndex = 43
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
        'btnShowLeftText
        '
        Me.btnShowLeftText.ForeColor = System.Drawing.SystemColors.WindowText
        Me.btnShowLeftText.Location = New System.Drawing.Point(120, 268)
        Me.btnShowLeftText.Name = "btnShowLeftText"
        Me.btnShowLeftText.Size = New System.Drawing.Size(92, 24)
        Me.btnShowLeftText.TabIndex = 41
        Me.btnShowLeftText.Text = "SHOW TEXT"
        '
        'tbLeftText
        '
        Me.tbLeftText.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(176, Byte))
        Me.tbLeftText.Font = New System.Drawing.Font("Arial", 20.0!, System.Drawing.FontStyle.Bold)
        Me.tbLeftText.ForeColor = System.Drawing.Color.White
        Me.tbLeftText.Location = New System.Drawing.Point(24, 52)
        Me.tbLeftText.Multiline = True
        Me.tbLeftText.Name = "tbLeftText"
        Me.tbLeftText.Size = New System.Drawing.Size(280, 210)
        Me.tbLeftText.TabIndex = 38
        Me.tbLeftText.Text = "JANIS v2.0 Beta" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "(Single Display)" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "by" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Bill Cernansky"
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
        Me.grpRightColors.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnChooseTextColorRight, Me.pnlTextColorRight6, Me.pnlTextColorRight5, Me.pnlTextColorRight4, Me.pnlTextColorRight3, Me.pnlTextColorRight2, Me.pnlTextColorRight1})
        Me.grpRightColors.Location = New System.Drawing.Point(512, 12)
        Me.grpRightColors.Name = "grpRightColors"
        Me.grpRightColors.Size = New System.Drawing.Size(228, 40)
        Me.grpRightColors.TabIndex = 30
        Me.grpRightColors.TabStop = False
        Me.grpRightColors.Text = "Background"
        '
        'btnChooseTextColorRight
        '
        Me.btnChooseTextColorRight.Location = New System.Drawing.Point(152, 12)
        Me.btnChooseTextColorRight.Name = "btnChooseTextColorRight"
        Me.btnChooseTextColorRight.Size = New System.Drawing.Size(68, 24)
        Me.btnChooseTextColorRight.TabIndex = 37
        Me.btnChooseTextColorRight.Text = "Choose..."
        '
        'pnlTextColorRight6
        '
        Me.pnlTextColorRight6.BackColor = System.Drawing.Color.Black
        Me.pnlTextColorRight6.Location = New System.Drawing.Point(128, 16)
        Me.pnlTextColorRight6.Name = "pnlTextColorRight6"
        Me.pnlTextColorRight6.Size = New System.Drawing.Size(16, 16)
        Me.pnlTextColorRight6.TabIndex = 36
        Me.pnlTextColorRight6.TabStop = True
        '
        'pnlTextColorRight5
        '
        Me.pnlTextColorRight5.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(64, Byte), CType(0, Byte))
        Me.pnlTextColorRight5.Location = New System.Drawing.Point(104, 16)
        Me.pnlTextColorRight5.Name = "pnlTextColorRight5"
        Me.pnlTextColorRight5.Size = New System.Drawing.Size(16, 16)
        Me.pnlTextColorRight5.TabIndex = 35
        Me.pnlTextColorRight5.TabStop = True
        '
        'pnlTextColorRight4
        '
        Me.pnlTextColorRight4.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(0, Byte), CType(64, Byte))
        Me.pnlTextColorRight4.Location = New System.Drawing.Point(80, 16)
        Me.pnlTextColorRight4.Name = "pnlTextColorRight4"
        Me.pnlTextColorRight4.Size = New System.Drawing.Size(16, 16)
        Me.pnlTextColorRight4.TabIndex = 34
        Me.pnlTextColorRight4.TabStop = True
        '
        'pnlTextColorRight3
        '
        Me.pnlTextColorRight3.BackColor = System.Drawing.Color.FromArgb(CType(100, Byte), CType(50, Byte), CType(0, Byte))
        Me.pnlTextColorRight3.Location = New System.Drawing.Point(56, 16)
        Me.pnlTextColorRight3.Name = "pnlTextColorRight3"
        Me.pnlTextColorRight3.Size = New System.Drawing.Size(16, 16)
        Me.pnlTextColorRight3.TabIndex = 33
        Me.pnlTextColorRight3.TabStop = True
        '
        'pnlTextColorRight2
        '
        Me.pnlTextColorRight2.BackColor = System.Drawing.Color.Maroon
        Me.pnlTextColorRight2.Location = New System.Drawing.Point(32, 16)
        Me.pnlTextColorRight2.Name = "pnlTextColorRight2"
        Me.pnlTextColorRight2.Size = New System.Drawing.Size(16, 16)
        Me.pnlTextColorRight2.TabIndex = 32
        Me.pnlTextColorRight2.TabStop = True
        '
        'pnlTextColorRight1
        '
        Me.pnlTextColorRight1.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(176, Byte))
        Me.pnlTextColorRight1.Location = New System.Drawing.Point(8, 16)
        Me.pnlTextColorRight1.Name = "pnlTextColorRight1"
        Me.pnlTextColorRight1.Size = New System.Drawing.Size(16, 16)
        Me.pnlTextColorRight1.TabIndex = 31
        Me.pnlTextColorRight1.TabStop = True
        '
        'btnShowRightText
        '
        Me.btnShowRightText.AllowDrop = True
        Me.btnShowRightText.ForeColor = System.Drawing.SystemColors.WindowText
        Me.btnShowRightText.Location = New System.Drawing.Point(552, 268)
        Me.btnShowRightText.Name = "btnShowRightText"
        Me.btnShowRightText.Size = New System.Drawing.Size(92, 24)
        Me.btnShowRightText.TabIndex = 45
        Me.btnShowRightText.Text = "SHOW TEXT"
        '
        'tbRightText
        '
        Me.tbRightText.BackColor = System.Drawing.Color.Maroon
        Me.tbRightText.Font = New System.Drawing.Font("Arial", 20.0!, System.Drawing.FontStyle.Bold)
        Me.tbRightText.ForeColor = System.Drawing.Color.White
        Me.tbRightText.Location = New System.Drawing.Point(460, 52)
        Me.tbRightText.Multiline = True
        Me.tbRightText.Name = "tbRightText"
        Me.tbRightText.Size = New System.Drawing.Size(280, 210)
        Me.tbRightText.TabIndex = 40
        Me.tbRightText.Text = ""
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
        Me.grpLeftColors.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnChooseTextColorLeft, Me.pnlTextColorLeft6, Me.pnlTextColorLeft5, Me.pnlTextColorLeft4, Me.pnlTextColorLeft3, Me.pnlTextColorLeft2, Me.pnlTextColorLeft1})
        Me.grpLeftColors.Location = New System.Drawing.Point(76, 12)
        Me.grpLeftColors.Name = "grpLeftColors"
        Me.grpLeftColors.Size = New System.Drawing.Size(228, 40)
        Me.grpLeftColors.TabIndex = 20
        Me.grpLeftColors.TabStop = False
        Me.grpLeftColors.Text = "Background"
        '
        'btnChooseTextColorLeft
        '
        Me.btnChooseTextColorLeft.Location = New System.Drawing.Point(152, 12)
        Me.btnChooseTextColorLeft.Name = "btnChooseTextColorLeft"
        Me.btnChooseTextColorLeft.Size = New System.Drawing.Size(68, 24)
        Me.btnChooseTextColorLeft.TabIndex = 27
        Me.btnChooseTextColorLeft.Text = "Choose..."
        '
        'pnlTextColorLeft6
        '
        Me.pnlTextColorLeft6.BackColor = System.Drawing.Color.Black
        Me.pnlTextColorLeft6.Location = New System.Drawing.Point(128, 16)
        Me.pnlTextColorLeft6.Name = "pnlTextColorLeft6"
        Me.pnlTextColorLeft6.Size = New System.Drawing.Size(16, 16)
        Me.pnlTextColorLeft6.TabIndex = 26
        Me.pnlTextColorLeft6.TabStop = True
        '
        'pnlTextColorLeft5
        '
        Me.pnlTextColorLeft5.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(64, Byte), CType(0, Byte))
        Me.pnlTextColorLeft5.Location = New System.Drawing.Point(104, 16)
        Me.pnlTextColorLeft5.Name = "pnlTextColorLeft5"
        Me.pnlTextColorLeft5.Size = New System.Drawing.Size(16, 16)
        Me.pnlTextColorLeft5.TabIndex = 25
        Me.pnlTextColorLeft5.TabStop = True
        '
        'pnlTextColorLeft4
        '
        Me.pnlTextColorLeft4.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(0, Byte), CType(64, Byte))
        Me.pnlTextColorLeft4.Location = New System.Drawing.Point(80, 16)
        Me.pnlTextColorLeft4.Name = "pnlTextColorLeft4"
        Me.pnlTextColorLeft4.Size = New System.Drawing.Size(16, 16)
        Me.pnlTextColorLeft4.TabIndex = 24
        Me.pnlTextColorLeft4.TabStop = True
        '
        'pnlTextColorLeft3
        '
        Me.pnlTextColorLeft3.BackColor = System.Drawing.Color.FromArgb(CType(100, Byte), CType(50, Byte), CType(0, Byte))
        Me.pnlTextColorLeft3.Location = New System.Drawing.Point(56, 16)
        Me.pnlTextColorLeft3.Name = "pnlTextColorLeft3"
        Me.pnlTextColorLeft3.Size = New System.Drawing.Size(16, 16)
        Me.pnlTextColorLeft3.TabIndex = 23
        Me.pnlTextColorLeft3.TabStop = True
        '
        'pnlTextColorLeft2
        '
        Me.pnlTextColorLeft2.BackColor = System.Drawing.Color.Maroon
        Me.pnlTextColorLeft2.Location = New System.Drawing.Point(32, 16)
        Me.pnlTextColorLeft2.Name = "pnlTextColorLeft2"
        Me.pnlTextColorLeft2.Size = New System.Drawing.Size(16, 16)
        Me.pnlTextColorLeft2.TabIndex = 22
        Me.pnlTextColorLeft2.TabStop = True
        '
        'pnlTextColorLeft1
        '
        Me.pnlTextColorLeft1.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(176, Byte))
        Me.pnlTextColorLeft1.Location = New System.Drawing.Point(8, 16)
        Me.pnlTextColorLeft1.Name = "pnlTextColorLeft1"
        Me.pnlTextColorLeft1.Size = New System.Drawing.Size(16, 16)
        Me.pnlTextColorLeft1.TabIndex = 21
        Me.pnlTextColorLeft1.TabStop = True
        '
        'tp5Things
        '
        Me.tp5Things.Controls.AddRange(New System.Windows.Forms.Control() {Me.tbCurrentThing, Me.Label12, Me.clbThings, Me.Label5, Me.grpThingsColor, Me.btnClearThings, Me.btnRemoveThing, Me.btnListLeft, Me.btnShowThingLeft, Me.tbSubstitutions, Me.btnAddThing, Me.tbNewThing, Me.btnThingDown, Me.btnThingUp})
        Me.tp5Things.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tp5Things.Location = New System.Drawing.Point(4, 25)
        Me.tp5Things.Name = "tp5Things"
        Me.tp5Things.Size = New System.Drawing.Size(764, 331)
        Me.tp5Things.TabIndex = 1
        Me.tp5Things.Text = "5/6 Things"
        '
        'tbCurrentThing
        '
        Me.tbCurrentThing.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(176, Byte))
        Me.tbCurrentThing.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbCurrentThing.ForeColor = System.Drawing.Color.White
        Me.tbCurrentThing.Location = New System.Drawing.Point(464, 108)
        Me.tbCurrentThing.Name = "tbCurrentThing"
        Me.tbCurrentThing.Size = New System.Drawing.Size(232, 24)
        Me.tbCurrentThing.TabIndex = 63
        Me.tbCurrentThing.Text = "Substitutions"
        Me.tbCurrentThing.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(68, 40)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(212, 16)
        Me.Label12.TabIndex = 48
        Me.Label12.Text = "Name of Thing to Add"
        '
        'clbThings
        '
        Me.clbThings.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(176, Byte))
        Me.clbThings.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clbThings.ForeColor = System.Drawing.Color.White
        Me.clbThings.Location = New System.Drawing.Point(68, 96)
        Me.clbThings.Name = "clbThings"
        Me.clbThings.Size = New System.Drawing.Size(212, 148)
        Me.clbThings.TabIndex = 51
        Me.clbThings.ThreeDCheckBoxes = True
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 160)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 20)
        Me.Label5.TabIndex = 53
        Me.Label5.Text = "ORDER"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpThingsColor
        '
        Me.grpThingsColor.BackColor = System.Drawing.Color.Transparent
        Me.grpThingsColor.Controls.AddRange(New System.Windows.Forms.Control() {Me.radioThingColorRight, Me.radioThingColorLeft})
        Me.grpThingsColor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpThingsColor.Location = New System.Drawing.Point(408, 36)
        Me.grpThingsColor.Name = "grpThingsColor"
        Me.grpThingsColor.Size = New System.Drawing.Size(332, 48)
        Me.grpThingsColor.TabIndex = 60
        Me.grpThingsColor.TabStop = False
        Me.grpThingsColor.Text = "Select Currently Playing Team"
        '
        'radioThingColorRight
        '
        Me.radioThingColorRight.BackColor = System.Drawing.Color.Maroon
        Me.radioThingColorRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radioThingColorRight.ForeColor = System.Drawing.Color.White
        Me.radioThingColorRight.Location = New System.Drawing.Point(172, 16)
        Me.radioThingColorRight.Name = "radioThingColorRight"
        Me.radioThingColorRight.Size = New System.Drawing.Size(156, 28)
        Me.radioThingColorRight.TabIndex = 62
        Me.radioThingColorRight.Text = "Right"
        Me.radioThingColorRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'radioThingColorLeft
        '
        Me.radioThingColorLeft.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(176, Byte))
        Me.radioThingColorLeft.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.radioThingColorLeft.Checked = True
        Me.radioThingColorLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radioThingColorLeft.ForeColor = System.Drawing.Color.White
        Me.radioThingColorLeft.Location = New System.Drawing.Point(4, 16)
        Me.radioThingColorLeft.Name = "radioThingColorLeft"
        Me.radioThingColorLeft.Size = New System.Drawing.Size(156, 28)
        Me.radioThingColorLeft.TabIndex = 61
        Me.radioThingColorLeft.TabStop = True
        Me.radioThingColorLeft.Text = "Left"
        Me.radioThingColorLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnClearThings
        '
        Me.btnClearThings.Location = New System.Drawing.Point(296, 192)
        Me.btnClearThings.Name = "btnClearThings"
        Me.btnClearThings.Size = New System.Drawing.Size(80, 32)
        Me.btnClearThings.TabIndex = 56
        Me.btnClearThings.Text = "CLEAR LIST"
        '
        'btnRemoveThing
        '
        Me.btnRemoveThing.Location = New System.Drawing.Point(296, 120)
        Me.btnRemoveThing.Name = "btnRemoveThing"
        Me.btnRemoveThing.Size = New System.Drawing.Size(80, 40)
        Me.btnRemoveThing.TabIndex = 55
        Me.btnRemoveThing.Text = "REMOVE SELECTION"
        '
        'btnListLeft
        '
        Me.btnListLeft.Location = New System.Drawing.Point(120, 268)
        Me.btnListLeft.Name = "btnListLeft"
        Me.btnListLeft.Size = New System.Drawing.Size(88, 32)
        Me.btnListLeft.TabIndex = 57
        Me.btnListLeft.Text = "SHOW LIST"
        '
        'btnShowThingLeft
        '
        Me.btnShowThingLeft.Location = New System.Drawing.Point(520, 268)
        Me.btnShowThingLeft.Name = "btnShowThingLeft"
        Me.btnShowThingLeft.Size = New System.Drawing.Size(116, 32)
        Me.btnShowThingLeft.TabIndex = 65
        Me.btnShowThingLeft.Text = "SHOW THIS THING"
        '
        'tbSubstitutions
        '
        Me.tbSubstitutions.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(176, Byte))
        Me.tbSubstitutions.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbSubstitutions.ForeColor = System.Drawing.Color.White
        Me.tbSubstitutions.Location = New System.Drawing.Point(464, 132)
        Me.tbSubstitutions.Multiline = True
        Me.tbSubstitutions.Name = "tbSubstitutions"
        Me.tbSubstitutions.Size = New System.Drawing.Size(232, 112)
        Me.tbSubstitutions.TabIndex = 64
        Me.tbSubstitutions.Text = ""
        Me.tbSubstitutions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnAddThing
        '
        Me.btnAddThing.Location = New System.Drawing.Point(296, 48)
        Me.btnAddThing.Name = "btnAddThing"
        Me.btnAddThing.Size = New System.Drawing.Size(80, 32)
        Me.btnAddThing.TabIndex = 50
        Me.btnAddThing.Text = "ADD THING"
        '
        'tbNewThing
        '
        Me.tbNewThing.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbNewThing.Location = New System.Drawing.Point(68, 56)
        Me.tbNewThing.Name = "tbNewThing"
        Me.tbNewThing.Size = New System.Drawing.Size(212, 23)
        Me.tbNewThing.TabIndex = 49
        Me.tbNewThing.Text = ""
        '
        'btnThingDown
        '
        Me.btnThingDown.Font = New System.Drawing.Font("Webdings", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnThingDown.Location = New System.Drawing.Point(16, 192)
        Me.btnThingDown.Name = "btnThingDown"
        Me.btnThingDown.Size = New System.Drawing.Size(40, 32)
        Me.btnThingDown.TabIndex = 54
        Me.btnThingDown.Text = "6"
        '
        'btnThingUp
        '
        Me.btnThingUp.Font = New System.Drawing.Font("Webdings", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnThingUp.Location = New System.Drawing.Point(16, 116)
        Me.btnThingUp.Name = "btnThingUp"
        Me.btnThingUp.Size = New System.Drawing.Size(40, 32)
        Me.btnThingUp.TabIndex = 52
        Me.btnThingUp.Text = "5"
        '
        'tpHotButtons
        '
        Me.tpHotButtons.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnClearHB, Me.lblHBinstructions, Me.btnSaveHB, Me.btnLoadHB, Me.gbHB, Me.cbHBActive})
        Me.tpHotButtons.Location = New System.Drawing.Point(4, 25)
        Me.tpHotButtons.Name = "tpHotButtons"
        Me.tpHotButtons.Size = New System.Drawing.Size(764, 331)
        Me.tpHotButtons.TabIndex = 4
        Me.tpHotButtons.Text = "Hot Buttons"
        '
        'btnClearHB
        '
        Me.btnClearHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearHB.Location = New System.Drawing.Point(20, 208)
        Me.btnClearHB.Name = "btnClearHB"
        Me.btnClearHB.Size = New System.Drawing.Size(116, 32)
        Me.btnClearHB.TabIndex = 43
        Me.btnClearHB.Text = "CLEAR ALL"
        '
        'lblHBinstructions
        '
        Me.lblHBinstructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHBinstructions.ForeColor = System.Drawing.Color.DarkBlue
        Me.lblHBinstructions.Location = New System.Drawing.Point(4, 56)
        Me.lblHBinstructions.Name = "lblHBinstructions"
        Me.lblHBinstructions.Size = New System.Drawing.Size(148, 144)
        Me.lblHBinstructions.TabIndex = 42
        Me.lblHBinstructions.Text = "Hot Buttons are image shortcuts that you can define for quick access to stored im" & _
        "ages. Select a name && image for each button. Save groups of buttons for specifi" & _
        "c uses."
        Me.lblHBinstructions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnSaveHB
        '
        Me.btnSaveHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveHB.Location = New System.Drawing.Point(20, 288)
        Me.btnSaveHB.Name = "btnSaveHB"
        Me.btnSaveHB.Size = New System.Drawing.Size(116, 32)
        Me.btnSaveHB.TabIndex = 6
        Me.btnSaveHB.Text = "SAVE"
        '
        'btnLoadHB
        '
        Me.btnLoadHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadHB.Location = New System.Drawing.Point(20, 248)
        Me.btnLoadHB.Name = "btnLoadHB"
        Me.btnLoadHB.Size = New System.Drawing.Size(116, 32)
        Me.btnLoadHB.TabIndex = 5
        Me.btnLoadHB.Text = "LOAD"
        '
        'gbHB
        '
        Me.gbHB.Controls.AddRange(New System.Windows.Forms.Control() {Me.tbHBfile10, Me.tbHBfile9, Me.tbHBfile8, Me.tbHBfile7, Me.tbHBfile6, Me.tbHBfile5, Me.tbHBfile4, Me.tbHBfile3, Me.tbHBfile2, Me.tbHBfile1, Me.Label14, Me.Label13, Me.Label30, Me.Label31, Me.Label28, Me.Label29, Me.Label26, Me.Label27, Me.Label24, Me.Label25, Me.Label23, Me.Label22, Me.btnHBSelect10, Me.btnHBSelect9, Me.btnHBSelect8, Me.btnHBSelect7, Me.btnHBSelect6, Me.btnHBSelect5, Me.btnHBSelect4, Me.btnHBSelect3, Me.btnHBSelect2, Me.btnHBSelect1, Me.tbHBtext10, Me.tbHBtext9, Me.tbHBtext8, Me.tbHBtext7, Me.tbHBtext6, Me.tbHBtext5, Me.tbHBtext4, Me.tbHBtext3, Me.tbHBtext2, Me.tbHBtext1})
        Me.gbHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbHB.Location = New System.Drawing.Point(156, 4)
        Me.gbHB.Name = "gbHB"
        Me.gbHB.Size = New System.Drawing.Size(608, 320)
        Me.gbHB.TabIndex = 4
        Me.gbHB.TabStop = False
        '
        'tbHBfile10
        '
        Me.tbHBfile10.BackColor = System.Drawing.SystemColors.Control
        Me.tbHBfile10.Location = New System.Drawing.Point(160, 292)
        Me.tbHBfile10.Name = "tbHBfile10"
        Me.tbHBfile10.ReadOnly = True
        Me.tbHBfile10.Size = New System.Drawing.Size(368, 23)
        Me.tbHBfile10.TabIndex = 55
        Me.tbHBfile10.Text = ""
        '
        'tbHBfile9
        '
        Me.tbHBfile9.BackColor = System.Drawing.SystemColors.Control
        Me.tbHBfile9.Location = New System.Drawing.Point(160, 264)
        Me.tbHBfile9.Name = "tbHBfile9"
        Me.tbHBfile9.ReadOnly = True
        Me.tbHBfile9.Size = New System.Drawing.Size(368, 23)
        Me.tbHBfile9.TabIndex = 54
        Me.tbHBfile9.Text = ""
        '
        'tbHBfile8
        '
        Me.tbHBfile8.BackColor = System.Drawing.SystemColors.Control
        Me.tbHBfile8.Location = New System.Drawing.Point(160, 236)
        Me.tbHBfile8.Name = "tbHBfile8"
        Me.tbHBfile8.ReadOnly = True
        Me.tbHBfile8.Size = New System.Drawing.Size(368, 23)
        Me.tbHBfile8.TabIndex = 53
        Me.tbHBfile8.Text = ""
        '
        'tbHBfile7
        '
        Me.tbHBfile7.BackColor = System.Drawing.SystemColors.Control
        Me.tbHBfile7.Location = New System.Drawing.Point(160, 208)
        Me.tbHBfile7.Name = "tbHBfile7"
        Me.tbHBfile7.ReadOnly = True
        Me.tbHBfile7.Size = New System.Drawing.Size(368, 23)
        Me.tbHBfile7.TabIndex = 52
        Me.tbHBfile7.Text = ""
        '
        'tbHBfile6
        '
        Me.tbHBfile6.BackColor = System.Drawing.SystemColors.Control
        Me.tbHBfile6.Location = New System.Drawing.Point(160, 180)
        Me.tbHBfile6.Name = "tbHBfile6"
        Me.tbHBfile6.ReadOnly = True
        Me.tbHBfile6.Size = New System.Drawing.Size(368, 23)
        Me.tbHBfile6.TabIndex = 51
        Me.tbHBfile6.Text = ""
        '
        'tbHBfile5
        '
        Me.tbHBfile5.BackColor = System.Drawing.SystemColors.Control
        Me.tbHBfile5.Location = New System.Drawing.Point(160, 152)
        Me.tbHBfile5.Name = "tbHBfile5"
        Me.tbHBfile5.ReadOnly = True
        Me.tbHBfile5.Size = New System.Drawing.Size(368, 23)
        Me.tbHBfile5.TabIndex = 50
        Me.tbHBfile5.Text = ""
        '
        'tbHBfile4
        '
        Me.tbHBfile4.BackColor = System.Drawing.SystemColors.Control
        Me.tbHBfile4.Location = New System.Drawing.Point(160, 124)
        Me.tbHBfile4.Name = "tbHBfile4"
        Me.tbHBfile4.ReadOnly = True
        Me.tbHBfile4.Size = New System.Drawing.Size(368, 23)
        Me.tbHBfile4.TabIndex = 49
        Me.tbHBfile4.Text = ""
        '
        'tbHBfile3
        '
        Me.tbHBfile3.BackColor = System.Drawing.SystemColors.Control
        Me.tbHBfile3.Location = New System.Drawing.Point(160, 96)
        Me.tbHBfile3.Name = "tbHBfile3"
        Me.tbHBfile3.ReadOnly = True
        Me.tbHBfile3.Size = New System.Drawing.Size(368, 23)
        Me.tbHBfile3.TabIndex = 48
        Me.tbHBfile3.Text = ""
        '
        'tbHBfile2
        '
        Me.tbHBfile2.BackColor = System.Drawing.SystemColors.Control
        Me.tbHBfile2.Location = New System.Drawing.Point(160, 68)
        Me.tbHBfile2.Name = "tbHBfile2"
        Me.tbHBfile2.ReadOnly = True
        Me.tbHBfile2.Size = New System.Drawing.Size(368, 23)
        Me.tbHBfile2.TabIndex = 47
        Me.tbHBfile2.Text = ""
        '
        'tbHBfile1
        '
        Me.tbHBfile1.BackColor = System.Drawing.SystemColors.Control
        Me.tbHBfile1.Location = New System.Drawing.Point(160, 40)
        Me.tbHBfile1.Name = "tbHBfile1"
        Me.tbHBfile1.ReadOnly = True
        Me.tbHBfile1.Size = New System.Drawing.Size(368, 23)
        Me.tbHBfile1.TabIndex = 46
        Me.tbHBfile1.Text = ""
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, (System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(180, 16)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(328, 20)
        Me.Label14.TabIndex = 45
        Me.Label14.Text = "Image File"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, (System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(68, 16)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(96, 20)
        Me.Label13.TabIndex = 44
        Me.Label13.Text = "Button Name"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label30
        '
        Me.Label30.Location = New System.Drawing.Point(8, 296)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(64, 20)
        Me.Label30.TabIndex = 43
        Me.Label30.Text = "Button 10"
        '
        'Label31
        '
        Me.Label31.Location = New System.Drawing.Point(8, 268)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(64, 20)
        Me.Label31.TabIndex = 42
        Me.Label31.Text = "Button 9"
        '
        'Label28
        '
        Me.Label28.Location = New System.Drawing.Point(8, 240)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(64, 20)
        Me.Label28.TabIndex = 41
        Me.Label28.Text = "Button 8"
        '
        'Label29
        '
        Me.Label29.Location = New System.Drawing.Point(8, 212)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(64, 20)
        Me.Label29.TabIndex = 40
        Me.Label29.Text = "Button 7"
        '
        'Label26
        '
        Me.Label26.Location = New System.Drawing.Point(8, 184)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(64, 20)
        Me.Label26.TabIndex = 39
        Me.Label26.Text = "Button 6"
        '
        'Label27
        '
        Me.Label27.Location = New System.Drawing.Point(8, 156)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(64, 20)
        Me.Label27.TabIndex = 38
        Me.Label27.Text = "Button 5"
        '
        'Label24
        '
        Me.Label24.Location = New System.Drawing.Point(8, 128)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(64, 20)
        Me.Label24.TabIndex = 37
        Me.Label24.Text = "Button 4"
        '
        'Label25
        '
        Me.Label25.Location = New System.Drawing.Point(8, 100)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(64, 20)
        Me.Label25.TabIndex = 36
        Me.Label25.Text = "Button 3"
        '
        'Label23
        '
        Me.Label23.Location = New System.Drawing.Point(8, 72)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(64, 20)
        Me.Label23.TabIndex = 35
        Me.Label23.Text = "Button 2"
        '
        'Label22
        '
        Me.Label22.Location = New System.Drawing.Point(8, 44)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(64, 20)
        Me.Label22.TabIndex = 34
        Me.Label22.Text = "Button 1"
        '
        'btnHBSelect10
        '
        Me.btnHBSelect10.Location = New System.Drawing.Point(532, 292)
        Me.btnHBSelect10.Name = "btnHBSelect10"
        Me.btnHBSelect10.Size = New System.Drawing.Size(68, 23)
        Me.btnHBSelect10.TabIndex = 33
        Me.btnHBSelect10.Tag = "9"
        Me.btnHBSelect10.Text = "Select..."
        '
        'btnHBSelect9
        '
        Me.btnHBSelect9.Location = New System.Drawing.Point(532, 264)
        Me.btnHBSelect9.Name = "btnHBSelect9"
        Me.btnHBSelect9.Size = New System.Drawing.Size(68, 23)
        Me.btnHBSelect9.TabIndex = 32
        Me.btnHBSelect9.Tag = "8"
        Me.btnHBSelect9.Text = "Select..."
        '
        'btnHBSelect8
        '
        Me.btnHBSelect8.Location = New System.Drawing.Point(532, 236)
        Me.btnHBSelect8.Name = "btnHBSelect8"
        Me.btnHBSelect8.Size = New System.Drawing.Size(68, 23)
        Me.btnHBSelect8.TabIndex = 31
        Me.btnHBSelect8.Tag = "7"
        Me.btnHBSelect8.Text = "Select..."
        '
        'btnHBSelect7
        '
        Me.btnHBSelect7.Location = New System.Drawing.Point(532, 208)
        Me.btnHBSelect7.Name = "btnHBSelect7"
        Me.btnHBSelect7.Size = New System.Drawing.Size(68, 23)
        Me.btnHBSelect7.TabIndex = 30
        Me.btnHBSelect7.Tag = "6"
        Me.btnHBSelect7.Text = "Select..."
        '
        'btnHBSelect6
        '
        Me.btnHBSelect6.Location = New System.Drawing.Point(532, 180)
        Me.btnHBSelect6.Name = "btnHBSelect6"
        Me.btnHBSelect6.Size = New System.Drawing.Size(68, 23)
        Me.btnHBSelect6.TabIndex = 29
        Me.btnHBSelect6.Tag = "5"
        Me.btnHBSelect6.Text = "Select..."
        '
        'btnHBSelect5
        '
        Me.btnHBSelect5.Location = New System.Drawing.Point(532, 152)
        Me.btnHBSelect5.Name = "btnHBSelect5"
        Me.btnHBSelect5.Size = New System.Drawing.Size(68, 23)
        Me.btnHBSelect5.TabIndex = 28
        Me.btnHBSelect5.Tag = "4"
        Me.btnHBSelect5.Text = "Select..."
        '
        'btnHBSelect4
        '
        Me.btnHBSelect4.Location = New System.Drawing.Point(532, 124)
        Me.btnHBSelect4.Name = "btnHBSelect4"
        Me.btnHBSelect4.Size = New System.Drawing.Size(68, 23)
        Me.btnHBSelect4.TabIndex = 27
        Me.btnHBSelect4.Tag = "3"
        Me.btnHBSelect4.Text = "Select..."
        '
        'btnHBSelect3
        '
        Me.btnHBSelect3.Location = New System.Drawing.Point(532, 96)
        Me.btnHBSelect3.Name = "btnHBSelect3"
        Me.btnHBSelect3.Size = New System.Drawing.Size(68, 23)
        Me.btnHBSelect3.TabIndex = 26
        Me.btnHBSelect3.Tag = "2"
        Me.btnHBSelect3.Text = "Select..."
        '
        'btnHBSelect2
        '
        Me.btnHBSelect2.Location = New System.Drawing.Point(532, 68)
        Me.btnHBSelect2.Name = "btnHBSelect2"
        Me.btnHBSelect2.Size = New System.Drawing.Size(68, 23)
        Me.btnHBSelect2.TabIndex = 25
        Me.btnHBSelect2.Tag = "1"
        Me.btnHBSelect2.Text = "Select..."
        '
        'btnHBSelect1
        '
        Me.btnHBSelect1.Location = New System.Drawing.Point(532, 40)
        Me.btnHBSelect1.Name = "btnHBSelect1"
        Me.btnHBSelect1.Size = New System.Drawing.Size(68, 23)
        Me.btnHBSelect1.TabIndex = 24
        Me.btnHBSelect1.Tag = "0"
        Me.btnHBSelect1.Text = "Select..."
        '
        'tbHBtext10
        '
        Me.tbHBtext10.Location = New System.Drawing.Point(76, 292)
        Me.tbHBtext10.MaxLength = 10
        Me.tbHBtext10.Name = "tbHBtext10"
        Me.tbHBtext10.Size = New System.Drawing.Size(80, 23)
        Me.tbHBtext10.TabIndex = 13
        Me.tbHBtext10.Tag = "9"
        Me.tbHBtext10.Text = "Hot 10"
        '
        'tbHBtext9
        '
        Me.tbHBtext9.Location = New System.Drawing.Point(76, 264)
        Me.tbHBtext9.MaxLength = 10
        Me.tbHBtext9.Name = "tbHBtext9"
        Me.tbHBtext9.Size = New System.Drawing.Size(80, 23)
        Me.tbHBtext9.TabIndex = 12
        Me.tbHBtext9.Tag = "8"
        Me.tbHBtext9.Text = "Hot 9"
        '
        'tbHBtext8
        '
        Me.tbHBtext8.Location = New System.Drawing.Point(76, 236)
        Me.tbHBtext8.MaxLength = 10
        Me.tbHBtext8.Name = "tbHBtext8"
        Me.tbHBtext8.Size = New System.Drawing.Size(80, 23)
        Me.tbHBtext8.TabIndex = 11
        Me.tbHBtext8.Tag = "7"
        Me.tbHBtext8.Text = "Hot 8"
        '
        'tbHBtext7
        '
        Me.tbHBtext7.Location = New System.Drawing.Point(76, 208)
        Me.tbHBtext7.MaxLength = 10
        Me.tbHBtext7.Name = "tbHBtext7"
        Me.tbHBtext7.Size = New System.Drawing.Size(80, 23)
        Me.tbHBtext7.TabIndex = 10
        Me.tbHBtext7.Tag = "6"
        Me.tbHBtext7.Text = "Hot 7"
        '
        'tbHBtext6
        '
        Me.tbHBtext6.Location = New System.Drawing.Point(76, 180)
        Me.tbHBtext6.MaxLength = 10
        Me.tbHBtext6.Name = "tbHBtext6"
        Me.tbHBtext6.Size = New System.Drawing.Size(80, 23)
        Me.tbHBtext6.TabIndex = 9
        Me.tbHBtext6.Tag = "5"
        Me.tbHBtext6.Text = "Hot 6"
        '
        'tbHBtext5
        '
        Me.tbHBtext5.Location = New System.Drawing.Point(76, 152)
        Me.tbHBtext5.MaxLength = 10
        Me.tbHBtext5.Name = "tbHBtext5"
        Me.tbHBtext5.Size = New System.Drawing.Size(80, 23)
        Me.tbHBtext5.TabIndex = 8
        Me.tbHBtext5.Tag = "4"
        Me.tbHBtext5.Text = "Hot 5"
        '
        'tbHBtext4
        '
        Me.tbHBtext4.Location = New System.Drawing.Point(76, 124)
        Me.tbHBtext4.MaxLength = 10
        Me.tbHBtext4.Name = "tbHBtext4"
        Me.tbHBtext4.Size = New System.Drawing.Size(80, 23)
        Me.tbHBtext4.TabIndex = 7
        Me.tbHBtext4.Tag = "3"
        Me.tbHBtext4.Text = "Hot 4"
        '
        'tbHBtext3
        '
        Me.tbHBtext3.Location = New System.Drawing.Point(76, 96)
        Me.tbHBtext3.MaxLength = 10
        Me.tbHBtext3.Name = "tbHBtext3"
        Me.tbHBtext3.Size = New System.Drawing.Size(80, 23)
        Me.tbHBtext3.TabIndex = 6
        Me.tbHBtext3.Tag = "2"
        Me.tbHBtext3.Text = "Hot 3"
        '
        'tbHBtext2
        '
        Me.tbHBtext2.Location = New System.Drawing.Point(76, 68)
        Me.tbHBtext2.MaxLength = 10
        Me.tbHBtext2.Name = "tbHBtext2"
        Me.tbHBtext2.Size = New System.Drawing.Size(80, 23)
        Me.tbHBtext2.TabIndex = 5
        Me.tbHBtext2.Tag = "1"
        Me.tbHBtext2.Text = "Hot 2"
        '
        'tbHBtext1
        '
        Me.tbHBtext1.Location = New System.Drawing.Point(76, 40)
        Me.tbHBtext1.MaxLength = 10
        Me.tbHBtext1.Name = "tbHBtext1"
        Me.tbHBtext1.Size = New System.Drawing.Size(80, 23)
        Me.tbHBtext1.TabIndex = 4
        Me.tbHBtext1.Tag = "0"
        Me.tbHBtext1.Text = "Hot 1"
        '
        'cbHBActive
        '
        Me.cbHBActive.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cbHBActive.Checked = True
        Me.cbHBActive.CheckState = System.Windows.Forms.CheckState.Indeterminate
        Me.cbHBActive.Location = New System.Drawing.Point(4, 12)
        Me.cbHBActive.Name = "cbHBActive"
        Me.cbHBActive.Size = New System.Drawing.Size(148, 40)
        Me.cbHBActive.TabIndex = 0
        Me.cbHBActive.Text = "Show Hot Buttons"
        Me.cbHBActive.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'tpSlides
        '
        Me.tpSlides.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnPauseSlides, Me.btnNextSlide, Me.btnPrevSlide, Me.btnClearSlideList, Me.FolderTree1, Me.Label9, Me.nudDelay, Me.Label8, Me.Label7, Me.btnSaveSlides, Me.btnLoadSlides, Me.FileListBox1, Me.btnRemoveSlides, Me.btnAddSlide, Me.Label6, Me.btnSlideDown, Me.btnSlideUp, Me.btnStopSlides, Me.btnLastSlide, Me.btnPlaySlides, Me.btnFirstSlide, Me.lbSlideList, Me.picSlidePreview})
        Me.tpSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tpSlides.Location = New System.Drawing.Point(4, 25)
        Me.tpSlides.Name = "tpSlides"
        Me.tpSlides.Size = New System.Drawing.Size(764, 331)
        Me.tpSlides.TabIndex = 2
        Me.tpSlides.Text = "Slide Show"
        '
        'btnPauseSlides
        '
        Me.btnPauseSlides.Font = New System.Drawing.Font("Webdings", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.btnPauseSlides.ForeColor = System.Drawing.Color.Blue
        Me.btnPauseSlides.Location = New System.Drawing.Point(620, 300)
        Me.btnPauseSlides.Name = "btnPauseSlides"
        Me.btnPauseSlides.Size = New System.Drawing.Size(28, 28)
        Me.btnPauseSlides.TabIndex = 86
        Me.btnPauseSlides.Text = ";"
        '
        'btnNextSlide
        '
        Me.btnNextSlide.Font = New System.Drawing.Font("Webdings", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNextSlide.Location = New System.Drawing.Point(692, 300)
        Me.btnNextSlide.Name = "btnNextSlide"
        Me.btnNextSlide.Size = New System.Drawing.Size(28, 28)
        Me.btnNextSlide.TabIndex = 88
        Me.btnNextSlide.Text = "8"
        '
        'btnPrevSlide
        '
        Me.btnPrevSlide.Font = New System.Drawing.Font("Webdings", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrevSlide.Location = New System.Drawing.Point(548, 300)
        Me.btnPrevSlide.Name = "btnPrevSlide"
        Me.btnPrevSlide.Size = New System.Drawing.Size(28, 28)
        Me.btnPrevSlide.TabIndex = 84
        Me.btnPrevSlide.Text = "7"
        '
        'btnClearSlideList
        '
        Me.btnClearSlideList.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearSlideList.Location = New System.Drawing.Point(416, 176)
        Me.btnClearSlideList.Name = "btnClearSlideList"
        Me.btnClearSlideList.Size = New System.Drawing.Size(64, 28)
        Me.btnClearSlideList.TabIndex = 78
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
        Me.FolderTree1.TabIndex = 68
        Me.FolderTree1.Text = "FolderTree"
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(388, 300)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(56, 24)
        Me.Label9.TabIndex = 81
        Me.Label9.Text = "Seconds:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudDelay
        '
        Me.nudDelay.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudDelay.Location = New System.Drawing.Point(448, 300)
        Me.nudDelay.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
        Me.nudDelay.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudDelay.Name = "nudDelay"
        Me.nudDelay.Size = New System.Drawing.Size(56, 26)
        Me.nudDelay.TabIndex = 82
        Me.nudDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.nudDelay.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(512, 4)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(244, 16)
        Me.Label8.TabIndex = 72
        Me.Label8.Text = "Slide List"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Location = New System.Drawing.Point(200, 4)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(120, 16)
        Me.Label7.TabIndex = 68
        Me.Label7.Text = "Preview"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnSaveSlides
        '
        Me.btnSaveSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveSlides.Location = New System.Drawing.Point(400, 260)
        Me.btnSaveSlides.Name = "btnSaveSlides"
        Me.btnSaveSlides.Size = New System.Drawing.Size(96, 28)
        Me.btnSaveSlides.TabIndex = 80
        Me.btnSaveSlides.Text = "Save Slideshow"
        '
        'btnLoadSlides
        '
        Me.btnLoadSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadSlides.Location = New System.Drawing.Point(400, 224)
        Me.btnLoadSlides.Name = "btnLoadSlides"
        Me.btnLoadSlides.Size = New System.Drawing.Size(96, 28)
        Me.btnLoadSlides.TabIndex = 79
        Me.btnLoadSlides.Text = "Load Slideshow"
        '
        'FileListBox1
        '
        Me.FileListBox1.Location = New System.Drawing.Point(200, 112)
        Me.FileListBox1.Name = "FileListBox1"
        Me.FileListBox1.Pattern = "*.jpg;*.gif;*.bmp;*.wmf"
        Me.FileListBox1.Size = New System.Drawing.Size(184, 212)
        Me.FileListBox1.TabIndex = 70
        '
        'btnRemoveSlides
        '
        Me.btnRemoveSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveSlides.Location = New System.Drawing.Point(416, 128)
        Me.btnRemoveSlides.Name = "btnRemoveSlides"
        Me.btnRemoveSlides.Size = New System.Drawing.Size(64, 32)
        Me.btnRemoveSlides.TabIndex = 77
        Me.btnRemoveSlides.Text = "Remove"
        '
        'btnAddSlide
        '
        Me.btnAddSlide.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddSlide.Location = New System.Drawing.Point(324, 36)
        Me.btnAddSlide.Name = "btnAddSlide"
        Me.btnAddSlide.Size = New System.Drawing.Size(60, 48)
        Me.btnAddSlide.TabIndex = 71
        Me.btnAddSlide.Text = "Add To List"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(420, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 15)
        Me.Label6.TabIndex = 75
        Me.Label6.Text = "ORDER"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnSlideDown
        '
        Me.btnSlideDown.Font = New System.Drawing.Font("Webdings", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSlideDown.Location = New System.Drawing.Point(432, 80)
        Me.btnSlideDown.Name = "btnSlideDown"
        Me.btnSlideDown.Size = New System.Drawing.Size(32, 28)
        Me.btnSlideDown.TabIndex = 76
        Me.btnSlideDown.Text = "6"
        '
        'btnSlideUp
        '
        Me.btnSlideUp.Font = New System.Drawing.Font("Webdings", 20.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSlideUp.Location = New System.Drawing.Point(432, 28)
        Me.btnSlideUp.Name = "btnSlideUp"
        Me.btnSlideUp.Size = New System.Drawing.Size(32, 28)
        Me.btnSlideUp.TabIndex = 74
        Me.btnSlideUp.Text = "5"
        '
        'btnStopSlides
        '
        Me.btnStopSlides.Font = New System.Drawing.Font("Webdings", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStopSlides.ForeColor = System.Drawing.Color.Red
        Me.btnStopSlides.Location = New System.Drawing.Point(656, 300)
        Me.btnStopSlides.Name = "btnStopSlides"
        Me.btnStopSlides.Size = New System.Drawing.Size(28, 28)
        Me.btnStopSlides.TabIndex = 87
        Me.btnStopSlides.Text = "<"
        '
        'btnLastSlide
        '
        Me.btnLastSlide.Font = New System.Drawing.Font("Webdings", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLastSlide.Location = New System.Drawing.Point(728, 300)
        Me.btnLastSlide.Name = "btnLastSlide"
        Me.btnLastSlide.Size = New System.Drawing.Size(28, 28)
        Me.btnLastSlide.TabIndex = 89
        Me.btnLastSlide.Text = ":"
        '
        'btnPlaySlides
        '
        Me.btnPlaySlides.Font = New System.Drawing.Font("Webdings", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPlaySlides.ForeColor = System.Drawing.Color.Green
        Me.btnPlaySlides.Location = New System.Drawing.Point(584, 300)
        Me.btnPlaySlides.Name = "btnPlaySlides"
        Me.btnPlaySlides.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnPlaySlides.Size = New System.Drawing.Size(28, 28)
        Me.btnPlaySlides.TabIndex = 85
        Me.btnPlaySlides.Text = "4"
        '
        'btnFirstSlide
        '
        Me.btnFirstSlide.Font = New System.Drawing.Font("Webdings", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFirstSlide.Location = New System.Drawing.Point(512, 300)
        Me.btnFirstSlide.Name = "btnFirstSlide"
        Me.btnFirstSlide.Size = New System.Drawing.Size(28, 28)
        Me.btnFirstSlide.TabIndex = 83
        Me.btnFirstSlide.Text = "9"
        '
        'lbSlideList
        '
        Me.lbSlideList.HorizontalScrollbar = True
        Me.lbSlideList.Location = New System.Drawing.Point(508, 20)
        Me.lbSlideList.Name = "lbSlideList"
        Me.lbSlideList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lbSlideList.Size = New System.Drawing.Size(252, 277)
        Me.lbSlideList.TabIndex = 73
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
        'tpPrefs
        '
        Me.tpPrefs.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnDefaultPrefs, Me.tbDefaultImageDir, Me.btnChooseDefaultImageDir, Me.Label18, Me.tbDefaultFontSize, Me.Label17, Me.GroupBox2, Me.btnSavePrefs, Me.btnRevertPrefs, Me.cbPlaySlidesAtStart, Me.Label16, Me.nudDefaultSlideDelay, Me.tbDefaultSlideShow, Me.btnChooseDefaultSlideShow, Me.tbDefaultHBFile, Me.btnChooseDefaultHB, Me.tbDefaultImageFile, Me.btnChooseDefaultImage, Me.cbDisplayDefaultImage, Me.cbLoadDefaultSlides, Me.cbLoadDefaultHB})
        Me.tpPrefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tpPrefs.Location = New System.Drawing.Point(4, 25)
        Me.tpPrefs.Name = "tpPrefs"
        Me.tpPrefs.Size = New System.Drawing.Size(764, 331)
        Me.tpPrefs.TabIndex = 5
        Me.tpPrefs.Text = "Preferences"
        '
        'btnDefaultPrefs
        '
        Me.btnDefaultPrefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDefaultPrefs.Location = New System.Drawing.Point(8, 288)
        Me.btnDefaultPrefs.Name = "btnDefaultPrefs"
        Me.btnDefaultPrefs.Size = New System.Drawing.Size(172, 28)
        Me.btnDefaultPrefs.TabIndex = 97
        Me.btnDefaultPrefs.Text = "FACTORY DEFAULTS"
        '
        'tbDefaultImageDir
        '
        Me.tbDefaultImageDir.BackColor = System.Drawing.SystemColors.Control
        Me.tbDefaultImageDir.Location = New System.Drawing.Point(224, 100)
        Me.tbDefaultImageDir.Name = "tbDefaultImageDir"
        Me.tbDefaultImageDir.ReadOnly = True
        Me.tbDefaultImageDir.Size = New System.Drawing.Size(464, 20)
        Me.tbDefaultImageDir.TabIndex = 96
        Me.tbDefaultImageDir.Text = "C:\Program Files\JANIS"
        '
        'btnChooseDefaultImageDir
        '
        Me.btnChooseDefaultImageDir.Location = New System.Drawing.Point(692, 100)
        Me.btnChooseDefaultImageDir.Name = "btnChooseDefaultImageDir"
        Me.btnChooseDefaultImageDir.Size = New System.Drawing.Size(64, 23)
        Me.btnChooseDefaultImageDir.TabIndex = 95
        Me.btnChooseDefaultImageDir.Tag = "0"
        Me.btnChooseDefaultImageDir.Text = "Select..."
        '
        'Label18
        '
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(8, 100)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(212, 20)
        Me.Label18.TabIndex = 94
        Me.Label18.Text = "Default image search directory:"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbDefaultFontSize
        '
        Me.tbDefaultFontSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.tbDefaultFontSize.Location = New System.Drawing.Point(224, 68)
        Me.tbDefaultFontSize.MaxLength = 3
        Me.tbDefaultFontSize.Name = "tbDefaultFontSize"
        Me.tbDefaultFontSize.Size = New System.Drawing.Size(52, 23)
        Me.tbDefaultFontSize.TabIndex = 93
        Me.tbDefaultFontSize.Tag = "0"
        Me.tbDefaultFontSize.Text = "60"
        Me.tbDefaultFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label17
        '
        Me.Label17.BackColor = System.Drawing.Color.Transparent
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(8, 68)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(212, 20)
        Me.Label17.TabIndex = 92
        Me.Label17.Text = "Default font size for ""Text Display"":"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.AddRange(New System.Windows.Forms.Control() {Me.lblDefaultColorRight, Me.lblDefaultColorLeft, Me.grpDefaultColorsRight, Me.grpDefaultColorsLeft})
        Me.GroupBox2.Location = New System.Drawing.Point(8, 4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(748, 60)
        Me.GroupBox2.TabIndex = 91
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Default team colors at startup:"
        '
        'lblDefaultColorRight
        '
        Me.lblDefaultColorRight.BackColor = System.Drawing.Color.Maroon
        Me.lblDefaultColorRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDefaultColorRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDefaultColorRight.ForeColor = System.Drawing.Color.White
        Me.lblDefaultColorRight.Location = New System.Drawing.Point(388, 20)
        Me.lblDefaultColorRight.Name = "lblDefaultColorRight"
        Me.lblDefaultColorRight.Size = New System.Drawing.Size(100, 28)
        Me.lblDefaultColorRight.TabIndex = 94
        Me.lblDefaultColorRight.Text = "Right Team"
        Me.lblDefaultColorRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDefaultColorLeft
        '
        Me.lblDefaultColorLeft.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(176, Byte))
        Me.lblDefaultColorLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDefaultColorLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDefaultColorLeft.ForeColor = System.Drawing.Color.White
        Me.lblDefaultColorLeft.Location = New System.Drawing.Point(260, 20)
        Me.lblDefaultColorLeft.Name = "lblDefaultColorLeft"
        Me.lblDefaultColorLeft.Size = New System.Drawing.Size(100, 28)
        Me.lblDefaultColorLeft.TabIndex = 93
        Me.lblDefaultColorLeft.Text = "Left Team"
        Me.lblDefaultColorLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpDefaultColorsRight
        '
        Me.grpDefaultColorsRight.BackColor = System.Drawing.Color.Transparent
        Me.grpDefaultColorsRight.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnChooseDefaultTextColorRight, Me.pnlDefaultTextColorRight6, Me.pnlDefaultTextColorRight5, Me.pnlDefaultTextColorRight4, Me.pnlDefaultTextColorRight3, Me.pnlDefaultTextColorRight2, Me.pnlDefaultTextColorRight1})
        Me.grpDefaultColorsRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpDefaultColorsRight.Location = New System.Drawing.Point(508, 12)
        Me.grpDefaultColorsRight.Name = "grpDefaultColorsRight"
        Me.grpDefaultColorsRight.Size = New System.Drawing.Size(228, 40)
        Me.grpDefaultColorsRight.TabIndex = 92
        Me.grpDefaultColorsRight.TabStop = False
        '
        'btnChooseDefaultTextColorRight
        '
        Me.btnChooseDefaultTextColorRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChooseDefaultTextColorRight.Location = New System.Drawing.Point(152, 12)
        Me.btnChooseDefaultTextColorRight.Name = "btnChooseDefaultTextColorRight"
        Me.btnChooseDefaultTextColorRight.Size = New System.Drawing.Size(68, 24)
        Me.btnChooseDefaultTextColorRight.TabIndex = 27
        Me.btnChooseDefaultTextColorRight.Text = "Choose..."
        '
        'pnlDefaultTextColorRight6
        '
        Me.pnlDefaultTextColorRight6.BackColor = System.Drawing.Color.Black
        Me.pnlDefaultTextColorRight6.Location = New System.Drawing.Point(128, 16)
        Me.pnlDefaultTextColorRight6.Name = "pnlDefaultTextColorRight6"
        Me.pnlDefaultTextColorRight6.Size = New System.Drawing.Size(16, 16)
        Me.pnlDefaultTextColorRight6.TabIndex = 26
        Me.pnlDefaultTextColorRight6.TabStop = True
        '
        'pnlDefaultTextColorRight5
        '
        Me.pnlDefaultTextColorRight5.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(64, Byte), CType(0, Byte))
        Me.pnlDefaultTextColorRight5.Location = New System.Drawing.Point(104, 16)
        Me.pnlDefaultTextColorRight5.Name = "pnlDefaultTextColorRight5"
        Me.pnlDefaultTextColorRight5.Size = New System.Drawing.Size(16, 16)
        Me.pnlDefaultTextColorRight5.TabIndex = 25
        Me.pnlDefaultTextColorRight5.TabStop = True
        '
        'pnlDefaultTextColorRight4
        '
        Me.pnlDefaultTextColorRight4.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(0, Byte), CType(64, Byte))
        Me.pnlDefaultTextColorRight4.Location = New System.Drawing.Point(80, 16)
        Me.pnlDefaultTextColorRight4.Name = "pnlDefaultTextColorRight4"
        Me.pnlDefaultTextColorRight4.Size = New System.Drawing.Size(16, 16)
        Me.pnlDefaultTextColorRight4.TabIndex = 24
        Me.pnlDefaultTextColorRight4.TabStop = True
        '
        'pnlDefaultTextColorRight3
        '
        Me.pnlDefaultTextColorRight3.BackColor = System.Drawing.Color.FromArgb(CType(100, Byte), CType(50, Byte), CType(0, Byte))
        Me.pnlDefaultTextColorRight3.Location = New System.Drawing.Point(56, 16)
        Me.pnlDefaultTextColorRight3.Name = "pnlDefaultTextColorRight3"
        Me.pnlDefaultTextColorRight3.Size = New System.Drawing.Size(16, 16)
        Me.pnlDefaultTextColorRight3.TabIndex = 23
        Me.pnlDefaultTextColorRight3.TabStop = True
        '
        'pnlDefaultTextColorRight2
        '
        Me.pnlDefaultTextColorRight2.BackColor = System.Drawing.Color.Maroon
        Me.pnlDefaultTextColorRight2.Location = New System.Drawing.Point(32, 16)
        Me.pnlDefaultTextColorRight2.Name = "pnlDefaultTextColorRight2"
        Me.pnlDefaultTextColorRight2.Size = New System.Drawing.Size(16, 16)
        Me.pnlDefaultTextColorRight2.TabIndex = 22
        Me.pnlDefaultTextColorRight2.TabStop = True
        '
        'pnlDefaultTextColorRight1
        '
        Me.pnlDefaultTextColorRight1.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(176, Byte))
        Me.pnlDefaultTextColorRight1.Location = New System.Drawing.Point(8, 16)
        Me.pnlDefaultTextColorRight1.Name = "pnlDefaultTextColorRight1"
        Me.pnlDefaultTextColorRight1.Size = New System.Drawing.Size(16, 16)
        Me.pnlDefaultTextColorRight1.TabIndex = 21
        Me.pnlDefaultTextColorRight1.TabStop = True
        '
        'grpDefaultColorsLeft
        '
        Me.grpDefaultColorsLeft.BackColor = System.Drawing.Color.Transparent
        Me.grpDefaultColorsLeft.Controls.AddRange(New System.Windows.Forms.Control() {Me.btnChooseDefaultTextColorLeft, Me.pnlDefaultTextColorLeft6, Me.pnlDefaultTextColorLeft5, Me.pnlDefaultTextColorLeft4, Me.pnlDefaultTextColorLeft3, Me.pnlDefaultTextColorLeft2, Me.pnlDefaultTextColorLeft1})
        Me.grpDefaultColorsLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpDefaultColorsLeft.Location = New System.Drawing.Point(12, 12)
        Me.grpDefaultColorsLeft.Name = "grpDefaultColorsLeft"
        Me.grpDefaultColorsLeft.Size = New System.Drawing.Size(228, 40)
        Me.grpDefaultColorsLeft.TabIndex = 91
        Me.grpDefaultColorsLeft.TabStop = False
        '
        'btnChooseDefaultTextColorLeft
        '
        Me.btnChooseDefaultTextColorLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChooseDefaultTextColorLeft.Location = New System.Drawing.Point(152, 12)
        Me.btnChooseDefaultTextColorLeft.Name = "btnChooseDefaultTextColorLeft"
        Me.btnChooseDefaultTextColorLeft.Size = New System.Drawing.Size(68, 24)
        Me.btnChooseDefaultTextColorLeft.TabIndex = 27
        Me.btnChooseDefaultTextColorLeft.Text = "Choose..."
        '
        'pnlDefaultTextColorLeft6
        '
        Me.pnlDefaultTextColorLeft6.BackColor = System.Drawing.Color.Black
        Me.pnlDefaultTextColorLeft6.Location = New System.Drawing.Point(128, 16)
        Me.pnlDefaultTextColorLeft6.Name = "pnlDefaultTextColorLeft6"
        Me.pnlDefaultTextColorLeft6.Size = New System.Drawing.Size(16, 16)
        Me.pnlDefaultTextColorLeft6.TabIndex = 26
        Me.pnlDefaultTextColorLeft6.TabStop = True
        '
        'pnlDefaultTextColorLeft5
        '
        Me.pnlDefaultTextColorLeft5.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(64, Byte), CType(0, Byte))
        Me.pnlDefaultTextColorLeft5.Location = New System.Drawing.Point(104, 16)
        Me.pnlDefaultTextColorLeft5.Name = "pnlDefaultTextColorLeft5"
        Me.pnlDefaultTextColorLeft5.Size = New System.Drawing.Size(16, 16)
        Me.pnlDefaultTextColorLeft5.TabIndex = 25
        Me.pnlDefaultTextColorLeft5.TabStop = True
        '
        'pnlDefaultTextColorLeft4
        '
        Me.pnlDefaultTextColorLeft4.BackColor = System.Drawing.Color.FromArgb(CType(64, Byte), CType(0, Byte), CType(64, Byte))
        Me.pnlDefaultTextColorLeft4.Location = New System.Drawing.Point(80, 16)
        Me.pnlDefaultTextColorLeft4.Name = "pnlDefaultTextColorLeft4"
        Me.pnlDefaultTextColorLeft4.Size = New System.Drawing.Size(16, 16)
        Me.pnlDefaultTextColorLeft4.TabIndex = 24
        Me.pnlDefaultTextColorLeft4.TabStop = True
        '
        'pnlDefaultTextColorLeft3
        '
        Me.pnlDefaultTextColorLeft3.BackColor = System.Drawing.Color.FromArgb(CType(100, Byte), CType(50, Byte), CType(0, Byte))
        Me.pnlDefaultTextColorLeft3.Location = New System.Drawing.Point(56, 16)
        Me.pnlDefaultTextColorLeft3.Name = "pnlDefaultTextColorLeft3"
        Me.pnlDefaultTextColorLeft3.Size = New System.Drawing.Size(16, 16)
        Me.pnlDefaultTextColorLeft3.TabIndex = 23
        Me.pnlDefaultTextColorLeft3.TabStop = True
        '
        'pnlDefaultTextColorLeft2
        '
        Me.pnlDefaultTextColorLeft2.BackColor = System.Drawing.Color.Maroon
        Me.pnlDefaultTextColorLeft2.Location = New System.Drawing.Point(32, 16)
        Me.pnlDefaultTextColorLeft2.Name = "pnlDefaultTextColorLeft2"
        Me.pnlDefaultTextColorLeft2.Size = New System.Drawing.Size(16, 16)
        Me.pnlDefaultTextColorLeft2.TabIndex = 22
        Me.pnlDefaultTextColorLeft2.TabStop = True
        '
        'pnlDefaultTextColorLeft1
        '
        Me.pnlDefaultTextColorLeft1.BackColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(176, Byte))
        Me.pnlDefaultTextColorLeft1.Location = New System.Drawing.Point(8, 16)
        Me.pnlDefaultTextColorLeft1.Name = "pnlDefaultTextColorLeft1"
        Me.pnlDefaultTextColorLeft1.Size = New System.Drawing.Size(16, 16)
        Me.pnlDefaultTextColorLeft1.TabIndex = 21
        Me.pnlDefaultTextColorLeft1.TabStop = True
        '
        'btnSavePrefs
        '
        Me.btnSavePrefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSavePrefs.Location = New System.Drawing.Point(296, 288)
        Me.btnSavePrefs.Name = "btnSavePrefs"
        Me.btnSavePrefs.Size = New System.Drawing.Size(172, 28)
        Me.btnSavePrefs.TabIndex = 87
        Me.btnSavePrefs.Text = "SAVE PREFS"
        '
        'btnRevertPrefs
        '
        Me.btnRevertPrefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRevertPrefs.Location = New System.Drawing.Point(584, 288)
        Me.btnRevertPrefs.Name = "btnRevertPrefs"
        Me.btnRevertPrefs.Size = New System.Drawing.Size(172, 28)
        Me.btnRevertPrefs.TabIndex = 86
        Me.btnRevertPrefs.Text = "UNDO CHANGES"
        '
        'cbPlaySlidesAtStart
        '
        Me.cbPlaySlidesAtStart.BackColor = System.Drawing.Color.Transparent
        Me.cbPlaySlidesAtStart.Enabled = False
        Me.cbPlaySlidesAtStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbPlaySlidesAtStart.Location = New System.Drawing.Point(40, 252)
        Me.cbPlaySlidesAtStart.Name = "cbPlaySlidesAtStart"
        Me.cbPlaySlidesAtStart.Size = New System.Drawing.Size(228, 24)
        Me.cbPlaySlidesAtStart.TabIndex = 85
        Me.cbPlaySlidesAtStart.Text = "Play this Slideshow when program starts"
        '
        'Label16
        '
        Me.Label16.BackColor = System.Drawing.Color.Transparent
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(8, 196)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(212, 20)
        Me.Label16.TabIndex = 83
        Me.Label16.Text = "Default seconds between slides:"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudDefaultSlideDelay
        '
        Me.nudDefaultSlideDelay.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte), True)
        Me.nudDefaultSlideDelay.Location = New System.Drawing.Point(224, 192)
        Me.nudDefaultSlideDelay.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
        Me.nudDefaultSlideDelay.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudDefaultSlideDelay.Name = "nudDefaultSlideDelay"
        Me.nudDefaultSlideDelay.Size = New System.Drawing.Size(56, 26)
        Me.nudDefaultSlideDelay.TabIndex = 84
        Me.nudDefaultSlideDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.nudDefaultSlideDelay.Value = New Decimal(New Integer() {15, 0, 0, 0})
        '
        'tbDefaultSlideShow
        '
        Me.tbDefaultSlideShow.BackColor = System.Drawing.SystemColors.ControlDark
        Me.tbDefaultSlideShow.Enabled = False
        Me.tbDefaultSlideShow.Location = New System.Drawing.Point(224, 224)
        Me.tbDefaultSlideShow.Name = "tbDefaultSlideShow"
        Me.tbDefaultSlideShow.ReadOnly = True
        Me.tbDefaultSlideShow.Size = New System.Drawing.Size(464, 20)
        Me.tbDefaultSlideShow.TabIndex = 52
        Me.tbDefaultSlideShow.Text = ""
        '
        'btnChooseDefaultSlideShow
        '
        Me.btnChooseDefaultSlideShow.Enabled = False
        Me.btnChooseDefaultSlideShow.Location = New System.Drawing.Point(692, 224)
        Me.btnChooseDefaultSlideShow.Name = "btnChooseDefaultSlideShow"
        Me.btnChooseDefaultSlideShow.Size = New System.Drawing.Size(64, 23)
        Me.btnChooseDefaultSlideShow.TabIndex = 51
        Me.btnChooseDefaultSlideShow.Tag = "0"
        Me.btnChooseDefaultSlideShow.Text = "Select..."
        '
        'tbDefaultHBFile
        '
        Me.tbDefaultHBFile.BackColor = System.Drawing.SystemColors.ControlDark
        Me.tbDefaultHBFile.Enabled = False
        Me.tbDefaultHBFile.Location = New System.Drawing.Point(224, 148)
        Me.tbDefaultHBFile.Name = "tbDefaultHBFile"
        Me.tbDefaultHBFile.ReadOnly = True
        Me.tbDefaultHBFile.Size = New System.Drawing.Size(464, 20)
        Me.tbDefaultHBFile.TabIndex = 50
        Me.tbDefaultHBFile.Text = ""
        '
        'btnChooseDefaultHB
        '
        Me.btnChooseDefaultHB.Enabled = False
        Me.btnChooseDefaultHB.Location = New System.Drawing.Point(692, 148)
        Me.btnChooseDefaultHB.Name = "btnChooseDefaultHB"
        Me.btnChooseDefaultHB.Size = New System.Drawing.Size(64, 23)
        Me.btnChooseDefaultHB.TabIndex = 49
        Me.btnChooseDefaultHB.Tag = "0"
        Me.btnChooseDefaultHB.Text = "Select..."
        '
        'tbDefaultImageFile
        '
        Me.tbDefaultImageFile.BackColor = System.Drawing.SystemColors.ControlDark
        Me.tbDefaultImageFile.Enabled = False
        Me.tbDefaultImageFile.Location = New System.Drawing.Point(224, 124)
        Me.tbDefaultImageFile.Name = "tbDefaultImageFile"
        Me.tbDefaultImageFile.ReadOnly = True
        Me.tbDefaultImageFile.Size = New System.Drawing.Size(464, 20)
        Me.tbDefaultImageFile.TabIndex = 48
        Me.tbDefaultImageFile.Text = ""
        '
        'btnChooseDefaultImage
        '
        Me.btnChooseDefaultImage.Enabled = False
        Me.btnChooseDefaultImage.Location = New System.Drawing.Point(692, 124)
        Me.btnChooseDefaultImage.Name = "btnChooseDefaultImage"
        Me.btnChooseDefaultImage.Size = New System.Drawing.Size(64, 23)
        Me.btnChooseDefaultImage.TabIndex = 47
        Me.btnChooseDefaultImage.Tag = "0"
        Me.btnChooseDefaultImage.Text = "Select..."
        '
        'cbDisplayDefaultImage
        '
        Me.cbDisplayDefaultImage.BackColor = System.Drawing.Color.Transparent
        Me.cbDisplayDefaultImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbDisplayDefaultImage.Location = New System.Drawing.Point(8, 120)
        Me.cbDisplayDefaultImage.Name = "cbDisplayDefaultImage"
        Me.cbDisplayDefaultImage.Size = New System.Drawing.Size(216, 24)
        Me.cbDisplayDefaultImage.TabIndex = 2
        Me.cbDisplayDefaultImage.Text = "Display image when program starts:"
        '
        'cbLoadDefaultSlides
        '
        Me.cbLoadDefaultSlides.BackColor = System.Drawing.Color.Transparent
        Me.cbLoadDefaultSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbLoadDefaultSlides.Location = New System.Drawing.Point(8, 224)
        Me.cbLoadDefaultSlides.Name = "cbLoadDefaultSlides"
        Me.cbLoadDefaultSlides.Size = New System.Drawing.Size(216, 24)
        Me.cbLoadDefaultSlides.TabIndex = 1
        Me.cbLoadDefaultSlides.Text = "Load a SlideShow file as default:"
        '
        'cbLoadDefaultHB
        '
        Me.cbLoadDefaultHB.BackColor = System.Drawing.Color.Transparent
        Me.cbLoadDefaultHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbLoadDefaultHB.Location = New System.Drawing.Point(8, 144)
        Me.cbLoadDefaultHB.Name = "cbLoadDefaultHB"
        Me.cbLoadDefaultHB.Size = New System.Drawing.Size(216, 24)
        Me.cbLoadDefaultHB.TabIndex = 0
        Me.cbLoadDefaultHB.Text = "Load a HotButtons file at startup:"
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
        Me.TextBox2.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(64, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(8, 120)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(752, 208)
        Me.TextBox2.TabIndex = 91
        Me.TextBox2.TabStop = False
        Me.TextBox2.Text = "This program may only be used with explicit permission of the author, Bill Cernan" & _
        "sky." & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Special thanks to:" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Jay and MaryAnn Rambo, Andrew Berkowitz, Patrick S" & _
        "hort, Eric Wood and ComedySportz-Portland;" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Wade Minter, whose ""Mr. Voice"" sof" & _
        "tware is an inspiration;" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Portland Brewery, for MacTarnahan's Scottish Ale, wh" & _
        "ich refreshed me during intense bug fighting;" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "WARNING: Do not press CTRL-SHIF" & _
        "T-B !"
        Me.TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Comic Sans MS", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(0, Byte), CType(64, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(8, 8)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(752, 104)
        Me.TextBox1.TabIndex = 90
        Me.TextBox1.TabStop = False
        Me.TextBox1.Text = "JANIS Single Display" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "Version 2.0 Beta 3  Released Feb. 15, 2006" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "by Bill Cernans" & _
        "ky ( bill@easybeing.com )" & Microsoft.VisualBasic.ChrW(13) & Microsoft.VisualBasic.ChrW(10) & "© 2004-2006 Easy Being Productions"
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'SlideTimer
        '
        Me.SlideTimer.Interval = 8000
        '
        'btnHot1
        '
        Me.btnHot1.BackColor = System.Drawing.Color.Gray
        Me.btnHot1.ForeColor = System.Drawing.Color.White
        Me.btnHot1.Location = New System.Drawing.Point(8, 188)
        Me.btnHot1.Name = "btnHot1"
        Me.btnHot1.Size = New System.Drawing.Size(68, 24)
        Me.btnHot1.TabIndex = 31
        Me.btnHot1.Text = "Hot 1"
        '
        'btnHot2
        '
        Me.btnHot2.BackColor = System.Drawing.Color.Gray
        Me.btnHot2.ForeColor = System.Drawing.Color.White
        Me.btnHot2.Location = New System.Drawing.Point(84, 188)
        Me.btnHot2.Name = "btnHot2"
        Me.btnHot2.Size = New System.Drawing.Size(68, 24)
        Me.btnHot2.TabIndex = 32
        Me.btnHot2.Text = "Hot 2"
        '
        'btnHot3
        '
        Me.btnHot3.BackColor = System.Drawing.Color.Gray
        Me.btnHot3.ForeColor = System.Drawing.Color.White
        Me.btnHot3.Location = New System.Drawing.Point(160, 188)
        Me.btnHot3.Name = "btnHot3"
        Me.btnHot3.Size = New System.Drawing.Size(68, 24)
        Me.btnHot3.TabIndex = 33
        Me.btnHot3.Text = "Hot 3"
        '
        'btnHot4
        '
        Me.btnHot4.BackColor = System.Drawing.Color.Gray
        Me.btnHot4.ForeColor = System.Drawing.Color.White
        Me.btnHot4.Location = New System.Drawing.Point(236, 188)
        Me.btnHot4.Name = "btnHot4"
        Me.btnHot4.Size = New System.Drawing.Size(68, 24)
        Me.btnHot4.TabIndex = 34
        Me.btnHot4.Text = "Hot 4"
        '
        'btnHot5
        '
        Me.btnHot5.BackColor = System.Drawing.Color.Gray
        Me.btnHot5.ForeColor = System.Drawing.Color.White
        Me.btnHot5.Location = New System.Drawing.Point(312, 188)
        Me.btnHot5.Name = "btnHot5"
        Me.btnHot5.Size = New System.Drawing.Size(68, 24)
        Me.btnHot5.TabIndex = 35
        Me.btnHot5.Text = "Hot 5"
        '
        'btnHot6
        '
        Me.btnHot6.BackColor = System.Drawing.Color.Gray
        Me.btnHot6.ForeColor = System.Drawing.Color.White
        Me.btnHot6.Location = New System.Drawing.Point(392, 188)
        Me.btnHot6.Name = "btnHot6"
        Me.btnHot6.Size = New System.Drawing.Size(68, 24)
        Me.btnHot6.TabIndex = 36
        Me.btnHot6.Text = "Hot 6"
        '
        'btnHot7
        '
        Me.btnHot7.BackColor = System.Drawing.Color.Gray
        Me.btnHot7.ForeColor = System.Drawing.Color.White
        Me.btnHot7.Location = New System.Drawing.Point(468, 188)
        Me.btnHot7.Name = "btnHot7"
        Me.btnHot7.Size = New System.Drawing.Size(68, 24)
        Me.btnHot7.TabIndex = 37
        Me.btnHot7.Text = "Hot 7"
        '
        'btnHot8
        '
        Me.btnHot8.BackColor = System.Drawing.Color.Gray
        Me.btnHot8.ForeColor = System.Drawing.Color.White
        Me.btnHot8.Location = New System.Drawing.Point(544, 188)
        Me.btnHot8.Name = "btnHot8"
        Me.btnHot8.Size = New System.Drawing.Size(68, 24)
        Me.btnHot8.TabIndex = 38
        Me.btnHot8.Text = "Hot 8"
        '
        'btnHot9
        '
        Me.btnHot9.BackColor = System.Drawing.Color.Gray
        Me.btnHot9.ForeColor = System.Drawing.Color.White
        Me.btnHot9.Location = New System.Drawing.Point(620, 188)
        Me.btnHot9.Name = "btnHot9"
        Me.btnHot9.Size = New System.Drawing.Size(68, 24)
        Me.btnHot9.TabIndex = 39
        Me.btnHot9.Text = "Hot 9"
        '
        'btnHot10
        '
        Me.btnHot10.BackColor = System.Drawing.Color.Gray
        Me.btnHot10.ForeColor = System.Drawing.Color.White
        Me.btnHot10.Location = New System.Drawing.Point(696, 188)
        Me.btnHot10.Name = "btnHot10"
        Me.btnHot10.Size = New System.Drawing.Size(68, 24)
        Me.btnHot10.TabIndex = 40
        Me.btnHot10.Text = "Hot 10"
        '
        'pnlPicBackLeft
        '
        Me.pnlPicBackLeft.BackColor = System.Drawing.Color.Black
        Me.pnlPicBackLeft.Controls.AddRange(New System.Windows.Forms.Control() {Me.picLeft})
        Me.pnlPicBackLeft.Location = New System.Drawing.Point(28, 60)
        Me.pnlPicBackLeft.Name = "pnlPicBackLeft"
        Me.pnlPicBackLeft.Size = New System.Drawing.Size(160, 120)
        Me.pnlPicBackLeft.TabIndex = 41
        '
        'CountdownTimer
        '
        Me.CountdownTimer.Interval = 10
        '
        'cbExpandPicLeft
        '
        Me.cbExpandPicLeft.Location = New System.Drawing.Point(192, 64)
        Me.cbExpandPicLeft.Name = "cbExpandPicLeft"
        Me.cbExpandPicLeft.Size = New System.Drawing.Size(64, 16)
        Me.cbExpandPicLeft.TabIndex = 43
        Me.cbExpandPicLeft.Text = "Expand"
        '
        'fmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(772, 565)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.cbExpandPicLeft, Me.pnlPicBackLeft, Me.btnHot10, Me.btnHot9, Me.btnHot8, Me.btnHot7, Me.btnHot6, Me.btnHot5, Me.btnHot4, Me.btnHot3, Me.btnHot2, Me.btnHot1, Me.btnBlackout, Me.TabControl1, Me.btnRightScoreColor, Me.btnLeftScoreColor, Me.btnPictureLeft, Me.btnScoreBoth, Me.Label4, Me.Label3, Me.tbRightScore, Me.tbLeftScore, Me.Label2, Me.Label1, Me.tbRightTeam, Me.tbLeftTeam})
        Me.ForeColor = System.Drawing.SystemColors.WindowText
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
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
        Me.tpHotButtons.ResumeLayout(False)
        Me.gbHB.ResumeLayout(False)
        Me.tpSlides.ResumeLayout(False)
        CType(Me.nudDelay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpPrefs.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.grpDefaultColorsRight.ResumeLayout(False)
        Me.grpDefaultColorsLeft.ResumeLayout(False)
        CType(Me.nudDefaultSlideDelay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpAbout.ResumeLayout(False)
        Me.pnlPicBackLeft.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Left = SystemInformation.PrimaryMonitorSize.Width - Me.Width
        Me.tbCurrentThing.Visible = False
        Me.tbSubstitutions.Visible = False
        Me.btnShowThingLeft.Visible = False

        Me.LS = New fmScreen()

        LoadPrefsFromFile(PREFS_FILE)
        ApplyPrefs()

        '* Here's the wacky way you change font sizes in VB.NET. Piece of crap.
        Me.tbLeftText.Font = New Font(Me.tbLeftText.Font.Name, CSng(Val(Me.tbLeftFontSize.Text) / DisplayToEntryFontRatio), Me.tbLeftText.Font.Style)
        Me.tbRightText.Font = New Font(Me.tbRightText.Font.Name, CSng(Val(Me.tbRightFontSize.Text) / DisplayToEntryFontRatio), Me.tbRightText.Font.Style)

        Me.SetMonitorDisplayMode()

        If (Not Me.cbDisplayDefaultImage.Checked) Then
            DisplayTextScreen(Me.LS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, CSng(Me.tbLeftFontSize.Text) * Me.DisplayFontRatio)
        End If

        Me.LS.Show()

        '* If the default support dirs aren't there, create them
        Dim MyDir As String = Dir(ROOT_SUPPORT_DIR, FileAttribute.Directory)
        If MyDir = "" Then MkDir(ROOT_SUPPORT_DIR)
        MyDir = Dir(ROOT_SUPPORT_DIR + DEFAULT_SLIDESHOW_DIR, FileAttribute.Directory)
        If MyDir = "" Then MkDir(ROOT_SUPPORT_DIR + DEFAULT_SLIDESHOW_DIR)
        MyDir = Dir(ROOT_SUPPORT_DIR + DEFAULT_HOTBUTTON_DIR, FileAttribute.Directory)
        If MyDir = "" Then MkDir(ROOT_SUPPORT_DIR + DEFAULT_HOTBUTTON_DIR)

        Me.HotButtonsChanged = False
        Me.cbHBActive.Checked = True    '* default to "can see"

        '* The FolderTree control can't be selected programmatically
        '* (what the hell was the guy thinking), but we can expand it down to the
        '* directory we want to see at the start. However, since we can't select it,
        '* the FileListBox's path will be empty.
        '* Me.FTreeAutoExpand_C("Program Files\JANIS")
        Me.FTreeAutoExpand_C(Me.FolderTree1, Me.tbDefaultImageDir.Text.Substring(3)) '* skip the c:\ part
        Me.FileListBox1.Path = ""
    End Sub

    Private Sub Form1_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Dim Ans As DialogResult
        If Me.PrefsChanged() Then
            Ans = MessageBox.Show("Modifications to preferences have not been saved. Save them before closing?", "Preferences Have Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3, MessageBoxOptions.DefaultDesktopOnly)
            If Ans = DialogResult.Cancel Then
                e.Cancel = True
                Return
            ElseIf Ans = DialogResult.Yes Then
                Me.SavePrefsToFile(PREFS_FILE)
            End If
        End If
    End Sub

    Private Sub btnScoreBoth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScoreBoth.Click
        DisplayScores()  ' Clears the local pic images itself.
    End Sub
    Private Sub btnLeftScoreColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeftScoreColor.Click
        Dim newcolor As Color = PickColor(sender.Left, sender.Top, tbLeftScore.BackColor)
        SetTeamColor("Left", newcolor)
    End Sub
    Private Sub btnRightScoreColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRightScoreColor.Click
        Dim newcolor As Color = PickColor(sender.Left - 100, sender.Top, tbRightScore.BackColor)
        SetTeamColor("Right", newcolor)
    End Sub
    Private Sub SetTeamColor(ByVal side As String, ByVal newcolor As Color)
        If side = "Left" Then
            Me.tbLeftScore.BackColor = newcolor
            Me.radioThingColorLeft.BackColor = newcolor
        Else
            Me.tbRightScore.BackColor = newcolor
            Me.radioThingColorRight.BackColor = newcolor
        End If
        If (Me.radioThingColorLeft.Checked And (side = "Left")) Or (Me.radioThingColorRight.Checked And (side = "Right")) Then
            Me.clbThings.BackColor = newcolor
            Me.tbSubstitutions.BackColor = newcolor
            Me.tbCurrentThing.BackColor = newcolor
        End If
    End Sub

    Private Sub btnClearTextLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearTextLeft.Click
        Me.tbLeftText.Text = ""
        Me.tbLeftText.Focus()
    End Sub
    Private Sub btnClearTextRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearTextRight.Click
        Me.tbRightText.Text = ""
        Me.tbRightText.Focus()
    End Sub
    Private Sub btnClearTextBoth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearTextBoth.Click
        btnClearTextLeft_Click(sender, e)
        btnClearTextRight_Click(sender, e)
        Me.tbLeftText.Focus()
    End Sub
    Private Sub btnShowLeftText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowLeftText.Click
        DisplayTextScreen(Me.LS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, CSng(Me.tbLeftFontSize.Text) * Me.DisplayFontRatio)
    End Sub
    Private Sub btnShowRightText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowRightText.Click
        DisplayTextScreen(Me.LS, Me.tbRightText.Text, Me.tbRightText.BackColor, CSng(Me.tbRightFontSize.Text) * Me.DisplayFontRatio)
    End Sub

    Private Sub btnDocLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDocLoadLeft.Click, btnDocLoadRight.Click, btnDocLoadBoth.Click
        Dim Doc As String = LoadDoc()
        If Doc <> "" Then
            If sender.Tag <> "Left" Then  '* This will work for Right and Both
                Me.tbRightText.Text = Doc
            End If
            If sender.Tag <> "Right" Then '* This will work for Left and Both
                Me.tbLeftText.Text = Doc
            End If
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

    Private Sub pnlTextColorLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlTextColorLeft1.Click, pnlTextColorLeft2.Click, pnlTextColorLeft3.Click, pnlTextColorLeft4.Click, pnlTextColorLeft5.Click, pnlTextColorLeft6.Click
        Me.tbLeftText.BackColor = sender.BackColor
    End Sub
    Private Sub pnlTextColorRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlTextColorRight1.Click, pnlTextColorRight2.Click, pnlTextColorRight3.Click, pnlTextColorRight4.Click, pnlTextColorRight5.Click, pnlTextColorRight6.Click
        Me.tbRightText.BackColor = sender.BackColor
    End Sub
    Private Sub btnChooseTextColorLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseTextColorLeft.Click
        tbLeftText.BackColor = PickColor(50, 300, tbLeftText.BackColor)
    End Sub
    Private Sub btnChooseTextColorRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseTextColorRight.Click
        tbRightText.BackColor = PickColor(450, 300, tbRightText.BackColor)
        'tbRightText.BackColor = ChooseColor(tbRightText.BackColor)
    End Sub

    Private Sub btnBlackout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlackout.Click
        '* First, stop the slideshow if it's running.
        StopSlideShow()

        '* Shut them down, Artoo! Shut them all down!
        Me.LS.BackColor = System.Drawing.Color.Black
        Me.LS.picGraphic.Visible = False
        Me.picLeft.Image = Nothing
        Me.LS.lblMsg.Visible = False
        Me.LS.lblScoreLeft.Visible = False
        Me.LS.lblScoreRight.Visible = False
        Me.LS.lblTeamNameLeft.Visible = False
        Me.LS.lblTeamNameRight.Visible = False

    End Sub

    Private Sub tbFontSize_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbLeftFontSize.KeyUp, tbRightFontSize.KeyUp, tbDefaultFontSize.KeyUp
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
            Me.tbLeftText.Font = New Font(Me.tbLeftText.Font.Name, (fsiz / DisplayToEntryFontRatio), Me.tbLeftText.Font.Style)
        ElseIf sender.Name = "tbRightFontSize" Then
            Me.tbRightText.Font = New Font(Me.tbRightText.Font.Name, (fsiz / DisplayToEntryFontRatio), Me.tbRightText.Font.Style)
        End If '* don't do anything for tbDefaultFontSize - that's a startup setting
    End Sub
    Private Sub tbFontSize_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbLeftFontSize.KeyPress, tbRightFontSize.KeyPress, tbDefaultFontSize.KeyPress
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

    Private Sub SetMonitorDisplayMode()
        If SystemInformation.MonitorCount = 1 Then        '* We're in single monitor/test mode
            Me.TestMode = True
            Me.DisplayModeAdjustment = 25.0  '* = 5w x 5h
            Me.LS.Left = Me.Left
            Me.LS.Top = Me.Height
            Me.LS.Height = Me.LS.Height / 5
            Me.LS.Width = Me.LS.Width / 5
            Me.LS.lblScoreLeft.Left = Me.LS.lblScoreLeft.Left / 5
            Me.LS.lblScoreLeft.Top = Me.LS.lblScoreLeft.Top / 5
            Me.LS.lblScoreLeft.Height = Me.LS.lblScoreLeft.Height / 5
            Me.LS.lblScoreLeft.Width = Me.LS.lblScoreLeft.Width / 5
            Me.LS.lblScoreLeft.Font = New Font(Me.LS.lblScoreLeft.Font.Name, CSng(Val(Me.LS.lblScoreLeft.Font.Size) / 5), Me.LS.lblScoreLeft.Font.Style)
            Me.LS.lblScoreRight.Left = Me.LS.lblScoreRight.Left / 5
            Me.LS.lblScoreRight.Top = Me.LS.lblScoreRight.Top / 5
            Me.LS.lblScoreRight.Height = Me.LS.lblScoreRight.Height / 5
            Me.LS.lblScoreRight.Width = Me.LS.lblScoreRight.Width / 5
            Me.LS.lblScoreRight.Font = New Font(Me.LS.lblScoreRight.Font.Name, CSng(Val(Me.LS.lblScoreRight.Font.Size) / 5), Me.LS.lblScoreRight.Font.Style)
            Me.LS.lblTeamNameLeft.Left = Me.LS.lblTeamNameLeft.Left / 5
            Me.LS.lblTeamNameLeft.Top = Me.LS.lblTeamNameLeft.Top / 5
            Me.LS.lblTeamNameLeft.Height = Me.LS.lblTeamNameLeft.Height / 5
            Me.LS.lblTeamNameLeft.Width = Me.LS.lblTeamNameLeft.Width / 5
            Me.LS.lblTeamNameLeft.Font = New Font(Me.LS.lblTeamNameLeft.Font.Name, CSng(Val(Me.LS.lblTeamNameLeft.Font.Size) / 5), Me.LS.lblTeamNameLeft.Font.Style)
            Me.LS.lblTeamNameRight.Left = Me.LS.lblTeamNameRight.Left / 5
            Me.LS.lblTeamNameRight.Top = Me.LS.lblTeamNameRight.Top / 5
            Me.LS.lblTeamNameRight.Height = Me.LS.lblTeamNameRight.Height / 5
            Me.LS.lblTeamNameRight.Width = Me.LS.lblTeamNameRight.Width / 5
            Me.LS.lblTeamNameRight.Font = New Font(Me.LS.lblTeamNameRight.Font.Name, CSng(Val(Me.LS.lblTeamNameRight.Font.Size) / 5), Me.LS.lblTeamNameRight.Font.Style)
            Me.LS.lblMsg.Left = Me.LS.lblMsg.Left / 5
            Me.LS.lblMsg.Top = Me.LS.lblMsg.Top / 5
            Me.LS.lblMsg.Height = Me.LS.lblMsg.Height / 5
            Me.LS.lblMsg.Width = Me.LS.lblMsg.Width / 5
            Me.LS.lblMsg.Font = New Font(Me.LS.lblMsg.Font.Name, CSng(Val(Me.LS.lblMsg.Font.Size) / 5), Me.LS.lblMsg.Font.Style)
            Me.LS.picGraphic.Left = Me.LS.picGraphic.Left / 5
            Me.LS.picGraphic.Top = Me.LS.picGraphic.Top / 5
            Me.LS.picGraphic.Height = Me.LS.picGraphic.Height / 5
            Me.LS.picGraphic.Width = Me.LS.picGraphic.Width / 5

            Me.tbRightText.Text = "TEST MODE"
            Me.Text = Me.Text + "   **** TEST MODE ****"
        Else
            Me.LS.Left = SystemInformation.PrimaryMonitorSize.Width
            Me.tbRightText.Text = "Arena Mode (Single)"
            Me.Text = Me.Text + " - Arena Mode (Single)"
        End If
        Me.tbRightText.Text = Me.tbRightText.Text + EOL + EOL + SystemInformation.MonitorCount.ToString + " monitors found"

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
        Me.picLeft.Image = Nothing

        If Me.TestMode Then fontsize = fontsize * 5
        Scr.lblScoreLeft.Visible = False
        Scr.lblTeamNameLeft.Visible = False
        Scr.lblScoreRight.Visible = False
        Scr.lblTeamNameRight.Visible = False
        Scr.picGraphic.Visible = False
        Scr.lblMsg.Font = New Font(Scr.lblMsg.Font.Name, CSng(DisplayToEntryFontRatio * fontsize / Me.DisplayModeAdjustment), Scr.lblMsg.Font.Style)
        Scr.lblMsg.Visible = True
        Scr.lblMsg.Text = translation
        Scr.BackColor = hue
    End Sub

    Private Sub DisplayScore(ByVal Scr As fmScreen, ByVal TeamLeft As String, ByVal ScoreLeft As String, ByVal BackColorLeft As System.Drawing.Color, ByVal TeamRight As String, ByVal ScoreRight As String, ByVal BackColorRight As System.Drawing.Color)
        '* First, stop the slideshow if it's running.
        StopSlideShow()

        Scr.lblTeamNameLeft.BackColor = BackColorLeft
        Scr.lblScoreLeft.BackColor = BackColorLeft
        Scr.lblTeamNameRight.BackColor = BackColorRight
        Scr.lblScoreRight.BackColor = BackColorRight
        Scr.lblMsg.Visible = False
        Scr.picGraphic.Visible = False
        Scr.lblScoreLeft.Text = ScoreLeft
        Scr.lblScoreRight.Text = ScoreRight
        Scr.lblTeamNameLeft.Text = TeamLeft
        Scr.lblTeamNameRight.Text = TeamRight
        Scr.lblScoreLeft.Visible = True
        Scr.lblScoreRight.Visible = True
        Scr.lblTeamNameLeft.Visible = True
        Scr.lblTeamNameRight.Visible = True
    End Sub

    Private Sub DisplayImage(ByVal Scr As fmScreen, ByVal Img As Image, ByVal Expand As Boolean)
        Dim monitor_width_height_ratio As Single = CSng(Scr.Width) / CSng(Scr.Height)
        Dim image_width_height_ratio As Single = CSng(Img.Width) / CSng(Img.Height)
        Dim ratio_compare As Single = image_width_height_ratio / monitor_width_height_ratio
        Dim new_height, new_width As Integer

        Scr.BackColor = System.Drawing.Color.Black
        Scr.lblScoreLeft.Visible = False
        Scr.lblTeamNameLeft.Visible = False
        Scr.lblScoreRight.Visible = False
        Scr.lblTeamNameRight.Visible = False
        Scr.lblMsg.Visible = False

        If Expand Or (ratio_compare < 1.15 And ratio_compare > 0.85) Then     '* close enough to equal
            new_width = Scr.Width
            new_height = Scr.Height
        ElseIf ratio_compare > 1 Then   '* wider/shorter
            new_width = Scr.Width
            new_height = CInt(new_width / image_width_height_ratio)
        Else    '* narrower/taller
            new_height = Scr.Height
            new_width = CInt(new_height * image_width_height_ratio)
        End If
        If new_width <> Scr.picGraphic.Width Then
            '* put the picture in the middle
            Scr.picGraphic.Left = (Scr.Width - new_width) / 2
            Scr.picGraphic.Width = new_width
        End If
        If new_height <> Scr.picGraphic.Height Then
            '* put the picture in the middle
            Scr.picGraphic.Top = (Scr.Height - new_height) / 2
            Scr.picGraphic.Height = new_height
        End If

        Scr.picGraphic.Image = Img
        Scr.picGraphic.Visible = True
    End Sub

    Private Sub PreviewImage(ByVal picture As PictureBox, ByVal Img As Image, ByVal Expand As Boolean)
        Dim backpanel As Panel = Me.pnlPicBackLeft
        Dim panel_width_height_ratio As Single = CSng(backpanel.Width) / CSng(backpanel.Height)
        Dim image_width_height_ratio As Single = CSng(Img.Width) / CSng(Img.Height)
        Dim ratio_compare As Single = image_width_height_ratio / panel_width_height_ratio
        Dim new_height, new_width As Integer

        'picture.Visible = False
        backpanel.BackColor = System.Drawing.Color.Black
        If Expand Or (ratio_compare < 1.15 And ratio_compare > 0.85) Then     '* close enough to equal
            new_width = backpanel.Width
            new_height = backpanel.Height
        ElseIf ratio_compare > 1 Then   '* wider/shorter
            new_width = backpanel.Width
            new_height = CInt(new_width / image_width_height_ratio)
        Else    '* narrower/taller
            new_height = backpanel.Height
            new_width = CInt(new_height * image_width_height_ratio)
        End If
        If new_width <> picture.Width Then
            '* put the picture in the middle
            picture.Left = (backpanel.Width - new_width) / 2
            picture.Width = new_width
        End If
        If new_height <> picture.Height Then
            '* put the picture in the middle
            picture.Top = (backpanel.Height - new_height) / 2
            picture.Height = new_height
        End If

        picture.Image = Img
        picture.Visible = True
    End Sub


    Private Sub DisplayBothImages(ByVal fnam As String)
        Dim img As Image
        Try
            img = Image.FromFile(fnam)
        Catch ex As Exception
            img = Nothing
        End Try
        Me.picLeft.Image = img
        DisplayImage(Me.LS, img, Me.cbExpandPicLeft.Checked)
        PreviewImage(Me.picLeft, img, Me.cbExpandPicLeft.Checked)
    End Sub


    Private Sub DisplayScores()
        Me.picLeft.Image = Nothing
        DisplayScore(Me.LS, Me.tbLeftTeam.Text, Me.tbLeftScore.Text, Me.tbLeftScore.BackColor, Me.tbRightTeam.Text, Me.tbRightScore.Text, Me.tbRightScore.BackColor)
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
        If score > 99 Then score = 99
        If score < -99 Then score = -99
        tbSource.Text = score.ToString
        DisplayScores()
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
        X = X + Me.Left
        Y = Y + Me.Top
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
        Me.Select()
    End Sub

    Public Function SelectImageFilename() As String
        '* Returns blank if you don't pick a legal file
        Dim fn As String = ""
        Dim of As New OpenFileDialog()
        of.Filter = "Image Files(*.BMP;*.GIF;*.JPG;*.PNG;*.WMF)|*.BMP;*.GIF;*.JPG;*.PNG;*.WMF"
        of.InitialDirectory = Me.tbDefaultImageDir.Text
        If of.ShowDialog(Me) = DialogResult.OK Then
            fn = of.FileName
        End If
        of.Dispose()        '* We made a new one, we have to dispose of it.
        Return fn
    End Function


    Private Sub btnPicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPictureLeft.Click
        '* First, stop the slideshow if it's running.
        StopSlideShow()

        Dim fn As String = SelectImageFilename()
        If fn <> "" Then
            Dim img As Image = Image.FromFile(fn)
            '* Do the displays before the previews for speed
            Me.picLeft.Image = img
            DisplayImage(Me.LS, img, Me.cbExpandPicLeft.Checked)
            PreviewImage(Me.picLeft, img, Me.cbExpandPicLeft.Checked)
        End If
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
        If Me.tbNewThing.Text <> "" Then
            If Me.clbThings.Items.Count < MAX_THINGS Then
                Me.clbThings.Items.Add(Me.tbNewThing.Text)
                Me.tbNewThing.ResetText()
            Else
                MessageBox.Show("The list has already reached its max number of Things (" + MAX_THINGS.ToString + "). If you need to add an item, remove another item first.", "Too many Things", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
            End If
            Me.tbNewThing.Focus()
            Me.clbThings.SelectedIndex = -1
        End If
    End Sub
    Private Sub btnClearThings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearThings.Click
        If AskIfSure("Are you sure you want to remove all the Things from the list?") Then
            Dim i As Integer
            ' Set focus to tbNewThing to avoid index errors when
            ' focus is in substitution boxes.
            Me.tbNewThing.Focus()
            For i = 0 To MAX_THINGS
                ThingSubs(i) = ""
            Next
            Me.clbThings.Items.Clear()
            Me.tbCurrentThing.Visible = False
            Me.tbSubstitutions.Visible = False
            Me.btnShowThingLeft.Visible = False
        End If
        'Me.Select()
    End Sub
    Private Sub btnRemoveThing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveThing.Click
        If (Me.clbThings.SelectedIndex >= 0) Then
            Dim prompt As String = "Are you sure you want to remove Thing: '" + Me.clbThings.SelectedItem + "' ?"
            If AskIfSure(prompt) Then
                ' Set focus to tbNewThing to avoid index errors when
                ' focus is in substitution boxes.
                Me.tbNewThing.Focus()
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
            Me.tbCurrentThing.Text = sender.SelectedItem
            Me.tbSubstitutions.Text = ThingSubs(sender.SelectedIndex)
            Me.tbCurrentThing.Visible = True
            Me.tbSubstitutions.Visible = True
            Me.btnShowThingLeft.Visible = True
            Me.clbThings.Hide()
            Me.tbSubstitutions.Select(Me.tbSubstitutions.TextLength, 0)  '* deselect all text here
            Me.clbThings.Show()
            Me.tbSubstitutions.Focus()
        Else
            Me.tbCurrentThing.Visible = False
            Me.tbSubstitutions.Visible = False
            Me.btnShowThingLeft.Visible = False
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

    Private Sub tbCurrentThing_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbCurrentThing.Leave
        Dim sel As Integer = Me.clbThings.SelectedIndex
        If sel >= 0 Then
            Me.clbThings.Items(sel) = sender.Text
        End If
    End Sub

    Private Sub btnList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnListLeft.Click
        Dim ThingsFontSize As Single = 16
        Dim i As Integer
        Dim s As String
        For i = 0 To (Me.clbThings.Items.Count - 1)
            If i > 0 Then s = s + EOL
            If Me.clbThings.CheckedIndices.Contains(i) Then s = s + ChrW(&H25BA)
            s = s + (i + 1).ToString + ". " + Me.clbThings.Items.Item(i)
        Next

        If (Me.clbThings.Items.Count > 4) Then
            If (s.Length > 200) Then
                ThingsFontSize = 11
            ElseIf (s.Length > 160) Or (Me.clbThings.Items.Count > 8) Then
                ThingsFontSize = 12
            ElseIf (s.Length > 130) Or (Me.clbThings.Items.Count > 6) Then
                ThingsFontSize = 14
            End If
        End If
        ' s = s + " " + s.Length.ToString

        DisplayTextScreen(Me.LS, s, Me.clbThings.BackColor, ThingsFontSize)
    End Sub
    Private Sub btnShowThing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowThingLeft.Click
        Dim s As String
        s = Me.tbCurrentThing.Text + EOL + EOL + Me.tbSubstitutions.Text
        DisplayTextScreen(Me.LS, s, clbThings.BackColor, 16)
    End Sub
    Private Sub radioThingColor_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioThingColorLeft.CheckedChanged, radioThingColorRight.CheckedChanged
        Me.clbThings.BackColor = sender.BackColor
        Me.tbSubstitutions.BackColor = sender.BackColor
        Me.tbCurrentThing.BackColor = sender.BackColor
    End Sub

    Private Sub tbNewThing_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNewThing.Enter
        Me.AcceptButton = Me.btnAddThing
    End Sub
    Private Sub tbNewThing_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNewThing.Leave
        Me.AcceptButton = Nothing
    End Sub

    '=================================================================================================
    '* BEGIN SLIDESHOW STUFF

    Private Sub FTreeAutoExpand_C(ByVal ftree As HyperCoder.Win.FileSystemControls.FolderTree, ByVal fpath As String)
        '* This subroutine takes the path given, expands the foldertree nodes down
        '* the path as far as the subdirectories keep matching, from left to right
        '* in the backslash-separated string.
        '* The drive is assumed to be C:, so drive info should NOT be included!
        Dim idx As Integer = fpath.IndexOf("\")
        Dim nextchunk As String
        Dim n1 As System.Windows.Forms.TreeNode = ftree.Nodes(0).Nodes(1).Nodes(1)
        Dim n2 As System.Windows.Forms.TreeNode

        ftree.Nodes(0).Nodes(1).Expand()
        ftree.Nodes(0).Nodes(1).Nodes(1).Expand()

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
        '* ftree.SelectItem(n1.Index)
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
                Dim idx As Integer = Me.lbSlideList.SelectedIndices(i)
                Me.lbSlideList.Items.RemoveAt(idx)
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
            Me.picSlidePreview.Image = Nothing   '** Clear the preview img if we change dirs
        End If
        'Me.FolderTree1.Refresh(Me.FolderTree1.SelectedNode)
        'Me.FolderTree1.Refresh(Me.FolderTree1.TopNode)
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

    Private Function SelectSlideShowFileName() As String
        Dim fn As String
        Dim of As New OpenFileDialog()
        of.Filter = "JANIS SlideShow(*.JSL)|*.JSL"
        of.InitialDirectory = ROOT_SUPPORT_DIR + DEFAULT_SLIDESHOW_DIR
        If of.ShowDialog(Me) = DialogResult.OK Then fn = of.FileName Else fn = ""
        of.Dispose()
        Return fn
    End Function
    Private Sub btnLoadSlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadSlides.Click
        If Me.lbSlideList.Items.Count > 0 Then
            'There are already slides in the list - ask if we want to continue and overwrite
            If Not Me.AskIfSure("Replace the current Slideshow list?") Then Return
        End If
        Dim slidefile As String = SelectSlideShowFileName()
        If slidefile <> "" Then
            Me.lbSlideList.Items.Clear()     '** Empty the list first
            Dim fn As Integer = FreeFile()
            Dim FileErr As Boolean
            Try
                FileOpen(fn, slidefile, OpenMode.Input)
            Catch ex As Exception
                FileErr = True
                MessageBox.Show("An error occurred opening file '" + slidefile + "'.", "File Error")
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
        AllScreensToFront()
    End Sub
    Private Sub btnSaveSlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSlides.Click
        If Me.lbSlideList.Items.Count = 0 Then
            MessageBox.Show("There are no items in the SlideShow yet.", "Nothing to Save", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
            Return
        End If
        Dim sf As New SaveFileDialog()
        sf.Filter = "JANIS Slideshow(*.JSL)|*.JSL"
        sf.InitialDirectory = ROOT_SUPPORT_DIR + DEFAULT_SLIDESHOW_DIR
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
        Me.SlideTimer.Interval = Me.nudDelay.Value * 1000
    End Sub
    Private Sub StartTimer()
        SetTimerInterval()
        Me.SlideTimer.Start()
    End Sub
    Private Sub StopSlideShow()
        Me.SlideTimer.Stop()
        SlidesStatus = SLIDES_STOPPED
        SetPauseButtonColor(False)
        SetPlayButtonColor(False)
        Me.lbSlideList.SelectionMode = SelectionMode.MultiExtended
    End Sub
    Private Sub SetPauseButtonColor(ByVal pause_on As Boolean)
        Dim backclr As System.Drawing.Color = btnStopSlides.BackColor
        If pause_on Then
            btnPauseSlides.BackColor = System.Drawing.Color.Blue
            btnPauseSlides.ForeColor = backclr
        Else
            btnPauseSlides.BackColor = backclr
            btnPauseSlides.ForeColor = System.Drawing.Color.Blue
        End If
    End Sub
    Private Sub SetPlayButtonColor(ByVal play_on As Boolean)
        Dim backclr As System.Drawing.Color = btnStopSlides.BackColor
        If play_on Then
            btnPlaySlides.BackColor = System.Drawing.Color.Green
            btnPlaySlides.ForeColor = backclr
        Else
            btnPlaySlides.BackColor = backclr
            btnPlaySlides.ForeColor = System.Drawing.Color.Green
        End If
    End Sub
    Private Sub nudDelay_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDelay.ValueChanged
        SetTimerInterval()
    End Sub

    Private Sub SlideTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SlideTimer.Tick
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
        If SlidesStatus = SLIDES_PLAYING Then Return
        If Me.lbSlideList.Items.Count < 1 Then Return
        Me.lbSlideList.SelectionMode = SelectionMode.One
        If SlidesStatus <> SLIDES_PAUSED Then Me.lbSlideList.SelectedIndex = 0
        DisplayBothImages(Me.lbSlideList.SelectedItem)
        SetPauseButtonColor(False)
        SetPlayButtonColor(True)
        SlidesStatus = SLIDES_PLAYING
        StartTimer()
    End Sub
    Private Sub btnStopSlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStopSlides.Click
        StopSlideShow()
    End Sub
    Private Sub btnPauseSlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPauseSlides.Click
        If SlidesStatus = SLIDES_STOPPED Then Return
        If SlidesStatus = SLIDES_PAUSED Then
            SlidesStatus = SLIDES_PLAYING
            SetPauseButtonColor(False)
            StartTimer()
            Return
        End If
        '** If we get here, Slides Status must be PLAYING
        Me.SlideTimer.Stop()
        SlidesStatus = SLIDES_PAUSED
        SetPauseButtonColor(True)
    End Sub
    Private Sub btnChangeSlide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirstSlide.Click, btnNextSlide.Click, btnPrevSlide.Click, btnLastSlide.Click
        If SlidesStatus = SLIDES_STOPPED Then Return
        If SlidesStatus = SLIDES_PLAYING Then Me.SlideTimer.Stop() '* temporary stoppage
        Select Case sender.Name
            Case "btnFirstSlide"
                Me.lbSlideList.SelectedIndex = 0
            Case "btnPrevSlide"
                If Me.lbSlideList.SelectedIndex > 0 Then
                    Me.lbSlideList.SelectedIndex = Me.lbSlideList.SelectedIndex - 1
                Else
                    '** Roll over before the beginning
                    Me.lbSlideList.SelectedIndex = Me.lbSlideList.Items.Count - 1
                End If
            Case "btnNextSlide"
                If Me.lbSlideList.SelectedIndex < (Me.lbSlideList.Items.Count - 1) Then
                    Me.lbSlideList.SelectedIndex = Me.lbSlideList.SelectedIndex + 1
                Else
                    '** Roll over past the end
                    Me.lbSlideList.SelectedIndex = 0
                End If
            Case "btnLastSlide"
                Me.lbSlideList.SelectedIndex = Me.lbSlideList.Items.Count - 1
        End Select
        DisplayBothImages(Me.lbSlideList.SelectedItem)
        If SlidesStatus = SLIDES_PLAYING Then StartTimer()
    End Sub
    Private Sub lbSlideList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSlideList.DoubleClick
        '**********************************************
        '* A double click changes the slide immediately
        '**********************************************
        If SlidesStatus = SLIDES_STOPPED Then Return
        If SlidesStatus = SLIDES_PLAYING Then Me.SlideTimer.Stop() '* temporary stoppage
        DisplayBothImages(Me.lbSlideList.SelectedItem)
        If SlidesStatus = SLIDES_PLAYING Then StartTimer()
    End Sub

    '=================================================================================================
    '* BEGIN HOTBUTTONS STUFF

    Private Function HotButton(ByVal i As Integer) As Button
        Dim hotbtns() As Button = {Me.btnHot1, Me.btnHot2, Me.btnHot3, Me.btnHot4, Me.btnHot5, Me.btnHot6, Me.btnHot7, Me.btnHot8, Me.btnHot9, Me.btnHot10}
        Return hotbtns(i)
    End Function

    Private Function HotImage(ByVal i As Integer) As TextBox
        Dim textboxes() As TextBox = {Me.tbHBfile1, Me.tbHBfile2, Me.tbHBfile3, Me.tbHBfile4, Me.tbHBfile5, Me.tbHBfile6, Me.tbHBfile7, Me.tbHBfile8, Me.tbHBfile9, Me.tbHBfile10}
        Return textboxes(i)
    End Function

    Private Function HotText(ByVal i As Integer) As TextBox
        Dim textboxes() As TextBox = {Me.tbHBtext1, Me.tbHBtext2, Me.tbHBtext3, Me.tbHBtext4, Me.tbHBtext5, Me.tbHBtext6, Me.tbHBtext7, Me.tbHBtext8, Me.tbHBtext9, Me.tbHBtext10}
        Return textboxes(i)
    End Function

    Private Sub cbHBActive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbHBActive.CheckedChanged
        Dim i As Integer
        For i = 0 To 9
            HotButton(i).Visible = Me.cbHBActive.Checked
        Next
    End Sub

    Private Function SelectHotButtonsFileName() As String
        Dim fn As String
        Dim of As New OpenFileDialog()
        of.Filter = "JANIS HotButtons File(*.JHB)|*.JHB"
        of.InitialDirectory = ROOT_SUPPORT_DIR + DEFAULT_HOTBUTTON_DIR
        If of.ShowDialog(Me) = DialogResult.OK Then fn = of.FileName Else fn = ""
        of.Dispose()
        Return fn
    End Function

    Private Sub LoadHotButtons(ByVal hbfile As String)
        Dim fn As Integer = FreeFile()
        Dim FileErr As Boolean
        Try
            FileOpen(fn, hbfile, OpenMode.Input)
        Catch ex As Exception
            FileErr = True
            MessageBox.Show("An error occurred opening HotButtons file '" + hbfile + "'.", "File Error")
        End Try
        If Not FileErr Then
            Dim i As Integer = 0
            While Not EOF(fn)
                Dim s As String
                Input(fn, s)
                Dim info() As String = Split(s, "¶")
                HotButton(i).Text = info(0)
                HotText(i).Text = info(0)
                HotButton(i).Tag = info(1)
                HotImage(i).Text = info(1)
                i = i + 1
            End While
            FileClose(fn)
            Me.HotButtonsChanged = False
        End If
        AllScreensToFront()
    End Sub

    Private Sub SaveHotButtons()
        If Not HotButtonsChanged Then
            If MessageBox.Show("You haven't made any changes to the HotButtons. Save anyway?", "Nothing to Save", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly) <> DialogResult.Yes Then Return
        End If
        Dim sf As New SaveFileDialog()
        sf.Filter = "JANIS HotButtons File(*.JHB)|*.JHB"
        sf.InitialDirectory = ROOT_SUPPORT_DIR + DEFAULT_HOTBUTTON_DIR
        If sf.ShowDialog(Me) = DialogResult.OK Then
            Dim fn As Integer = FreeFile()
            FileOpen(fn, sf.FileName, OpenMode.Output)
            Dim i As Integer
            For i = 0 To 9
                PrintLine(fn, HotButton(i).Text + "¶" + HotButton(i).Tag)
            Next
            FileClose(fn)
            HotButtonsChanged = False
        End If
        sf.Dispose()
        AllScreensToFront()
    End Sub

    Private Sub btnSaveHB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveHB.Click
        SaveHotButtons()
    End Sub

    Private Sub btnLoadHB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadHB.Click
        If Me.HotButtonsChanged Then
            'Ask if we want to continue and overwrite
            If MessageBox.Show("Replace the current Hot Buttons without saving?", "Hot Button Definitions Changed", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly) <> DialogResult.OK Then
                Return
            End If
        End If
        Dim hbfile As String = SelectHotButtonsFileName()
        If hbfile <> "" Then LoadHotButtons(hbfile)
    End Sub

    Private Sub btnClearHB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearHB.Click
        If AskIfSure("Are you sure you want to clear all the Hot Button settings?") Then
            Dim i As Integer
            For i = 0 To 9
                HotText(i).Text = ""
                HotButton(i).Text = ""
                HotButton(i).Tag = ""
                HotImage(i).Text = ""
            Next
            Me.HotButtonsChanged = False
        End If
    End Sub

    Private Sub btnHBSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHBSelect1.Click, btnHBSelect2.Click, btnHBSelect3.Click, btnHBSelect4.Click, btnHBSelect5.Click, btnHBSelect6.Click, btnHBSelect7.Click, btnHBSelect8.Click, btnHBSelect9.Click, btnHBSelect10.Click
        Dim fn As String = SelectImageFilename()
        If fn <> "" Then
            Dim i As Integer = CInt(sender.Tag)
            HotButton(i).Tag = fn
            HotImage(i).Text = fn
            Me.HotButtonsChanged = True
        End If
    End Sub

    Private Sub tbHBtext_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbHBtext1.TextChanged, tbHBtext2.TextChanged, tbHBtext3.TextChanged, tbHBtext4.TextChanged, tbHBtext5.TextChanged, tbHBtext6.TextChanged, tbHBtext7.TextChanged, tbHBtext8.TextChanged, tbHBtext9.TextChanged, tbHBtext10.TextChanged
        HotButton(CInt(sender.Tag)).Text = sender.Text
        Me.HotButtonsChanged = True
    End Sub

    Private Sub tbHBfile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbHBfile1.TextChanged, tbHBfile2.TextChanged, tbHBfile3.TextChanged, tbHBfile4.TextChanged, tbHBfile5.TextChanged, tbHBfile6.TextChanged, tbHBfile7.TextChanged, tbHBfile8.TextChanged, tbHBfile9.TextChanged, tbHBfile10.TextChanged
        '* Put the cursor at the end so that we can see the
        '* more sigificant part of the file name info
        sender.SelectionStart = sender.TextLength
    End Sub

    Private Sub btnHot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHot1.Click, btnHot2.Click, btnHot3.Click, btnHot4.Click, btnHot5.Click, btnHot6.Click, btnHot7.Click, btnHot8.Click, btnHot9.Click, btnHot10.Click
        Dim img_name As String = sender.Tag
        If img_name <> "" Then
            '* First, stop the slideshow if it's running.
            StopSlideShow()
            Me.DisplayBothImages(img_name)
        End If
    End Sub

    '=================================================================================================
    '* BEGIN PREFERENCES STUFF

    Private Sub pnlDefaultTextColorLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlDefaultTextColorLeft1.Click, pnlDefaultTextColorLeft2.Click, pnlDefaultTextColorLeft3.Click, pnlDefaultTextColorLeft4.Click, pnlDefaultTextColorLeft5.Click, pnlDefaultTextColorLeft6.Click
        Me.lblDefaultColorLeft.BackColor = sender.BackColor
    End Sub
    Private Sub pnlDefaultTextColorRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlDefaultTextColorRight1.Click, pnlDefaultTextColorRight2.Click, pnlDefaultTextColorRight3.Click, pnlDefaultTextColorRight4.Click, pnlDefaultTextColorRight5.Click, pnlDefaultTextColorRight6.Click
        Me.lblDefaultColorRight.BackColor = sender.BackColor
    End Sub
    Private Sub btnChooseDefaultTextColorLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseDefaultTextColorLeft.Click
        Me.lblDefaultColorLeft.BackColor = PickColor(50, 300, Me.lblDefaultColorLeft.BackColor)
    End Sub
    Private Sub btnChooseDefaultTextColorRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseDefaultTextColorRight.Click
        Me.lblDefaultColorRight.BackColor = PickColor(450, 300, Me.lblDefaultColorRight.BackColor)
    End Sub

    Private Sub btnChooseDefaultImageDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseDefaultImageDir.Click
        Me.tbDefaultImageDir.Text = SelectDir(Me.tbDefaultImageDir.Text)
    End Sub

    Private Sub cbDisplayDefaultImage_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDisplayDefaultImage.CheckedChanged
        Me.tbDefaultImageFile.Enabled = sender.Checked
        Me.btnChooseDefaultImage.Enabled = sender.Checked
        If sender.Checked Then
            Me.tbDefaultImageFile.BackColor = Color.FromName("Control")
        Else
            Me.tbDefaultImageFile.BackColor = Color.FromName("ControlDark")
        End If
    End Sub
    Private Sub btnChooseDefaultImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseDefaultImage.Click
        Dim fn As String = SelectImageFilename()
        If fn <> "" Then Me.tbDefaultImageFile.Text = fn
    End Sub

    Private Sub cbLoadDefaultHB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbLoadDefaultHB.CheckedChanged
        Me.tbDefaultHBFile.Enabled = sender.Checked
        Me.btnChooseDefaultHB.Enabled = sender.Checked
        If sender.Checked Then
            Me.tbDefaultHBFile.BackColor = Color.FromName("Control")
        Else
            Me.tbDefaultHBFile.BackColor = Color.FromName("ControlDark")
        End If
    End Sub
    Private Sub btnChooseDefaultHB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseDefaultHB.Click
        Dim fn As String = SelectHotButtonsFileName()
        If fn <> "" Then Me.tbDefaultHBFile.Text = fn
    End Sub

    Private Sub cbLoadDefaultSlides_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbLoadDefaultSlides.CheckedChanged
        Me.tbDefaultSlideShow.Enabled = sender.Checked
        Me.btnChooseDefaultSlideShow.Enabled = sender.Checked
        Me.cbPlaySlidesAtStart.Enabled = sender.Checked
        If sender.Checked Then
            Me.tbDefaultSlideShow.BackColor = Color.FromName("Control")
        Else
            Me.tbDefaultSlideShow.BackColor = Color.FromName("ControlDark")
        End If
    End Sub
    Private Sub btnChooseDefaultSlideShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseDefaultSlideShow.Click
        Dim fn As String = SelectSlideShowFileName()
        If fn <> "" Then Me.tbDefaultSlideShow.Text = fn
    End Sub

    Private Sub btnDefaultPrefs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefaultPrefs.Click
        Me.SetDefaultPrefs()
    End Sub
    Private Sub btnSavePrefs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSavePrefs.Click
        '* Write the prefs off to a file (JANIS.ini).
        '*
        SavePrefsToFile(PREFS_FILE)
    End Sub
    Private Sub btnRevertPrefs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevertPrefs.Click
        '* The last saved values are stored in the TAG properties of all the controls on this tab.
        '* Use them to put everything back.
        Me.lblDefaultColorLeft.BackColor = Color.FromArgb(Me.lblDefaultColorLeft.Tag)
        Me.lblDefaultColorRight.BackColor = Color.FromArgb(Me.lblDefaultColorRight.Tag)
        Me.tbDefaultFontSize.Text = Me.tbDefaultFontSize.Tag
        Me.tbDefaultImageDir.Text = Me.tbDefaultImageDir.Tag
        Me.tbDefaultImageFile.Text = Me.tbDefaultImageFile.Tag
        Me.cbDisplayDefaultImage.Checked = Me.cbDisplayDefaultImage.Tag
        Me.tbDefaultHBFile.Text = Me.tbDefaultHBFile.Tag
        Me.cbLoadDefaultHB.Checked = Me.cbLoadDefaultHB.Tag
        Me.nudDefaultSlideDelay.Value = CInt(Me.nudDefaultSlideDelay.Tag)
        Me.tbDefaultSlideShow.Text = Me.tbDefaultSlideShow.Tag
        Me.cbPlaySlidesAtStart.Checked = Me.cbPlaySlidesAtStart.Tag
        Me.cbLoadDefaultSlides.Checked = Me.cbLoadDefaultSlides.Tag
    End Sub

    Private Function PrefsChanged()
        If Me.lblDefaultColorLeft.BackColor.ToArgb <> Me.lblDefaultColorLeft.Tag Then Return True
        If Me.lblDefaultColorRight.BackColor.ToArgb <> Me.lblDefaultColorRight.Tag Then Return True
        If Me.tbDefaultFontSize.Text <> Me.tbDefaultFontSize.Tag Then Return True
        If Me.tbDefaultImageDir.Text <> Me.tbDefaultImageDir.Tag Then Return True
        If Me.tbDefaultImageFile.Text <> Me.tbDefaultImageFile.Tag Then Return True
        If Me.cbDisplayDefaultImage.Checked <> Me.cbDisplayDefaultImage.Tag Then Return True
        If Me.tbDefaultHBFile.Text <> Me.tbDefaultHBFile.Tag Then Return True
        If Me.cbLoadDefaultHB.Checked <> Me.cbLoadDefaultHB.Tag Then Return True
        If Me.nudDefaultSlideDelay.Value <> Me.nudDefaultSlideDelay.Tag Then Return True
        If Me.tbDefaultSlideShow.Text <> Me.tbDefaultSlideShow.Tag Then Return True
        If Me.cbPlaySlidesAtStart.Checked <> Me.cbPlaySlidesAtStart.Tag Then Return True
        If Me.cbLoadDefaultSlides.Checked <> Me.cbLoadDefaultSlides.Tag Then Return True
        Return False
    End Function

    Private Sub LoadPrefsFromFile(ByVal filename As String)
        '* Determine if the file exists
        Dim MyPrefsFile As New System.IO.FileInfo(filename)
        If MyPrefsFile.Exists Then
            Dim fn As Integer = FreeFile()
            Dim s As String
            Dim FileErr As Boolean
            Try
                FileOpen(fn, filename, OpenMode.Input)
            Catch ex As Exception
                FileErr = True
                MessageBox.Show("An error occurred opening file '" + filename + "'.", "File Error")
            End Try
            If Not FileErr Then
                Input(fn, s)
                Me.lblDefaultColorLeft.BackColor = Color.FromArgb(CInt(s))
                Input(fn, s)
                Me.lblDefaultColorRight.BackColor = Color.FromArgb(CInt(s))
                Input(fn, Me.tbDefaultFontSize.Text)
                Input(fn, Me.tbDefaultImageDir.Text)
                Input(fn, s)
                Me.cbDisplayDefaultImage.Checked = (s = "True")
                Input(fn, Me.tbDefaultImageFile.Text)
                Input(fn, s)
                Me.cbLoadDefaultHB.Checked = (s = "True")
                Input(fn, Me.tbDefaultHBFile.Text)
                Input(fn, s)
                Me.nudDefaultSlideDelay.Value = CInt(s)
                Input(fn, s)
                Me.cbLoadDefaultSlides.Checked = (s = "True")
                Input(fn, Me.tbDefaultSlideShow.Text)
                Input(fn, s)
                Me.cbPlaySlidesAtStart.Checked = (s = "True")
                FileClose(fn)
                StorePrefs()
            End If
        Else
            '* OK, the prefs file was not found. So load defaults and
            '* write the prefs file with those defaults.
            SetDefaultPrefs()
            SavePrefsToFile(filename)
        End If
        AllScreensToFront()
    End Sub
    Private Sub SavePrefsToFile(ByVal filename As String)
        If Not PrefsChanged() Then Exit Sub
        Dim fn As Integer = FreeFile()
        FileOpen(fn, filename, OpenMode.Output)
        PrintLine(fn, Me.lblDefaultColorLeft.BackColor.ToArgb.ToString)
        PrintLine(fn, Me.lblDefaultColorRight.BackColor.ToArgb.ToString)
        PrintLine(fn, Me.tbDefaultFontSize.Text)
        PrintLine(fn, Me.tbDefaultImageDir.Text)
        PrintLine(fn, Me.cbDisplayDefaultImage.Checked.ToString)
        PrintLine(fn, Me.tbDefaultImageFile.Text)
        PrintLine(fn, Me.cbLoadDefaultHB.Checked.ToString)
        PrintLine(fn, Me.tbDefaultHBFile.Text)
        PrintLine(fn, Me.nudDefaultSlideDelay.Value.ToString)
        PrintLine(fn, Me.cbLoadDefaultSlides.Checked.ToString)
        PrintLine(fn, Me.tbDefaultSlideShow.Text)
        PrintLine(fn, Me.cbPlaySlidesAtStart.Checked.ToString)
        FileClose(fn)
        StorePrefs()
        AllScreensToFront()
    End Sub
    Private Sub SetDefaultPrefs()
        '* Reset the Preferences screen settings to factory defaults
        Me.lblDefaultColorLeft.BackColor = Me.pnlDefaultTextColorLeft1.BackColor
        Me.lblDefaultColorRight.BackColor = Me.pnlDefaultTextColorLeft2.BackColor
        Me.tbDefaultFontSize.Text = "60"
        Me.tbDefaultImageDir.Text = ROOT_SUPPORT_DIR
        Me.tbDefaultImageFile.Text = ""
        Me.cbDisplayDefaultImage.Checked = False
        Me.tbDefaultHBFile.Text = ""
        Me.cbLoadDefaultHB.Checked = False
        Me.nudDefaultSlideDelay.Value = 15
        Me.tbDefaultSlideShow.Text = ""
        Me.cbPlaySlidesAtStart.Checked = False
        Me.cbLoadDefaultSlides.Checked = False
    End Sub
    Private Sub StorePrefs()
        '* Stores whatever the current set of preferences is in the TAG properties of the controls on the
        '* preferences tab. Compare later to see if things have changed or to restore to last happy prefs.
        Me.lblDefaultColorLeft.Tag = Me.lblDefaultColorLeft.BackColor.ToArgb
        Me.lblDefaultColorRight.Tag = Me.lblDefaultColorRight.BackColor.ToArgb
        Me.tbDefaultFontSize.Tag = Me.tbDefaultFontSize.Text
        Me.tbDefaultImageDir.Tag = Me.tbDefaultImageDir.Text
        Me.tbDefaultImageFile.Tag = Me.tbDefaultImageFile.Text
        Me.cbDisplayDefaultImage.Tag = Me.cbDisplayDefaultImage.Checked
        Me.tbDefaultHBFile.Tag = Me.tbDefaultHBFile.Text
        Me.cbLoadDefaultHB.Tag = Me.cbLoadDefaultHB.Checked
        Me.nudDefaultSlideDelay.Tag = Me.nudDefaultSlideDelay.Value
        Me.tbDefaultSlideShow.Tag = Me.tbDefaultSlideShow.Text
        Me.cbPlaySlidesAtStart.Tag = Me.cbPlaySlidesAtStart.Checked
        Me.cbLoadDefaultSlides.Tag = Me.cbLoadDefaultSlides.Checked
    End Sub
    Private Sub ApplyPrefs()
        SetTeamColor("Left", Me.lblDefaultColorLeft.BackColor)
        SetTeamColor("Right", Me.lblDefaultColorRight.BackColor)
        Me.tbLeftText.BackColor = Me.lblDefaultColorLeft.BackColor
        Me.tbRightText.BackColor = Me.lblDefaultColorRight.BackColor
        Me.tbLeftFontSize.Text = Me.tbDefaultFontSize.Text
        Me.tbRightFontSize.Text = Me.tbDefaultFontSize.Text
        If Me.cbDisplayDefaultImage.Checked Then DisplayBothImages(Me.tbDefaultImageFile.Text)
        If Me.cbLoadDefaultHB.Checked Then
            If Me.tbDefaultHBFile.Text <> "" Then LoadHotButtons(Me.tbDefaultHBFile.Text)
        End If
        Me.radioThingColorLeft.BackColor = Me.lblDefaultColorLeft.BackColor
        Me.radioThingColorRight.BackColor = Me.lblDefaultColorRight.BackColor
        Me.nudDelay.Value = Me.nudDefaultSlideDelay.Value
        If Me.cbLoadDefaultSlides.Checked Then
            '* Load SlideShow
            If Me.cbPlaySlidesAtStart.Checked Then
                '* Play SlideShow
            End If
        End If
        StorePrefs()
    End Sub
    Private Function SelectDir(ByVal startdir As String) As String
        Dim pickdir As String = startdir
        Dim choose As New fmDirDialog()
        '* The FolderTree control can't be selected programmatically
        '* (what the hell was the guy thinking), but we can expand it down to the
        '* directory we want to see at the start. However, since we can't select it,
        '* the FileListBox's path will be empty.
        Me.FTreeAutoExpand_C(choose.FolderTree1, startdir.Substring(3)) '* skip the c:\ part
        If choose.ShowDialog(Me) = DialogResult.OK Then
            pickdir = choose.FolderTree1.SelectedNode.FilePath
        End If
        choose.Dispose()
        Return pickdir
    End Function

    '=================================================================================================
    '* BEGIN COUNTDOWN TIMER STUFF

    Private Sub MenuCountdownToggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCountdownToggle.Click
        Me.btnStartCountdown.Visible = Not (Me.btnStartCountdown.Visible)
    End Sub
    Private Sub btnStartCountdown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartCountdown.Click
        Me.CountdownSeconds = 1800 ' = 30 * 60
        Me.CountdownTimer.Interval = 1000

        Me.CountdownTimer.Enabled = True
        MsgBox("Click OK to end the countdown and return to JANIS.", MsgBoxStyle.ApplicationModal Or MsgBoxStyle.OKOnly Or MsgBoxStyle.MsgBoxSetForeground, "COUNTDOWN MODE")
        Me.CountdownTimer.Enabled = False
    End Sub
    Private Sub CountdownTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CountdownTimer.Tick
        Dim CountdownBackground As System.Drawing.Color
        Dim CountdownSize As Integer
        Dim minutes As Integer = CInt(Fix(Me.CountdownSeconds / 60))
        Dim seconds As Integer = Me.CountdownSeconds Mod 60

        If Me.CountdownSeconds > 120 Then
            CountdownBackground = System.Drawing.Color.DarkGreen
            CountdownSize = 200 - CInt(0.09524! * (Me.CountdownSeconds - 120))
        ElseIf Me.CountdownSeconds <= 120 And Me.CountdownSeconds > 30 Then
            CountdownBackground = System.Drawing.Color.Maroon
            CountdownSize = 250 - CInt(5.0! / 9.0! * (Me.CountdownSeconds - 30))
        ElseIf CountdownSeconds > 10 Then
            CountdownBackground = System.Drawing.Color.Maroon
            CountdownSize = 300 - CInt(5.0! / 2.0! * (Me.CountdownSeconds - 10))
        ElseIf seconds >= 0 Then
            CountdownBackground = System.Drawing.Color.Maroon
            CountdownSize = 300
        Else
            If Me.CountdownTimer.Interval <> 250 Then Me.CountdownTimer.Interval = 250
            minutes = 0
            seconds = 0
            CountdownSize = 300
            If Fix(Me.CountdownSeconds / 2.0!) * 2 = Me.CountdownSeconds Then
                CountdownBackground = System.Drawing.Color.Black
            Else
                CountdownBackground = System.Drawing.Color.Maroon
            End If
        End If

        Dim TimeText As String
        If minutes > 0 Then TimeText = minutes.ToString
        TimeText = TimeText + ":"
        If seconds < 10 Then TimeText = TimeText + "0"
        TimeText = TimeText + seconds.ToString

        Me.CountdownSeconds = Me.CountdownSeconds - 1

        DisplayTextScreen(Me.LS, TimeText, CountdownBackground, CountdownSize * Me.DisplayFontRatio)

    End Sub

    '=================================================================================================

    Private Sub EasterEgg1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EasterEgg1.Click
        MessageBox.Show("Pass it on...", "BILL LOVES BETSE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
    End Sub

End Class
