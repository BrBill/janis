Namespace JANIS
    Partial Public Class fmMain
        '=================================================================================================
        '* 5 THINGS STUFF

        Private Sub btnAddThing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddThing.Click
            If Me.tbNewThing.Text <> "" Then
                If Me.clbThings.Items.Count < MAX_THINGS Then
                    Me.clbThings.Items.Add(Me.tbNewThing.Text)
                    Me.tbNewThing.ResetText()
                Else
                    MessageBox.Show(Me, "The list has already reached its max number of Things (" & MAX_THINGS.ToString & "). If you need to add an item, remove another item first.", "Too many Things", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                End If
                Me.tbNewThing.Focus()
                Me.clbThings.SelectedIndex = -1
            End If
        End Sub
        Private Sub btnClearThings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearThings.Click
            If AskIfSure("Are you sure you want to remove all the Things from the list?") Then
                Dim i As Integer
                ' Set focus to tbNewThing to avoid index errors when
                ' focus is in substitution boxes.
                Me.tbNewThing.Focus()
                For i = 0 To MAX_THINGS
                    Me.ThingSubs(i) = ""
                Next
                Me.clbThings.Items.Clear()
                Me.tbCurrentThing.Visible = False
                Me.tbSubstitutions.Visible = False
                Me.btnShowThingLeft.Visible = False
            End If
            'Me.Select()
        End Sub
        Private Sub btnRemoveThing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveThing.Click
            If (Me.clbThings.SelectedIndex >= 0) Then
                Dim prompt As String = "Are you sure you want to remove Thing: '" & Me.clbThings.SelectedItem.ToString & "' ?"
                If AskIfSure(prompt) Then
                    ' Set focus to tbNewThing to avoid index errors when
                    ' focus is in substitution boxes.
                    Me.tbNewThing.Focus()
                    Dim i As Integer
                    For i = Me.clbThings.SelectedIndex To (Me.clbThings.Items.Count - 1)
                        Me.ThingSubs(i) = Me.ThingSubs(i + 1)
                    Next
                    Me.clbThings.Items.RemoveAt(Me.clbThings.SelectedIndex())
                End If
            End If
        End Sub

        Private Sub clbThings_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clbThings.SelectedIndexChanged
            Dim clbox As CheckedListBox = DirectCast(sender, CheckedListBox)
            If clbox.SelectedIndex >= 0 Then
                Me.tbCurrentThing.Text = clbox.SelectedItem.ToString
                Me.tbSubstitutions.Text = Me.ThingSubs(CInt(clbox.SelectedIndex))
                Me.tbCurrentThing.Visible = True
                Me.tbSubstitutions.Visible = True
                Me.btnShowThingLeft.Visible = True
                Me.clbThings.Hide()
                Me.tbSubstitutions.Select(Me.tbSubstitutions.TextLength, 0)  '* deselect all text here
                Me.clbThings.Show()
                Me.tbSubstitutions.Focus()
            Else
                Me.tbCurrentThing.Visible = False
                Me.tbSubstitutions.Visible = False
                Me.btnShowThingLeft.Visible = False
                Me.tbNewThing.Focus()
            End If
        End Sub

        Private Sub btnThingUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnThingUp.Click
            Dim sel As Integer = Me.clbThings.SelectedIndex
            If sel > 0 Then
                Dim chk As Boolean = Me.clbThings.CheckedIndices.Contains(sel)
                Dim s As String = Me.ThingSubs(sel - 1)
                Me.ThingSubs(sel - 1) = Me.ThingSubs(sel)
                Me.ThingSubs(sel) = s

                Me.clbThings.Items.Insert(sel - 1, Me.clbThings.SelectedItem)
                If chk Then Me.clbThings.SetItemCheckState(sel - 1, CheckState.Checked)
                Me.clbThings.Items.RemoveAt(sel + 1)
                Me.clbThings.SelectedIndex = sel - 1
            End If
        End Sub
        Private Sub btnThingDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnThingDown.Click
            Dim sel As Integer = Me.clbThings.SelectedIndex
            If (sel < (Me.clbThings.Items.Count - 1)) And (sel >= 0) Then
                Dim chk As Boolean = Me.clbThings.CheckedIndices.Contains(sel)
                Dim s As String = Me.ThingSubs(sel)
                Me.ThingSubs(sel) = Me.ThingSubs(sel + 1)
                Me.ThingSubs(sel + 1) = s

                Me.clbThings.Items.Insert(sel + 2, Me.clbThings.Items.Item(sel))
                If chk Then Me.clbThings.SetItemCheckState(sel + 2, CheckState.Checked)
                Me.clbThings.Items.RemoveAt(sel)
                Me.clbThings.SelectedIndex = sel + 1
            End If
        End Sub

        Private Sub tbSubstitutions_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbSubstitutions.Leave
            If Me.clbThings.SelectedIndex >= 0 Then
                Me.ThingSubs(Me.clbThings.SelectedIndex) = DirectCast(sender, TextBox).Text
            End If
        End Sub

        Private Sub tbCurrentThing_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbCurrentThing.Leave
            Dim sel As Integer = Me.clbThings.SelectedIndex
            If sel >= 0 Then
                Me.clbThings.Items(sel) = DirectCast(sender, TextBox).Text
            End If
        End Sub

        Private Sub btnList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnListLeft.Click
            Dim ThingsFontSize As Single = 20
            Dim i As Integer
            Dim s As String = ""
            For i = 0 To (Me.clbThings.Items.Count - 1)
                If i > 0 Then s = s & vbCrLf
                If Me.clbThings.CheckedIndices.Contains(i) Then s = s & ChrW(&H25BA)
                s = s & (i + 1).ToString & ". " & Me.clbThings.Items.Item(i).ToString
            Next

            If (Me.clbThings.Items.Count > 4) Then
                If (s.Length > 200) Then
                    ThingsFontSize = 14
                ElseIf (s.Length > 160) Or (Me.clbThings.Items.Count > 8) Then
                    ThingsFontSize = 15
                ElseIf (s.Length > 130) Or (Me.clbThings.Items.Count > 6) Then
                    ThingsFontSize = 18
                End If
            End If
            ' s = s & " " & s.Length.ToString

            DisplayTextScreen(Me.LS, s, Me.clbThings.BackColor, ThingsFontSize)
        End Sub
        Private Sub btnShowThing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowThingLeft.Click
            Dim s As String
            s = Me.tbCurrentThing.Text & vbCrLf & vbCrLf & Me.tbSubstitutions.Text
            DisplayTextScreen(Me.LS, s, Me.clbThings.BackColor, 19)
        End Sub
        Private Sub radioThingColor_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioThingColorLeft.CheckedChanged, radioThingColorRight.CheckedChanged
            Dim radbtn As RadioButton = DirectCast(sender, RadioButton)
            Me.clbThings.BackColor = radbtn.BackColor
            Me.tbSubstitutions.BackColor = radbtn.BackColor
            Me.tbCurrentThing.BackColor = radbtn.BackColor
        End Sub

        Private Sub tbNewThing_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNewThing.Enter
            Me.AcceptButton = Me.btnAddThing
        End Sub
        Private Sub tbNewThing_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbNewThing.Leave
            Me.AcceptButton = Nothing
        End Sub

    End Class
End Namespace

