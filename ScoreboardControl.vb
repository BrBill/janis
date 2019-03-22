Public Class ScoreboardControl
    Inherits System.Windows.Forms.UserControl

    Dim LEFT_TEAM_COLOR As System.Drawing.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
    Dim RIGHT_TEAM_COLOR As System.Drawing.Color = System.Drawing.Color.Maroon
    Dim DEFAULT_SCORE_COLOR As System.Drawing.Color = System.Drawing.Color.White
    Dim LEFT_FADE_INCREMENTS() As Integer = {0, 16, 16, 5}     '* If any of these had to decrease to reach the goal color,
    Dim RIGHT_FADE_INCREMENTS() As Integer = {0, 8, 16, 16}    '* The numbers would be negative, but that would break AddColor()
    Const MAX_SCORE As Integer = 999
    Const MIN_SCORE As Integer = -99


    Private Sub Scoreboard_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Function Limited_Score(ByVal score As Integer) As Integer
        '* If the score is too big or too small, it will be too wide to display
        Return Math.Min(Math.Max(score, MIN_SCORE), MAX_SCORE)
    End Function

    Private Function AddColor(ByVal color As System.Drawing.Color, ByVal increments() As Integer) As System.Drawing.Color
        Dim a As Integer = Math.Min(increments(0) + color.A, DEFAULT_SCORE_COLOR.A)
        Dim r As Integer = Math.Min(increments(1) + color.R, DEFAULT_SCORE_COLOR.R)
        Dim g As Integer = Math.Min(increments(2) + color.G, DEFAULT_SCORE_COLOR.G)
        Dim b As Integer = Math.Min(increments(3) + color.B, DEFAULT_SCORE_COLOR.B)

        Return System.Drawing.Color.FromArgb(a, r, g, b)
    End Function

    Private Sub FadeBuff(ByRef lbl As System.Windows.Forms.Label, buffcolor As System.Drawing.Color)
        lbl.ForeColor = buffcolor
        If Not Me.FadeTimer.Enabled Then Me.FadeTimer.Start()
    End Sub

    Private Sub FadeTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FadeTimer.Tick
        Dim changed As Boolean = False
        If Not Me.lblScoreLeft.ForeColor.Equals(DEFAULT_SCORE_COLOR) Then
            Me.lblScoreLeft.ForeColor = Me.AddColor(Me.lblScoreLeft.ForeColor, LEFT_FADE_INCREMENTS)
            changed = True
        End If
        If Not Me.lblScoreRight.ForeColor.Equals(DEFAULT_SCORE_COLOR) Then
            Me.lblScoreRight.ForeColor = Me.AddColor(Me.lblScoreRight.ForeColor, RIGHT_FADE_INCREMENTS)
            changed = True
        End If
        If Not changed Then Me.FadeTimer.Stop()
    End Sub

#Region "Public Properties"
    Public Property TeamLocLeft As String
        Get
            Return Me.lblTeamLocLeft.Text
        End Get
        Set(value As String)
            Me.lblTeamLocLeft.Text = value
        End Set
    End Property
    Public Property TeamLocRight As String
        Get
            Return Me.lblTeamLocRight.Text
        End Get
        Set(value As String)
            Me.lblTeamLocRight.Text = value
        End Set
    End Property
    Public Property TeamNameLeft As String
        Get
            Return Me.lblTeamNameLeft.Text
        End Get
        Set(value As String)
            Me.lblTeamNameLeft.Text = value
        End Set
    End Property
    Public Property TeamNameRight As String
        Get
            Return Me.lblTeamNameRight.Text
        End Get
        Set(value As String)
            Me.lblTeamNameRight.Text = value
        End Set
    End Property
    Public Property ScoreLeft As Integer
        Get
            Return Convert.ToInt32(Me.lblScoreLeft.Text)
        End Get
        Set(ByVal value As Integer)
            Me.lblScoreLeft.Text = Limited_Score(value).ToString
            FadeBuff(lblScoreLeft, LEFT_TEAM_COLOR)
        End Set
    End Property
    Public Property ScoreRight As Integer
        Get
            Return Convert.ToInt32(Me.lblScoreRight.Text)
        End Get
        Set(ByVal value As Integer)
            Me.lblScoreRight.Text = Limited_Score(value).ToString
            FadeBuff(lblScoreRight, RIGHT_TEAM_COLOR)
        End Set
    End Property

#End Region
#Region "Public Methods"

#End Region

End Class

