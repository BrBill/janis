Imports System.Collections.Generic
Imports System.Linq

Namespace JANIS
    Partial Public Class fmMain

        '=================================================================================================
        '* HOTBUTTONS STUFF

        Private Class HotButtonJson
            Public Property Title As String
            Public Property File As String
        End Class

        Private Function HotButtonsToDto() As List(Of HotButtonJson)
            Dim list As New List(Of HotButtonJson)
            For i As Integer = 0 To 9
                list.Add(New HotButtonJson With {
                    .Title = Me.HotText(i).Text,
                    .File = Me.HotButton(i).Tag.ToString
                })
            Next
            Return list
        End Function

        Private Sub DtoToHotButtons(ByVal list As List(Of HotButtonJson))
            For i As Integer = 0 To Math.Min(9, list.Count - 1)
                Me.HotText(i).Text = list(i).Title
                Me.HotButton(i).Tag = list(i).File
                Me.HotImage(i).Text = list(i).File
                Me.HotButton(i).Text = MediaPrefix(list(i).File) & list(i).Title
            Next
        End Sub


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

        Private Function IsLegacyHotButtonFile(ByVal hbfile As String) As Boolean
            Dim firstLine As String = System.IO.File.ReadLines(hbfile).FirstOrDefault()
            Return firstLine IsNot Nothing AndAlso Not firstLine.TrimStart().StartsWith("[")
        End Function

        Private Function SelectHotButtonsFileName() As String
            Dim fn As String
            Dim openDialog As New OpenFileDialog()
            With openDialog
                .Filter = "JANIS HotButtons Files (*.JHB)|*.JHB"
                .InitialDirectory = ROOT_SUPPORT_DIR & DEFAULT_HOTBUTTON_DIR
                If .ShowDialog(Me) = DialogResult.OK Then fn = .FileName Else fn = ""
                .Dispose()
            End With
            Return fn
        End Function

        Private Sub LoadHotButtons(ByVal hbfile As String)
            Try
                If IsLegacyHotButtonFile(hbfile) Then
                    LoadLegacyHotButtons(hbfile)
                Else
                    LoadJsonHotButtons(hbfile)
                End If
                Me.HotButtonsChanged = False
            Catch ex As Exception
                MessageBox.Show(Me, "An error occurred opening HotButtons file '" & hbfile & "'.", "File Error")
            End Try
            Me.AllScreensToFront()
        End Sub

        Private Sub LoadJsonHotButtons(ByVal hbfile As String)
            Dim json As String = System.IO.File.ReadAllText(hbfile)
            Dim options As New System.Text.Json.JsonSerializerOptions With {.PropertyNameCaseInsensitive = True}
            Dim list As List(Of HotButtonJson) = System.Text.Json.JsonSerializer.Deserialize(Of List(Of HotButtonJson))(json, options)
            DtoToHotButtons(list)
        End Sub

        Private Sub LoadLegacyHotButtons(ByVal hbfile As String)
            Dim lines As String() = System.IO.File.ReadAllLines(hbfile)
            Dim list As New List(Of HotButtonJson)
            For Each line As String In lines
                Dim info() As String = line.Split("¶"c)
                If info.Length >= 2 Then
                    list.Add(New HotButtonJson With {
                        .Title = info(0),
                        .File = info(1)
                    })
                End If
            Next
            DtoToHotButtons(list)

            '* Recycle the old file because we're gonna stomp it.
            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(hbfile, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin)

            '* Rewrite as JSON
            WriteHotButtonsToFile(hbfile, list)
        End Sub

        Private Sub SaveHotButtons()
            If Not Me.HotButtonsChanged Then
                If MessageBox.Show(Me, "You haven't made any changes to the HotButtons. Save anyway?", "Nothing to Save", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
            End If
            Dim sf As New SaveFileDialog()
            With sf
                .Filter = "JANIS HotButtons File (*.JHB)|*.JHB"
                .InitialDirectory = ROOT_SUPPORT_DIR & DEFAULT_HOTBUTTON_DIR
                If .ShowDialog(Me) = DialogResult.OK Then
                    WriteHotButtonsToFile(.FileName, HotButtonsToDto())
                    Me.HotButtonsChanged = False
                End If
                .Dispose()
            End With
            Me.AllScreensToFront()
        End Sub

        Private Sub WriteHotButtonsToFile(ByVal filename As String, ByVal list As List(Of HotButtonJson))
            Dim tempFile As String = filename & ".tmp"
            Dim tempWritten As Boolean = False
            Try
                Dim options As New System.Text.Json.JsonSerializerOptions With {.WriteIndented = True}
                Dim json As String = System.Text.Json.JsonSerializer.Serialize(list, options)
                Using stream As New System.IO.StreamWriter(tempFile, False, System.Text.Encoding.UTF8)
                    stream.Write(json)
                End Using
                tempWritten = True
                If System.IO.File.Exists(filename) Then
                    System.IO.File.Replace(tempFile, filename, filename & ".bak")
                Else
                    System.IO.File.Move(tempFile, filename)
                End If
            Catch ex As Exception
                MessageBox.Show(Me, "An error occurred saving HotButtons file '" & filename & "'." & vbCrLf & "Detail: " & ex.Message,
                                    "HotButtons File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Finally
                If Not tempWritten Then
                    If System.IO.File.Exists(tempFile) Then System.IO.File.Delete(tempFile)
                End If
            End Try
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