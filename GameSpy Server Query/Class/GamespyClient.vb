Imports System.Net.Sockets
Imports System.IO
Imports System.Threading
Public Class GamespyClient
    Public Property Hostname As String
    Public Property Port As Int32

    Private client As TcpClient
    Private stream As NetworkStream

    Public Event DataInput(ByVal sender As GamespyClient, ByVal data() As Byte)
    Public Event ConnectFailed(ByVal sender As GamespyClient, ByVal ex As Exception)

    Public Overridable Sub Connect()
        Me.client = New TcpClient
        Me.client.ReceiveTimeout = 5000
        Try
            Me.client.Connect(Me.Hostname, Me.Port)
        Catch ex As Exception
            RaiseEvent ConnectFailed(Me, ex)
            Return
        End Try

        Me.stream = client.GetStream()
    End Sub

    Private Function waitForResponse() As Byte()
        Dim response() As Byte = New Byte() {}
        Dim rlen As Integer = 0
        'client.ReceiveTimeout = 10

        client.ReceiveBufferSize = 3472

        While (True)
            Dim temp() As Byte = New Byte(response.Length - 1) {}
            Array.Copy(response, temp, response.Length)
            response = New Byte(temp.Length + client.ReceiveBufferSize - 1) {}
            Array.Copy(temp, response, temp.Length)

            rlen += client.GetStream.Read(response, rlen, client.ReceiveBufferSize) 'client.ReceiveBufferSize)

            If response(response.Length - 1) = 0 And response(response.Length - 2) = 0 Then
                temp = New Byte(response.Length - 1) {}
                Array.Copy(response, temp, response.Length)
                response = New Byte(rlen - 1) {}
                Array.Copy(temp, response, rlen)
                Exit While
            End If

        End While

        'Me.OnDataInput(response)
        Return response
    End Function

    Public Function Send(ByVal data() As Byte) As Byte()
        Me.OnSendData(data)
        Return Me.waitForResponse()
    End Function

    Friend Overridable Sub OnSendData(ByVal data() As Byte)
        Me.stream.Write(data, 0, data.Length)
        Me.stream.Flush()
    End Sub

    Friend Overridable Sub OnDataInput(ByVal data() As Byte)
        RaiseEvent DataInput(Me, data)
    End Sub

    Public Sub dispose()
        Me.stream.Close()
        Me.client.Close()
    End Sub
End Class
