<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.btnRefresh = New System.Windows.Forms.ToolStripButton()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.chHost = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chSessname = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chPlayers = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chPwd = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chVariant = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chMap = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chNat = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chNATLAN = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chNATWAN = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.chNetregion = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnRefresh})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(865, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnRefresh
        '
        Me.btnRefresh.Image = Global.GameSpy_Server_Query.My.Resources.Resources.arrow_refresh
        Me.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(66, 22)
        Me.btnRefresh.Text = "Refresh"
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chHost, Me.chSessname, Me.chPlayers, Me.chPwd, Me.chVariant, Me.chMap, Me.chType, Me.chNat, Me.chNATLAN, Me.chNATWAN, Me.chNetregion})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.FullRowSelect = True
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(0, 25)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(865, 217)
        Me.ListView1.TabIndex = 1
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'chHost
        '
        Me.chHost.Text = "Host"
        Me.chHost.Width = 97
        '
        'chSessname
        '
        Me.chSessname.Text = "Session"
        Me.chSessname.Width = 83
        '
        'chPlayers
        '
        Me.chPlayers.Text = "Players"
        Me.chPlayers.Width = 82
        '
        'chPwd
        '
        Me.chPwd.Text = "Pwd"
        Me.chPwd.Width = 103
        '
        'chVariant
        '
        Me.chVariant.Text = "Variant"
        Me.chVariant.Width = 86
        '
        'chMap
        '
        Me.chMap.Text = "Map"
        '
        'chType
        '
        Me.chType.Text = "Typ"
        '
        'chNat
        '
        Me.chNat.Text = "NAT"
        Me.chNat.Width = 46
        '
        'chNATLAN
        '
        Me.chNATLAN.Text = "NAT LAN"
        '
        'chNATWAN
        '
        Me.chNATWAN.Text = "NAT WAN"
        Me.chNATWAN.Width = 83
        '
        'chNetregion
        '
        Me.chNetregion.Text = "Netregion"
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(865, 242)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "FormMain"
        Me.Text = "GS MS Query [swbfront2pc.ms6.gamespy.com]"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnRefresh As System.Windows.Forms.ToolStripButton
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents chHost As System.Windows.Forms.ColumnHeader
    Friend WithEvents chSessname As System.Windows.Forms.ColumnHeader
    Friend WithEvents chPlayers As System.Windows.Forms.ColumnHeader
    Friend WithEvents chPwd As System.Windows.Forms.ColumnHeader
    Friend WithEvents chVariant As System.Windows.Forms.ColumnHeader
    Friend WithEvents chMap As System.Windows.Forms.ColumnHeader
    Friend WithEvents chType As System.Windows.Forms.ColumnHeader
    Friend WithEvents chNat As System.Windows.Forms.ColumnHeader
    Friend WithEvents chNATLAN As System.Windows.Forms.ColumnHeader
    Friend WithEvents chNATWAN As System.Windows.Forms.ColumnHeader
    Friend WithEvents chNetregion As System.Windows.Forms.ColumnHeader

End Class
