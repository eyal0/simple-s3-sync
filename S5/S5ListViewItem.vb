Imports Amazon.S3.Model

Public Enum CompareResult
    COMPARERESULT_UNKNOWN
    COMPARERESULT_SAME
    COMPARERESULT_LOCAL_NEWER
    COMPARERESULT_REMOTE_NEWER
    COMPARERESULT_LOCAL_MISSING
    COMPARERESULT_REMOTE_MISSING
End Enum

Public Enum CopyAction
    ACTION_NONE
    ACTION_COPY_LOCAL_TO_REMOTE
    ACTION_COPY_REMOTE_TO_LOCAL
    ACTION_UNKNOWN
End Enum

Public Class S5ListViewItem
    Inherits ListViewItem

    Property RowCompareResult As CompareResult = CompareResult.COMPARERESULT_UNKNOWN

    'These are the values that we got from the metadata or elsewhere, whether or not they're in the metadata.
    Public Property RemoteLastModifiedTimeUTC As DateTimeOffset
    Public Property RemoteMD5Hash As String
    Public Property RemoteCannedAcl As S3CannedACL
    Public Property RemoteLength As Long
    Public Property RemoteUntrustedLastmodifiedTimeUTC As DateTimeOffset

    Private Property RemoteMetadata As System.Collections.Specialized.NameValueCollection

    Private Property LocalLastModifiedTimeUTC As DateTimeOffset
        Get
            Dim localinfo As New System.IO.FileInfo(Local)
            Return localinfo.LastWriteTimeUtc
        End Get
        Set(value As DateTimeOffset)
            Dim localinfo As New System.IO.FileInfo(Local)
            System.IO.File.SetLastWriteTimeUtc(Local, value.UtcDateTime)
        End Set
    End Property
    Private Property LocalMD5Hash As Byte()

#Region "SubItem Accessors"
    Property Status As String
        Get
            Return SubItems("Status").Text
        End Get
        Set(value As String)
            SubItems("Status").Text = value
        End Set
    End Property

    Property Action As String
        Get
            Return SubItems("Action").Text
        End Get
        Set(value As String)
            SubItems("Action").Text = value
        End Set
    End Property

    Property Progress As String
        Get
            Return SubItems("Progress").Text
        End Get
        Set(value As String)
            SubItems("Progress").Text = value
        End Set
    End Property

    Property Local As String
        Get
            Return Me.Text
        End Get
        Set(value As String)
            Me.Text = value
        End Set
    End Property

    Property Remote As String
        Get
            Return SubItems("Remote").Text
        End Get
        Set(value As String)
            SubItems("Remote").Text = value
        End Set
    End Property

    Private Shadows ReadOnly Property SubItems As ListViewSubItemCollection
        Get
            Return MyBase.SubItems
        End Get
    End Property

    Private Shadows ReadOnly Property SubItems(index As Integer) As ListViewSubItem
        Get
            Return MyBase.SubItems(index)
        End Get
    End Property

    Private Shadows ReadOnly Property SubItems(index As String) As ListViewSubItem
        Get
            Return MyBase.SubItems(index)
        End Get
    End Property
#End Region

    Public Function RemoteMetadataNeedsUpdate(RowStrategy As S5Form.Strategy, cannedacl As S3CannedACL) As Boolean
        If RowCompareResult = CompareResult.COMPARERESULT_SAME Then
            If RowStrategy(CompareResult.COMPARERESULT_REMOTE_NEWER) = CopyAction.ACTION_COPY_LOCAL_TO_REMOTE AndAlso _
               RowStrategy(CompareResult.COMPARERESULT_LOCAL_NEWER) = CopyAction.ACTION_COPY_LOCAL_TO_REMOTE Then
                'clearly we want the remote to have the local's metadata always
                If Not RemoteMetadata.Keys.ContainsKey(METADATA_LAST_MODIFIED_TIME_UTC) _
                    OrElse DateTimeOffset.Parse(RemoteMetadata(METADATA_LAST_MODIFIED_TIME_UTC),
                                                Globalization.CultureInfo.InvariantCulture,
                                                Globalization.DateTimeStyles.RoundtripKind) <> LocalLastModifiedTimeUTC _
                    OrElse Not RemoteMetadata.Keys.ContainsKey(METADATA_MD5_HASH) _
                    OrElse (ByteArrayToString(LocalMD5Hash) <> "" AndAlso RemoteMetadata(METADATA_MD5_HASH) <> ByteArrayToString(LocalMD5Hash)) _
                    OrElse Not RemoteMetadata.Keys.ContainsKey(METADATA_CANNED_ACL) _
                    OrElse (cannedacl.ToString <> "NoACL" AndAlso RemoteMetadata(METADATA_CANNED_ACL) <> cannedacl.ToString) Then
                    Return True
                End If
            ElseIf RowStrategy(CompareResult.COMPARERESULT_REMOTE_NEWER) = CopyAction.ACTION_COPY_REMOTE_TO_LOCAL AndAlso _
                   RowStrategy(CompareResult.COMPARERESULT_LOCAL_NEWER) = CopyAction.ACTION_COPY_LOCAL_TO_REMOTE Then
                'synchronize
                If LocalLastModifiedTimeUTC > RemoteLastModifiedTimeUTC Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Public Function LocalMetadataNeedsUpdate(RowStrategy As S5Form.Strategy, cannedacl As S3CannedACL) As Boolean
        If RowCompareResult = CompareResult.COMPARERESULT_SAME Then
            If RowStrategy(CompareResult.COMPARERESULT_REMOTE_NEWER) = CopyAction.ACTION_COPY_REMOTE_TO_LOCAL AndAlso _
               RowStrategy(CompareResult.COMPARERESULT_LOCAL_NEWER) = CopyAction.ACTION_COPY_REMOTE_TO_LOCAL Then
                'clearly we want the local to have the remote's metadata, always
                If IO.File.GetLastWriteTimeUtc(Local) <> RemoteLastModifiedTimeUTC.UtcDateTime Then
                    Return True
                End If
            ElseIf RowStrategy(CompareResult.COMPARERESULT_REMOTE_NEWER) = CopyAction.ACTION_COPY_REMOTE_TO_LOCAL AndAlso _
                   RowStrategy(CompareResult.COMPARERESULT_LOCAL_NEWER) = CopyAction.ACTION_COPY_LOCAL_TO_REMOTE Then
                'synchronize
                If LocalLastModifiedTimeUTC < RemoteLastModifiedTimeUTC Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Public Sub UpdateLvi(RowStrategy As S5Form.Strategy, CannedAcl As S3CannedACL, Optional Progress As Double = -1, Optional Failed As Boolean = False)
        Dim localinfo As New IO.FileInfo(Local)
        Select Case RowCompareResult
            Case CompareResult.COMPARERESULT_LOCAL_MISSING
                Status = "Only on remote"
                Select Case RowStrategy(RowCompareResult)
                    Case CopyAction.ACTION_COPY_LOCAL_TO_REMOTE
                        Action = "Delete remote"
                    Case CopyAction.ACTION_COPY_REMOTE_TO_LOCAL
                        Action = "Copy to local"
                End Select
            Case CompareResult.COMPARERESULT_LOCAL_NEWER
                Status = "Local file is newer"
                Select Case RowStrategy(RowCompareResult)
                    Case CopyAction.ACTION_COPY_LOCAL_TO_REMOTE
                        Action = "Overwrite remote with newer"
                    Case CopyAction.ACTION_COPY_REMOTE_TO_LOCAL
                        Action = "Overwrite local with older"
                End Select
            Case CompareResult.COMPARERESULT_REMOTE_MISSING
                Status = "Only on local"
                Select Case RowStrategy(RowCompareResult)
                    Case CopyAction.ACTION_COPY_LOCAL_TO_REMOTE
                        Action = "Copy to remote"
                    Case CopyAction.ACTION_COPY_REMOTE_TO_LOCAL
                        Action = "Delete local"
                End Select
            Case CompareResult.COMPARERESULT_REMOTE_NEWER
                Status = "Remote file is newer"
                Select Case RowStrategy(RowCompareResult)
                    Case CopyAction.ACTION_COPY_LOCAL_TO_REMOTE
                        Action = "Overwrite remote with older"
                    Case CopyAction.ACTION_COPY_REMOTE_TO_LOCAL
                        Action = "Overwrite local with newer"
                End Select
            Case CompareResult.COMPARERESULT_SAME
                Status = "Files are identical"
                If RemoteMetadataNeedsUpdate(RowStrategy, CannedAcl) Then
                    Status += ", but remote metadata needs update"
                ElseIf LocalMetadataNeedsUpdate(RowStrategy, CannedAcl) Then
                    Status += ", but local filetime needs update"
                End If
            Case CompareResult.COMPARERESULT_UNKNOWN
                Status = "Not compared yet"
                Action = "Unknown"
        End Select
        If Failed Then
            Me.Progress = Progress.ToString("Failed")
        ElseIf Progress >= 0 Then
            Me.Progress = Progress.ToString("p")

        ElseIf RowStrategy(RowCompareResult) = CopyAction.ACTION_NONE Then
            If RemoteMetadataNeedsUpdate(RowStrategy, CannedAcl) Then
                Me.Progress = 0.ToString("p")
                Action = "Update remote metadata"
            ElseIf LocalMetadataNeedsUpdate(RowStrategy, CannedAcl) Then
                Me.Progress = 0.ToString("p")
                Action = "Update local metadata"
            Else
                Me.Progress = 1.ToString("p")
                Action = "Do nothing"
            End If
        Else
            Me.Progress = 0.ToString("p")
        End If
    End Sub

    Private Const METADATA_LAST_MODIFIED_TIME_UTC As String = "x-amz-meta-s5-last-modified-time-utc"
    Private Const METADATA_MD5_HASH As String = "x-amz-meta-s5-md5-hash"
    Private Const METADATA_CANNED_ACL As String = "x-amz-meta-s5-canned-acl"

    Async Function CompareLviAsync(s3client As Amazon.S3.AmazonS3Client, DontDownloadMetadata As Boolean, CurrentStrategy As S5Form.Strategy, CannedAcl As S3CannedACL, DontHashFiles As Boolean) As Task
        If DontDownloadMetadata Then
            RemoteCannedAcl = S3CannedACL.NoACL
            RemoteLastModifiedTimeUTC = RemoteUntrustedLastmodifiedTimeUTC
        Else
            Dim gomr As New Amazon.S3.Model.GetObjectMetadataRequest
            gomr.BucketName = S3FileDialog.GetBucketName(Remote)
            gomr.Key = S3FileDialog.GetPrefix(Remote)
            Dim response As GetObjectMetadataResponse = Await s3client.GetObjectMetadataAsync(gomr)
            If response Is Nothing Then
                RowCompareResult = CompareResult.COMPARERESULT_REMOTE_MISSING
                Return
            End If
            RemoteMetadata = response.Metadata
            If Not RemoteMetadata.Keys.ContainsKey(METADATA_LAST_MODIFIED_TIME_UTC) OrElse _
               Not DateTimeOffset.TryParse(RemoteMetadata(METADATA_LAST_MODIFIED_TIME_UTC),
                                            Globalization.CultureInfo.InvariantCulture,
                                            Globalization.DateTimeStyles.RoundtripKind,
                                            RemoteLastModifiedTimeUTC) Then
                RemoteLastModifiedTimeUTC = response.LastModified
            End If
            If RemoteMetadata.Keys.ContainsKey(METADATA_MD5_HASH) Then
                RemoteMD5Hash = RemoteMetadata(METADATA_MD5_HASH)
            Else
                RemoteMD5Hash = response.ETag.Trim(""""c)
            End If
            If Not RemoteMetadata.Keys.ContainsKey(METADATA_LAST_MODIFIED_TIME_UTC) _
               OrElse Not [Enum].TryParse(RemoteMetadata(METADATA_CANNED_ACL), RemoteCannedAcl) Then
                RemoteCannedAcl = S3CannedACL.NoACL
            End If
        End If
        Dim localinfo As New System.IO.FileInfo(Local)
        Dim LocalLength As Long = localinfo.Length
        If DontHashFiles Then
            If LocalLastModifiedTimeUTC < RemoteLastModifiedTimeUTC Then
                RowCompareResult = CompareResult.COMPARERESULT_REMOTE_NEWER
            ElseIf LocalLastModifiedTimeUTC > RemoteLastModifiedTimeUTC Then
                RowCompareResult = CompareResult.COMPARERESULT_LOCAL_NEWER
            ElseIf LocalLastModifiedTimeUTC = RemoteLastModifiedTimeUTC AndAlso LocalLength = RemoteLength Then
                RowCompareResult = CompareResult.COMPARERESULT_SAME
            End If
        Else
            LocalMD5Hash = Await Task.Run(Function() MD5CalcFile(Local))
            If LocalLength = RemoteLength AndAlso RemoteMD5Hash = ByteArrayToString(LocalMD5Hash) Then
                RowCompareResult = CompareResult.COMPARERESULT_SAME
            ElseIf LocalLastModifiedTimeUTC < RemoteLastModifiedTimeUTC Then
                RowCompareResult = CompareResult.COMPARERESULT_REMOTE_NEWER
            ElseIf LocalLastModifiedTimeUTC > RemoteLastModifiedTimeUTC Then
                RowCompareResult = CompareResult.COMPARERESULT_LOCAL_NEWER
            End If
        End If
        UpdateLvi(CurrentStrategy, CannedAcl)
    End Function

    Private Shared ObjectThrottler As New Throttler(4)

    Async Function SynchronizeLviAsync(s3client As Amazon.S3.AmazonS3Client, CurrentStrategy As S5Form.Strategy, CannedAcl As S3CannedACL) As Task
        Select Case CurrentStrategy(RowCompareResult)
            Case CopyAction.ACTION_COPY_LOCAL_TO_REMOTE
                If RowCompareResult = CompareResult.COMPARERESULT_LOCAL_MISSING Then 'this is a delete
                    Dim dor As New DeleteObjectRequest
                    dor.BucketName = S3FileDialog.GetBucketName(Remote)
                    dor.Key = S3FileDialog.GetPrefix(Remote)
                    Dim resp As Task(Of DeleteObjectResponse) = s3client.DeleteObjectAsync(dor)
                    Do Until TypeOf Await Task.WhenAny(resp, Task.Delay(250)) Is Task(Of DeleteObjectResponse)
                        Try
                            If dor.InputStream IsNot Nothing Then
                                UpdateLvi(CurrentStrategy, CannedAcl, (1.0 * dor.InputStream.Position / dor.InputStream.Length))
                            End If
                        Catch ex As Exception
                            UpdateLvi(CurrentStrategy, 0)
                        End Try
                    Loop
                Else
                    Dim por As New PutObjectRequest
                    por.BucketName = S3FileDialog.GetBucketName(Remote)
                    por.CannedACL = CannedAcl
                    por.GenerateMD5Digest = True
                    por.Key = S3FileDialog.GetPrefix(Remote)
                    por.FilePath = Local
                    If LocalMD5Hash Is Nothing Then
                        LocalMD5Hash = Await Task.Run(Function() MD5CalcFile(Local))
                    End If
                    por.MD5Digest = Convert.ToBase64String(LocalMD5Hash)
                    por = por.WithMetaData(RemoteMetadata)
                    por.RemoveMetaData(METADATA_LAST_MODIFIED_TIME_UTC)
                    por = por.WithMetaData(METADATA_LAST_MODIFIED_TIME_UTC, LocalLastModifiedTimeUTC.ToString("o", Globalization.CultureInfo.InvariantCulture))
                    por.RemoveMetaData(METADATA_MD5_HASH)
                    por = por.WithMetaData(METADATA_MD5_HASH, ByteArrayToString(LocalMD5Hash))
                    por.RemoveMetaData(METADATA_CANNED_ACL)
                    por = por.WithMetaData(METADATA_CANNED_ACL, CannedAcl.ToString)
                    por.CannedACL = CannedAcl

                    Dim PutObjectAsync0 As Func(Of Task(Of Object))
                    PutObjectAsync0 = Async Function() As Task(Of Object)
                                          Return Await s3client.PutObjectAsync(por)
                                      End Function
                    Dim resp As Task(Of PutObjectResponse) = CTypeAsync(Of Object, PutObjectResponse)(ObjectThrottler.Run(PutObjectAsync0))
                    Do Until TypeOf Await Task.WhenAny(resp, Task.Delay(250)) Is Task(Of PutObjectResponse)
                        Try
                            If por.InputStream IsNot Nothing Then
                                UpdateLvi(CurrentStrategy, CannedAcl, (1.0 * por.InputStream.Position / por.InputStream.Length))
                            End If
                        Catch ex As Exception
                            UpdateLvi(CurrentStrategy, 0)
                        End Try
                    Loop
                    Await resp
                End If
            Case CopyAction.ACTION_COPY_REMOTE_TO_LOCAL
                If RowCompareResult = CompareResult.COMPARERESULT_REMOTE_MISSING Then
                    IO.File.Delete(Local)
                Else 'copy to local
                    Dim gor As New GetObjectRequest
                    gor.BucketName = S3FileDialog.GetBucketName(Remote)
                    gor.Key = S3FileDialog.GetPrefix(Remote)

                    Dim GetObjectAsync0 As Func(Of Task(Of Object))
                    GetObjectAsync0 = Async Function() As Task(Of Object)
                                          Return Await s3client.GetObjectAsync(gor)
                                      End Function
                    Dim resp As Task(Of GetObjectResponse) = CTypeAsync(Of Object, GetObjectResponse)(ObjectThrottler.Run(GetObjectAsync0))
                    Dim resp2 As GetObjectResponse = Await resp

                    Dim downloadStream As IO.Stream = New IO.BufferedStream(New IO.FileStream(Local, IO.FileMode.Create))
                    Dim copyTask As Task = resp2.ResponseStream.CopyToAsync(downloadStream)

                    Do Until Await Task.WhenAny(copyTask, Task.Delay(250)) Is copyTask
                        Try
                            If resp2.ResponseStream IsNot Nothing Then
                                UpdateLvi(CurrentStrategy, CannedAcl, (1.0 * downloadStream.Position / resp2.ContentLength))
                            End If
                        Catch ex As Exception
                            UpdateLvi(CurrentStrategy, 0)
                        End Try
                    Loop
                    Await copyTask
                    downloadStream.Close()
                    LocalLastModifiedTimeUTC = RemoteLastModifiedTimeUTC.UtcDateTime
                End If
            Case CopyAction.ACTION_NONE
                'just update the metadata if needed
                If RemoteMetadataNeedsUpdate(CurrentStrategy, CannedAcl) Then
                    'write the metadata to the remote
                    Dim cor As New Amazon.S3.Model.CopyObjectRequest
                    cor = cor.WithMetaData(RemoteMetadata) 'copy the old ones
                    cor.RemoveMetaData(METADATA_LAST_MODIFIED_TIME_UTC)
                    cor = cor.WithMetaData(METADATA_LAST_MODIFIED_TIME_UTC, LocalLastModifiedTimeUTC.ToString("o", Globalization.CultureInfo.InvariantCulture))
                    cor.RemoveMetaData(METADATA_MD5_HASH)
                    If LocalMD5Hash IsNot Nothing Then
                        'if the files are SAME then either we didn't calculate the localhash or we did and it was equal to the remote hash
                        cor = cor.WithMetaData(METADATA_MD5_HASH, ByteArrayToString(LocalMD5Hash))
                    End If
                    cor.RemoveMetaData(METADATA_CANNED_ACL)
                    cor = cor.WithMetaData(METADATA_CANNED_ACL, CannedAcl.ToString)
                    cor.CannedACL = CannedAcl
                    cor.DestinationBucket = S3FileDialog.GetBucketName(Remote)
                    cor.DestinationKey = S3FileDialog.GetPrefix(Remote)
                    cor.Directive = S3MetadataDirective.REPLACE
                    cor.SourceBucket = cor.DestinationBucket
                    cor.SourceKey = cor.DestinationKey
                    Dim resp As Task(Of CopyObjectResponse) = s3client.CopyObjectAsync(cor)
                    Do Until Await Task.WhenAny(resp, Task.Delay(250)) Is resp
                        Try
                            If cor.InputStream IsNot Nothing Then
                                UpdateLvi(CurrentStrategy, CannedAcl, (1.0 * cor.InputStream.Position / cor.InputStream.Length))
                            End If
                        Catch ex As Exception
                            UpdateLvi(CurrentStrategy, 0)
                        End Try
                    Loop
                    Await resp
                ElseIf LocalMetadataNeedsUpdate(CurrentStrategy, CannedAcl) Then
                    LocalLastModifiedTimeUTC = RemoteLastModifiedTimeUTC.UtcDateTime
                End If
        End Select
        UpdateLvi(CurrentStrategy, CannedAcl, 1)
    End Function

    Sub New(local As String, remote As String, CurrentStrategy As S5Form.Strategy, CannedAcl As S3CannedACL)
        Me.Local = local
        SubItems.Add(remote).Name = "Remote"
        SubItems.Add("").Name = "Status"
        SubItems.Add("").Name = "Action"
        SubItems.Add("").Name = "Progress"
        UpdateLvi(CurrentStrategy, CannedAcl)
    End Sub
End Class
