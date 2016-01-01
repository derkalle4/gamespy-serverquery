Public Class ManagedGamespyClient
    Inherits GamespyClient

    Private Const defaultPort = 28910
    Public Property GameName As String
    Public Property MasterServerName As String
    Public Property MasterServerKey As String

    Friend Overrides Sub OnDataInput(data() As Byte)
        MyBase.OnDataInput(data)
    End Sub

    Friend Overrides Sub OnSendData(data() As Byte)
        MyBase.OnSendData(data)
    End Sub

    Public Function GetServerList(Optional ByVal desiredParams() As String = Nothing) As List(Of GamespyGameserver)
        Dim requestPacket As New ListRequestPacket
        If Not desiredParams Is Nothing Then requestPacket.DesiredParams = desiredParams
        requestPacket.Key = Me.makeKey
        requestPacket.GameName = GameName
        requestPacket.MasterServerName = MasterServerName
        requestPacket.MasterServerKey = MasterServerKey

        Me.SendPacket(requestPacket)

        Return requestPacket.ServerList

    End Function

    Public Overrides Sub Connect()
        MyBase.Port = defaultPort
        MyBase.Connect()
    End Sub

    Private Sub SendPacket(ByVal packet As GamespyPacket)
        packet.ReceiveBytes(MyBase.Send(packet.Compile))

    End Sub

    Private Function makeKey() As Byte()
        Dim key(7) As Byte
        Dim rand As New Random
        For i = 0 To 7
            key(i) = rand.Next(1, 10)
        Next
        Return key
    End Function

End Class
