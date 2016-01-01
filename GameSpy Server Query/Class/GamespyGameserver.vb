Imports System.Reflection
Public Class GamespyGameserver
    '"hostname", "gamemode", "mapname", "gamever", "numplayers", "maxplayers", "gametype", "session", "prevsession", "swbregion", "servertype", "password"}
    Public Property TypeID As Byte
    Public Property Address As String
    Public Property GamePort As UInt16

    Public Property MaxPlayers As String = String.Empty
    Public Property NumPlayers As String = String.Empty

    Public Property Hostname As String = String.Empty
    Public Property MapName As String = String.Empty
    Public Property GameType As String = String.Empty
    Public Property GameMode As String = String.Empty
    Public Property Password As String = String.Empty

    Public Property GameVer As String = String.Empty
    Public Property Session As String = String.Empty
    Public Property PrevSession As String = String.Empty
    Public Property ServerType As String = String.Empty
    Public Property SwbRegion As String = String.Empty

    Public Property BehindNAT As String = False
    Public Property NATLANAddress As String = String.Empty
    Public Property NATLANPort As String = String.Empty
    Public Property NATRoutingAddress As String = String.Empty

    Private bytesParsed As Int32 = 0
    Public Function ParseBytes(ByVal data() As Byte, ByVal offset As Int32, ByVal Params() As String) As Int32

        'Globale Creds
        Me.TypeID = data(offset)

        If Me.TypeID <> 21 And Me.TypeID <> 85 And Me.TypeID <> 126 Then Return -1

        Me.BehindNAT = (data(offset) = 126)
        Me.Address = ParseIPA(data, offset + 1)
        Me.GamePort = GetInvertedUInt16(data, offset + 5)
        bytesParsed += 7
        If Me.TypeID = 21 Then Return bytesParsed + offset


        'NAT
        If Me.BehindNAT Then
            Me.NATLANAddress = ParseIPA(data, offset + 7)
            Me.NATLANPort = GetInvertedUInt16(data, offset + 11)
            Me.NATRoutingAddress = ParseIPA(data, offset + 13)
            bytesParsed += 10
        End If

        'String-Params
        For i = 0 To Params.Length - 1
            Dim propInfo As PropertyInfo = Me.GetType().GetProperty(Params(i), BindingFlags.Public Or BindingFlags.Instance Or BindingFlags.IgnoreCase)

            propInfo.SetValue(Me, Me.FetchParam(data, offset + bytesParsed + 1))
        Next
        Return bytesParsed + offset
    End Function

    Private Function ParseIPA(ByVal buffer() As Byte, ByVal offset As Int32) As String
        Return buffer(offset).ToString & "." &
               buffer(offset + 1).ToString & "." &
               buffer(offset + 2).ToString & "." &
               buffer(offset + 3).ToString
    End Function

    Private Function FetchParam(ByVal buffer() As Byte, ByVal offset As Int32) As String
        Dim endIndex As Int32 = Array.IndexOf(buffer, CByte(0), offset)
        If (endIndex = -1) Then
            Return String.Empty
        End If

        Dim strBuffer(endIndex - offset) As Byte

        Array.Copy(buffer, offset, strBuffer, 0, strBuffer.Length)

        bytesParsed += strBuffer.Length + 1
        Return System.Text.Encoding.ASCII.GetString(strBuffer)
    End Function

    Private Function GetInvertedUInt16(ByVal data() As Byte, ByVal offset As Int32) As UInt16
        Dim buf(1) As Byte
        Array.Copy(data, offset, buf, 0, buf.Length)
        Array.Reverse(buf)
        Return BitConverter.ToUInt16(buf, 0)
    End Function

End Class
