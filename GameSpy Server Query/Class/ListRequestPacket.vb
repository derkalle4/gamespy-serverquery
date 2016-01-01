Public Class ListRequestPacket
    Inherits GamespyPacket

    Public Property DesiredParams As String() = {"hostname", "gamemode", "mapname", "gamever", "numplayers", "maxplayers", "gametype", "session", "prevsession", "swbregion", "servertype", "password"}
    Public Property FilterExpression As String = ""
    Public Property RequestType As Byte = 1
    Public Property ServerList As List(Of GamespyGameserver)

    Public Overrides Function Compile() As Byte()
        '0 len 0 1 3 0 0 0 0 gamename 0 msname 0 key filter 0 params 0 0 0 0 128 244 1 0 0
        Dim packetBase() As Byte = {}

        packetBase = ConcatArray({0, &HFF, 0, 1, 3, 0, 0, 0, 0}, packetBase)             '<< Länge
        packetBase = ConcatArray(Me.GetBytes(Me.GameName), packetBase, 0)                '<< Gamename
        packetBase = ConcatArray(Me.GetBytes(Me.MasterServerName), packetBase, 0)        '<< MsName
        packetBase = ConcatArray(Me.Key, packetBase)                                     '<< Key
        packetBase = ConcatArray(Me.GetBytes(Me.FilterExpression), packetBase, 0)        '<< Filter
        packetBase = ConcatArray(Me.GetBytes(Me.BuildParamString), packetBase)           '<< Parameter
        packetBase = ConcatArray({0, 0, 0, 0, Me.RequestType, 0, 0}, packetBase)         '<< Typ
        packetBase(1) = packetBase.Length - 2

        Return packetBase
    End Function

    Private Function BuildParamString() As String
        Dim res As String = String.Empty
        For Each str As String In Me.DesiredParams
            res &= "\" & str
        Next
        Return res
    End Function
    Private Function buildNiceString(ByVal data() As Byte) As String
        Dim line1 As String = String.Empty
        Dim line2 As String = String.Empty

        For i = 0 To data.Length - 1
            Dim charAsString As String = data(i).ToString
            If charAsString.Length = 1 Then
                line1 &= "00"
            ElseIf charAsString.Length = 2 Then
                line1 &= "0"
            End If
            line1 &= charAsString & ":"

            line2 &= System.Text.Encoding.ASCII.GetString({data(i)}) & "   "
        Next
        Return line1 & vbCrLf & line2
    End Function
    Friend Overrides Sub OnReceive(data() As Byte)
        Dim dbg As String = buildNiceString(data)
        Dim str As String = System.Text.Encoding.ASCII.GetString(data)
        'Dim cache As String
        'For Each b As Byte In Response
        ' cache &= b.ToString & ":"
        ' Next
        Try
            Dim parameterReference(data(6) - 1) As String   'Liste mit Parametern

            Dim foundParameters As Byte = 0     'Gefundene Trennzeichen
            Dim serverOffset As Int32 = 0       'Offset für ersten Server
            Dim lastOffset As Int32 = 8

            For serverOffset = 0 To data.Length - 3
                If data(serverOffset) = 0 And data(serverOffset + 1) = 0 And serverOffset > 4 Then
                    Dim param(serverOffset - lastOffset - 1) As Byte

                    Array.Copy(data, lastOffset, param, 0, param.Length)
                    parameterReference(foundParameters) = System.Text.Encoding.ASCII.GetString(param)
                    serverOffset += 2
                    lastOffset = serverOffset
                    foundParameters += 1
                    If (foundParameters = parameterReference.Length) Then Exit For
                End If
            Next

            Me.ServerList = New List(Of GamespyGameserver)

            While serverOffset < data.Length And serverOffset > 0
                Dim server As New GamespyGameserver
                serverOffset = server.ParseBytes(data, serverOffset, parameterReference)
                If serverOffset > 0 Then
                    Me.ServerList.Add(server)
                End If
            End While
        Catch ex As Exception
            MsgBox("")
        End Try
      

    End Sub
End Class
