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

        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing OrElse Not (TypeOf obj Is Preferences) Then Return False
            Dim other As Preferences = DirectCast(obj, Preferences)
            Return LeftTeamColor = other.LeftTeamColor AndAlso
                   RightTeamColor = other.RightTeamColor AndAlso
                   DefaultFontSize = other.DefaultFontSize AndAlso
                   ShadowsEnabled = other.ShadowsEnabled AndAlso
                   DefaultImageDir = other.DefaultImageDir AndAlso
                   DefaultImageFile = other.DefaultImageFile AndAlso
                   DisplayDefaultImage = other.DisplayDefaultImage AndAlso
                   DefaultHBFile = other.DefaultHBFile AndAlso
                   LoadDefaultHB = other.LoadDefaultHB AndAlso
                   DefaultSlideDelay = other.DefaultSlideDelay AndAlso
                   DefaultSlideShow = other.DefaultSlideShow AndAlso
                   PlaySlidesAtStart = other.PlaySlidesAtStart AndAlso
                   LoadDefaultSlides = other.LoadDefaultSlides AndAlso
                   DefaultCountdownHours = other.DefaultCountdownHours AndAlso
                   DefaultCountdownMinutes = other.DefaultCountdownMinutes AndAlso
                   DefaultCountdownSeconds = other.DefaultCountdownSeconds
        End Function

        Public Overrides Function GetHashCode() As Integer
            Dim hash As Integer = 17
            hash = hash * 31 + LeftTeamColor.GetHashCode()
            hash = hash * 31 + RightTeamColor.GetHashCode()
            hash = hash * 31 + If(DefaultFontSize IsNot Nothing, DefaultFontSize.GetHashCode(), 0)
            hash = hash * 31 + ShadowsEnabled.GetHashCode()
            hash = hash * 31 + If(DefaultImageDir IsNot Nothing, DefaultImageDir.GetHashCode(), 0)
            hash = hash * 31 + If(DefaultImageFile IsNot Nothing, DefaultImageFile.GetHashCode(), 0)
            hash = hash * 31 + DisplayDefaultImage.GetHashCode()
            hash = hash * 31 + If(DefaultHBFile IsNot Nothing, DefaultHBFile.GetHashCode(), 0)
            hash = hash * 31 + LoadDefaultHB.GetHashCode()
            hash = hash * 31 + DefaultSlideDelay.GetHashCode()
            hash = hash * 31 + If(DefaultSlideShow IsNot Nothing, DefaultSlideShow.GetHashCode(), 0)
            hash = hash * 31 + PlaySlidesAtStart.GetHashCode()
            hash = hash * 31 + LoadDefaultSlides.GetHashCode()
            hash = hash * 31 + DefaultCountdownHours.GetHashCode()
            hash = hash * 31 + DefaultCountdownMinutes.GetHashCode()
            hash = hash * 31 + DefaultCountdownSeconds.GetHashCode()
            Return hash
        End Function
    End Class
End Namespace