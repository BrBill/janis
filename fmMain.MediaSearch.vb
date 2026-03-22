Imports System.IO
Imports System.Threading.Tasks

Namespace JANIS
    Partial Public Class fmMain
        '=================================================================================================
        '* MEDIA Images, Videos, and Search

        Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
            '* Stop any preview videos that we are running in a tab if we switch away from that tab
            '* Don't need to check for previous tab value = Nothing, because we initialized it in InitializeSettings(). It's never Nothing.
            If (TabControl1.SelectedTab.Name <> "tpMediaSearch" AndAlso PreviousSelectedTab.Name = "tpMediaSearch") Then
                StopPreviewSearchVideo()
            ElseIf (TabControl1.SelectedTab.Name <> "tpSlides" AndAlso PreviousSelectedTab.Name = "tpSlides") Then
                StopPreviewSlideVideo()
            End If
            PreviousSelectedTab = TabControl1.SelectedTab
        End Sub

        Private Sub AssignImageToPictureBox(ByVal picture As PictureBox, ByVal Img As Image)
            If Img Is Nothing Then Exit Sub
            Me.ClearCurrentPictureboxImage(picture)
            picture.Image = Img
            picture.Visible = True
        End Sub

        Private Sub Present_Image(ByVal img As Image, Optional ByVal KillSlideShow As Boolean = True)
            '* Display this image to the display and also the operator remote view box.

            If img IsNot Nothing Then
                If KillSlideShow Then Me.StopSlideShow()

                '* Show displays first for speed.
                Me.LS.ShowImage(img)
                If Me.SlidesStatus <> SLIDES_WHAMMY Then
                    '* Let the operator see what's showing remotely
                    Me.AssignImageToPictureBox(Me.picRemoteViewer, New Bitmap(img))
                End If

                Me.AllScreensToFront()
            End If
        End Sub

        Private Sub LaunchVideo(ByVal fnam As String, Optional ByVal KillSlideShow As Boolean = True)
            If KillSlideShow Then
                Me.StopSlideShow()
            End If

            '* Audio only ever plays if slideshow's not active AND cbMutVideo is unchecked
            'Me.LS.SetVideoMute((Me.SlidesStatus <> SLIDES_STOPPED) Or cbMuteVideo.Checked)
            Dim resultMessage As String = Me.LS.LaunchVideo(fnam)

            If resultMessage IsNot Nothing Then
                If Me.SlidesStatus = SLIDES_PLAYING Then
                    If Me.lbSlideList.Items.Count > 1 Then
                        Return '* Quietly move on to the next slide, similar to slideshow image failures
                    End If
                    Me.StopSlideShow()
                Else
                    MessageBox.Show(Me, resultMessage, "Video Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            Else
                If Me.SlidesStatus = SLIDES_PLAYING Then
                    '* stop the slide timer so the video can play until it ends. The Video Timer will notice when it finishes and restart everything
                    '* however, we still want to look like we're playing slides.
                    Me.SlideTimer.Stop()

                    '* The following delay prevents the slideshow from giving up on the video before the video starts, which
                    '* causes the slideshow to advance a slide immediately after starting the video. This is a kludge, but it works.
                    Me.VideoEventTimer.Interval = 500 '* Give WMP time to transition to playing state (interval is reset in the Tick event)
                End If
                Me.ClearCurrentPictureboxImage(Me.picRemoteViewer)
                Me.VideoEventTimer.Start()
            End If
            Me.AllScreensToFront()
        End Sub

        Private Sub VideoEventTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VideoEventTimer.Tick
            Me.VideoEventTimer.Interval = 250  '* Reset to normal polling interval
            If Me.LS.IsVideoPlaying() Then
                '* Update remoteviewer with snapshot (this is once a second, currently)
                Me.ShowRemoteView()
            Else
                Me.VideoEventTimer.Enabled = False
                Me.LS.StopVideo()
                Me.ClearCurrentPictureboxImage(Me.picRemoteViewer)
                '* If we are mid-slideshow, then the video just ended, so re-enable the slideshow timer and advance
                If Me.SlidesStatus = SLIDES_PLAYING Then  '* Whammy won't start if it contains any video so no need to check for it
                    '* Pretend we were pausing the whole time and restart slideshow. A sensible kludge.
                    Me.SlidesStatus = SLIDES_PAUSED
                    If Me.lbSlideList.Items.Count > 1 Then Me.AdvanceOneSlide() '* If there's only one slide, we loop the video
                    Me.StartSlideShow()
                    '* Freaking elegant
                End If
            End If
        End Sub
        Private Sub cbMuteVideo_CheckedChanged(sender As Object, e As EventArgs) Handles cbMuteVideo.CheckedChanged
            '* It looks like a button, but it's a checkbox!
            If cbMuteVideo.Checked Then
                cbMuteVideo.BackgroundImage = My.Resources.sound_off_red
                ToolTip1.SetToolTip(cbMuteVideo, "Video sound is MUTED")
            Else
                cbMuteVideo.BackgroundImage = My.Resources.sound_on_green
                ToolTip1.SetToolTip(cbMuteVideo, "Video sound is ON")
            End If
            Me.LS.SetVideoMute(cbMuteVideo.Checked)
        End Sub


        Private Sub DisplayRawImage(ByVal img As Image)
            If img Is Nothing Then Exit Sub
            Me.LS.ShowImage(img)
        End Sub

        Private Function DisplayImageFile(ByVal fnam As String, Optional ByVal KillSlideShow As Boolean = True) As Boolean
            Dim img As Image
            Try
                img = Image.FromFile(fnam)
            Catch ex As Exception
                Return False
            End Try
            Me.Present_Image(img, KillSlideShow)
            Return True
        End Function

        Private Sub ShowMediaFile(ByVal fnam As String, Optional ByVal KillSlideShow As Boolean = True)
            '* If the sender doesn't know if the filename is video or image, this is the subroutine they want.
            If fnam = "" Then
                Return
            ElseIf IsVideoFile(fnam) Then
                Me.LaunchVideo(fnam, KillSlideShow)
            ElseIf Not Me.DisplayImageFile(fnam, KillSlideShow) AndAlso KillSlideShow Then
                If File.Exists(fnam) Then
                    MessageBox.Show(Me, "Could not load HotButton image '" & fnam & "'.", "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Else
                    MessageBox.Show(Me, "Could not load HotButton image '" & fnam & "'. The file does not exist.", "Hotbutton Image Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If
        End Sub

        Private Function SelectMediaFilename() As String
            '* Returns blank if you don't pick a legal file
            Dim fn As String = ""
            Dim fileFilter As String = ""
            For Each ext As String In MediaFileExtensions
                fileFilter &= "*" & ext & ";"
            Next
            '* trim off the last semicolon
            If fileFilter.EndsWith(";") Then fileFilter = fileFilter.Substring(0, fileFilter.Length - 1)

            Dim openDialog As New OpenFileDialog()
            With openDialog
                .Filter = "Media Files(" & fileFilter & ")|" & fileFilter
                .InitialDirectory = Me.tbDefaultImageDir.Text
                If .ShowDialog(Me) = DialogResult.OK Then
                    fn = .FileName
                End If
                .Dispose()        '* We made a new file object, we have to dispose of it.
            End With
            Return fn
        End Function

        Private Sub btnMediaLoadFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMediaLoadFile.Click
            Dim fnam As String = SelectMediaFilename()
            If fnam = "" Then
                Return
            ElseIf IsVideoFile(fnam) Then
                System.Threading.Thread.Sleep(100)   '* Avoid failure if things happen too fast
                Me.LaunchVideo(fnam)
                Me.AssignImageToPictureBox(picRemoteViewer, Nothing)
            Else
                Dim img As Image
                Try
                    img = Image.FromFile(fnam)
                    Me.Present_Image(img)
                Catch ex As Exception
                    MessageBox.Show(Me, "Could not load image '" & fnam & "'." & vbCrLf & ex.Message, "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End Try
            End If
        End Sub

        Private Sub btnPasteMedia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPasteMedia.Click
            '* If there's a bitmap in the clipboard, paste it to the screens.
            '* It only works for images
            If Clipboard.GetDataObject.GetDataPresent(GetType(System.Drawing.Bitmap)) Then
                Dim img As Image = CType(Clipboard.GetDataObject.GetData(GetType(System.Drawing.Bitmap)), Bitmap)
                Me.Present_Image(img)
            Else
                MessageBox.Show(Me, "There Is no usable image In the clipboard. Please copy an image To the clipboard And Try again.", "Can't Paste Image")
            End If
        End Sub

        Private Sub picDisplay_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles btnMediaLoadFile.DragDrop, btnPasteMedia.DragDrop, picRemoteViewer.DragDrop
            '* Drag-drop an image from an external source onto one of these picture boxes and have it
            '* display there. This works really well with Firefox.
            Dim img As Image

            '* We've limited this to only 2 kinds of data in picDisplay_DragEnter.
            If e.Data.GetDataPresent(DataFormats.Bitmap) Then
                '* Standard bitmap
                img = CType(e.Data.GetData(GetType(System.Drawing.Bitmap)), Bitmap)
            Else
                '* Device Independent Bitmap
                Dim myStream As Stream = DirectCast(e.Data.GetData(DataFormats.Dib), Stream)
                Using bmp As New BitmapFromDibStream(myStream)
                    img = New Bitmap(bmp)
                End Using
            End If

            Me.Present_Image(img)
        End Sub

        Private Sub picDisplay_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles picRemoteViewer.DragEnter, btnMediaLoadFile.DragEnter, btnPasteMedia.DragEnter
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
            With Me.lbMediaResults
                .SelectedIndex = -1   '* select none
                If .Items.Count > 0 Then .Items.Clear()
            End With

            If Not Me.comboImgSearchText.Text.Length > 0 Then
                MessageBox.Show(Me, "Please enter text to search for.", "No Search Text Supplied", MessageBoxButtons.OK)
                Exit Sub
            End If

            '* Add an item to the combobox dropdown list, removing the oldest one if we need the room.
            '* If it's a match of a previous one, just move it to the top.
            '* Everything is stored and compared as lowercase for easy matching.
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

            Dim UpdateStarted As Boolean = False
            CompareText = "*" & CompareText & "*"   '* wrap for fuzzy search
            For Each CheckFileName As FileID In MediaLibrary
                '* Does this filename contain the supplied text?
                If CheckFileName.Name.ToLower Like CompareText Then
                    If Not UpdateStarted Then
                        Me.lbMediaResults.BeginUpdate()
                        UpdateStarted = True
                    End If
                    Me.lbMediaResults.Items.Add(CheckFileName.FullPath)
                End If
            Next
            If UpdateStarted Then
                Me.lbMediaResults.EndUpdate()
                Me.lbMediaResults.SelectedIndex = 0  '* show the image of the first match
            End If
        End Sub

        Private Sub comboImgSearchText_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboImgSearchText.Enter
            Me.AcceptButton = Me.btnImgSearch
        End Sub
        Private Sub comboImgSearchText_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboImgSearchText.Leave
            Me.AcceptButton = Nothing  '* unassign "default button" behavior
        End Sub

        Private Sub btnSearchMediaShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchMediaShow.Click, lbMediaResults.DoubleClick
            If Me.lbMediaResults.SelectedItem Is Nothing Then Exit Sub
            Dim mediaItem As String = Me.lbMediaResults.SelectedItem.ToString

            If IsVideoFile(mediaItem) Then
                System.Threading.Thread.Sleep(100)  '* Avoid failure if double-clicked too fast after selecting
                Me.LaunchVideo(mediaItem)
                Me.AssignImageToPictureBox(picRemoteViewer, Nothing)
            Else '* is image file
                If Me.picImgSearchPreview.Image IsNot Nothing Then
                    Me.Present_Image(Me.picImgSearchPreview.Image)
                End If
            End If
        End Sub

        Private Sub btnSearchMediaAddSlide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchMediaAddSlide.Click
            Me.lbSlideList.Items.Add(Me.lbMediaResults.SelectedItem)
            'MessageBox(Me, "Media player thinks its mute value is " & Me.LS.GetVideoMute().ToString & " and volume is " & Me.LS.AxMediaPlayer.settings.volume, "Mute Value", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub


        Private Sub PreviewSearchMedia()
            Static PrevSelect As String
            If Me.lbMediaResults.SelectedItems.Count = 1 Then
                If Me.lbMediaResults.SelectedItem.ToString <> PrevSelect Then
                    Dim mediaItem As String = Me.lbMediaResults.SelectedItem.ToString
                    PrevSelect = mediaItem
                    StopPreviewSearchVideo()  '* No-op if not playing

                    If IsVideoFile(mediaItem) Then
                        Me.PlayPreviewSearchVideo(mediaItem)
                    Else
                        Me.ShowPreviewSearchImage(mediaItem)
                    End If
                End If
            Else
                Me.ClearCurrentPictureboxImage(Me.picImgSearchPreview)
                Me.StopPreviewSearchVideo()
                PrevSelect = ""
            End If
        End Sub

        Private Sub StopPreviewSearchVideo()
            If AxMediaSearchPreview.playState = WMPLib.WMPPlayState.wmppsPlaying Then
                AxMediaSearchPreview.Ctlcontrols.stop()
                AxMediaSearchPreview.close()
            End If
        End Sub
        Private Sub PlayPreviewSearchVideo(fnam As String)
            Me.picImgSearchPreview.Hide()
            Me.AxMediaSearchPreview.Show()
            Try
                Me.AxMediaSearchPreview.URL = fnam
                If Me.AxMediaSearchPreview.currentMedia IsNot Nothing Then
                    Me.AxMediaSearchPreview.Ctlcontrols.currentPosition = Me.AxMediaSearchPreview.currentMedia.duration / 4
                End If
                Me.AxMediaSearchPreview.Ctlcontrols.play()
            Catch ex As Exception
                Me.AxMediaSearchPreview.close()
            End Try
        End Sub
        Private Sub ShowPreviewSearchImage(fnam As String)
            Me.AxMediaSearchPreview.Hide()
            Me.picImgSearchPreview.Show()
            Try
                Dim newImg As Image = Image.FromFile(fnam)
                Me.ClearCurrentPictureboxImage(Me.picImgSearchPreview)
                Me.picImgSearchPreview.Image = newImg
            Catch
                Me.ClearCurrentPictureboxImage(Me.picImgSearchPreview)
            End Try
            Me.AxMediaSearchPreview.close()
        End Sub

        Private Sub lbMediaResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbMediaResults.SelectedIndexChanged
            Me.PreviewSearchMedia()
        End Sub
        Private Sub lbMediaResults_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbMediaResults.MouseDown
            'MessageBox(Me, "lbMediaResults_SelectedIndex is " & Str(lbMediaResults.SelectedIndex) & vbCrLf & "value is: " & lbMediaResults.SelectedItem, "SelectedIndex", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ' ----- Prepare the draggable content.
            If Me.lbMediaResults.SelectedIndex >= 0 Then
                Me.PreviewSearchMedia()

                ' ----- Don't start the drag yet. Wait until we move a
                '       certain amount.
                Me.DragBounds = New Rectangle(New Point(e.X -
                   CInt(SystemInformation.DragSize.Width / 2),
                   e.Y - CInt(SystemInformation.DragSize.Height / 2)),
                   SystemInformation.DragSize)
                Me.DragMethod = "from_lbMediaResults"
            End If
        End Sub
        Private Sub lbMediaResults_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbMediaResults.MouseMove
            ' ----- Ignore if not dragging from this control.
            If (Me.DragMethod <> "from_lbMediaResults") Then Return

            ' ----- Have we left the drag boundary?
            'If (DragBounds.Contains(e.X, e.Y) = False) Then
            ' ----- Start the drag-and-drop operation.
            'If (lbMediaResults.DoDragDrop(lbMediaResults.SelectedItems, _
            '      DragDropEffects.Move) = DragDropEffects.Move) Then
            If Me.lbMediaResults.SelectedIndex >= 0 Then
                Me.lbMediaResults.DoDragDrop(Me.lbMediaResults.SelectedItem, DragDropEffects.Copy)
            End If
            '        Me.lbMediaResults.DoDragDrop(Me.lbMediaResults.SelectedItem, DragDropEffects.Copy)
            'End If
            Me.DragMethod = ""
            'End If
        End Sub
        Private Sub lbMediaResults_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lbMediaResults.MouseUp
            ' ----- End of drag-and-drop.
            Me.DragMethod = ""
        End Sub

        Private Sub picImgSearchPreview_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picImgSearchPreview.MouseDown
            '* This routine defines picImgSearchPreview as a draggable entity, text copy only (for image filename)
            If (Not DirectCast(sender, PictureBox).Image Is Nothing) And (Me.lbMediaResults.SelectedIndex >= 0) Then
                Me.lbMediaResults.DoDragDrop(Me.lbMediaResults.SelectedItem, DragDropEffects.Copy)
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
            Dim NewText As String = e.Data.GetData("Text").ToString
            If NewText <> "" Then
                Dim i As Integer = CInt(DirectCast(sender, Button).Name.Substring(6)) - 1      '* the hot button index from the control name
                Dim finfo As FileInfo

                Try
                    finfo = New FileInfo(NewText)
                Catch ex As Exception
                    '* Do nothing. This probably indicates that what was dropped is not a local filename. Likely a webpath.
                    Return
                End Try
                Me.HotButton(i).Tag = NewText
                Me.HotImage(i).Text = NewText
                Dim namelength As Integer = finfo.Name.Replace(finfo.Extension, "").Length
                If namelength > Me.HotText(i).MaxLength Then namelength = Me.HotText(i).MaxLength
                Me.HotText(i).Text = finfo.Name.Substring(0, namelength).ToLower
                Me.HotButton(i).Text = MediaPrefix(NewText) & Me.HotText(i).Text
                Me.HotButtonsChanged = True
            End If
        End Sub

        Private Sub ClearMediaLibrary()
            Me.MediaLibrary.Clear()
            Me.ShowMediaLibraryCount()
        End Sub

        Private Sub BuildMediaLibrary()
            Me.ProcessMediaDir(Me.tbDefaultImageDir.Text)
            Me.ShowMediaLibraryCount()

            '* Test Mode Only - if library is bigger than 10 items DON'T use this
            'If Me.TestMode Then
            '    Dim FileElem As FileID
            '    Me.tbLeftText.Text = "Image Library Dump (TEST MODE)"
            '    For Each FileElem In MediaLibrary
            '        Me.tbLeftText.Text = Me.tbLeftText.Text & vbCrLf & FileElem.FullPath
            '    Next
            'End If
        End Sub

        Private Sub ShowMediaLibraryCount()
            Me.lblMediaLibraryCount.Text = "Items in Media Library: " & Me.MediaLibrary.Count.ToString
            lblMediaLibraryCount.Refresh()   '* Incrementally display count for big libraries
        End Sub

        Private Sub btnReIndexImgLib_Click(sender As System.Object, e As System.EventArgs) Handles btnReIndexImgLib.Click
            Me.ClearMediaLibrary()
            Me.BuildMediaLibrary()
        End Sub

        Private Sub ProcessMediaDir(ByVal DirName As String)
            '* A recursive function to add all graphics file names into the MediaLibrary list.
            '* The list is for searching later.
            '* PASS DIRECTORIES ONLY

            If Not System.IO.Directory.Exists(DirName) Then Exit Sub

            Try
                For Each filePath As String In System.IO.Directory.GetFiles(DirName)
                    Dim fileName As String = System.IO.Path.GetFileName(filePath)
                    If fileName <> ".xvpics" AndAlso IsMediaFile(fileName) Then
                        Dim fileElem As New FileID()
                        fileElem.Path = DirName
                        fileElem.Name = fileName
                        Me.MediaLibrary.Add(fileElem)
                        If (Me.MediaLibrary.Count Mod 250) = 0 Then Me.ShowMediaLibraryCount()
                    End If
                Next
            Catch ex As UnauthorizedAccessException
                Exit Sub  ' No rights to read this directory's files; skip it
            Catch ex As Exception
                Exit Sub  ' Any other file enumeration error; skip it
            End Try

            ' Recurse into subdirectories
            Dim subDirs As String()
            Try
                subDirs = System.IO.Directory.GetDirectories(DirName)
            Catch ex As Exception
                Exit Sub   '* If there aren't any, or if there's an error reading them, skip this subdir lookup.
            End Try

            For Each subDir As String In subDirs
                If System.IO.Path.GetFileName(subDir) <> ".xvpics" Then
                    Me.ProcessMediaDir(subDir)
                End If
            Next

        End Sub

        Private Function IsImageFile(ByVal fnam As String) As Boolean
            If fnam Is Nothing OrElse fnam = "" Then Return False
            Return Me.ImageFileExtensions.Contains(Path.GetExtension(fnam))
        End Function

        Private Function IsVideoFile(ByVal fnam As String) As Boolean
            If fnam Is Nothing OrElse fnam = "" Then Return False
            Return Me.VideoFileExtensions.Contains(Path.GetExtension(fnam))
        End Function

        Private Function IsMediaFile(ByVal fnam As String) As Boolean
            If fnam Is Nothing OrElse fnam = "" Then Return False
            Return Me.MediaFileExtensions.Contains(Path.GetExtension(fnam))
        End Function
    End Class
End Namespace
