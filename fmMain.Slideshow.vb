Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Threading.Tasks

Namespace JANIS
    Partial Public Class fmMain

        '=================================================================================================
        '* SLIDESHOW STUFF

        Private Sub AddToSlideList(ByVal fnams As System.Windows.Forms.ListBox.SelectedObjectCollection)
            'Yup.
            Dim fobj As Object
            For Each fobj In fnams
                Me.lbSlideList.Items.Add(Me.tvSlideFolders.SelectedNode.Name & "\" & fobj.ToString)
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
            Me.AddToSlideList(Me.lbSlideCandidates.SelectedItems)
        End Sub
        Private Sub btnClearSlideList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearSlideList.Click
            If Me.lbSlideList.Items.Count < 1 Then Return
            If Not Me.AskIfSure("Clear all slides from the list?") Then Return
            Me.StopSlideShow()
            Me.lbSlideList.Items.Clear()     '** Empty the list first
        End Sub

        Private Sub lbMediaFiles_DoubleClick(ByVal sender As Object, e As System.EventArgs) Handles lbSlideCandidates.DoubleClick
            Me.AddToSlideList(DirectCast(sender, ListBox).SelectedItems)
        End Sub
        Private Sub lbMediaFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbSlideCandidates.SelectedIndexChanged
            Me.PreviewSlideMedia()
        End Sub
        Private Sub PreviewSlideMedia()
            Static PrevSelect As String = ""
            If Me.lbSlideCandidates.SelectedItem Is Nothing Then Exit Sub
            Dim selectedItem As String = Me.lbSlideCandidates.SelectedItem.ToString
            Dim filenameToPreview As String = Me.tvSlideFolders.SelectedNode.Name & "\" & selectedItem
            If Me.lbSlideCandidates.SelectedItems.Count = 1 Then
                If filenameToPreview <> PrevSelect Then
                    PrevSelect = filenameToPreview
                    StopPreviewSlideVideo()  '* No-op if not playing

                    If IsVideoFile(filenameToPreview) Then
                        Me.PlayPreviewSlideVideo(filenameToPreview)
                    Else
                        Me.ShowPreviewSlideImage(filenameToPreview)
                    End If
                End If
            Else
                Me.ClearCurrentPictureboxImage(Me.picSlidePreview)
                Me.StopPreviewSlideVideo()
                PrevSelect = ""
            End If
        End Sub

        Private Sub StopPreviewSlideVideo()
            Me._slidePreviewVideoPlayer.Stop()
            Me._slidePreviewVideoView.Hide()
        End Sub
        Private Sub PlayPreviewSlideVideo(fnam As String)
            Me.picSlidePreview.Hide()
            Me._slidePreviewVideoView.Show()
            Try
                Dim media As New LibVLCSharp.Shared.Media(Me._libVLC, fnam, LibVLCSharp.Shared.FromType.FromPath)
                Me._slidePreviewVideoPlayer.Play(media)
                media.Dispose()
            Catch ex As Exception
                Me._slidePreviewVideoPlayer.Stop()
                Me._slidePreviewVideoView.Hide()
            End Try
        End Sub
        Private Sub ShowPreviewSlideImage(fnam As String)
            Me.StopPreviewSlideVideo()
            Me._slidePreviewVideoView.Hide()
            Me.picSlidePreview.Show()
            Try
                Dim newImg As Image = Image.FromFile(fnam)
                Me.ClearCurrentPictureboxImage(Me.picSlidePreview)
                Me.picSlidePreview.Image = newImg
            Catch
                '* If error, clear image display. I don't care what the error was about.
                Me.ClearCurrentPictureboxImage(Me.picSlidePreview)
            End Try
        End Sub

        Private Sub tvSlideFolders_Init(ByVal startPath As String)
            With Me.tvSlideFolders
                Try
                    .ImageList = New ImageList
                    .ImageList.Images.Add(My.Resources.forbidden)
                    .ImageList.Images.Add(My.Resources.harddrive)
                    .ImageList.Images.Add(My.Resources.cdrom)
                    .ImageList.Images.Add(My.Resources.web)
                    .ImageList.Images.Add(My.Resources.lock)
                    .ImageList.Images.Add(My.Resources.folder)
                    .Nodes.Clear()    '* Always start fresh
                Catch ex As Exception
                    MessageBox.Show(Me, "Could not fetch " & ex.Message, "tvSlideFolders_Init icon loading error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

                tvSlideFolders_LoadDrives()
                tvSlideFolders_SetFolder(startPath)
            End With
        End Sub
        Private Sub tvSlideFolders_LoadDrives()
            Dim driveArray As String() = Environment.GetLogicalDrives
            Dim drive As String

            For Each drive In driveArray
                Dim di As New DriveInfo(drive)
                Dim driveImage As Integer
                Select Case di.DriveType
                    Case DriveType.CDRom
                        driveImage = 2
                    Case DriveType.Network
                        driveImage = 3
                    Case DriveType.NoRootDirectory, DriveType.Unknown
                        driveImage = 0
                    Case Else
                        driveImage = 1
                End Select

                Dim driveName As String = drive.TrimEnd("\"c)
                Dim node As TreeNode = New TreeNode(driveName, driveImage, driveImage)
                node.Name = driveName

                If di.IsReady = True Then
                    node.Nodes.Add("...")
                End If

                Me.tvSlideFolders.Nodes.Add(node)
            Next
            Me.tvSlideFolders.SelectedNode = Me.tvSlideFolders.Nodes(0)
        End Sub
        Private Sub tvSlideFolders_SetFolder(ByVal folder As String)
            '* Expand the tvSlideFolders to match the supplied path, by climbing up from the beginning of the path to the leaf node.

            If String.IsNullOrEmpty(folder) OrElse (Not System.IO.Directory.Exists(folder)) Then Return

            '* we're gonna be messing about with the selected node over and over, so let's hide some stuff
            'Me.tvSlideFolders.HideSelection = True
            'Me.lbMediaFiles.Visible = False

            Dim pathChunks As String() = folder.Split(Path.DirectorySeparatorChar)
            Dim pathSoFar As String = ""
            Dim currentNodes As TreeNodeCollection = Me.tvSlideFolders.Nodes
            Dim lastFound As TreeNode = Nothing

            For Each chunk As String In pathChunks
                If pathSoFar <> "" Then pathSoFar &= Path.DirectorySeparatorChar
                pathSoFar &= chunk

                Dim found As TreeNode = Nothing
                For Each node As TreeNode In currentNodes
                    If node.Name.Equals(pathSoFar, StringComparison.OrdinalIgnoreCase) Then
                        found = node
                        Exit For
                    End If
                Next

                If found Is Nothing Then Exit For

                '* Expand this node programmatically — fires BeforeExpand which populates children
                found.Expand()
                currentNodes = found.Nodes
                lastFound = found
            Next

            If lastFound IsNot Nothing Then
                Me.tvSlideFolders.SelectedNode = lastFound
            End If
        End Sub
        Private Sub tvSlideFolders_BeforeExpand(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles tvSlideFolders.BeforeExpand
            '* Expanding the folder nodes out one level
            If e.Node.Nodes.Count > 0 Then
                If (e.Node.Nodes(0).Text = "...") And (e.Node.Nodes(0).Name = "") Then
                    e.Node.Nodes.Clear()

                    '* get the list of sub directories
                    Dim thisDir As String = e.Node.Name
                    If thisDir.EndsWith(":") Then thisDir = thisDir & "\"
                    Dim dirs As String() = Directory.GetDirectories(thisDir)

                    For Each dir As String In dirs
                        Dim di As DirectoryInfo = New DirectoryInfo(dir)
                        Dim node As TreeNode = New TreeNode(di.Name, 5, 5)

                        Try
                            node.Name = dir  '* Name the node with directory's full path for use later
                            If di.GetDirectories().Length > 0 Then  '* if the directory has sub directories add the place holder
                                node.Nodes.Add("", "...", 0, 0)
                            End If
                        Catch ex As UnauthorizedAccessException
                            '* display a locked folder icon for access denied
                            node.ImageIndex = 4
                            node.SelectedImageIndex = 4
                        Catch ex As Exception
                            MessageBox.Show(Me, ex.Message, "SlidesFolderBrowser BeforeExpand Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            node = Nothing
                        Finally
                            If Not (node Is Nothing) Then e.Node.Nodes.Add(node)
                        End Try
                    Next
                End If
            End If
        End Sub
        Private Sub tvSlideFolders_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvSlideFolders.AfterSelect
            '* Reflect the change of the selected folder in the graphics file selection & display
            Static PrevSelect As String = ""
            Dim SelPath As String = tvSlideFolders.SelectedNode.Name
            If SelPath.EndsWith(":") Then SelPath = SelPath & "\" '* in case it's the root of a drive
            If SelPath <> PrevSelect Then
                PopulateSlideCandidatesList(SelPath)
                PrevSelect = SelPath
            End If
        End Sub

        Private Sub PopulateSlideCandidatesList(Folder As String)
            '* List all the graphics and video files in the selected folder in the lbSlideCandidates control
            ' MessageBox.Show(Me, Folder)
            Me.ClearCurrentPictureboxImage(Me.picSlidePreview)
            Me.lbSlideCandidates.Items.Clear()
            If System.IO.Directory.Exists(Folder) Then
                Try
                    For Each foundfile As String In Directory.GetFiles(Folder)
                        If IsMediaFile(System.IO.Path.GetExtension(foundfile)) Then
                            Me.lbSlideCandidates.Items.Add(System.IO.Path.GetFileName(foundfile))
                            'Me.lbMediaFiles.Items(newindex)
                        End If
                    Next
                Catch ex As UnauthorizedAccessException
                    MessageBox.Show(Me, "JANIS does not have access to '" & Folder & "'.", "Permission Denied", MessageBoxButtons.OK)
                End Try

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
            Dim openDialog As New OpenFileDialog()
            With openDialog
                .Filter = "JANIS SlideShow(*.JSL)|*.JSL"
                .InitialDirectory = ROOT_SUPPORT_DIR & DEFAULT_SLIDESHOW_DIR
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
                MessageBox.Show(Me, "There are no items in the SlideShow yet.", "Nothing to Save", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                Return
            End If
            Dim sf As New SaveFileDialog()
            With sf
                .Filter = "JANIS Slideshow(*.JSL)|*.JSL"
                .InitialDirectory = ROOT_SUPPORT_DIR & DEFAULT_SLIDESHOW_DIR
                If .ShowDialog(Me) = DialogResult.OK Then
                    Try
                        Dim lines As New List(Of String)
                        For i As Integer = 0 To Me.lbSlideList.Items.Count - 1
                            lines.Add(Me.lbSlideList.Items.Item(i).ToString)
                        Next
                        System.IO.File.WriteAllLines(.FileName, lines)
                    Catch ex As Exception
                        MessageBox.Show(Me, "An error occurred saving slideshow file '" & .FileName & "'.", "File Error")
                    End Try
                End If
                .Dispose()
            End With
            Me.AllScreensToFront()
        End Sub

        Private Sub LoadSlideShow(ByVal slidefile As String)
            If slidefile = "" Then Return
            Try
                Me.lbSlideList.Items.Clear()
                For Each line As String In System.IO.File.ReadAllLines(slidefile)
                    If line <> "" Then Me.lbSlideList.Items.Add(line)
                Next
            Catch ex As Exception
                MessageBox.Show(Me, "An error occurred opening slideshow file '" & slidefile & "'.", "File Error")
            End Try
        End Sub

        Private Sub PreSelectRandomSlide()
            '* Select a random slide and buffer it into BufferedSlide.
            If Me.lbSlideList.Items.Count < 2 And (Me.BufferedSlide IsNot Nothing) Then Return
            Dim NewIndex As Integer
            Do
                NewIndex = WhammyRandomizer.Next(Me.lbSlideList.Items.Count)
            Loop While NewIndex = Me.lbSlideList.SelectedIndex
            Me.lbSlideList.SelectedIndex = NewIndex
            Try
                Me.BufferedSlide = Image.FromFile(Me.lbSlideList.SelectedItem.ToString)
            Catch ex As Exception
                Me.BufferedSlide = Nothing
            End Try
        End Sub

        Private Sub SetSlideTimerInterval()
            If Me.SlidesStatus = SLIDES_WHAMMY Then
                Me.SlideTimer.Interval = 333
            Else
                Me.SlideTimer.Interval = CInt(Me.nudDelay.Value) * 1000
            End If
        End Sub
        Private Sub StartSlideTimer()
            Me.SetSlideTimerInterval()
            Me.SlideTimer.Start()
        End Sub
        Private Sub StartSlideShow()
            '* Starts a slideshow if not running. If Whammy mode, prefetches next whammy slide and shows it.
            If Me.SlidesStatus = SLIDES_PLAYING Then Return
            If Me.lbSlideList.Items.Count < 1 Then Return
            Me.lbSlideList.SelectionMode = SelectionMode.One
            If Me.SlidesStatus = SLIDES_WHAMMY Then
                Me.PreSelectRandomSlide()

                '* PreSlideshowMuteState was already set when Whammy button was clicked
                If Me.cbMuteVideo.Enabled Then
                    Me.cbMuteVideo.Checked = True
                    Me.cbMuteVideo.Enabled = False
                End If
            ElseIf SlidesStatus <> SLIDES_PAUSED Then
                Me.lbSlideList.SelectedIndex = 0

                '* Slideshows are SILENT.
                Me.PreSlideshowMuteState = Me.cbMuteVideo.Checked
                Me.cbMuteVideo.Checked = True
                Me.cbMuteVideo.Enabled = False
            End If

            Me.ShowSelectedSlide()
            Me.SetPauseButtonColor(False)
            Me.SetPlayButtonColor(True)
            If Me.SlidesStatus <> SLIDES_WHAMMY Then Me.SlidesStatus = SLIDES_PLAYING
            Me.StartSlideTimer()
        End Sub
        Public Sub StopSlideShow()
            If Me.SlidesStatus = SLIDES_STOPPED Then Return

            Dim WhammyWasActive As Boolean = (Me.SlidesStatus = SLIDES_WHAMMY)
            Me.SlideTimer.Stop()
            Me.SetPauseButtonColor(False)
            Me.SetPlayButtonColor(False)
            Me.lbSlideList.SelectionMode = SelectionMode.MultiExtended

            Me.cbMuteVideo.Enabled = True

            Me.SlidesStatus = SLIDES_STOPPED

            If WhammyWasActive Then
                '* No worries about videos playing unmuted by accident, since Whammy doesn't play videos 
                Me.lblRemoteStatus.Text = "Audience Display"
                Me.ShowSelectedSlide()
            Else
                Me.LS.StopVideo()
                Me.VideoEventTimer.Stop()

                '* Wait up to 100 ms for the video to stop; if we don't wait, can run into slide timing issues
                Dim timeout As Integer = 0
                While Me.LS.IsVideoPlaying() AndAlso timeout < 10
                    System.Threading.Thread.Sleep(10)
                    timeout += 1
                End While
            End If

            Me.cbMuteVideo.Checked = Me.PreSlideshowMuteState
        End Sub

        Private Sub SetPauseButtonColor(ByVal pause_on As Boolean)
            With Me.btnPauseSlides
                Dim backclr As System.Drawing.Color = Me.btnStopSlides.BackColor  '* use a reference color from a button that never changes
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
                Dim backclr As System.Drawing.Color = Me.btnStopSlides.BackColor   '* use a reference color from a button that never changes
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
            With Me.lbSlideList
                If .Items.Count < 1 Then
                    Me.StopSlideShow()
                    Return
                ElseIf Me.SlidesStatus = SLIDES_WHAMMY Then
                    Me.DisplayRawImage(Me.BufferedSlide)
                    Me.PreSelectRandomSlide()                    '* Set Me.BufferedSlide to a random slide
                Else
                    Me.AdvanceOneSlide()
                    Me.ShowSelectedSlide()
                End If
            End With
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
            '** If we get here, current state is SLIDES_PLAYING
            Me.SlideTimer.Stop()
            Me.LS.StopVideo()
            Me.SlidesStatus = SLIDES_PAUSED
            Me.SetPauseButtonColor(True)
        End Sub
        Private Sub btnChangeSlide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirstSlide.Click, btnNextSlide.Click, btnPrevSlide.Click, btnLastSlide.Click
            If Me.SlidesStatus = SLIDES_STOPPED Or Me.SlidesStatus = SLIDES_WHAMMY Then Return
            If Me.SlidesStatus = SLIDES_PLAYING Then Me.SlideTimer.Stop() '* temporary stoppage
            Dim controlname As String = DirectCast(sender, Button).Name
            Select Case controlname
                Case "btnFirstSlide"
                    Me.GotoFirstSlide()
                Case "btnPrevSlide"
                    Me.BackUpOneSlide()
                Case "btnNextSlide"
                    Me.AdvanceOneSlide()
                Case "btnLastSlide"
                    Me.AdvanceToLastSlide()
            End Select
            Me.ShowSelectedSlide()
            If Me.SlidesStatus = SLIDES_PLAYING Then Me.StartSlideTimer()
        End Sub
        Private Sub ShowSelectedSlide()
            Dim KillSlideShow As Boolean = False  '* and always will be, but makes the calls clearer
            Me.ShowMediaFile(Me.lbSlideList.SelectedItem.ToString, KillSlideShow)
        End Sub
        Private Sub GotoFirstSlide()
            Me.lbSlideList.SelectedIndex = 0
        End Sub
        Private Sub BackUpOneSlide()
            With Me.lbSlideList
                If .SelectedIndex > 0 Then
                    .SelectedIndex -= 1
                Else
                    '** Roll over before the beginning
                    .SelectedIndex = .Items.Count - 1
                End If
            End With
        End Sub
        Private Sub AdvanceOneSlide()
            With Me.lbSlideList
                If .SelectedIndex < (.Items.Count - 1) Then
                    .SelectedIndex += 1
                Else
                    '** Roll over past the end
                    .SelectedIndex = 0
                End If
            End With
        End Sub
        Private Sub AdvanceToLastSlide()
            Me.lbSlideList.SelectedIndex = Me.lbSlideList.Items.Count - 1
        End Sub


        Private Sub lbSlideList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSlideList.DoubleClick
            '************************************************************************************
            '* A double click changes the slide immediately REGARDLESS OF MODE, except for WHAMMY
            '************************************************************************************
            If Me.SlidesStatus = SLIDES_WHAMMY Then Return
            If Me.SlidesStatus = SLIDES_PLAYING Then Me.SlideTimer.Stop() '* temporary stoppage
            Me.ShowMediaFile(Me.lbSlideList.SelectedItem.ToString, False)
            If Me.SlidesStatus = SLIDES_PLAYING Then StartSlideTimer()
        End Sub
        Private Sub btnWhammy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWhammy.Click
            If Me.SlidesStatus = SLIDES_WHAMMY Then Return
            If Me.lbSlideList.Items.Count < 1 Then Return

            '* If there are any videos in the slideshow list, Whammy is not allowed.
            If Me.SlideListContainsVideoFile() Then
                MessageBox.Show(Me, "The slide list contains video. Please remove all video files from the list before using WHAMMY.", "Whammy Can't Play Video", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                Return
            End If

            'If Me.SlidesStatus <> SLIDES_STOPPED Then Me.StopSlideShow()
            Me.lblRemoteStatus.Text = "WHAMMY Running"
            Me.SlidesStatus = SLIDES_WHAMMY
            Me.StartSlideShow()
            Dim WaitForm As New fmClickWait(Me)
            WaitForm.Show() '* Will automatically stop the whammy slideshow when it closes
        End Sub

        Private Function SlideListContainsVideoFile() As Boolean
            For Each filename As String In Me.lbSlideList.Items
                If Me.IsVideoFile(filename) Then
                    Return True
                End If
            Next
            Return False
        End Function

    End Class
End Namespace
