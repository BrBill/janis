Public Class fmColorDialog
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents Panel8 As System.Windows.Forms.Panel
    Friend WithEvents Panel9 As System.Windows.Forms.Panel
    Friend WithEvents Panel10 As System.Windows.Forms.Panel
    Friend WithEvents Panel11 As System.Windows.Forms.Panel
    Friend WithEvents Panel12 As System.Windows.Forms.Panel
    Friend WithEvents Panel13 As System.Windows.Forms.Panel
    Friend WithEvents Panel14 As System.Windows.Forms.Panel
    Friend WithEvents Panel15 As System.Windows.Forms.Panel
    Friend WithEvents Panel16 As System.Windows.Forms.Panel
    Friend WithEvents Panel17 As System.Windows.Forms.Panel
    Friend WithEvents Panel18 As System.Windows.Forms.Panel
    Friend WithEvents Panel19 As System.Windows.Forms.Panel
    Friend WithEvents Panel20 As System.Windows.Forms.Panel
    Friend WithEvents Panel21 As System.Windows.Forms.Panel
    Friend WithEvents Panel22 As System.Windows.Forms.Panel
    Friend WithEvents Panel23 As System.Windows.Forms.Panel
    Friend WithEvents Panel24 As System.Windows.Forms.Panel
    Friend WithEvents Panel25 As System.Windows.Forms.Panel
    Friend WithEvents Panel26 As System.Windows.Forms.Panel
    Friend WithEvents Panel27 As System.Windows.Forms.Panel
    Friend WithEvents Panel28 As System.Windows.Forms.Panel
    Friend WithEvents Panel29 As System.Windows.Forms.Panel
    Friend WithEvents Panel30 As System.Windows.Forms.Panel
    Friend WithEvents Panel31 As System.Windows.Forms.Panel
    Friend WithEvents Panel32 As System.Windows.Forms.Panel
    Friend WithEvents Panel33 As System.Windows.Forms.Panel
    Friend WithEvents Panel34 As System.Windows.Forms.Panel
    Friend WithEvents Panel35 As System.Windows.Forms.Panel
    Friend WithEvents Panel36 As System.Windows.Forms.Panel
    Friend WithEvents Panel37 As System.Windows.Forms.Panel
    Friend WithEvents Panel38 As System.Windows.Forms.Panel
    Friend WithEvents Panel39 As System.Windows.Forms.Panel
    Friend WithEvents Panel40 As System.Windows.Forms.Panel
    Friend WithEvents Panel41 As System.Windows.Forms.Panel
    Friend WithEvents Panel42 As System.Windows.Forms.Panel
    Friend WithEvents Panel43 As System.Windows.Forms.Panel
    Friend WithEvents Panel44 As System.Windows.Forms.Panel
    Friend WithEvents Panel45 As System.Windows.Forms.Panel
    Friend WithEvents Panel46 As System.Windows.Forms.Panel
    Friend WithEvents Panel47 As System.Windows.Forms.Panel
    Friend WithEvents Panel48 As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents grp1 As System.Windows.Forms.GroupBox
    Friend WithEvents pnlSelected As System.Windows.Forms.Panel
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.grp1 = New System.Windows.Forms.GroupBox()
        Me.pnlSelected = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.Panel9 = New System.Windows.Forms.Panel()
        Me.Panel10 = New System.Windows.Forms.Panel()
        Me.Panel11 = New System.Windows.Forms.Panel()
        Me.Panel12 = New System.Windows.Forms.Panel()
        Me.Panel13 = New System.Windows.Forms.Panel()
        Me.Panel14 = New System.Windows.Forms.Panel()
        Me.Panel15 = New System.Windows.Forms.Panel()
        Me.Panel16 = New System.Windows.Forms.Panel()
        Me.Panel17 = New System.Windows.Forms.Panel()
        Me.Panel18 = New System.Windows.Forms.Panel()
        Me.Panel19 = New System.Windows.Forms.Panel()
        Me.Panel20 = New System.Windows.Forms.Panel()
        Me.Panel21 = New System.Windows.Forms.Panel()
        Me.Panel22 = New System.Windows.Forms.Panel()
        Me.Panel23 = New System.Windows.Forms.Panel()
        Me.Panel24 = New System.Windows.Forms.Panel()
        Me.Panel25 = New System.Windows.Forms.Panel()
        Me.Panel26 = New System.Windows.Forms.Panel()
        Me.Panel27 = New System.Windows.Forms.Panel()
        Me.Panel28 = New System.Windows.Forms.Panel()
        Me.Panel29 = New System.Windows.Forms.Panel()
        Me.Panel30 = New System.Windows.Forms.Panel()
        Me.Panel31 = New System.Windows.Forms.Panel()
        Me.Panel32 = New System.Windows.Forms.Panel()
        Me.Panel33 = New System.Windows.Forms.Panel()
        Me.Panel34 = New System.Windows.Forms.Panel()
        Me.Panel35 = New System.Windows.Forms.Panel()
        Me.Panel36 = New System.Windows.Forms.Panel()
        Me.Panel37 = New System.Windows.Forms.Panel()
        Me.Panel38 = New System.Windows.Forms.Panel()
        Me.Panel39 = New System.Windows.Forms.Panel()
        Me.Panel40 = New System.Windows.Forms.Panel()
        Me.Panel41 = New System.Windows.Forms.Panel()
        Me.Panel42 = New System.Windows.Forms.Panel()
        Me.Panel43 = New System.Windows.Forms.Panel()
        Me.Panel44 = New System.Windows.Forms.Panel()
        Me.Panel45 = New System.Windows.Forms.Panel()
        Me.Panel46 = New System.Windows.Forms.Panel()
        Me.Panel47 = New System.Windows.Forms.Panel()
        Me.Panel48 = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.grp1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grp1
        '
        Me.grp1.BackColor = System.Drawing.Color.Transparent
        Me.grp1.Controls.Add(Me.pnlSelected)
        Me.grp1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grp1.Location = New System.Drawing.Point(204, 8)
        Me.grp1.Name = "grp1"
        Me.grp1.Size = New System.Drawing.Size(84, 140)
        Me.grp1.TabIndex = 0
        Me.grp1.TabStop = False
        Me.grp1.Text = "Selection"
        '
        'pnlSelected
        '
        Me.pnlSelected.BackColor = System.Drawing.Color.Transparent
        Me.pnlSelected.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSelected.Location = New System.Drawing.Point(2, 20)
        Me.pnlSelected.Name = "pnlSelected"
        Me.pnlSelected.Size = New System.Drawing.Size(80, 116)
        Me.pnlSelected.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Location = New System.Drawing.Point(8, 8)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(20, 20)
        Me.Panel1.TabIndex = 1
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Location = New System.Drawing.Point(8, 32)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(20, 20)
        Me.Panel2.TabIndex = 2
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Silver
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel3.Location = New System.Drawing.Point(8, 56)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(20, 20)
        Me.Panel3.TabIndex = 3
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Gray
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel4.Location = New System.Drawing.Point(8, 80)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(20, 20)
        Me.Panel4.TabIndex = 4
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel5.Location = New System.Drawing.Point(8, 104)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(20, 20)
        Me.Panel5.TabIndex = 5
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.Black
        Me.Panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel6.Location = New System.Drawing.Point(8, 128)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(20, 20)
        Me.Panel6.TabIndex = 6
        '
        'Panel7
        '
        Me.Panel7.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel7.Location = New System.Drawing.Point(32, 8)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(20, 20)
        Me.Panel7.TabIndex = 7
        '
        'Panel8
        '
        Me.Panel8.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Panel8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel8.Location = New System.Drawing.Point(32, 32)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(20, 20)
        Me.Panel8.TabIndex = 8
        '
        'Panel9
        '
        Me.Panel9.BackColor = System.Drawing.Color.Red
        Me.Panel9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel9.Location = New System.Drawing.Point(32, 56)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(20, 20)
        Me.Panel9.TabIndex = 9
        '
        'Panel10
        '
        Me.Panel10.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Panel10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel10.Location = New System.Drawing.Point(32, 80)
        Me.Panel10.Name = "Panel10"
        Me.Panel10.Size = New System.Drawing.Size(20, 20)
        Me.Panel10.TabIndex = 10
        '
        'Panel11
        '
        Me.Panel11.BackColor = System.Drawing.Color.Maroon
        Me.Panel11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel11.Location = New System.Drawing.Point(32, 104)
        Me.Panel11.Name = "Panel11"
        Me.Panel11.Size = New System.Drawing.Size(20, 20)
        Me.Panel11.TabIndex = 11
        '
        'Panel12
        '
        Me.Panel12.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Panel12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel12.Location = New System.Drawing.Point(32, 128)
        Me.Panel12.Name = "Panel12"
        Me.Panel12.Size = New System.Drawing.Size(20, 20)
        Me.Panel12.TabIndex = 12
        '
        'Panel13
        '
        Me.Panel13.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel13.Location = New System.Drawing.Point(56, 8)
        Me.Panel13.Name = "Panel13"
        Me.Panel13.Size = New System.Drawing.Size(20, 20)
        Me.Panel13.TabIndex = 13
        '
        'Panel14
        '
        Me.Panel14.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Panel14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel14.Location = New System.Drawing.Point(56, 32)
        Me.Panel14.Name = "Panel14"
        Me.Panel14.Size = New System.Drawing.Size(20, 20)
        Me.Panel14.TabIndex = 14
        '
        'Panel15
        '
        Me.Panel15.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Panel15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel15.Location = New System.Drawing.Point(56, 56)
        Me.Panel15.Name = "Panel15"
        Me.Panel15.Size = New System.Drawing.Size(20, 20)
        Me.Panel15.TabIndex = 15
        '
        'Panel16
        '
        Me.Panel16.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Panel16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel16.Location = New System.Drawing.Point(56, 80)
        Me.Panel16.Name = "Panel16"
        Me.Panel16.Size = New System.Drawing.Size(20, 20)
        Me.Panel16.TabIndex = 16
        '
        'Panel17
        '
        Me.Panel17.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Panel17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel17.Location = New System.Drawing.Point(56, 104)
        Me.Panel17.Name = "Panel17"
        Me.Panel17.Size = New System.Drawing.Size(20, 20)
        Me.Panel17.TabIndex = 17
        '
        'Panel18
        '
        Me.Panel18.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Panel18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel18.Location = New System.Drawing.Point(56, 128)
        Me.Panel18.Name = "Panel18"
        Me.Panel18.Size = New System.Drawing.Size(20, 20)
        Me.Panel18.TabIndex = 18
        '
        'Panel19
        '
        Me.Panel19.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel19.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel19.Location = New System.Drawing.Point(80, 8)
        Me.Panel19.Name = "Panel19"
        Me.Panel19.Size = New System.Drawing.Size(20, 20)
        Me.Panel19.TabIndex = 19
        '
        'Panel20
        '
        Me.Panel20.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Panel20.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel20.Location = New System.Drawing.Point(80, 32)
        Me.Panel20.Name = "Panel20"
        Me.Panel20.Size = New System.Drawing.Size(20, 20)
        Me.Panel20.TabIndex = 20
        '
        'Panel21
        '
        Me.Panel21.BackColor = System.Drawing.Color.Yellow
        Me.Panel21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel21.Location = New System.Drawing.Point(80, 56)
        Me.Panel21.Name = "Panel21"
        Me.Panel21.Size = New System.Drawing.Size(20, 20)
        Me.Panel21.TabIndex = 21
        '
        'Panel22
        '
        Me.Panel22.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Panel22.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel22.Location = New System.Drawing.Point(80, 80)
        Me.Panel22.Name = "Panel22"
        Me.Panel22.Size = New System.Drawing.Size(20, 20)
        Me.Panel22.TabIndex = 22
        '
        'Panel23
        '
        Me.Panel23.BackColor = System.Drawing.Color.Olive
        Me.Panel23.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel23.Location = New System.Drawing.Point(80, 104)
        Me.Panel23.Name = "Panel23"
        Me.Panel23.Size = New System.Drawing.Size(20, 20)
        Me.Panel23.TabIndex = 23
        '
        'Panel24
        '
        Me.Panel24.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Panel24.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel24.Location = New System.Drawing.Point(80, 128)
        Me.Panel24.Name = "Panel24"
        Me.Panel24.Size = New System.Drawing.Size(20, 20)
        Me.Panel24.TabIndex = 24
        '
        'Panel25
        '
        Me.Panel25.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel25.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel25.Location = New System.Drawing.Point(104, 8)
        Me.Panel25.Name = "Panel25"
        Me.Panel25.Size = New System.Drawing.Size(20, 20)
        Me.Panel25.TabIndex = 25
        '
        'Panel26
        '
        Me.Panel26.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Panel26.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel26.Location = New System.Drawing.Point(104, 32)
        Me.Panel26.Name = "Panel26"
        Me.Panel26.Size = New System.Drawing.Size(20, 20)
        Me.Panel26.TabIndex = 26
        '
        'Panel27
        '
        Me.Panel27.BackColor = System.Drawing.Color.Lime
        Me.Panel27.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel27.Location = New System.Drawing.Point(104, 56)
        Me.Panel27.Name = "Panel27"
        Me.Panel27.Size = New System.Drawing.Size(20, 20)
        Me.Panel27.TabIndex = 27
        '
        'Panel28
        '
        Me.Panel28.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Panel28.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel28.Location = New System.Drawing.Point(104, 80)
        Me.Panel28.Name = "Panel28"
        Me.Panel28.Size = New System.Drawing.Size(20, 20)
        Me.Panel28.TabIndex = 28
        '
        'Panel29
        '
        Me.Panel29.BackColor = System.Drawing.Color.Green
        Me.Panel29.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel29.Location = New System.Drawing.Point(104, 104)
        Me.Panel29.Name = "Panel29"
        Me.Panel29.Size = New System.Drawing.Size(20, 20)
        Me.Panel29.TabIndex = 29
        '
        'Panel30
        '
        Me.Panel30.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Panel30.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel30.Location = New System.Drawing.Point(104, 128)
        Me.Panel30.Name = "Panel30"
        Me.Panel30.Size = New System.Drawing.Size(20, 20)
        Me.Panel30.TabIndex = 30
        '
        'Panel31
        '
        Me.Panel31.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel31.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel31.Location = New System.Drawing.Point(128, 8)
        Me.Panel31.Name = "Panel31"
        Me.Panel31.Size = New System.Drawing.Size(20, 20)
        Me.Panel31.TabIndex = 31
        '
        'Panel32
        '
        Me.Panel32.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel32.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel32.Location = New System.Drawing.Point(128, 32)
        Me.Panel32.Name = "Panel32"
        Me.Panel32.Size = New System.Drawing.Size(20, 20)
        Me.Panel32.TabIndex = 32
        '
        'Panel33
        '
        Me.Panel33.BackColor = System.Drawing.Color.Aqua
        Me.Panel33.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel33.Location = New System.Drawing.Point(128, 56)
        Me.Panel33.Name = "Panel33"
        Me.Panel33.Size = New System.Drawing.Size(20, 20)
        Me.Panel33.TabIndex = 33
        '
        'Panel34
        '
        Me.Panel34.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel34.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel34.Location = New System.Drawing.Point(128, 80)
        Me.Panel34.Name = "Panel34"
        Me.Panel34.Size = New System.Drawing.Size(20, 20)
        Me.Panel34.TabIndex = 34
        '
        'Panel35
        '
        Me.Panel35.BackColor = System.Drawing.Color.Teal
        Me.Panel35.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel35.Location = New System.Drawing.Point(128, 104)
        Me.Panel35.Name = "Panel35"
        Me.Panel35.Size = New System.Drawing.Size(20, 20)
        Me.Panel35.TabIndex = 35
        '
        'Panel36
        '
        Me.Panel36.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Panel36.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel36.Location = New System.Drawing.Point(128, 128)
        Me.Panel36.Name = "Panel36"
        Me.Panel36.Size = New System.Drawing.Size(20, 20)
        Me.Panel36.TabIndex = 36
        '
        'Panel37
        '
        Me.Panel37.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel37.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel37.Location = New System.Drawing.Point(152, 8)
        Me.Panel37.Name = "Panel37"
        Me.Panel37.Size = New System.Drawing.Size(20, 20)
        Me.Panel37.TabIndex = 37
        '
        'Panel38
        '
        Me.Panel38.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel38.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel38.Location = New System.Drawing.Point(152, 32)
        Me.Panel38.Name = "Panel38"
        Me.Panel38.Size = New System.Drawing.Size(20, 20)
        Me.Panel38.TabIndex = 38
        '
        'Panel39
        '
        Me.Panel39.BackColor = System.Drawing.Color.Blue
        Me.Panel39.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel39.Location = New System.Drawing.Point(152, 56)
        Me.Panel39.Name = "Panel39"
        Me.Panel39.Size = New System.Drawing.Size(20, 20)
        Me.Panel39.TabIndex = 39
        '
        'Panel40
        '
        Me.Panel40.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(176, Byte), Integer))
        Me.Panel40.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel40.Location = New System.Drawing.Point(152, 80)
        Me.Panel40.Name = "Panel40"
        Me.Panel40.Size = New System.Drawing.Size(20, 20)
        Me.Panel40.TabIndex = 40
        '
        'Panel41
        '
        Me.Panel41.BackColor = System.Drawing.Color.Navy
        Me.Panel41.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel41.Location = New System.Drawing.Point(152, 104)
        Me.Panel41.Name = "Panel41"
        Me.Panel41.Size = New System.Drawing.Size(20, 20)
        Me.Panel41.TabIndex = 41
        '
        'Panel42
        '
        Me.Panel42.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Panel42.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel42.Location = New System.Drawing.Point(152, 128)
        Me.Panel42.Name = "Panel42"
        Me.Panel42.Size = New System.Drawing.Size(20, 20)
        Me.Panel42.TabIndex = 42
        '
        'Panel43
        '
        Me.Panel43.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel43.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel43.Location = New System.Drawing.Point(176, 8)
        Me.Panel43.Name = "Panel43"
        Me.Panel43.Size = New System.Drawing.Size(20, 20)
        Me.Panel43.TabIndex = 43
        '
        'Panel44
        '
        Me.Panel44.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel44.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel44.Location = New System.Drawing.Point(176, 32)
        Me.Panel44.Name = "Panel44"
        Me.Panel44.Size = New System.Drawing.Size(20, 20)
        Me.Panel44.TabIndex = 44
        '
        'Panel45
        '
        Me.Panel45.BackColor = System.Drawing.Color.Magenta
        Me.Panel45.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel45.Location = New System.Drawing.Point(176, 56)
        Me.Panel45.Name = "Panel45"
        Me.Panel45.Size = New System.Drawing.Size(20, 20)
        Me.Panel45.TabIndex = 45
        '
        'Panel46
        '
        Me.Panel46.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel46.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel46.Location = New System.Drawing.Point(176, 80)
        Me.Panel46.Name = "Panel46"
        Me.Panel46.Size = New System.Drawing.Size(20, 20)
        Me.Panel46.TabIndex = 46
        '
        'Panel47
        '
        Me.Panel47.BackColor = System.Drawing.Color.Purple
        Me.Panel47.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel47.Location = New System.Drawing.Point(176, 104)
        Me.Panel47.Name = "Panel47"
        Me.Panel47.Size = New System.Drawing.Size(20, 20)
        Me.Panel47.TabIndex = 47
        '
        'Panel48
        '
        Me.Panel48.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Panel48.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel48.Location = New System.Drawing.Point(176, 128)
        Me.Panel48.Name = "Panel48"
        Me.Panel48.Size = New System.Drawing.Size(20, 20)
        Me.Panel48.TabIndex = 48
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(8, 160)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(108, 32)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(180, 160)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(108, 32)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.FileName = "doc1"
        '
        'fmColorDialog
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(296, 223)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.Panel48)
        Me.Controls.Add(Me.Panel47)
        Me.Controls.Add(Me.Panel46)
        Me.Controls.Add(Me.Panel45)
        Me.Controls.Add(Me.Panel44)
        Me.Controls.Add(Me.Panel43)
        Me.Controls.Add(Me.Panel42)
        Me.Controls.Add(Me.Panel41)
        Me.Controls.Add(Me.Panel40)
        Me.Controls.Add(Me.Panel39)
        Me.Controls.Add(Me.Panel38)
        Me.Controls.Add(Me.Panel37)
        Me.Controls.Add(Me.Panel36)
        Me.Controls.Add(Me.Panel35)
        Me.Controls.Add(Me.Panel34)
        Me.Controls.Add(Me.Panel33)
        Me.Controls.Add(Me.Panel32)
        Me.Controls.Add(Me.Panel31)
        Me.Controls.Add(Me.Panel30)
        Me.Controls.Add(Me.Panel29)
        Me.Controls.Add(Me.Panel28)
        Me.Controls.Add(Me.Panel27)
        Me.Controls.Add(Me.Panel26)
        Me.Controls.Add(Me.Panel25)
        Me.Controls.Add(Me.Panel24)
        Me.Controls.Add(Me.Panel23)
        Me.Controls.Add(Me.Panel22)
        Me.Controls.Add(Me.Panel21)
        Me.Controls.Add(Me.Panel20)
        Me.Controls.Add(Me.Panel19)
        Me.Controls.Add(Me.Panel18)
        Me.Controls.Add(Me.Panel17)
        Me.Controls.Add(Me.Panel16)
        Me.Controls.Add(Me.Panel15)
        Me.Controls.Add(Me.Panel14)
        Me.Controls.Add(Me.Panel13)
        Me.Controls.Add(Me.Panel12)
        Me.Controls.Add(Me.Panel11)
        Me.Controls.Add(Me.Panel10)
        Me.Controls.Add(Me.Panel9)
        Me.Controls.Add(Me.Panel8)
        Me.Controls.Add(Me.Panel7)
        Me.Controls.Add(Me.Panel6)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.grp1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(350, 250)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "fmColorDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Select Color"
        Me.TopMost = True
        Me.grp1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub Color_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Panel1.Click, Panel2.Click, Panel3.Click, Panel4.Click, Panel5.Click, Panel6.Click, Panel7.Click, Panel8.Click, Panel9.Click, Panel10.Click, Panel11.Click, Panel12.Click, Panel13.Click, Panel14.Click, Panel15.Click, Panel16.Click, Panel17.Click, Panel18.Click, Panel19.Click, Panel20.Click, Panel21.Click, Panel22.Click, Panel23.Click, Panel24.Click, Panel25.Click, Panel26.Click, Panel27.Click, Panel28.Click, Panel29.Click, Panel30.Click, Panel31.Click, Panel32.Click, Panel33.Click, Panel34.Click, Panel35.Click, Panel36.Click, Panel37.Click, Panel38.Click, Panel39.Click, Panel40.Click, Panel41.Click, Panel42.Click, Panel43.Click, Panel44.Click, Panel45.Click, Panel46.Click, Panel47.Click, Panel48.Click
        pnlSelected.BackColor = sender.BackColor
    End Sub
End Class
