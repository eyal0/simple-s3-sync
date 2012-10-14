Imports Amazon.S3.Model

Public Class S5Form
    Private Sub OpenRemote_Click(sender As Object, e As EventArgs) Handles OpenRemote.Click
        Dim S3FileDialog As New S3FileDialog(KeyId.Text, keySecret.Text)
        If S3FileDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtRemotePath.Text = S3FileDialog.Path
        End If
    End Sub

    Private Sub OpenLocal_Click(sender As Object, e As EventArgs) Handles OpenLocal.Click
        Dim frmd As DirectoryDialog = New DirectoryDialog()
        frmd.BrowseFor = DirectoryDialog.BrowseForTypes.FilesAndDirectories
        frmd.Title = "Select a file or a folder"
        frmd.Selected = txtLocalPath.Text
        If frmd.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtLocalPath.Text = frmd.Selected
        End If
    End Sub

    Public Class Strategy
        Dim d As New Dictionary(Of CompareResult, CopyAction)

        Default Public Property Item(c As CompareResult) As CopyAction
            Get
                Return d(c)
            End Get
            Set(ByVal value As CopyAction)
                d(c) = value
            End Set
        End Property

        Sub New(LocalNewer As CopyAction, RemoteNewer As CopyAction, LocalMissing As CopyAction, RemoteMissing As CopyAction)
            Me.d.Add(CompareResult.COMPARERESULT_LOCAL_NEWER, LocalNewer)
            Me.d.Add(CompareResult.COMPARERESULT_REMOTE_NEWER, RemoteNewer)
            Me.d.Add(CompareResult.COMPARERESULT_LOCAL_MISSING, LocalMissing)
            Me.d.Add(CompareResult.COMPARERESULT_REMOTE_MISSING, RemoteMissing)
            Me.d.Add(CompareResult.COMPARERESULT_SAME, CopyAction.ACTION_NONE)
            Me.d.Add(CompareResult.COMPARERESULT_UNKNOWN, CopyAction.ACTION_UNKNOWN)
        End Sub

        Shared Operator =(a As Strategy, b As Strategy) As Boolean
            If a.d.Count <> b.d.Count Then
                Return False
            Else
                For Each k As CompareResult In a.d.Keys
                    If a.d(k) <> b.d(k) Then
                        Return False
                    End If
                Next
                Return True
            End If
        End Operator

        Shared Operator <>(a As Strategy, b As Strategy) As Boolean
            Return Not (a = b)
        End Operator

        Public Shared NameToStrategy As New Dictionary(Of String, Strategy) From _
            {{"Do nothing", New Strategy(CopyAction.ACTION_NONE,
                                         CopyAction.ACTION_NONE,
                                         CopyAction.ACTION_NONE,
                                         CopyAction.ACTION_NONE)},
             {"Copy Local to Remote, Overwrite all", New Strategy(CopyAction.ACTION_COPY_LOCAL_TO_REMOTE,
                                                                  CopyAction.ACTION_COPY_LOCAL_TO_REMOTE,
                                                                  CopyAction.ACTION_NONE,
                                                                  CopyAction.ACTION_COPY_LOCAL_TO_REMOTE)},
             {"Copy Local to Remote, Overwrite older", New Strategy(CopyAction.ACTION_COPY_LOCAL_TO_REMOTE,
                                                                    CopyAction.ACTION_NONE,
                                                                    CopyAction.ACTION_NONE,
                                                                    CopyAction.ACTION_COPY_LOCAL_TO_REMOTE)},
             {"Copy Local to Remote, Overwrite none", New Strategy(CopyAction.ACTION_NONE,
                                                                   CopyAction.ACTION_NONE,
                                                                   CopyAction.ACTION_NONE,
                                                                   CopyAction.ACTION_COPY_LOCAL_TO_REMOTE)},
             {"Mirror Local to Remote", New Strategy(CopyAction.ACTION_COPY_LOCAL_TO_REMOTE,
                                                     CopyAction.ACTION_COPY_LOCAL_TO_REMOTE,
                                                     CopyAction.ACTION_COPY_LOCAL_TO_REMOTE,
                                                     CopyAction.ACTION_COPY_LOCAL_TO_REMOTE)},
             {"Copy Remote to Local, Overwrite all", New Strategy(CopyAction.ACTION_COPY_REMOTE_TO_LOCAL,
                                                                  CopyAction.ACTION_COPY_REMOTE_TO_LOCAL,
                                                                  CopyAction.ACTION_COPY_REMOTE_TO_LOCAL,
                                                                  CopyAction.ACTION_NONE)},
             {"Copy Remote to Local, Overwrite older", New Strategy(CopyAction.ACTION_NONE,
                                                                    CopyAction.ACTION_COPY_REMOTE_TO_LOCAL,
                                                                    CopyAction.ACTION_COPY_REMOTE_TO_LOCAL,
                                                                    CopyAction.ACTION_NONE)},
             {"Copy Remote to Local, Overwrite none", New Strategy(CopyAction.ACTION_NONE,
                                                                   CopyAction.ACTION_NONE,
                                                                   CopyAction.ACTION_COPY_REMOTE_TO_LOCAL,
                                                                   CopyAction.ACTION_NONE)},
             {"Mirror Remote to Local", New Strategy(CopyAction.ACTION_COPY_REMOTE_TO_LOCAL,
                                                    CopyAction.ACTION_COPY_REMOTE_TO_LOCAL,
                                                    CopyAction.ACTION_COPY_REMOTE_TO_LOCAL,
                                                    CopyAction.ACTION_COPY_REMOTE_TO_LOCAL)},
             {"Synchronize", New Strategy(CopyAction.ACTION_COPY_LOCAL_TO_REMOTE,
                                          CopyAction.ACTION_COPY_REMOTE_TO_LOCAL,
                                          CopyAction.ACTION_NONE,
                                          CopyAction.ACTION_NONE)},
             {"Custom", Nothing}}
    End Class


    Function ComboBoxToStrategy() As Strategy
        Dim s As New Strategy(DirectCast(cbxLocalNewer.SelectedIndex, CopyAction),
                              DirectCast(cbxRemoteNewer.SelectedIndex, CopyAction),
                              DirectCast(cbxLocalMissing.SelectedIndex, CopyAction),
                              DirectCast(cbxRemoteMissing.SelectedIndex, CopyAction))
        Return s
    End Function

    Sub StrategyToComboBoxes(s As Strategy)
        cbxLocalNewer.SelectedIndex = s(CompareResult.COMPARERESULT_LOCAL_NEWER)
        cbxRemoteNewer.SelectedIndex = s(CompareResult.COMPARERESULT_REMOTE_NEWER)
        cbxLocalMissing.SelectedIndex = s(CompareResult.COMPARERESULT_LOCAL_MISSING)
        cbxRemoteMissing.SelectedIndex = s(CompareResult.COMPARERESULT_REMOTE_MISSING)
    End Sub

    Private _CurrentStrategy As Strategy
    Private Property CurrentStrategy As Strategy
        Get
            Return _CurrentStrategy
        End Get
        Set(value As Strategy)
            _CurrentStrategy = value
            For Each lvi As S5ListViewItem In lvStatus.Items
                lvi.UpdateLvi(CurrentStrategy, CurrentCannedAcl)
            Next
        End Set
    End Property

    Private FixingCombobox As Boolean = False

    Private Sub cbxAction_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxAction.SelectedIndexChanged
        If Not FixingCombobox Then
            FixingCombobox = True
            CurrentStrategy = Strategy.NameToStrategy(cbxAction.SelectedItem.ToString)
            StrategyToComboBoxes(CurrentStrategy)
            FixingCombobox = False
        End If
    End Sub

    Private Sub cbxspecific_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxLocalNewer.SelectedIndexChanged, _
                                                                                           cbxRemoteNewer.SelectedIndexChanged, _
                                                                                           cbxLocalMissing.SelectedIndexChanged,
                                                                                           cbxRemoteMissing.SelectedIndexChanged
        If Not FixingCombobox Then
            FixingCombobox = True
            CurrentStrategy = ComboBoxToStrategy()
            For Each kvp As KeyValuePair(Of String, Strategy) In Strategy.NameToStrategy
                If kvp.Value Is Nothing OrElse kvp.Value = CurrentStrategy Then
                    cbxAction.SelectedItem = kvp.Key
                    Exit For
                End If
            Next
            FixingCombobox = False
        End If
    End Sub

    Private Async Sub Add_Click(sender As Object, e As EventArgs) Handles Add.Click
        Me.Cursor = Cursors.AppStarting
        Compare.Enabled = False
        Synchronize.Enabled = False
        Dim remotefiles As New Dictionary(Of String, S5ListViewItem)
        Dim localfiles As New Dictionary(Of String, S5ListViewItem)
        Dim remotepath As String = txtRemotePath.Text
        Dim localpath As String = txtLocalPath.Text
        If System.IO.File.Exists(localpath) Then
            localpath = IO.Path.GetFullPath(localpath).TrimEnd(IO.Path.DirectorySeparatorChar)
            If remotepath.EndsWith("/") Then
                remotepath = remotepath + System.IO.Path.GetFileName(localpath)
                Dim lvi As New S5ListViewItem(localpath, remotepath, CurrentStrategy, CurrentCannedAcl)
                lvStatus.Items.Add(lvi)
                localfiles.Add(lvi.Local, lvi)
                remotefiles.Add(lvi.Remote, lvi)
            Else
                Dim lvi As New S5ListViewItem(localpath, remotepath, CurrentStrategy, CurrentCannedAcl)
                lvStatus.Items.Add(lvi)
                localfiles.Add(lvi.Local, lvi)
                remotefiles.Add(lvi.Remote, lvi)
            End If
        ElseIf System.IO.Directory.Exists(localpath) Then
            If Not remotepath.EndsWith("/"c) Then
                remotepath += "/"c
            End If
            localpath = IO.Path.GetFullPath(localpath).TrimEnd(IO.Path.DirectorySeparatorChar) + IO.Path.DirectorySeparatorChar
            For Each filename As String In System.IO.Directory.EnumerateFiles(localpath, "*.*", IO.SearchOption.AllDirectories)
                Dim lvi As New S5ListViewItem(filename,
                                              remotepath + filename.Substring(localpath.Length).Replace(IO.Path.DirectorySeparatorChar, "/"c),
                                              CurrentStrategy, CurrentCannedAcl)
                lvStatus.Items.Add(lvi)
                localfiles.Add(lvi.Local, lvi)
                remotefiles.Add(lvi.Remote, lvi)
            Next
        End If
        Dim s3client As New Amazon.S3.AmazonS3Client(KeyId.Text, keySecret.Text)
        Dim respTask As Task(Of ListObjectsResponse) = Nothing
        Dim resp As ListObjectsResponse = Nothing
        Dim s3oCount As Integer = 0
        Do
            Dim lor As New ListObjectsRequest
            lor.Prefix = S3FileDialog.GetPrefix(remotepath)
            lor.BucketName = S3FileDialog.GetBucketName(remotepath)
            If resp IsNot Nothing Then
                lor.Marker = resp.NextMarker
            End If
            respTask = s3client.ListObjectsAsync(lor)

            resp = Await respTask
            For Each s3o As S3Object In resp.S3Objects
                s3oCount += 1
                Add.Text = "Add (" & s3oCount & ")"
                If Not remotefiles.ContainsKey("/" + S3FileDialog.GetBucketName(remotepath) + "/" + s3o.Key) Then
                    Dim lvi As New S5ListViewItem(IO.Path.Combine(localpath, s3o.Key.Substring(lor.Prefix.Length).Replace("/"c, IO.Path.DirectorySeparatorChar)),
                                                  "/" + S3FileDialog.GetBucketName(remotepath) + "/" + s3o.Key,
                                                  CurrentStrategy, CurrentCannedAcl)
                    lvStatus.Items.Add(lvi)
                    lvi.RowCompareResult = CompareResult.COMPARERESULT_LOCAL_MISSING
                    lvi.UpdateLvi(CurrentStrategy, CurrentCannedAcl)
                Else
                    Dim lvi As S5ListViewItem = remotefiles("/" + S3FileDialog.GetBucketName(remotepath) + "/" + s3o.Key)
                    lvi.RemoteLength = s3o.Size
                    lvi.RemoteMD5Hash = s3o.ETag.Trim(""""c)
                    lvi.RemoteUntrustedLastmodifiedTimeUTC = DateTimeOffset.Parse(s3o.LastModified)
                    remotefiles.Remove("/" + S3FileDialog.GetBucketName(remotepath) + "/" + s3o.Key)
                    localfiles.Remove(IO.Path.Combine(localpath, s3o.Key.Substring(lor.Prefix.Length).Replace("/"c, IO.Path.DirectorySeparatorChar)))
                End If
            Next
        Loop While resp.IsTruncated
        For Each kvp As KeyValuePair(Of String, S5ListViewItem) In localfiles
            kvp.Value.RowCompareResult = CompareResult.COMPARERESULT_REMOTE_MISSING
            kvp.Value.UpdateLvi(CurrentStrategy, CurrentCannedAcl)
        Next
        If lvStatus.Items.Count > 0 Then
            Compare.Enabled = True
            Synchronize.Enabled = True
        End If
        Add.Text = "Add (" & lvStatus.Items.Count & ")"
        Me.Cursor = Me.DefaultCursor
    End Sub

    Private Function CurrentCannedAcl() As S3CannedACL
        Return DirectCast([Enum].Parse(GetType(Amazon.S3.Model.S3CannedACL), cbxAcl.SelectedItem.ToString), S3CannedACL)
    End Function

    Private Sub S5Form_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.AccessKeyID = KeyId.Text
        My.Settings.Acl = cbxAcl.SelectedIndex
        My.Settings.Action = cbxAction.SelectedIndex
        My.Settings.DontDownloadMetadata = DontDownloadMetadata.Checked
        My.Settings.DontHashFiles = DontHashFiles.Checked
        My.Settings.LocalPath = txtLocalPath.Text
        My.Settings.RemotePath = txtRemotePath.Text
        My.Settings.SecretAccessKey = keySecret.Text
        My.Settings.Save()
    End Sub

    Private Sub S5_Load(sender As Object, e As EventArgs) Handles Me.Load
        KeyId.Text = My.Settings.AccessKeyID
        keySecret.Text = My.Settings.SecretAccessKey
        txtLocalPath.Text = My.Settings.LocalPath
        txtRemotePath.Text = My.Settings.RemotePath
        For Each kvp As KeyValuePair(Of String, Strategy) In Strategy.NameToStrategy
            cbxAction.Items.Add(kvp.Key)
        Next
        cbxAction.SelectedIndex = My.Settings.Action
        For Each acl As String In [Enum].GetNames(GetType(Amazon.S3.Model.S3CannedACL))
            cbxAcl.Items.Add(acl)
        Next
        cbxAcl.SelectedIndex = My.Settings.Acl
        DontDownloadMetadata.Checked = My.Settings.DontDownloadMetadata
        DontHashFiles.Checked = My.Settings.DontHashFiles
    End Sub

    Async Function CompareThenSynchronizeLvi(lvi As S5ListViewItem) As Task
        Dim s3client As New Amazon.S3.AmazonS3Client(KeyId.Text, keySecret.Text)
        If lvi.RowCompareResult = CompareResult.COMPARERESULT_UNKNOWN Then
            Await lvi.CompareLviAsync(s3client, DontDownloadMetadata.Checked, CurrentStrategy, CurrentCannedAcl, DontHashFiles.Checked)
        End If
        Await lvi.SynchronizeLviAsync(s3client, CurrentStrategy, CurrentCannedAcl)
    End Function

    Private Sub Compare_Click(sender As Object, e As EventArgs) Handles Compare.Click
        Dim CompareTasks As New List(Of Task)
        Dim ComparedCount As Integer = 0
        Dim s3client As New Amazon.S3.AmazonS3Client(KeyId.Text, keySecret.Text)
        For Each lvi As S5ListViewItem In lvStatus.Items
            If lvi.RowCompareResult = CompareResult.COMPARERESULT_UNKNOWN Then
                CompareTasks.Add(lvi.CompareLviAsync(s3client, DontDownloadMetadata.Checked, CurrentStrategy, CurrentCannedAcl, DontHashFiles.Checked))
            Else
                ComparedCount += 1
            End If
        Next
        Task.Run(Async Function()
                     Dim LastUpdate As DateTime = Now
                     Do While CompareTasks.Count > 0
                         Dim FinishedTask As Task = Await Task.WhenAny(CompareTasks)
                         CompareTasks.Remove(FinishedTask)
                         ComparedCount += 1
                         If Now > LastUpdate.AddMilliseconds(250) Then
                             Compare.Invoke(Sub() Compare.Text = "Compare (" & ComparedCount & ")")
                             LastUpdate = Now
                         End If
                     Loop
                     Compare.Invoke(Sub() Compare.Text = "Compare (" & ComparedCount & ")")
                 End Function)
    End Sub

    Private Sub Synchronize_Click(sender As Object, e As EventArgs) Handles Synchronize.Click
        Dim SynchTasks As New List(Of Task)
        For Each lvi As S5ListViewItem In lvStatus.Items
            SynchTasks.Add(CompareThenSynchronizeLvi(lvi))
        Next
        Task.Run(Async Function()
                     Dim LastUpdate As DateTime = Now
                     Dim SynchedCount As Integer = 0
                     Do While SynchTasks.Count > 0
                         Dim FinishedTask As Task = Await Task.WhenAny(SynchTasks)
                         SynchTasks.Remove(FinishedTask)
                         SynchedCount += 1
                         If Now > LastUpdate.AddMilliseconds(250) Then
                             Synchronize.Invoke(Sub() Synchronize.Text = "Synchronize (" & SynchedCount & ")")
                             LastUpdate = Now
                         End If
                     Loop
                     Synchronize.Invoke(Sub() Synchronize.Text = "Synchronize (" & SynchedCount & ")")
                 End Function)

    End Sub

    Private Sub cbxAcl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxAcl.SelectedIndexChanged
        For Each lvi As S5ListViewItem In lvStatus.Items
            lvi.UpdateLvi(CurrentStrategy, CurrentCannedAcl)
        Next

    End Sub

    Private Sub ClearList_Click(sender As Object, e As EventArgs) Handles ClearList.Click
        lvStatus.Items.Clear()
    End Sub

    Private Sub ClearCompare_Click(sender As Object, e As EventArgs) Handles ClearCompare.Click
        For Each lvi As S5ListViewItem In lvStatus.Items
            lvi.RowCompareResult = CompareResult.COMPARERESULT_UNKNOWN
            lvi.UpdateLvi(CurrentStrategy, CurrentCannedAcl)
        Next
    End Sub
End Class
