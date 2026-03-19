Namespace JANIS
    Partial Public Class fmMain

        '=================================================================================================
        '* COUNTDOWN TIMER STUFF

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
            Me.CountdownWarnSeconds = Me.ComputeSeconds(CInt(Me.nudCountdownWarnHours.Value), CInt(Me.nudCountdownWarnMinutes.Value), CInt(Me.nudCountdownWarnSeconds.Value))
        End Sub

        Private Sub StartCountdown()
            If Me.CountdownTimer.Enabled Then Exit Sub
            Me.CountdownSeconds = Me.ComputeSeconds(CInt(Me.nudCountdownHours.Value), CInt(Me.nudCountdownMinutes.Value), CInt(Me.nudCountdownSeconds.Value))
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
            Me.CountdownSeconds = Me.ComputeSeconds(CInt(Me.nudCountdownHours.Value), CInt(Me.nudCountdownMinutes.Value), CInt(Me.nudCountdownSeconds.Value))
            Me.UpdateCountdown()
        End Sub

        Private Function ComputeSeconds(ByVal hours As Integer, ByVal minutes As Integer, ByVal seconds As Integer) As Integer
            Return (hours * 3600) + (minutes * 60) + seconds
        End Function

        Private Sub UpdateCountdown()
            If Not Me.ComponentsDoneInitializing Then Exit Sub '* OR ELSE unhandled exception at app launch
            Dim TimeText As String = ""
            Dim bgColor As System.Drawing.Color = COUNTDOWN_DEFAULT_COLOR

            If Me.CountdownSeconds <= Me.CountdownWarnSeconds Then
                bgColor = COUNTDOWN_WARN_COLOR
            End If

            If Me.nudCountdownHours.Value > 0 Then TimeText = Me.nudCountdownHours.Value.ToString & ":"
            TimeText = TimeText & Format(Me.nudCountdownMinutes.Value, "00") & ":" & Format(Me.nudCountdownSeconds.Value, "00")

            Me.LS.ShowCountdownText(TimeText, bgColor, Me.cbCountdownVisible.Checked)
            Me.ShowRemoteView()
        End Sub

    End Class
End Namespace
