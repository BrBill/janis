'Imports System
'Imports System.Environment
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
'Imports System.Security.Principal

'Imports System.Runtime.InteropServices
Imports System.Threading.Tasks

Namespace JANIS
    Public Class fmMain
        Inherits System.Windows.Forms.Form

        <System.STAThread()>
        Public Shared Sub Main()
            '* This wraps the program initialization to enable modern Windows visual styling (XP+)
            System.Windows.Forms.Application.EnableVisualStyles()
            System.Windows.Forms.Application.Run(New fmMain)
        End Sub

        '***************
        '* System Constants
        '***************
        Const MAX_THINGS As Integer = 9
        Const DEFAULT_SLIDESHOW_DIR As String = "\SlideShows"
        Const DEFAULT_HOTBUTTON_DIR As String = "\HotButtons"
        Const SLIDES_STOPPED As Integer = 0
        Const SLIDES_PAUSED As Integer = 1
        Const SLIDES_PLAYING As Integer = 2
        Const SLIDES_WHAMMY As Integer = 3
        Private ReadOnly CTRL_A As Char = Convert.ToChar(1)
        Private ReadOnly BACKSPACE As Char = Convert.ToChar(8)

        Private Class FileID   ' Used by the image indexing system
            Public Path As String = ""   ' The directory it sits in.
            Public Name As String = ""   ' The name of this file.
            Public ReadOnly Property FullPath() As String
                Get
                    Return Path & "\" & Name
                End Get
            End Property
        End Class

        '***************
        '* Globals
        '***************
        Private ROOT_SUPPORT_DIR As String = "C:\JANIS"
        Private PREFS_FILE As String
        Private CurrentPrefs As New Preferences()
        Private SavedPrefs As New Preferences()
        Private splash As fmSplash
        Private ScreenReady As Boolean = False
        Private TestMode As Boolean = False
        Dim DisplayModeAdjustment As Single = 0.7     ' Divide font setting by this for display. Differs for test/arena mode.
        Dim DisplayToEntryFontRatio As Single = 123 / 42  ' This is the size ratio of fonts in the display vs. in the textbox
        Dim DisplayFontRatio As Single = 33 / 80          ' The "should be" size ratio of display to what I once thought it was.
        Private ReadOnly COUNTDOWN_DEFAULT_COLOR As System.Drawing.Color = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Private ReadOnly COUNTDOWN_WARN_COLOR As System.Drawing.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))

        '* NOTE: Many .MOV files are not playable by Windows Media Player w/o additional codecs (technically a purchased Microsoft Store app is required)
        Private VideoFileExtensions As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {".ASF", ".AVI", ".M2TS", ".M4V", ".MP4", ".MP4V", ".MPG", ".MPEG", ".WMV"}
        Private ImageFileExtensions As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {".BMP", ".GIF", ".JPG", ".JPEG", ".PNG", ".WMF", ".EXIF", ".TIFF"}
        Private MediaFileExtensions As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Private MediaLibrary As New List(Of FileID)

        Private WithEvents LS As fmScreen      '* The audience screen

        '* LibVLC preview players only
        Private _libVLC As LibVLCSharp.Shared.LibVLC
        Private _searchPreviewVideoPlayer As LibVLCSharp.Shared.MediaPlayer
        Private _searchPreviewVideoView As LibVLCSharp.WinForms.VideoView
        Private _slidePreviewVideoPlayer As LibVLCSharp.Shared.MediaPlayer
        Private _slidePreviewVideoView As LibVLCSharp.WinForms.VideoView

        Private ThingSubs(MAX_THINGS) As String           '* Substitutions for 5 Things
        Private SlidesStatus As Integer = SLIDES_STOPPED  '* Keep track of whether Slideshow is running
        Private SlideTimerTag As String = ""
        Private WhammyRandomizer As New Random
        Private PreSlideshowMuteState As Boolean = False  '* For returning to previous mute state in StopSlideShow()
        Private HotButtonsChanged As Boolean              '* Have we changed the hot buttons?
        Private BufferedSlide As Image = Nothing          '* For holding next whammy slide
        Private DragBounds As Rectangle
        Private DragMethod As String
        Private CountdownSeconds As Integer = 300
        Private CountdownWarnSeconds As Integer
        Private PreviousSelectedTab As TabPage = Nothing
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
            '   Dim splash as New fmSplash(Me)
            ' This example splashscreen also has the ability to show itself for a minimum
            ' number of seconds. For example, if you want to show the splash for at least 6
            ' seconds, change the code above to:
            ' Dim F As New fmSplash(Me, 6)
            splash = New fmSplash(Me, 3)

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call
            Me.ComponentsDoneInitializing = True
        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
                Me.SlideTimer?.Stop()
                Me.CountdownTimer?.Stop()
                Me.VideoEventTimer?.Stop()
                Me._searchPreviewVideoPlayer?.Stop()
                Me._slidePreviewVideoPlayer?.Stop()
                Me._searchPreviewVideoView?.Dispose()
                Me._slidePreviewVideoView?.Dispose()
                Me._searchPreviewVideoPlayer?.Dispose()
                Me._slidePreviewVideoPlayer?.Dispose()
                Me._libVLC?.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.
        'Do not modify it using the code editor.
        Friend WithEvents Label15 As System.Windows.Forms.Label
        Friend WithEvents tbLeftLoc As System.Windows.Forms.TextBox
        Friend WithEvents Label41 As System.Windows.Forms.Label
        Friend WithEvents tbRightLoc As System.Windows.Forms.TextBox
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
        Friend WithEvents grpClearText As System.Windows.Forms.GroupBox
        Friend WithEvents grpLoadText As System.Windows.Forms.GroupBox
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
        Friend WithEvents btnShowScore As System.Windows.Forms.Button
        Friend WithEvents grpInstantMedia As GroupBox
        Friend WithEvents picRemoteViewer As System.Windows.Forms.PictureBox
        Friend WithEvents btnMediaLoadFile As System.Windows.Forms.Button
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
        Friend WithEvents cbMuteVideo As CheckBox
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
        Friend WithEvents tvSlideFolders As System.Windows.Forms.TreeView
        Friend WithEvents lbSlideCandidates As System.Windows.Forms.ListBox
        Friend WithEvents lbSlideList As System.Windows.Forms.ListBox
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents btnThingDown As System.Windows.Forms.Button
        Friend WithEvents btnThingUp As System.Windows.Forms.Button
        Friend WithEvents btnClearThings As System.Windows.Forms.Button
        Friend WithEvents btnRemoveSlides As System.Windows.Forms.Button
        Friend WithEvents btnAddSlide As System.Windows.Forms.Button
        Friend WithEvents btnSaveSlides As System.Windows.Forms.Button
        Friend WithEvents btnLoadSlides As System.Windows.Forms.Button
        Friend WithEvents btnSlideDown As System.Windows.Forms.Button
        Friend WithEvents btnSlideUp As System.Windows.Forms.Button
        Friend WithEvents btnStopSlides As System.Windows.Forms.Button
        Friend WithEvents btnLastSlide As System.Windows.Forms.Button
        Friend WithEvents btnPlaySlides As System.Windows.Forms.Button
        Friend WithEvents btnFirstSlide As System.Windows.Forms.Button
        Friend WithEvents pnlMediaSearchPreview As Panel
        Friend WithEvents picSlidePreview As System.Windows.Forms.PictureBox

        Friend WithEvents SlideTimer As System.Windows.Forms.Timer
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents nudDelay As System.Windows.Forms.NumericUpDown
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents tbAboutHeader As System.Windows.Forms.TextBox
        Friend WithEvents tbAboutBody As System.Windows.Forms.TextBox
        Friend WithEvents Label12 As System.Windows.Forms.Label
        Friend WithEvents btnWhammy As System.Windows.Forms.Button
        Friend WithEvents btnClearSlideList As System.Windows.Forms.Button
        Friend WithEvents btnShowLeftText As System.Windows.Forms.Button
        Friend WithEvents btnDocLoadRight As System.Windows.Forms.Button
        Friend WithEvents btnDocLoadLeft As System.Windows.Forms.Button
        Friend WithEvents btnPrevSlide As System.Windows.Forms.Button
        Friend WithEvents btnNextSlide As System.Windows.Forms.Button
        Friend WithEvents btnPauseSlides As System.Windows.Forms.Button
        Friend WithEvents tbCurrentThing As System.Windows.Forms.TextBox
        Friend WithEvents btnShowRightText As System.Windows.Forms.Button
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
        Friend WithEvents pnlRemoteViewer As System.Windows.Forms.Panel
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
        Friend WithEvents tpMediaSearch As System.Windows.Forms.TabPage
        Friend WithEvents btnImgSearch As System.Windows.Forms.Button
        Friend WithEvents btnSearchMediaAddSlide As System.Windows.Forms.Button
        Friend WithEvents btnSearchMediaShow As System.Windows.Forms.Button
        Friend WithEvents picImgSearchPreview As System.Windows.Forms.PictureBox
        Friend WithEvents Label20 As System.Windows.Forms.Label
        Friend WithEvents comboImgSearchText As System.Windows.Forms.ComboBox
        Friend WithEvents lbMediaResults As System.Windows.Forms.ListBox
        Friend WithEvents Label21 As System.Windows.Forms.Label
        Friend WithEvents Label32 As System.Windows.Forms.Label
        Friend WithEvents lblMediaLibraryCount As System.Windows.Forms.Label
        Friend WithEvents btnPasteMedia As System.Windows.Forms.Button
        Friend WithEvents lblRemoteStatus As Label
        Friend WithEvents VideoEventTimer As Timer



        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim Label19 As System.Windows.Forms.Label
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fmMain))
            Me.btnBlackout = New System.Windows.Forms.Button()
            Me.tbLeftTeam = New System.Windows.Forms.TextBox()
            Me.tbRightTeam = New System.Windows.Forms.TextBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.tbLeftScore = New System.Windows.Forms.TextBox()
            Me.tbRightScore = New System.Windows.Forms.TextBox()
            Me.btnShowScore = New System.Windows.Forms.Button()
            Me.btnMediaLoadFile = New System.Windows.Forms.Button()
            Me.btnLeftScoreColor = New System.Windows.Forms.Button()
            Me.btnRightScoreColor = New System.Windows.Forms.Button()
            Me.TabControl1 = New System.Windows.Forms.TabControl()
            Me.tpScreenText = New System.Windows.Forms.TabPage()
            Me.btnShowRightText = New System.Windows.Forms.Button()
            Me.btnShowLeftText = New System.Windows.Forms.Button()
            Me.grpLoadText = New System.Windows.Forms.GroupBox()
            Me.btnDocLoadLeft = New System.Windows.Forms.Button()
            Me.btnDocLoadRight = New System.Windows.Forms.Button()
            Me.grpClearText = New System.Windows.Forms.GroupBox()
            Me.btnClearTextLeft = New System.Windows.Forms.Button()
            Me.btnClearTextBoth = New System.Windows.Forms.Button()
            Me.btnClearTextRight = New System.Windows.Forms.Button()
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
            Me.tpMediaSearch = New System.Windows.Forms.TabPage()
            Me.pnlMediaSearchPreview = New System.Windows.Forms.Panel()
            Me.picImgSearchPreview = New System.Windows.Forms.PictureBox()
            Me.Label32 = New System.Windows.Forms.Label()
            Me.Label21 = New System.Windows.Forms.Label()
            Me.lbMediaResults = New System.Windows.Forms.ListBox()
            Me.comboImgSearchText = New System.Windows.Forms.ComboBox()
            Me.Label20 = New System.Windows.Forms.Label()
            Me.btnImgSearch = New System.Windows.Forms.Button()
            Me.btnSearchMediaAddSlide = New System.Windows.Forms.Button()
            Me.btnSearchMediaShow = New System.Windows.Forms.Button()
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
            Me.tpSlides = New System.Windows.Forms.TabPage()
            Me.lbSlideCandidates = New System.Windows.Forms.ListBox()
            Me.tvSlideFolders = New System.Windows.Forms.TreeView()
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
            Me.tbAboutHeader = New System.Windows.Forms.TextBox()
            Me.tbAboutBody = New System.Windows.Forms.TextBox()
            Me.SlideTimer = New System.Windows.Forms.Timer(Me.components)
            Me.pnlRemoteViewer = New System.Windows.Forms.Panel()
            Me.cbMuteVideo = New System.Windows.Forms.CheckBox()
            Me.lblRemoteStatus = New System.Windows.Forms.Label()
            Me.picRemoteViewer = New System.Windows.Forms.PictureBox()
            Me.CountdownTimer = New System.Windows.Forms.Timer(Me.components)
            Me.lblMediaLibraryCount = New System.Windows.Forms.Label()
            Me.btnPasteMedia = New System.Windows.Forms.Button()
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
            Me.btnHot10 = New System.Windows.Forms.Button()
            Me.btnHot9 = New System.Windows.Forms.Button()
            Me.btnHot8 = New System.Windows.Forms.Button()
            Me.btnHot7 = New System.Windows.Forms.Button()
            Me.btnHot6 = New System.Windows.Forms.Button()
            Me.btnHot5 = New System.Windows.Forms.Button()
            Me.btnHot4 = New System.Windows.Forms.Button()
            Me.btnHot3 = New System.Windows.Forms.Button()
            Me.btnHot2 = New System.Windows.Forms.Button()
            Me.btnHot1 = New System.Windows.Forms.Button()
            Me.Label15 = New System.Windows.Forms.Label()
            Me.tbLeftLoc = New System.Windows.Forms.TextBox()
            Me.Label41 = New System.Windows.Forms.Label()
            Me.tbRightLoc = New System.Windows.Forms.TextBox()
            Me.grpInstantMedia = New System.Windows.Forms.GroupBox()
            Me.VideoEventTimer = New System.Windows.Forms.Timer(Me.components)
            Label19 = New System.Windows.Forms.Label()
            Me.TabControl1.SuspendLayout()
            Me.tpScreenText.SuspendLayout()
            Me.grpLoadText.SuspendLayout()
            Me.grpClearText.SuspendLayout()
            Me.grpRightColors.SuspendLayout()
            Me.grpLeftColors.SuspendLayout()
            Me.tpMediaSearch.SuspendLayout()
            Me.pnlMediaSearchPreview.SuspendLayout()
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
            Me.pnlRemoteViewer.SuspendLayout()
            CType(Me.picRemoteViewer, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.gbCountdownControls.SuspendLayout()
            CType(Me.nudCountdownWarnSeconds, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudCountdownWarnMinutes, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudCountdownWarnHours, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudCountdownSeconds, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudCountdownMinutes, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudCountdownHours, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpInstantMedia.SuspendLayout()
            Me.SuspendLayout()
            '
            'Label19
            '
            Label19.BackColor = System.Drawing.Color.Transparent
            Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold)
            Label19.Location = New System.Drawing.Point(313, 0)
            Label19.Name = "Label19"
            Label19.Size = New System.Drawing.Size(243, 21)
            Label19.TabIndex = 90
            Label19.Text = "Media Preview:"
            Label19.TextAlign = System.Drawing.ContentAlignment.BottomCenter
            '
            'btnBlackout
            '
            Me.btnBlackout.BackColor = System.Drawing.Color.Black
            Me.btnBlackout.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control
            Me.btnBlackout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray
            Me.btnBlackout.FlatStyle = System.Windows.Forms.FlatStyle.Popup
            Me.btnBlackout.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnBlackout.ForeColor = System.Drawing.Color.White
            Me.btnBlackout.Location = New System.Drawing.Point(76, 180)
            Me.btnBlackout.Name = "btnBlackout"
            Me.btnBlackout.Size = New System.Drawing.Size(160, 65)
            Me.btnBlackout.TabIndex = 19
            Me.btnBlackout.Text = " &BLACKOUT!"
            Me.ToolTip1.SetToolTip(Me.btnBlackout, "Hotkey: ALT-B")
            Me.btnBlackout.UseVisualStyleBackColor = False
            '
            'tbLeftTeam
            '
            Me.tbLeftTeam.BackColor = System.Drawing.SystemColors.Window
            Me.tbLeftTeam.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbLeftTeam.ForeColor = System.Drawing.SystemColors.WindowText
            Me.tbLeftTeam.Location = New System.Drawing.Point(103, 29)
            Me.tbLeftTeam.MaxLength = 13
            Me.tbLeftTeam.Name = "tbLeftTeam"
            Me.tbLeftTeam.Size = New System.Drawing.Size(172, 22)
            Me.tbLeftTeam.TabIndex = 3
            Me.tbLeftTeam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbRightTeam
            '
            Me.tbRightTeam.BackColor = System.Drawing.SystemColors.Window
            Me.tbRightTeam.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbRightTeam.ForeColor = System.Drawing.SystemColors.WindowText
            Me.tbRightTeam.Location = New System.Drawing.Point(720, 29)
            Me.tbRightTeam.MaxLength = 13
            Me.tbRightTeam.Name = "tbRightTeam"
            Me.tbRightTeam.Size = New System.Drawing.Size(172, 22)
            Me.tbRightTeam.TabIndex = 8
            Me.tbRightTeam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.Transparent
            Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label1.Location = New System.Drawing.Point(3, 29)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(97, 24)
            Me.Label1.TabIndex = 2
            Me.Label1.Text = "Team Name"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.Transparent
            Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label2.Location = New System.Drawing.Point(894, 29)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(97, 24)
            Me.Label2.TabIndex = 9
            Me.Label2.Text = "Team Name"
            Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'tbLeftScore
            '
            Me.tbLeftScore.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.tbLeftScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
            Me.tbLeftScore.ForeColor = System.Drawing.Color.White
            Me.tbLeftScore.Location = New System.Drawing.Point(386, 2)
            Me.tbLeftScore.MaxLength = 3
            Me.tbLeftScore.Name = "tbLeftScore"
            Me.tbLeftScore.Size = New System.Drawing.Size(70, 35)
            Me.tbLeftScore.TabIndex = 11
            Me.tbLeftScore.Text = "0"
            Me.tbLeftScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.ToolTip1.SetToolTip(Me.tbLeftScore, "Left Team Instant ScoreKeys:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "F1: +1, F2: -1, F3: +5, F4: -5")
            '
            'tbRightScore
            '
            Me.tbRightScore.BackColor = System.Drawing.Color.Maroon
            Me.tbRightScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold)
            Me.tbRightScore.ForeColor = System.Drawing.Color.White
            Me.tbRightScore.Location = New System.Drawing.Point(542, 2)
            Me.tbRightScore.MaxLength = 3
            Me.tbRightScore.Name = "tbRightScore"
            Me.tbRightScore.Size = New System.Drawing.Size(70, 35)
            Me.tbRightScore.TabIndex = 13
            Me.tbRightScore.Text = "0"
            Me.tbRightScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.ToolTip1.SetToolTip(Me.tbRightScore, "Right Team Instant ScoreKeys:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "F5: +1, F6: -1, F7: +5, F8: -5")
            '
            'btnShowScore
            '
            Me.btnShowScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnShowScore.Location = New System.Drawing.Point(459, 2)
            Me.btnShowScore.Name = "btnShowScore"
            Me.btnShowScore.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
            Me.btnShowScore.Size = New System.Drawing.Size(78, 35)
            Me.btnShowScore.TabIndex = 18
            Me.btnShowScore.Text = "SCORE!"
            Me.ToolTip1.SetToolTip(Me.btnShowScore, "Hover over scores to see scoring hot keys")
            '
            'btnMediaLoadFile
            '
            Me.btnMediaLoadFile.AllowDrop = True
            Me.btnMediaLoadFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
            Me.btnMediaLoadFile.Location = New System.Drawing.Point(3, 18)
            Me.btnMediaLoadFile.Name = "btnMediaLoadFile"
            Me.btnMediaLoadFile.Size = New System.Drawing.Size(119, 28)
            Me.btnMediaLoadFile.TabIndex = 16
            Me.btnMediaLoadFile.Text = "OPEN FILE"
            '
            'btnLeftScoreColor
            '
            Me.btnLeftScoreColor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnLeftScoreColor.Location = New System.Drawing.Point(282, 3)
            Me.btnLeftScoreColor.Name = "btnLeftScoreColor"
            Me.btnLeftScoreColor.Size = New System.Drawing.Size(70, 24)
            Me.btnLeftScoreColor.TabIndex = 4
            Me.btnLeftScoreColor.Text = "Color..."
            '
            'btnRightScoreColor
            '
            Me.btnRightScoreColor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnRightScoreColor.Location = New System.Drawing.Point(644, 4)
            Me.btnRightScoreColor.Name = "btnRightScoreColor"
            Me.btnRightScoreColor.Size = New System.Drawing.Size(70, 24)
            Me.btnRightScoreColor.TabIndex = 5
            Me.btnRightScoreColor.Text = "Color..."
            '
            'TabControl1
            '
            Me.TabControl1.Controls.Add(Me.tpScreenText)
            Me.TabControl1.Controls.Add(Me.tpMediaSearch)
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
            Me.TabControl1.TabIndex = 47
            '
            'tpScreenText
            '
            Me.tpScreenText.BackColor = System.Drawing.Color.Transparent
            Me.tpScreenText.Controls.Add(Me.btnShowRightText)
            Me.tpScreenText.Controls.Add(Me.btnShowLeftText)
            Me.tpScreenText.Controls.Add(Me.grpLoadText)
            Me.tpScreenText.Controls.Add(Me.grpClearText)
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
            'btnShowRightText
            '
            Me.btnShowRightText.Font = New System.Drawing.Font("Wingdings", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
            Me.btnShowRightText.Location = New System.Drawing.Point(752, 305)
            Me.btnShowRightText.Name = "btnShowRightText"
            Me.btnShowRightText.Padding = New System.Windows.Forms.Padding(4, 0, 0, 0)
            Me.btnShowRightText.Size = New System.Drawing.Size(68, 48)
            Me.btnShowRightText.TabIndex = 79
            Me.btnShowRightText.Text = "y"
            Me.ToolTip1.SetToolTip(Me.btnShowRightText, "Display Text")
            '
            'btnShowLeftText
            '
            Me.btnShowLeftText.Font = New System.Drawing.Font("Wingdings", 27.75!, System.Drawing.FontStyle.Bold)
            Me.btnShowLeftText.Location = New System.Drawing.Point(168, 305)
            Me.btnShowLeftText.Name = "btnShowLeftText"
            Me.btnShowLeftText.Padding = New System.Windows.Forms.Padding(4, 0, 0, 0)
            Me.btnShowLeftText.Size = New System.Drawing.Size(68, 48)
            Me.btnShowLeftText.TabIndex = 78
            Me.btnShowLeftText.Text = "y"
            Me.ToolTip1.SetToolTip(Me.btnShowLeftText, "Display Text")
            '
            'grpLoadText
            '
            Me.grpLoadText.Controls.Add(Me.btnDocLoadLeft)
            Me.grpLoadText.Controls.Add(Me.btnDocLoadRight)
            Me.grpLoadText.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpLoadText.Location = New System.Drawing.Point(422, 192)
            Me.grpLoadText.Name = "grpLoadText"
            Me.grpLoadText.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.grpLoadText.Size = New System.Drawing.Size(148, 70)
            Me.grpLoadText.TabIndex = 74
            Me.grpLoadText.TabStop = False
            Me.grpLoadText.Text = "Load Text File"
            '
            'btnDocLoadLeft
            '
            Me.btnDocLoadLeft.BackgroundImage = Global.JANIS.My.Resources.Resources.left_arrow
            Me.btnDocLoadLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            Me.btnDocLoadLeft.FlatAppearance.BorderSize = 0
            Me.btnDocLoadLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray
            Me.btnDocLoadLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnDocLoadLeft.Font = New System.Drawing.Font("Arial Narrow", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnDocLoadLeft.Location = New System.Drawing.Point(2, 18)
            Me.btnDocLoadLeft.Name = "btnDocLoadLeft"
            Me.btnDocLoadLeft.Size = New System.Drawing.Size(47, 46)
            Me.btnDocLoadLeft.TabIndex = 75
            Me.btnDocLoadLeft.Tag = "Left"
            Me.btnDocLoadLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'btnDocLoadRight
            '
            Me.btnDocLoadRight.BackgroundImage = Global.JANIS.My.Resources.Resources.right_arrow
            Me.btnDocLoadRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            Me.btnDocLoadRight.FlatAppearance.BorderSize = 0
            Me.btnDocLoadRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray
            Me.btnDocLoadRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnDocLoadRight.Font = New System.Drawing.Font("Arial Narrow", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnDocLoadRight.Location = New System.Drawing.Point(98, 18)
            Me.btnDocLoadRight.Name = "btnDocLoadRight"
            Me.btnDocLoadRight.Size = New System.Drawing.Size(47, 46)
            Me.btnDocLoadRight.TabIndex = 77
            Me.btnDocLoadRight.Tag = "Right"
            Me.btnDocLoadRight.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'grpClearText
            '
            Me.grpClearText.Controls.Add(Me.btnClearTextLeft)
            Me.grpClearText.Controls.Add(Me.btnClearTextBoth)
            Me.grpClearText.Controls.Add(Me.btnClearTextRight)
            Me.grpClearText.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpClearText.Location = New System.Drawing.Point(422, 92)
            Me.grpClearText.Name = "grpClearText"
            Me.grpClearText.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.grpClearText.Size = New System.Drawing.Size(148, 70)
            Me.grpClearText.TabIndex = 70
            Me.grpClearText.TabStop = False
            Me.grpClearText.Text = "Clear Text"
            '
            'btnClearTextLeft
            '
            Me.btnClearTextLeft.BackColor = System.Drawing.Color.Transparent
            Me.btnClearTextLeft.BackgroundImage = Global.JANIS.My.Resources.Resources.left_arrow
            Me.btnClearTextLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            Me.btnClearTextLeft.FlatAppearance.BorderSize = 0
            Me.btnClearTextLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray
            Me.btnClearTextLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnClearTextLeft.Font = New System.Drawing.Font("Arial Narrow", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnClearTextLeft.ForeColor = System.Drawing.SystemColors.WindowText
            Me.btnClearTextLeft.Location = New System.Drawing.Point(2, 18)
            Me.btnClearTextLeft.Name = "btnClearTextLeft"
            Me.btnClearTextLeft.Size = New System.Drawing.Size(47, 46)
            Me.btnClearTextLeft.TabIndex = 71
            Me.btnClearTextLeft.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.btnClearTextLeft.UseVisualStyleBackColor = False
            '
            'btnClearTextBoth
            '
            Me.btnClearTextBoth.BackColor = System.Drawing.Color.Transparent
            Me.btnClearTextBoth.BackgroundImage = Global.JANIS.My.Resources.Resources.left_right_arrow
            Me.btnClearTextBoth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            Me.btnClearTextBoth.FlatAppearance.BorderSize = 0
            Me.btnClearTextBoth.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray
            Me.btnClearTextBoth.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnClearTextBoth.Font = New System.Drawing.Font("Arial Narrow", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnClearTextBoth.ForeColor = System.Drawing.SystemColors.WindowText
            Me.btnClearTextBoth.Location = New System.Drawing.Point(50, 18)
            Me.btnClearTextBoth.Name = "btnClearTextBoth"
            Me.btnClearTextBoth.Size = New System.Drawing.Size(47, 46)
            Me.btnClearTextBoth.TabIndex = 72
            Me.btnClearTextBoth.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.btnClearTextBoth.UseVisualStyleBackColor = False
            '
            'btnClearTextRight
            '
            Me.btnClearTextRight.BackColor = System.Drawing.Color.Transparent
            Me.btnClearTextRight.BackgroundImage = Global.JANIS.My.Resources.Resources.right_arrow
            Me.btnClearTextRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            Me.btnClearTextRight.FlatAppearance.BorderSize = 0
            Me.btnClearTextRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray
            Me.btnClearTextRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnClearTextRight.Font = New System.Drawing.Font("Arial Narrow", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnClearTextRight.ForeColor = System.Drawing.SystemColors.WindowText
            Me.btnClearTextRight.Location = New System.Drawing.Point(98, 18)
            Me.btnClearTextRight.Name = "btnClearTextRight"
            Me.btnClearTextRight.Size = New System.Drawing.Size(47, 46)
            Me.btnClearTextRight.TabIndex = 73
            Me.btnClearTextRight.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.btnClearTextRight.UseVisualStyleBackColor = False
            '
            'Label11
            '
            Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label11.Location = New System.Drawing.Point(902, 2)
            Me.Label11.Name = "Label11"
            Me.Label11.Size = New System.Drawing.Size(65, 20)
            Me.Label11.TabIndex = 66
            Me.Label11.Text = "Font Size"
            Me.Label11.TextAlign = System.Drawing.ContentAlignment.BottomCenter
            '
            'Label10
            '
            Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label10.Location = New System.Drawing.Point(25, 2)
            Me.Label10.Name = "Label10"
            Me.Label10.Size = New System.Drawing.Size(64, 23)
            Me.Label10.TabIndex = 48
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
            Me.tbLeftText.TabIndex = 68
            Me.tbLeftText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbRightFontSize
            '
            Me.tbRightFontSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbRightFontSize.Location = New System.Drawing.Point(905, 27)
            Me.tbRightFontSize.MaxLength = 3
            Me.tbRightFontSize.Name = "tbRightFontSize"
            Me.tbRightFontSize.Size = New System.Drawing.Size(56, 24)
            Me.tbRightFontSize.TabIndex = 67
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
            Me.grpRightColors.TabIndex = 58
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
            Me.btnChooseTextColorRight.TabIndex = 59
            Me.btnChooseTextColorRight.Text = "Other..."
            '
            'pnlTextColorRight6
            '
            Me.pnlTextColorRight6.BackColor = System.Drawing.Color.Black
            Me.pnlTextColorRight6.Location = New System.Drawing.Point(219, 18)
            Me.pnlTextColorRight6.Name = "pnlTextColorRight6"
            Me.pnlTextColorRight6.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorRight6.TabIndex = 65
            Me.pnlTextColorRight6.TabStop = True
            '
            'pnlTextColorRight5
            '
            Me.pnlTextColorRight5.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlTextColorRight5.Location = New System.Drawing.Point(195, 18)
            Me.pnlTextColorRight5.Name = "pnlTextColorRight5"
            Me.pnlTextColorRight5.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorRight5.TabIndex = 64
            Me.pnlTextColorRight5.TabStop = True
            '
            'pnlTextColorRight4
            '
            Me.pnlTextColorRight4.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.pnlTextColorRight4.Location = New System.Drawing.Point(171, 18)
            Me.pnlTextColorRight4.Name = "pnlTextColorRight4"
            Me.pnlTextColorRight4.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorRight4.TabIndex = 63
            Me.pnlTextColorRight4.TabStop = True
            '
            'pnlTextColorRight3
            '
            Me.pnlTextColorRight3.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlTextColorRight3.Location = New System.Drawing.Point(147, 18)
            Me.pnlTextColorRight3.Name = "pnlTextColorRight3"
            Me.pnlTextColorRight3.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorRight3.TabIndex = 62
            Me.pnlTextColorRight3.TabStop = True
            '
            'pnlTextColorRight2
            '
            Me.pnlTextColorRight2.BackColor = System.Drawing.Color.Maroon
            Me.pnlTextColorRight2.Location = New System.Drawing.Point(123, 18)
            Me.pnlTextColorRight2.Name = "pnlTextColorRight2"
            Me.pnlTextColorRight2.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorRight2.TabIndex = 61
            Me.pnlTextColorRight2.TabStop = True
            '
            'pnlTextColorRight1
            '
            Me.pnlTextColorRight1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.pnlTextColorRight1.Location = New System.Drawing.Point(99, 18)
            Me.pnlTextColorRight1.Name = "pnlTextColorRight1"
            Me.pnlTextColorRight1.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorRight1.TabIndex = 60
            Me.pnlTextColorRight1.TabStop = True
            '
            'tbRightText
            '
            Me.tbRightText.BackColor = System.Drawing.Color.Maroon
            Me.tbRightText.Font = New System.Drawing.Font("Arial", 20.0!, System.Drawing.FontStyle.Bold)
            Me.tbRightText.ForeColor = System.Drawing.Color.White
            Me.tbRightText.Location = New System.Drawing.Point(586, 60)
            Me.tbRightText.Multiline = True
            Me.tbRightText.Name = "tbRightText"
            Me.tbRightText.Size = New System.Drawing.Size(400, 225)
            Me.tbRightText.TabIndex = 69
            Me.tbRightText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbLeftFontSize
            '
            Me.tbLeftFontSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbLeftFontSize.Location = New System.Drawing.Point(28, 27)
            Me.tbLeftFontSize.MaxLength = 3
            Me.tbLeftFontSize.Name = "tbLeftFontSize"
            Me.tbLeftFontSize.Size = New System.Drawing.Size(56, 24)
            Me.tbLeftFontSize.TabIndex = 49
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
            Me.grpLeftColors.TabIndex = 50
            Me.grpLeftColors.TabStop = False
            Me.grpLeftColors.Text = "Background"
            '
            'btnChooseTextColorLeft
            '
            Me.btnChooseTextColorLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnChooseTextColorLeft.Location = New System.Drawing.Point(156, 13)
            Me.btnChooseTextColorLeft.Name = "btnChooseTextColorLeft"
            Me.btnChooseTextColorLeft.Size = New System.Drawing.Size(78, 26)
            Me.btnChooseTextColorLeft.TabIndex = 57
            Me.btnChooseTextColorLeft.Text = "Other..."
            '
            'pnlTextColorLeft6
            '
            Me.pnlTextColorLeft6.BackColor = System.Drawing.Color.Black
            Me.pnlTextColorLeft6.Location = New System.Drawing.Point(129, 18)
            Me.pnlTextColorLeft6.Name = "pnlTextColorLeft6"
            Me.pnlTextColorLeft6.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorLeft6.TabIndex = 56
            Me.pnlTextColorLeft6.TabStop = True
            '
            'pnlTextColorLeft5
            '
            Me.pnlTextColorLeft5.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlTextColorLeft5.Location = New System.Drawing.Point(105, 18)
            Me.pnlTextColorLeft5.Name = "pnlTextColorLeft5"
            Me.pnlTextColorLeft5.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorLeft5.TabIndex = 55
            Me.pnlTextColorLeft5.TabStop = True
            '
            'pnlTextColorLeft4
            '
            Me.pnlTextColorLeft4.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.pnlTextColorLeft4.Location = New System.Drawing.Point(81, 18)
            Me.pnlTextColorLeft4.Name = "pnlTextColorLeft4"
            Me.pnlTextColorLeft4.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorLeft4.TabIndex = 54
            Me.pnlTextColorLeft4.TabStop = True
            '
            'pnlTextColorLeft3
            '
            Me.pnlTextColorLeft3.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlTextColorLeft3.Location = New System.Drawing.Point(57, 18)
            Me.pnlTextColorLeft3.Name = "pnlTextColorLeft3"
            Me.pnlTextColorLeft3.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorLeft3.TabIndex = 53
            Me.pnlTextColorLeft3.TabStop = True
            '
            'pnlTextColorLeft2
            '
            Me.pnlTextColorLeft2.BackColor = System.Drawing.Color.Maroon
            Me.pnlTextColorLeft2.Location = New System.Drawing.Point(33, 18)
            Me.pnlTextColorLeft2.Name = "pnlTextColorLeft2"
            Me.pnlTextColorLeft2.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorLeft2.TabIndex = 52
            Me.pnlTextColorLeft2.TabStop = True
            '
            'pnlTextColorLeft1
            '
            Me.pnlTextColorLeft1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.pnlTextColorLeft1.Location = New System.Drawing.Point(9, 18)
            Me.pnlTextColorLeft1.Name = "pnlTextColorLeft1"
            Me.pnlTextColorLeft1.Size = New System.Drawing.Size(16, 16)
            Me.pnlTextColorLeft1.TabIndex = 51
            Me.pnlTextColorLeft1.TabStop = True
            '
            'tpMediaSearch
            '
            Me.tpMediaSearch.Controls.Add(Me.pnlMediaSearchPreview)
            Me.tpMediaSearch.Controls.Add(Me.Label32)
            Me.tpMediaSearch.Controls.Add(Me.Label21)
            Me.tpMediaSearch.Controls.Add(Me.lbMediaResults)
            Me.tpMediaSearch.Controls.Add(Me.comboImgSearchText)
            Me.tpMediaSearch.Controls.Add(Me.Label20)
            Me.tpMediaSearch.Controls.Add(Me.btnImgSearch)
            Me.tpMediaSearch.Controls.Add(Me.btnSearchMediaAddSlide)
            Me.tpMediaSearch.Controls.Add(Me.btnSearchMediaShow)
            Me.tpMediaSearch.Controls.Add(Label19)
            Me.tpMediaSearch.Location = New System.Drawing.Point(4, 28)
            Me.tpMediaSearch.Name = "tpMediaSearch"
            Me.tpMediaSearch.Size = New System.Drawing.Size(988, 372)
            Me.tpMediaSearch.TabIndex = 6
            Me.tpMediaSearch.Text = "Media Search"
            '
            'pnlMediaSearchPreview
            '
            Me.pnlMediaSearchPreview.Controls.Add(Me.picImgSearchPreview)
            Me.pnlMediaSearchPreview.Location = New System.Drawing.Point(316, 24)
            Me.pnlMediaSearchPreview.Name = "pnlMediaSearchPreview"
            Me.pnlMediaSearchPreview.Size = New System.Drawing.Size(240, 135)
            Me.pnlMediaSearchPreview.TabIndex = 96
            '
            'picImgSearchPreview
            '
            Me.picImgSearchPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.picImgSearchPreview.Location = New System.Drawing.Point(0, 0)
            Me.picImgSearchPreview.Name = "picImgSearchPreview"
            Me.picImgSearchPreview.Size = New System.Drawing.Size(240, 135)
            Me.picImgSearchPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.picImgSearchPreview.TabIndex = 69
            Me.picImgSearchPreview.TabStop = False
            '
            'Label32
            '
            Me.Label32.BackColor = System.Drawing.Color.LemonChiffon
            Me.Label32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.Label32.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label32.ForeColor = System.Drawing.SystemColors.ControlText
            Me.Label32.Location = New System.Drawing.Point(572, 11)
            Me.Label32.Name = "Label32"
            Me.Label32.Size = New System.Drawing.Size(402, 40)
            Me.Label32.TabIndex = 91
            Me.Label32.Text = "Hint: Drag Media Preview to a Hot Button to assign it instantly!"
            Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'Label21
            '
            Me.Label21.BackColor = System.Drawing.Color.Transparent
            Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label21.Location = New System.Drawing.Point(12, 140)
            Me.Label21.Name = "Label21"
            Me.Label21.Size = New System.Drawing.Size(124, 16)
            Me.Label21.TabIndex = 94
            Me.Label21.Text = "Search Results"
            Me.Label21.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'lbMediaResults
            '
            Me.lbMediaResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lbMediaResults.HorizontalScrollbar = True
            Me.lbMediaResults.ItemHeight = 18
            Me.lbMediaResults.Location = New System.Drawing.Point(12, 162)
            Me.lbMediaResults.Name = "lbMediaResults"
            Me.lbMediaResults.Size = New System.Drawing.Size(962, 202)
            Me.lbMediaResults.Sorted = True
            Me.lbMediaResults.TabIndex = 95
            '
            'comboImgSearchText
            '
            Me.comboImgSearchText.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.comboImgSearchText.Location = New System.Drawing.Point(15, 76)
            Me.comboImgSearchText.Name = "comboImgSearchText"
            Me.comboImgSearchText.Size = New System.Drawing.Size(293, 26)
            Me.comboImgSearchText.TabIndex = 81
            Me.ToolTip1.SetToolTip(Me.comboImgSearchText, "Use * for wildcard")
            '
            'Label20
            '
            Me.Label20.BackColor = System.Drawing.Color.Transparent
            Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label20.Location = New System.Drawing.Point(12, 24)
            Me.Label20.Name = "Label20"
            Me.Label20.Size = New System.Drawing.Size(296, 36)
            Me.Label20.TabIndex = 80
            Me.Label20.Text = "Media Library text search" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(manage in Preferences):"
            Me.Label20.TextAlign = System.Drawing.ContentAlignment.BottomCenter
            '
            'btnImgSearch
            '
            Me.btnImgSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnImgSearch.Location = New System.Drawing.Point(212, 116)
            Me.btnImgSearch.Name = "btnImgSearch"
            Me.btnImgSearch.Size = New System.Drawing.Size(96, 34)
            Me.btnImgSearch.TabIndex = 82
            Me.btnImgSearch.Text = "SEARCH"
            '
            'btnSearchMediaAddSlide
            '
            Me.btnSearchMediaAddSlide.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSearchMediaAddSlide.Location = New System.Drawing.Point(688, 116)
            Me.btnSearchMediaAddSlide.Name = "btnSearchMediaAddSlide"
            Me.btnSearchMediaAddSlide.Size = New System.Drawing.Size(164, 34)
            Me.btnSearchMediaAddSlide.TabIndex = 93
            Me.btnSearchMediaAddSlide.Text = " ADD TO SLIDESHOW"
            Me.ToolTip1.SetToolTip(Me.btnSearchMediaAddSlide, "Add the selected slide to the end of the slide show list.")
            '
            'btnSearchMediaShow
            '
            Me.btnSearchMediaShow.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSearchMediaShow.Location = New System.Drawing.Point(703, 72)
            Me.btnSearchMediaShow.Name = "btnSearchMediaShow"
            Me.btnSearchMediaShow.Size = New System.Drawing.Size(134, 34)
            Me.btnSearchMediaShow.TabIndex = 92
            Me.btnSearchMediaShow.Text = "DISPLAY MEDIA"
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
            Me.tp5Things.Controls.Add(Me.btnListLeft)
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
            Me.tbCurrentThing.TabIndex = 109
            Me.tbCurrentThing.Text = "Substitutions"
            Me.tbCurrentThing.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'Label12
            '
            Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label12.Location = New System.Drawing.Point(68, 8)
            Me.Label12.Name = "Label12"
            Me.Label12.Size = New System.Drawing.Size(212, 24)
            Me.Label12.TabIndex = 96
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
            Me.clbThings.TabIndex = 99
            Me.clbThings.ThreeDCheckBoxes = True
            '
            'Label5
            '
            Me.Label5.BackColor = System.Drawing.Color.Transparent
            Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label5.Location = New System.Drawing.Point(14, 170)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(56, 20)
            Me.Label5.TabIndex = 101
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
            Me.grpThingsColor.TabIndex = 106
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
            Me.radioThingColorRight.TabIndex = 108
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
            Me.radioThingColorLeft.TabIndex = 107
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
            Me.btnClearThings.TabIndex = 104
            Me.btnClearThings.Text = "CLEAR LIST"
            '
            'btnRemoveThing
            '
            Me.btnRemoveThing.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnRemoveThing.Location = New System.Drawing.Point(410, 137)
            Me.btnRemoveThing.Name = "btnRemoveThing"
            Me.btnRemoveThing.Size = New System.Drawing.Size(96, 44)
            Me.btnRemoveThing.TabIndex = 103
            Me.btnRemoveThing.Text = "REMOVE SELECTION"
            '
            'btnListLeft
            '
            Me.btnListLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnListLeft.Location = New System.Drawing.Point(182, 300)
            Me.btnListLeft.Name = "btnListLeft"
            Me.btnListLeft.Size = New System.Drawing.Size(102, 44)
            Me.btnListLeft.TabIndex = 105
            Me.btnListLeft.Text = "SHOW LIST"
            '
            'btnShowThingLeft
            '
            Me.btnShowThingLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnShowThingLeft.Location = New System.Drawing.Point(661, 300)
            Me.btnShowThingLeft.Name = "btnShowThingLeft"
            Me.btnShowThingLeft.Size = New System.Drawing.Size(192, 44)
            Me.btnShowThingLeft.TabIndex = 111
            Me.btnShowThingLeft.Text = "SHOW SUBSTITUTIONS"
            '
            'tbSubstitutions
            '
            Me.tbSubstitutions.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.tbSubstitutions.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbSubstitutions.ForeColor = System.Drawing.Color.White
            Me.tbSubstitutions.Location = New System.Drawing.Point(595, 119)
            Me.tbSubstitutions.MaxLength = 2000
            Me.tbSubstitutions.Multiline = True
            Me.tbSubstitutions.Name = "tbSubstitutions"
            Me.tbSubstitutions.Size = New System.Drawing.Size(324, 169)
            Me.tbSubstitutions.TabIndex = 110
            Me.tbSubstitutions.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'btnAddThing
            '
            Me.btnAddThing.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnAddThing.Location = New System.Drawing.Point(410, 31)
            Me.btnAddThing.Name = "btnAddThing"
            Me.btnAddThing.Size = New System.Drawing.Size(96, 32)
            Me.btnAddThing.TabIndex = 98
            Me.btnAddThing.Text = "ADD!"
            '
            'tbNewThing
            '
            Me.tbNewThing.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbNewThing.Location = New System.Drawing.Point(68, 35)
            Me.tbNewThing.Name = "tbNewThing"
            Me.tbNewThing.Size = New System.Drawing.Size(327, 23)
            Me.tbNewThing.TabIndex = 97
            '
            'btnThingDown
            '
            Me.btnThingDown.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnThingDown.Location = New System.Drawing.Point(22, 193)
            Me.btnThingDown.Name = "btnThingDown"
            Me.btnThingDown.Size = New System.Drawing.Size(40, 41)
            Me.btnThingDown.TabIndex = 102
            Me.btnThingDown.Text = ""
            '
            'btnThingUp
            '
            Me.btnThingUp.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnThingUp.Location = New System.Drawing.Point(22, 126)
            Me.btnThingUp.Name = "btnThingUp"
            Me.btnThingUp.Size = New System.Drawing.Size(40, 41)
            Me.btnThingUp.TabIndex = 100
            Me.btnThingUp.Text = ""
            '
            'tpSlides
            '
            Me.tpSlides.Controls.Add(Me.lbSlideCandidates)
            Me.tpSlides.Controls.Add(Me.tvSlideFolders)
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
            'lbSlideCandidates
            '
            Me.lbSlideCandidates.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
            Me.lbSlideCandidates.FormattingEnabled = True
            Me.lbSlideCandidates.ItemHeight = 16
            Me.lbSlideCandidates.Location = New System.Drawing.Point(268, 152)
            Me.lbSlideCandidates.Name = "lbSlideCandidates"
            Me.lbSlideCandidates.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
            Me.lbSlideCandidates.Size = New System.Drawing.Size(250, 212)
            Me.lbSlideCandidates.Sorted = True
            Me.lbSlideCandidates.TabIndex = 113
            '
            'tvSlideFolders
            '
            Me.tvSlideFolders.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
            Me.tvSlideFolders.FullRowSelect = True
            Me.tvSlideFolders.HideSelection = False
            Me.tvSlideFolders.Location = New System.Drawing.Point(3, 3)
            Me.tvSlideFolders.Name = "tvSlideFolders"
            Me.tvSlideFolders.Size = New System.Drawing.Size(251, 361)
            Me.tvSlideFolders.TabIndex = 112
            '
            'btnWhammy
            '
            Me.btnWhammy.BackColor = System.Drawing.Color.Yellow
            Me.btnWhammy.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnWhammy.ForeColor = System.Drawing.Color.Blue
            Me.btnWhammy.Location = New System.Drawing.Point(547, 202)
            Me.btnWhammy.Name = "btnWhammy"
            Me.btnWhammy.Size = New System.Drawing.Size(95, 28)
            Me.btnWhammy.TabIndex = 121
            Me.btnWhammy.Text = "WHAMMY!"
            Me.ToolTip1.SetToolTip(Me.btnWhammy, "Launch slideshow as high-speed randomizer")
            Me.btnWhammy.UseVisualStyleBackColor = False
            '
            'btnPauseSlides
            '
            Me.btnPauseSlides.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnPauseSlides.ForeColor = System.Drawing.Color.Blue
            Me.btnPauseSlides.Location = New System.Drawing.Point(822, 333)
            Me.btnPauseSlides.Name = "btnPauseSlides"
            Me.btnPauseSlides.Size = New System.Drawing.Size(36, 33)
            Me.btnPauseSlides.TabIndex = 131
            Me.btnPauseSlides.Text = ""
            Me.ToolTip1.SetToolTip(Me.btnPauseSlides, "Pause")
            '
            'btnNextSlide
            '
            Me.btnNextSlide.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnNextSlide.Location = New System.Drawing.Point(906, 333)
            Me.btnNextSlide.Name = "btnNextSlide"
            Me.btnNextSlide.Size = New System.Drawing.Size(36, 33)
            Me.btnNextSlide.TabIndex = 133
            Me.btnNextSlide.Text = ""
            Me.ToolTip1.SetToolTip(Me.btnNextSlide, "Next slide")
            '
            'btnPrevSlide
            '
            Me.btnPrevSlide.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnPrevSlide.Location = New System.Drawing.Point(738, 333)
            Me.btnPrevSlide.Name = "btnPrevSlide"
            Me.btnPrevSlide.Size = New System.Drawing.Size(36, 33)
            Me.btnPrevSlide.TabIndex = 129
            Me.btnPrevSlide.Text = ""
            Me.ToolTip1.SetToolTip(Me.btnPrevSlide, "Back to previous slide")
            '
            'btnClearSlideList
            '
            Me.btnClearSlideList.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnClearSlideList.Location = New System.Drawing.Point(558, 165)
            Me.btnClearSlideList.Name = "btnClearSlideList"
            Me.btnClearSlideList.Size = New System.Drawing.Size(72, 28)
            Me.btnClearSlideList.TabIndex = 120
            Me.btnClearSlideList.Text = "Clear List"
            '
            'Label9
            '
            Me.Label9.BackColor = System.Drawing.Color.Transparent
            Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label9.Location = New System.Drawing.Point(535, 336)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(92, 24)
            Me.Label9.TabIndex = 126
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
            Me.nudDelay.TabIndex = 127
            Me.nudDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.ToolTip1.SetToolTip(Me.nudDelay, "Length of time that each slide will display (videos in slideshow will not adhere " &
        "to this setting)")
            Me.nudDelay.Value = New Decimal(New Integer() {15, 0, 0, 0})
            '
            'Label8
            '
            Me.Label8.BackColor = System.Drawing.Color.Transparent
            Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label8.Location = New System.Drawing.Point(653, 3)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(332, 16)
            Me.Label8.TabIndex = 124
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
            Me.Label7.TabIndex = 114
            Me.Label7.Text = "Preview"
            Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnSaveSlides
            '
            Me.btnSaveSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSaveSlides.Location = New System.Drawing.Point(547, 284)
            Me.btnSaveSlides.Name = "btnSaveSlides"
            Me.btnSaveSlides.Size = New System.Drawing.Size(96, 40)
            Me.btnSaveSlides.TabIndex = 123
            Me.btnSaveSlides.Text = "Save Slideshow"
            '
            'btnLoadSlides
            '
            Me.btnLoadSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnLoadSlides.Location = New System.Drawing.Point(547, 240)
            Me.btnLoadSlides.Name = "btnLoadSlides"
            Me.btnLoadSlides.Size = New System.Drawing.Size(96, 40)
            Me.btnLoadSlides.TabIndex = 122
            Me.btnLoadSlides.Text = "Load Slideshow"
            '
            'btnRemoveSlides
            '
            Me.btnRemoveSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnRemoveSlides.Location = New System.Drawing.Point(558, 131)
            Me.btnRemoveSlides.Name = "btnRemoveSlides"
            Me.btnRemoveSlides.Size = New System.Drawing.Size(72, 28)
            Me.btnRemoveSlides.TabIndex = 119
            Me.btnRemoveSlides.Text = "Remove"
            '
            'btnAddSlide
            '
            Me.btnAddSlide.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnAddSlide.Location = New System.Drawing.Point(458, 50)
            Me.btnAddSlide.Name = "btnAddSlide"
            Me.btnAddSlide.Size = New System.Drawing.Size(60, 52)
            Me.btnAddSlide.TabIndex = 115
            Me.btnAddSlide.Text = "Add To List"
            '
            'Label6
            '
            Me.Label6.BackColor = System.Drawing.Color.Transparent
            Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label6.Location = New System.Drawing.Point(562, 54)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(71, 27)
            Me.Label6.TabIndex = 117
            Me.Label6.Text = "ORDER"
            Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnSlideDown
            '
            Me.btnSlideDown.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnSlideDown.Location = New System.Drawing.Point(577, 80)
            Me.btnSlideDown.Name = "btnSlideDown"
            Me.btnSlideDown.Size = New System.Drawing.Size(40, 36)
            Me.btnSlideDown.TabIndex = 118
            '
            'btnSlideUp
            '
            Me.btnSlideUp.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnSlideUp.Location = New System.Drawing.Point(577, 19)
            Me.btnSlideUp.Name = "btnSlideUp"
            Me.btnSlideUp.Size = New System.Drawing.Size(40, 36)
            Me.btnSlideUp.TabIndex = 116
            Me.btnSlideUp.Text = ""
            '
            'btnStopSlides
            '
            Me.btnStopSlides.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnStopSlides.ForeColor = System.Drawing.Color.Red
            Me.btnStopSlides.Location = New System.Drawing.Point(864, 333)
            Me.btnStopSlides.Name = "btnStopSlides"
            Me.btnStopSlides.Size = New System.Drawing.Size(36, 33)
            Me.btnStopSlides.TabIndex = 132
            Me.btnStopSlides.Text = "" & Global.Microsoft.VisualBasic.ChrW(9)
            Me.ToolTip1.SetToolTip(Me.btnStopSlides, "Stop")
            '
            'btnLastSlide
            '
            Me.btnLastSlide.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnLastSlide.Location = New System.Drawing.Point(948, 333)
            Me.btnLastSlide.Name = "btnLastSlide"
            Me.btnLastSlide.Size = New System.Drawing.Size(36, 33)
            Me.btnLastSlide.TabIndex = 134
            Me.btnLastSlide.Text = ""
            Me.ToolTip1.SetToolTip(Me.btnLastSlide, "Jump to last slide")
            '
            'btnPlaySlides
            '
            Me.btnPlaySlides.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnPlaySlides.ForeColor = System.Drawing.Color.Green
            Me.btnPlaySlides.Location = New System.Drawing.Point(780, 333)
            Me.btnPlaySlides.Name = "btnPlaySlides"
            Me.btnPlaySlides.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.btnPlaySlides.Size = New System.Drawing.Size(36, 33)
            Me.btnPlaySlides.TabIndex = 130
            Me.btnPlaySlides.Text = ""
            Me.ToolTip1.SetToolTip(Me.btnPlaySlides, "Play")
            '
            'btnFirstSlide
            '
            Me.btnFirstSlide.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnFirstSlide.Location = New System.Drawing.Point(693, 333)
            Me.btnFirstSlide.Name = "btnFirstSlide"
            Me.btnFirstSlide.Size = New System.Drawing.Size(36, 33)
            Me.btnFirstSlide.TabIndex = 128
            Me.btnFirstSlide.Text = ""
            Me.ToolTip1.SetToolTip(Me.btnFirstSlide, "Jump back to first slide")
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
            Me.lbSlideList.TabIndex = 125
            '
            'picSlidePreview
            '
            Me.picSlidePreview.BackColor = System.Drawing.Color.Black
            Me.picSlidePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.picSlidePreview.Location = New System.Drawing.Point(268, 20)
            Me.picSlidePreview.Name = "picSlidePreview"
            Me.picSlidePreview.Size = New System.Drawing.Size(184, 121)
            Me.picSlidePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
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
            Me.btnClearHB.TabIndex = 137
            Me.btnClearHB.Text = "CLEAR ALL"
            '
            'lblHBinstructions
            '
            Me.lblHBinstructions.BackColor = System.Drawing.Color.LemonChiffon
            Me.lblHBinstructions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblHBinstructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblHBinstructions.ForeColor = System.Drawing.Color.Black
            Me.lblHBinstructions.Location = New System.Drawing.Point(6, 60)
            Me.lblHBinstructions.Name = "lblHBinstructions"
            Me.lblHBinstructions.Size = New System.Drawing.Size(168, 129)
            Me.lblHBinstructions.TabIndex = 136
            Me.lblHBinstructions.Text = "Shortcuts that you can define for quick access to stored images and videos. Selec" &
    "t a name && image or video for each button. Save lists of buttons for specific u" &
    "ses."
            Me.lblHBinstructions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnSaveHB
            '
            Me.btnSaveHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSaveHB.Location = New System.Drawing.Point(27, 318)
            Me.btnSaveHB.Name = "btnSaveHB"
            Me.btnSaveHB.Size = New System.Drawing.Size(124, 36)
            Me.btnSaveHB.TabIndex = 139
            Me.btnSaveHB.Text = "SAVE LIST"
            '
            'btnLoadHB
            '
            Me.btnLoadHB.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnLoadHB.Location = New System.Drawing.Point(27, 272)
            Me.btnLoadHB.Name = "btnLoadHB"
            Me.btnLoadHB.Size = New System.Drawing.Size(124, 36)
            Me.btnLoadHB.TabIndex = 138
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
            Me.gbHB.TabIndex = 140
            Me.gbHB.TabStop = False
            '
            'tbHBfile10
            '
            Me.tbHBfile10.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile10.Location = New System.Drawing.Point(177, 332)
            Me.tbHBfile10.Name = "tbHBfile10"
            Me.tbHBfile10.ReadOnly = True
            Me.tbHBfile10.Size = New System.Drawing.Size(564, 22)
            Me.tbHBfile10.TabIndex = 181
            Me.tbHBfile10.TabStop = False
            '
            'tbHBfile9
            '
            Me.tbHBfile9.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile9.Location = New System.Drawing.Point(177, 299)
            Me.tbHBfile9.Name = "tbHBfile9"
            Me.tbHBfile9.ReadOnly = True
            Me.tbHBfile9.Size = New System.Drawing.Size(564, 22)
            Me.tbHBfile9.TabIndex = 177
            Me.tbHBfile9.TabStop = False
            '
            'tbHBfile8
            '
            Me.tbHBfile8.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile8.Location = New System.Drawing.Point(177, 266)
            Me.tbHBfile8.Name = "tbHBfile8"
            Me.tbHBfile8.ReadOnly = True
            Me.tbHBfile8.Size = New System.Drawing.Size(564, 22)
            Me.tbHBfile8.TabIndex = 173
            Me.tbHBfile8.TabStop = False
            '
            'tbHBfile7
            '
            Me.tbHBfile7.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile7.Location = New System.Drawing.Point(177, 233)
            Me.tbHBfile7.Name = "tbHBfile7"
            Me.tbHBfile7.ReadOnly = True
            Me.tbHBfile7.Size = New System.Drawing.Size(564, 22)
            Me.tbHBfile7.TabIndex = 169
            Me.tbHBfile7.TabStop = False
            '
            'tbHBfile6
            '
            Me.tbHBfile6.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile6.Location = New System.Drawing.Point(177, 200)
            Me.tbHBfile6.Name = "tbHBfile6"
            Me.tbHBfile6.ReadOnly = True
            Me.tbHBfile6.Size = New System.Drawing.Size(564, 22)
            Me.tbHBfile6.TabIndex = 165
            Me.tbHBfile6.TabStop = False
            '
            'tbHBfile5
            '
            Me.tbHBfile5.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile5.Location = New System.Drawing.Point(177, 167)
            Me.tbHBfile5.Name = "tbHBfile5"
            Me.tbHBfile5.ReadOnly = True
            Me.tbHBfile5.Size = New System.Drawing.Size(564, 22)
            Me.tbHBfile5.TabIndex = 161
            Me.tbHBfile5.TabStop = False
            '
            'tbHBfile4
            '
            Me.tbHBfile4.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile4.Location = New System.Drawing.Point(177, 134)
            Me.tbHBfile4.Name = "tbHBfile4"
            Me.tbHBfile4.ReadOnly = True
            Me.tbHBfile4.Size = New System.Drawing.Size(564, 22)
            Me.tbHBfile4.TabIndex = 157
            Me.tbHBfile4.TabStop = False
            '
            'tbHBfile3
            '
            Me.tbHBfile3.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile3.Location = New System.Drawing.Point(177, 101)
            Me.tbHBfile3.Name = "tbHBfile3"
            Me.tbHBfile3.ReadOnly = True
            Me.tbHBfile3.Size = New System.Drawing.Size(564, 22)
            Me.tbHBfile3.TabIndex = 153
            Me.tbHBfile3.TabStop = False
            '
            'tbHBfile2
            '
            Me.tbHBfile2.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile2.Location = New System.Drawing.Point(177, 68)
            Me.tbHBfile2.Name = "tbHBfile2"
            Me.tbHBfile2.ReadOnly = True
            Me.tbHBfile2.Size = New System.Drawing.Size(564, 22)
            Me.tbHBfile2.TabIndex = 149
            Me.tbHBfile2.TabStop = False
            '
            'tbHBfile1
            '
            Me.tbHBfile1.BackColor = System.Drawing.SystemColors.Control
            Me.tbHBfile1.Location = New System.Drawing.Point(177, 35)
            Me.tbHBfile1.Name = "tbHBfile1"
            Me.tbHBfile1.ReadOnly = True
            Me.tbHBfile1.Size = New System.Drawing.Size(564, 22)
            Me.tbHBfile1.TabIndex = 145
            Me.tbHBfile1.TabStop = False
            '
            'Label14
            '
            Me.Label14.BackColor = System.Drawing.Color.Transparent
            Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label14.Location = New System.Drawing.Point(180, 8)
            Me.Label14.Name = "Label14"
            Me.Label14.Size = New System.Drawing.Size(328, 20)
            Me.Label14.TabIndex = 142
            Me.Label14.Text = "Image / Video File"
            Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'Label13
            '
            Me.Label13.BackColor = System.Drawing.Color.Transparent
            Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label13.Location = New System.Drawing.Point(68, 8)
            Me.Label13.Name = "Label13"
            Me.Label13.Size = New System.Drawing.Size(96, 20)
            Me.Label13.TabIndex = 141
            Me.Label13.Text = "Title"
            Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'Label30
            '
            Me.Label30.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label30.Location = New System.Drawing.Point(4, 335)
            Me.Label30.Name = "Label30"
            Me.Label30.Size = New System.Drawing.Size(64, 20)
            Me.Label30.TabIndex = 179
            Me.Label30.Text = "Button 10"
            '
            'Label31
            '
            Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label31.Location = New System.Drawing.Point(4, 302)
            Me.Label31.Name = "Label31"
            Me.Label31.Size = New System.Drawing.Size(64, 20)
            Me.Label31.TabIndex = 175
            Me.Label31.Text = "Button 9"
            '
            'Label28
            '
            Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label28.Location = New System.Drawing.Point(4, 269)
            Me.Label28.Name = "Label28"
            Me.Label28.Size = New System.Drawing.Size(64, 20)
            Me.Label28.TabIndex = 171
            Me.Label28.Text = "Button 8"
            '
            'Label29
            '
            Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label29.Location = New System.Drawing.Point(4, 236)
            Me.Label29.Name = "Label29"
            Me.Label29.Size = New System.Drawing.Size(64, 20)
            Me.Label29.TabIndex = 167
            Me.Label29.Text = "Button 7"
            '
            'Label26
            '
            Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label26.Location = New System.Drawing.Point(4, 203)
            Me.Label26.Name = "Label26"
            Me.Label26.Size = New System.Drawing.Size(64, 20)
            Me.Label26.TabIndex = 163
            Me.Label26.Text = "Button 6"
            '
            'Label27
            '
            Me.Label27.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label27.Location = New System.Drawing.Point(4, 170)
            Me.Label27.Name = "Label27"
            Me.Label27.Size = New System.Drawing.Size(64, 20)
            Me.Label27.TabIndex = 159
            Me.Label27.Text = "Button 5"
            '
            'Label24
            '
            Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label24.Location = New System.Drawing.Point(4, 137)
            Me.Label24.Name = "Label24"
            Me.Label24.Size = New System.Drawing.Size(64, 20)
            Me.Label24.TabIndex = 155
            Me.Label24.Text = "Button 4"
            '
            'Label25
            '
            Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label25.Location = New System.Drawing.Point(4, 104)
            Me.Label25.Name = "Label25"
            Me.Label25.Size = New System.Drawing.Size(64, 20)
            Me.Label25.TabIndex = 151
            Me.Label25.Text = "Button 3"
            '
            'Label23
            '
            Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label23.Location = New System.Drawing.Point(4, 71)
            Me.Label23.Name = "Label23"
            Me.Label23.Size = New System.Drawing.Size(64, 20)
            Me.Label23.TabIndex = 147
            Me.Label23.Text = "Button 2"
            '
            'Label22
            '
            Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label22.Location = New System.Drawing.Point(4, 38)
            Me.Label22.Name = "Label22"
            Me.Label22.Size = New System.Drawing.Size(64, 20)
            Me.Label22.TabIndex = 143
            Me.Label22.Text = "Button 1"
            '
            'btnHBSelect10
            '
            Me.btnHBSelect10.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnHBSelect10.Location = New System.Drawing.Point(748, 330)
            Me.btnHBSelect10.Name = "btnHBSelect10"
            Me.btnHBSelect10.Size = New System.Drawing.Size(44, 26)
            Me.btnHBSelect10.TabIndex = 182
            Me.btnHBSelect10.Tag = "9"
            Me.btnHBSelect10.Text = ""
            '
            'btnHBSelect9
            '
            Me.btnHBSelect9.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnHBSelect9.Location = New System.Drawing.Point(748, 297)
            Me.btnHBSelect9.Name = "btnHBSelect9"
            Me.btnHBSelect9.Size = New System.Drawing.Size(44, 26)
            Me.btnHBSelect9.TabIndex = 178
            Me.btnHBSelect9.Tag = "8"
            Me.btnHBSelect9.Text = ""
            '
            'btnHBSelect8
            '
            Me.btnHBSelect8.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnHBSelect8.Location = New System.Drawing.Point(748, 264)
            Me.btnHBSelect8.Name = "btnHBSelect8"
            Me.btnHBSelect8.Size = New System.Drawing.Size(44, 26)
            Me.btnHBSelect8.TabIndex = 174
            Me.btnHBSelect8.Tag = "7"
            Me.btnHBSelect8.Text = ""
            '
            'btnHBSelect7
            '
            Me.btnHBSelect7.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnHBSelect7.Location = New System.Drawing.Point(748, 231)
            Me.btnHBSelect7.Name = "btnHBSelect7"
            Me.btnHBSelect7.Size = New System.Drawing.Size(44, 26)
            Me.btnHBSelect7.TabIndex = 170
            Me.btnHBSelect7.Tag = "6"
            Me.btnHBSelect7.Text = ""
            '
            'btnHBSelect6
            '
            Me.btnHBSelect6.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnHBSelect6.Location = New System.Drawing.Point(748, 198)
            Me.btnHBSelect6.Name = "btnHBSelect6"
            Me.btnHBSelect6.Size = New System.Drawing.Size(44, 26)
            Me.btnHBSelect6.TabIndex = 166
            Me.btnHBSelect6.Tag = "5"
            Me.btnHBSelect6.Text = ""
            '
            'btnHBSelect5
            '
            Me.btnHBSelect5.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnHBSelect5.Location = New System.Drawing.Point(748, 165)
            Me.btnHBSelect5.Name = "btnHBSelect5"
            Me.btnHBSelect5.Size = New System.Drawing.Size(44, 26)
            Me.btnHBSelect5.TabIndex = 162
            Me.btnHBSelect5.Tag = "4"
            Me.btnHBSelect5.Text = ""
            '
            'btnHBSelect4
            '
            Me.btnHBSelect4.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnHBSelect4.Location = New System.Drawing.Point(748, 132)
            Me.btnHBSelect4.Name = "btnHBSelect4"
            Me.btnHBSelect4.Size = New System.Drawing.Size(44, 26)
            Me.btnHBSelect4.TabIndex = 158
            Me.btnHBSelect4.Tag = "3"
            Me.btnHBSelect4.Text = ""
            '
            'btnHBSelect3
            '
            Me.btnHBSelect3.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnHBSelect3.Location = New System.Drawing.Point(748, 99)
            Me.btnHBSelect3.Name = "btnHBSelect3"
            Me.btnHBSelect3.Size = New System.Drawing.Size(44, 26)
            Me.btnHBSelect3.TabIndex = 154
            Me.btnHBSelect3.Tag = "2"
            Me.btnHBSelect3.Text = ""
            '
            'btnHBSelect2
            '
            Me.btnHBSelect2.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnHBSelect2.Location = New System.Drawing.Point(748, 66)
            Me.btnHBSelect2.Name = "btnHBSelect2"
            Me.btnHBSelect2.Size = New System.Drawing.Size(44, 26)
            Me.btnHBSelect2.TabIndex = 150
            Me.btnHBSelect2.Tag = "1"
            Me.btnHBSelect2.Text = ""
            '
            'btnHBSelect1
            '
            Me.btnHBSelect1.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnHBSelect1.Location = New System.Drawing.Point(748, 33)
            Me.btnHBSelect1.Name = "btnHBSelect1"
            Me.btnHBSelect1.Size = New System.Drawing.Size(44, 26)
            Me.btnHBSelect1.TabIndex = 146
            Me.btnHBSelect1.Tag = "0"
            Me.btnHBSelect1.Text = ""
            '
            'tbHBtext10
            '
            Me.tbHBtext10.Location = New System.Drawing.Point(76, 332)
            Me.tbHBtext10.MaxLength = 11
            Me.tbHBtext10.Name = "tbHBtext10"
            Me.tbHBtext10.Size = New System.Drawing.Size(95, 22)
            Me.tbHBtext10.TabIndex = 180
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
            Me.tbHBtext9.TabIndex = 176
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
            Me.tbHBtext8.TabIndex = 172
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
            Me.tbHBtext7.TabIndex = 168
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
            Me.tbHBtext6.TabIndex = 164
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
            Me.tbHBtext5.TabIndex = 160
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
            Me.tbHBtext4.TabIndex = 156
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
            Me.tbHBtext3.TabIndex = 152
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
            Me.tbHBtext2.TabIndex = 148
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
            Me.tbHBtext1.TabIndex = 144
            Me.tbHBtext1.Tag = "0"
            Me.tbHBtext1.Text = "Hot 1"
            Me.tbHBtext1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'cbHBActive
            '
            Me.cbHBActive.Anchor = System.Windows.Forms.AnchorStyles.Left
            Me.cbHBActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.cbHBActive.Checked = True
            Me.cbHBActive.CheckState = System.Windows.Forms.CheckState.Checked
            Me.cbHBActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbHBActive.Location = New System.Drawing.Point(15, 8)
            Me.cbHBActive.Name = "cbHBActive"
            Me.cbHBActive.Size = New System.Drawing.Size(147, 35)
            Me.cbHBActive.TabIndex = 135
            Me.cbHBActive.Text = "Hot Buttons?"
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
            Me.cbShadowsEnabled.TabIndex = 204
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
            Me.Label43.TabIndex = 220
            Me.Label43.Text = "Default Countdown Timer Values:"
            Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Label38
            '
            Me.Label38.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label38.Location = New System.Drawing.Point(924, 236)
            Me.Label38.Name = "Label38"
            Me.Label38.Size = New System.Drawing.Size(40, 16)
            Me.Label38.TabIndex = 223
            Me.Label38.Text = "SS"
            Me.Label38.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'Label39
            '
            Me.Label39.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label39.Location = New System.Drawing.Point(880, 236)
            Me.Label39.Name = "Label39"
            Me.Label39.Size = New System.Drawing.Size(40, 16)
            Me.Label39.TabIndex = 222
            Me.Label39.Text = "MM"
            Me.Label39.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'Label40
            '
            Me.Label40.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label40.Location = New System.Drawing.Point(836, 236)
            Me.Label40.Name = "Label40"
            Me.Label40.Size = New System.Drawing.Size(40, 16)
            Me.Label40.TabIndex = 221
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
            Me.nudDefaultCountdownSeconds.TabIndex = 226
            Me.nudDefaultCountdownSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'nudDefaultCountdownMinutes
            '
            Me.nudDefaultCountdownMinutes.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.nudDefaultCountdownMinutes.Location = New System.Drawing.Point(880, 260)
            Me.nudDefaultCountdownMinutes.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
            Me.nudDefaultCountdownMinutes.Name = "nudDefaultCountdownMinutes"
            Me.nudDefaultCountdownMinutes.Size = New System.Drawing.Size(44, 23)
            Me.nudDefaultCountdownMinutes.TabIndex = 225
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
            Me.nudDefaultCountdownHours.TabIndex = 224
            Me.nudDefaultCountdownHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'btnDefaultPrefs
            '
            Me.btnDefaultPrefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnDefaultPrefs.Location = New System.Drawing.Point(11, 330)
            Me.btnDefaultPrefs.Name = "btnDefaultPrefs"
            Me.btnDefaultPrefs.Size = New System.Drawing.Size(190, 32)
            Me.btnDefaultPrefs.TabIndex = 227
            Me.btnDefaultPrefs.Text = "FACTORY DEFAULTS"
            '
            'tbDefaultImageDir
            '
            Me.tbDefaultImageDir.BackColor = System.Drawing.SystemColors.Control
            Me.tbDefaultImageDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbDefaultImageDir.Location = New System.Drawing.Point(27, 170)
            Me.tbDefaultImageDir.Name = "tbDefaultImageDir"
            Me.tbDefaultImageDir.ReadOnly = True
            Me.tbDefaultImageDir.Size = New System.Drawing.Size(382, 23)
            Me.tbDefaultImageDir.TabIndex = 206
            Me.tbDefaultImageDir.TabStop = False
            Me.tbDefaultImageDir.Text = "C:\JANIS"
            '
            'btnChooseDefaultImageDir
            '
            Me.btnChooseDefaultImageDir.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnChooseDefaultImageDir.Location = New System.Drawing.Point(415, 168)
            Me.btnChooseDefaultImageDir.Name = "btnChooseDefaultImageDir"
            Me.btnChooseDefaultImageDir.Size = New System.Drawing.Size(56, 26)
            Me.btnChooseDefaultImageDir.TabIndex = 207
            Me.btnChooseDefaultImageDir.Tag = "0"
            Me.btnChooseDefaultImageDir.Text = ""
            '
            'Label18
            '
            Me.Label18.BackColor = System.Drawing.Color.Transparent
            Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label18.Location = New System.Drawing.Point(8, 145)
            Me.Label18.Name = "Label18"
            Me.Label18.Size = New System.Drawing.Size(240, 22)
            Me.Label18.TabIndex = 205
            Me.Label18.Text = "Default media search directory:"
            Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'tbDefaultFontSize
            '
            Me.tbDefaultFontSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbDefaultFontSize.Location = New System.Drawing.Point(278, 76)
            Me.tbDefaultFontSize.MaxLength = 3
            Me.tbDefaultFontSize.Name = "tbDefaultFontSize"
            Me.tbDefaultFontSize.Size = New System.Drawing.Size(52, 24)
            Me.tbDefaultFontSize.TabIndex = 203
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
            Me.Label17.TabIndex = 202
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
            Me.GroupBox2.TabIndex = 183
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
            Me.lblDefaultColorRight.TabIndex = 193
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
            Me.lblDefaultColorLeft.TabIndex = 192
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
            Me.grpDefaultColorsRight.TabIndex = 194
            Me.grpDefaultColorsRight.TabStop = False
            '
            'btnChooseDefaultTextColorRight
            '
            Me.btnChooseDefaultTextColorRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnChooseDefaultTextColorRight.Location = New System.Drawing.Point(156, 13)
            Me.btnChooseDefaultTextColorRight.Name = "btnChooseDefaultTextColorRight"
            Me.btnChooseDefaultTextColorRight.Size = New System.Drawing.Size(78, 26)
            Me.btnChooseDefaultTextColorRight.TabIndex = 201
            Me.btnChooseDefaultTextColorRight.Text = "Choose..."
            '
            'pnlDefaultTextColorRight6
            '
            Me.pnlDefaultTextColorRight6.BackColor = System.Drawing.Color.Black
            Me.pnlDefaultTextColorRight6.Location = New System.Drawing.Point(129, 18)
            Me.pnlDefaultTextColorRight6.Name = "pnlDefaultTextColorRight6"
            Me.pnlDefaultTextColorRight6.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorRight6.TabIndex = 200
            Me.pnlDefaultTextColorRight6.TabStop = True
            '
            'pnlDefaultTextColorRight5
            '
            Me.pnlDefaultTextColorRight5.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlDefaultTextColorRight5.Location = New System.Drawing.Point(105, 18)
            Me.pnlDefaultTextColorRight5.Name = "pnlDefaultTextColorRight5"
            Me.pnlDefaultTextColorRight5.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorRight5.TabIndex = 199
            Me.pnlDefaultTextColorRight5.TabStop = True
            '
            'pnlDefaultTextColorRight4
            '
            Me.pnlDefaultTextColorRight4.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.pnlDefaultTextColorRight4.Location = New System.Drawing.Point(81, 18)
            Me.pnlDefaultTextColorRight4.Name = "pnlDefaultTextColorRight4"
            Me.pnlDefaultTextColorRight4.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorRight4.TabIndex = 198
            Me.pnlDefaultTextColorRight4.TabStop = True
            '
            'pnlDefaultTextColorRight3
            '
            Me.pnlDefaultTextColorRight3.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlDefaultTextColorRight3.Location = New System.Drawing.Point(57, 18)
            Me.pnlDefaultTextColorRight3.Name = "pnlDefaultTextColorRight3"
            Me.pnlDefaultTextColorRight3.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorRight3.TabIndex = 197
            Me.pnlDefaultTextColorRight3.TabStop = True
            '
            'pnlDefaultTextColorRight2
            '
            Me.pnlDefaultTextColorRight2.BackColor = System.Drawing.Color.Maroon
            Me.pnlDefaultTextColorRight2.Location = New System.Drawing.Point(33, 18)
            Me.pnlDefaultTextColorRight2.Name = "pnlDefaultTextColorRight2"
            Me.pnlDefaultTextColorRight2.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorRight2.TabIndex = 196
            Me.pnlDefaultTextColorRight2.TabStop = True
            '
            'pnlDefaultTextColorRight1
            '
            Me.pnlDefaultTextColorRight1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.pnlDefaultTextColorRight1.Location = New System.Drawing.Point(9, 18)
            Me.pnlDefaultTextColorRight1.Name = "pnlDefaultTextColorRight1"
            Me.pnlDefaultTextColorRight1.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorRight1.TabIndex = 195
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
            Me.grpDefaultColorsLeft.TabIndex = 184
            Me.grpDefaultColorsLeft.TabStop = False
            '
            'btnChooseDefaultTextColorLeft
            '
            Me.btnChooseDefaultTextColorLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnChooseDefaultTextColorLeft.Location = New System.Drawing.Point(156, 13)
            Me.btnChooseDefaultTextColorLeft.Name = "btnChooseDefaultTextColorLeft"
            Me.btnChooseDefaultTextColorLeft.Size = New System.Drawing.Size(78, 26)
            Me.btnChooseDefaultTextColorLeft.TabIndex = 191
            Me.btnChooseDefaultTextColorLeft.Text = "Choose..."
            '
            'pnlDefaultTextColorLeft6
            '
            Me.pnlDefaultTextColorLeft6.BackColor = System.Drawing.Color.Black
            Me.pnlDefaultTextColorLeft6.Location = New System.Drawing.Point(129, 18)
            Me.pnlDefaultTextColorLeft6.Name = "pnlDefaultTextColorLeft6"
            Me.pnlDefaultTextColorLeft6.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorLeft6.TabIndex = 190
            Me.pnlDefaultTextColorLeft6.TabStop = True
            '
            'pnlDefaultTextColorLeft5
            '
            Me.pnlDefaultTextColorLeft5.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlDefaultTextColorLeft5.Location = New System.Drawing.Point(105, 18)
            Me.pnlDefaultTextColorLeft5.Name = "pnlDefaultTextColorLeft5"
            Me.pnlDefaultTextColorLeft5.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorLeft5.TabIndex = 189
            Me.pnlDefaultTextColorLeft5.TabStop = True
            '
            'pnlDefaultTextColorLeft4
            '
            Me.pnlDefaultTextColorLeft4.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.pnlDefaultTextColorLeft4.Location = New System.Drawing.Point(81, 18)
            Me.pnlDefaultTextColorLeft4.Name = "pnlDefaultTextColorLeft4"
            Me.pnlDefaultTextColorLeft4.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorLeft4.TabIndex = 188
            Me.pnlDefaultTextColorLeft4.TabStop = True
            '
            'pnlDefaultTextColorLeft3
            '
            Me.pnlDefaultTextColorLeft3.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.pnlDefaultTextColorLeft3.Location = New System.Drawing.Point(57, 18)
            Me.pnlDefaultTextColorLeft3.Name = "pnlDefaultTextColorLeft3"
            Me.pnlDefaultTextColorLeft3.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorLeft3.TabIndex = 187
            Me.pnlDefaultTextColorLeft3.TabStop = True
            '
            'pnlDefaultTextColorLeft2
            '
            Me.pnlDefaultTextColorLeft2.BackColor = System.Drawing.Color.Maroon
            Me.pnlDefaultTextColorLeft2.Location = New System.Drawing.Point(33, 18)
            Me.pnlDefaultTextColorLeft2.Name = "pnlDefaultTextColorLeft2"
            Me.pnlDefaultTextColorLeft2.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorLeft2.TabIndex = 186
            Me.pnlDefaultTextColorLeft2.TabStop = True
            '
            'pnlDefaultTextColorLeft1
            '
            Me.pnlDefaultTextColorLeft1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
            Me.pnlDefaultTextColorLeft1.Location = New System.Drawing.Point(9, 18)
            Me.pnlDefaultTextColorLeft1.Name = "pnlDefaultTextColorLeft1"
            Me.pnlDefaultTextColorLeft1.Size = New System.Drawing.Size(16, 16)
            Me.pnlDefaultTextColorLeft1.TabIndex = 185
            Me.pnlDefaultTextColorLeft1.TabStop = True
            '
            'btnSavePrefs
            '
            Me.btnSavePrefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSavePrefs.Location = New System.Drawing.Point(380, 330)
            Me.btnSavePrefs.Name = "btnSavePrefs"
            Me.btnSavePrefs.Size = New System.Drawing.Size(190, 32)
            Me.btnSavePrefs.TabIndex = 228
            Me.btnSavePrefs.Text = "SAVE PREFS"
            '
            'btnRevertPrefs
            '
            Me.btnRevertPrefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnRevertPrefs.Location = New System.Drawing.Point(786, 330)
            Me.btnRevertPrefs.Name = "btnRevertPrefs"
            Me.btnRevertPrefs.Size = New System.Drawing.Size(190, 32)
            Me.btnRevertPrefs.TabIndex = 229
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
            Me.cbPlaySlidesAtStart.TabIndex = 219
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
            Me.Label16.TabIndex = 214
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
            Me.nudDefaultSlideDelay.TabIndex = 215
            Me.nudDefaultSlideDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.nudDefaultSlideDelay.Value = New Decimal(New Integer() {15, 0, 0, 0})
            '
            'tbDefaultSlideShow
            '
            Me.tbDefaultSlideShow.BackColor = System.Drawing.SystemColors.ControlDark
            Me.tbDefaultSlideShow.Enabled = False
            Me.tbDefaultSlideShow.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbDefaultSlideShow.Location = New System.Drawing.Point(536, 145)
            Me.tbDefaultSlideShow.Name = "tbDefaultSlideShow"
            Me.tbDefaultSlideShow.ReadOnly = True
            Me.tbDefaultSlideShow.Size = New System.Drawing.Size(376, 23)
            Me.tbDefaultSlideShow.TabIndex = 217
            Me.tbDefaultSlideShow.TabStop = False
            '
            'btnChooseDefaultSlideShow
            '
            Me.btnChooseDefaultSlideShow.Enabled = False
            Me.btnChooseDefaultSlideShow.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnChooseDefaultSlideShow.Location = New System.Drawing.Point(918, 143)
            Me.btnChooseDefaultSlideShow.Name = "btnChooseDefaultSlideShow"
            Me.btnChooseDefaultSlideShow.Size = New System.Drawing.Size(56, 26)
            Me.btnChooseDefaultSlideShow.TabIndex = 218
            Me.btnChooseDefaultSlideShow.Tag = "0"
            Me.btnChooseDefaultSlideShow.Text = ""
            '
            'tbDefaultHBFile
            '
            Me.tbDefaultHBFile.BackColor = System.Drawing.SystemColors.ControlDark
            Me.tbDefaultHBFile.Enabled = False
            Me.tbDefaultHBFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbDefaultHBFile.Location = New System.Drawing.Point(25, 284)
            Me.tbDefaultHBFile.Name = "tbDefaultHBFile"
            Me.tbDefaultHBFile.ReadOnly = True
            Me.tbDefaultHBFile.Size = New System.Drawing.Size(382, 23)
            Me.tbDefaultHBFile.TabIndex = 212
            Me.tbDefaultHBFile.TabStop = False
            '
            'btnChooseDefaultHB
            '
            Me.btnChooseDefaultHB.Enabled = False
            Me.btnChooseDefaultHB.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnChooseDefaultHB.Location = New System.Drawing.Point(415, 282)
            Me.btnChooseDefaultHB.Name = "btnChooseDefaultHB"
            Me.btnChooseDefaultHB.Size = New System.Drawing.Size(56, 26)
            Me.btnChooseDefaultHB.TabIndex = 213
            Me.btnChooseDefaultHB.Tag = "0"
            Me.btnChooseDefaultHB.Text = ""
            '
            'tbDefaultImageFile
            '
            Me.tbDefaultImageFile.BackColor = System.Drawing.SystemColors.ControlDark
            Me.tbDefaultImageFile.Enabled = False
            Me.tbDefaultImageFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbDefaultImageFile.Location = New System.Drawing.Point(27, 227)
            Me.tbDefaultImageFile.Name = "tbDefaultImageFile"
            Me.tbDefaultImageFile.ReadOnly = True
            Me.tbDefaultImageFile.Size = New System.Drawing.Size(382, 23)
            Me.tbDefaultImageFile.TabIndex = 209
            Me.tbDefaultImageFile.TabStop = False
            '
            'btnChooseDefaultImage
            '
            Me.btnChooseDefaultImage.Enabled = False
            Me.btnChooseDefaultImage.Font = New System.Drawing.Font("Segoe MDL2 Assets", 14.25!, System.Drawing.FontStyle.Bold)
            Me.btnChooseDefaultImage.Location = New System.Drawing.Point(415, 225)
            Me.btnChooseDefaultImage.Name = "btnChooseDefaultImage"
            Me.btnChooseDefaultImage.Size = New System.Drawing.Size(56, 26)
            Me.btnChooseDefaultImage.TabIndex = 210
            Me.btnChooseDefaultImage.Tag = "0"
            Me.btnChooseDefaultImage.Text = ""
            '
            'cbDisplayDefaultImage
            '
            Me.cbDisplayDefaultImage.BackColor = System.Drawing.Color.Transparent
            Me.cbDisplayDefaultImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbDisplayDefaultImage.Location = New System.Drawing.Point(8, 199)
            Me.cbDisplayDefaultImage.Name = "cbDisplayDefaultImage"
            Me.cbDisplayDefaultImage.Size = New System.Drawing.Size(240, 22)
            Me.cbDisplayDefaultImage.TabIndex = 208
            Me.cbDisplayDefaultImage.Text = "Display media at program start:"
            Me.cbDisplayDefaultImage.UseVisualStyleBackColor = False
            '
            'cbLoadDefaultSlides
            '
            Me.cbLoadDefaultSlides.BackColor = System.Drawing.Color.Transparent
            Me.cbLoadDefaultSlides.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbLoadDefaultSlides.Location = New System.Drawing.Point(519, 117)
            Me.cbLoadDefaultSlides.Name = "cbLoadDefaultSlides"
            Me.cbLoadDefaultSlides.Size = New System.Drawing.Size(242, 22)
            Me.cbLoadDefaultSlides.TabIndex = 216
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
            Me.cbLoadDefaultHB.TabIndex = 211
            Me.cbLoadDefaultHB.Text = "Load a HotButtons file at start:"
            Me.cbLoadDefaultHB.UseVisualStyleBackColor = False
            '
            'tpAbout
            '
            Me.tpAbout.BackColor = System.Drawing.SystemColors.Window
            Me.tpAbout.Controls.Add(Me.tbAboutHeader)
            Me.tpAbout.Controls.Add(Me.tbAboutBody)
            Me.tpAbout.Location = New System.Drawing.Point(4, 28)
            Me.tpAbout.Name = "tpAbout"
            Me.tpAbout.Size = New System.Drawing.Size(988, 372)
            Me.tpAbout.TabIndex = 3
            Me.tpAbout.Text = "About JANIS"
            '
            'tbAboutHeader
            '
            Me.tbAboutHeader.BackColor = System.Drawing.SystemColors.Window
            Me.tbAboutHeader.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.tbAboutHeader.Font = New System.Drawing.Font("Arial", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbAboutHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.tbAboutHeader.Location = New System.Drawing.Point(8, 20)
            Me.tbAboutHeader.Multiline = True
            Me.tbAboutHeader.Name = "tbAboutHeader"
            Me.tbAboutHeader.ReadOnly = True
            Me.tbAboutHeader.Size = New System.Drawing.Size(972, 105)
            Me.tbAboutHeader.TabIndex = 230
            Me.tbAboutHeader.TabStop = False
            Me.tbAboutHeader.Text = "JANIS v5.0.0.1 ALPHA" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Released March 30, 2026" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "by Bill Cernansky (boctorbill@gmai" &
    "l.com)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "© 2004-2026 Easy Being Productions"
            Me.tbAboutHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'tbAboutBody
            '
            Me.tbAboutBody.BackColor = System.Drawing.SystemColors.Window
            Me.tbAboutBody.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.tbAboutBody.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbAboutBody.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.tbAboutBody.Location = New System.Drawing.Point(8, 132)
            Me.tbAboutBody.Multiline = True
            Me.tbAboutBody.Name = "tbAboutBody"
            Me.tbAboutBody.ReadOnly = True
            Me.tbAboutBody.Size = New System.Drawing.Size(972, 232)
            Me.tbAboutBody.TabIndex = 232
            Me.tbAboutBody.TabStop = False
            Me.tbAboutBody.Text = resources.GetString("tbAboutBody.Text")
            Me.tbAboutBody.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'SlideTimer
            '
            Me.SlideTimer.Interval = 8000
            '
            'pnlRemoteViewer
            '
            Me.pnlRemoteViewer.BackColor = System.Drawing.Color.Black
            Me.pnlRemoteViewer.Controls.Add(Me.cbMuteVideo)
            Me.pnlRemoteViewer.Controls.Add(Me.lblRemoteStatus)
            Me.pnlRemoteViewer.Controls.Add(Me.picRemoteViewer)
            Me.pnlRemoteViewer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.pnlRemoteViewer.Location = New System.Drawing.Point(316, 56)
            Me.pnlRemoteViewer.Name = "pnlRemoteViewer"
            Me.pnlRemoteViewer.Size = New System.Drawing.Size(364, 204)
            Me.pnlRemoteViewer.TabIndex = 14
            '
            'cbMuteVideo
            '
            Me.cbMuteVideo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cbMuteVideo.Appearance = System.Windows.Forms.Appearance.Button
            Me.cbMuteVideo.BackgroundImage = Global.JANIS.My.Resources.Resources.sound_on_green
            Me.cbMuteVideo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            Me.cbMuteVideo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.cbMuteVideo.FlatAppearance.BorderSize = 0
            Me.cbMuteVideo.FlatAppearance.CheckedBackColor = System.Drawing.Color.Black
            Me.cbMuteVideo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black
            Me.cbMuteVideo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.cbMuteVideo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbMuteVideo.ForeColor = System.Drawing.Color.Black
            Me.cbMuteVideo.Location = New System.Drawing.Point(0, 173)
            Me.cbMuteVideo.Name = "cbMuteVideo"
            Me.cbMuteVideo.Size = New System.Drawing.Size(30, 30)
            Me.cbMuteVideo.TabIndex = 48
            Me.ToolTip1.SetToolTip(Me.cbMuteVideo, "Video Sound is ON")
            '
            'lblRemoteStatus
            '
            Me.lblRemoteStatus.BackColor = System.Drawing.Color.DarkGray
            Me.lblRemoteStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblRemoteStatus.Location = New System.Drawing.Point(107, 10)
            Me.lblRemoteStatus.Name = "lblRemoteStatus"
            Me.lblRemoteStatus.Size = New System.Drawing.Size(150, 20)
            Me.lblRemoteStatus.TabIndex = 29
            Me.lblRemoteStatus.Text = "Audience Display"
            Me.lblRemoteStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            Me.ToolTip1.SetToolTip(Me.lblRemoteStatus, "Video playback mirrors 1x/second")
            Me.lblRemoteStatus.UseWaitCursor = True
            '
            'picRemoteViewer
            '
            Me.picRemoteViewer.BackColor = System.Drawing.Color.Transparent
            Me.picRemoteViewer.ImageLocation = ""
            Me.picRemoteViewer.Location = New System.Drawing.Point(0, 0)
            Me.picRemoteViewer.Name = "picRemoteViewer"
            Me.picRemoteViewer.Size = New System.Drawing.Size(364, 204)
            Me.picRemoteViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.picRemoteViewer.TabIndex = 28
            Me.picRemoteViewer.TabStop = False
            Me.ToolTip1.SetToolTip(Me.picRemoteViewer, "Drag images here from a browser to display instantly")
            '
            'CountdownTimer
            '
            Me.CountdownTimer.Interval = 1000
            '
            'lblMediaLibraryCount
            '
            Me.lblMediaLibraryCount.BackColor = System.Drawing.Color.PaleTurquoise
            Me.lblMediaLibraryCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblMediaLibraryCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblMediaLibraryCount.Location = New System.Drawing.Point(719, 225)
            Me.lblMediaLibraryCount.Name = "lblMediaLibraryCount"
            Me.lblMediaLibraryCount.Size = New System.Drawing.Size(272, 24)
            Me.lblMediaLibraryCount.TabIndex = 35
            Me.lblMediaLibraryCount.Text = "Items in Media Library:"
            Me.lblMediaLibraryCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.ToolTip1.SetToolTip(Me.lblMediaLibraryCount, "Double-Click this message to re-index the image search library." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Useful if you've" &
        " added new images while JANIS is running.")
            Me.lblMediaLibraryCount.UseMnemonic = False
            '
            'btnPasteMedia
            '
            Me.btnPasteMedia.AllowDrop = True
            Me.btnPasteMedia.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
            Me.btnPasteMedia.Location = New System.Drawing.Point(3, 50)
            Me.btnPasteMedia.Name = "btnPasteMedia"
            Me.btnPasteMedia.Size = New System.Drawing.Size(119, 28)
            Me.btnPasteMedia.TabIndex = 17
            Me.btnPasteMedia.Text = "PASTE IMAGE"
            Me.ToolTip1.SetToolTip(Me.btnPasteMedia, "Copy image from browser, etc. and paste here to display.")
            '
            'gbCountdownControls
            '
            Me.gbCountdownControls.BackColor = System.Drawing.Color.LightGray
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
            Me.gbCountdownControls.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.gbCountdownControls.Location = New System.Drawing.Point(719, 57)
            Me.gbCountdownControls.Name = "gbCountdownControls"
            Me.gbCountdownControls.Size = New System.Drawing.Size(272, 153)
            Me.gbCountdownControls.TabIndex = 20
            Me.gbCountdownControls.TabStop = False
            Me.gbCountdownControls.Text = "Countdown Timer"
            '
            'Label37
            '
            Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label37.Location = New System.Drawing.Point(207, 22)
            Me.Label37.Name = "Label37"
            Me.Label37.Size = New System.Drawing.Size(40, 16)
            Me.Label37.TabIndex = 23
            Me.Label37.Text = "SS"
            Me.Label37.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'Label36
            '
            Me.Label36.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label36.Location = New System.Drawing.Point(163, 22)
            Me.Label36.Name = "Label36"
            Me.Label36.Size = New System.Drawing.Size(40, 16)
            Me.Label36.TabIndex = 22
            Me.Label36.Text = "MM"
            Me.Label36.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'Label35
            '
            Me.Label35.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label35.Location = New System.Drawing.Point(119, 22)
            Me.Label35.Name = "Label35"
            Me.Label35.Size = New System.Drawing.Size(40, 16)
            Me.Label35.TabIndex = 21
            Me.Label35.Text = "HH"
            Me.Label35.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'Label34
            '
            Me.Label34.BackColor = System.Drawing.Color.LightCoral
            Me.Label34.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label34.ForeColor = System.Drawing.Color.Black
            Me.Label34.Location = New System.Drawing.Point(28, 70)
            Me.Label34.Name = "Label34"
            Me.Label34.Size = New System.Drawing.Size(85, 20)
            Me.Label34.TabIndex = 28
            Me.Label34.Text = "Turn Red:"
            Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'nudCountdownWarnSeconds
            '
            Me.nudCountdownWarnSeconds.Location = New System.Drawing.Point(207, 70)
            Me.nudCountdownWarnSeconds.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
            Me.nudCountdownWarnSeconds.Name = "nudCountdownWarnSeconds"
            Me.nudCountdownWarnSeconds.Size = New System.Drawing.Size(44, 23)
            Me.nudCountdownWarnSeconds.TabIndex = 31
            Me.nudCountdownWarnSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'nudCountdownWarnMinutes
            '
            Me.nudCountdownWarnMinutes.Location = New System.Drawing.Point(163, 70)
            Me.nudCountdownWarnMinutes.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
            Me.nudCountdownWarnMinutes.Name = "nudCountdownWarnMinutes"
            Me.nudCountdownWarnMinutes.Size = New System.Drawing.Size(44, 23)
            Me.nudCountdownWarnMinutes.TabIndex = 30
            Me.nudCountdownWarnMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'nudCountdownWarnHours
            '
            Me.nudCountdownWarnHours.Location = New System.Drawing.Point(119, 70)
            Me.nudCountdownWarnHours.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
            Me.nudCountdownWarnHours.Name = "nudCountdownWarnHours"
            Me.nudCountdownWarnHours.Size = New System.Drawing.Size(44, 23)
            Me.nudCountdownWarnHours.TabIndex = 29
            Me.nudCountdownWarnHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'Label33
            '
            Me.Label33.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label33.Location = New System.Drawing.Point(28, 42)
            Me.Label33.Name = "Label33"
            Me.Label33.Size = New System.Drawing.Size(85, 20)
            Me.Label33.TabIndex = 24
            Me.Label33.Text = "Time Left:"
            Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'cbCountdownVisible
            '
            Me.cbCountdownVisible.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.cbCountdownVisible.Location = New System.Drawing.Point(112, 112)
            Me.cbCountdownVisible.Name = "cbCountdownVisible"
            Me.cbCountdownVisible.Size = New System.Drawing.Size(67, 22)
            Me.cbCountdownVisible.TabIndex = 33
            Me.cbCountdownVisible.Text = "Visible"
            '
            'nudCountdownSeconds
            '
            Me.nudCountdownSeconds.Location = New System.Drawing.Point(207, 42)
            Me.nudCountdownSeconds.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
            Me.nudCountdownSeconds.Name = "nudCountdownSeconds"
            Me.nudCountdownSeconds.Size = New System.Drawing.Size(44, 23)
            Me.nudCountdownSeconds.TabIndex = 27
            Me.nudCountdownSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'nudCountdownMinutes
            '
            Me.nudCountdownMinutes.Location = New System.Drawing.Point(163, 42)
            Me.nudCountdownMinutes.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
            Me.nudCountdownMinutes.Name = "nudCountdownMinutes"
            Me.nudCountdownMinutes.Size = New System.Drawing.Size(44, 23)
            Me.nudCountdownMinutes.TabIndex = 26
            Me.nudCountdownMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.nudCountdownMinutes.Value = New Decimal(New Integer() {5, 0, 0, 0})
            '
            'nudCountdownHours
            '
            Me.nudCountdownHours.Location = New System.Drawing.Point(119, 42)
            Me.nudCountdownHours.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
            Me.nudCountdownHours.Name = "nudCountdownHours"
            Me.nudCountdownHours.Size = New System.Drawing.Size(44, 23)
            Me.nudCountdownHours.TabIndex = 25
            Me.nudCountdownHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'btnResetCountdown
            '
            Me.btnResetCountdown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnResetCountdown.ForeColor = System.Drawing.SystemColors.ControlText
            Me.btnResetCountdown.Location = New System.Drawing.Point(30, 110)
            Me.btnResetCountdown.Name = "btnResetCountdown"
            Me.btnResetCountdown.Size = New System.Drawing.Size(60, 24)
            Me.btnResetCountdown.TabIndex = 32
            Me.btnResetCountdown.Text = "RESET"
            '
            'btnStartCountdown
            '
            Me.btnStartCountdown.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnStartCountdown.ForeColor = System.Drawing.Color.Green
            Me.btnStartCountdown.Location = New System.Drawing.Point(195, 110)
            Me.btnStartCountdown.Name = "btnStartCountdown"
            Me.btnStartCountdown.Size = New System.Drawing.Size(60, 24)
            Me.btnStartCountdown.TabIndex = 34
            Me.btnStartCountdown.Text = "START"
            '
            'btnReIndexImgLib
            '
            Me.btnReIndexImgLib.BackColor = System.Drawing.Color.DarkTurquoise
            Me.btnReIndexImgLib.FlatAppearance.BorderSize = 0
            Me.btnReIndexImgLib.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnReIndexImgLib.Font = New System.Drawing.Font("Segoe MDL2 Assets", 8.0!, System.Drawing.FontStyle.Bold)
            Me.btnReIndexImgLib.Location = New System.Drawing.Point(966, 226)
            Me.btnReIndexImgLib.Name = "btnReIndexImgLib"
            Me.btnReIndexImgLib.Size = New System.Drawing.Size(24, 22)
            Me.btnReIndexImgLib.TabIndex = 36
            Me.btnReIndexImgLib.Text = ""
            Me.btnReIndexImgLib.TextAlign = System.Drawing.ContentAlignment.TopCenter
            Me.ToolTip1.SetToolTip(Me.btnReIndexImgLib, "Click here to re-index the image library" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "if you've added or deleted image files." &
        "")
            Me.btnReIndexImgLib.UseVisualStyleBackColor = False
            '
            'btnHot10
            '
            Me.btnHot10.AllowDrop = True
            Me.btnHot10.BackColor = System.Drawing.Color.FromArgb(CType(CType(98, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(152, Byte), Integer))
            Me.btnHot10.Font = New System.Drawing.Font("Segoe UI Emoji", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot10.ForeColor = System.Drawing.Color.White
            Me.btnHot10.Location = New System.Drawing.Point(895, 266)
            Me.btnHot10.Name = "btnHot10"
            Me.btnHot10.Size = New System.Drawing.Size(96, 28)
            Me.btnHot10.TabIndex = 46
            Me.btnHot10.Tag = "10"
            Me.btnHot10.Text = "Hot 10"
            Me.btnHot10.UseVisualStyleBackColor = False
            '
            'btnHot9
            '
            Me.btnHot9.AllowDrop = True
            Me.btnHot9.BackColor = System.Drawing.Color.FromArgb(CType(CType(98, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(152, Byte), Integer))
            Me.btnHot9.Font = New System.Drawing.Font("Segoe UI Emoji", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot9.ForeColor = System.Drawing.Color.White
            Me.btnHot9.Location = New System.Drawing.Point(796, 266)
            Me.btnHot9.Name = "btnHot9"
            Me.btnHot9.Size = New System.Drawing.Size(96, 28)
            Me.btnHot9.TabIndex = 45
            Me.btnHot9.Tag = "9"
            Me.btnHot9.Text = "Hot 9"
            Me.btnHot9.UseVisualStyleBackColor = False
            '
            'btnHot8
            '
            Me.btnHot8.AllowDrop = True
            Me.btnHot8.BackColor = System.Drawing.Color.FromArgb(CType(CType(98, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(152, Byte), Integer))
            Me.btnHot8.Font = New System.Drawing.Font("Segoe UI Emoji", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot8.ForeColor = System.Drawing.Color.White
            Me.btnHot8.Location = New System.Drawing.Point(697, 266)
            Me.btnHot8.Name = "btnHot8"
            Me.btnHot8.Size = New System.Drawing.Size(96, 28)
            Me.btnHot8.TabIndex = 44
            Me.btnHot8.Tag = "8"
            Me.btnHot8.Text = "Hot 8"
            Me.btnHot8.UseVisualStyleBackColor = False
            '
            'btnHot7
            '
            Me.btnHot7.AllowDrop = True
            Me.btnHot7.BackColor = System.Drawing.Color.FromArgb(CType(CType(98, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(152, Byte), Integer))
            Me.btnHot7.Font = New System.Drawing.Font("Segoe UI Emoji", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot7.ForeColor = System.Drawing.Color.White
            Me.btnHot7.Location = New System.Drawing.Point(598, 266)
            Me.btnHot7.Name = "btnHot7"
            Me.btnHot7.Size = New System.Drawing.Size(96, 28)
            Me.btnHot7.TabIndex = 43
            Me.btnHot7.Tag = "7"
            Me.btnHot7.Text = "Hot 7"
            Me.btnHot7.UseVisualStyleBackColor = False
            '
            'btnHot6
            '
            Me.btnHot6.AllowDrop = True
            Me.btnHot6.BackColor = System.Drawing.Color.FromArgb(CType(CType(98, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(152, Byte), Integer))
            Me.btnHot6.Font = New System.Drawing.Font("Segoe UI Emoji", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot6.ForeColor = System.Drawing.Color.White
            Me.btnHot6.Location = New System.Drawing.Point(499, 266)
            Me.btnHot6.Name = "btnHot6"
            Me.btnHot6.Size = New System.Drawing.Size(96, 28)
            Me.btnHot6.TabIndex = 42
            Me.btnHot6.Tag = "6"
            Me.btnHot6.Text = "Hot 6"
            Me.btnHot6.UseVisualStyleBackColor = False
            '
            'btnHot5
            '
            Me.btnHot5.AllowDrop = True
            Me.btnHot5.BackColor = System.Drawing.Color.FromArgb(CType(CType(98, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(152, Byte), Integer))
            Me.btnHot5.Font = New System.Drawing.Font("Segoe UI Emoji", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot5.ForeColor = System.Drawing.Color.White
            Me.btnHot5.Location = New System.Drawing.Point(399, 266)
            Me.btnHot5.Name = "btnHot5"
            Me.btnHot5.Size = New System.Drawing.Size(96, 28)
            Me.btnHot5.TabIndex = 41
            Me.btnHot5.Tag = "5"
            Me.btnHot5.Text = "Hot 5"
            Me.btnHot5.UseVisualStyleBackColor = False
            '
            'btnHot4
            '
            Me.btnHot4.AllowDrop = True
            Me.btnHot4.BackColor = System.Drawing.Color.FromArgb(CType(CType(98, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(152, Byte), Integer))
            Me.btnHot4.Font = New System.Drawing.Font("Segoe UI Emoji", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot4.ForeColor = System.Drawing.Color.White
            Me.btnHot4.Location = New System.Drawing.Point(300, 266)
            Me.btnHot4.Name = "btnHot4"
            Me.btnHot4.Size = New System.Drawing.Size(96, 28)
            Me.btnHot4.TabIndex = 40
            Me.btnHot4.Tag = "4"
            Me.btnHot4.Text = "Hot 4"
            Me.btnHot4.UseVisualStyleBackColor = False
            '
            'btnHot3
            '
            Me.btnHot3.AllowDrop = True
            Me.btnHot3.BackColor = System.Drawing.Color.FromArgb(CType(CType(98, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(152, Byte), Integer))
            Me.btnHot3.Font = New System.Drawing.Font("Segoe UI Emoji", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot3.ForeColor = System.Drawing.Color.White
            Me.btnHot3.Location = New System.Drawing.Point(202, 266)
            Me.btnHot3.Name = "btnHot3"
            Me.btnHot3.Size = New System.Drawing.Size(96, 28)
            Me.btnHot3.TabIndex = 39
            Me.btnHot3.Tag = "3"
            Me.btnHot3.Text = "Hot 3"
            Me.btnHot3.UseVisualStyleBackColor = False
            '
            'btnHot2
            '
            Me.btnHot2.AllowDrop = True
            Me.btnHot2.BackColor = System.Drawing.Color.FromArgb(CType(CType(98, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(152, Byte), Integer))
            Me.btnHot2.Font = New System.Drawing.Font("Segoe UI Emoji", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot2.ForeColor = System.Drawing.Color.White
            Me.btnHot2.Location = New System.Drawing.Point(103, 266)
            Me.btnHot2.Name = "btnHot2"
            Me.btnHot2.Size = New System.Drawing.Size(96, 28)
            Me.btnHot2.TabIndex = 38
            Me.btnHot2.Tag = "2"
            Me.btnHot2.Text = "Hot 2"
            Me.btnHot2.UseVisualStyleBackColor = False
            '
            'btnHot1
            '
            Me.btnHot1.AllowDrop = True
            Me.btnHot1.BackColor = System.Drawing.Color.FromArgb(CType(CType(98, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(152, Byte), Integer))
            Me.btnHot1.Font = New System.Drawing.Font("Segoe UI Emoji", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnHot1.ForeColor = System.Drawing.Color.White
            Me.btnHot1.Location = New System.Drawing.Point(4, 266)
            Me.btnHot1.Name = "btnHot1"
            Me.btnHot1.Size = New System.Drawing.Size(96, 28)
            Me.btnHot1.TabIndex = 37
            Me.btnHot1.TabStop = False
            Me.btnHot1.Tag = "1"
            Me.btnHot1.Text = "Hot 1"
            Me.btnHot1.UseVisualStyleBackColor = False
            '
            'Label15
            '
            Me.Label15.BackColor = System.Drawing.Color.Transparent
            Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label15.Location = New System.Drawing.Point(3, 4)
            Me.Label15.Name = "Label15"
            Me.Label15.Size = New System.Drawing.Size(110, 24)
            Me.Label15.TabIndex = 0
            Me.Label15.Text = "Team Location"
            Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'tbLeftLoc
            '
            Me.tbLeftLoc.BackColor = System.Drawing.SystemColors.Window
            Me.tbLeftLoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbLeftLoc.ForeColor = System.Drawing.SystemColors.WindowText
            Me.tbLeftLoc.Location = New System.Drawing.Point(103, 4)
            Me.tbLeftLoc.MaxLength = 13
            Me.tbLeftLoc.Name = "tbLeftLoc"
            Me.tbLeftLoc.Size = New System.Drawing.Size(172, 22)
            Me.tbLeftLoc.TabIndex = 1
            Me.tbLeftLoc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'Label41
            '
            Me.Label41.BackColor = System.Drawing.Color.Transparent
            Me.Label41.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label41.Location = New System.Drawing.Point(879, 4)
            Me.Label41.Name = "Label41"
            Me.Label41.Size = New System.Drawing.Size(112, 24)
            Me.Label41.TabIndex = 7
            Me.Label41.Text = "Team Location"
            Me.Label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'tbRightLoc
            '
            Me.tbRightLoc.BackColor = System.Drawing.SystemColors.Window
            Me.tbRightLoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tbRightLoc.ForeColor = System.Drawing.SystemColors.WindowText
            Me.tbRightLoc.Location = New System.Drawing.Point(720, 4)
            Me.tbRightLoc.MaxLength = 13
            Me.tbRightLoc.Name = "tbRightLoc"
            Me.tbRightLoc.Size = New System.Drawing.Size(172, 22)
            Me.tbRightLoc.TabIndex = 6
            Me.tbRightLoc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'grpInstantMedia
            '
            Me.grpInstantMedia.Controls.Add(Me.btnPasteMedia)
            Me.grpInstantMedia.Controls.Add(Me.btnMediaLoadFile)
            Me.grpInstantMedia.Cursor = System.Windows.Forms.Cursors.Arrow
            Me.grpInstantMedia.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpInstantMedia.Location = New System.Drawing.Point(95, 66)
            Me.grpInstantMedia.Name = "grpInstantMedia"
            Me.grpInstantMedia.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.grpInstantMedia.Size = New System.Drawing.Size(126, 82)
            Me.grpInstantMedia.TabIndex = 15
            Me.grpInstantMedia.TabStop = False
            Me.grpInstantMedia.Text = "Images && Videos"
            '
            'VideoEventTimer
            '
            Me.VideoEventTimer.Interval = 250
            '
            'fmMain
            '
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
            Me.ClientSize = New System.Drawing.Size(996, 699)
            Me.Controls.Add(Me.tbRightLoc)
            Me.Controls.Add(Me.Label41)
            Me.Controls.Add(Me.tbLeftLoc)
            Me.Controls.Add(Me.Label15)
            Me.Controls.Add(Me.btnReIndexImgLib)
            Me.Controls.Add(Me.gbCountdownControls)
            Me.Controls.Add(Me.grpInstantMedia)
            Me.Controls.Add(Me.lblMediaLibraryCount)
            Me.Controls.Add(Me.pnlRemoteViewer)
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
            Me.Controls.Add(Me.btnShowScore)
            Me.Controls.Add(Me.tbRightScore)
            Me.Controls.Add(Me.tbLeftScore)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.tbRightTeam)
            Me.Controls.Add(Me.tbLeftTeam)
            Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ForeColor = System.Drawing.SystemColors.WindowText
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.KeyPreview = True
            Me.Location = New System.Drawing.Point(220, 30)
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(1012, 738)
            Me.MinimumSize = New System.Drawing.Size(1012, 738)
            Me.Name = "fmMain"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
            Me.Text = "JANIS"
            Me.ToolTip1.SetToolTip(Me, "Blackout previews and audience displays instantly")
            Me.TabControl1.ResumeLayout(False)
            Me.tpScreenText.ResumeLayout(False)
            Me.tpScreenText.PerformLayout()
            Me.grpLoadText.ResumeLayout(False)
            Me.grpClearText.ResumeLayout(False)
            Me.grpRightColors.ResumeLayout(False)
            Me.grpLeftColors.ResumeLayout(False)
            Me.tpMediaSearch.ResumeLayout(False)
            Me.pnlMediaSearchPreview.ResumeLayout(False)
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
            Me.pnlRemoteViewer.ResumeLayout(False)
            CType(Me.picRemoteViewer, System.ComponentModel.ISupportInitialize).EndInit()
            Me.gbCountdownControls.ResumeLayout(False)
            CType(Me.nudCountdownWarnSeconds, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudCountdownWarnMinutes, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudCountdownWarnHours, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudCountdownSeconds, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudCountdownMinutes, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudCountdownHours, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpInstantMedia.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private Sub fmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Me.Opacity = 0.0F          '* Make invisible until we finish loading
            If Me.AppAlreadyRunning() Then
                '* An autoexit has already been configured with the slide timer.
                Me.splash.Close()
                Me.InitiateQuietShutdown()
            Else
                Me.InitializeSettings()

                Me.LS = New fmScreen()
                '* Save the label message heights because the timer screws around with them and needs to restore them
                Me.LS.lblMsg.Tag = Me.LS.lblMsg.Height
                Me.LS.SetVideoMute(Me.cbMuteVideo.Checked)

                Me.VerifyInfrastructure()

                Me.splash.SetStatus("Setting Preferences...")

                Me.LoadPrefsFromFile(PREFS_FILE)
                Me.ApplyPrefs()

                '* Here's the wacky way you change font sizes in VB.NET. 
                Me.tbLeftText.Font = New Font(Me.tbLeftText.Font.Name, CSng(Single.Parse(Me.tbLeftFontSize.Text) / DisplayToEntryFontRatio), Me.tbLeftText.Font.Style)
                Me.tbRightText.Font = New Font(Me.tbRightText.Font.Name, CSng(Single.Parse(Me.tbRightFontSize.Text) / DisplayToEntryFontRatio), Me.tbRightText.Font.Style)

                Me.SetMonitorDisplayMode()

                If Not (Me.cbDisplayDefaultImage.Checked Or Me.cbPlaySlidesAtStart.Checked) Then
                    '* Me.DisplayTextScreen(Me.LS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, CSng(Me.tbLeftFontSize.Text) * Me.DisplayFontRatio)  'For debugging
                    Me.LS.Blackout()
                    Me.picRemoteViewer.ImageLocation = ""
                    Me.picRemoteViewer.Image = Nothing
                End If

                '* Show the audience display early, so loading image library doesn't delay its appearance
                Me.LS.Show()
                Me.ScreenReady = True

                Me.tvSlideFolders_Init(Me.tbDefaultImageDir.Text)

                Me.splash.SetStatus("Building Image Library...")
                Me.BuildMediaLibrary()
                Me.Opacity = 1.0F          '* Make visible again
            End If
        End Sub

        Private Sub fmMain_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
            If Not Me.SlideTimerTag = "AppAlreadyRunning" Then
                Me.CurrentPrefs = ReadPrefsFromUI()
                If Me.PrefsChanged() Then
                    Dim Ans As DialogResult = MessageBox.Show(Me, "Modifications to preferences have not been saved. Save them before closing?", "Preferences Have Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)
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
            MessageBox.Show(Me, "A previous instance of JANIS is already open!", "Already Running", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' This instance will self-destruct in 5 seconds.
            Me.SlideTimer.Interval = 5000
            Me.SlideTimerTag = "AppAlreadyRunning"
            Me.SlideTimer.Enabled = True
            Me.SlideTimer.Start()
        End Sub

        Private Sub InitializeSettings()
            PREFS_FILE = ROOT_SUPPORT_DIR & "\JANIS.json"

            Me.MediaFileExtensions.UnionWith(Me.VideoFileExtensions)
            Me.MediaFileExtensions.UnionWith(Me.ImageFileExtensions)

            '* Get the working dimensions of the primary monitor
            Dim workingArea As System.Drawing.Rectangle
            workingArea = Screen.GetWorkingArea(New System.Drawing.Point(200, 200))

            Me.Top = workingArea.Top
            Me.Left = SystemInformation.PrimaryMonitorSize.Width - Me.Size.Width

            Me.InitializeButtonGlyphs()

            '* Here's what we want to see or hide.
            Me.tbCurrentThing.Visible = False
            Me.tbSubstitutions.Visible = False
            Me.btnShowThingLeft.Visible = False

            Me.HotButtonsChanged = False
            Me.cbHBActive.Checked = True    '* default to "can see"
            Me.picRemoteViewer.AllowDrop = True
            Me.PreviousSelectedTab = TabControl1.SelectedTab

            Me.cbMuteVideo.Checked = False
            Me.cbMuteVideo.BackgroundImage = My.Resources.sound_on_green

            Me.InitializePreviewPlayers()
        End Sub

        Private Sub InitializeButtonGlyphs()
            btnPlaySlides.Text = Convert.ToChar(&HF5B0)
            btnStopSlides.Text = Convert.ToChar(&HE009)
            btnPauseSlides.Text = Convert.ToChar(&HF8AE)
            btnPrevSlide.Text = Convert.ToChar(&HE96F)
            btnNextSlide.Text = Convert.ToChar(&HE970)
            btnFirstSlide.Text = Convert.ToChar(&HF8AC)
            btnLastSlide.Text = Convert.ToChar(&HF8AD)
            btnThingUp.Text = Convert.ToChar(&HEB11)
            btnThingDown.Text = Convert.ToChar(&HEB0F)
            btnSlideUp.Text = Convert.ToChar(&HEB11)
            btnSlideDown.Text = Convert.ToChar(&HEB0F)
        End Sub

        Private Sub InitializePreviewPlayers()
            '* Shared LibVLC instance for both preview players (no audio at all, won't affect audience video)
            Me._libVLC = New LibVLCSharp.Shared.LibVLC("--no-audio")

            '* DO NOT TRUST that .Mute works for th

            '* Media Search preview
            Me._searchPreviewVideoPlayer = New LibVLCSharp.Shared.MediaPlayer(Me._libVLC)
            Me._searchPreviewVideoPlayer.Volume = 0
            Me._searchPreviewVideoPlayer.Mute = True
            Me._searchPreviewVideoView = New LibVLCSharp.WinForms.VideoView()
            Me._searchPreviewVideoView.MediaPlayer = Me._searchPreviewVideoPlayer
            Me._searchPreviewVideoView.Location = New Point(0, 0)
            Me._searchPreviewVideoView.Size = Me.pnlMediaSearchPreview.Size
            Me._searchPreviewVideoView.BackColor = Color.Black
            Me._searchPreviewVideoView.Visible = False
            Me.pnlMediaSearchPreview.Controls.Add(Me._searchPreviewVideoView)

            '* Slideshow preview
            Me._slidePreviewVideoPlayer = New LibVLCSharp.Shared.MediaPlayer(Me._libVLC)
            Me._slidePreviewVideoPlayer.Volume = 0
            Me._slidePreviewVideoPlayer.Mute = True
            Me._slidePreviewVideoView = New LibVLCSharp.WinForms.VideoView()
            Me._slidePreviewVideoView.MediaPlayer = Me._slidePreviewVideoPlayer
            Me._slidePreviewVideoView.Location = Me.picSlidePreview.Location
            Me._slidePreviewVideoView.Size = Me.picSlidePreview.Size
            Me._slidePreviewVideoView.BackColor = Color.Black
            Me._slidePreviewVideoView.Visible = False
            Me.tpSlides.Controls.Add(Me._slidePreviewVideoView)
        End Sub

        Private Sub VerifyInfrastructure()
            '* If the default support dirs aren't there, create them
            If Not System.IO.Directory.Exists(ROOT_SUPPORT_DIR) Then System.IO.Directory.CreateDirectory(ROOT_SUPPORT_DIR)
            If Not System.IO.Directory.Exists(ROOT_SUPPORT_DIR & DEFAULT_SLIDESHOW_DIR) Then System.IO.Directory.CreateDirectory(ROOT_SUPPORT_DIR & DEFAULT_SLIDESHOW_DIR)
            If Not System.IO.Directory.Exists(ROOT_SUPPORT_DIR & DEFAULT_HOTBUTTON_DIR) Then System.IO.Directory.CreateDirectory(ROOT_SUPPORT_DIR & DEFAULT_HOTBUTTON_DIR)
        End Sub

        Private Sub fmMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
            '* Shortcut keys
            Select Case e.KeyCode
                Case Keys.F1
                    Me.AddScore("Left", 1)
                Case Keys.F2
                    Me.AddScore("Left", -1)
                Case Keys.F3
                    Me.AddScore("Left", 5)
                Case Keys.F4
                    Me.AddScore("Left", -5)
                Case Keys.F5
                    Me.AddScore("Right", 1)
                Case Keys.F6
                    Me.AddScore("Right", -1)
                Case Keys.F7
                    Me.AddScore("Right", 5)
                Case Keys.F8
                    Me.AddScore("Right", -5)
                Case Keys.B
                    If e.Modifiers = (Keys.Control Or Keys.Shift) Then  '* Mathematical Or = both at same time
                        MessageBox.Show(Me, "Pass it on...", "BILL LOVES BETSE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                    ElseIf e.Modifiers = Keys.Alt Then
                        Me.btnBlackout_Click(sender, e)
                    End If
            End Select
        End Sub

        Private Sub ListBox_KeyPress(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles lbSlideCandidates.KeyPress, lbSlideList.KeyPress
            Dim lbox As ListBox = DirectCast(sender, ListBox)
            If e.KeyChar = CTRL_A Then   '* SELECT ALL
                If lbox.SelectionMode.ToString Like "*Multi*" Then
                    '* Walk through and select all items in the listbox
                    Dim i As Integer
                    For i = 0 To CInt(lbox.Items.Count) - 1
                        lbox.SetSelected(i, True)
                    Next i
                End If
                e.Handled = True
            End If
        End Sub
        Private Sub NumericEntry_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbLeftFontSize.KeyPress, tbRightFontSize.KeyPress, tbDefaultFontSize.KeyPress, tbLeftScore.KeyPress, tbRightScore.KeyPress
            If e.KeyChar = CTRL_A Then   '* SELECT ALL
                DirectCast(sender, TextBox).SelectAll()
                e.Handled = True
            ElseIf (e.KeyChar <> BACKSPACE) And (e.KeyChar <> "-") And ((e.KeyChar < "0") Or (e.KeyChar > "9")) Then
                'Keep this for later easy debugging of keystrokes: tbLeftText.Text = CStr(Asc(e.KeyChar))
                e.Handled = True
            End If
        End Sub
        Private Sub tbTextEntry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tbLeftText.KeyPress, tbRightText.KeyPress, tbLeftTeam.KeyPress, tbRightTeam.KeyPress, comboImgSearchText.KeyPress, tbSubstitutions.KeyPress, tbNewThing.KeyPress, tbHBtext1.KeyPress, tbHBtext2.KeyPress, tbHBtext3.KeyPress, tbHBtext4.KeyPress, tbHBtext5.KeyPress, tbHBtext6.KeyPress, tbHBtext7.KeyPress, tbHBtext8.KeyPress, tbHBtext9.KeyPress, tbHBtext10.KeyPress
            If e.KeyChar = CTRL_A Then   '* SELECT ALL
                If TypeOf sender Is TextBox Then
                    DirectCast(sender, TextBox).SelectAll()
                ElseIf TypeOf sender Is ComboBox Then
                    DirectCast(sender, ComboBox).SelectAll()
                End If
                e.Handled = True
            End If
        End Sub

        Private Sub LS_ScoreUpdateComplete() Handles LS.ScoreUpdateComplete
            Me.ShowRemoteView()
        End Sub

        Private Sub ClearCurrentPictureboxImage(myPicBox As PictureBox)
            If myPicBox.Image IsNot Nothing Then
                myPicBox.Image.Dispose()
                myPicBox.Image = Nothing
            End If
        End Sub

        Private Sub ShowRemoteView()
            Me.LS.Update()
            Me.ClearCurrentPictureboxImage(Me.picRemoteViewer)
            Me.picRemoteViewer.Image = Me.LS.CaptureScreenImage()
        End Sub
        Private Sub ClearRemoteViewer()
            Me.ClearCurrentPictureboxImage(Me.picRemoteViewer)
        End Sub

        Private Sub btnShowScore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowScore.Click
            Me.DisplayScore()
        End Sub
        Private Sub btnLeftScoreColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeftScoreColor.Click
            Dim btn As Button = DirectCast(sender, Button)
            Dim newcolor As Color = PickColor(CInt(btn.Left), CInt(btn.Top), tbLeftScore.BackColor)
            Me.SetTeamColor("Left", newcolor)
        End Sub
        Private Sub btnRightScoreColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRightScoreColor.Click
            Dim btn As Button = DirectCast(sender, Button)
            Dim newcolor As Color = PickColor(CInt(btn.Left) - 100, CInt(btn.Top), tbRightScore.BackColor)
            Me.SetTeamColor("Right", newcolor)
        End Sub
        Private Sub SetTeamColor(ByVal side As String, ByVal newcolor As Color)
            If side = "Left" Then
                Me.tbLeftScore.BackColor = newcolor
                Me.radioThingColorLeft.BackColor = newcolor
                Me.LS.SetTeamColor("Left", newcolor)
            Else
                Me.tbRightScore.BackColor = newcolor
                Me.radioThingColorRight.BackColor = newcolor
                Me.LS.SetTeamColor("Right", newcolor)
            End If
            Me.DisplayScore()
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
            Dim btn As Button = DirectCast(sender, Button)
            Me.btnClearTextLeft_Click(btn, e)
            Me.btnClearTextRight_Click(btn, e)
            Me.tbLeftText.Focus()
        End Sub
        Private Sub btnShowLeftText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowLeftText.Click
            Me.DisplayTextScreen(Me.LS, Me.tbLeftText.Text, Me.tbLeftText.BackColor, CSng(Me.tbLeftFontSize.Text) * Me.DisplayFontRatio)
        End Sub
        Private Sub btnShowRightText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowRightText.Click
            Me.DisplayTextScreen(Me.LS, Me.tbRightText.Text, Me.tbRightText.BackColor, CSng(Me.tbRightFontSize.Text) * Me.DisplayFontRatio)
        End Sub
        Private Sub btnDocLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDocLoadLeft.Click, btnDocLoadRight.Click
            '* Load the contents of a document into the left or right text entry boxes
            Dim btn As Button = DirectCast(sender, Button)
            Dim Doc As String = LoadDoc()
            If Doc <> "" Then
                If btn.Tag.ToString <> "Left" Then
                    Me.tbRightText.Text = Doc
                End If
                If btn.Tag.ToString <> "Right" Then
                    Me.tbLeftText.Text = Doc
                End If
            End If
            Me.AllScreensToFront()
        End Sub

        Private Function LoadDoc() As String
            Dim Doc As String = ""
            Dim openDialog As New OpenFileDialog()
            openDialog.Filter = "Text File (*.TXT)|*.TXT"
            openDialog.InitialDirectory = ROOT_SUPPORT_DIR
            If openDialog.ShowDialog(Me) = DialogResult.OK Then
                Try
                    Doc = System.IO.File.ReadAllText(openDialog.FileName)
                Catch ex As Exception
                    MessageBox.Show(Me, "An error occurred opening file '" & openDialog.FileName & "'.", "File Error")
                End Try
            End If
            openDialog.Dispose()
            Return Doc
        End Function

        Private Sub pnlTextColorLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlTextColorLeft1.Click, pnlTextColorLeft2.Click, pnlTextColorLeft3.Click, pnlTextColorLeft4.Click, pnlTextColorLeft5.Click, pnlTextColorLeft6.Click
            Me.tbLeftText.BackColor = DirectCast(sender, Panel).BackColor
        End Sub
        Private Sub pnlTextColorRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlTextColorRight1.Click, pnlTextColorRight2.Click, pnlTextColorRight3.Click, pnlTextColorRight4.Click, pnlTextColorRight5.Click, pnlTextColorRight6.Click
            Me.tbRightText.BackColor = DirectCast(sender, Panel).BackColor
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
            Me.ClearCurrentPictureboxImage(Me.picRemoteViewer)
        End Sub

        Private Sub tbFontSize_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbLeftFontSize.KeyUp, tbRightFontSize.KeyUp, tbDefaultFontSize.KeyUp
            Dim tbox As TextBox = DirectCast(sender, TextBox)
            Dim fsiz As Single
            'tbLeftText.Text = "KeyCode: " & CStr(e.KeyCode) & vbCrLf & "KeyData: " & CStr(e.KeyData) & vbCrLf & "KeyValue: " & CStr(e.KeyValue)

            '*** FIRST, zero sizes or smaller = unhandled exception. Don't allow.
            If (tbox.Text = "") Then
                fsiz = 1
            Else
                fsiz = CSng(tbox.Text)
            End If
            If fsiz < 1 Then fsiz = 1
            If tbox.Name = "tbLeftFontSize" Then
                Me.tbLeftText.Font = New Font(Me.tbLeftText.Font.Name, (fsiz / Me.DisplayToEntryFontRatio), Me.tbLeftText.Font.Style)
            ElseIf tbox.Name = "tbRightFontSize" Then
                Me.tbRightText.Font = New Font(Me.tbRightText.Font.Name, (fsiz / Me.DisplayToEntryFontRatio), Me.tbRightText.Font.Style)
            End If '* don't do anything for tbDefaultFontSize - that's a startup setting
        End Sub

        Private Sub SetMonitorDisplayMode()
            Dim screens As Screen() = Screen.AllScreens

            If screens.Length > 1 Then
                ' Full blown 2-monitor mode (control + display)
                ' Find the rightmost screen
                Dim rightmostScreen As Screen = screens.OrderByDescending(Function(s) s.Bounds.X).First()
                Dim upperLeft As Point = rightmostScreen.Bounds.Location

                Me.LS.SetLeft(rightmostScreen.Bounds.Location.X)
                Me.LS.SetTop(rightmostScreen.Bounds.Location.Y)

                'Me.LS.SetLeft(monitor2TopLeft.X)
                'Me.LS.SetTop(monitor2TopLeft.Y)
                '* Me.LS.SetLeft(SystemInformation.PrimaryMonitorSize.Width)
                'Me.LS.SetTop(0)
                Me.Text = Me.Text & " - Arena Mode"
            Else
                '* We're in test mode
                Dim sRatio As Integer = 5
                Me.TestMode = True
                Me.DisplayModeAdjustment = sRatio * sRatio  '* = w x h

                Me.LS.SetLeft(Me.Left)
                Me.LS.SetTop(Me.Height + Me.Top)
                Me.LS.AdjustSize(sRatio)
                '* Following is only for Bill to use at home in home testing, when the 3 lines above are commented out
                'Me.LS.SetLeft(0)

                Me.tbLeftText.Text = "TEST MODE"
                Me.Text = Me.Text & "   **** TEST MODE ****"

                '* ONLY FOR TESTING MONITOR DISCONNECTION
                'Me.LS.SetLeft(SystemInformation.PrimaryMonitorSize.Width)  'Force the window onto screen 2
                'Me.LS.SetTop(0)
            End If
            ' Me.tbLeftText.Text = Me.tbLeftText.Text & vbCrLf & vbCrLf & (SystemInformation.MonitorCount - 1).ToString & " audience displays found"
            Me.Text = Me.Text & " (displays: " & (SystemInformation.MonitorCount - 1).ToString & ")"
            Me.tbLeftText.Focus()
        End Sub

        Private Sub DisplayTextScreen(ByVal Scr As fmScreen, ByVal text As String, ByVal hue As Color, ByVal fontsize As Single)
            '* Stop the slideshow if it's running.
            Me.StopSlideShow()
            Me.ClearCurrentPictureboxImage(Me.picRemoteViewer)

            If TestMode Then fontsize = CSng(fontsize * 7.2)
            Scr.SetTextShadows(cbShadowsEnabled.Checked)
            Scr.ShowText(text, hue, Me.DisplayToEntryFontRatio * fontsize / Me.DisplayModeAdjustment)
            Me.ShowRemoteView()
        End Sub

        Private Sub DisplayScore()
            If Not Me.ScreenReady Then Return
            Me.StopSlideShow()
            Me.ClearCurrentPictureboxImage(Me.picRemoteViewer)
            If Me.tbLeftScore.Text = "" Then Me.tbLeftScore.Text = "0"
            If Me.tbRightScore.Text = "" Then Me.tbRightScore.Text = "0"
            Me.LS.ShowScore(Me.tbLeftScore.Text, Me.tbLeftLoc.Text, Me.tbLeftTeam.Text, Me.tbRightScore.Text, Me.tbRightLoc.Text, Me.tbRightTeam.Text)
            Me.ShowRemoteView()
        End Sub

        Private Sub AddScore(ByVal Side As String, ByVal Points As Integer)
            Dim score As Integer
            Dim tbSource As TextBox
            If Side = "Left" Then
                tbSource = tbLeftScore
            Else
                tbSource = tbRightScore
            End If
            score = CInt(tbSource.Text) + Points
            If score > 999 Then score = 999
            If score < -99 Then score = -99
            tbSource.Text = score.ToString
            Me.DisplayScore()
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

        Private Sub AllScreensToFront()
            Me.LS.Select()
            Me.LS.BringToFront()
            Me.Select()
            'Me.BringToFront()
            'Me.Activate()
        End Sub

        Public Function AskIfSure(ByVal prompt As String) As Boolean
            Dim response As Boolean = False

            If MessageBox.Show(Me, prompt, "Are You Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
                response = True
            End If

            Return response
        End Function

    End Class
End Namespace