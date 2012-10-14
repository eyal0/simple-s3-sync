Module MD5
    ''' <summary>
    ''' Calculate a hex string of the MD5 of a file
    ''' </summary>
    ''' <param name="filepath">Path to file</param>
    ''' <returns>Hex string with 32 hex characters</returns>
    ''' <remarks></remarks>
    Public Function MD5CalcFileString(ByVal filepath As String) As String
        Return ByteArrayToString(MD5CalcFile(filepath))
    End Function

    ''' <summary>
    ''' Calculate byte array of MD5 of a file
    ''' </summary>
    ''' <param name="filepath">Path to file</param>
    ''' <returns>Byte array of 16 bytes</returns>
    ''' <remarks></remarks>
    Public Function MD5CalcFile(ByVal filepath As String) As Byte()
        ' open file (as read-only)
        Using reader As New System.IO.FileStream(filepath, IO.FileMode.Open, IO.FileAccess.Read)
            Using md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
                ' hash contents of this stream
                Dim hash() As Byte = md5.ComputeHash(reader)
                ' return formatted hash
                Return hash
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Convert byte array to string of uppercase hex characters
    ''' </summary>
    ''' <param name="arrInput">Bytes to convert</param>
    ''' <returns>String of hex characters, uppercase</returns>
    ''' <remarks></remarks>
    Public Function ByteArrayToString(ByVal arrInput() As Byte) As String
        Dim sb As New System.Text.StringBuilder(arrInput.Length * 2)

        For i As Integer = 0 To arrInput.Length - 1
            sb.Append(arrInput(i).ToString("X2"))
        Next
        Return sb.ToString().ToLower
    End Function
End Module
