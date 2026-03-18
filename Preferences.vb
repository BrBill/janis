Namespace JANIS
    Public Class Preferences
        Public Property LeftTeamColor As Color
        Public Property RightTeamColor As Color
        Public Property DefaultFontSize As String
        Public Property ShadowsEnabled As Boolean
        Public Property DefaultImageDir As String
        Public Property DefaultImageFile As String
        Public Property DisplayDefaultImage As Boolean
        Public Property DefaultHBFile As String
        Public Property LoadDefaultHB As Boolean
        Public Property DefaultSlideDelay As Decimal
        Public Property DefaultSlideShow As String
        Public Property PlaySlidesAtStart As Boolean
        Public Property LoadDefaultSlides As Boolean
        Public Property DefaultCountdownHours As Decimal
        Public Property DefaultCountdownMinutes As Decimal
        Public Property DefaultCountdownSeconds As Decimal

        Public Sub New()
            '* Factory defaults
            LeftTeamColor = Color.FromArgb(0, 0, 176)
            RightTeamColor = Color.Maroon
            DefaultFontSize = "60"
            ShadowsEnabled = True
            DefaultImageDir = "C:\JANIS"
            DefaultImageFile = ""
            DisplayDefaultImage = False
            DefaultHBFile = ""
            LoadDefaultHB = False
            DefaultSlideDelay = 15
            DefaultSlideShow = ""
            PlaySlidesAtStart = False
            LoadDefaultSlides = False
            DefaultCountdownHours = 0
            DefaultCountdownMinutes = 5
            DefaultCountdownSeconds = 0
        End Sub

        Public Function Clone() As Preferences
            Return DirectCast(Me.MemberwiseClone(), Preferences)
        End Function
    End Class
End Namespace