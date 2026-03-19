Imports System.Collections.Generic

Namespace JANIS
    Partial Public Class fmMain

        '=================================================================================================
        '* HOTBUTTONS STUFF

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
                Me.HotButton(i).Visible = Me.cbHBActive.Checked
            Next
        End Sub

        Private Function SelectHotButtonsFileName() As String
            Dim fn As String
            Dim [of] As New OpenFileDialog()
            With [of]
                .Filter = "JANIS HotButtons File(*.JHB)|*.JHB"
                .InitialDirectory = ROOT_SUPPORT_DIR & DEFAULT_HOTBUTTON_DIR
                If .ShowDialog(Me) = DialogResult.OK Then fn = .FileName Else fn = ""
                .Dispose()
            End With
            Return fn
        End Function

        Private Sub LoadHotButtons(ByVal hbfile As String)
            Try
                Dim lines As String() = System.IO.File.ReadAllLines(hbfile)
                Dim i As Integer = 0
                For Each line As String In lines
                    If i > 9 Then Exit For
                    Dim info() As String = Split(line, "¶")
                    If info.Length >= 2 Then
                        Me.HotText(i).Text = info(0)
                        Me.HotButton(i).Tag = info(1)
                        Me.HotImage(i).Text = info(1)
                        Me.HotButton(i).Text = MediaPrefix(info(1)) & info(0)
                    End If
                    i += 1
                Next
                Me.HotButtonsChanged = False
            Catch ex As Exception
                MessageBox.Show(Me, "An error occurred opening HotButtons file '" & hbfile & "'.", "File Error")
            End Try
            Me.AllScreensToFront()
        End Sub

        Private Sub SaveHotButtons()
            If Not Me.HotButtonsChanged Then
                If MessageBox.Show(Me, "You haven't made any changes to the HotButtons. Save anyway?", "Nothing to Save", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
            End If
            Dim sf As New SaveFileDialog()
            With sf
                .Filter = "JANIS HotButtons File(*.JHB)|*.JHB"
                .InitialDirectory = ROOT_SUPPORT_DIR & DEFAULT_HOTBUTTON_DIR
                If .ShowDialog(Me) = DialogResult.OK Then
                    Try
                        Dim lines As New List(Of String)
                        For i As Integer = 0 To 9
                            lines.Add(Me.HotText(i).Text.ToString & "¶" & Me.HotButton(i).Tag.ToString)
                        Next
                        System.IO.File.WriteAllLines(.FileName, lines)
                        Me.HotButtonsChanged = False
                    Catch ex As Exception
                        MessageBox.Show(Me, "An error occurred saving HotButtons file '" & .FileName & "'.", "File Error")
                    End Try
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
                If MessageBox.Show(Me, "Replace the current Hot Buttons without saving?", "Hot Button Definitions Changed", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.OK Then
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
            Dim fn As String = Me.SelectMediaFilename()
            If fn <> "" Then
                Dim i As Integer = CInt(DirectCast(sender, Button).Tag)
                Me.HotButton(i).Text = MediaPrefix(Me.HotImage(i).Text) & HotText(i).Text  '* in case filetype changes
                Me.HotButton(i).Tag = fn
                Me.HotImage(i).Text = fn
                Me.HotButtonsChanged = True
            End If
        End Sub

        Private Sub tbHBtext_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbHBtext1.TextChanged, tbHBtext2.TextChanged, tbHBtext3.TextChanged, tbHBtext4.TextChanged, tbHBtext5.TextChanged, tbHBtext6.TextChanged, tbHBtext7.TextChanged, tbHBtext8.TextChanged, tbHBtext9.TextChanged, tbHBtext10.TextChanged
            Dim buttonIndex As Integer = CInt(DirectCast(sender, TextBox).Tag.ToString)
            Me.HotButton(buttonIndex).Text = MediaPrefix(Me.HotImage(buttonIndex).Text) & Me.HotText(buttonIndex).Text
            Me.HotButtonsChanged = True
        End Sub

        Private Sub tbHBfile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbHBfile1.TextChanged, tbHBfile2.TextChanged, tbHBfile3.TextChanged, tbHBfile4.TextChanged, tbHBfile5.TextChanged, tbHBfile6.TextChanged, tbHBfile7.TextChanged, tbHBfile8.TextChanged, tbHBfile9.TextChanged, tbHBfile10.TextChanged
            '* Put the cursor at the end so that we can see the
            '* more sigificant part of the file name info
            Dim tbox As TextBox = DirectCast(sender, TextBox)
            tbox.SelectionStart = tbox.TextLength
        End Sub

        Private Sub btnHot_MouseClick(ByVal sender As System.Object, ByVal e As MouseEventArgs) Handles btnHot1.MouseClick, btnHot2.MouseClick, btnHot3.MouseClick, btnHot4.MouseClick, btnHot5.MouseClick, btnHot6.MouseClick, btnHot7.MouseClick, btnHot8.MouseClick, btnHot9.MouseClick, btnHot10.MouseClick
            Dim fnam As String = DirectCast(sender, Button).Tag.ToString
            Me.ShowMediaFile(fnam)    '* this stops slideshow if running
        End Sub

        Private Function MediaPrefix(fnam As String) As String
            If fnam IsNot Nothing AndAlso IsVideoFile(fnam) Then
                Return "🎬"
            End If
            Return ""
        End Function

    End Class
End Namespace