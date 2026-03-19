Namespace JANIS
    Partial Public Class fmMain
        '=================================================================================================
        '* JANIS Preferences Management and I/O

        Private Sub pnlDefaultTextColorLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlDefaultTextColorLeft1.Click, pnlDefaultTextColorLeft2.Click, pnlDefaultTextColorLeft3.Click, pnlDefaultTextColorLeft4.Click, pnlDefaultTextColorLeft5.Click, pnlDefaultTextColorLeft6.Click
            Me.lblDefaultColorLeft.BackColor = DirectCast(sender, Panel).BackColor
        End Sub
        Private Sub pnlDefaultTextColorRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pnlDefaultTextColorRight1.Click, pnlDefaultTextColorRight2.Click, pnlDefaultTextColorRight3.Click, pnlDefaultTextColorRight4.Click, pnlDefaultTextColorRight5.Click, pnlDefaultTextColorRight6.Click
            Me.lblDefaultColorRight.BackColor = DirectCast(sender, Panel).BackColor
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
            Dim checked As Boolean = DirectCast(sender, CheckBox).Checked
            Me.tbDefaultImageFile.Enabled = checked
            Me.btnChooseDefaultImage.Enabled = checked
            If checked Then
                Me.tbDefaultImageFile.BackColor = Color.FromName("Control")
            Else
                Me.tbDefaultImageFile.BackColor = Color.FromName("ControlDark")
            End If
        End Sub
        Private Sub btnChooseDefaultImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseDefaultImage.Click
            Dim fn As String = SelectMediaFilename()
            If fn <> "" Then Me.tbDefaultImageFile.Text = fn
        End Sub

        Private Sub cbLoadDefaultHB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbLoadDefaultHB.CheckedChanged
            Dim checked As Boolean = DirectCast(sender, CheckBox).Checked
            Me.tbDefaultHBFile.Enabled = checked
            Me.btnChooseDefaultHB.Enabled = checked
            If checked Then
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
            Dim checked As Boolean = DirectCast(sender, CheckBox).Checked
            Me.tbDefaultSlideShow.Enabled = checked
            Me.btnChooseDefaultSlideShow.Enabled = checked
            Me.cbPlaySlidesAtStart.Enabled = checked
            If checked Then
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
            Me.SavePrefsToFile(PREFS_FILE)
        End Sub
        Private Sub btnRevertPrefs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevertPrefs.Click
            '* The last saved values are restored.
            Me.ApplyPrefsToUI(Me.SavedPrefs)
        End Sub

        Private Function PrefsChanged() As Boolean
            If Me.CurrentPrefs.LeftTeamColor <> Me.SavedPrefs.LeftTeamColor Then Return True
            If Me.CurrentPrefs.RightTeamColor <> Me.SavedPrefs.RightTeamColor Then Return True
            If Me.CurrentPrefs.DefaultFontSize <> Me.SavedPrefs.DefaultFontSize Then Return True
            If Me.CurrentPrefs.ShadowsEnabled <> Me.SavedPrefs.ShadowsEnabled Then Return True
            If Me.CurrentPrefs.DefaultImageDir <> Me.SavedPrefs.DefaultImageDir Then Return True
            If Me.CurrentPrefs.DefaultImageFile <> Me.SavedPrefs.DefaultImageFile Then Return True
            If Me.CurrentPrefs.DisplayDefaultImage <> Me.SavedPrefs.DisplayDefaultImage Then Return True
            If Me.CurrentPrefs.DefaultHBFile <> Me.SavedPrefs.DefaultHBFile Then Return True
            If Me.CurrentPrefs.LoadDefaultHB <> Me.SavedPrefs.LoadDefaultHB Then Return True
            If Me.CurrentPrefs.DefaultSlideDelay <> Me.SavedPrefs.DefaultSlideDelay Then Return True
            If Me.CurrentPrefs.DefaultSlideShow <> Me.SavedPrefs.DefaultSlideShow Then Return True
            If Me.CurrentPrefs.PlaySlidesAtStart <> Me.SavedPrefs.PlaySlidesAtStart Then Return True
            If Me.CurrentPrefs.LoadDefaultSlides <> Me.SavedPrefs.LoadDefaultSlides Then Return True
            If Me.CurrentPrefs.DefaultCountdownHours <> Me.SavedPrefs.DefaultCountdownHours Then Return True
            If Me.CurrentPrefs.DefaultCountdownMinutes <> Me.SavedPrefs.DefaultCountdownMinutes Then Return True
            If Me.CurrentPrefs.DefaultCountdownSeconds <> Me.SavedPrefs.DefaultCountdownSeconds Then Return True
            Return False
        End Function

        '=================================================================================================
        '* PREFERENCES FILE I/O  (JSON format as of v5)
        '*
        '* Colors cannot be serialized directly by DataContractJsonSerializer, so we store them as
        '* their ARGB integer value — the same representation the old .ini format used.
        '*
        '* Legacy migration: if JANIS.json does not exist but JANIS.ini does, the old positional
        '* text file is read automatically and the result is immediately saved as JANIS.json so the
        '* upgrade is transparent to the user.

        ' ── Thin DTO (Data Transfer Object) used only for JSON serialization ───────────────────────
        <System.Runtime.Serialization.DataContract()>
        Private Class PrefsJson
            <System.Runtime.Serialization.DataMember()> Public LeftTeamColorArgb As Integer
            <System.Runtime.Serialization.DataMember()> Public RightTeamColorArgb As Integer
            <System.Runtime.Serialization.DataMember()> Public DefaultFontSize As String
            <System.Runtime.Serialization.DataMember()> Public ShadowsEnabled As Boolean
            <System.Runtime.Serialization.DataMember()> Public DefaultImageDir As String
            <System.Runtime.Serialization.DataMember()> Public DefaultImageFile As String
            <System.Runtime.Serialization.DataMember()> Public DisplayDefaultImage As Boolean
            <System.Runtime.Serialization.DataMember()> Public DefaultHBFile As String
            <System.Runtime.Serialization.DataMember()> Public LoadDefaultHB As Boolean
            <System.Runtime.Serialization.DataMember()> Public DefaultSlideDelay As Integer
            <System.Runtime.Serialization.DataMember()> Public DefaultSlideShow As String
            <System.Runtime.Serialization.DataMember()> Public PlaySlidesAtStart As Boolean
            <System.Runtime.Serialization.DataMember()> Public LoadDefaultSlides As Boolean
            <System.Runtime.Serialization.DataMember()> Public DefaultCountdownHours As Integer
            <System.Runtime.Serialization.DataMember()> Public DefaultCountdownMinutes As Integer
            <System.Runtime.Serialization.DataMember()> Public DefaultCountdownSeconds As Integer
        End Class

        ' ── Convert between Preferences and the JSON DTO ───────────────────────────────────────────
        Private Shared Function PrefsToDto(ByVal p As Preferences) As PrefsJson
            Dim dto As New PrefsJson()
            dto.LeftTeamColorArgb = p.LeftTeamColor.ToArgb()
            dto.RightTeamColorArgb = p.RightTeamColor.ToArgb()
            dto.DefaultFontSize = p.DefaultFontSize
            dto.ShadowsEnabled = p.ShadowsEnabled
            dto.DefaultImageDir = p.DefaultImageDir
            dto.DefaultImageFile = p.DefaultImageFile
            dto.DisplayDefaultImage = p.DisplayDefaultImage
            dto.DefaultHBFile = p.DefaultHBFile
            dto.LoadDefaultHB = p.LoadDefaultHB
            dto.DefaultSlideDelay = CInt(p.DefaultSlideDelay)
            dto.DefaultSlideShow = p.DefaultSlideShow
            dto.PlaySlidesAtStart = p.PlaySlidesAtStart
            dto.LoadDefaultSlides = p.LoadDefaultSlides
            dto.DefaultCountdownHours = CInt(p.DefaultCountdownHours)
            dto.DefaultCountdownMinutes = CInt(p.DefaultCountdownMinutes)
            dto.DefaultCountdownSeconds = CInt(p.DefaultCountdownSeconds)
            Return dto
        End Function

        Private Shared Function DtoToPrefs(ByVal dto As PrefsJson) As Preferences
            Dim p As New Preferences()
            p.LeftTeamColor = Color.FromArgb(dto.LeftTeamColorArgb)
            p.RightTeamColor = Color.FromArgb(dto.RightTeamColorArgb)
            p.DefaultFontSize = dto.DefaultFontSize
            p.ShadowsEnabled = dto.ShadowsEnabled
            p.DefaultImageDir = dto.DefaultImageDir
            p.DefaultImageFile = dto.DefaultImageFile
            p.DisplayDefaultImage = dto.DisplayDefaultImage
            p.DefaultHBFile = dto.DefaultHBFile
            p.LoadDefaultHB = dto.LoadDefaultHB
            p.DefaultSlideDelay = dto.DefaultSlideDelay
            p.DefaultSlideShow = dto.DefaultSlideShow
            p.PlaySlidesAtStart = dto.PlaySlidesAtStart
            p.LoadDefaultSlides = dto.LoadDefaultSlides
            p.DefaultCountdownHours = dto.DefaultCountdownHours
            p.DefaultCountdownMinutes = dto.DefaultCountdownMinutes
            p.DefaultCountdownSeconds = dto.DefaultCountdownSeconds
            Return p
        End Function

        ' ── Main load entry point ──────────────────────────────────────────────────────────────────
        Private Sub LoadPrefsFromFile(ByVal filename As String)
            '* filename is the JSON path (JANIS.json).
            '* If it doesn't exist, check for the legacy JANIS.ini alongside it.
            '* If that exists, migrate it. If neither exists, write factory defaults.

            Dim p As Preferences = Nothing

            If Not System.IO.File.Exists(filename) Then
                Dim legacyFile As String = System.IO.Path.ChangeExtension(filename, ".ini")
                If System.IO.File.Exists(legacyFile) Then
                    p = Me.LoadLegacyPrefsFromFile(legacyFile)
                    If p IsNot Nothing Then
                        '* Auto-convert: immediately persist as JSON so we won't read .ini again
                        '* Use WritePrefsToFile directly — SavePrefsToFile's change-guard would
                        '* bail out here because SavedPrefs hasn't been set yet.
                        Me.CurrentPrefs = p
                        Me.WritePrefsToFile(filename, p)
                    End If
                End If

                If p Is Nothing Then
                    '* Truly first run — write factory defaults and we're done
                    '* Same reason: bypass the change-guard.
                    Dim defaults As New Preferences()
                    Me.WritePrefsToFile(filename, defaults)
                    Me.CurrentPrefs = defaults
                    Me.ApplyPrefsToUI(defaults)
                    Me.StorePrefs()
                    Me.AllScreensToFront()
                    Return
                End If
            Else
                p = Me.LoadJsonPrefsFromFile(filename)
            End If

            If p Is Nothing Then
                Me.SetDefaultPrefs()
            Else
                Me.CurrentPrefs = p
                Me.ApplyPrefsToUI(p)
            End If

            Me.StorePrefs()
            Me.AllScreensToFront()
        End Sub

        ' ── Read the JSON format ───────────────────────────────────────────────────────────────────
        Private Function LoadJsonPrefsFromFile(ByVal filename As String) As Preferences
            Try
                Dim serializer As New System.Runtime.Serialization.Json.DataContractJsonSerializer(GetType(PrefsJson))
                Using stream As New System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                    Dim dto As PrefsJson = DirectCast(serializer.ReadObject(stream), PrefsJson)
                    Return DtoToPrefs(dto)
                End Using
            Catch ex As Exception
                MessageBox.Show(Me,
                    "An error occurred reading preferences file '" & filename & "'." & vbCrLf &
                    "Factory defaults will be used." & vbCrLf & vbCrLf &
                    "Detail: " & ex.Message,
                    "Preferences File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return Nothing
            End Try
        End Function

        ' ── Read the legacy positional-text format (JANIS.ini) ────────────────────────────────────
        Private Function LoadLegacyPrefsFromFile(ByVal filename As String) As Preferences
            '* This reader exists solely for one-time migration of pre-v5 preference files.
            '* The .ini format is a sequence of bare values, one per line, in a fixed order.
            '* Fields added in later versions (countdown, shadows) were appended at the end,
            '* so we guard every late addition with a Nothing check before using it.
            Try
                Dim lines As String() = System.IO.File.ReadAllLines(filename)
                Dim i As Integer = 0

                Dim ReadLine As Func(Of String) = Function()
                                                      If i < lines.Length Then
                                                          Dim val As String = lines(i)
                                                          i += 1
                                                          Return val
                                                      End If
                                                      Return Nothing
                                                  End Function

                Dim p As New Preferences()
                p.LeftTeamColor = Color.FromArgb(CInt(ReadLine()))
                p.RightTeamColor = Color.FromArgb(CInt(ReadLine()))
                p.DefaultFontSize = ReadLine()
                p.DefaultImageDir = ReadLine()
                p.DisplayDefaultImage = (ReadLine() = "True")
                p.DefaultImageFile = ReadLine()
                p.LoadDefaultHB = (ReadLine() = "True")
                p.DefaultHBFile = ReadLine()
                p.DefaultSlideDelay = CInt(ReadLine())
                p.LoadDefaultSlides = (ReadLine() = "True")
                p.DefaultSlideShow = ReadLine()
                p.PlaySlidesAtStart = (ReadLine() = "True")

                '* These fields were added after the initial release
                Dim countdownHours As String = ReadLine()
                If countdownHours IsNot Nothing Then
                    p.DefaultCountdownHours = CInt(countdownHours)
                    p.DefaultCountdownMinutes = CInt(ReadLine())
                    p.DefaultCountdownSeconds = CInt(ReadLine())
                End If

                Dim shadowsEnabled As String = ReadLine()
                If shadowsEnabled IsNot Nothing Then p.ShadowsEnabled = (shadowsEnabled = "True")

                Return p

            Catch ex As Exception
                MessageBox.Show(Me,
                    "An error occurred reading legacy preferences file '" & filename & "'." & vbCrLf &
                    "Factory defaults will be used." & vbCrLf & vbCrLf &
                    "Detail: " & ex.Message,
                    "Preferences Migration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return Nothing
            End Try
        End Function

        ' ── Save in JSON format ────────────────────────────────────────────────────────────────────
        Private Sub SavePrefsToFile(ByVal filename As String)
            '* SavePrefsToFile: guarded — skips write if nothing has changed.
            '* Called from normal UI save paths (Save button, closing dialog).
            CurrentPrefs = ReadPrefsFromUI()
            If Not PrefsChanged() Then Exit Sub

            Dim RebuildMediaLibrary As Boolean = (CurrentPrefs.DefaultImageDir <> SavedPrefs.DefaultImageDir)
            Me.WritePrefsToFile(filename, CurrentPrefs)

            Me.StorePrefs()
            Me.AllScreensToFront()
            If RebuildMediaLibrary Then
                Me.ClearMediaLibrary()
                Me.BuildMediaLibrary()
            End If
        End Sub

        ' ── Unconditional JSON write — no change-guard, no UI read ────────────────────────────────
        '* Used directly by LoadPrefsFromFile for first-run and legacy migration, where
        '* SavedPrefs hasn't been populated yet and the change-guard would always bail out.
        Private Sub WritePrefsToFile(ByVal filename As String, ByVal p As Preferences)
            Try
                Dim dto As PrefsJson = PrefsToDto(p)
                Dim serializer As New System.Runtime.Serialization.Json.DataContractJsonSerializer(GetType(PrefsJson))

                '* Write to a temp file first, then replace — avoids a corrupt prefs file if we
                '* crash or lose power mid-write.
                Dim tempFile As String = filename & ".tmp"
                Using stream As New System.IO.FileStream(tempFile, System.IO.FileMode.Create, System.IO.FileAccess.Write)
                    serializer.WriteObject(stream, dto)
                End Using
                System.IO.File.Delete(filename)
                System.IO.File.Move(tempFile, filename)

            Catch ex As Exception
                MessageBox.Show(Me, "An error occurred saving preferences file '" & filename & "'." & vbCrLf &
                    "Detail: " & ex.Message,
                    "Preferences File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End Sub

        Private Sub SetDefaultPrefs()
            '* Reset the Preferences screen settings to factory defaults
            Me.CurrentPrefs = New Preferences()
            Me.ApplyPrefsToUI(Me.CurrentPrefs)
        End Sub
        Private Sub StorePrefs()
            Me.SavedPrefs = Me.CurrentPrefs.Clone()
        End Sub
        Private Sub ApplyPrefsToUI(ByVal p As Preferences)
            Me.lblDefaultColorLeft.BackColor = p.LeftTeamColor
            Me.lblDefaultColorRight.BackColor = p.RightTeamColor
            Me.tbDefaultFontSize.Text = p.DefaultFontSize
            Me.cbShadowsEnabled.Checked = p.ShadowsEnabled
            Me.tbDefaultImageDir.Text = p.DefaultImageDir
            Me.tbDefaultImageFile.Text = p.DefaultImageFile
            Me.cbDisplayDefaultImage.Checked = p.DisplayDefaultImage
            Me.tbDefaultHBFile.Text = p.DefaultHBFile
            Me.cbLoadDefaultHB.Checked = p.LoadDefaultHB
            Me.nudDefaultSlideDelay.Value = p.DefaultSlideDelay
            Me.tbDefaultSlideShow.Text = p.DefaultSlideShow
            Me.cbPlaySlidesAtStart.Checked = p.PlaySlidesAtStart
            Me.cbLoadDefaultSlides.Checked = p.LoadDefaultSlides
            Me.nudDefaultCountdownHours.Value = p.DefaultCountdownHours
            Me.nudDefaultCountdownMinutes.Value = p.DefaultCountdownMinutes
            Me.nudDefaultCountdownSeconds.Value = p.DefaultCountdownSeconds
        End Sub
        Private Function ReadPrefsFromUI() As Preferences
            Dim p As New Preferences()
            p.LeftTeamColor = Me.lblDefaultColorLeft.BackColor
            p.RightTeamColor = Me.lblDefaultColorRight.BackColor
            p.DefaultFontSize = Me.tbDefaultFontSize.Text
            p.ShadowsEnabled = Me.cbShadowsEnabled.Checked
            p.DefaultImageDir = Me.tbDefaultImageDir.Text
            p.DefaultImageFile = Me.tbDefaultImageFile.Text
            p.DisplayDefaultImage = Me.cbDisplayDefaultImage.Checked
            p.DefaultHBFile = Me.tbDefaultHBFile.Text
            p.LoadDefaultHB = Me.cbLoadDefaultHB.Checked
            p.DefaultSlideDelay = Me.nudDefaultSlideDelay.Value
            p.DefaultSlideShow = Me.tbDefaultSlideShow.Text
            p.PlaySlidesAtStart = Me.cbPlaySlidesAtStart.Checked
            p.LoadDefaultSlides = Me.cbLoadDefaultSlides.Checked
            p.DefaultCountdownHours = Me.nudDefaultCountdownHours.Value
            p.DefaultCountdownMinutes = Me.nudDefaultCountdownMinutes.Value
            p.DefaultCountdownSeconds = Me.nudDefaultCountdownSeconds.Value
            Return p
        End Function
        Private Sub ApplyPrefs()
            Me.SetTeamColor("Left", Me.lblDefaultColorLeft.BackColor)
            Me.SetTeamColor("Right", Me.lblDefaultColorRight.BackColor)
            Me.tbLeftText.BackColor = Me.lblDefaultColorLeft.BackColor
            Me.tbRightText.BackColor = Me.lblDefaultColorRight.BackColor
            Me.tbLeftFontSize.Text = Me.tbDefaultFontSize.Text
            Me.tbRightFontSize.Text = Me.tbDefaultFontSize.Text
            Me.LS.SetTextShadows(Me.cbShadowsEnabled.Checked)
            If Me.cbDisplayDefaultImage.Checked Then Me.ShowMediaFile(Me.tbDefaultImageFile.Text)
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

    End Class
End Namespace
