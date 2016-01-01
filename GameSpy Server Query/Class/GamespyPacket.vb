Public Class GamespyPacket

    Public Property GameName As String
    Public Property MasterServerName As String
    Public Property MasterServerKey As String = "hMO2d4"
    Public Property Key As Byte()
    Public Property Response As Byte()

    Public Overridable Function Compile() As Byte()
        Return {}
    End Function

    Friend Function ConcatArray(ByVal source() As Byte, ByVal dest() As Byte, Optional ByVal separator As Byte = Byte.MaxValue) As Byte()
        Dim newSize As Int32 = dest.Length + source.Length
        Dim oldSize As Int32 = dest.Length
        If separator <> Byte.MaxValue Then newSize += 1

        Array.Resize(dest, newSize)
        Array.Copy(source, 0, dest, oldSize, source.Length)

        If separator <> Byte.MaxValue Then dest(dest.Length - 1) = separator
        Return dest
    End Function
    Friend Function GetBytes(ByVal str As String) As Byte()
        Return System.Text.Encoding.ASCII.GetBytes(str)
    End Function
    Friend Function GetString(ByVal bytes() As Byte) As String
        Return System.Text.Encoding.ASCII.GetString(bytes)
    End Function

    Public Sub ReceiveBytes(ByVal buffer() As Byte)
        Me.Response = Me.DecryptPacket(buffer)
        Me.OnReceive(Me.Response)
    End Sub

    Private Function DecryptPacket(ByVal data() As Byte) As Byte()
        Return enctypex_decoder(Me.GetBytes(MasterServerKey), Me.Key, data, data.Length, Nothing)
    End Function

    Friend Overridable Sub OnReceive(ByVal data() As Byte)

    End Sub


  End Class

Module Enctypex
    Public Class enctypex_data_t
        Public encxkey(260) As Byte ' static key
        Public offset As Long ' everything decrypted till now (total)
        Public start As Long ' where starts the buffer (so how much big is the header), this is the only one you need to zero
    End Class
    Public Function enctypex_func5(ByRef encxkey() As Byte, ByRef cnt As Long, ByRef id() As Byte, ByRef idlen As Long, ByRef n1 As Long, ByRef n2 As Long) As Long
        Dim i As Long
        Dim tmp As Long
        Dim mask As Long = 1

        If cnt = 0 Then
            Return 0
        End If
        If cnt > 1 Then
            Do
                mask = (mask << 1) + 1
            Loop While mask < cnt
        End If

        i = 0
        Do
            n1 = CLng(encxkey(n1 And &HFF)) + CLng(id(n2))
            n2 += 1
            If n2 >= idlen Then
                n2 = 0
                n1 += idlen
            End If
            tmp = n1 And mask
            i += 1
            If i > 11 Then
                tmp = tmp Mod cnt
            End If
        Loop While tmp > cnt

        Return (tmp)
    End Function
    Public Sub enctypex_func4(ByRef encxkey() As Byte, ByRef id() As Byte, ByRef idlen As Long)
        Dim i As Long
        Dim n1 As Long = 0
        Dim n2 As Long = 0
        Dim t1 As Byte
        Dim t2 As Byte

        If idlen < 1 Then
            Return
        End If

        For i = 0 To 255
            encxkey(i) = i
        Next i

        For i = 255 To 0 Step -1
            t1 = enctypex_func5(encxkey, i, id, idlen, n1, n2)
            t2 = encxkey(i)
            encxkey(i) = encxkey(t1)
            encxkey(t1) = t2
        Next i

        encxkey(256) = encxkey(1)
        encxkey(257) = encxkey(3)
        encxkey(258) = encxkey(5)
        encxkey(259) = encxkey(7)
        encxkey(260) = encxkey(n1 And &HFF)
    End Sub
    Public Function enctypex_func7(ByRef encxkey() As Byte, ByRef d As Byte) As Byte
        Dim a As Integer
        Dim b As Integer
        Dim c As Integer

        a = encxkey(256)
        b = encxkey(257)
        c = encxkey(a)
        encxkey(256) = (a + 1) Mod 256
        encxkey(257) = (b + c) Mod 256
        a = encxkey(260)
        b = encxkey(257)
        b = encxkey(b)
        c = encxkey(a)
        encxkey(a) = b
        a = encxkey(259)
        b = encxkey(257)
        a = encxkey(a)
        encxkey(b) = a
        a = encxkey(256)
        b = encxkey(259)
        a = encxkey(a)
        encxkey(b) = a
        a = encxkey(256)
        encxkey(a) = c
        b = encxkey(258)
        a = encxkey(c)
        c = encxkey(259)
        b = (b + a) Mod 256
        encxkey(258) = b
        a = b
        c = encxkey(c)
        b = encxkey(257)
        b = encxkey(b)
        a = encxkey(a)
        c = (c + b) Mod 256
        b = encxkey(260)
        b = encxkey(b)
        c = (c + b) Mod 256
        b = encxkey(c)
        c = encxkey(256)
        c = encxkey(c)
        a = (a + c) Mod 256
        c = encxkey(b)
        b = encxkey(a)
        encxkey(260) = d
        c = (c Xor b Xor d) Mod 256
        encxkey(259) = c

        Return c
    End Function
    Public Function enctypex_func6(ByRef encxkey() As Byte, ByRef data() As Byte, ByVal len As Long) As Long
        Dim i As Long

        For i = 0 To len - 1
            data(i) = enctypex_func7(encxkey, data(i))
        Next
        Return (len)
    End Function
    Public Sub enctypex_funcx(ByRef encxkey() As Byte, ByRef key() As Byte, ByRef encxvalidate() As Byte, ByRef data() As Byte, ByVal datalen As Long)
        Dim i As Long
        Dim keylen As Long

        keylen = key.Length
        For i = 0 To datalen - 1
            encxvalidate((key(i Mod keylen) * i) And 7) = encxvalidate((key(i Mod keylen) * i) And 7) Xor encxvalidate(i And 7) Xor data(i)
        Next
        enctypex_func4(encxkey, encxvalidate, 8)
    End Sub

    Public Function enctypex_init(ByRef encxkey() As Byte, ByRef key() As Byte, ByRef validate() As Byte, ByRef data() As Byte, ByRef datalen As Long, ByRef enctypex_data As enctypex_data_t) As Byte()
        Dim a As Long
        Dim b As Long
        Dim encxvalidate(7) As Byte

        If datalen < 1 Then
            Return (Nothing)
        End If
        a = (data(0) Xor &HEC) + 2
        If datalen < a Then
            Return (Nothing)
        End If
        b = data(a - 1) Xor &HEA
        If datalen < (a + b) Then
            Return (Nothing)
        End If
        Array.Copy(validate, encxvalidate, 8)
        Dim temp1() As Byte = New Byte(datalen - a) {}
        Array.ConstrainedCopy(data, a, temp1, 0, datalen - a)
        enctypex_funcx(encxkey, key, encxvalidate, temp1, b)
        Array.ConstrainedCopy(temp1, 0, data, a, datalen - a)
        a += b
        enctypex_data = New enctypex_data_t
        If enctypex_data Is Nothing Then
            Dim temp2() As Byte = New Byte(datalen - a) {}
            Array.ConstrainedCopy(data, a, temp2, 0, datalen - a)
            data = temp2
            datalen -= a ' datalen is untouched in stream mode!!!
        Else
            enctypex_data.offset = a
            enctypex_data.start = a
        End If

        Return data
    End Function
    Public Function enctypex_decoder(ByRef key() As Byte, ByRef validate() As Byte, ByRef data() As Byte, ByRef datalen As Long, ByRef enctypex_data As enctypex_data_t) As Byte()
        Dim encxkeyb(260) As Byte
        Dim encxkey() As Byte

        If enctypex_data Is Nothing Then
            encxkey = encxkeyb
        Else
            encxkey = enctypex_data.encxkey
        End If


        If enctypex_data Is Nothing OrElse enctypex_data.start <> Nothing Then
            data = enctypex_init(encxkey, key, validate, data, datalen, enctypex_data)
            If data Is Nothing Then
                Return Nothing
            End If
        End If

        If enctypex_data Is Nothing Then
            enctypex_func6(encxkey, data, datalen)
            Return data
        ElseIf enctypex_data.start <> Nothing Then
            Dim temp1() As Byte = New Byte(datalen - enctypex_data.offset - 1) {}
            Array.ConstrainedCopy(data, enctypex_data.offset, temp1, 0, datalen - enctypex_data.offset)
            Dim temp3 As Long = enctypex_func6(encxkey, temp1, datalen - enctypex_data.offset)

            Array.ConstrainedCopy(temp1, 0, data, enctypex_data.offset, datalen - enctypex_data.offset)
            enctypex_data.offset += temp3
            Dim temp2() As Byte = New Byte(datalen - enctypex_data.start - 1) {}
            Array.ConstrainedCopy(data, enctypex_data.start, temp2, 0, datalen - enctypex_data.start)
            Return temp2
        End If
        Return Nothing
    End Function
    Public Function enctypex_msname(ByRef gamename As String, ByRef retname As String) As String
        Static msname As String
        'Dim c As ULong
        Dim server_num As ULong = Now.TimeOfDay.TotalSeconds
        'For i = 0 To gamename.Length - 1
        '    c = AscW(Char.ToLower(ChrW(gamename(i))))
        '    server_num = c - (server_num * &H63306CE7)
        'Next

        server_num = server_num Mod 20

        If retname IsNot Nothing Then
            retname = String.Format("{0}.ms{1}.gamespy.com", gamename, server_num)
            Return retname
        End If

        msname = String.Format("{0}.ms{1}.gamespy.com", gamename, server_num)
        Return msname
    End Function
    Public Function compileXPacket(ByVal gamestr As String, ByVal validate() As Byte, ByVal info As String, ByVal type As Integer, Optional ByVal filter As String = "") As Byte()
        Dim msgamestr As String = "gslive"
        Dim plen As Integer = 2 + 7 + gamestr.Length + 1 + msgamestr.Length + 1 + validate.Length + filter.Length + 1 + info.Length + 1 + 4
        Dim p() As Byte = New Byte(plen) {}
        Dim i As Integer = 0
        i += 2
        p(i) = 0
        i += 1
        p(i) = 1
        i += 1
        p(i) = 3
        i += 1
        p(i) = 0
        i += 1
        p(i) = 0
        i += 1
        p(i) = 0
        i += 1
        p(i) = 0
        i += 1
        For Each ccchar As Char In gamestr
            p(i) = AscW(ccchar)
            i += 1
        Next
        p(i) = 0
        i += 1
        For Each ccchar As Char In msgamestr
            p(i) = AscW(ccchar)
            i += 1
        Next
        p(i) = 0
        i += 1
        For Each ccbyte As Byte In validate
            p(i) = ccbyte
            i += 1
        Next
        i -= 1
        For Each ccchar As Char In filter
            p(i) = AscW(ccchar)
            i += 1
        Next
        p(i) = 0
        i += 1
        For Each ccchar As Char In info
            p(i) = AscW(ccchar)
            i += 1
        Next
        p(i) = 0
        i += 1
        p(i) = 0
        i += 1
        p(i) = 0
        i += 1
        p(i) = 0
        i += 1
        p(i) = type
        '// bits which compose the "type" byte:
        '// 00: plain server list, sometimes the master server returns also the informations if requested
        '// 01: requested informations of the server, like \hostname\numplayers and so on
        '// 02: nothing???
        '// 04: available informations on the master server??? hostname, mapname, gametype, numplayers, maxplayers, country, gamemode, password, gamever
        '// 08: nothing???
        '// 10: ???
        '// 20: peerchat IRC rooms
        '// 40: ???
        '// 80: nothing???
        i += 1
        p(0) = i >> 8
        p(1) = i
        Return p
    End Function
#Region "Unused in test"
    'Checked
    Public Function enctypex_func6e(ByRef encxkey() As Byte, ByRef data() As Byte, ByVal len As Long) As Long
        Dim i As Long

        For i = 0 To len - 1
            data(i) = enctypex_func7e(encxkey, data(i))
        Next
        Return (len)
    End Function

    'Checked
    Public Function enctypex_func7e(ByRef encxkey() As Byte, ByVal d As Byte) As Long
        Dim a As Byte
        Dim b As Byte
        Dim c As Byte

        a = encxkey(256)
        b = encxkey(257)
        c = encxkey(a)
        encxkey(256) = a + 1
        encxkey(257) = b + c
        a = encxkey(260)
        b = encxkey(257)
        b = encxkey(b)
        c = encxkey(a)
        encxkey(a) = b
        a = encxkey(259)
        b = encxkey(257)
        a = encxkey(a)
        encxkey(b) = a
        a = encxkey(256)
        b = encxkey(259)
        a = encxkey(a)
        encxkey(b) = a
        a = encxkey(256)
        encxkey(a) = c
        b = encxkey(258)
        a = encxkey(c)
        c = encxkey(259)
        b += a
        encxkey(258) = b
        a = b
        c = encxkey(c)
        b = encxkey(257)
        b = encxkey(b)
        a = encxkey(a)
        c += b
        b = encxkey(260)
        b = encxkey(b)
        c += b
        b = encxkey(c)
        c = encxkey(256)
        c = encxkey(c)
        a += c
        c = encxkey(b)
        b = encxkey(a)
        c = c Xor b Xor d
        encxkey(260) = c ' encrypt
        encxkey(259) = d ' encrypt
        Return (c)
    End Function

    'Checked
    ' exactly as above but with enctypex_func6e instead of enctypex_func6
    Public Function enctypex_encoder(ByRef key() As Byte, ByRef validate() As Byte, ByRef data() As Byte, ByRef datalen As Long, ByRef enctypex_data As enctypex_data_t) As Byte()
        Dim encxkeyb(260) As Byte
        Dim encxkey() As Byte

        If enctypex_data IsNot Nothing Then
            encxkey = enctypex_data.encxkey
        Else
            encxkey = encxkeyb
        End If

        If enctypex_data Is Nothing OrElse enctypex_data.start <> Nothing Then
            data = enctypex_init(encxkey, key, validate, data, datalen, enctypex_data)
            If data Is Nothing Then
                Return (Nothing)
            End If
        End If

        If enctypex_data Is Nothing Then
            enctypex_func6e(encxkey, data, datalen)
            Return data
        ElseIf enctypex_data.start <> 0 Then
            Dim temp1() As Byte = New Byte() {}
            Array.ConstrainedCopy(data, enctypex_data.offset, temp1, 0, datalen - enctypex_data.offset)

            Dim temp2 As Long = enctypex_func6e(encxkey, temp1, datalen - enctypex_data.offset)
            Array.ConstrainedCopy(temp1, 0, data, enctypex_data.offset, datalen - enctypex_data.offset)
            enctypex_data.offset += temp2

            Dim temp3() As Byte = New Byte() {}
            Array.ConstrainedCopy(data, enctypex_data.start, temp3, 0, datalen - enctypex_data.start)
            Return temp3
        End If
        Return Nothing
    End Function

    'Checked
    ' data must be enough big to include the 23 bytes header, remember it: data = realloc(data, size + 23);
    Public Function enctypex_quick_encrypt(ByRef key() As Byte, ByRef validate() As Byte, ByRef data() As Byte, ByRef size As Long) As Long
        Dim rnd As Long
        Dim tmpsize As Long
        Dim keylen As Long
        Dim vallen As Long
        Dim tmp(22) As Byte

        If key Is Nothing Or validate Is Nothing Or data Is Nothing Or size < 0 Then
            Return 0
        End If

        keylen = key.Length ' only for giving a certain randomness, so useless
        vallen = validate.Length
        rnd = Not Now.TimeOfDay.TotalSeconds

        For i = 0 To tmp.Length - 1
            rnd = (rnd * &H343FD) + &H269EC3
            tmp(i) = rnd Xor key(i Mod keylen) Xor validate(i Mod vallen)
        Next

        tmp(0) = &HEB ' 7
        tmp(1) = &H0
        tmp(2) = &H0
        tmp(8) = &HE4 ' 14

        For i = size - 1 To 0 Step -1
            data(tmp.Length + i) = data(i)
        Next

        Array.Copy(tmp, data, tmp.Length)
        size += tmp.Length

        tmpsize = size
        enctypex_encoder(key, validate, data, tmpsize, Nothing)
        Return size
    End Function
#End Region
End Module


