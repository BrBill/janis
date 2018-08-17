Imports System
Imports System.IO
Imports System.Environment
Imports System.Runtime.InteropServices

Namespace JANIS
    Public Class fmMain
        Inherits System.Windows.Forms.Form

        '***************
        '* System Constants
        '***************
        Const MAX_THINGS As Integer = 9
        Const EOL As String = Chr(13) + Chr(10)
        Const DEFAULT_SLIDESHOW_DIR As String = "\SlideShows"
        Const DEFAULT_HOTBUTTON_DIR As String = "\HotButtons"
        Const SLIDES_STOPPED As Integer = 0
        Const SLIDES_PAUSED As Integer = 1
        Const SLIDES_PLAYING As Integer = 2
        Const SLIDES_WHAMMY As Integer = 3
        Const HOTSIDE_PIXELS As Integer = 15


        Private Class FileID   ' Used by the image indexing system
            Public Path As String = ""   ' The directory it sits in.
            Public Name As String = ""   ' The name of this file.
            Public ReadOnly Property FullPath() As String
                Get
                    Return Path + "\" + Name
                End Get
            End Property
        End Class

        '***************
        '* Globals
        '***************
        Private ROOT_SUPPORT_DIR As String
        Private PREFS_FILE As String
        Private splash As fmSplash
        Private TestMode As Boolean = False
        Dim DisplayModeAdjustment As Single = 1.0 ' Divide font setting by this for display. Differs for test/arena mode.
        Dim DisplayToEntryFontRatio As Single = 123 / 42  ' This is the size ratio of fonts in the display vs. in the textbox
        Dim DisplayFontRatio As Single = 33 / 80 ' This is the "should be" size ratio of display to what I once thought it was.

        Private ImageFileExtensions() As String = {".BMP", ".GIF", ".JPG", ".PNG", ".WMF"}
        Private ImageLibrary As New Collection()

        Private LS As fmScreen      '* The left team screen
        Private RS As fmScreen      '* The right team screen

        Private ThingSubs(MAX_THINGS) As String           '* Substitutions for 5 Things
        Private SlidesStatus As Integer = SLIDES_STOPPED  '* Keep track of whether Slideshow is running
        Private SlideTimerTag As String = ""
        Private HotButtonsChanged As Boolean              '* Have we changed the hot buttons?
        Private BufferedSlide As Image = Nothing          '* For holding next whammy slide
        Private DragBounds As Rectangle
        Private DragMethod As String
        Private CountdownSeconds As Integer = 300
        Private CountdownWarnSeconds As Integer
        Public ComponentsDoneInitializing As Boolean = False


#Region " Windows Form Designer generated code "

        Public Sub New()
            ' To ensure the splashscreen is shown before/while the form starts loading its
            ' controls, don't put it in the load event, but in the constructor (sub New).
            ' Normally the sub is located inside the " Windows Form Designer generated code "
            ' region, but I moved it outside the region for readability.
            MyBase.New()

            ' The code to show itself is integrated in the constructor of the splashscreen.
            ' This line is the only line that needs to be added when using fmSplash. The rest
            ' is generated automatically when adding a new form.
            'splash as New fmSplashScreen(Me)
            ' This example splashscreen also has the ability to show itself for a minimum
            ' number of seconds. For example, if you want to show the splash for at least 6
            ' seconds, change the code above to:
            ' Dim F As New fmSplash(Me, 6)
            splash = New fmSplash(Me, 5)

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            Me.ComponentsDoneInitializing = True
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
        Friend WithEvents cbShadowsEnabled As System.Windows.Forms.CheckBox
        Friend WithEvents btnReIndexImgLib As System.Windows.Forms.Button
        Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
        Friend WithEvents Label43 As System.Windows.Forms.Label
        Friend WithEvents Label38 As System.Windows.Forms.Label
        Friend WithEvents Label39 As System.Windows.Forms.Label
        Friend WithEvents Label40 As System.Windows.Forms.Label
        Friend WithEvents nudDefaultCountdownSeconds As System.Windows.Forms.NumericUpDown
        Friend WithEvents nudDefaultCountdownMinutes As System.Windows.Forms.NumericUpDown
        Friend WithEvents nudDefaultCountdownHours As System.Windows.Forms.NumericUpDown
        Friend WithEvents grpLoadImage As System.Windows.Forms.GroupBox
        Friend WithEvents grpPasteImage As System.Windows.Forms.GroupBox
        Friend WithEvents grpClearText As System.Windows.Forms.GroupBox
        Friend WithEvents grpLoadText As System.Windows.Forms.GroupBox
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents grpShowLeftText As System.Windows.Forms.GroupBox
        Friend WithEvents gbCountdownControls As System.Windows.Forms.GroupBox
        Friend WithEvents Label37 As System.Windows.Forms.Label
        Friend WithEvents Label36 As System.Windows.Forms.Label
        Friend WithEvents Label35 As System.Windows.Forms.Label
        Friend WithEvents Label34 As System.Windows.Forms.Label
        Friend WithEvents nudCountdownWarnSeconds As System.Windows.Forms.NumericUpDown
        Friend WithEvents nudCountdownWarnMinutes As System.Windows.Forms.NumericUpDown
        Friend WithEvents nudCountdownWarnHours As System.Windows.Forms.NumericUpDown
        Friend WithEvents Label33 As System.Windows.Forms.Label
        Friend WithEvents cbCountdownVisible As System.Windows.Forms.CheckBox
        Friend WithEvents nudCountdownSeconds As System.Windows.Forms.NumericUpDown
        Friend WithEvents nudCountdownMinutes As System.Windows.Forms.NumericUpDown
        Friend WithEvents nudCountdownHours As System.Windows.Forms.NumericUpDown
        Friend WithEvents btnResetCountdown As System.Windows.Forms.Button
        Friend WithEvents btnStartCountdown As System.Windows.Forms.Button
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
        Friend WithEvents btnPictureLeft As System.Windows.Forms.Button
        Friend WithEvents btnPictureRight As System.Windows.Forms.Button
        Friend WithEvents btnPictureBoth As System.Windows.Forms.Button
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
        Friend WithEvents menuDummy As System.Windows.Forms.MenuItem
        Friend WithEvents menuAdd1Left As System.Windows.Forms.MenuItem
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
        Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
        Friend WithEvents Label12 As System.Windows.Forms.Label
        Friend WithEvents btnWhammy As System.Windows.Forms.Button
        Friend WithEvents btnClearSlideList As System.Windows.Forms.Button
        Friend WithEvents btnLeftTextBoth As System.Windows.Forms.Button
        Friend WithEvents btnShowLeftText As System.Windows.Forms.Button
        Friend WithEvents btnRightTextBoth As System.Windows.Forms.Button
        Friend WithEvents btnShowRightText As System.Windows.Forms.Button
        Friend WithEvents btnDocLoadBoth As System.Windows.Forms.Button
        Friend WithEvents btnDocLoadRight As System.Windows.Forms.Button
        Friend WithEvents btnDocLoadLeft As System.Windows.Forms.Button
        Friend WithEvents EasterEgg1 As System.Windows.Forms.MenuItem
        Friend WithEvents btnPrevSlide As System.Windows.Forms.Button
        Friend WithEvents btnNextSlide As System.Windows.Forms.Button
        Friend WithEvents btnPauseSlides As System.Windows.Forms.Button
        Friend WithEvents tbCurrentThing As System.Windows.Forms.TextBox
        Friend WithEvents btnBothTextScreens As System.Windows.Forms.Button
        Friend WithEvents btnRightTextLeft As System.Windows.Forms.Button
        Friend WithEvents btnLeftTextRight As System.Windows.Forms.Button
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
        Friend WithEvents btnClearTextRight As System.Windows.Forms.Button
        Friend WithEvents btnClearTextLeft As System.Windows.Forms.Button
        Friend WithEvents Label16 As System.Windows.Forms.Label
        Friend WithEvents pnlPicBackLeft As System.Windows.Forms.Panel
        Friend WithEvents pnlPicBackRight As System.Windows.Forms.Panel
        Friend WithEvents picRight As System.Windows.Forms.PictureBox
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
        Friend WithEvents btnClearTextBoth As System.Windows.Forms.Button
        Friend WithEvents CountdownTimer As System.Windows.Forms.Timer
        Friend WithEvents cbExpandPicLeft As System.Windows.Forms.CheckBox
        Friend WithEvents cbExpandPicRight As System.Windows.Forms.CheckBox
        Friend WithEvents tpImgSearch As System.Windows.Forms.TabPage
        Friend WithEvents Label19 As System.Windows.Forms.Label
        Friend WithEvents btnImgSearch As System.Windows.Forms.Button
        Friend WithEvents btnSearchImgAddSlide As System.Windows.Forms.Button
        Friend WithEvents btnSearchImgShowRight As System.Windows.Forms.Button
        Friend WithEvents btnSearchImgShowBoth As System.Windows.Forms.Button
        Friend WithEvents btnSearchImgShowLeft As System.Windows.Forms.Button
        Friend WithEvents picImgSearchPreview As System.Windows.Forms.PictureBox
        Friend WithEvents Label20 As System.Windows.Forms.Label
        Friend WithEvents comboImgSearchText As System.Windows.Forms.ComboBox
        Friend WithEvents lbImgResults As System.Windows.Forms.ListBox
        Friend WithEvents Label21 As System.Windows.Forms.Label
        Friend WithEvents Label32 As System.Windows.Forms.Label
        Friend WithEvents lblLibraryCount As System.Windows.Forms.Label
        Friend WithEvents btnPasteBoth As System.Windows.Forms.Button
        Friend WithEvents btnPasteRight As System.Windows.Forms.Button
        Friend WithEvents btnPasteLeft As System.Windows.Forms.Button
        Friend WithEvents ExpTree1 As ExpTreeLib.ExpTree


        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fmMain))
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
            Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
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
            Me.picLeft = New System.Windows.Forms.PictureBox()
            Me.btnPictureLeft = New System.Windows.Forms.Button()
            Me.btnPictureRight = New System.Windows.Forms.Button()
            Me.btnPictureBoth = New System.Windows.Forms.Button()
            Me.btnLeftScoreColor = New System.Windows.Forms.Button()
            Me.btnRightScoreColor = New System.Windows.Forms.Button()
            Me.TabControl1 = New System.Windows.Forms.TabControl()
            Me.tpScreenText = New System.Windows.Forms.TabPage()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.btnRightTextLeft = New System.Windows.Forms.Button()
            Me.btnRightTextBoth = New System.Windows.Forms.Button()
            Me.btnShowRightText = New System.Windows.Forms.Button()
            Me.grpShowLeftText = New System.Windows.Forms.GroupBox()
            Me.btnShowLeftText = New System.Windows.Forms.Button()
            Me.btnLeftTextBoth = New System.Windows.Forms.Button()
            Me.btnLeftTextRight = New System.Windows.Forms.Button()
            Me.grpLoadText = New System.Windows.Forms.GroupBox()
            Me.btnDocLoadLeft = New System.Windows.Forms.Button()
            Me.btnDocLoadBoth = New System.Windows.Forms.Button()
            Me.btnDocLoadRight = New System.Windows.Forms.Button()
            Me.grpClearText = New System.Windows.Forms.GroupBox()
            Me.btnClearTextLeft = New System.Windows.Forms.Button()
            Me.btnClearTextBoth = New System.Windows.Forms.Button()
            Me.btnClearTextRight = New System.Windows.Forms.Button()
            Me.btnBothTextScreens = New System.Windows.Forms.Button()
            Me.Label11 = New System.Windows.Forms.Label()
            Me.Label10 = New System.Windows.Forms.Label()
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
            Me.tpImgSearch = New System.Windows.Forms.TabPage()
            Me.Label32 = New System.Windows.Forms.Label()
            Me.Label21 = New System.Windows.Forms.Label()
            Me.lbImgResults = New System.Windows.Forms.ListBox()
            Me.comboImgSearchText = New System.Windows.Forms.ComboBox()
            Me.Label20 = New System.Windows.Forms.Label()
            Me.btnImgSearch = New System.Windows.Forms.Button()
            Me.btnSearchImgAddSlide = New System.Windows.Forms.Button()
            Me.btnSearchImgShowRight = New System.Windows.Forms.Button()
            Me.btnSearchImgShowBoth = New System.Windows.Forms.Button()
            Me.btnSearchImgShowLeft = New System.Windows.Forms.Button()
            Me.Label19 = New System.Windows.Forms.Label()
            Me.picImgSearchPreview = New System.Windows.Forms.PictureBox()
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
            Me.ExpTree1 = New ExpTreeLib.ExpTree()
            Me.btnWhammy = New System.Windows.Forms.Button()
            Me.btnPauseSlides = New System.Windows.Forms.Button()
            Me.btnNextSlide = New System.Windows.Forms.Button()
            Me.btnPrevSlide = New System.Windows.Forms.Button()
            Me.btnClearSlideList = New System.Windows.Forms.Button()
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
            Me.tpPrefs = New System.Windows.Forms.TabPage()
            Me.cbShadowsEnabled = New System.Windows.Forms.CheckBox()
            Me.Label43 = New System.Windows.Forms.Label()
            Me.Label38 = New System.Windows.Forms.Label()
            Me.Label39 = New System.Windows.Forms.Label()
            Me.Label40 = New System.Windows.Forms.Label()
            Me.nudDefaultCountdownSeconds = New System.Windows.Forms.NumericUpDown()
            Me.nudDefaultCountdownMinutes = New System.Windows.Forms.NumericUpDown()
            Me.nudDefaultCountdownHours = New System.Windows.Forms.NumericUpDown()
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
            Me.pnlPicBackRight = New System.Windows.Forms.Panel()
            Me.picRight = New System.Windows.Forms.PictureBox()
            Me.CountdownTimer = New System.Windows.Forms.Timer(Me.components)
            Me.cbExpandPicLeft = New System.Windows.Forms.CheckBox()
            Me.cbExpandPicRight = New System.Windows.Forms.CheckBox()
            Me.lblLibraryCount = New System.Windows.Forms.Label()
            Me.btnPasteBoth = New System.Windows.Forms.Button()
            Me.btnPasteRight = New System.Windows.Forms.Button()
            Me.btnPasteLeft = New System.Windows.Forms.Button()
            Me.grpLoadImage = New System.Windows.Forms.GroupBox()
            Me.grpPasteImage = New System.Windows.Forms.GroupBox()
            Me.gbCountdownControls = New System.Windows.Forms.GroupBox()
            Me.Label37 = New System.Windows.Forms.Label()
            Me.Label36 = New System.Windows.Forms.Label()
            Me.Label35 = New System.Windows.Forms.Label()
            Me.Label34 = New System.Windows.Forms.Label()
            Me.nudCountdownWarnSeconds = New System.Windows.Forms.NumericUpDown()
            Me.nudCountdownWarnMinutes = New System.Windows.Forms.NumericUpDown()
            Me.nudCountdownWarnHours = New System.Windows.Forms.NumericUpDown()
            Me.Label33 = New System.Windows.Forms.Label()
            Me.cbCountdownVisible = New System.Windows.Forms.CheckBox()
            Me.nudCountdownSeconds = New System.Windows.Forms.NumericUpDown()
            Me.nudCountdownMinutes = New System.Windows.Forms.NumericUpDown()
            Me.nudCountdownHours = New System.Windows.Forms.NumericUpDown()
            Me.btnResetCountdown = New System.Windows.Forms.Button()
            Me.btnStartCountdown = New System.Windows.Forms.Button()
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.btnReIndexImgLib = New System.Windows.Forms.Button()
            CType(Me.picLeft, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabControl1.SuspendLayout()
            Me.tpScreenText.SuspendLayout()
            Me.GroupBox1.SuspendLayout()
            Me.grpShowLeftText.SuspendLayout()
            Me.grpLoadText.SuspendLayout()
            Me.grpClearText.SuspendLayout()
            Me.grpRightColors.SuspendLayout()
            Me.grpLeftColors.SuspendLayout()
            Me.tpImgSearch.SuspendLayout()
            CType(Me.picImgSearchPreview, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tp5Things.SuspendLayout()
            Me.grpThingsColor.SuspendLayout()
            Me.tpSlides.SuspendLayout()
            CType(Me.nudDelay, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picSlidePreview, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpHotButtons.SuspendLayout()
            Me.gbHB.SuspendLayout()
            Me.tpPrefs.SuspendLayout()
            CType(Me.nudDefaultCountdownSeconds, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudDefaultCountdownMinutes, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudDefaultCountdownHours, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.GroupBox2.SuspendLayout()
            Me.grpDefaultColorsRight.SuspendLayout()
            Me.grpDefaultColorsLeft.SuspendLayout()
            CType(Me.nudDefaultSlideDelay, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpAbout.SuspendLayout()
            Me.pnlPicBackLeft.SuspendLayout()
            Me.pnlPicBackRight.SuspendLayout()
            CType(Me.picRight, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpLoadImage.SuspendLayout()
            Me.grpPasteImage.SuspendLayout()
            Me.gbCountdownControls.SuspendLayout()
            CType(Me.nudCountdownWarnSeconds, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudCountdownWarnMinutes, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudCountdownWarnHours, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudCountdownSeconds, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudCountdownMinutes, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudCountdownHours, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnBlackout
            '
            Me.btnBlackout.BackColor = System.Drawing.SystemColors.Control
            Me.btnBlackout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnBlackout.ForeColor = System.Drawing.SystemColors.WindowText
            Me.btnBlackout.Location = New System.Drawing.Point(442, 104)
            Me.btnBlackout.Name = "btnBlackout"
            Me.btnBlackout.Size = New System.Drawing.Size(112, 40)
            Me.btnBlackout.TabIndex = 13
            Me.btnBlackout.Text = " &BLACKOUT"
            Me.btnBlackout.UseVisualStyleBackColor = True
            '
            'tbLeftTeam
            '
            Me.tbLeftTeam.BackColor = System.Drawing.SystemColors.Window
            Me.tbLeftTeam.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbLeftTeam.ForeColor = System.Drawing.SystemColors.WindowText
            Me.tbLeftTeam.Location = New System.Drawing.Point(88, 4)
            Me.tbLeftTeam.MaxLength = 26
            Me.tbLeftTeam.Name = "tbLeftTeam"
            Me.tbLeftTeam.Size = New System.Drawing.Size(188, 22)
            Me.tbLeftTeam.TabIndex = 1
            Me.tbLeftTeam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbRightTeam
            '
            Me.tbRightTeam.BackColor = System.Drawing.SystemColors.Window
            Me.tbRightTeam.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbRightTeam.ForeColor = System.Drawing.SystemColors.WindowText
            Me.tbRightTeam.Location = New System.Drawing.Point(720, 4)
            Me.tbRightTeam.MaxLength = 26
            Me.tbRightTeam.Name = "tbRightTeam"
            Me.tbRightTeam.Size = New System.Drawing.Size(188, 22)
            Me.tbRightTeam.TabIndex = 8
            Me.tbRightTeam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.Transparent
            Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
            Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label2.Location = New System.Drawing.Point(916, 4)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(80, 24)
            Me.Label2.TabIndex = 9
            Me.Label2.Text = "Team Name"
            Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'tbLeftScore
            '
            Me.tbLeftScore.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.tbLeftScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbLeftScore.ForeColor = System.Drawing.Color.White
            Me.tbLeftScore.Location = New System.Drawing.Point(88, 30)
            Me.tbLeftScore.MaxLength = 3
            Me.tbLeftScore.Name = "tbLeftScore"
            Me.tbLeftScore.Size = New System.Drawing.Size(60, 24)
            Me.tbLeftScore.TabIndex = 3
            Me.tbLeftScore.Text = "0"
            Me.tbLeftScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbRightScore
            '
            Me.tbRightScore.BackColor = System.Drawing.Color.Maroon
            Me.tbRightScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbRightScore.ForeColor = System.Drawing.Color.White
            Me.tbRightScore.Location = New System.Drawing.Point(848, 30)
            Me.tbRightScore.MaxLength = 3
            Me.tbRightScore.Name = "tbRightScore"
            Me.tbRightScore.Size = New System.Drawing.Size(60, 24)
            Me.tbRightScore.TabIndex = 11
            Me.tbRightScore.Text = "0"
            Me.tbRightScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'Label3
            '
            Me.Label3.BackColor = System.Drawing.Color.Transparent
            Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label3.Location = New System.Drawing.Point(28, 29)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(56, 24)
            Me.Label3.TabIndex = 2
            Me.Label3.Text = "Score"
            Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'Label4
            '
            Me.Label4.BackColor = System.Drawing.Color.Transparent
            Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label4.Location = New System.Drawing.Point(916, 29)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(56, 24)
            Me.Label4.TabIndex = 12
            Me.Label4.Text = "Score"
            Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'btnScoreLeft
            '
            Me.btnScoreLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnScoreLeft.Location = New System.Drawing.Point(343, 2)
            Me.btnScoreLeft.Name = "btnScoreLeft"
            Me.btnScoreLeft.Size = New System.Drawing.Size(76, 40)
            Me.btnScoreLeft.TabIndex = 5
            Me.btnScoreLeft.Text = "SCORE LEFT"
            '
            'btnScoreRight
            '
            Me.btnScoreRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnScoreRight.Location = New System.Drawing.Point(577, 2)
            Me.btnScoreRight.Name = "btnScoreRight"
            Me.btnScoreRight.Size = New System.Drawing.Size(76, 40)
            Me.btnScoreRight.TabIndex = 7
            Me.btnScoreRight.Text = "SCORE RIGHT"
            '
            'btnScoreBoth
            '
            Me.btnScoreBoth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnScoreBoth.Location = New System.Drawing.Point(457, 2)
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
            Me.menuDummy.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuAdd1Left, Me.menuSubtract1Left, Me.menuAdd5Left, Me.menuSubtract5Left, Me.menuAdd1Right, Me.menuSubtract1Right, Me.menuAdd5Right, Me.menuSubtract5Right, Me.EasterEgg1})
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
            'picLeft
            '
            Me.picLeft.BackColor = System.Drawing.Color.Transparent
            Me.picLeft.ImageLocation = ""
            Me.picLeft.Location = New System.Drawing.Point(0, 0)
            Me.picLeft.Name = "picLeft"
            Me.picLeft.Size = New System.Drawing.Size(272, 153)
            Me.picLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.picLeft.TabIndex = 28
            Me.picLeft.TabStop = False
            '
            'btnPictureLeft
            '
            Me.btnPictureLeft.AllowDrop = True
            Me.btnPictureLeft.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnPictureLeft.Location = New System.Drawing.Point(2, 18)
            Me.btnPictureLeft.Name = "btnPictureLeft"
            Me.btnPictureLeft.Size = New System.Drawing.Size(47, 28)
            Me.btnPictureLeft.TabIndex = 14
            Me.btnPictureLeft.Text = "ï"
            Me.btnPictureLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'btnPictureRight
            '
            Me.btnPictureRight.AllowDrop = True
            Me.btnPictureRight.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnPictureRight.Location = New System.Drawing.Point(98, 18)
            Me.btnPictureRight.Name = "btnPictureRight"
            Me.btnPictureRight.Size = New System.Drawing.Size(47, 28)
            Me.btnPictureRight.TabIndex = 16
            Me.btnPictureRight.Text = "ð"
            Me.btnPictureRight.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'btnPictureBoth
            '
            Me.btnPictureBoth.AllowDrop = True
            Me.btnPictureBoth.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnPictureBoth.Location = New System.Drawing.Point(50, 18)
            Me.btnPictureBoth.Name = "btnPictureBoth"
            Me.btnPictureBoth.Size = New System.Drawing.Size(47, 28)
            Me.btnPictureBoth.TabIndex = 15
            Me.btnPictureBoth.Text = "ó"
            Me.btnPictureBoth.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'btnLeftScoreColor
            '
            Me.btnLeftScoreColor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnLeftScoreColor.Location = New System.Drawing.Point(176, 29)
            Me.btnLeftScoreColor.Name = "btnLeftScoreColor"
            Me.btnLeftScoreColor.Size = New System.Drawing.Size(100, 24)
            Me.btnLeftScoreColor.TabIndex = 4
            Me.btnLeftScoreColor.Text = "Color..."
            '
            'btnRightScoreColor
            '
            Me.btnRightScoreColor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnRightScoreColor.Location = New System.Drawing.Point(720, 29)
            Me.btnRightScoreColor.Name = "btnRightScoreColor"
            Me.btnRightScoreColor.Size = New System.Drawing.Size(100, 24)
            Me.btnRightScoreColor.TabIndex = 10
            Me.btnRightScoreColor.Text = "Color..."
            '
            'TabControl1
            '
            Me.TabControl1.Controls.Add(Me.tpScreenText)
            Me.TabControl1.Controls.Add(Me.tpImgSearch)
            Me.TabControl1.Controls.Add(Me.tp5Things)
            Me.TabControl1.Controls.Add(Me.tpSlides)
            Me.TabControl1.Controls.Add(Me.tpHotButtons)
            Me.TabControl1.Controls.Add(Me.tpPrefs)
            Me.TabControl1.Controls.Add(Me.tpAbout)
            Me.TabControl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.TabControl1.HotTrack = True
            Me.TabControl1.ItemSize = New System.Drawing.Size(120, 24)
            Me.TabControl1.Location = New System.Drawing.Point(0, 298)
            Me.TabControl1.Name = "TabControl1"
            Me.TabControl1.SelectedIndex = 0
            Me.TabControl1.Size = New System.Drawing.Size(996, 404)
            Me.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
            Me.TabControl1.TabIndex = 17
            '
            'tpScreenText
            '
            Me.tpScreenText.BackColor = System.Drawing.Color.Transparent
            Me.tpScreenText.Controls.Add(Me.GroupBox1)
            Me.tpScreenText.Controls.Add(Me.grpShowLeftText)
            Me.tpScreenText.Controls.Add(Me.grpLoadText)
            Me.tpScreenText.Controls.Add(Me.grpClearText)
            Me.tpScreenText.Controls.Add(Me.btnBothTextScreens)
            Me.tpScreenText.Controls.Add(Me.Label11)
            Me.tpScreenText.Controls.Add(Me.Label10)
            Me.tpScreenText.Controls.Add(Me.tbLeftText)
            Me.tpScreenText.Controls.Add(Me.tbRightFontSize)
            Me.tpScreenText.Controls.Add(Me.grpRightColors)
            Me.tpScreenText.Controls.Add(Me.tbRightText)
            Me.tpScreenText.Controls.Add(Me.tbLeftFontSize)
            Me.tpScreenText.Controls.Add(Me.grpLeftColors)
            Me.tpScreenText.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tpScreenText.Location = New System.Drawing.Point(4, 28)
            Me.tpScreenText.Name = "tpScreenText"
            Me.tpScreenText.Size = New System.Drawing.Size(988, 372)
            Me.tpScreenText.TabIndex = 0
            Me.tpScreenText.Text = "Text Display"
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.btnRightTextLeft)
            Me.GroupBox1.Controls.Add(Me.btnRightTextBoth)
            Me.GroupBox1.Controls.Add(Me.btnShowRightText)
            Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.GroupBox1.Location = New System.Drawing.Point(822, 300)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.GroupBox1.Size = New System.Drawing.Size(148, 52)
            Me.GroupBox1.TabIndex = 61
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "SHOW THIS TEXT"
            '
            'btnRightTextLeft
            '
            Me.btnRightTextLeft.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnRightTextLeft.Location = New System.Drawing.Point(2, 18)
            Me.btnRightTextLeft.Name = "btnRightTextLeft"
            Me.btnRightTextLeft.Size = New System.Drawing.Size(47, 28)
            Me.btnRightTextLeft.TabIndex = 48
            Me.btnRightTextLeft.Text = "ï"
            Me.btnRightTextLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'btnRightTextBoth
            '
            Me.btnRightTextBoth.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnRightTextBoth.Location = New System.Drawing.Point(50, 18)
            Me.btnRightTextBoth.Name = "btnRightTextBoth"
            Me.btnRightTextBoth.Size = New System.Drawing.Size(47, 28)
            Me.btnRightTextBoth.TabIndex = 46
            Me.btnRightTextBoth.Text = "ó"
            Me.btnRightTextBoth.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'btnShowRightText
            '
            Me.btnShowRightText.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnShowRightText.Location = New System.Drawing.Point(98, 18)
            Me.btnShowRightText.Name = "btnShowRightText"
            Me.btnShowRightText.Size = New System.Drawing.Size(47, 28)
            Me.btnShowRightText.TabIndex = 45
            Me.btnShowRightText.Text = "ð"
            Me.btnShowRightText.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'grpShowLeftText
            '
            Me.grpShowLeftText.Controls.Add(Me.btnShowLeftText)
            Me.grpShowLeftText.Controls.Add(Me.btnLeftTextBoth)
            Me.grpShowLeftText.Controls.Add(Me.btnLeftTextRight)
            Me.grpShowLeftText.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpShowLeftText.Location = New System.Drawing.Point(27, 300)
            Me.grpShowLeftText.Name = "grpShowLeftText"
            Me.grpShowLeftText.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.grpShowLeftText.Size = New System.Drawing.Size(148, 52)
            Me.grpShowLeftText.TabIndex = 60
            Me.grpShowLeftText.TabStop = False
            Me.grpShowLeftText.Text = "SHOW THIS TEXT"
            '
            'btnShowLeftText
            '
            Me.btnShowLeftText.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnShowLeftText.Location = New System.Drawing.Point(2, 18)
            Me.btnShowLeftText.Name = "btnShowLeftText"
            Me.btnShowLeftText.Size = New System.Drawing.Size(47, 28)
            Me.btnShowLeftText.TabIndex = 41
            Me.btnShowLeftText.Text = "ï"
            Me.btnShowLeftText.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'btnLeftTextBoth
            '
            Me.btnLeftTextBoth.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnLeftTextBoth.Location = New System.Drawing.Point(50, 18)
            Me.btnLeftTextBoth.Name = "btnLeftTextBoth"
            Me.btnLeftTextBoth.Size = New System.Drawing.Size(47, 28)
            Me.btnLeftTextBoth.TabIndex = 42
            Me.btnLeftTextBoth.Text = "ó"
            Me.btnLeftTextBoth.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'btnLeftTextRight
            '
            Me.btnLeftTextRight.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnLeftTextRight.Location = New System.Drawing.Point(98, 18)
            Me.btnLeftTextRight.Name = "btnLeftTextRight"
            Me.btnLeftTextRight.Size = New System.Drawing.Size(47, 28)
            Me.btnLeftTextRight.TabIndex = 49
            Me.btnLeftTextRight.Text = "ð"
            Me.btnLeftTextRight.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'grpLoadText
            '
            Me.grpLoadText.Controls.Add(Me.btnDocLoadLeft)
            Me.grpLoadText.Controls.Add(Me.btnDocLoadBoth)
            Me.grpLoadText.Controls.Add(Me.btnDocLoadRight)
            Me.grpLoadText.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpLoadText.Location = New System.Drawing.Point(424, 197)
            Me.grpLoadText.Name = "grpLoadText"
            Me.grpLoadText.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.grpLoadText.Size = New System.Drawing.Size(148, 52)
            Me.grpLoadText.TabIndex = 59
            Me.grpLoadText.TabStop = False
            Me.grpLoadText.Text = "Load Text File"
            '
            'btnDocLoadLeft
            '
            Me.btnDocLoadLeft.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnDocLoadLeft.Location = New System.Drawing.Point(2, 18)
            Me.btnDocLoadLeft.Name = "btnDocLoadLeft"
            Me.btnDocLoadLeft.Size = New System.Drawing.Size(47, 28)
            Me.btnDocLoadLeft.TabIndex = 43
            Me.btnDocLoadLeft.Tag = "Left"
            Me.btnDocLoadLeft.Text = "ï"
            Me.btnDocLoadLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'btnDocLoadBoth
            '
            Me.btnDocLoadBoth.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnDocLoadBoth.Location = New System.Drawing.Point(50, 18)
            Me.btnDocLoadBoth.Name = "btnDocLoadBoth"
            Me.btnDocLoadBoth.Size = New System.Drawing.Size(47, 28)
            Me.btnDocLoadBoth.TabIndex = 44
            Me.btnDocLoadBoth.Tag = "Both"
            Me.btnDocLoadBoth.Text = "ó"
            Me.btnDocLoadBoth.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'btnDocLoadRight
            '
            Me.btnDocLoadRight.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnDocLoadRight.Location = New System.Drawing.Point(98, 18)
            Me.btnDocLoadRight.Name = "btnDocLoadRight"
            Me.btnDocLoadRight.Size = New System.Drawing.Size(47, 28)
            Me.btnDocLoadRight.TabIndex = 47
            Me.btnDocLoadRight.Tag = "Right"
            Me.btnDocLoadRight.Text = "ð"
            Me.btnDocLoadRight.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'grpClearText
            '
            Me.grpClearText.Controls.Add(Me.btnClearTextLeft)
            Me.grpClearText.Controls.Add(Me.btnClearTextBoth)
            Me.grpClearText.Controls.Add(Me.btnClearTextRight)
            Me.grpClearText.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpClearText.Location = New System.Drawing.Point(424, 98)
            Me.grpClearText.Name = "grpClearText"
            Me.grpClearText.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.grpClearText.Size = New System.Drawing.Size(148, 52)
            Me.grpClearText.TabIndex = 58
            Me.grpClearText.TabStop = False
            Me.grpClearText.Text = "Clear Text"
            '
            'btnClearTextLeft
            '
            Me.btnClearTextLeft.BackColor = System.Drawing.Color.Transparent
            Me.btnClearTextLeft.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnClearTextLeft.ForeColor = System.Drawing.SystemColors.WindowText
            Me.btnClearTextLeft.Location = New System.Drawing.Point(2, 18)
            Me.btnClearTextLeft.Name = "btnClearTextLeft"
            Me.btnClearTextLeft.Size = New System.Drawing.Size(47, 28)
            Me.btnClearTextLeft.TabIndex = 53
            Me.btnClearTextLeft.Text = "ï"
            Me.btnClearTextLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.btnClearTextLeft.UseVisualStyleBackColor = False
            '
            'btnClearTextBoth
            '
            Me.btnClearTextBoth.BackColor = System.Drawing.Color.Transparent
            Me.btnClearTextBoth.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnClearTextBoth.ForeColor = System.Drawing.SystemColors.WindowText
            Me.btnClearTextBoth.Location = New System.Drawing.Point(50, 18)
            Me.btnClearTextBoth.Name = "btnClearTextBoth"
            Me.btnClearTextBoth.Size = New System.Drawing.Size(47, 28)
            Me.btnClearTextBoth.TabIndex = 56
            Me.btnClearTextBoth.Text = "ó"
            Me.btnClearTextBoth.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.btnClearTextBoth.UseVisualStyleBackColor = False
            '
            'btnClearTextRight
            '
            Me.btnClearTextRight.BackColor = System.Drawing.Color.Transparent
            Me.btnClearTextRight.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnClearTextRight.ForeColor = System.Drawing.SystemColors.WindowText
            Me.btnClearTextRight.Location = New System.Drawing.Point(98, 18)
            Me.btnClearTextRight.Name = "btnClearTextRight"
            Me.btnClearTextRight.Size = New System.Drawing.Size(47, 28)
            Me.btnClearTextRight.TabIndex = 54
            Me.btnClearTextRight.Text = "ð"
            Me.btnClearTextRight.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.btnClearTextRight.UseVisualStyleBackColor = False
            '
            'btnBothTextScreens
            '
            Me.btnBothTextScreens.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnBothTextScreens.Location = New System.Drawing.Point(444, 300)
            Me.btnBothTextScreens.Name = "btnBothTextScreens"
            Me.btnBothTextScreens.Size = New System.Drawing.Size(108, 44)
            Me.btnBothTextScreens.TabIndex = 39
            Me.btnBothTextScreens.Tag = "Both"
            Me.btnBothTextScreens.Text = "SIMUL-SHOW BOTH"
            '
            'Label11
            '
            Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label11.Location = New System.Drawing.Point(902, 2)
            Me.Label11.Name = "Label11"
            Me.Label11.Size = New System.Drawing.Size(65, 20)
            Me.Label11.TabIndex = 28
            Me.Label11.Text = "Font Size"
            Me.Label11.TextAlign = System.Drawing.ContentAlignment.BottomCenter
            '
            'Label10
            '
            Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label10.Location = New System.Drawing.Point(25, 2)
            Me.Label10.Name = "Label10"
            Me.Label10.Size = New System.Drawing.Size(64, 23)
            Me.Label10.TabIndex = 18
            Me.Label10.Text = "Font Size"
            Me.Label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter
            '
            'tbLeftText
            '
            Me.tbLeftText.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.tbLeftText.Font = New System.Drawing.Font("Arial", 20.0!, System.Drawing.FontStyle.Bold)
            Me.tbLeftText.ForeColor = System.Drawing.Color.White
            Me.tbLeftText.Location = New System.Drawing.Point(2, 60)
            Me.tbLeftText.Multiline = True
            Me.tbLeftText.Name = "tbLeftText"
            Me.tbLeftText.Size = New System.Drawing.Size(400, 225)
            Me.tbLeftText.TabIndex = 38
            Me.tbLeftText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbRightFontSize
            '
            Me.tbRightFontSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbRightFontSize.Location = New System.Drawing.Point(905, 27)
            Me.tbRightFontSize.MaxLength = 3
            Me.tbRightFontSize.Name = "tbRightFontSize"
            Me.tbRightFontSize.Size = New System.Drawing.Size(56, 24)
            Me.tbRightFontSize.TabIndex = 29
            Me.tbRightFontSize.Text = "60"
            Me.tbRightFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'grpRightColors
            '
            Me.grpRightColors.BackColor = System.Drawing.Color.Transparent
            Me.grpRightColors.Controls.Add(Me.btnChooseTextColorRight)
            Me.grpRightColors.Controls.Add(Me.pnlTextColorRight6)
            Me.grpRightColors.Controls.Add(Me.pnlTextColorRight5)
            Me.grpRightColors.Controls.Add(Me.pnlTextColorRight4)
            Me.grpRightColors.Controls.Add(Me.pnlTextColorRight3)
            Me.grpRightColors.Controls.Add(Me.pnlTextColorRight2)
            Me.grpRightColors.Controls.Add(Me.pnlTextColorRight1)
            Me.grpRightColors.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpRightColors.Location = New System.Drawing.Point(608, 10)
            Me.grpRightColors.Name = "grpRightColors"
            Me.grpRightColors.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.grpRightColors.Size = New System.Drawing.Size(244, 44)
            Me.grpRightColors.TabIndex = 30
            Me.grpRightColors.TabStop = False
            Me.grpRightColors.Text = "Background"
            '
            'btnChooseTextColorRight
            '
            Me.btnChooseTextColorRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnChooseTextColorRight.Location = New System.Drawing.Point(10, 13)
            Me.btnChooseTextColorRight.Name = "btnChooseTextColorRight"
            Me.btnChooseTextColorRight.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.btnChooseTextColorRight.Size = New System.Drawing.Size(78, 26)
            Me.btnChooseTextColorRight.TabIndex = 37
            Me.btnChooseTextColorRight.Text = "Other..."
            '
            'pnlTextColorRight6
            '
            Me.pnlTextColorRight6.BackColor = System.Drawing.Color.Black
            Me.pnlTextColorRight6.Location = New System.Drawing.Point(219, 18)
            Me.pnlTextColorRight6.Name = "pnlTextColorRight6"
            Me.pnlTextColorRight6.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorRight6.TabIndex = 36
            Me.pnlTextColorRight6.TabStop = True
            '
            'pnlTextColorRight5
            '
            Me.pnlTextColorRight5.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlTextColorRight5.Location = New System.Drawing.Point(195, 18)
            Me.pnlTextColorRight5.Name = "pnlTextColorRight5"
            Me.pnlTextColorRight5.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorRight5.TabIndex = 35
            Me.pnlTextColorRight5.TabStop = True
            '
            'pnlTextColorRight4
            '
            Me.pnlTextColorRight4.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.pnlTextColorRight4.Location = New System.Drawing.Point(171, 18)
            Me.pnlTextColorRight4.Name = "pnlTextColorRight4"
            Me.pnlTextColorRight4.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorRight4.TabIndex = 34
            Me.pnlTextColorRight4.TabStop = True
            '
            'pnlTextColorRight3
            '
            Me.pnlTextColorRight3.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlTextColorRight3.Location = New System.Drawing.Point(147, 18)
            Me.pnlTextColorRight3.Name = "pnlTextColorRight3"
            Me.pnlTextColorRight3.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorRight3.TabIndex = 33
            Me.pnlTextColorRight3.TabStop = True
            '
            'pnlTextColorRight2
            '
            Me.pnlTextColorRight2.BackColor = System.Drawing.Color.Maroon
            Me.pnlTextColorRight2.Location = New System.Drawing.Point(123, 18)
            Me.pnlTextColorRight2.Name = "pnlTextColorRight2"
            Me.pnlTextColorRight2.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorRight2.TabIndex = 32
            Me.pnlTextColorRight2.TabStop = True
            '
            'pnlTextColorRight1
            '
            Me.pnlTextColorRight1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.pnlTextColorRight1.Location = New System.Drawing.Point(99, 18)
            Me.pnlTextColorRight1.Name = "pnlTextColorRight1"
            Me.pnlTextColorRight1.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorRight1.TabIndex = 31
            Me.pnlTextColorRight1.TabStop = True
            '
            'tbRightText
            '
            Me.tbRightText.BackColor = System.Drawing.Color.Maroon
            Me.tbRightText.Font = New System.Drawing.Font("Arial", 20.0!, System.Drawing.FontStyle.Bold)
            Me.tbRightText.ForeColor = System.Drawing.Color.White
            Me.tbRightText.Location = New System.Drawing.Point(591, 60)
            Me.tbRightText.Multiline = True
            Me.tbRightText.Name = "tbRightText"
            Me.tbRightText.Size = New System.Drawing.Size(400, 225)
            Me.tbRightText.TabIndex = 40
            Me.tbRightText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbLeftFontSize
            '
            Me.tbLeftFontSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbLeftFontSize.Location = New System.Drawing.Point(28, 27)
            Me.tbLeftFontSize.MaxLength = 3
            Me.tbLeftFontSize.Name = "tbLeftFontSize"
            Me.tbLeftFontSize.Size = New System.Drawing.Size(56, 24)
            Me.tbLeftFontSize.TabIndex = 19
            Me.tbLeftFontSize.Text = "60"
            Me.tbLeftFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'grpLeftColors
            '
            Me.grpLeftColors.BackColor = System.Drawing.Color.Transparent
            Me.grpLeftColors.Controls.Add(Me.btnChooseTextColorLeft)
            Me.grpLeftColors.Controls.Add(Me.pnlTextColorLeft6)
            Me.grpLeftColors.Controls.Add(Me.pnlTextColorLeft5)
            Me.grpLeftColors.Controls.Add(Me.pnlTextColorLeft4)
            Me.grpLeftColors.Controls.Add(Me.pnlTextColorLeft3)
            Me.grpLeftColors.Controls.Add(Me.pnlTextColorLeft2)
            Me.grpLeftColors.Controls.Add(Me.pnlTextColorLeft1)
            Me.grpLeftColors.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpLeftColors.Location = New System.Drawing.Point(135, 10)
            Me.grpLeftColors.Name = "grpLeftColors"
            Me.grpLeftColors.Size = New System.Drawing.Size(244, 44)
            Me.grpLeftColors.TabIndex = 20
            Me.grpLeftColors.TabStop = False
            Me.grpLeftColors.Text = "Background"
            '
            'btnChooseTextColorLeft
            '
            Me.btnChooseTextColorLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnChooseTextColorLeft.Location = New System.Drawing.Point(156, 13)
            Me.btnChooseTextColorLeft.Name = "btnChooseTextColorLeft"
            Me.btnChooseTextColorLeft.Size = New System.Drawing.Size(78, 26)
            Me.btnChooseTextColorLeft.TabIndex = 27
            Me.btnChooseTextColorLeft.Text = "Other..."
            '
            'pnlTextColorLeft6
            '
            Me.pnlTextColorLeft6.BackColor = System.Drawing.Color.Black
            Me.pnlTextColorLeft6.Location = New System.Drawing.Point(129, 18)
            Me.pnlTextColorLeft6.Name = "pnlTextColorLeft6"
            Me.pnlTextColorLeft6.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorLeft6.TabIndex = 26
            Me.pnlTextColorLeft6.TabStop = True
            '
            'pnlTextColorLeft5
            '
            Me.pnlTextColorLeft5.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlTextColorLeft5.Location = New System.Drawing.Point(105, 18)
            Me.pnlTextColorLeft5.Name = "pnlTextColorLeft5"
            Me.pnlTextColorLeft5.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorLeft5.TabIndex = 25
            Me.pnlTextColorLeft5.TabStop = True
            '
            'pnlTextColorLeft4
            '
            Me.pnlTextColorLeft4.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.pnlTextColorLeft4.Location = New System.Drawing.Point(81, 18)
            Me.pnlTextColorLeft4.Name = "pnlTextColorLeft4"
            Me.pnlTextColorLeft4.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorLeft4.TabIndex = 24
            Me.pnlTextColorLeft4.TabStop = True
            '
            'pnlTextColorLeft3
            '
            Me.pnlTextColorLeft3.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlTextColorLeft3.Location = New System.Drawing.Point(57, 18)
            Me.pnlTextColorLeft3.Name = "pnlTextColorLeft3"
            Me.pnlTextColorLeft3.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorLeft3.TabIndex = 23
            Me.pnlTextColorLeft3.TabStop = True
            '
            'pnlTextColorLeft2
            '
            Me.pnlTextColorLeft2.BackColor = System.Drawing.Color.Maroon
            Me.pnlTextColorLeft2.Location = New System.Drawing.Point(33, 18)
            Me.pnlTextColorLeft2.Name = "pnlTextColorLeft2"
            Me.pnlTextColorLeft2.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorLeft2.TabIndex = 22
            Me.pnlTextColorLeft2.TabStop = True
            '
            'pnlTextColorLeft1
            '
            Me.pnlTextColorLeft1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.pnlTextColorLeft1.Location = New System.Drawing.Point(9, 18)
            Me.pnlTextColorLeft1.Name = "pnlTextColorLeft1"
            Me.pnlTextColorLeft1.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorLeft1.TabIndex = 21
            Me.pnlTextColorLeft1.TabStop = True
            '
            'tpImgSearch
            '
            Me.tpImgSearch.Controls.Add(Me.Label32)
            Me.tpImgSearch.Controls.Add(Me.Label21)
            Me.tpImgSearch.Controls.Add(Me.lbImgResults)
            Me.tpImgSearch.Controls.Add(Me.comboImgSearchText)
            Me.tpImgSearch.Controls.Add(Me.Label20)
            Me.tpImgSearch.Controls.Add(Me.btnImgSearch)
            Me.tpImgSearch.Controls.Add(Me.btnSearchImgAddSlide)
            Me.tpImgSearch.Controls.Add(Me.btnSearchImgShowRight)
            Me.tpImgSearch.Controls.Add(Me.btnSearchImgShowBoth)
            Me.tpImgSearch.Controls.Add(Me.btnSearchImgShowLeft)
            Me.tpImgSearch.Controls.Add(Me.Label19)
            Me.tpImgSearch.Controls.Add(Me.picImgSearchPreview)
            Me.tpImgSearch.Location = New System.Drawing.Point(4, 28)
            Me.tpImgSearch.Name = "tpImgSearch"
            Me.tpImgSearch.Size = New System.Drawing.Size(988, 372)
            Me.tpImgSearch.TabIndex = 6
            Me.tpImgSearch.Text = "Image Search"
            '
            'Label32
            '
            Me.Label32.BackColor = System.Drawing.Color.LemonChiffon
            Me.Label32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.Label32.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label32.ForeColor = System.Drawing.SystemColors.ControlText
            Me.Label32.Location = New System.Drawing.Point(572, 11)
            Me.Label32.Name = "Label32"
            Me.Label32.Size = New System.Drawing.Size(402, 49)
            Me.Label32.TabIndex = 97
            Me.Label32.Text = "Hint: You can drag this Image Preview to a Hot Button to assign it instantly!"
            Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'Label21
            '
            Me.Label21.BackColor = System.Drawing.Color.Transparent
            Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label21.Location = New System.Drawing.Point(12, 140)
            Me.Label21.Name = "Label21"
            Me.Label21.Size = New System.Drawing.Size(124, 16)
            Me.Label21.TabIndex = 96
            Me.Label21.Text = "Search Results"
            Me.Label21.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'lbImgResults
            '
            Me.lbImgResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lbImgResults.HorizontalScrollbar = True
            Me.lbImgResults.ItemHeight = 18
            Me.lbImgResults.Location = New System.Drawing.Point(12, 162)
            Me.lbImgResults.Name = "lbImgResults"
            Me.lbImgResults.Size = New System.Drawing.Size(962, 202)
            Me.lbImgResults.Sorted = True
            Me.lbImgResults.TabIndex = 95
            '
            'comboImgSearchText
            '
            Me.comboImgSearchText.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.comboImgSearchText.Location = New System.Drawing.Point(15, 76)
            Me.comboImgSearchText.Name = "comboImgSearchText"
            Me.comboImgSearchText.Size = New System.Drawing.Size(293, 26)
            Me.comboImgSearchText.TabIndex = 94
            '
            'Label20
            '
            Me.Label20.BackColor = System.Drawing.Color.Transparent
            Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label20.Location = New System.Drawing.Point(12, 24)
            Me.Label20.Name = "Label20"
            Me.Label20.Size = New System.Drawing.Size(296, 36)
            Me.Label20.TabIndex = 76
            Me.Label20.Text = "Search for text in the filenames in the Image Search Library (see Preferences):"
            Me.Label20.TextAlign = System.Drawing.ContentAlignment.BottomCenter
            '
            'btnImgSearch
            '
            Me.btnImgSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnImgSearch.Location = New System.Drawing.Point(212, 116)
            Me.btnImgSearch.Name = "btnImgSearch"
            Me.btnImgSearch.Size = New System.Drawing.Size(96, 34)
            Me.btnImgSearch.TabIndex = 75
            Me.btnImgSearch.Text = "SEARCH"
            '
            'btnSearchImgAddSlide
            '
            Me.btnSearchImgAddSlide.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSearchImgAddSlide.Location = New System.Drawing.Point(688, 116)
            Me.btnSearchImgAddSlide.Name = "btnSearchImgAddSlide"
            Me.btnSearchImgAddSlide.Size = New System.Drawing.Size(164, 34)
            Me.btnSearchImgAddSlide.TabIndex = 74
            Me.btnSearchImgAddSlide.Text = " ADD TO SLIDESHOW"
            '
            'btnSearchImgShowRight
            '
            Me.btnSearchImgShowRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSearchImgShowRight.Location = New System.Drawing.Point(864, 72)
            Me.btnSearchImgShowRight.Name = "btnSearchImgShowRight"
            Me.btnSearchImgShowRight.Size = New System.Drawing.Size(110, 34)
            Me.btnSearchImgShowRight.TabIndex = 73
            Me.btnSearchImgShowRight.Text = "SHOW RIGHT"
            '
            'btnSearchImgShowBoth
            '
            Me.btnSearchImgShowBoth.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSearchImgShowBoth.Location = New System.Drawing.Point(716, 72)
            Me.btnSearchImgShowBoth.Name = "btnSearchImgShowBoth"
            Me.btnSearchImgShowBoth.Size = New System.Drawing.Size(110, 34)
            Me.btnSearchImgShowBoth.TabIndex = 72
            Me.btnSearchImgShowBoth.Text = "SHOW BOTH"
            '
            'btnSearchImgShowLeft
            '
            Me.btnSearchImgShowLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSearchImgShowLeft.Location = New System.Drawing.Point(572, 72)
            Me.btnSearchImgShowLeft.Name = "btnSearchImgShowLeft"
            Me.btnSearchImgShowLeft.Size = New System.Drawing.Size(110, 34)
            Me.btnSearchImgShowLeft.TabIndex = 71
            Me.btnSearchImgShowLeft.Text = "SHOW LEFT"
            '
            'Label19
            '
            Me.Label19.BackColor = System.Drawing.Color.Transparent
            Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label19.Location = New System.Drawing.Point(313, 0)
            Me.Label19.Name = "Label19"
            Me.Label19.Size = New System.Drawing.Size(124, 21)
            Me.Label19.TabIndex = 70
            Me.Label19.Text = "Image Preview:"
            Me.Label19.TextAlign = System.Drawing.ContentAlignment.BottomCenter
            '
            'picImgSearchPreview
            '
            Me.picImgSearchPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.picImgSearchPreview.Location = New System.Drawing.Point(316, 24)
            Me.picImgSearchPreview.Name = "picImgSearchPreview"
            Me.picImgSearchPreview.Size = New System.Drawing.Size(240, 135)
            Me.picImgSearchPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.picImgSearchPreview.TabIndex = 69
            Me.picImgSearchPreview.TabStop = False
            '
            'tp5Things
            '
            Me.tp5Things.Controls.Add(Me.tbCurrentThing)
            Me.tp5Things.Controls.Add(Me.Label12)
            Me.tp5Things.Controls.Add(Me.clbThings)
            Me.tp5Things.Controls.Add(Me.Label5)
            Me.tp5Things.Controls.Add(Me.grpThingsColor)
            Me.tp5Things.Controls.Add(Me.btnClearThings)
            Me.tp5Things.Controls.Add(Me.btnRemoveThing)
            Me.tp5Things.Controls.Add(Me.btnListRight)
            Me.tp5Things.Controls.Add(Me.btnListBoth)
            Me.tp5Things.Controls.Add(Me.btnListLeft)
            Me.tp5Things.Controls.Add(Me.btnShowThingRight)
            Me.tp5Things.Controls.Add(Me.btnShowThingBoth)
            Me.tp5Things.Controls.Add(Me.btnShowThingLeft)
            Me.tp5Things.Controls.Add(Me.tbSubstitutions)
            Me.tp5Things.Controls.Add(Me.btnAddThing)
            Me.tp5Things.Controls.Add(Me.tbNewThing)
            Me.tp5Things.Controls.Add(Me.btnThingDown)
            Me.tp5Things.Controls.Add(Me.btnThingUp)
            Me.tp5Things.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tp5Things.Location = New System.Drawing.Point(4, 28)
            Me.tp5Things.Name = "tp5Things"
            Me.tp5Things.Size = New System.Drawing.Size(988, 372)
            Me.tp5Things.TabIndex = 1
            Me.tp5Things.Text = "5/6 Things"
            '
            'tbCurrentThing
            '
            Me.tbCurrentThing.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.tbCurrentThing.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbCurrentThing.ForeColor = System.Drawing.Color.White
            Me.tbCurrentThing.Location = New System.Drawing.Point(595, 90)
            Me.tbCurrentThing.Name = "tbCurrentThing"
            Me.tbCurrentThing.Size = New System.Drawing.Size(324, 26)
            Me.tbCurrentThing.TabIndex = 63
            Me.tbCurrentThing.Text = "Substitutions"
            Me.tbCurrentThing.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'Label12
            '
            Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label12.Location = New System.Drawing.Point(68, 8)
            Me.Label12.Name = "Label12"
            Me.Label12.Size = New System.Drawing.Size(212, 24)
            Me.Label12.TabIndex = 48
            Me.Label12.Text = "Name of Thing to Add"
            Me.Label12.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'clbThings
            '
            Me.clbThings.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.clbThings.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.clbThings.ForeColor = System.Drawing.Color.White
            Me.clbThings.Location = New System.Drawing.Point(71, 75)
            Me.clbThings.Name = "clbThings"
            Me.clbThings.Size = New System.Drawing.Size(324, 213)
            Me.clbThings.TabIndex = 51
            Me.clbThings.ThreeDCheckBoxes = True
            '
            'Label5
            '
            Me.Label5.BackColor = System.Drawing.Color.Transparent
            Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label5.Location = New System.Drawing.Point(14, 170)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(56, 20)
            Me.Label5.TabIndex = 53
            Me.Label5.Text = "ORDER"
            Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'grpThingsColor
            '
            Me.grpThingsColor.BackColor = System.Drawing.Color.Transparent
            Me.grpThingsColor.Controls.Add(Me.radioThingColorRight)
            Me.grpThingsColor.Controls.Add(Me.radioThingColorLeft)
            Me.grpThingsColor.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpThingsColor.Location = New System.Drawing.Point(592, 16)
            Me.grpThingsColor.Name = "grpThingsColor"
            Me.grpThingsColor.Size = New System.Drawing.Size(332, 54)
            Me.grpThingsColor.TabIndex = 60
            Me.grpThingsColor.TabStop = False
            Me.grpThingsColor.Text = "Select Currently Playing Team"
            '
            'radioThingColorRight
            '
            Me.radioThingColorRight.BackColor = System.Drawing.Color.Maroon
            Me.radioThingColorRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radioThingColorRight.ForeColor = System.Drawing.Color.White
            Me.radioThingColorRight.Location = New System.Drawing.Point(171, 20)
            Me.radioThingColorRight.Name = "radioThingColorRight"
            Me.radioThingColorRight.Size = New System.Drawing.Size(156, 28)
            Me.radioThingColorRight.TabIndex = 62
            Me.radioThingColorRight.Text = "Right"
            Me.radioThingColorRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            Me.radioThingColorRight.UseVisualStyleBackColor = False
            '
            'radioThingColorLeft
            '
            Me.radioThingColorLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.radioThingColorLeft.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.radioThingColorLeft.Checked = True
            Me.radioThingColorLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.radioThingColorLeft.ForeColor = System.Drawing.Color.White
            Me.radioThingColorLeft.Location = New System.Drawing.Point(3, 20)
            Me.radioThingColorLeft.Name = "radioThingColorLeft"
            Me.radioThingColorLeft.Size = New System.Drawing.Size(156, 28)
            Me.radioThingColorLeft.TabIndex = 61
            Me.radioThingColorLeft.TabStop = True
            Me.radioThingColorLeft.Text = "Left"
            Me.radioThingColorLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            Me.radioThingColorLeft.UseVisualStyleBackColor = False
            '
            'btnClearThings
            '
            Me.btnClearThings.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnClearThings.Location = New System.Drawing.Point(410, 224)
            Me.btnClearThings.Name = "btnClearThings"
            Me.btnClearThings.Size = New System.Drawing.Size(96, 32)
            Me.btnClearThings.TabIndex = 56
            Me.btnClearThings.Text = "CLEAR LIST"
            '
            'btnRemoveThing
            '
            Me.btnRemoveThing.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnRemoveThing.Location = New System.Drawing.Point(410, 137)
            Me.btnRemoveThing.Name = "btnRemoveThing"
            Me.btnRemoveThing.Size = New System.Drawing.Size(96, 44)
            Me.btnRemoveThing.TabIndex = 55
            Me.btnRemoveThing.Text = "REMOVE SELECTION"
            '
            'btnListRight
            '
            Me.btnListRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnListRight.Location = New System.Drawing.Point(299, 300)
            Me.btnListRight.Name = "btnListRight"
            Me.btnListRight.Size = New System.Drawing.Size(96, 44)
            Me.btnListRight.TabIndex = 59
            Me.btnListRight.Text = "LIST RIGHT"
            '
            'btnListBoth
            '
            Me.btnListBoth.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnListBoth.Location = New System.Drawing.Point(185, 300)
            Me.btnListBoth.Name = "btnListBoth"
            Me.btnListBoth.Size = New System.Drawing.Size(96, 44)
            Me.btnListBoth.TabIndex = 58
            Me.btnListBoth.Text = "LIST BOTH"
            '
            'btnListLeft
            '
            Me.btnListLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnListLeft.Location = New System.Drawing.Point(71, 300)
            Me.btnListLeft.Name = "btnListLeft"
            Me.btnListLeft.Size = New System.Drawing.Size(96, 44)
            Me.btnListLeft.TabIndex = 57
            Me.btnListLeft.Text = "LIST LEFT"
            '
            'btnShowThingRight
            '
            Me.btnShowThingRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnShowThingRight.Location = New System.Drawing.Point(823, 300)
            Me.btnShowThingRight.Name = "btnShowThingRight"
            Me.btnShowThingRight.Size = New System.Drawing.Size(96, 44)
            Me.btnShowThingRight.TabIndex = 67
            Me.btnShowThingRight.Text = "SHOW RIGHT"
            '
            'btnShowThingBoth
            '
            Me.btnShowThingBoth.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnShowThingBoth.Location = New System.Drawing.Point(710, 300)
            Me.btnShowThingBoth.Name = "btnShowThingBoth"
            Me.btnShowThingBoth.Size = New System.Drawing.Size(96, 44)
            Me.btnShowThingBoth.TabIndex = 66
            Me.btnShowThingBoth.Text = "SHOW BOTH"
            '
            'btnShowThingLeft
            '
            Me.btnShowThingLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnShowThingLeft.Location = New System.Drawing.Point(595, 300)
            Me.btnShowThingLeft.Name = "btnShowThingLeft"
            Me.btnShowThingLeft.Size = New System.Drawing.Size(96, 44)
            Me.btnShowThingLeft.TabIndex = 65
            Me.btnShowThingLeft.Text = "SHOW LEFT"
            '
            'tbSubstitutions
            '
            Me.tbSubstitutions.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.tbSubstitutions.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbSubstitutions.ForeColor = System.Drawing.Color.White
            Me.tbSubstitutions.Location = New System.Drawing.Point(595, 119)
            Me.tbSubstitutions.Multiline = True
            Me.tbSubstitutions.Name = "tbSubstitutions"
            Me.tbSubstitutions.Size = New System.Drawing.Size(324, 169)
            Me.tbSubstitutions.TabIndex = 64
            Me.tbSubstitutions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'btnAddThing
            '
            Me.btnAddThing.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnAddThing.Location = New System.Drawing.Point(410, 31)
            Me.btnAddThing.Name = "btnAddThing"
            Me.btnAddThing.Size = New System.Drawing.Size(96, 32)
            Me.btnAddThing.TabIndex = 50
            Me.btnAddThing.Text = "ADD THING"
            '
            'tbNewThing
            '
            Me.tbNewThing.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbNewThing.Location = New System.Drawing.Point(68, 35)
            Me.tbNewThing.Name = "tbNewThing"
            Me.tbNewThing.Size = New System.Drawing.Size(327, 23)
            Me.tbNewThing.TabIndex = 49
            '
            'btnThingDown
            '
            Me.btnThingDown.Font = New System.Drawing.Font("Webdings", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnThingDown.Location = New System.Drawing.Point(22, 193)
            Me.btnThingDown.Name = "btnThingDown"
            Me.btnThingDown.Size = New System.Drawing.Size(40, 41)
            Me.btnThingDown.TabIndex = 54
            Me.btnThingDown.Text = "6"
            '
            'btnThingUp
            '
            Me.btnThingUp.Font = New System.Drawing.Font("Webdings", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnThingUp.Location = New System.Drawing.Point(22, 126)
            Me.btnThingUp.Name = "btnThingUp"
            Me.btnThingUp.Size = New System.Drawing.Size(40, 41)
            Me.btnThingUp.TabIndex = 52
            Me.btnThingUp.Text = "5"
            '
            'tpSlides
            '
            Me.tpSlides.Controls.Add(Me.ExpTree1)
            Me.tpSlides.Controls.Add(Me.btnWhammy)
            Me.tpSlides.Controls.Add(Me.btnPauseSlides)
            Me.tpSlides.Controls.Add(Me.btnNextSlide)
            Me.tpSlides.Controls.Add(Me.btnPrevSlide)
            Me.tpSlides.Controls.Add(Me.btnClearSlideList)
            Me.tpSlides.Controls.Add(Me.Label9)
            Me.tpSlides.Controls.Add(Me.nudDelay)
            Me.tpSlides.Controls.Add(Me.Label8)
            Me.tpSlides.Controls.Add(Me.Label7)
            Me.tpSlides.Controls.Add(Me.btnSaveSlides)
            Me.tpSlides.Controls.Add(Me.btnLoadSlides)
            Me.tpSlides.Controls.Add(Me.FileListBox1)
            Me.tpSlides.Controls.Add(Me.btnRemoveSlides)
            Me.tpSlides.Controls.Add(Me.btnAddSlide)
            Me.tpSlides.Controls.Add(Me.Label6)
            Me.tpSlides.Controls.Add(Me.btnSlideDown)
            Me.tpSlides.Controls.Add(Me.btnSlideUp)
            Me.tpSlides.Controls.Add(Me.btnStopSlides)
            Me.tpSlides.Controls.Add(Me.btnLastSlide)
            Me.tpSlides.Controls.Add(Me.btnPlaySlides)
            Me.tpSlides.Controls.Add(Me.btnFirstSlide)
            Me.tpSlides.Controls.Add(Me.lbSlideList)
            Me.tpSlides.Controls.Add(Me.picSlidePreview)
            Me.tpSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tpSlides.Location = New System.Drawing.Point(4, 28)
            Me.tpSlides.Name = "tpSlides"
            Me.tpSlides.Size = New System.Drawing.Size(988, 372)
            Me.tpSlides.TabIndex = 2
            Me.tpSlides.Text = "Slide Show"
            '
            'ExpTree1
            '
            Me.ExpTree1.AllowDrop = True
            Me.ExpTree1.Cursor = System.Windows.Forms.Cursors.Default
            Me.ExpTree1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ExpTree1.Location = New System.Drawing.Point(3, 3)
            Me.ExpTree1.Name = "ExpTree1"
            Me.ExpTree1.ShowRootLines = False
            Me.ExpTree1.Size = New System.Drawing.Size(251, 361)
            Me.ExpTree1.StartUpDirectory = ExpTreeLib.ExpTree.StartDir.Controls
            Me.ExpTree1.TabIndex = 91
            '
            'btnWhammy
            '
            Me.btnWhammy.BackColor = System.Drawing.Color.Yellow
            Me.btnWhammy.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnWhammy.ForeColor = System.Drawing.Color.Blue
            Me.btnWhammy.Location = New System.Drawing.Point(547, 202)
            Me.btnWhammy.Name = "btnWhammy"
            Me.btnWhammy.Size = New System.Drawing.Size(95, 28)
            Me.btnWhammy.TabIndex = 90
            Me.btnWhammy.Text = "WHAMMY!"
            Me.ToolTip1.SetToolTip(Me.btnWhammy, "Launch slideshow as high-speed randomizer")
            Me.btnWhammy.UseVisualStyleBackColor = False
            '
            'btnPauseSlides
            '
            Me.btnPauseSlides.Font = New System.Drawing.Font("Webdings", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnPauseSlides.ForeColor = System.Drawing.Color.Blue
            Me.btnPauseSlides.Location = New System.Drawing.Point(822, 334)
            Me.btnPauseSlides.Name = "btnPauseSlides"
            Me.btnPauseSlides.Size = New System.Drawing.Size(36, 32)
            Me.btnPauseSlides.TabIndex = 86
            Me.btnPauseSlides.Text = ";"
            '
            'btnNextSlide
            '
            Me.btnNextSlide.Font = New System.Drawing.Font("Webdings", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnNextSlide.Location = New System.Drawing.Point(906, 334)
            Me.btnNextSlide.Name = "btnNextSlide"
            Me.btnNextSlide.Size = New System.Drawing.Size(36, 32)
            Me.btnNextSlide.TabIndex = 88
            Me.btnNextSlide.Text = "8"
            '
            'btnPrevSlide
            '
            Me.btnPrevSlide.Font = New System.Drawing.Font("Webdings", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnPrevSlide.Location = New System.Drawing.Point(738, 334)
            Me.btnPrevSlide.Name = "btnPrevSlide"
            Me.btnPrevSlide.Size = New System.Drawing.Size(36, 32)
            Me.btnPrevSlide.TabIndex = 84
            Me.btnPrevSlide.Text = "7"
            '
            'btnClearSlideList
            '
            Me.btnClearSlideList.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnClearSlideList.Location = New System.Drawing.Point(558, 165)
            Me.btnClearSlideList.Name = "btnClearSlideList"
            Me.btnClearSlideList.Size = New System.Drawing.Size(72, 28)
            Me.btnClearSlideList.TabIndex = 78
            Me.btnClearSlideList.Text = "Clear List"
            '
            'Label9
            '
            Me.Label9.BackColor = System.Drawing.Color.Transparent
            Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label9.Location = New System.Drawing.Point(535, 336)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(92, 24)
            Me.Label9.TabIndex = 81
            Me.Label9.Text = "Seconds:"
            Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'nudDelay
            '
            Me.nudDelay.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.nudDelay.Location = New System.Drawing.Point(631, 334)
            Me.nudDelay.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
            Me.nudDelay.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.nudDelay.Name = "nudDelay"
            Me.nudDelay.Size = New System.Drawing.Size(56, 31)
            Me.nudDelay.TabIndex = 82
            Me.nudDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.nudDelay.Value = New Decimal(New Integer() {15, 0, 0, 0})
            '
            'Label8
            '
            Me.Label8.BackColor = System.Drawing.Color.Transparent
            Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label8.Location = New System.Drawing.Point(653, 3)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(332, 16)
            Me.Label8.TabIndex = 72
            Me.Label8.Text = "Slide List"
            Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'Label7
            '
            Me.Label7.BackColor = System.Drawing.Color.Transparent
            Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label7.Location = New System.Drawing.Point(268, 2)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(120, 16)
            Me.Label7.TabIndex = 68
            Me.Label7.Text = "Preview"
            Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnSaveSlides
            '
            Me.btnSaveSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSaveSlides.Location = New System.Drawing.Point(547, 284)
            Me.btnSaveSlides.Name = "btnSaveSlides"
            Me.btnSaveSlides.Size = New System.Drawing.Size(96, 40)
            Me.btnSaveSlides.TabIndex = 80
            Me.btnSaveSlides.Text = "Save Slideshow"
            '
            'btnLoadSlides
            '
            Me.btnLoadSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnLoadSlides.Location = New System.Drawing.Point(547, 240)
            Me.btnLoadSlides.Name = "btnLoadSlides"
            Me.btnLoadSlides.Size = New System.Drawing.Size(96, 40)
            Me.btnLoadSlides.TabIndex = 79
            Me.btnLoadSlides.Text = "Load Slideshow"
            '
            'FileListBox1
            '
            Me.FileListBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FileListBox1.FormattingEnabled = True
            Me.FileListBox1.Location = New System.Drawing.Point(268, 152)
            Me.FileListBox1.Name = "FileListBox1"
            Me.FileListBox1.Pattern = "*.jpg;*.gif;*.bmp;*.wmf;*.png"
            Me.FileListBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
            Me.FileListBox1.Size = New System.Drawing.Size(250, 212)
            Me.FileListBox1.TabIndex = 70
            '
            'btnRemoveSlides
            '
            Me.btnRemoveSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnRemoveSlides.Location = New System.Drawing.Point(558, 131)
            Me.btnRemoveSlides.Name = "btnRemoveSlides"
            Me.btnRemoveSlides.Size = New System.Drawing.Size(72, 28)
            Me.btnRemoveSlides.TabIndex = 77
            Me.btnRemoveSlides.Text = "Remove"
            '
            'btnAddSlide
            '
            Me.btnAddSlide.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnAddSlide.Location = New System.Drawing.Point(458, 50)
            Me.btnAddSlide.Name = "btnAddSlide"
            Me.btnAddSlide.Size = New System.Drawing.Size(60, 52)
            Me.btnAddSlide.TabIndex = 71
            Me.btnAddSlide.Text = "Add To List"
            '
            'Label6
            '
            Me.Label6.BackColor = System.Drawing.Color.Transparent
            Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label6.Location = New System.Drawing.Point(562, 54)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(71, 27)
            Me.Label6.TabIndex = 75
            Me.Label6.Text = "ORDER"
            Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnSlideDown
            '
            Me.btnSlideDown.Font = New System.Drawing.Font("Webdings", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnSlideDown.Location = New System.Drawing.Point(577, 80)
            Me.btnSlideDown.Name = "btnSlideDown"
            Me.btnSlideDown.Size = New System.Drawing.Size(40, 36)
            Me.btnSlideDown.TabIndex = 76
            Me.btnSlideDown.Text = "6"
            '
            'btnSlideUp
            '
            Me.btnSlideUp.Font = New System.Drawing.Font("Webdings", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnSlideUp.Location = New System.Drawing.Point(577, 19)
            Me.btnSlideUp.Name = "btnSlideUp"
            Me.btnSlideUp.Size = New System.Drawing.Size(40, 36)
            Me.btnSlideUp.TabIndex = 74
            Me.btnSlideUp.Text = "5"
            '
            'btnStopSlides
            '
            Me.btnStopSlides.Font = New System.Drawing.Font("Webdings", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnStopSlides.ForeColor = System.Drawing.Color.Red
            Me.btnStopSlides.Location = New System.Drawing.Point(864, 334)
            Me.btnStopSlides.Name = "btnStopSlides"
            Me.btnStopSlides.Size = New System.Drawing.Size(36, 32)
            Me.btnStopSlides.TabIndex = 87
            Me.btnStopSlides.Text = "<"
            '
            'btnLastSlide
            '
            Me.btnLastSlide.Font = New System.Drawing.Font("Webdings", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnLastSlide.Location = New System.Drawing.Point(948, 334)
            Me.btnLastSlide.Name = "btnLastSlide"
            Me.btnLastSlide.Size = New System.Drawing.Size(36, 32)
            Me.btnLastSlide.TabIndex = 89
            Me.btnLastSlide.Text = ":"
            '
            'btnPlaySlides
            '
            Me.btnPlaySlides.Font = New System.Drawing.Font("Webdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnPlaySlides.ForeColor = System.Drawing.Color.Green
            Me.btnPlaySlides.Location = New System.Drawing.Point(780, 334)
            Me.btnPlaySlides.Name = "btnPlaySlides"
            Me.btnPlaySlides.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.btnPlaySlides.Size = New System.Drawing.Size(36, 32)
            Me.btnPlaySlides.TabIndex = 85
            Me.btnPlaySlides.Text = "4"
            '
            'btnFirstSlide
            '
            Me.btnFirstSlide.Font = New System.Drawing.Font("Webdings", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnFirstSlide.Location = New System.Drawing.Point(693, 334)
            Me.btnFirstSlide.Name = "btnFirstSlide"
            Me.btnFirstSlide.Size = New System.Drawing.Size(36, 32)
            Me.btnFirstSlide.TabIndex = 83
            Me.btnFirstSlide.Text = "9"
            '
            'lbSlideList
            '
            Me.lbSlideList.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lbSlideList.HorizontalScrollbar = True
            Me.lbSlideList.ItemHeight = 16
            Me.lbSlideList.Location = New System.Drawing.Point(656, 20)
            Me.lbSlideList.Name = "lbSlideList"
            Me.lbSlideList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
            Me.lbSlideList.Size = New System.Drawing.Size(329, 308)
            Me.lbSlideList.TabIndex = 73
            '
            'picSlidePreview
            '
            Me.picSlidePreview.BackColor = System.Drawing.SystemColors.ActiveBorder
            Me.picSlidePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.picSlidePreview.Location = New System.Drawing.Point(268, 20)
            Me.picSlidePreview.Name = "picSlidePreview"
            Me.picSlidePreview.Size = New System.Drawing.Size(184, 121)
            Me.picSlidePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.picSlidePreview.TabIndex = 4
            Me.picSlidePreview.TabStop = False
            '
            'tpHotButtons
            '
            Me.tpHotButtons.Controls.Add(Me.btnClearHB)
            Me.tpHotButtons.Controls.Add(Me.lblHBinstructions)
            Me.tpHotButtons.Controls.Add(Me.btnSaveHB)
            Me.tpHotButtons.Controls.Add(Me.btnLoadHB)
            Me.tpHotButtons.Controls.Add(Me.gbHB)
            Me.tpHotButtons.Controls.Add(Me.cbHBActive)
            Me.tpHotButtons.Location = New System.Drawing.Point(4, 28)
            Me.tpHotButtons.Name = "tpHotButtons"
            Me.tpHotButtons.Size = New System.Drawing.Size(988, 372)
            Me.tpHotButtons.TabIndex = 4
            Me.tpHotButtons.Text = "Hot Buttons"
            '
            'btnClearHB
            '
            Me.btnClearHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnClearHB.Location = New System.Drawing.Point(27, 226)
            Me.btnClearHB.Name = "btnClearHB"
            Me.btnClearHB.Size = New System.Drawing.Size(124, 36)
            Me.btnClearHB.TabIndex = 43
            Me.btnClearHB.Text = "CLEAR ALL"
            '
            'lblHBinstructions
            '
            Me.lblHBinstructions.BackColor = System.Drawing.Color.LemonChiffon
            Me.lblHBinstructions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblHBinstructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblHBinstructions.ForeColor = System.Drawing.Color.Black
            Me.lblHBinstructions.Location = New System.Drawing.Point(6, 68)
            Me.lblHBinstructions.Name = "lblHBinstructions"
            Me.lblHBinstructions.Size = New System.Drawing.Size(168, 139)
            Me.lblHBinstructions.TabIndex = 42
            Me.lblHBinstructions.Text = "Hint: Hot Buttons are image shortcuts that you can define for quick access to sto" & _
        "red images. Select a name && image for each button. Save lists of buttons for sp" & _
        "ecific uses."
            Me.lblHBinstructions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnSaveHB
            '
            Me.btnSaveHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSaveHB.Location = New System.Drawing.Point(27, 318)
            Me.btnSaveHB.Name = "btnSaveHB"
            Me.btnSaveHB.Size = New System.Drawing.Size(124, 36)
            Me.btnSaveHB.TabIndex = 6
            Me.btnSaveHB.Text = "SAVE LIST"
            '
            'btnLoadHB
            '
            Me.btnLoadHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnLoadHB.Location = New System.Drawing.Point(27, 272)
            Me.btnLoadHB.Name = "btnLoadHB"
            Me.btnLoadHB.Size = New System.Drawing.Size(124, 36)
            Me.btnLoadHB.TabIndex = 5
            Me.btnLoadHB.Text = "LOAD LIST"
            '
            'gbHB
            '
            Me.gbHB.Controls.Add(Me.tbHBfile10)
            Me.gbHB.Controls.Add(Me.tbHBfile9)
            Me.gbHB.Controls.Add(Me.tbHBfile8)
            Me.gbHB.Controls.Add(Me.tbHBfile7)
            Me.gbHB.Controls.Add(Me.tbHBfile6)
            Me.gbHB.Controls.Add(Me.tbHBfile5)
            Me.gbHB.Controls.Add(Me.tbHBfile4)
            Me.gbHB.Controls.Add(Me.tbHBfile3)
            Me.gbHB.Controls.Add(Me.tbHBfile2)
            Me.gbHB.Controls.Add(Me.tbHBfile1)
            Me.gbHB.Controls.Add(Me.Label14)
            Me.gbHB.Controls.Add(Me.Label13)
            Me.gbHB.Controls.Add(Me.Label30)
            Me.gbHB.Controls.Add(Me.Label31)
            Me.gbHB.Controls.Add(Me.Label28)
            Me.gbHB.Controls.Add(Me.Label29)
            Me.gbHB.Controls.Add(Me.Label26)
            Me.gbHB.Controls.Add(Me.Label27)
            Me.gbHB.Controls.Add(Me.Label24)
            Me.gbHB.Controls.Add(Me.Label25)
            Me.gbHB.Controls.Add(Me.Label23)
            Me.gbHB.Controls.Add(Me.Label22)
            Me.gbHB.Controls.Add(Me.btnHBSelect10)
            Me.gbHB.Controls.Add(Me.btnHBSelect9)
            Me.gbHB.Controls.Add(Me.btnHBSelect8)
            Me.gbHB.Controls.Add(Me.btnHBSelect7)
            Me.gbHB.Controls.Add(Me.btnHBSelect6)
            Me.gbHB.Controls.Add(Me.btnHBSelect5)
            Me.gbHB.Controls.Add(Me.btnHBSelect4)
            Me.gbHB.Controls.Add(Me.btnHBSelect3)
            Me.gbHB.Controls.Add(Me.btnHBSelect2)
            Me.gbHB.Controls.Add(Me.btnHBSelect1)
            Me.gbHB.Controls.Add(Me.tbHBtext10)
            Me.gbHB.Controls.Add(Me.tbHBtext9)
            Me.gbHB.Controls.Add(Me.tbHBtext8)
            Me.gbHB.Controls.Add(Me.tbHBtext7)
            Me.gbHB.Controls.Add(Me.tbHBtext6)
            Me.gbHB.Controls.Add(Me.tbHBtext5)
            Me.gbHB.Controls.Add(Me.tbHBtext4)
            Me.gbHB.Controls.Add(Me.tbHBtext3)
            Me.gbHB.Controls.Add(Me.tbHBtext2)
            Me.gbHB.Controls.Add(Me.tbHBtext1)
            Me.gbHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.gbHB.Location = New System.Drawing.Point(182, 0)
            Me.gbHB.Name = "gbHB"
            Me.gbHB.Size = New System.Drawing.Size(798, 364)
            Me.gbHB.TabIndex = 4
            Me.gbHB.TabStop = False
            '
            'tbHBfile10
            '
            Me.tbHBfile10.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile10.Location = New System.Drawing.Point(177, 332)
            Me.tbHBfile10.Name = "tbHBfile10"
            Me.tbHBfile10.ReadOnly = True
            Me.tbHBfile10.Size = New System.Drawing.Size(535, 22)
            Me.tbHBfile10.TabIndex = 55
            '
            'tbHBfile9
            '
            Me.tbHBfile9.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile9.Location = New System.Drawing.Point(177, 299)
            Me.tbHBfile9.Name = "tbHBfile9"
            Me.tbHBfile9.ReadOnly = True
            Me.tbHBfile9.Size = New System.Drawing.Size(535, 22)
            Me.tbHBfile9.TabIndex = 54
            '
            'tbHBfile8
            '
            Me.tbHBfile8.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile8.Location = New System.Drawing.Point(177, 266)
            Me.tbHBfile8.Name = "tbHBfile8"
            Me.tbHBfile8.ReadOnly = True
            Me.tbHBfile8.Size = New System.Drawing.Size(535, 22)
            Me.tbHBfile8.TabIndex = 53
            '
            'tbHBfile7
            '
            Me.tbHBfile7.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile7.Location = New System.Drawing.Point(177, 233)
            Me.tbHBfile7.Name = "tbHBfile7"
            Me.tbHBfile7.ReadOnly = True
            Me.tbHBfile7.Size = New System.Drawing.Size(535, 22)
            Me.tbHBfile7.TabIndex = 52
            '
            'tbHBfile6
            '
            Me.tbHBfile6.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile6.Location = New System.Drawing.Point(177, 200)
            Me.tbHBfile6.Name = "tbHBfile6"
            Me.tbHBfile6.ReadOnly = True
            Me.tbHBfile6.Size = New System.Drawing.Size(535, 22)
            Me.tbHBfile6.TabIndex = 51
            '
            'tbHBfile5
            '
            Me.tbHBfile5.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile5.Location = New System.Drawing.Point(177, 167)
            Me.tbHBfile5.Name = "tbHBfile5"
            Me.tbHBfile5.ReadOnly = True
            Me.tbHBfile5.Size = New System.Drawing.Size(535, 22)
            Me.tbHBfile5.TabIndex = 50
            '
            'tbHBfile4
            '
            Me.tbHBfile4.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile4.Location = New System.Drawing.Point(177, 134)
            Me.tbHBfile4.Name = "tbHBfile4"
            Me.tbHBfile4.ReadOnly = True
            Me.tbHBfile4.Size = New System.Drawing.Size(535, 22)
            Me.tbHBfile4.TabIndex = 49
            '
            'tbHBfile3
            '
            Me.tbHBfile3.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile3.Location = New System.Drawing.Point(177, 101)
            Me.tbHBfile3.Name = "tbHBfile3"
            Me.tbHBfile3.ReadOnly = True
            Me.tbHBfile3.Size = New System.Drawing.Size(535, 22)
            Me.tbHBfile3.TabIndex = 48
            '
            'tbHBfile2
            '
            Me.tbHBfile2.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile2.Location = New System.Drawing.Point(177, 68)
            Me.tbHBfile2.Name = "tbHBfile2"
            Me.tbHBfile2.ReadOnly = True
            Me.tbHBfile2.Size = New System.Drawing.Size(535, 22)
            Me.tbHBfile2.TabIndex = 47
            '
            'tbHBfile1
            '
            Me.tbHBfile1.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile1.Location = New System.Drawing.Point(177, 35)
            Me.tbHBfile1.Name = "tbHBfile1"
            Me.tbHBfile1.ReadOnly = True
            Me.tbHBfile1.Size = New System.Drawing.Size(535, 22)
            Me.tbHBfile1.TabIndex = 46
            '
            'Label14
            '
            Me.Label14.BackColor = System.Drawing.Color.Transparent
            Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label14.Location = New System.Drawing.Point(180, 8)
            Me.Label14.Name = "Label14"
            Me.Label14.Size = New System.Drawing.Size(328, 20)
            Me.Label14.TabIndex = 45
            Me.Label14.Text = "Image File"
            Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'Label13
            '
            Me.Label13.BackColor = System.Drawing.Color.Transparent
            Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label13.Location = New System.Drawing.Point(68, 8)
            Me.Label13.Name = "Label13"
            Me.Label13.Size = New System.Drawing.Size(96, 20)
            Me.Label13.TabIndex = 44
            Me.Label13.Text = "Title"
            Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'Label30
            '
            Me.Label30.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label30.Location = New System.Drawing.Point(4, 335)
            Me.Label30.Name = "Label30"
            Me.Label30.Size = New System.Drawing.Size(64, 20)
            Me.Label30.TabIndex = 43
            Me.Label30.Text = "Button 10"
            '
            'Label31
            '
            Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label31.Location = New System.Drawing.Point(4, 302)
            Me.Label31.Name = "Label31"
            Me.Label31.Size = New System.Drawing.Size(64, 20)
            Me.Label31.TabIndex = 42
            Me.Label31.Text = "Button 9"
            '
            'Label28
            '
            Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label28.Location = New System.Drawing.Point(4, 269)
            Me.Label28.Name = "Label28"
            Me.Label28.Size = New System.Drawing.Size(64, 20)
            Me.Label28.TabIndex = 41
            Me.Label28.Text = "Button 8"
            '
            'Label29
            '
            Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label29.Location = New System.Drawing.Point(4, 236)
            Me.Label29.Name = "Label29"
            Me.Label29.Size = New System.Drawing.Size(64, 20)
            Me.Label29.TabIndex = 40
            Me.Label29.Text = "Button 7"
            '
            'Label26
            '
            Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label26.Location = New System.Drawing.Point(4, 203)
            Me.Label26.Name = "Label26"
            Me.Label26.Size = New System.Drawing.Size(64, 20)
            Me.Label26.TabIndex = 39
            Me.Label26.Text = "Button 6"
            '
            'Label27
            '
            Me.Label27.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label27.Location = New System.Drawing.Point(4, 170)
            Me.Label27.Name = "Label27"
            Me.Label27.Size = New System.Drawing.Size(64, 20)
            Me.Label27.TabIndex = 38
            Me.Label27.Text = "Button 5"
            '
            'Label24
            '
            Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label24.Location = New System.Drawing.Point(4, 137)
            Me.Label24.Name = "Label24"
            Me.Label24.Size = New System.Drawing.Size(64, 20)
            Me.Label24.TabIndex = 37
            Me.Label24.Text = "Button 4"
            '
            'Label25
            '
            Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label25.Location = New System.Drawing.Point(4, 104)
            Me.Label25.Name = "Label25"
            Me.Label25.Size = New System.Drawing.Size(64, 20)
            Me.Label25.TabIndex = 36
            Me.Label25.Text = "Button 3"
            '
            'Label23
            '
            Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label23.Location = New System.Drawing.Point(4, 71)
            Me.Label23.Name = "Label23"
            Me.Label23.Size = New System.Drawing.Size(64, 20)
            Me.Label23.TabIndex = 35
            Me.Label23.Text = "Button 2"
            '
            'Label22
            '
            Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label22.Location = New System.Drawing.Point(4, 38)
            Me.Label22.Name = "Label22"
            Me.Label22.Size = New System.Drawing.Size(64, 20)
            Me.Label22.TabIndex = 34
            Me.Label22.Text = "Button 1"
            '
            'btnHBSelect10
            '
            Me.btnHBSelect10.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHBSelect10.Location = New System.Drawing.Point(718, 329)
            Me.btnHBSelect10.Name = "btnHBSelect10"
            Me.btnHBSelect10.Size = New System.Drawing.Size(74, 26)
            Me.btnHBSelect10.TabIndex = 33
            Me.btnHBSelect10.Tag = "9"
            Me.btnHBSelect10.Text = "Select..."
            '
            'btnHBSelect9
            '
            Me.btnHBSelect9.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHBSelect9.Location = New System.Drawing.Point(718, 296)
            Me.btnHBSelect9.Name = "btnHBSelect9"
            Me.btnHBSelect9.Size = New System.Drawing.Size(74, 26)
            Me.btnHBSelect9.TabIndex = 32
            Me.btnHBSelect9.Tag = "8"
            Me.btnHBSelect9.Text = "Select..."
            '
            'btnHBSelect8
            '
            Me.btnHBSelect8.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHBSelect8.Location = New System.Drawing.Point(718, 263)
            Me.btnHBSelect8.Name = "btnHBSelect8"
            Me.btnHBSelect8.Size = New System.Drawing.Size(74, 26)
            Me.btnHBSelect8.TabIndex = 31
            Me.btnHBSelect8.Tag = "7"
            Me.btnHBSelect8.Text = "Select..."
            '
            'btnHBSelect7
            '
            Me.btnHBSelect7.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHBSelect7.Location = New System.Drawing.Point(718, 230)
            Me.btnHBSelect7.Name = "btnHBSelect7"
            Me.btnHBSelect7.Size = New System.Drawing.Size(74, 26)
            Me.btnHBSelect7.TabIndex = 30
            Me.btnHBSelect7.Tag = "6"
            Me.btnHBSelect7.Text = "Select..."
            '
            'btnHBSelect6
            '
            Me.btnHBSelect6.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHBSelect6.Location = New System.Drawing.Point(718, 197)
            Me.btnHBSelect6.Name = "btnHBSelect6"
            Me.btnHBSelect6.Size = New System.Drawing.Size(74, 26)
            Me.btnHBSelect6.TabIndex = 29
            Me.btnHBSelect6.Tag = "5"
            Me.btnHBSelect6.Text = "Select..."
            '
            'btnHBSelect5
            '
            Me.btnHBSelect5.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHBSelect5.Location = New System.Drawing.Point(718, 164)
            Me.btnHBSelect5.Name = "btnHBSelect5"
            Me.btnHBSelect5.Size = New System.Drawing.Size(74, 26)
            Me.btnHBSelect5.TabIndex = 28
            Me.btnHBSelect5.Tag = "4"
            Me.btnHBSelect5.Text = "Select..."
            '
            'btnHBSelect4
            '
            Me.btnHBSelect4.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHBSelect4.Location = New System.Drawing.Point(718, 131)
            Me.btnHBSelect4.Name = "btnHBSelect4"
            Me.btnHBSelect4.Size = New System.Drawing.Size(74, 26)
            Me.btnHBSelect4.TabIndex = 27
            Me.btnHBSelect4.Tag = "3"
            Me.btnHBSelect4.Text = "Select..."
            '
            'btnHBSelect3
            '
            Me.btnHBSelect3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHBSelect3.Location = New System.Drawing.Point(718, 98)
            Me.btnHBSelect3.Name = "btnHBSelect3"
            Me.btnHBSelect3.Size = New System.Drawing.Size(74, 26)
            Me.btnHBSelect3.TabIndex = 26
            Me.btnHBSelect3.Tag = "2"
            Me.btnHBSelect3.Text = "Select..."
            '
            'btnHBSelect2
            '
            Me.btnHBSelect2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHBSelect2.Location = New System.Drawing.Point(718, 65)
            Me.btnHBSelect2.Name = "btnHBSelect2"
            Me.btnHBSelect2.Size = New System.Drawing.Size(74, 26)
            Me.btnHBSelect2.TabIndex = 25
            Me.btnHBSelect2.Tag = "1"
            Me.btnHBSelect2.Text = "Select..."
            '
            'btnHBSelect1
            '
            Me.btnHBSelect1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHBSelect1.Location = New System.Drawing.Point(718, 32)
            Me.btnHBSelect1.Name = "btnHBSelect1"
            Me.btnHBSelect1.Size = New System.Drawing.Size(74, 26)
            Me.btnHBSelect1.TabIndex = 24
            Me.btnHBSelect1.Tag = "0"
            Me.btnHBSelect1.Text = "Select..."
            '
            'tbHBtext10
            '
            Me.tbHBtext10.Location = New System.Drawing.Point(76, 332)
            Me.tbHBtext10.MaxLength = 11
            Me.tbHBtext10.Name = "tbHBtext10"
            Me.tbHBtext10.Size = New System.Drawing.Size(95, 22)
            Me.tbHBtext10.TabIndex = 13
            Me.tbHBtext10.Tag = "9"
            Me.tbHBtext10.Text = "Hot 10"
            Me.tbHBtext10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbHBtext9
            '
            Me.tbHBtext9.Location = New System.Drawing.Point(76, 299)
            Me.tbHBtext9.MaxLength = 11
            Me.tbHBtext9.Name = "tbHBtext9"
            Me.tbHBtext9.Size = New System.Drawing.Size(95, 22)
            Me.tbHBtext9.TabIndex = 12
            Me.tbHBtext9.Tag = "8"
            Me.tbHBtext9.Text = "Hot 9"
            Me.tbHBtext9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbHBtext8
            '
            Me.tbHBtext8.Location = New System.Drawing.Point(76, 266)
            Me.tbHBtext8.MaxLength = 11
            Me.tbHBtext8.Name = "tbHBtext8"
            Me.tbHBtext8.Size = New System.Drawing.Size(95, 22)
            Me.tbHBtext8.TabIndex = 11
            Me.tbHBtext8.Tag = "7"
            Me.tbHBtext8.Text = "Hot 8"
            Me.tbHBtext8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbHBtext7
            '
            Me.tbHBtext7.Location = New System.Drawing.Point(76, 233)
            Me.tbHBtext7.MaxLength = 11
            Me.tbHBtext7.Name = "tbHBtext7"
            Me.tbHBtext7.Size = New System.Drawing.Size(95, 22)
            Me.tbHBtext7.TabIndex = 10
            Me.tbHBtext7.Tag = "6"
            Me.tbHBtext7.Text = "Hot 7"
            Me.tbHBtext7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbHBtext6
            '
            Me.tbHBtext6.Location = New System.Drawing.Point(76, 200)
            Me.tbHBtext6.MaxLength = 11
            Me.tbHBtext6.Name = "tbHBtext6"
            Me.tbHBtext6.Size = New System.Drawing.Size(95, 22)
            Me.tbHBtext6.TabIndex = 9
            Me.tbHBtext6.Tag = "5"
            Me.tbHBtext6.Text = "Hot 6"
            Me.tbHBtext6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbHBtext5
            '
            Me.tbHBtext5.Location = New System.Drawing.Point(76, 167)
            Me.tbHBtext5.MaxLength = 11
            Me.tbHBtext5.Name = "tbHBtext5"
            Me.tbHBtext5.Size = New System.Drawing.Size(95, 22)
            Me.tbHBtext5.TabIndex = 8
            Me.tbHBtext5.Tag = "4"
            Me.tbHBtext5.Text = "Hot 5"
            Me.tbHBtext5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbHBtext4
            '
            Me.tbHBtext4.Location = New System.Drawing.Point(76, 134)
            Me.tbHBtext4.MaxLength = 11
            Me.tbHBtext4.Name = "tbHBtext4"
            Me.tbHBtext4.Size = New System.Drawing.Size(95, 22)
            Me.tbHBtext4.TabIndex = 7
            Me.tbHBtext4.Tag = "3"
            Me.tbHBtext4.Text = "Hot 4"
            Me.tbHBtext4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbHBtext3
            '
            Me.tbHBtext3.Location = New System.Drawing.Point(76, 101)
            Me.tbHBtext3.MaxLength = 11
            Me.tbHBtext3.Name = "tbHBtext3"
            Me.tbHBtext3.Size = New System.Drawing.Size(95, 22)
            Me.tbHBtext3.TabIndex = 6
            Me.tbHBtext3.Tag = "2"
            Me.tbHBtext3.Text = "Hot 3"
            Me.tbHBtext3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbHBtext2
            '
            Me.tbHBtext2.Location = New System.Drawing.Point(76, 68)
            Me.tbHBtext2.MaxLength = 11
            Me.tbHBtext2.Name = "tbHBtext2"
            Me.tbHBtext2.Size = New System.Drawing.Size(95, 22)
            Me.tbHBtext2.TabIndex = 5
            Me.tbHBtext2.Tag = "1"
            Me.tbHBtext2.Text = "Hot 2"
            Me.tbHBtext2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbHBtext1
            '
            Me.tbHBtext1.Location = New System.Drawing.Point(76, 35)
            Me.tbHBtext1.MaxLength = 11
            Me.tbHBtext1.Name = "tbHBtext1"
            Me.tbHBtext1.Size = New System.Drawing.Size(95, 22)
            Me.tbHBtext1.TabIndex = 4
            Me.tbHBtext1.Tag = "0"
            Me.tbHBtext1.Text = "Hot 1"
            Me.tbHBtext1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'cbHBActive
            '
            Me.cbHBActive.CheckAlign = System.Drawing.ContentAlignment.BottomCenter
            Me.cbHBActive.Checked = True
            Me.cbHBActive.CheckState = System.Windows.Forms.CheckState.Indeterminate
            Me.cbHBActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbHBActive.Location = New System.Drawing.Point(4, 4)
            Me.cbHBActive.Name = "cbHBActive"
            Me.cbHBActive.Size = New System.Drawing.Size(172, 39)
            Me.cbHBActive.TabIndex = 0
            Me.cbHBActive.Text = "Toggle Hot Buttons"
            Me.cbHBActive.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'tpPrefs
            '
            Me.tpPrefs.Controls.Add(Me.cbShadowsEnabled)
            Me.tpPrefs.Controls.Add(Me.Label43)
            Me.tpPrefs.Controls.Add(Me.Label38)
            Me.tpPrefs.Controls.Add(Me.Label39)
            Me.tpPrefs.Controls.Add(Me.Label40)
            Me.tpPrefs.Controls.Add(Me.nudDefaultCountdownSeconds)
            Me.tpPrefs.Controls.Add(Me.nudDefaultCountdownMinutes)
            Me.tpPrefs.Controls.Add(Me.nudDefaultCountdownHours)
            Me.tpPrefs.Controls.Add(Me.btnDefaultPrefs)
            Me.tpPrefs.Controls.Add(Me.tbDefaultImageDir)
            Me.tpPrefs.Controls.Add(Me.btnChooseDefaultImageDir)
            Me.tpPrefs.Controls.Add(Me.Label18)
            Me.tpPrefs.Controls.Add(Me.tbDefaultFontSize)
            Me.tpPrefs.Controls.Add(Me.Label17)
            Me.tpPrefs.Controls.Add(Me.GroupBox2)
            Me.tpPrefs.Controls.Add(Me.btnSavePrefs)
            Me.tpPrefs.Controls.Add(Me.btnRevertPrefs)
            Me.tpPrefs.Controls.Add(Me.cbPlaySlidesAtStart)
            Me.tpPrefs.Controls.Add(Me.Label16)
            Me.tpPrefs.Controls.Add(Me.nudDefaultSlideDelay)
            Me.tpPrefs.Controls.Add(Me.tbDefaultSlideShow)
            Me.tpPrefs.Controls.Add(Me.btnChooseDefaultSlideShow)
            Me.tpPrefs.Controls.Add(Me.tbDefaultHBFile)
            Me.tpPrefs.Controls.Add(Me.btnChooseDefaultHB)
            Me.tpPrefs.Controls.Add(Me.tbDefaultImageFile)
            Me.tpPrefs.Controls.Add(Me.btnChooseDefaultImage)
            Me.tpPrefs.Controls.Add(Me.cbDisplayDefaultImage)
            Me.tpPrefs.Controls.Add(Me.cbLoadDefaultSlides)
            Me.tpPrefs.Controls.Add(Me.cbLoadDefaultHB)
            Me.tpPrefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tpPrefs.Location = New System.Drawing.Point(4, 28)
            Me.tpPrefs.Name = "tpPrefs"
            Me.tpPrefs.Size = New System.Drawing.Size(988, 372)
            Me.tpPrefs.TabIndex = 5
            Me.tpPrefs.Text = "Preferences"
            '
            'cbShadowsEnabled
            '
            Me.cbShadowsEnabled.BackColor = System.Drawing.Color.Transparent
            Me.cbShadowsEnabled.Checked = True
            Me.cbShadowsEnabled.CheckState = System.Windows.Forms.CheckState.Checked
            Me.cbShadowsEnabled.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbShadowsEnabled.Location = New System.Drawing.Point(11, 108)
            Me.cbShadowsEnabled.Name = "cbShadowsEnabled"
            Me.cbShadowsEnabled.Size = New System.Drawing.Size(242, 22)
            Me.cbShadowsEnabled.TabIndex = 120
            Me.cbShadowsEnabled.Text = "Display text shadows"
            Me.cbShadowsEnabled.UseVisualStyleBackColor = False
            '
            'Label43
            '
            Me.Label43.BackColor = System.Drawing.Color.Transparent
            Me.Label43.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label43.Location = New System.Drawing.Point(516, 260)
            Me.Label43.Name = "Label43"
            Me.Label43.Size = New System.Drawing.Size(277, 20)
            Me.Label43.TabIndex = 113
            Me.Label43.Text = "Default Countdown Timer Values:"
            Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Label38
            '
            Me.Label38.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label38.Location = New System.Drawing.Point(924, 236)
            Me.Label38.Name = "Label38"
            Me.Label38.Size = New System.Drawing.Size(40, 16)
            Me.Label38.TabIndex = 112
            Me.Label38.Text = "SS"
            Me.Label38.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'Label39
            '
            Me.Label39.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label39.Location = New System.Drawing.Point(880, 236)
            Me.Label39.Name = "Label39"
            Me.Label39.Size = New System.Drawing.Size(40, 16)
            Me.Label39.TabIndex = 111
            Me.Label39.Text = "MM"
            Me.Label39.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'Label40
            '
            Me.Label40.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label40.Location = New System.Drawing.Point(836, 236)
            Me.Label40.Name = "Label40"
            Me.Label40.Size = New System.Drawing.Size(40, 16)
            Me.Label40.TabIndex = 110
            Me.Label40.Text = "HH"
            Me.Label40.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'nudDefaultCountdownSeconds
            '
            Me.nudDefaultCountdownSeconds.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.nudDefaultCountdownSeconds.Location = New System.Drawing.Point(924, 260)
            Me.nudDefaultCountdownSeconds.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
            Me.nudDefaultCountdownSeconds.Name = "nudDefaultCountdownSeconds"
            Me.nudDefaultCountdownSeconds.Size = New System.Drawing.Size(44, 23)
            Me.nudDefaultCountdownSeconds.TabIndex = 116
            Me.nudDefaultCountdownSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'nudDefaultCountdownMinutes
            '
            Me.nudDefaultCountdownMinutes.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.nudDefaultCountdownMinutes.Location = New System.Drawing.Point(880, 260)
            Me.nudDefaultCountdownMinutes.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
            Me.nudDefaultCountdownMinutes.Name = "nudDefaultCountdownMinutes"
            Me.nudDefaultCountdownMinutes.Size = New System.Drawing.Size(44, 23)
            Me.nudDefaultCountdownMinutes.TabIndex = 115
            Me.nudDefaultCountdownMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.nudDefaultCountdownMinutes.Value = New Decimal(New Integer() {5, 0, 0, 0})
            '
            'nudDefaultCountdownHours
            '
            Me.nudDefaultCountdownHours.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.nudDefaultCountdownHours.Location = New System.Drawing.Point(836, 260)
            Me.nudDefaultCountdownHours.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
            Me.nudDefaultCountdownHours.Name = "nudDefaultCountdownHours"
            Me.nudDefaultCountdownHours.Size = New System.Drawing.Size(44, 23)
            Me.nudDefaultCountdownHours.TabIndex = 114
            Me.nudDefaultCountdownHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'btnDefaultPrefs
            '
            Me.btnDefaultPrefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnDefaultPrefs.Location = New System.Drawing.Point(11, 330)
            Me.btnDefaultPrefs.Name = "btnDefaultPrefs"
            Me.btnDefaultPrefs.Size = New System.Drawing.Size(190, 32)
            Me.btnDefaultPrefs.TabIndex = 117
            Me.btnDefaultPrefs.Text = "FACTORY DEFAULTS"
            '
            'tbDefaultImageDir
            '
            Me.tbDefaultImageDir.BackColor = System.Drawing.SystemColors.Control
            Me.tbDefaultImageDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbDefaultImageDir.Location = New System.Drawing.Point(27, 170)
            Me.tbDefaultImageDir.Name = "tbDefaultImageDir"
            Me.tbDefaultImageDir.ReadOnly = True
            Me.tbDefaultImageDir.Size = New System.Drawing.Size(366, 23)
            Me.tbDefaultImageDir.TabIndex = 95
            Me.tbDefaultImageDir.Text = "C:\JANIS"
            '
            'btnChooseDefaultImageDir
            '
            Me.btnChooseDefaultImageDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnChooseDefaultImageDir.Location = New System.Drawing.Point(399, 168)
            Me.btnChooseDefaultImageDir.Name = "btnChooseDefaultImageDir"
            Me.btnChooseDefaultImageDir.Size = New System.Drawing.Size(72, 26)
            Me.btnChooseDefaultImageDir.TabIndex = 96
            Me.btnChooseDefaultImageDir.Tag = "0"
            Me.btnChooseDefaultImageDir.Text = "Select..."
            '
            'Label18
            '
            Me.Label18.BackColor = System.Drawing.Color.Transparent
            Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label18.Location = New System.Drawing.Point(8, 145)
            Me.Label18.Name = "Label18"
            Me.Label18.Size = New System.Drawing.Size(240, 22)
            Me.Label18.TabIndex = 94
            Me.Label18.Text = "Default image search directory:"
            Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'tbDefaultFontSize
            '
            Me.tbDefaultFontSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbDefaultFontSize.Location = New System.Drawing.Point(278, 76)
            Me.tbDefaultFontSize.MaxLength = 3
            Me.tbDefaultFontSize.Name = "tbDefaultFontSize"
            Me.tbDefaultFontSize.Size = New System.Drawing.Size(52, 24)
            Me.tbDefaultFontSize.TabIndex = 93
            Me.tbDefaultFontSize.Tag = "0"
            Me.tbDefaultFontSize.Text = "60"
            Me.tbDefaultFontSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'Label17
            '
            Me.Label17.BackColor = System.Drawing.Color.Transparent
            Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label17.Location = New System.Drawing.Point(8, 76)
            Me.Label17.Name = "Label17"
            Me.Label17.Size = New System.Drawing.Size(242, 22)
            Me.Label17.TabIndex = 92
            Me.Label17.Text = "Default font size for ""Text Display"":"
            Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.lblDefaultColorRight)
            Me.GroupBox2.Controls.Add(Me.lblDefaultColorLeft)
            Me.GroupBox2.Controls.Add(Me.grpDefaultColorsRight)
            Me.GroupBox2.Controls.Add(Me.grpDefaultColorsLeft)
            Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.GroupBox2.Location = New System.Drawing.Point(8, 4)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(968, 62)
            Me.GroupBox2.TabIndex = 91
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Default team colors at startup:"
            '
            'lblDefaultColorRight
            '
            Me.lblDefaultColorRight.BackColor = System.Drawing.Color.Maroon
            Me.lblDefaultColorRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblDefaultColorRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDefaultColorRight.ForeColor = System.Drawing.Color.White
            Me.lblDefaultColorRight.Location = New System.Drawing.Point(582, 24)
            Me.lblDefaultColorRight.Name = "lblDefaultColorRight"
            Me.lblDefaultColorRight.Size = New System.Drawing.Size(112, 32)
            Me.lblDefaultColorRight.TabIndex = 94
            Me.lblDefaultColorRight.Text = "Right Team"
            Me.lblDefaultColorRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblDefaultColorLeft
            '
            Me.lblDefaultColorLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.lblDefaultColorLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblDefaultColorLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDefaultColorLeft.ForeColor = System.Drawing.Color.White
            Me.lblDefaultColorLeft.Location = New System.Drawing.Point(273, 24)
            Me.lblDefaultColorLeft.Name = "lblDefaultColorLeft"
            Me.lblDefaultColorLeft.Size = New System.Drawing.Size(112, 32)
            Me.lblDefaultColorLeft.TabIndex = 93
            Me.lblDefaultColorLeft.Text = "Left Team"
            Me.lblDefaultColorLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'grpDefaultColorsRight
            '
            Me.grpDefaultColorsRight.BackColor = System.Drawing.Color.Transparent
            Me.grpDefaultColorsRight.Controls.Add(Me.btnChooseDefaultTextColorRight)
            Me.grpDefaultColorsRight.Controls.Add(Me.pnlDefaultTextColorRight6)
            Me.grpDefaultColorsRight.Controls.Add(Me.pnlDefaultTextColorRight5)
            Me.grpDefaultColorsRight.Controls.Add(Me.pnlDefaultTextColorRight4)
            Me.grpDefaultColorsRight.Controls.Add(Me.pnlDefaultTextColorRight3)
            Me.grpDefaultColorsRight.Controls.Add(Me.pnlDefaultTextColorRight2)
            Me.grpDefaultColorsRight.Controls.Add(Me.pnlDefaultTextColorRight1)
            Me.grpDefaultColorsRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpDefaultColorsRight.Location = New System.Drawing.Point(712, 15)
            Me.grpDefaultColorsRight.Name = "grpDefaultColorsRight"
            Me.grpDefaultColorsRight.Size = New System.Drawing.Size(244, 44)
            Me.grpDefaultColorsRight.TabIndex = 92
            Me.grpDefaultColorsRight.TabStop = False
            '
            'btnChooseDefaultTextColorRight
            '
            Me.btnChooseDefaultTextColorRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnChooseDefaultTextColorRight.Location = New System.Drawing.Point(156, 13)
            Me.btnChooseDefaultTextColorRight.Name = "btnChooseDefaultTextColorRight"
            Me.btnChooseDefaultTextColorRight.Size = New System.Drawing.Size(78, 26)
            Me.btnChooseDefaultTextColorRight.TabIndex = 27
            Me.btnChooseDefaultTextColorRight.Text = "Choose..."
            '
            'pnlDefaultTextColorRight6
            '
            Me.pnlDefaultTextColorRight6.BackColor = System.Drawing.Color.Black
            Me.pnlDefaultTextColorRight6.Location = New System.Drawing.Point(129, 18)
            Me.pnlDefaultTextColorRight6.Name = "pnlDefaultTextColorRight6"
            Me.pnlDefaultTextColorRight6.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorRight6.TabIndex = 26
            Me.pnlDefaultTextColorRight6.TabStop = True
            '
            'pnlDefaultTextColorRight5
            '
            Me.pnlDefaultTextColorRight5.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlDefaultTextColorRight5.Location = New System.Drawing.Point(105, 18)
            Me.pnlDefaultTextColorRight5.Name = "pnlDefaultTextColorRight5"
            Me.pnlDefaultTextColorRight5.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorRight5.TabIndex = 25
            Me.pnlDefaultTextColorRight5.TabStop = True
            '
            'pnlDefaultTextColorRight4
            '
            Me.pnlDefaultTextColorRight4.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.pnlDefaultTextColorRight4.Location = New System.Drawing.Point(81, 18)
            Me.pnlDefaultTextColorRight4.Name = "pnlDefaultTextColorRight4"
            Me.pnlDefaultTextColorRight4.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorRight4.TabIndex = 24
            Me.pnlDefaultTextColorRight4.TabStop = True
            '
            'pnlDefaultTextColorRight3
            '
            Me.pnlDefaultTextColorRight3.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlDefaultTextColorRight3.Location = New System.Drawing.Point(57, 18)
            Me.pnlDefaultTextColorRight3.Name = "pnlDefaultTextColorRight3"
            Me.pnlDefaultTextColorRight3.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorRight3.TabIndex = 23
            Me.pnlDefaultTextColorRight3.TabStop = True
            '
            'pnlDefaultTextColorRight2
            '
            Me.pnlDefaultTextColorRight2.BackColor = System.Drawing.Color.Maroon
            Me.pnlDefaultTextColorRight2.Location = New System.Drawing.Point(33, 18)
            Me.pnlDefaultTextColorRight2.Name = "pnlDefaultTextColorRight2"
            Me.pnlDefaultTextColorRight2.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorRight2.TabIndex = 22
            Me.pnlDefaultTextColorRight2.TabStop = True
            '
            'pnlDefaultTextColorRight1
            '
            Me.pnlDefaultTextColorRight1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.pnlDefaultTextColorRight1.Location = New System.Drawing.Point(9, 18)
            Me.pnlDefaultTextColorRight1.Name = "pnlDefaultTextColorRight1"
            Me.pnlDefaultTextColorRight1.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorRight1.TabIndex = 21
            Me.pnlDefaultTextColorRight1.TabStop = True
            '
            'grpDefaultColorsLeft
            '
            Me.grpDefaultColorsLeft.BackColor = System.Drawing.Color.Transparent
            Me.grpDefaultColorsLeft.Controls.Add(Me.btnChooseDefaultTextColorLeft)
            Me.grpDefaultColorsLeft.Controls.Add(Me.pnlDefaultTextColorLeft6)
            Me.grpDefaultColorsLeft.Controls.Add(Me.pnlDefaultTextColorLeft5)
            Me.grpDefaultColorsLeft.Controls.Add(Me.pnlDefaultTextColorLeft4)
            Me.grpDefaultColorsLeft.Controls.Add(Me.pnlDefaultTextColorLeft3)
            Me.grpDefaultColorsLeft.Controls.Add(Me.pnlDefaultTextColorLeft2)
            Me.grpDefaultColorsLeft.Controls.Add(Me.pnlDefaultTextColorLeft1)
            Me.grpDefaultColorsLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpDefaultColorsLeft.Location = New System.Drawing.Point(11, 15)
            Me.grpDefaultColorsLeft.Name = "grpDefaultColorsLeft"
            Me.grpDefaultColorsLeft.Size = New System.Drawing.Size(244, 44)
            Me.grpDefaultColorsLeft.TabIndex = 91
            Me.grpDefaultColorsLeft.TabStop = False
            '
            'btnChooseDefaultTextColorLeft
            '
            Me.btnChooseDefaultTextColorLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnChooseDefaultTextColorLeft.Location = New System.Drawing.Point(156, 13)
            Me.btnChooseDefaultTextColorLeft.Name = "btnChooseDefaultTextColorLeft"
            Me.btnChooseDefaultTextColorLeft.Size = New System.Drawing.Size(78, 26)
            Me.btnChooseDefaultTextColorLeft.TabIndex = 27
            Me.btnChooseDefaultTextColorLeft.Text = "Choose..."
            '
            'pnlDefaultTextColorLeft6
            '
            Me.pnlDefaultTextColorLeft6.BackColor = System.Drawing.Color.Black
            Me.pnlDefaultTextColorLeft6.Location = New System.Drawing.Point(129, 18)
            Me.pnlDefaultTextColorLeft6.Name = "pnlDefaultTextColorLeft6"
            Me.pnlDefaultTextColorLeft6.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorLeft6.TabIndex = 26
            Me.pnlDefaultTextColorLeft6.TabStop = True
            '
            'pnlDefaultTextColorLeft5
            '
            Me.pnlDefaultTextColorLeft5.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlDefaultTextColorLeft5.Location = New System.Drawing.Point(105, 18)
            Me.pnlDefaultTextColorLeft5.Name = "pnlDefaultTextColorLeft5"
            Me.pnlDefaultTextColorLeft5.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorLeft5.TabIndex = 25
            Me.pnlDefaultTextColorLeft5.TabStop = True
            '
            'pnlDefaultTextColorLeft4
            '
            Me.pnlDefaultTextColorLeft4.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.pnlDefaultTextColorLeft4.Location = New System.Drawing.Point(81, 18)
            Me.pnlDefaultTextColorLeft4.Name = "pnlDefaultTextColorLeft4"
            Me.pnlDefaultTextColorLeft4.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorLeft4.TabIndex = 24
            Me.pnlDefaultTextColorLeft4.TabStop = True
            '
            'pnlDefaultTextColorLeft3
            '
            Me.pnlDefaultTextColorLeft3.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlDefaultTextColorLeft3.Location = New System.Drawing.Point(57, 18)
            Me.pnlDefaultTextColorLeft3.Name = "pnlDefaultTextColorLeft3"
            Me.pnlDefaultTextColorLeft3.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorLeft3.TabIndex = 23
            Me.pnlDefaultTextColorLeft3.TabStop = True
            '
            'pnlDefaultTextColorLeft2
            '
            Me.pnlDefaultTextColorLeft2.BackColor = System.Drawing.Color.Maroon
            Me.pnlDefaultTextColorLeft2.Location = New System.Drawing.Point(33, 18)
            Me.pnlDefaultTextColorLeft2.Name = "pnlDefaultTextColorLeft2"
            Me.pnlDefaultTextColorLeft2.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorLeft2.TabIndex = 22
            Me.pnlDefaultTextColorLeft2.TabStop = True
            '
            'pnlDefaultTextColorLeft1
            '
            Me.pnlDefaultTextColorLeft1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.pnlDefaultTextColorLeft1.Location = New System.Drawing.Point(9, 18)
            Me.pnlDefaultTextColorLeft1.Name = "pnlDefaultTextColorLeft1"
            Me.pnlDefaultTextColorLeft1.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorLeft1.TabIndex = 21
            Me.pnlDefaultTextColorLeft1.TabStop = True
            '
            'btnSavePrefs
            '
            Me.btnSavePrefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSavePrefs.Location = New System.Drawing.Point(380, 330)
            Me.btnSavePrefs.Name = "btnSavePrefs"
            Me.btnSavePrefs.Size = New System.Drawing.Size(190, 32)
            Me.btnSavePrefs.TabIndex = 118
            Me.btnSavePrefs.Text = "SAVE PREFS"
            '
            'btnRevertPrefs
            '
            Me.btnRevertPrefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnRevertPrefs.Location = New System.Drawing.Point(786, 330)
            Me.btnRevertPrefs.Name = "btnRevertPrefs"
            Me.btnRevertPrefs.Size = New System.Drawing.Size(190, 32)
            Me.btnRevertPrefs.TabIndex = 119
            Me.btnRevertPrefs.Text = "UNDO CHANGES"
            '
            'cbPlaySlidesAtStart
            '
            Me.cbPlaySlidesAtStart.BackColor = System.Drawing.Color.Transparent
            Me.cbPlaySlidesAtStart.Enabled = False
            Me.cbPlaySlidesAtStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbPlaySlidesAtStart.Location = New System.Drawing.Point(542, 174)
            Me.cbPlaySlidesAtStart.Name = "cbPlaySlidesAtStart"
            Me.cbPlaySlidesAtStart.Size = New System.Drawing.Size(299, 22)
            Me.cbPlaySlidesAtStart.TabIndex = 108
            Me.cbPlaySlidesAtStart.Text = "Play this Slideshow when program starts"
            Me.cbPlaySlidesAtStart.UseVisualStyleBackColor = False
            '
            'Label16
            '
            Me.Label16.BackColor = System.Drawing.Color.Transparent
            Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label16.Location = New System.Drawing.Point(516, 76)
            Me.Label16.Name = "Label16"
            Me.Label16.Size = New System.Drawing.Size(242, 22)
            Me.Label16.TabIndex = 103
            Me.Label16.Text = "Default seconds between slides:"
            Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'nudDefaultSlideDelay
            '
            Me.nudDefaultSlideDelay.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.nudDefaultSlideDelay.Location = New System.Drawing.Point(785, 76)
            Me.nudDefaultSlideDelay.Maximum = New Decimal(New Integer() {60, 0, 0, 0})
            Me.nudDefaultSlideDelay.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.nudDefaultSlideDelay.Name = "nudDefaultSlideDelay"
            Me.nudDefaultSlideDelay.Size = New System.Drawing.Size(56, 24)
            Me.nudDefaultSlideDelay.TabIndex = 104
            Me.nudDefaultSlideDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.nudDefaultSlideDelay.Value = New Decimal(New Integer() {15, 0, 0, 0})
            '
            'tbDefaultSlideShow
            '
            Me.tbDefaultSlideShow.BackColor = System.Drawing.SystemColors.ControlDark
            Me.tbDefaultSlideShow.Enabled = False
            Me.tbDefaultSlideShow.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbDefaultSlideShow.Location = New System.Drawing.Point(531, 145)
            Me.tbDefaultSlideShow.Name = "tbDefaultSlideShow"
            Me.tbDefaultSlideShow.ReadOnly = True
            Me.tbDefaultSlideShow.Size = New System.Drawing.Size(365, 23)
            Me.tbDefaultSlideShow.TabIndex = 6
            Me.tbDefaultSlideShow.TabStop = False
            '
            'btnChooseDefaultSlideShow
            '
            Me.btnChooseDefaultSlideShow.Enabled = False
            Me.btnChooseDefaultSlideShow.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnChooseDefaultSlideShow.Location = New System.Drawing.Point(902, 143)
            Me.btnChooseDefaultSlideShow.Name = "btnChooseDefaultSlideShow"
            Me.btnChooseDefaultSlideShow.Size = New System.Drawing.Size(72, 26)
            Me.btnChooseDefaultSlideShow.TabIndex = 107
            Me.btnChooseDefaultSlideShow.Tag = "0"
            Me.btnChooseDefaultSlideShow.Text = "Select..."
            '
            'tbDefaultHBFile
            '
            Me.tbDefaultHBFile.BackColor = System.Drawing.SystemColors.ControlDark
            Me.tbDefaultHBFile.Enabled = False
            Me.tbDefaultHBFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbDefaultHBFile.Location = New System.Drawing.Point(25, 284)
            Me.tbDefaultHBFile.Name = "tbDefaultHBFile"
            Me.tbDefaultHBFile.ReadOnly = True
            Me.tbDefaultHBFile.Size = New System.Drawing.Size(366, 23)
            Me.tbDefaultHBFile.TabIndex = 101
            Me.tbDefaultHBFile.TabStop = False
            '
            'btnChooseDefaultHB
            '
            Me.btnChooseDefaultHB.Enabled = False
            Me.btnChooseDefaultHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnChooseDefaultHB.Location = New System.Drawing.Point(397, 282)
            Me.btnChooseDefaultHB.Name = "btnChooseDefaultHB"
            Me.btnChooseDefaultHB.Size = New System.Drawing.Size(72, 26)
            Me.btnChooseDefaultHB.TabIndex = 102
            Me.btnChooseDefaultHB.Tag = "0"
            Me.btnChooseDefaultHB.Text = "Select..."
            '
            'tbDefaultImageFile
            '
            Me.tbDefaultImageFile.BackColor = System.Drawing.SystemColors.ControlDark
            Me.tbDefaultImageFile.Enabled = False
            Me.tbDefaultImageFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbDefaultImageFile.Location = New System.Drawing.Point(27, 227)
            Me.tbDefaultImageFile.Name = "tbDefaultImageFile"
            Me.tbDefaultImageFile.ReadOnly = True
            Me.tbDefaultImageFile.Size = New System.Drawing.Size(366, 23)
            Me.tbDefaultImageFile.TabIndex = 98
            Me.tbDefaultImageFile.TabStop = False
            '
            'btnChooseDefaultImage
            '
            Me.btnChooseDefaultImage.Enabled = False
            Me.btnChooseDefaultImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnChooseDefaultImage.Location = New System.Drawing.Point(399, 225)
            Me.btnChooseDefaultImage.Name = "btnChooseDefaultImage"
            Me.btnChooseDefaultImage.Size = New System.Drawing.Size(72, 26)
            Me.btnChooseDefaultImage.TabIndex = 99
            Me.btnChooseDefaultImage.Tag = "0"
            Me.btnChooseDefaultImage.Text = "Select..."
            '
            'cbDisplayDefaultImage
            '
            Me.cbDisplayDefaultImage.BackColor = System.Drawing.Color.Transparent
            Me.cbDisplayDefaultImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbDisplayDefaultImage.Location = New System.Drawing.Point(8, 199)
            Me.cbDisplayDefaultImage.Name = "cbDisplayDefaultImage"
            Me.cbDisplayDefaultImage.Size = New System.Drawing.Size(240, 22)
            Me.cbDisplayDefaultImage.TabIndex = 97
            Me.cbDisplayDefaultImage.Text = "Display image at program start:"
            Me.cbDisplayDefaultImage.UseVisualStyleBackColor = False
            '
            'cbLoadDefaultSlides
            '
            Me.cbLoadDefaultSlides.BackColor = System.Drawing.Color.Transparent
            Me.cbLoadDefaultSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbLoadDefaultSlides.Location = New System.Drawing.Point(519, 117)
            Me.cbLoadDefaultSlides.Name = "cbLoadDefaultSlides"
            Me.cbLoadDefaultSlides.Size = New System.Drawing.Size(242, 22)
            Me.cbLoadDefaultSlides.TabIndex = 105
            Me.cbLoadDefaultSlides.Text = "Load a SlideShow file as default:"
            Me.cbLoadDefaultSlides.UseVisualStyleBackColor = False
            '
            'cbLoadDefaultHB
            '
            Me.cbLoadDefaultHB.BackColor = System.Drawing.Color.Transparent
            Me.cbLoadDefaultHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbLoadDefaultHB.Location = New System.Drawing.Point(6, 256)
            Me.cbLoadDefaultHB.Name = "cbLoadDefaultHB"
            Me.cbLoadDefaultHB.Size = New System.Drawing.Size(242, 22)
            Me.cbLoadDefaultHB.TabIndex = 100
            Me.cbLoadDefaultHB.Text = "Load a HotButtons file at start:"
            Me.cbLoadDefaultHB.UseVisualStyleBackColor = False
            '
            'tpAbout
            '
            Me.tpAbout.BackColor = System.Drawing.SystemColors.Window
            Me.tpAbout.Controls.Add(Me.TextBox2)
            Me.tpAbout.Controls.Add(Me.TextBox1)
            Me.tpAbout.Location = New System.Drawing.Point(4, 28)
            Me.tpAbout.Name = "tpAbout"
            Me.tpAbout.Size = New System.Drawing.Size(988, 372)
            Me.tpAbout.TabIndex = 3
            Me.tpAbout.Text = "About JANIS"
            '
            'TextBox2
            '
            Me.TextBox2.BackColor = System.Drawing.SystemColors.Window
            Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.TextBox2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.TextBox2.Location = New System.Drawing.Point(8, 144)
            Me.TextBox2.Multiline = True
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.ReadOnly = True
            Me.TextBox2.Size = New System.Drawing.Size(972, 232)
            Me.TextBox2.TabIndex = 91
            Me.TextBox2.TabStop = False
            Me.TextBox2.Text = resources.GetString("TextBox2.Text")
            Me.TextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TextBox1
            '
            Me.TextBox1.BackColor = System.Drawing.SystemColors.Window
            Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.TextBox1.Font = New System.Drawing.Font("Lucida Fax", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.TextBox1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.TextBox1.Location = New System.Drawing.Point(8, 20)
            Me.TextBox1.Multiline = True
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.ReadOnly = True
            Me.TextBox1.Size = New System.Drawing.Size(972, 118)
            Me.TextBox1.TabIndex = 90
            Me.TextBox1.TabStop = False
            Me.TextBox1.Text = "JANIS Dual Display" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Version 3.3 Released August 19, 2018" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "by Bill Cernansky ( bil" & _
        "l@easybeing.com )" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "© 2003-2018 Easy Being Productions"
            Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'SlideTimer
            '
            Me.SlideTimer.Interval = 8000
            '
            'btnHot1
            '
            Me.btnHot1.AllowDrop = True
            Me.btnHot1.BackColor = System.Drawing.Color.Gray
            Me.btnHot1.BackgroundImage = Global.JANIS.My.Resources.Resources.HotButton_Divided
            Me.btnHot1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.btnHot1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot1.ForeColor = System.Drawing.Color.White
            Me.btnHot1.Location = New System.Drawing.Point(4, 266)
            Me.btnHot1.Name = "btnHot1"
            Me.btnHot1.Size = New System.Drawing.Size(96, 28)
            Me.btnHot1.TabIndex = 31
            Me.btnHot1.TabStop = False
            Me.btnHot1.Tag = "1"
            Me.btnHot1.Text = "Hot 1"
            Me.btnHot1.UseVisualStyleBackColor = False
            '
            'btnHot2
            '
            Me.btnHot2.AllowDrop = True
            Me.btnHot2.BackColor = System.Drawing.Color.Gray
            Me.btnHot2.BackgroundImage = CType(resources.GetObject("btnHot2.BackgroundImage"), System.Drawing.Image)
            Me.btnHot2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.btnHot2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot2.ForeColor = System.Drawing.Color.White
            Me.btnHot2.Location = New System.Drawing.Point(103, 266)
            Me.btnHot2.Name = "btnHot2"
            Me.btnHot2.Size = New System.Drawing.Size(96, 28)
            Me.btnHot2.TabIndex = 32
            Me.btnHot2.Tag = "2"
            Me.btnHot2.Text = "Hot 2"
            Me.btnHot2.UseVisualStyleBackColor = False
            '
            'btnHot3
            '
            Me.btnHot3.AllowDrop = True
            Me.btnHot3.BackColor = System.Drawing.Color.Gray
            Me.btnHot3.BackgroundImage = CType(resources.GetObject("btnHot3.BackgroundImage"), System.Drawing.Image)
            Me.btnHot3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.btnHot3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot3.ForeColor = System.Drawing.Color.White
            Me.btnHot3.Location = New System.Drawing.Point(202, 266)
            Me.btnHot3.Name = "btnHot3"
            Me.btnHot3.Size = New System.Drawing.Size(96, 28)
            Me.btnHot3.TabIndex = 33
            Me.btnHot3.Tag = "3"
            Me.btnHot3.Text = "Hot 3"
            Me.btnHot3.UseVisualStyleBackColor = False
            '
            'btnHot4
            '
            Me.btnHot4.AllowDrop = True
            Me.btnHot4.BackColor = System.Drawing.Color.Gray
            Me.btnHot4.BackgroundImage = CType(resources.GetObject("btnHot4.BackgroundImage"), System.Drawing.Image)
            Me.btnHot4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.btnHot4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot4.ForeColor = System.Drawing.Color.White
            Me.btnHot4.Location = New System.Drawing.Point(300, 266)
            Me.btnHot4.Name = "btnHot4"
            Me.btnHot4.Size = New System.Drawing.Size(96, 28)
            Me.btnHot4.TabIndex = 34
            Me.btnHot4.Tag = "4"
            Me.btnHot4.Text = "Hot 4"
            Me.btnHot4.UseVisualStyleBackColor = False
            '
            'btnHot5
            '
            Me.btnHot5.AllowDrop = True
            Me.btnHot5.BackColor = System.Drawing.Color.Gray
            Me.btnHot5.BackgroundImage = CType(resources.GetObject("btnHot5.BackgroundImage"), System.Drawing.Image)
            Me.btnHot5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.btnHot5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot5.ForeColor = System.Drawing.Color.White
            Me.btnHot5.Location = New System.Drawing.Point(399, 266)
            Me.btnHot5.Name = "btnHot5"
            Me.btnHot5.Size = New System.Drawing.Size(96, 28)
            Me.btnHot5.TabIndex = 35
            Me.btnHot5.Tag = "5"
            Me.btnHot5.Text = "Hot 5"
            Me.btnHot5.UseVisualStyleBackColor = False
            '
            'btnHot6
            '
            Me.btnHot6.AllowDrop = True
            Me.btnHot6.BackColor = System.Drawing.Color.Gray
            Me.btnHot6.BackgroundImage = CType(resources.GetObject("btnHot6.BackgroundImage"), System.Drawing.Image)
            Me.btnHot6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.btnHot6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot6.ForeColor = System.Drawing.Color.White
            Me.btnHot6.Location = New System.Drawing.Point(499, 266)
            Me.btnHot6.Name = "btnHot6"
            Me.btnHot6.Size = New System.Drawing.Size(96, 28)
            Me.btnHot6.TabIndex = 36
            Me.btnHot6.Tag = "6"
            Me.btnHot6.Text = "Hot 6"
            Me.btnHot6.UseVisualStyleBackColor = False
            '
            'btnHot7
            '
            Me.btnHot7.AllowDrop = True
            Me.btnHot7.BackColor = System.Drawing.Color.Gray
            Me.btnHot7.BackgroundImage = CType(resources.GetObject("btnHot7.BackgroundImage"), System.Drawing.Image)
            Me.btnHot7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.btnHot7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot7.ForeColor = System.Drawing.Color.White
            Me.btnHot7.Location = New System.Drawing.Point(598, 266)
            Me.btnHot7.Name = "btnHot7"
            Me.btnHot7.Size = New System.Drawing.Size(96, 28)
            Me.btnHot7.TabIndex = 37
            Me.btnHot7.Tag = "7"
            Me.btnHot7.Text = "Hot 7"
            Me.btnHot7.UseVisualStyleBackColor = False
            '
            'btnHot8
            '
            Me.btnHot8.AllowDrop = True
            Me.btnHot8.BackColor = System.Drawing.Color.Gray
            Me.btnHot8.BackgroundImage = CType(resources.GetObject("btnHot8.BackgroundImage"), System.Drawing.Image)
            Me.btnHot8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.btnHot8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot8.ForeColor = System.Drawing.Color.White
            Me.btnHot8.Location = New System.Drawing.Point(697, 266)
            Me.btnHot8.Name = "btnHot8"
            Me.btnHot8.Size = New System.Drawing.Size(96, 28)
            Me.btnHot8.TabIndex = 38
            Me.btnHot8.Tag = "8"
            Me.btnHot8.Text = "Hot 8"
            Me.btnHot8.UseVisualStyleBackColor = False
            '
            'btnHot9
            '
            Me.btnHot9.AllowDrop = True
            Me.btnHot9.BackColor = System.Drawing.Color.Gray
            Me.btnHot9.BackgroundImage = CType(resources.GetObject("btnHot9.BackgroundImage"), System.Drawing.Image)
            Me.btnHot9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.btnHot9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot9.ForeColor = System.Drawing.Color.White
            Me.btnHot9.Location = New System.Drawing.Point(796, 266)
            Me.btnHot9.Name = "btnHot9"
            Me.btnHot9.Size = New System.Drawing.Size(96, 28)
            Me.btnHot9.TabIndex = 39
            Me.btnHot9.Tag = "9"
            Me.btnHot9.Text = "Hot 9"
            Me.btnHot9.UseVisualStyleBackColor = False
            '
            'btnHot10
            '
            Me.btnHot10.AllowDrop = True
            Me.btnHot10.BackColor = System.Drawing.Color.Gray
            Me.btnHot10.BackgroundImage = CType(resources.GetObject("btnHot10.BackgroundImage"), System.Drawing.Image)
            Me.btnHot10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.btnHot10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot10.ForeColor = System.Drawing.Color.White
            Me.btnHot10.Location = New System.Drawing.Point(895, 266)
            Me.btnHot10.Name = "btnHot10"
            Me.btnHot10.Size = New System.Drawing.Size(96, 28)
            Me.btnHot10.TabIndex = 40
            Me.btnHot10.TabStop = False
            Me.btnHot10.Tag = "10"
            Me.btnHot10.Text = "Hot 10"
            Me.btnHot10.UseVisualStyleBackColor = False
            '
            'pnlPicBackLeft
            '
            Me.pnlPicBackLeft.BackColor = System.Drawing.Color.Black
            Me.pnlPicBackLeft.Controls.Add(Me.picLeft)
            Me.pnlPicBackLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.pnlPicBackLeft.Location = New System.Drawing.Point(4, 57)
            Me.pnlPicBackLeft.Name = "pnlPicBackLeft"
            Me.pnlPicBackLeft.Size = New System.Drawing.Size(272, 153)
            Me.pnlPicBackLeft.TabIndex = 41
            '
            'pnlPicBackRight
            '
            Me.pnlPicBackRight.BackColor = System.Drawing.Color.Black
            Me.pnlPicBackRight.Controls.Add(Me.picRight)
            Me.pnlPicBackRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.pnlPicBackRight.Location = New System.Drawing.Point(720, 57)
            Me.pnlPicBackRight.Name = "pnlPicBackRight"
            Me.pnlPicBackRight.Size = New System.Drawing.Size(272, 153)
            Me.pnlPicBackRight.TabIndex = 42
            '
            'picRight
            '
            Me.picRight.BackColor = System.Drawing.Color.Transparent
            Me.picRight.ImageLocation = ""
            Me.picRight.Location = New System.Drawing.Point(0, 0)
            Me.picRight.Name = "picRight"
            Me.picRight.Size = New System.Drawing.Size(272, 153)
            Me.picRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.picRight.TabIndex = 30
            Me.picRight.TabStop = False
            '
            'CountdownTimer
            '
            Me.CountdownTimer.Interval = 10
            '
            'cbExpandPicLeft
            '
            Me.cbExpandPicLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbExpandPicLeft.Location = New System.Drawing.Point(282, 98)
            Me.cbExpandPicLeft.Name = "cbExpandPicLeft"
            Me.cbExpandPicLeft.Size = New System.Drawing.Size(84, 24)
            Me.cbExpandPicLeft.TabIndex = 43
            Me.cbExpandPicLeft.Text = "Expand"
            '
            'cbExpandPicRight
            '
            Me.cbExpandPicRight.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.cbExpandPicRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbExpandPicRight.Location = New System.Drawing.Point(630, 98)
            Me.cbExpandPicRight.Name = "cbExpandPicRight"
            Me.cbExpandPicRight.Size = New System.Drawing.Size(84, 24)
            Me.cbExpandPicRight.TabIndex = 44
            Me.cbExpandPicRight.Text = "Expand"
            Me.cbExpandPicRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblLibraryCount
            '
            Me.lblLibraryCount.BackColor = System.Drawing.Color.PaleTurquoise
            Me.lblLibraryCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblLibraryCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblLibraryCount.Location = New System.Drawing.Point(372, 56)
            Me.lblLibraryCount.Name = "lblLibraryCount"
            Me.lblLibraryCount.Size = New System.Drawing.Size(260, 24)
            Me.lblLibraryCount.TabIndex = 45
            Me.lblLibraryCount.Text = "Images in Search Library:"
            Me.lblLibraryCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.ToolTip1.SetToolTip(Me.lblLibraryCount, "Double-Click this message to re-index the image search library." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Useful if you've" & _
            " added new images while JANIS is running.")
            Me.lblLibraryCount.UseMnemonic = False
            '
            'btnPasteBoth
            '
            Me.btnPasteBoth.AllowDrop = True
            Me.btnPasteBoth.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnPasteBoth.Location = New System.Drawing.Point(59, 18)
            Me.btnPasteBoth.Name = "btnPasteBoth"
            Me.btnPasteBoth.Size = New System.Drawing.Size(47, 28)
            Me.btnPasteBoth.TabIndex = 47
            Me.btnPasteBoth.Text = "ó"
            Me.btnPasteBoth.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.ToolTip1.SetToolTip(Me.btnPasteBoth, "Copy image from browser, etc. and paste here to display.")
            '
            'btnPasteRight
            '
            Me.btnPasteRight.AllowDrop = True
            Me.btnPasteRight.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnPasteRight.Location = New System.Drawing.Point(107, 18)
            Me.btnPasteRight.Name = "btnPasteRight"
            Me.btnPasteRight.Size = New System.Drawing.Size(47, 28)
            Me.btnPasteRight.TabIndex = 48
            Me.btnPasteRight.Text = "ð"
            Me.btnPasteRight.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.ToolTip1.SetToolTip(Me.btnPasteRight, "Copy image from browser, etc. and paste here to display.")
            '
            'btnPasteLeft
            '
            Me.btnPasteLeft.AllowDrop = True
            Me.btnPasteLeft.Font = New System.Drawing.Font("Wingdings", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnPasteLeft.Location = New System.Drawing.Point(11, 18)
            Me.btnPasteLeft.Name = "btnPasteLeft"
            Me.btnPasteLeft.Size = New System.Drawing.Size(47, 28)
            Me.btnPasteLeft.TabIndex = 46
            Me.btnPasteLeft.Text = "ï"
            Me.btnPasteLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.ToolTip1.SetToolTip(Me.btnPasteLeft, "Copy image from browser, etc. and paste here to display.")
            '
            'grpLoadImage
            '
            Me.grpLoadImage.Controls.Add(Me.btnPictureLeft)
            Me.grpLoadImage.Controls.Add(Me.btnPictureBoth)
            Me.grpLoadImage.Controls.Add(Me.btnPictureRight)
            Me.grpLoadImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpLoadImage.Location = New System.Drawing.Point(69, 212)
            Me.grpLoadImage.Name = "grpLoadImage"
            Me.grpLoadImage.Size = New System.Drawing.Size(148, 52)
            Me.grpLoadImage.TabIndex = 49
            Me.grpLoadImage.TabStop = False
            Me.grpLoadImage.Text = "Load Image File"
            '
            'grpPasteImage
            '
            Me.grpPasteImage.Controls.Add(Me.btnPasteLeft)
            Me.grpPasteImage.Controls.Add(Me.btnPasteBoth)
            Me.grpPasteImage.Controls.Add(Me.btnPasteRight)
            Me.grpPasteImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpPasteImage.Location = New System.Drawing.Point(772, 212)
            Me.grpPasteImage.Name = "grpPasteImage"
            Me.grpPasteImage.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.grpPasteImage.Size = New System.Drawing.Size(166, 52)
            Me.grpPasteImage.TabIndex = 50
            Me.grpPasteImage.TabStop = False
            Me.grpPasteImage.Text = "Paste Clipboard Image"
            '
            'gbCountdownControls
            '
            Me.gbCountdownControls.Controls.Add(Me.Label37)
            Me.gbCountdownControls.Controls.Add(Me.Label36)
            Me.gbCountdownControls.Controls.Add(Me.Label35)
            Me.gbCountdownControls.Controls.Add(Me.Label34)
            Me.gbCountdownControls.Controls.Add(Me.nudCountdownWarnSeconds)
            Me.gbCountdownControls.Controls.Add(Me.nudCountdownWarnMinutes)
            Me.gbCountdownControls.Controls.Add(Me.nudCountdownWarnHours)
            Me.gbCountdownControls.Controls.Add(Me.Label33)
            Me.gbCountdownControls.Controls.Add(Me.cbCountdownVisible)
            Me.gbCountdownControls.Controls.Add(Me.nudCountdownSeconds)
            Me.gbCountdownControls.Controls.Add(Me.nudCountdownMinutes)
            Me.gbCountdownControls.Controls.Add(Me.nudCountdownHours)
            Me.gbCountdownControls.Controls.Add(Me.btnResetCountdown)
            Me.gbCountdownControls.Controls.Add(Me.btnStartCountdown)
            Me.gbCountdownControls.Location = New System.Drawing.Point(301, 166)
            Me.gbCountdownControls.Name = "gbCountdownControls"
            Me.gbCountdownControls.Size = New System.Drawing.Size(394, 91)
            Me.gbCountdownControls.TabIndex = 51
            Me.gbCountdownControls.TabStop = False
            Me.gbCountdownControls.Text = "Countdown Timer"
            '
            'Label37
            '
            Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label37.Location = New System.Drawing.Point(263, 11)
            Me.Label37.Name = "Label37"
            Me.Label37.Size = New System.Drawing.Size(40, 16)
            Me.Label37.TabIndex = 70
            Me.Label37.Text = "SS"
            Me.Label37.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'Label36
            '
            Me.Label36.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label36.Location = New System.Drawing.Point(219, 11)
            Me.Label36.Name = "Label36"
            Me.Label36.Size = New System.Drawing.Size(40, 16)
            Me.Label36.TabIndex = 69
            Me.Label36.Text = "MM"
            Me.Label36.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'Label35
            '
            Me.Label35.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label35.Location = New System.Drawing.Point(175, 11)
            Me.Label35.Name = "Label35"
            Me.Label35.Size = New System.Drawing.Size(40, 16)
            Me.Label35.TabIndex = 68
            Me.Label35.Text = "HH"
            Me.Label35.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'Label34
            '
            Me.Label34.ForeColor = System.Drawing.Color.Red
            Me.Label34.Location = New System.Drawing.Point(84, 59)
            Me.Label34.Name = "Label34"
            Me.Label34.Size = New System.Drawing.Size(85, 20)
            Me.Label34.TabIndex = 67
            Me.Label34.Text = "Turn Red:"
            Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'nudCountdownWarnSeconds
            '
            Me.nudCountdownWarnSeconds.Location = New System.Drawing.Point(263, 59)
            Me.nudCountdownWarnSeconds.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
            Me.nudCountdownWarnSeconds.Name = "nudCountdownWarnSeconds"
            Me.nudCountdownWarnSeconds.Size = New System.Drawing.Size(44, 23)
            Me.nudCountdownWarnSeconds.TabIndex = 63
            Me.nudCountdownWarnSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'nudCountdownWarnMinutes
            '
            Me.nudCountdownWarnMinutes.Location = New System.Drawing.Point(219, 59)
            Me.nudCountdownWarnMinutes.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
            Me.nudCountdownWarnMinutes.Name = "nudCountdownWarnMinutes"
            Me.nudCountdownWarnMinutes.Size = New System.Drawing.Size(44, 23)
            Me.nudCountdownWarnMinutes.TabIndex = 62
            Me.nudCountdownWarnMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'nudCountdownWarnHours
            '
            Me.nudCountdownWarnHours.Location = New System.Drawing.Point(175, 59)
            Me.nudCountdownWarnHours.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
            Me.nudCountdownWarnHours.Name = "nudCountdownWarnHours"
            Me.nudCountdownWarnHours.Size = New System.Drawing.Size(44, 23)
            Me.nudCountdownWarnHours.TabIndex = 61
            Me.nudCountdownWarnHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'Label33
            '
            Me.Label33.Location = New System.Drawing.Point(84, 31)
            Me.Label33.Name = "Label33"
            Me.Label33.Size = New System.Drawing.Size(85, 20)
            Me.Label33.TabIndex = 63
            Me.Label33.Text = "Time Left:"
            Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'cbCountdownVisible
            '
            Me.cbCountdownVisible.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbCountdownVisible.Location = New System.Drawing.Point(6, 46)
            Me.cbCountdownVisible.Name = "cbCountdownVisible"
            Me.cbCountdownVisible.Size = New System.Drawing.Size(67, 22)
            Me.cbCountdownVisible.TabIndex = 57
            Me.cbCountdownVisible.Text = "Visible"
            '
            'nudCountdownSeconds
            '
            Me.nudCountdownSeconds.Location = New System.Drawing.Point(263, 31)
            Me.nudCountdownSeconds.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
            Me.nudCountdownSeconds.Name = "nudCountdownSeconds"
            Me.nudCountdownSeconds.Size = New System.Drawing.Size(44, 23)
            Me.nudCountdownSeconds.TabIndex = 60
            Me.nudCountdownSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'nudCountdownMinutes
            '
            Me.nudCountdownMinutes.Location = New System.Drawing.Point(219, 31)
            Me.nudCountdownMinutes.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
            Me.nudCountdownMinutes.Name = "nudCountdownMinutes"
            Me.nudCountdownMinutes.Size = New System.Drawing.Size(44, 23)
            Me.nudCountdownMinutes.TabIndex = 59
            Me.nudCountdownMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.nudCountdownMinutes.Value = New Decimal(New Integer() {5, 0, 0, 0})
            '
            'nudCountdownHours
            '
            Me.nudCountdownHours.Location = New System.Drawing.Point(175, 31)
            Me.nudCountdownHours.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
            Me.nudCountdownHours.Name = "nudCountdownHours"
            Me.nudCountdownHours.Size = New System.Drawing.Size(44, 23)
            Me.nudCountdownHours.TabIndex = 58
            Me.nudCountdownHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'btnResetCountdown
            '
            Me.btnResetCountdown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnResetCountdown.ForeColor = System.Drawing.SystemColors.ControlText
            Me.btnResetCountdown.Location = New System.Drawing.Point(328, 15)
            Me.btnResetCountdown.Name = "btnResetCountdown"
            Me.btnResetCountdown.Size = New System.Drawing.Size(60, 24)
            Me.btnResetCountdown.TabIndex = 64
            Me.btnResetCountdown.Text = "RESET"
            '
            'btnStartCountdown
            '
            Me.btnStartCountdown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnStartCountdown.ForeColor = System.Drawing.Color.Green
            Me.btnStartCountdown.Location = New System.Drawing.Point(328, 59)
            Me.btnStartCountdown.Name = "btnStartCountdown"
            Me.btnStartCountdown.Size = New System.Drawing.Size(60, 24)
            Me.btnStartCountdown.TabIndex = 65
            Me.btnStartCountdown.Text = "START"
            '
            'btnReIndexImgLib
            '
            Me.btnReIndexImgLib.BackColor = System.Drawing.Color.DarkTurquoise
            Me.btnReIndexImgLib.FlatAppearance.BorderSize = 0
            Me.btnReIndexImgLib.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnReIndexImgLib.Font = New System.Drawing.Font("Wingdings 3", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnReIndexImgLib.Location = New System.Drawing.Point(607, 57)
            Me.btnReIndexImgLib.Name = "btnReIndexImgLib"
            Me.btnReIndexImgLib.Size = New System.Drawing.Size(24, 22)
            Me.btnReIndexImgLib.TabIndex = 54
            Me.btnReIndexImgLib.Text = "Q"
            Me.btnReIndexImgLib.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.ToolTip1.SetToolTip(Me.btnReIndexImgLib, "Click here to re-index the image library" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "if you've added or deleted image files." & _
            "")
            Me.btnReIndexImgLib.UseVisualStyleBackColor = False
            '
            'fmMain
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(6, 16)
            Me.ClientSize = New System.Drawing.Size(996, 703)
            Me.Controls.Add(Me.btnReIndexImgLib)
            Me.Controls.Add(Me.gbCountdownControls)
            Me.Controls.Add(Me.grpPasteImage)
            Me.Controls.Add(Me.lblLibraryCount)
            Me.Controls.Add(Me.cbExpandPicRight)
            Me.Controls.Add(Me.grpLoadImage)
            Me.Controls.Add(Me.cbExpandPicLeft)
            Me.Controls.Add(Me.pnlPicBackLeft)
            Me.Controls.Add(Me.btnHot10)
            Me.Controls.Add(Me.btnHot9)
            Me.Controls.Add(Me.btnHot8)
            Me.Controls.Add(Me.btnHot7)
            Me.Controls.Add(Me.btnHot6)
            Me.Controls.Add(Me.btnHot5)
            Me.Controls.Add(Me.btnHot4)
            Me.Controls.Add(Me.btnHot3)
            Me.Controls.Add(Me.btnHot2)
            Me.Controls.Add(Me.btnHot1)
            Me.Controls.Add(Me.btnBlackout)
            Me.Controls.Add(Me.TabControl1)
            Me.Controls.Add(Me.btnRightScoreColor)
            Me.Controls.Add(Me.btnLeftScoreColor)
            Me.Controls.Add(Me.btnScoreBoth)
            Me.Controls.Add(Me.btnScoreRight)
            Me.Controls.Add(Me.btnScoreLeft)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.tbRightScore)
            Me.Controls.Add(Me.tbLeftScore)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.tbRightTeam)
            Me.Controls.Add(Me.tbLeftTeam)
            Me.Controls.Add(Me.pnlPicBackRight)
            Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ForeColor = System.Drawing.SystemColors.WindowText
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Location = New System.Drawing.Point(220, 30)
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(1012, 762)
            Me.Menu = Me.MainMenu1
            Me.Name = "fmMain"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
            Me.Text = "JANIS"
            CType(Me.picLeft, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabControl1.ResumeLayout(False)
            Me.tpScreenText.ResumeLayout(False)
            Me.tpScreenText.PerformLayout()
            Me.GroupBox1.ResumeLayout(False)
            Me.grpShowLeftText.ResumeLayout(False)
            Me.grpLoadText.ResumeLayout(False)
            Me.grpClearText.ResumeLayout(False)
            Me.grpRightColors.ResumeLayout(False)
            Me.grpLeftColors.ResumeLayout(False)
            Me.tpImgSearch.ResumeLayout(False)
            CType(Me.picImgSearchPreview, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tp5Things.ResumeLayout(False)
            Me.tp5Things.PerformLayout()
            Me.grpThingsColor.ResumeLayout(False)
            Me.tpSlides.ResumeLayout(False)
            CType(Me.nudDelay, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picSlidePreview, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpHotButtons.ResumeLayout(False)
            Me.gbHB.ResumeLayout(False)
            Me.gbHB.PerformLayout()
            Me.tpPrefs.ResumeLayout(False)
            Me.tpPrefs.PerformLayout()
            CType(Me.nudDefaultCountdownSeconds, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudDefaultCountdownMinutes, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudDefaultCountdownHours, System.ComponentModel.ISupportInitialize).EndInit()
            Me.GroupBox2.ResumeLayout(False)
            Me.grpDefaultColorsRight.ResumeLayout(False)
            Me.grpDefaultColorsLeft.ResumeLayout(False)
            CType(Me.nudDefaultSlideDelay, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpAbout.ResumeLayout(False)
            Me.tpAbout.PerformLayout()
            Me.pnlPicBackLeft.ResumeLayout(False)
            Me.pnlPicBackRight.ResumeLayout(False)
            CType(Me.picRight, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpLoadImage.ResumeLayout(False)
            Me.grpPasteImage.ResumeLayout(False)
            Me.gbCountdownControls.ResumeLayout(False)
            CType(Me.nudCountdownWarnSeconds, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudCountdownWarnMinutes, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudCountdownWarnHours, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudCountdownSeconds, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudCountdownMinutes, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudCountdownHours, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private Sub fmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Application.EnableVisualStyles()
            Me.Opacity = 0          '* Make invisible until we finish loading
            If Me.AppAlreadyRunning() Then
                '* An autoexit has already been configured with the slide timer.
                Me.splash.Close()
                Me.InitiateQuietShutdown()
            Else
                Me.InitializeSettings()
                Me.picLeft.AllowDrop = True
                Me.picRight.AllowDrop = True

                Me.LS = New fmScreen()
                Me.RS = New fmScreen()

                Me.VerifyInfrastructure()

                Me.splash.SetStatus("Setting Preferences...")

                Me.LoadPrefsFromFile(PREFS_FILE)
                Me.ApplyPrefs()

                '* Here's the wacky way you change font sizes in VB.NET. Piece of crap.
                Me.tbLeftText.Font = New Font(Me.tbLeftText.Font.Name, CSng(Val(Me.tbLeftFontSize.Text) / DisplayToEntryFontRatio), Me.tbLeftText.Font.Style)
                Me.tbRightText.Font = New Font(Me.tbRightText.Font.Name, CSng(Val(Me.tbRightFontSize.Text) / DisplayToEntryFontRatio), Me.tbRightText.Font.Style)

                Me.SetMonitorDisplayMode()

                If Not (Me.cbDisplayDefaultImage.Checked Or Me.cbPlaySlidesAtStart.Checked) Then
                    Me.DisplayTextScreen(Me.LS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, CSng(Me.tbLeftFontSize.Text) * Me.DisplayFontRatio)
                    Me.DisplayTextScreen(Me.RS, Me.tbRightText.Text, Me.tbRightText.BackColor, CSng(Me.tbRightFontSize.Text) * Me.DisplayFontRatio)
                End If

                '* Save the label message heights because the timer screws around with them and needs to restore them
                Me.LS.lblMsg.Tag = Me.LS.lblMsg.Height
                Me.RS.lblMsg.Tag = Me.RS.lblMsg.Height

                '* Show the audience display monitors early, because it might take time to
                '* load the image library and we don't need the audience to see the O/S desktop.
                Me.LS.Show()
                Me.RS.Show()

                Me.FTreeAutoExpand(Me.ExpTree1, Me.tbDefaultImageDir.Text)

                Me.splash.SetStatus("Building Image Library...")
                Me.BuildImageLibrary(Me.tbDefaultImageDir.Text)
                Me.Opacity = 100          '* Make visible again
            End If
        End Sub

        Private Sub fmMain_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
            If Not SlideTimerTag = "AppAlreadyRunning" Then
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
            End If
        End Sub
        Private Function AppAlreadyRunning() As Boolean
            ' Check for the name of the current applications process and see whether or not there are
            ' more than 1x instance loaded. This code is similar to Visual Basic 6.0's App.Previnstance feature.
            Dim appName As String = Process.GetCurrentProcess.ProcessName
            Dim sameProcessTotal As Integer = Process.GetProcessesByName(appName).Length

            If sameProcessTotal > 1 Then
                Return True
            End If

            Return False
        End Function
        Private Sub InitiateQuietShutdown()
            '* Make the main form tiny = apparently invisible. Setting visible property to false doesn't work.
            Me.Width = 1
            Me.Height = 1
            Me.Left = 0
            Me.Top = 0
            MessageBox.Show("A previous instance of JANIS is already open!", "Already Running", _
                            MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' This instance will self-destruct in 5 seconds.
            Me.SlideTimer.Interval = 5000
            Me.SlideTimerTag = "AppAlreadyRunning"
            Me.SlideTimer.Enabled = True
            Me.SlideTimer.Start()
        End Sub

        Private Sub InitializeSettings()
            '* If this is Windows Vista or newer, different default home paths.
            If OSVersion.Version.Major >= 6 Then
                '* Vista or newer
                ROOT_SUPPORT_DIR = "C:\JANIS"
            Else
                '* XP or older
                ROOT_SUPPORT_DIR = "C:\Program Files\JANIS"
            End If
            PREFS_FILE = ROOT_SUPPORT_DIR + "\JANIS.ini"

            '* Get the working dimensions of the primary monitor
            Dim workingArea As System.Drawing.Rectangle
            workingArea = Screen.GetWorkingArea(New System.Drawing.Point(200, 200))

            Me.Top = workingArea.Top
            Me.Left = SystemInformation.PrimaryMonitorSize.Width - Me.Size.Width

            '* Here's what we want to see or hide.
            Me.tbCurrentThing.Visible = False
            Me.tbSubstitutions.Visible = False
            Me.btnShowThingLeft.Visible = False
            Me.btnShowThingRight.Visible = False
            Me.btnShowThingBoth.Visible = False

            Me.HotButtonsChanged = False
            Me.cbHBActive.Checked = True    '* default to "can see"
        End Sub
        Private Sub VerifyInfrastructure()
            '* If the default support dirs aren't there, create them
            Dim MyDir As String = Dir(ROOT_SUPPORT_DIR, FileAttribute.Directory)
            If MyDir = "" Then MkDir(ROOT_SUPPORT_DIR)
            MyDir = Dir(ROOT_SUPPORT_DIR + DEFAULT_SLIDESHOW_DIR, FileAttribute.Directory)
            If MyDir = "" Then MkDir(ROOT_SUPPORT_DIR + DEFAULT_SLIDESHOW_DIR)
            MyDir = Dir(ROOT_SUPPORT_DIR + DEFAULT_HOTBUTTON_DIR, FileAttribute.Directory)
            If MyDir = "" Then MkDir(ROOT_SUPPORT_DIR + DEFAULT_HOTBUTTON_DIR)
        End Sub

        Private Sub ListBox_KeyPress(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles FileListBox1.KeyPress, lbSlideList.KeyPress
            ' Ctrl-A will select all.
            If e.KeyChar = Chr(1) Then
                If sender.SelectionMode.ToString Like "*Multi*" Then
                    '* Walk through and select all items in the listbox
                    Dim i As Integer
                    For i = 0 To sender.Items.Count - 1
                        sender.SetSelected(i, True)
                    Next i
                End If
                e.Handled = True
            End If
        End Sub
        Private Sub NumericEntry_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbLeftFontSize.KeyPress, tbRightFontSize.KeyPress, tbDefaultFontSize.KeyPress, tbLeftScore.KeyPress, tbRightScore.KeyPress
            ' Chr(1) is Ctrl-A. Chr(8) is BackSpace.
            If e.KeyChar = Chr(1) Then
                sender.SelectAll()
                e.Handled = True
            ElseIf (e.KeyChar <> Chr(8)) And (e.KeyChar <> "-") And ((e.KeyChar < "0") Or (e.KeyChar > "9")) Then
                'tbLeftText.Text = CStr(Asc(e.KeyChar))
                e.Handled = True
            End If
        End Sub
        Private Sub tbTextEntry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbLeftText.KeyPress, tbRightText.KeyPress, tbLeftTeam.KeyPress, tbRightTeam.KeyPress, comboImgSearchText.KeyPress, tbSubstitutions.KeyPress, tbNewThing.KeyPress, tbHBtext1.KeyPress, tbHBtext2.KeyPress, tbHBtext3.KeyPress, tbHBtext4.KeyPress, tbHBtext5.KeyPress, tbHBtext6.KeyPress, tbHBtext7.KeyPress, tbHBtext8.KeyPress, tbHBtext9.KeyPress, tbHBtext10.KeyPress
            ' Ctrl-A will select all.
            If e.KeyChar = Chr(1) Then
                sender.SelectAll()
                e.Handled = True
            End If
        End Sub

        Private Sub btnScoreLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScoreLeft.Click
            Me.picLeft.Image = Nothing
            Me.DisplayScore(Me.LS, Me.tbLeftTeam.Text, Me.tbLeftScore.Text, tbLeftScore.BackColor)
        End Sub
        Private Sub btnScoreRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScoreRight.Click
            Me.picRight.Image = Nothing
            Me.DisplayScore(Me.RS, Me.tbRightTeam.Text, Me.tbRightScore.Text, Me.tbRightScore.BackColor)
        End Sub
        Private Sub btnScoreBoth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScoreBoth.Click
            Me.DisplayBothScores()  ' Clears the local pic images itself.
        End Sub
        Private Sub btnLeftScoreColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeftScoreColor.Click
            Dim newcolor As Color = PickColor(sender.Left, sender.Top, tbLeftScore.BackColor)
            Me.SetTeamColor("Left", newcolor)
        End Sub
        Private Sub btnRightScoreColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRightScoreColor.Click
            Dim newcolor As Color = PickColor(sender.Left - 100, sender.Top, tbRightScore.BackColor)
            Me.SetTeamColor("Right", newcolor)
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
            Me.UpdateHotButtonColor(side, newcolor)
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
            Me.btnClearTextLeft_Click(sender, e)
            Me.btnClearTextRight_Click(sender, e)
            Me.tbLeftText.Focus()
        End Sub
        Private Sub btnShowLeftText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowLeftText.Click
            Me.DisplayTextScreen(Me.LS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, CSng(Me.tbLeftFontSize.Text) * Me.DisplayFontRatio)
        End Sub
        Private Sub btnShowRightText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowRightText.Click
            Me.DisplayTextScreen(Me.RS, Me.tbRightText.Text, Me.tbRightText.BackColor, CSng(Me.tbRightFontSize.Text) * Me.DisplayFontRatio)
        End Sub
        Private Sub btnLeftTextRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeftTextRight.Click
            Me.DisplayTextScreen(Me.RS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, CSng(Me.tbLeftFontSize.Text) * Me.DisplayFontRatio)
        End Sub
        Private Sub btnRightTextLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRightTextLeft.Click
            Me.DisplayTextScreen(Me.LS, Me.tbRightText.Text, Me.tbRightText.BackColor, CSng(Me.tbRightFontSize.Text) * Me.DisplayFontRatio)
        End Sub
        Private Sub btnLeftTextBoth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeftTextBoth.Click
            Me.DisplayTextScreen(Me.LS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, CSng(Me.tbLeftFontSize.Text) * Me.DisplayFontRatio)
            Me.DisplayTextScreen(Me.RS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, CSng(Me.tbLeftFontSize.Text) * Me.DisplayFontRatio)
        End Sub
        Private Sub btnRightTextBoth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRightTextBoth.Click
            Me.DisplayTextScreen(Me.LS, Me.tbRightText.Text, Me.tbRightText.BackColor, CSng(Me.tbRightFontSize.Text) * Me.DisplayFontRatio)
            Me.DisplayTextScreen(Me.RS, Me.tbRightText.Text, Me.tbRightText.BackColor, CSng(Me.tbRightFontSize.Text) * Me.DisplayFontRatio)
        End Sub
        Private Sub btnBothTextScreens_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBothTextScreens.Click
            Me.DisplayTextScreen(Me.LS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, CSng(Me.tbLeftFontSize.Text) * Me.DisplayFontRatio)
            Me.DisplayTextScreen(Me.RS, Me.tbRightText.Text, Me.tbRightText.BackColor, CSng(Me.tbRightFontSize.Text) * Me.DisplayFontRatio)
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
            Me.AllScreensToFront()
        End Sub

        Private Function LoadDoc() As String
            Dim Doc As String = ""
            Dim FileErr As Boolean = False
            Dim [of] As New OpenFileDialog()
            [of].Filter = "Text File (*.TXT)|*.TXT"
            [of].InitialDirectory = ROOT_SUPPORT_DIR
            If [of].ShowDialog(Me) = DialogResult.OK Then
                Dim fn As Integer = FreeFile()
                Try
                    FileOpen(fn, [of].FileName, OpenMode.Input)
                Catch e As Exception
                    FileErr = True
                    MessageBox.Show("An error occurred opening file '" + [of].FileName + "'.", "File Error")
                End Try
                If Not FileErr Then
                    Doc = InputString(fn, LOF(fn))
                    FileClose(fn)
                End If
            End If
            [of].Dispose()
            Return Doc
        End Function

        Private Sub pnlTextColorLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlTextColorLeft1.Click, pnlTextColorLeft2.Click, pnlTextColorLeft3.Click, pnlTextColorLeft4.Click, pnlTextColorLeft5.Click, pnlTextColorLeft6.Click
            Me.tbLeftText.BackColor = sender.BackColor
        End Sub
        Private Sub pnlTextColorRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlTextColorRight1.Click, pnlTextColorRight2.Click, pnlTextColorRight3.Click, pnlTextColorRight4.Click, pnlTextColorRight5.Click, pnlTextColorRight6.Click
            Me.tbRightText.BackColor = sender.BackColor
        End Sub
        Private Sub btnChooseTextColorLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseTextColorLeft.Click
            Me.tbLeftText.BackColor = Me.PickColor(50, 300, Me.tbLeftText.BackColor)
        End Sub
        Private Sub btnChooseTextColorRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseTextColorRight.Click
            Me.tbRightText.BackColor = PickColor(450, 300, Me.tbRightText.BackColor)
            'Me.tbRightText.BackColor = ChooseColor(Me.tbRightText.BackColor)
        End Sub

        Private Sub btnBlackout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlackout.Click
            '* First, stop the slideshow if it's running.
            Me.StopSlideShow()

            '* Shut them down, Artoo! Shut them all down!
            Me.LS.Blackout()
            Me.RS.Blackout()
            Me.picLeft.ImageLocation = ""
            Me.picLeft.Image = Nothing
            Me.picLeft.ImageLocation = ""
            Me.picRight.Image = Nothing
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
                Me.tbLeftText.Font = New Font(Me.tbLeftText.Font.Name, (fsiz / Me.DisplayToEntryFontRatio), Me.tbLeftText.Font.Style)
            ElseIf sender.Name = "tbRightFontSize" Then
                Me.tbRightText.Font = New Font(Me.tbRightText.Font.Name, (fsiz / Me.DisplayToEntryFontRatio), Me.tbRightText.Font.Style)
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
            If SystemInformation.MonitorCount <= 2 Then        '* We're in test mode
                Dim sRatio As Integer = 4
                Me.TestMode = True
                Me.DisplayModeAdjustment = sRatio * sRatio  '* = w x h

                With Me.LS
                    .Left = Me.Left
                    .Top = Me.Height + Me.Top
                    .Height = .Height / sRatio
                    .Width = .Width / sRatio
                    .lblScore.Left = .lblScore.Left / sRatio
                    .lblScore.Top = .lblScore.Top / sRatio
                    .lblScore.Height = .lblScore.Height / sRatio
                    .lblScore.Width = .lblScore.Width / sRatio
                    .lblScore.Font = New Font(.lblScore.Font.Name, CSng(Val(.lblScore.Font.Size) / sRatio), .lblScore.Font.Style)
                    .lblTeamName.Left = .lblTeamName.Left / sRatio
                    .lblTeamName.Top = .lblTeamName.Top / sRatio
                    .lblTeamName.Height = .lblTeamName.Height / sRatio
                    .lblTeamName.Width = .lblTeamName.Width / sRatio
                    .lblTeamName.Font = New Font(.lblTeamName.Font.Name, CSng(Val(.lblTeamName.Font.Size) / sRatio), .lblTeamName.Font.Style)
                    .lblMsg.Left = .lblMsg.Left / sRatio
                    .lblMsg.Top = .lblMsg.Top / sRatio
                    .lblMsg.Height = .lblMsg.Height / sRatio
                    .lblMsg.Width = .lblMsg.Width / sRatio
                    .lblMsg.Font = New Font(.lblMsg.Font.Name, CSng(Val(.lblMsg.Font.Size) / sRatio), .lblMsg.Font.Style)
                    .lblCountdown.Left = .lblCountdown.Left / sRatio
                    .lblCountdown.Top = .lblCountdown.Top / sRatio
                    .lblCountdown.Height = .lblCountdown.Height / sRatio
                    .lblCountdown.Width = .lblCountdown.Width / sRatio
                    .lblCountdown.Font = New Font(.lblCountdown.Font.Name, CSng(Val(.lblCountdown.Font.Size) / sRatio), .lblCountdown.Font.Style)
                    .picGraphic.Left = .picGraphic.Left / sRatio
                    .picGraphic.Top = .picGraphic.Top / sRatio
                    .picGraphic.Height = .picGraphic.Height / sRatio
                    .picGraphic.Width = .picGraphic.Width / sRatio
                End With

                With Me.RS
                    .Left = Me.Left + Me.Width - (.Width / sRatio)
                    .Top = Me.LS.Top
                    .Height = .Height / sRatio
                    .Width = .Width / sRatio
                    .lblScore.Left = .lblScore.Left / sRatio
                    .lblScore.Top = .lblScore.Top / sRatio
                    .lblScore.Height = .lblScore.Height / sRatio
                    .lblScore.Width = .lblScore.Width / sRatio
                    .lblScore.Font = New Font(.lblScore.Font.Name, CSng(Val(.lblScore.Font.Size) / sRatio), .lblScore.Font.Style)
                    .lblTeamName.Left = .lblTeamName.Left / sRatio
                    .lblTeamName.Top = .lblTeamName.Top / sRatio
                    .lblTeamName.Height = .lblTeamName.Height / sRatio
                    .lblTeamName.Width = .lblTeamName.Width / sRatio
                    .lblTeamName.Font = New Font(.lblTeamName.Font.Name, CSng(Val(.lblTeamName.Font.Size) / sRatio), .lblTeamName.Font.Style)
                    .lblMsg.Left = .lblMsg.Left / sRatio
                    .lblMsg.Top = .lblMsg.Top / sRatio
                    .lblMsg.Height = .lblMsg.Height / sRatio
                    .lblMsg.Width = .lblMsg.Width / sRatio
                    .lblMsg.Font = New Font(.lblMsg.Font.Name, CSng(Val(.lblMsg.Font.Size) / sRatio), .lblMsg.Font.Style)
                    .lblCountdown.Left = .lblCountdown.Left / sRatio
                    .lblCountdown.Top = .lblCountdown.Top / sRatio
                    .lblCountdown.Height = .lblCountdown.Height / sRatio
                    .lblCountdown.Width = .lblCountdown.Width / sRatio
                    .lblCountdown.Font = New Font(.lblCountdown.Font.Name, CSng(Val(.lblCountdown.Font.Size) / sRatio), .lblCountdown.Font.Style)
                    .picGraphic.Left = .picGraphic.Left / sRatio
                    .picGraphic.Top = .picGraphic.Top / sRatio
                    .picGraphic.Height = .picGraphic.Height / sRatio
                    .picGraphic.Width = .picGraphic.Width / sRatio
                End With
                Me.tbRightText.Text = "TEST MODE"
                Me.Text = Me.Text + "   **** TEST MODE ****"
            Else  ' Full blown 3-monitor mode
                '* Dim Scrs As Screen() = System.Windows.Forms.Screen.AllScreens
                Me.LS.Left = SystemInformation.PrimaryMonitorSize.Width  'Don't really need to set this, but I'm doing it anyway
                ' TESTING ONLY Me.LS.Left = Me.Left + Me.Width + 1
                Me.RS.Left = Me.LS.Left + Me.LS.Width 'Right screen starts where the left screen stops
                Me.tbRightText.Text = "Arena Mode"
                Me.Text = Me.Text + " - Arena Mode"
            End If
            Me.tbRightText.Text = Me.tbRightText.Text + EOL + EOL + SystemInformation.MonitorCount.ToString + " monitors found"
        End Sub

        Private Sub DisplayTextScreen(ByVal Scr As fmScreen, ByVal s As String, ByVal hue As Color, ByVal fontsize As Single)
            '* Stop the slideshow if it's running.
            Me.StopSlideShow()

            '* Blank out the corresponding graphics preview
            If Scr Is Me.LS Then
                Me.picLeft.Image = Nothing
            ElseIf Scr Is Me.RS Then
                Me.picRight.Image = Nothing
            End If

            If TestMode Then fontsize = fontsize * 4.006
            Scr.SetTextShadows(cbShadowsEnabled.Checked)
            Scr.ShowText(s, hue, CSng(Me.DisplayToEntryFontRatio * fontsize / Me.DisplayModeAdjustment))
        End Sub

        Private Sub DisplayScore(ByVal Scr As fmScreen, ByVal Team As String, ByVal Score As String, ByVal BackColor As System.Drawing.Color)
            '* First, stop the slideshow if it's running.
            Me.StopSlideShow()
            Scr.ShowScore(Score, Team, BackColor)
        End Sub

        Private Sub DisplayBothScores()
            Me.picLeft.ImageLocation = ""
            Me.picLeft.Image = Nothing
            Me.DisplayScore(Me.LS, Me.tbLeftTeam.Text, Me.tbLeftScore.Text, Me.tbLeftScore.BackColor)
            Me.picRight.ImageLocation = ""
            Me.picRight.Image = Nothing
            Me.DisplayScore(Me.RS, Me.tbRightTeam.Text, Me.tbRightScore.Text, Me.tbRightScore.BackColor)
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
            tbSource.Text = score.ToString
            Me.DisplayBothScores()
        End Sub

        Private Sub menuChangeScore(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles menuAdd1Left.Click, menuSubtract1Left.Click, menuAdd5Left.Click, menuSubtract5Left.Click, menuAdd1Right.Click, menuSubtract1Right.Click, menuAdd5Right.Click, menuSubtract5Right.Click
            'This is the function that does function-key mod of the score.
            'The function keys are bound to this (invisible) main menu object group
            Dim MenuInfo() As String = Split(sender.Text)
            Dim Points As Integer = Val(MenuInfo(0))
            Me.AddScore(MenuInfo(1), Points)
        End Sub

        Private Function PickColor(ByVal X As Integer, ByVal Y As Integer, ByVal hue As Color) As Color
            Dim choose As New fmColorDialog()
            X += Me.Left
            Y += Me.Top
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
            Me.Activate()
        End Sub

        Public Function AskIfSure(ByVal prompt As String) As Boolean
            '* WHEN THIS FUNCTION EXITS, THE APP GIVES UP FOCUS TO BACKGROUND APPS. BUG.
            Dim App As Process = Process.GetCurrentProcess
            Dim response As Boolean = False

            If MessageBox.Show(prompt, "Are You Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly) = DialogResult.Yes Then
                response = True
            End If

            '* attempt to grab focus back from Windows, because the bug in .NET causes a different app to suddenly gain focus.
            AppActivate(App.Id)

            Return response
        End Function

        '=================================================================================================
        '* BEGIN IMAGE STUFF

        Private Sub PreviewImage(ByRef picture As PictureBox, ByRef Img As Image, ByVal Expand As Boolean)
            If Img Is Nothing Then Exit Sub

            Dim backpanel As Panel
            If picture.Name = "picLeft" Then backpanel = Me.pnlPicBackLeft Else backpanel = Me.pnlPicBackRight

            'picture.Visible = False
            backpanel.BackColor = System.Drawing.Color.Black
            If Expand Then
                picture.SizeMode = PictureBoxSizeMode.StretchImage
            Else
                picture.SizeMode = PictureBoxSizeMode.Zoom
            End If

            picture.Image = Img
            picture.Visible = True
        End Sub
        'Private Sub PreviewURL(ByRef picture As PictureBox, ByVal url As String)
        '    If url = "" Then Exit Sub

        '    Dim backpanel As Panel
        '    If picture.Name = "picLeft" Then backpanel = Me.pnlPicBackLeft Else backpanel = Me.pnlPicBackRight

        '    'picture.Visible = False

        '    '* when loading URL, we always maintain size ratio because we don't know pic info.
        '    backpanel.BackColor = System.Drawing.Color.Black
        '    picture.SizeMode = PictureBoxSizeMode.Zoom

        '    Try
        '        picture.Load(url)
        '        picture.Visible = True
        '    Catch ex As Exception
        '    End Try
        'End Sub

        Private Sub Present_Image(ByVal controlName As String, ByRef img As Image)
            '* Display this image to the proper displays and previews, whether controlName
            '* contains "Both", "Right", or "Left".

            If img IsNot Nothing Then
                Me.StopSlideShow()

                '* Show displays first for speed. Handles right, left or both.
                If Not controlName Like "*Right*" Then
                    Me.LS.ShowImage(img, Me.cbExpandPicLeft.Checked)
                End If
                If Not controlName Like "*Left*" Then
                    Me.RS.ShowImage(img, Me.cbExpandPicRight.Checked)
                End If
                If Not controlName Like "*Right*" Then
                    Me.PreviewImage(Me.picLeft, img, Me.cbExpandPicLeft.Checked)
                End If
                If Not controlName Like "*Left*" Then
                    Me.PreviewImage(Me.picRight, img, Me.cbExpandPicRight.Checked)
                End If

                Me.AllScreensToFront()
            End If
        End Sub

        Private Sub DisplayRawImageBoth(ByRef img As Image, ByVal expand As Boolean)
            If img Is Nothing Then Exit Sub
            Me.LS.ShowImage(img, expand)
            Me.RS.ShowImage(img, expand)
        End Sub

        Private Sub DisplayImageFile(ByVal Side As String, ByVal fnam As String)
            ''* If this is a web image, call with filename.
            'If fnam Like "http:*" Then
            '    Me.LS.ShowURLImage(fnam)
            '    Me.RS.ShowURLImage(fnam)
            '    Me.PreviewURL(Me.picLeft, fnam)
            '    Me.PreviewURL(Me.picRight, fnam)
            'Else
            Dim img As Image
            Try
                img = Image.FromFile(fnam)
            Catch ex As Exception
                img = Nothing
                'Exit Sub
            End Try
            Me.Present_Image(Side, img)
            'End If
        End Sub

        Private Function SelectImageFilename() As String
            '* Returns blank if you don't pick a legal file
            Dim fn As String = ""
            Dim [of] As New OpenFileDialog()
            With [of]
                .Filter = "Image Files(*.BMP;*.GIF;*.JPG;*.PNG;*.WMF)|*.BMP;*.GIF;*.JPG;*.PNG;*.WMF"
                .InitialDirectory = Me.tbDefaultImageDir.Text
                If .ShowDialog(Me) = DialogResult.OK Then
                    fn = .FileName
                End If
                .Dispose()        '* We made a new one, we have to dispose of it.
            End With
            Return fn
        End Function

        Private Sub btnPicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPictureBoth.Click, btnPictureLeft.Click, btnPictureRight.Click
            Dim fn As String = SelectImageFilename()
            If fn <> "" Then
                Dim img As Image

                Try
                    img = Image.FromFile(fn)
                    Me.Present_Image(sender.Name, img)
                Catch
                    '* Do nothing. Don't show the nonexistent image.
                End Try
            End If
        End Sub

        Private Sub btnPasteImg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPasteLeft.Click, btnPasteBoth.Click, btnPasteRight.Click
            '* If there's a bitmap in the clipboard, paste it to the screens.
            If Clipboard.GetDataObject.GetDataPresent(GetType(System.Drawing.Bitmap)) Then
                Dim img As Image = CType(Clipboard.GetDataObject.GetData(GetType(System.Drawing.Bitmap)), Bitmap)
                Me.Present_Image(sender.Name, img)
            Else
                MessageBox.Show("There is no usable image in the clipboard. Please copy an image to the clipboard and try again.", "Can't Paste Image")
            End If
        End Sub

        Private Sub picDisplay_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles picLeft.DragDrop, picRight.DragDrop, btnPictureBoth.DragDrop, btnPictureLeft.DragDrop, btnPictureRight.DragDrop, btnPasteBoth.DragDrop, btnPasteLeft.DragDrop, btnPasteRight.DragDrop
            '* Hopefully, we can drop an image on one of these picture boxes and have it
            '* display there. This works really well with Firefox. IE, of course, doesn't do
            '* things nice. If you wanna use IE, gotta use copy/paste instead of drag/drop.
            Dim img As Image

            '* We've limited this to only 2 kinds of data in picDisplay_DragEnter.
            If e.Data.GetDataPresent(DataFormats.Bitmap) Then
                '* Standard bitmap
                img = CType(e.Data.GetData(GetType(System.Drawing.Bitmap)), Bitmap)
            Else
                '* Device Independent Bitmap
                Dim myStream As Stream = e.Data.GetData(DataFormats.Dib)
                Dim bmp As BitmapFromDibStream = New BitmapFromDibStream(myStream)
                img = New Bitmap(bmp)
            End If

            Me.Present_Image(sender.Name, img)

        End Sub

        Private Sub picDisplay_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles picLeft.DragEnter, picRight.DragEnter, btnPictureBoth.DragEnter, btnPictureLeft.DragEnter, btnPictureRight.DragEnter, btnPasteBoth.DragEnter, btnPasteLeft.DragEnter, btnPasteRight.DragEnter
            '* This routine says that the pic preview boxes and "Display" buttons
            '* can accept dropped string copy only.
            If (e.Data.GetDataPresent(DataFormats.Dib) Or e.Data.GetDataPresent(DataFormats.Bitmap)) Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.None
            End If
        End Sub

        Private Sub btnImgSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImgSearch.Click
            '* Empty the results listbox first
            Me.picImgSearchPreview.Image = Nothing
            With Me.lbImgResults
                .SelectedIndex = -1   '* select none
                If .Items.Count > 0 Then .Items.Clear()
            End With

            If Not Me.comboImgSearchText.Text.Length > 0 Then
                MsgBox("Please enter text to search for.", MsgBoxStyle.OkOnly, "No Search Text Supplied")
                Exit Sub
            End If

            '* Add an item to the combobox, removing the oldest one if we need the room.
            '* If it's a match of a previous one, don't add anything.
            '* Everything is stored and compared as lowercase for exact matching.
            Dim CompareText As String = Me.comboImgSearchText.Text.ToLower
            With Me.comboImgSearchText
                Dim OrigText As String = .Text
                Dim TextMaxLoc As Integer = .FindStringExact(CompareText)
                If TextMaxLoc < 0 Then
                    '* This is a new string! Make room if necessary
                    If .Items.Count = .MaxDropDownItems Then
                        .Items.RemoveAt(.MaxDropDownItems - 1)
                    End If
                    .Items.Insert(0, CompareText)
                ElseIf TextMaxLoc > 0 Then
                    '* It's already in the dropdown list. Move it up to the top. Zero is the
                    '* topmost item, so we don't bother with that one.
                    .Items.RemoveAt(TextMaxLoc)
                    .Items.Insert(0, CompareText)
                End If
                .Text = OrigText
                .SelectionStart = 0
                .SelectionLength = .Text.Length
            End With

            Dim CheckFileName As FileID
            Dim UpdateStarted As Boolean = False
            CompareText = "*" + CompareText + "*"   '* wrap for fuzzy search
            For Each CheckFileName In ImageLibrary
                '* Does this filename contain the supplied text?
                If CheckFileName.Name.ToLower Like CompareText Then
                    If Not UpdateStarted Then
                        Me.lbImgResults.BeginUpdate()
                        UpdateStarted = True
                    End If
                    Me.lbImgResults.Items.Add(CheckFileName.FullPath)
                End If
            Next
            If UpdateStarted Then
                Me.lbImgResults.EndUpdate()
                Me.lbImgResults.SelectedIndex = 0  '* show the image of the first match
            End If
        End Sub

        Private Sub comboImgSearchText_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboImgSearchText.Enter
            Me.AcceptButton = Me.btnImgSearch
        End Sub
        Private Sub comboImgSearchText_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboImgSearchText.Leave
            Me.AcceptButton = Nothing
        End Sub

        Private Sub btnSearchImgShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchImgShowLeft.Click, btnSearchImgShowBoth.Click, btnSearchImgShowRight.Click
            If Me.picImgSearchPreview.Image IsNot Nothing Then
                Me.Present_Image(sender.Name, Me.picImgSearchPreview.Image)
            End If
        End Sub

        Private Sub btnSearchImgAddSlide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchImgAddSlide.Click
            Me.lbSlideList.Items.Add(Me.lbImgResults.SelectedItem)
        End Sub

        Private Sub PreviewSearchPic()
            Static PrevSelect As String
            If Me.lbImgResults.SelectedItems.Count = 1 Then
                If Me.lbImgResults.SelectedItem <> PrevSelect Then
                    PrevSelect = Me.lbImgResults.SelectedItem
                    Try
                        Me.picImgSearchPreview.Image = Image.FromFile(Me.lbImgResults.SelectedItem)
                    Catch
                        Me.picImgSearchPreview.Image = Nothing
                    End Try
                End If
            Else
                Me.picImgSearchPreview.Image = Nothing
                PrevSelect = ""
            End If
        End Sub

        Private Sub lbImgResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbImgResults.SelectedIndexChanged
            Me.PreviewSearchPic()
        End Sub
        Private Sub lbImgResults_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbImgResults.MouseDown
            ' ----- Prepare the draggable content.
            If Me.lbImgResults.SelectedIndex >= 0 Then
                Me.PreviewSearchPic()

                ' ----- Don't start the drag yet. Wait until we move a
                '       certain amount.
                Me.DragBounds = New Rectangle(New Point(e.X - _
                   (SystemInformation.DragSize.Width / 2), _
                   e.Y - (SystemInformation.DragSize.Height / 2)), _
                   SystemInformation.DragSize)
                Me.DragMethod = "from_lbImgResults"
            End If
        End Sub
        Private Sub lbImgResults_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbImgResults.MouseMove
            ' ----- Ignore if not dragging from this control.
            If (Me.DragMethod <> "from_lbImgResults") Then Return

            ' ----- Have we left the drag boundary?
            'If (DragBounds.Contains(e.X, e.Y) = False) Then
            ' ----- Start the drag-and-drop operation.
            'If (lbImgResults.DoDragDrop(lbImgResults.SelectedItems, _
            '      DragDropEffects.Move) = DragDropEffects.Move) Then
            If Me.lbImgResults.SelectedIndex >= 0 Then
                Me.lbImgResults.DoDragDrop(Me.lbImgResults.SelectedItem, DragDropEffects.Copy)
            End If
            '        Me.lbImgResults.DoDragDrop(Me.lbImgResults.SelectedItem, DragDropEffects.Copy)
            'End If
            Me.DragMethod = ""
            'End If
        End Sub
        Private Sub lbImgResults_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbImgResults.MouseUp
            ' ----- End of drag-and-drop.
            Me.DragMethod = ""
        End Sub

        Private Sub picImgSearchPreview_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picImgSearchPreview.MouseDown
            '* This routine defines picImgSearchPreview as a draggable entity, text copy only (for image filename)
            If (Not sender.Image Is Nothing) And (Me.lbImgResults.SelectedIndex >= 0) Then
                Me.lbImgResults.DoDragDrop(Me.lbImgResults.SelectedItem, DragDropEffects.Copy)
            End If
        End Sub
        Private Sub btnHot_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles btnHot1.DragEnter, btnHot2.DragEnter, btnHot3.DragEnter, btnHot4.DragEnter, btnHot5.DragEnter, btnHot6.DragEnter, btnHot7.DragEnter, btnHot8.DragEnter, btnHot9.DragEnter, btnHot10.DragEnter
            '* This routine says that hot buttons can accept dropped string copy only.
            If (e.Data.GetDataPresent(DataFormats.Text)) Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.None
            End If
        End Sub
        Private Overloads Sub btnHot_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles btnHot1.DragDrop, btnHot2.DragDrop, btnHot3.DragDrop, btnHot4.DragDrop, btnHot5.DragDrop, btnHot6.DragDrop, btnHot7.DragDrop, btnHot8.DragDrop, btnHot9.DragDrop, btnHot10.DragDrop
            '* This routine takes the dropped text and attaches it to the chosen hotbutton.
            Dim NewText As String = e.Data.GetData("Text")
            Dim i As Integer
            i = CInt(sender.Name.Substring(6)) - 1      '* the hot button index from the control name
            If NewText <> "" Then
                Me.HotButton(i).Tag = NewText
                Me.HotImage(i).Text = NewText
                Dim fi As New FileInfo(NewText)
                Dim namelength As Integer = fi.Name.Replace(fi.Extension, "").Length
                If namelength > Me.HotText(i).MaxLength Then namelength = Me.HotText(i).MaxLength
                Me.HotText(i).Text = fi.Name.Substring(0, namelength).ToLower
                Me.HotButtonsChanged = True
            End If
        End Sub

        Private Sub ClearImageLibrary()
            Dim i As Integer
            For i = 1 To Me.ImageLibrary.Count
                ' Since collections are reindexed automatically, remove
                ' the first member on each iteration
                Me.ImageLibrary.Remove(1)
            Next
            Me.ShowLibraryCount()
        End Sub

        Private Sub BuildImageLibrary(ByVal DirName As String)
            Me.ProcessImageDir(DirName)
            Me.ShowLibraryCount()
            Return

            '* Test Mode Only
            If Me.TestMode Then
                Dim FileElem As FileID
                Me.tbLeftText.Text = "Image Library Dump (TEST MODE)"
                For Each FileElem In ImageLibrary
                    Me.tbLeftText.Text = Me.tbLeftText.Text + EOL + FileElem.FullPath
                Next
            End If
        End Sub

        Private Sub ShowLibraryCount()
            Me.lblLibraryCount.Text = "Images in Search Library: " + Me.ImageLibrary.Count.ToString
            System.Windows.Forms.Application.DoEvents()
        End Sub

        Private Sub btnReIndexImgLib_Click(sender As System.Object, e As System.EventArgs) Handles btnReIndexImgLib.Click
            Me.ClearImageLibrary()
            Me.BuildImageLibrary(Me.tbDefaultImageDir.Tag)  '* Use the SAVED value, in case prefs weren't saved.
        End Sub


        Private Sub ProcessImageDir(ByVal DirName As String)
            '* A recursive function to add all graphics file names into the ImageLibrary list.
            '* The list is for searching later.
            '* PASS DIRECTORIES ONLY

            Dim DirList As New Collection()   '* of strings only
            Dim PrevDir As String = CurDir()
            Dim NextName As String
            Dim WholeName As String

            Try
                ChDir(DirName)
            Catch ex As Exception
                '* If no rights to look in that directory, skip it.
                Exit Try
            Finally
                NextName = Dir(".", FileAttribute.Directory)
                If NextName <> ".xvpics" Then '* GIMP non-image files to ignore
                    While NextName <> ""
                        WholeName = DirName + "\" + NextName
                        '* Dirs go into DirList for future processing. Filenames get added to Image Library.
                        If My.Computer.FileSystem.DirectoryExists(WholeName) Then
                            '*If (GetAttr(WholeName) And FileAttribute.Directory) = FileAttribute.Directory Then
                            DirList.Add(WholeName)
                        ElseIf IsImageFile(NextName) Then
                            Dim FileElem As New FileID()  '* Need a new instance each iteration
                            FileElem.Path = DirName
                            FileElem.Name = NextName
                            Me.ImageLibrary.Add(FileElem)
                            '* Occasionally show progress on building the library
                            If (Me.ImageLibrary.Count Mod 250) = 0 Then Me.ShowLibraryCount()
                        End If
                        NextName = Dir()
                    End While

                    Dim NextDir As String
                    For Each NextDir In DirList
                        Me.ProcessImageDir(NextDir)
                    Next
                    ChDir(PrevDir)
                End If
            End Try

        End Sub

        Private Function IsImageFile(ByVal fnam As String) As Boolean
            '* Match file extension against known image extensions.
            '* If it matches, return true; else return false.
            Dim fi As New FileInfo(fnam)
            Dim ext As String = fi.Extension.ToUpper

            '* If the extension is in the global image extension list, return true.
            Return (Array.IndexOf(Me.ImageFileExtensions, ext) >= 0)
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
                    Me.ThingSubs(i) = ""
                Next
                Me.clbThings.Items.Clear()
                Me.tbCurrentThing.Visible = False
                Me.tbSubstitutions.Visible = False
                Me.btnShowThingLeft.Visible = False
                Me.btnShowThingRight.Visible = False
                Me.btnShowThingBoth.Visible = False
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
                        Me.ThingSubs(i) = Me.ThingSubs(i + 1)
                    Next
                    Me.clbThings.Items.RemoveAt(Me.clbThings.SelectedIndex())
                End If
            End If
        End Sub

        Private Sub clbThings_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clbThings.SelectedIndexChanged
            If sender.SelectedIndex >= 0 Then
                Me.tbCurrentThing.Text = sender.SelectedItem
                Me.tbSubstitutions.Text = Me.ThingSubs(sender.SelectedIndex)
                Me.tbCurrentThing.Visible = True
                Me.tbSubstitutions.Visible = True
                Me.btnShowThingLeft.Visible = True
                Me.btnShowThingRight.Visible = True
                Me.btnShowThingBoth.Visible = True
                Me.clbThings.Hide()
                Me.tbSubstitutions.Select(Me.tbSubstitutions.TextLength, 0)  '* deselect all text here
                Me.clbThings.Show()
                Me.tbSubstitutions.Focus()
            Else
                Me.tbCurrentThing.Visible = False
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
                Dim s As String = Me.ThingSubs(sel - 1)
                Me.ThingSubs(sel - 1) = Me.ThingSubs(sel)
                Me.ThingSubs(sel) = s

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
                Dim s As String = Me.ThingSubs(sel)
                Me.ThingSubs(sel) = Me.ThingSubs(sel + 1)
                Me.ThingSubs(sel + 1) = s

                Me.clbThings.Items.Insert(sel + 2, Me.clbThings.Items.Item(sel))
                If chk Then Me.clbThings.SetItemCheckState(sel + 2, CheckState.Checked)
                Me.clbThings.Items.RemoveAt(sel)
                Me.clbThings.SelectedIndex = sel + 1
            End If
        End Sub

        Private Sub tbSubstitutions_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbSubstitutions.Leave
            Me.ThingSubs(Me.clbThings.SelectedIndex) = sender.Text
        End Sub

        Private Sub tbCurrentThing_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbCurrentThing.Leave
            Dim sel As Integer = Me.clbThings.SelectedIndex
            If sel >= 0 Then
                Me.clbThings.Items(sel) = sender.Text
            End If
        End Sub

        Private Sub btnList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnListLeft.Click, btnListRight.Click, btnListBoth.Click
            Dim ThingsFontSize As Single = 20
            Dim i As Integer
            Dim s As String = ""
            For i = 0 To (Me.clbThings.Items.Count - 1)
                If i > 0 Then s = s + EOL
                If Me.clbThings.CheckedIndices.Contains(i) Then s = s + ChrW(&H25BA)
                s = s + (i + 1).ToString + ". " + Me.clbThings.Items.Item(i)
            Next

            If (Me.clbThings.Items.Count > 4) Then
                If (s.Length > 200) Then
                    ThingsFontSize = 14
                ElseIf (s.Length > 160) Or (Me.clbThings.Items.Count > 8) Then
                    ThingsFontSize = 15
                ElseIf (s.Length > 130) Or (Me.clbThings.Items.Count > 6) Then
                    ThingsFontSize = 18
                End If
            End If
            ' s = s + " " + s.Length.ToString

            If sender.name <> "btnListRight" Then DisplayTextScreen(Me.LS, s, Me.clbThings.BackColor, ThingsFontSize)
            If sender.name <> "btnListLeft" Then DisplayTextScreen(Me.RS, s, Me.clbThings.BackColor, ThingsFontSize)
        End Sub
        Private Sub btnShowThing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowThingLeft.Click, btnShowThingRight.Click, btnShowThingBoth.Click
            Dim s As String
            s = Me.tbCurrentThing.Text + EOL + EOL + Me.tbSubstitutions.Text
            If sender.name <> "btnShowThingRight" Then DisplayTextScreen(Me.LS, s, Me.clbThings.BackColor, 19)
            If sender.name <> "btnShowThingLeft" Then DisplayTextScreen(Me.RS, s, Me.clbThings.BackColor, 19)
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

        Private Sub FTreeAutoExpand(ByRef ftree As ExpTreeLib.ExpTree, ByVal fpath As String)
            '* This subroutine assigns the path to the foldertree if the path exists.
            '* Determine if the directory exists
            If System.IO.Directory.Exists(fpath) Then
                Try
                    ftree.ExpandANode(fpath)
                Catch ex As Exception
                End Try
            End If
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
                Me.StopSlideShow()
            End If
        End Sub
        Private Sub btnAddSlide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSlide.Click
            Me.AddToSlideList(Me.FileListBox1.SelectedItems)
        End Sub
        Private Sub btnClearSlideList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearSlideList.Click
            If Me.lbSlideList.Items.Count < 1 Then Return
            If Not Me.AskIfSure("Clear all slides from the list?") Then Return
            Me.StopSlideShow()
            Me.lbSlideList.Items.Clear()     '** Empty the list first
        End Sub
        Private Sub FileListBox1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles FileListBox1.DoubleClick
            Me.AddToSlideList(sender.SelectedItems)
        End Sub
        Private Sub FileListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FileListBox1.SelectedIndexChanged
            If Me.FileListBox1.SelectedItems.Count = 1 Then
                Try
                    Me.picSlidePreview.Image = Image.FromFile(Me.FileListBox1.Path + "\" + Me.FileListBox1.SelectedItem)
                Catch
                    '* If error, show nothing. I don't care what the error was about.
                    Me.picSlidePreview.Image = Nothing
                End Try
            Else
                Me.picSlidePreview.Image = Nothing
            End If
        End Sub
        Private Sub ExpTree1_ExpTreeNodeSelected(ByVal SelPath As String, ByVal Item As ExpTreeLib.CShItem) Handles ExpTree1.ExpTreeNodeSelected
            Static PrevSelect As String
            If SelPath <> PrevSelect Then
                If System.IO.Directory.Exists(SelPath) Then
                    Me.FileListBox1.Path = SelPath
                Else
                    Me.FileListBox1.Path = ""
                End If
                PrevSelect = SelPath
                Me.picSlidePreview.Image = Nothing   '** Clear the preview img if we change dirs
            End If
        End Sub

        Private Sub btnSlideUp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSlideUp.Click
            With Me.lbSlideList
                ' Only think about doing anything if at least one slide is selected.
                If .SelectedIndices.Count > 0 Then
                    ' Only actually try to do anything if the very first slide isn't one of the chosen ones.
                    If .SelectedIndices(0) > 0 Then
                        Dim i As Integer
                        For i = 0 To (.SelectedIndices.Count - 1)
                            Dim idx As Integer = .SelectedIndices(i)
                            'Items.IndexOf(.SelectedItems.Item(i))
                            .Items.Insert(idx - 1, .SelectedItems.Item(i))
                            .Items.RemoveAt(idx + 1)
                            .SetSelected(idx - 1, True)
                        Next
                    End If
                End If
            End With
        End Sub
        Private Sub btnSlideDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSlideDown.Click
            ' Only think about doing anything if at least one slide is selected.
            With Me.lbSlideList
                If .SelectedIndices.Count > 0 Then
                    ' Only actually try to do anything if the very last slide isn't one of the chosen ones.
                    Dim LastIndex As Integer = .SelectedIndices(.SelectedIndices.Count - 1)
                    If LastIndex < (.Items.Count - 1) Then
                        Dim i As Integer
                        For i = (.SelectedIndices.Count - 1) To 0 Step -1
                            Dim idx As Integer = .SelectedIndices(i)
                            If (idx < (.Items.Count - 1)) And (idx >= 0) Then
                                .Items.Insert(idx + 2, .Items.Item(idx))
                                .Items.RemoveAt(idx)
                                .SetSelected(idx + 1, True)
                            End If
                        Next
                    End If
                End If
            End With
        End Sub

        Private Function SelectSlideShowFileName() As String
            Dim fn As String
            Dim [of] As New OpenFileDialog()
            With [of]
                .Filter = "JANIS SlideShow(*.JSL)|*.JSL"
                .InitialDirectory = ROOT_SUPPORT_DIR + DEFAULT_SLIDESHOW_DIR
                If .ShowDialog(Me) = DialogResult.OK Then fn = .FileName Else fn = ""
                .Dispose()
            End With
            Return fn
        End Function
        Private Sub btnLoadSlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadSlides.Click
            If Me.lbSlideList.Items.Count > 0 Then
                'There are already slides in the list - ask if we want to continue and overwrite
                If Not Me.AskIfSure("Replace the current Slideshow list?") Then Return
            End If
            Dim slidefile As String = Me.SelectSlideShowFileName()
            Me.LoadSlideShow(slidefile)
            Me.AllScreensToFront()
        End Sub
        Private Sub btnSaveSlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSlides.Click
            If Me.lbSlideList.Items.Count = 0 Then
                MessageBox.Show("There are no items in the SlideShow yet.", "Nothing to Save", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                Return
            End If
            Dim sf As New SaveFileDialog()
            With sf
                .Filter = "JANIS Slideshow(*.JSL)|*.JSL"
                .InitialDirectory = ROOT_SUPPORT_DIR + DEFAULT_SLIDESHOW_DIR
                If .ShowDialog(Me) = DialogResult.OK Then
                    Dim fn As Integer = FreeFile()
                    FileOpen(fn, .FileName, OpenMode.Output)
                    Dim i As Integer
                    For i = 0 To (Me.lbSlideList.Items.Count - 1)
                        PrintLine(fn, Me.lbSlideList.Items.Item(i))
                    Next
                    FileClose(fn)
                End If
                .Dispose()
            End With
            Me.AllScreensToFront()
        End Sub

        Private Sub LoadSlideShow(ByVal slidefile As String)
            If slidefile <> "" Then
                Me.lbSlideList.Items.Clear()     '** Empty the list first
                Dim fn As Integer = FreeFile()
                Dim FileErr As Boolean
                Try
                    FileOpen(fn, slidefile, OpenMode.Input)
                Catch ex As Exception
                    FileErr = True
                    MessageBox.Show("An error occurred opening slideshow file '" + slidefile + "'.", "File Error")
                End Try
                If Not FileErr Then
                    Dim s As String = ""
                    While Not EOF(fn)
                        Input(fn, s)
                        Me.lbSlideList.Items.Add(s)
                    End While
                    FileClose(fn)
                End If
            End If
        End Sub

        Private Sub SelectRandomSlide()
            '* Select a random slide and buffer it into BufferedSlide.
            If Me.lbSlideList.Items.Count < 2 And Not (Me.BufferedSlide Is Nothing) Then Return
            Dim NewIndex As Integer
            Dim MyRand As New Random()
            Do
                NewIndex = MyRand.Next(Me.lbSlideList.Items.Count)
            Loop While NewIndex = Me.lbSlideList.SelectedIndex
            Me.lbSlideList.SelectedIndex = NewIndex
            Try
                Me.BufferedSlide = Image.FromFile(Me.lbSlideList.SelectedItem)
            Catch ex As Exception
                Me.BufferedSlide = Nothing
            End Try
        End Sub

        Private Sub SetSlideTimerInterval()
            If Me.SlidesStatus = SLIDES_WHAMMY Then
                Me.SlideTimer.Interval = 333
            Else
                Me.SlideTimer.Interval = Me.nudDelay.Value * 1000
            End If
        End Sub
        Private Sub StartSlideTimer()
            Me.SetSlideTimerInterval()
            Me.SlideTimer.Start()
        End Sub
        Private Sub StartSlideShow()
            If Me.SlidesStatus = SLIDES_PLAYING Then Return
            If Me.lbSlideList.Items.Count < 1 Then Return
            Me.lbSlideList.SelectionMode = SelectionMode.One
            If Me.SlidesStatus = SLIDES_WHAMMY Then
                Me.SelectRandomSlide()
            ElseIf SlidesStatus <> SLIDES_PAUSED Then
                Me.lbSlideList.SelectedIndex = 0
            End If
            Me.DisplayImageFile("Both", Me.lbSlideList.SelectedItem)
            Me.SetPauseButtonColor(False)
            Me.SetPlayButtonColor(True)
            If Me.SlidesStatus <> SLIDES_WHAMMY Then Me.SlidesStatus = SLIDES_PLAYING
            Me.StartSlideTimer()
        End Sub
        Public Sub StopSlideShow()
            Dim WhammyWasActive As Boolean = (Me.SlidesStatus = SLIDES_WHAMMY)
            Me.SlideTimer.Stop()
            Me.SlidesStatus = SLIDES_STOPPED
            If WhammyWasActive Then Me.DisplayImageFile("Both", Me.lbSlideList.SelectedItem)
            Me.SetPauseButtonColor(False)
            Me.SetPlayButtonColor(False)
            Me.lbSlideList.SelectionMode = SelectionMode.MultiExtended
        End Sub

        Private Sub SetPauseButtonColor(ByVal pause_on As Boolean)
            With Me.btnPauseSlides
                Dim backclr As System.Drawing.Color = Me.btnStopSlides.BackColor
                If pause_on Then
                    .BackColor = System.Drawing.Color.Blue
                    .ForeColor = backclr
                Else
                    .BackColor = backclr
                    .ForeColor = System.Drawing.Color.Blue
                End If
            End With
        End Sub
        Private Sub SetPlayButtonColor(ByVal play_on As Boolean)
            With Me.btnPlaySlides
                Dim backclr As System.Drawing.Color = Me.btnStopSlides.BackColor
                If play_on Then
                    .BackColor = System.Drawing.Color.Green
                    .ForeColor = backclr
                Else
                    .BackColor = backclr
                    .ForeColor = System.Drawing.Color.Green
                End If
            End With
        End Sub
        Private Sub nudDelay_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDelay.ValueChanged
            If Me.SlidesStatus = SLIDES_WHAMMY Then Return
            Me.SetSlideTimerInterval()
        End Sub

        Private Sub SlideTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SlideTimer.Tick
            '* I "misused" the slide timer to check if there is already app instance at startup
            If Me.SlideTimerTag = "AppAlreadyRunning" Then
                Me.Close()
            Else
                With Me.lbSlideList
                    If .Items.Count < 1 Then
                        Me.StopSlideShow()
                        Return
                    ElseIf Me.SlidesStatus = SLIDES_WHAMMY Then
                        Me.DisplayRawImageBoth(Me.BufferedSlide, False)   '* maintain ratio for all images during whammy, for smoothness/speed
                        Me.SelectRandomSlide()                            '* Set Me.BufferedSlide to a random slide
                    Else
                        If .SelectedIndex < (.Items.Count - 1) Then
                            .SelectedIndex += 1
                        Else
                            .SelectedIndex = 0
                        End If
                        Me.DisplayImageFile("Both", .SelectedItem)
                    End If
                End With
            End If
        End Sub

        Private Sub btnPlaySlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlaySlides.Click
            If Me.SlidesStatus = SLIDES_WHAMMY Then Return
            Me.StartSlideShow()
        End Sub
        Private Sub btnStopSlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStopSlides.Click
            Me.StopSlideShow()
        End Sub
        Private Sub btnPauseSlides_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPauseSlides.Click
            If Me.SlidesStatus = SLIDES_WHAMMY Then Return
            If Me.SlidesStatus = SLIDES_STOPPED Then Return
            If Me.SlidesStatus = SLIDES_PAUSED Then
                Me.SlidesStatus = SLIDES_PLAYING
                Me.SetPauseButtonColor(False)
                Me.StartSlideTimer()
                Return
            End If
            '** If we get here, Slides Status must be PLAYING
            Me.SlideTimer.Stop()
            Me.SlidesStatus = SLIDES_PAUSED
            Me.SetPauseButtonColor(True)
        End Sub
        Private Sub btnChangeSlide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirstSlide.Click, btnNextSlide.Click, btnPrevSlide.Click, btnLastSlide.Click
            If Me.SlidesStatus = SLIDES_STOPPED Or Me.SlidesStatus = SLIDES_WHAMMY Then Return
            If Me.SlidesStatus = SLIDES_PLAYING Then Me.SlideTimer.Stop() '* temporary stoppage
            Dim controlname As String = sender.Name
            With Me.lbSlideList
                Select Case controlname
                    Case "btnFirstSlide"
                        .SelectedIndex = 0
                    Case "btnPrevSlide"
                        If .SelectedIndex > 0 Then
                            .SelectedIndex -= 1
                        Else
                            '** Roll over before the beginning
                            .SelectedIndex = .Items.Count - 1
                        End If
                    Case "btnNextSlide"
                        If .SelectedIndex < (.Items.Count - 1) Then
                            .SelectedIndex += 1
                        Else
                            '** Roll over past the end
                            .SelectedIndex = 0
                        End If
                    Case "btnLastSlide"
                        .SelectedIndex = .Items.Count - 1
                End Select
                Me.DisplayImageFile("Both", .SelectedItem)
            End With
            If Me.SlidesStatus = SLIDES_PLAYING Then Me.StartSlideTimer()
        End Sub
        Private Sub lbSlideList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSlideList.DoubleClick
            '************************************************************************************
            '* A double click changes the slide immediately REGARDLESS OF MODE, except for WHAMMY
            '************************************************************************************
            If Me.SlidesStatus = SLIDES_WHAMMY Then Return
            If Me.SlidesStatus = SLIDES_PLAYING Then Me.SlideTimer.Stop() '* temporary stoppage
            Me.DisplayImageFile("Both", Me.lbSlideList.SelectedItem)
            If Me.SlidesStatus = SLIDES_PLAYING Then StartSlideTimer()
        End Sub
        Private Sub btnWhammy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWhammy.Click
            If Me.SlidesStatus = SLIDES_WHAMMY Then Return
            If Me.lbSlideList.Items.Count < 1 Then Return

            'If Me.SlidesStatus <> SLIDES_STOPPED Then Me.StopSlideShow()
            Me.SlidesStatus = SLIDES_WHAMMY
            Me.StartSlideShow()
            Dim WaitForm As New fmClickWait(Me)
            WaitForm.Show() '* Will automatically stop the whammy slideshow when it closes
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
        Private Sub UpdateHotButtonColor(ByVal side As String, ByVal newcolor As Color)
            '* Used to update the side button colors on the HotButtons so they match the 
            '* corresponding side team colors.
            Dim x As Integer, y As Integer
            Dim oldRGBcolor As Integer
            Dim newRGBcolor As Integer = newcolor.ToArgb
            Dim leftbound As Integer, rightbound As Integer

            Dim img As Bitmap = New Bitmap(Me.btnHot1.BackgroundImage)

            If side Like "Left" Then
                leftbound = 0
                rightbound = HOTSIDE_PIXELS - 1
            Else
                leftbound = img.Width - HOTSIDE_PIXELS
                rightbound = img.Width - 1
            End If
            oldRGBcolor = img.GetPixel(leftbound, 0).ToArgb
            If newRGBcolor = oldRGBcolor Then
                Exit Sub
            End If

            For x = leftbound To rightbound
                For y = 0 To img.Height - 1
                    If img.GetPixel(x, y).ToArgb = oldRGBcolor Then
                        img.SetPixel(x, y, newcolor)
                    End If
                Next
            Next
            Me.btnHot1.BackgroundImage = img
        End Sub
        Private Sub btnHot1_BackgroundImageChanged(sender As Object, e As System.EventArgs) Handles btnHot1.BackgroundImageChanged
            '* If the background to button 1 changes, all 10 of them change.
            Me.btnHot2.BackgroundImage = Me.btnHot1.BackgroundImage
            Me.btnHot3.BackgroundImage = Me.btnHot1.BackgroundImage
            Me.btnHot4.BackgroundImage = Me.btnHot1.BackgroundImage
            Me.btnHot5.BackgroundImage = Me.btnHot1.BackgroundImage
            Me.btnHot6.BackgroundImage = Me.btnHot1.BackgroundImage
            Me.btnHot7.BackgroundImage = Me.btnHot1.BackgroundImage
            Me.btnHot8.BackgroundImage = Me.btnHot1.BackgroundImage
            Me.btnHot9.BackgroundImage = Me.btnHot1.BackgroundImage
            Me.btnHot10.BackgroundImage = Me.btnHot1.BackgroundImage
        End Sub
        Private Sub cbHBActive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbHBActive.CheckedChanged
            Dim i As Integer
            For i = 0 To 9
                Me.HotButton(i).Visible = Me.cbHBActive.Checked
            Next
        End Sub

        Private Function SelectHotButtonsFileName() As String
            Dim fn As String
            Dim [of] As New OpenFileDialog()
            With [of]
                .Filter = "JANIS HotButtons File(*.JHB)|*.JHB"
                .InitialDirectory = ROOT_SUPPORT_DIR + DEFAULT_HOTBUTTON_DIR
                If .ShowDialog(Me) = DialogResult.OK Then fn = .FileName Else fn = ""
                .Dispose()
            End With
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
                    Dim s As String = ""
                    Input(fn, s)
                    Dim info() As String = Split(s, "¶")
                    Me.HotButton(i).Text = info(0)
                    Me.HotText(i).Text = info(0)
                    Me.HotButton(i).Tag = info(1)
                    Me.HotImage(i).Text = info(1)
                    i = i + 1
                End While
                FileClose(fn)
                Me.HotButtonsChanged = False
            End If
            Me.AllScreensToFront()
        End Sub

        Private Sub SaveHotButtons()
            If Not Me.HotButtonsChanged Then
                If MessageBox.Show("You haven't made any changes to the HotButtons. Save anyway?", "Nothing to Save", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly) <> DialogResult.Yes Then Return
            End If
            Dim sf As New SaveFileDialog()
            With sf
                .Filter = "JANIS HotButtons File(*.JHB)|*.JHB"
                .InitialDirectory = ROOT_SUPPORT_DIR + DEFAULT_HOTBUTTON_DIR
                If .ShowDialog(Me) = DialogResult.OK Then
                    Dim fn As Integer = FreeFile()
                    FileOpen(fn, .FileName, OpenMode.Output)
                    Dim i As Integer
                    For i = 0 To 9
                        PrintLine(fn, Me.HotButton(i).Text + "¶" + Me.HotButton(i).Tag)
                    Next
                    FileClose(fn)
                    Me.HotButtonsChanged = False
                End If
                .Dispose()
            End With
            Me.AllScreensToFront()
        End Sub

        Private Sub btnSaveHB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveHB.Click
            Me.SaveHotButtons()
        End Sub

        Private Sub btnLoadHB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadHB.Click
            If Me.HotButtonsChanged Then
                'Ask if we want to continue and overwrite
                If MessageBox.Show("Replace the current Hot Buttons without saving?", "Hot Button Definitions Changed", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly) <> DialogResult.OK Then
                    Return
                End If
            End If
            Dim hbfile As String = Me.SelectHotButtonsFileName()
            If hbfile <> "" Then Me.LoadHotButtons(hbfile)
        End Sub

        Private Sub btnClearHB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearHB.Click
            If AskIfSure("Are you sure you want to clear all the Hot Button settings?") Then
                Dim i As Integer
                For i = 0 To 9
                    Me.HotText(i).Text = ""
                    Me.HotButton(i).Text = ""
                    Me.HotButton(i).Tag = ""
                    Me.HotImage(i).Text = ""
                Next
                Me.HotButtonsChanged = False
            End If
            Me.AllScreensToFront()
        End Sub

        Private Sub btnHBSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHBSelect1.Click, btnHBSelect2.Click, btnHBSelect3.Click, btnHBSelect4.Click, btnHBSelect5.Click, btnHBSelect6.Click, btnHBSelect7.Click, btnHBSelect8.Click, btnHBSelect9.Click, btnHBSelect10.Click
            Dim fn As String = Me.SelectImageFilename()
            If fn <> "" Then
                Dim i As Integer = CInt(sender.Tag)
                Me.HotButton(i).Tag = fn
                Me.HotImage(i).Text = fn
                Me.HotButtonsChanged = True
            End If
        End Sub

        Private Sub tbHBtext_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbHBtext1.TextChanged, tbHBtext2.TextChanged, tbHBtext3.TextChanged, tbHBtext4.TextChanged, tbHBtext5.TextChanged, tbHBtext6.TextChanged, tbHBtext7.TextChanged, tbHBtext8.TextChanged, tbHBtext9.TextChanged, tbHBtext10.TextChanged
            Me.HotButton(CInt(sender.Tag)).Text = sender.Text
            Me.HotButtonsChanged = True
        End Sub

        Private Sub tbHBfile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbHBfile1.TextChanged, tbHBfile2.TextChanged, tbHBfile3.TextChanged, tbHBfile4.TextChanged, tbHBfile5.TextChanged, tbHBfile6.TextChanged, tbHBfile7.TextChanged, tbHBfile8.TextChanged, tbHBfile9.TextChanged, tbHBfile10.TextChanged
            '* Put the cursor at the end so that we can see the
            '* more sigificant part of the file name info
            sender.SelectionStart = sender.TextLength
        End Sub

        Private Sub btnHot_MouseClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles btnHot1.MouseClick, btnHot2.MouseClick, btnHot3.MouseClick, btnHot4.MouseClick, btnHot5.MouseClick, btnHot6.MouseClick, btnHot7.MouseClick, btnHot8.MouseClick, btnHot9.MouseClick, btnHot10.MouseClick
            Dim side_width_pixels As Integer = 24      '* Border = 4, subbutton = 20
            Dim img_name As String = sender.Tag

            Me.tbLeftText.Text = e.X.ToString + ", " + e.Y.ToString
            If img_name <> "" Then
                '* First, stop the slideshow if it's running.
                Me.StopSlideShow()
                Dim location As String
                '* The left and right sub-buttons are for showing the image only left or right
                If e.X <= side_width_pixels Then
                    location = "Left"
                ElseIf e.X >= (sender.Width - side_width_pixels) Then
                    location = "Right"
                Else
                    location = "Both"
                End If
                Me.DisplayImageFile(location, img_name)
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
            Me.tbDefaultImageDir.Text = Me.SelectDir(Me.tbDefaultImageDir.Text)
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
            Dim fn As String = Me.SelectSlideShowFileName()
            If fn <> "" Then Me.tbDefaultSlideShow.Text = fn
        End Sub

        Private Sub btnDefaultPrefs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefaultPrefs.Click
            Me.SetDefaultPrefs()
        End Sub
        Private Sub btnSavePrefs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSavePrefs.Click
            '* Write the prefs off to a file (JANIS.ini).
            '*
            Me.SavePrefsToFile(PREFS_FILE)
        End Sub
        Private Sub btnRevertPrefs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevertPrefs.Click
            '* The last saved values are stored in the TAG properties of all the controls on this tab.
            '* Use them to put everything back.
            Me.lblDefaultColorLeft.BackColor = Color.FromArgb(Me.lblDefaultColorLeft.Tag)
            Me.lblDefaultColorRight.BackColor = Color.FromArgb(Me.lblDefaultColorRight.Tag)
            Me.tbDefaultFontSize.Text = Me.tbDefaultFontSize.Tag
            Me.cbShadowsEnabled.Checked = Me.cbShadowsEnabled.Tag
            Me.tbDefaultImageDir.Text = Me.tbDefaultImageDir.Tag
            Me.tbDefaultImageFile.Text = Me.tbDefaultImageFile.Tag
            Me.cbDisplayDefaultImage.Checked = Me.cbDisplayDefaultImage.Tag
            Me.tbDefaultHBFile.Text = Me.tbDefaultHBFile.Tag
            Me.cbLoadDefaultHB.Checked = Me.cbLoadDefaultHB.Tag
            Me.nudDefaultSlideDelay.Value = CInt(Me.nudDefaultSlideDelay.Tag)
            Me.tbDefaultSlideShow.Text = Me.tbDefaultSlideShow.Tag
            Me.cbPlaySlidesAtStart.Checked = Me.cbPlaySlidesAtStart.Tag
            Me.cbLoadDefaultSlides.Checked = Me.cbLoadDefaultSlides.Tag
            Me.nudDefaultCountdownHours.Value = CInt(Me.nudDefaultCountdownHours.Tag)
            Me.nudDefaultCountdownMinutes.Value = CInt(Me.nudDefaultCountdownMinutes.Tag)
            Me.nudDefaultCountdownSeconds.Value = CInt(Me.nudDefaultCountdownSeconds.Tag)
        End Sub

        Private Function PrefsChanged() As Boolean
            If Me.lblDefaultColorLeft.BackColor.ToArgb <> Me.lblDefaultColorLeft.Tag Then Return True
            If Me.lblDefaultColorRight.BackColor.ToArgb <> Me.lblDefaultColorRight.Tag Then Return True
            If Me.tbDefaultFontSize.Text <> Me.tbDefaultFontSize.Tag Then Return True
            If Me.cbShadowsEnabled.Checked <> Me.cbShadowsEnabled.Tag Then Return True
            If Me.tbDefaultImageDir.Text <> Me.tbDefaultImageDir.Tag Then Return True
            If Me.tbDefaultImageFile.Text <> Me.tbDefaultImageFile.Tag Then Return True
            If Me.cbDisplayDefaultImage.Checked <> Me.cbDisplayDefaultImage.Tag Then Return True
            If Me.tbDefaultHBFile.Text <> Me.tbDefaultHBFile.Tag Then Return True
            If Me.cbLoadDefaultHB.Checked <> Me.cbLoadDefaultHB.Tag Then Return True
            If Me.nudDefaultSlideDelay.Value <> Me.nudDefaultSlideDelay.Tag Then Return True
            If Me.tbDefaultSlideShow.Text <> Me.tbDefaultSlideShow.Tag Then Return True
            If Me.cbPlaySlidesAtStart.Checked <> Me.cbPlaySlidesAtStart.Tag Then Return True
            If Me.cbLoadDefaultSlides.Checked <> Me.cbLoadDefaultSlides.Tag Then Return True
            If Me.nudDefaultCountdownHours.Value <> Me.nudDefaultCountdownHours.Tag Then Return True
            If Me.nudDefaultCountdownMinutes.Value <> Me.nudDefaultCountdownMinutes.Tag Then Return True
            If Me.nudDefaultCountdownSeconds.Value <> Me.nudDefaultCountdownSeconds.Tag Then Return True
            Return False
        End Function

        Private Sub LoadPrefsFromFile(ByVal filename As String)
            '* Determine if the file exists
            Dim MyPrefsFile As New System.IO.FileInfo(filename)
            If MyPrefsFile.Exists Then
                Dim fn As Integer = FreeFile()
                Dim s As String = ""
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
                    Try
                        Input(fn, s)
                    Catch ex As Exception
                        FileErr = True
                        Me.nudDefaultCountdownHours.Value = 0
                        Me.nudDefaultCountdownMinutes.Value = 5
                        Me.nudDefaultCountdownSeconds.Value = 0
                    End Try
                    If Not FileErr Then
                        Me.nudDefaultCountdownHours.Value = CInt(s)
                        Input(fn, s)
                        Me.nudDefaultCountdownMinutes.Value = CInt(s)
                        Input(fn, s)
                        Me.nudDefaultCountdownSeconds.Value = CInt(s)
                        Try
                            Input(fn, s)
                        Catch ex As Exception
                            FileErr = True
                            Me.cbShadowsEnabled.Checked = True
                        End Try
                        If Not FileErr Then
                            Me.cbShadowsEnabled.Checked = (s = "True")
                        End If
                    End If
                End If
                FileClose(fn)
                Me.StorePrefs()
            Else
                '* OK, the prefs file was not found. So load defaults and
                '* write the prefs file with those defaults.
                Me.SetDefaultPrefs()
                Me.SavePrefsToFile(filename)
            End If
            Me.AllScreensToFront()
        End Sub
        Private Sub SavePrefsToFile(ByVal filename As String)
            If Not PrefsChanged() Then Exit Sub

            Dim RebuildImageLibrary As Boolean = (Me.tbDefaultImageDir.Text <> Me.tbDefaultImageDir.Tag)

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
            PrintLine(fn, Me.nudDefaultCountdownHours.Value.ToString)
            PrintLine(fn, Me.nudDefaultCountdownMinutes.Value.ToString)
            PrintLine(fn, Me.nudDefaultCountdownSeconds.Value.ToString)
            PrintLine(fn, Me.cbShadowsEnabled.Checked.ToString)
            FileClose(fn)
            Me.StorePrefs()
            Me.AllScreensToFront()
            If RebuildImageLibrary Then
                Me.ClearImageLibrary()
                Me.BuildImageLibrary(Me.tbDefaultImageDir.Text)
            End If
        End Sub
        Private Sub SetDefaultPrefs()
            '* Reset the Preferences screen settings to factory defaults
            Me.lblDefaultColorLeft.BackColor = Me.pnlDefaultTextColorLeft1.BackColor
            Me.lblDefaultColorRight.BackColor = Me.pnlDefaultTextColorLeft2.BackColor
            Me.tbDefaultFontSize.Text = "60"
            Me.cbShadowsEnabled.Checked = True
            Me.tbDefaultImageDir.Text = ROOT_SUPPORT_DIR
            Me.tbDefaultImageFile.Text = ""
            Me.cbDisplayDefaultImage.Checked = False
            Me.tbDefaultHBFile.Text = ""
            Me.cbLoadDefaultHB.Checked = False
            Me.nudDefaultSlideDelay.Value = 15
            Me.tbDefaultSlideShow.Text = ""
            Me.cbPlaySlidesAtStart.Checked = False
            Me.cbLoadDefaultSlides.Checked = False
            Me.nudDefaultCountdownHours.Value = 0
            Me.nudDefaultCountdownMinutes.Value = 5
            Me.nudDefaultCountdownSeconds.Value = 0
        End Sub
        Private Sub StorePrefs()
            '* Stores whatever the current set of preferences is in the TAG properties of the controls on the
            '* preferences tab. Compare later to see if things have changed or to restore to last happy prefs.
            Me.lblDefaultColorLeft.Tag = Me.lblDefaultColorLeft.BackColor.ToArgb
            Me.lblDefaultColorRight.Tag = Me.lblDefaultColorRight.BackColor.ToArgb
            Me.tbDefaultFontSize.Tag = Me.tbDefaultFontSize.Text
            Me.cbShadowsEnabled.Tag = Me.cbShadowsEnabled.Checked
            Me.tbDefaultImageDir.Tag = Me.tbDefaultImageDir.Text
            Me.tbDefaultImageFile.Tag = Me.tbDefaultImageFile.Text
            Me.cbDisplayDefaultImage.Tag = Me.cbDisplayDefaultImage.Checked
            Me.tbDefaultHBFile.Tag = Me.tbDefaultHBFile.Text
            Me.cbLoadDefaultHB.Tag = Me.cbLoadDefaultHB.Checked
            Me.nudDefaultSlideDelay.Tag = Me.nudDefaultSlideDelay.Value
            Me.tbDefaultSlideShow.Tag = Me.tbDefaultSlideShow.Text
            Me.cbPlaySlidesAtStart.Tag = Me.cbPlaySlidesAtStart.Checked
            Me.cbLoadDefaultSlides.Tag = Me.cbLoadDefaultSlides.Checked
            Me.nudDefaultCountdownHours.Tag = Me.nudDefaultCountdownHours.Value
            Me.nudDefaultCountdownMinutes.Tag = Me.nudDefaultCountdownMinutes.Value
            Me.nudDefaultCountdownSeconds.Tag = Me.nudDefaultCountdownSeconds.Value
        End Sub
        Private Sub ApplyPrefs()
            Me.SetTeamColor("Left", Me.lblDefaultColorLeft.BackColor)
            Me.SetTeamColor("Right", Me.lblDefaultColorRight.BackColor)
            Me.tbLeftText.BackColor = Me.lblDefaultColorLeft.BackColor
            Me.tbRightText.BackColor = Me.lblDefaultColorRight.BackColor
            Me.tbLeftFontSize.Text = Me.tbDefaultFontSize.Text
            Me.tbRightFontSize.Text = Me.tbDefaultFontSize.Text
            Me.LS.SetTextShadows(Me.cbShadowsEnabled.Checked)
            Me.RS.SetTextShadows(Me.cbShadowsEnabled.Checked)
            If Me.cbDisplayDefaultImage.Checked Then Me.DisplayImageFile("Both", Me.tbDefaultImageFile.Text)
            If Me.cbLoadDefaultHB.Checked Then
                If Me.tbDefaultHBFile.Text <> "" Then Me.LoadHotButtons(Me.tbDefaultHBFile.Text)
            End If
            Me.radioThingColorLeft.BackColor = Me.lblDefaultColorLeft.BackColor
            Me.radioThingColorRight.BackColor = Me.lblDefaultColorRight.BackColor
            Me.nudDelay.Value = Me.nudDefaultSlideDelay.Value
            If Me.cbLoadDefaultSlides.Checked And (Me.tbDefaultSlideShow.Text.Length > 0) Then
                Me.LoadSlideShow(Me.tbDefaultSlideShow.Text)
                If Me.cbPlaySlidesAtStart.Checked Then
                    Me.StartSlideShow()
                End If
            End If
            Me.nudCountdownHours.Value = Me.nudDefaultCountdownHours.Value
            Me.nudCountdownMinutes.Value = Me.nudDefaultCountdownMinutes.Value
            Me.nudCountdownSeconds.Value = Me.nudDefaultCountdownSeconds.Value
            Me.StorePrefs()
        End Sub
        Private Function SelectDir(ByVal startdir As String) As String
            Dim pickdir As String = startdir
            Dim choose As New FolderBrowserDialog
            choose.Description = "Select a directory for this action."
            choose.ShowNewFolderButton = False
            choose.RootFolder = Environment.SpecialFolder.Desktop
            choose.SelectedPath = startdir
            If choose.ShowDialog() = DialogResult.OK Then
                pickdir = choose.SelectedPath
            End If
            choose.Dispose()
            Return pickdir
        End Function

        '=================================================================================================
        '* BEGIN COUNTDOWN TIMER STUFF

        Private Sub btnStartCountdown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartCountdown.Click
            If Me.CountdownTimer.Enabled Then
                Me.StopCountdown()
            Else
                Me.StartCountdown()
            End If
        End Sub
        Private Sub btnResetCountdown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetCountdown.Click
            Me.ResetCountdown()
        End Sub
        Private Sub cbCountdownVisible_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCountdownVisible.CheckedChanged
            Me.UpdateCountdown()
        End Sub
        Private Sub CountdownTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CountdownTimer.Tick
            Me.CountdownOneTick()
        End Sub

        Private Sub CountdownOneTick()
            Me.nudCountdownHours.Value = CInt(Fix(Me.CountdownSeconds / 3600))
            Me.nudCountdownMinutes.Value = CInt(Fix((Me.CountdownSeconds Mod 3600) / 60))
            Me.nudCountdownSeconds.Value = Me.CountdownSeconds Mod 60

            Me.UpdateCountdown()

            If Me.CountdownSeconds < 1 Then
                Me.StopCountdown()
                Exit Sub
            End If

            Me.CountdownSeconds = Me.CountdownSeconds - 1
        End Sub

        Private Sub nudCountdown_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCountdownHours.ValueChanged, nudCountdownMinutes.ValueChanged, nudCountdownSeconds.ValueChanged
            If Not Me.CountdownTimer.Enabled Then Me.UpdateCountdown()
        End Sub

        Private Sub CountdownWarnTimeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCountdownWarnHours.ValueChanged, nudCountdownWarnMinutes.ValueChanged, nudCountdownWarnSeconds.ValueChanged, nudCountdownWarnHours.TextChanged, nudCountdownWarnMinutes.TextChanged, nudCountdownWarnSeconds.TextChanged
            Me.CountdownWarnSeconds = Me.ComputeSeconds(Me.nudCountdownWarnHours.Value, Me.nudCountdownWarnMinutes.Value, Me.nudCountdownWarnSeconds.Value)
        End Sub

        Private Sub StartCountdown()
            If Me.CountdownTimer.Enabled Then Exit Sub
            Me.CountdownSeconds = Me.ComputeSeconds(Me.nudCountdownHours.Value, Me.nudCountdownMinutes.Value, Me.nudCountdownSeconds.Value)
            Me.CountdownTimer.Interval = 1000
            Me.nudCountdownHours.Enabled = False
            Me.nudCountdownMinutes.Enabled = False
            Me.nudCountdownSeconds.Enabled = False
            Me.CountdownTimer.Enabled = True
            Me.btnStartCountdown.Text = "STOP"
            Me.btnStartCountdown.ForeColor = System.Drawing.Color.Red
            Me.CountdownOneTick()    '* First tick makes timer appear right away instead of 1 second delay
        End Sub
        Private Sub StopCountdown()
            If Not Me.CountdownTimer.Enabled Then Exit Sub
            Me.CountdownTimer.Enabled = False
            Me.nudCountdownHours.Enabled = True
            Me.nudCountdownMinutes.Enabled = True
            Me.nudCountdownSeconds.Enabled = True
            Me.btnStartCountdown.Text = "START"
            Me.btnStartCountdown.ForeColor = System.Drawing.Color.Green
        End Sub
        Private Sub ResetCountdown()
            Me.StopCountdown()
            Me.nudCountdownHours.Value = Me.nudDefaultCountdownHours.Value
            Me.nudCountdownMinutes.Value = Me.nudDefaultCountdownMinutes.Value
            Me.nudCountdownSeconds.Value = Me.nudDefaultCountdownSeconds.Value
            Me.CountdownSeconds = Me.ComputeSeconds(Me.nudCountdownHours.Value, Me.nudCountdownMinutes.Value, Me.nudCountdownSeconds.Value)
            Me.UpdateCountdown()
        End Sub

        Private Function ComputeSeconds(ByVal hours As Integer, ByVal minutes As Integer, ByVal seconds As Integer) As Integer
            Return (hours * 3600) + (minutes * 60) + seconds
        End Function

        Private Sub UpdateCountdown()
            If Not Me.ComponentsDoneInitializing Then Exit Sub '* OR ELSE unhandled exception at app launch
            Dim TimeText As String = ""
            Dim bgColor As System.Drawing.Color = System.Drawing.Color.Black

            If Me.CountdownSeconds <= Me.CountdownWarnSeconds Then
                bgColor = System.Drawing.Color.Red
            End If

            If Me.nudCountdownHours.Value > 0 Then TimeText = Me.nudCountdownHours.Value.ToString + ":"
            TimeText = TimeText + Format(Me.nudCountdownMinutes.Value, "00") + ":" + Format(Me.nudCountdownSeconds.Value, "00")

            Me.LS.ShowCountdownText(TimeText, bgColor, Me.cbCountdownVisible.Checked)
            Me.RS.ShowCountdownText(TimeText, bgColor, Me.cbCountdownVisible.Checked)
        End Sub

        '=================================================================================================

        Private Sub EasterEgg1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EasterEgg1.Click
            MessageBox.Show("Pass it on...", "BILL LOVES BETSE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
        End Sub
    End Class
End Namespace