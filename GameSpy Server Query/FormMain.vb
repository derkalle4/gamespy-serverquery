Public Class FormMain
    Private WithEvents client As ManagedGamespyClient
    Private Sub Refresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        client.Connect()
        Me.ListView1.Items.Clear()
        Dim l As List(Of GamespyGameserver) = client.GetServerList()
        For Each gs As GamespyGameserver In l
            Dim li As New ListViewItem

            li.Text &= "[" & gs.TypeID.ToString & "] " & gs.Address & ":" & gs.GamePort.ToString
            If gs.TypeID <> "21" Then
                li.SubItems.Add(gs.Hostname)

                li.SubItems.Add(gs.NumPlayers & " / " & gs.MaxPlayers)
                li.SubItems.Add(gs.Password.ToString)

                li.SubItems.Add(gs.GameVer)
                li.SubItems.Add(gs.MapName)
                li.SubItems.Add(gs.ServerType)
                If (gs.BehindNAT) Then
                    li.SubItems.Add(gs.BehindNAT.ToString)
                    li.SubItems.Add(gs.NATLANAddress.ToString & ":" & gs.NATLANPort.ToString)
                    li.SubItems.Add(gs.NATRoutingAddress.ToString)
                Else
                    li.SubItems.Add("")
                    li.SubItems.Add("")
                    li.SubItems.Add("")
                End If
                li.SubItems.Add(gs.SwbRegion.ToString)
            End If
           
            Me.ListView1.Items.Add(li)
        Next
        client.dispose()
    End Sub

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        client = New ManagedGamespyClient
        client.MasterServerKey = "hMO2d4"
        client.GameName = "swbfront2pc"
        client.MasterServerName = "swbfront2pc"
        client.Hostname = "192.168.178.35"

        'client.Hostname = "d.gameshare.me"
        Me.Text = "GS Server Query [" & Me.client.Hostname & "]"
    End Sub
End Class
