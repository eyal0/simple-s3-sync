Imports System.Windows.Forms
Imports Amazon.S3.Model

Public Class S3FileDialog
    Dim keyId As String
    Dim keySecret As String
    Dim S3Client As Amazon.S3.AmazonS3Client
    Dim currentPath As String = "/" 'first element is always bucket name
    Dim selectedPath As String

    ReadOnly Property Path As String
        Get
            Return selectedPath
        End Get
    End Property

    Sub New(keyId As String, keySecret As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.keyId = keyId
        Me.keySecret = keySecret
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Async Sub S5FileDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            S3Client = New Amazon.S3.AmazonS3Client(keyId, keySecret)
            Dim BucketList As Amazon.S3.Model.ListBucketsResponse = Await S3Client.ListBucketsAsync()
            For Each b As Amazon.S3.Model.S3Bucket In BucketList.Buckets
                FileList.Items.Add(b.BucketName + "/")
            Next
        Catch ex As Amazon.S3.AmazonS3Exception
            MsgBox("Failed to browse Amazon S3:" + vbCrLf + ex.Message, MsgBoxStyle.Exclamation, "Failed to browse Amazon S3")
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End Try
    End Sub

    Private Async Sub FileList_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles FileList.MouseDoubleClick
        If currentPath <> "/" And FileList.SelectedIndex = 0 Then
            'going up a directory
            currentPath = GetParent(currentPath)
        Else
            currentPath += FileList.SelectedItem.ToString
        End If
        FileList.Items.Clear()
        If currentPath = "/" Then
            Dim BucketList As Amazon.S3.Model.ListBucketsResponse = Await S3Client.ListBucketsAsync()
            For Each b As Amazon.S3.Model.S3Bucket In BucketList.Buckets
                FileList.Items.Add(b.BucketName + "/")
            Next
        ElseIf currentPath.EndsWith("/"c) Then
            Dim lor As New ListObjectsRequest
            lor.BucketName = GetBucketName(currentPath)
            lor.Delimiter = "/"
            lor.Prefix = GetPrefix(currentPath)
            Dim t As Task(Of Amazon.S3.Model.ListObjectsResponse) = S3Client.ListObjectsAsync(lor)
            FileList.Items.Clear()
            FileList.Items.Add("..")
            For Each dir As String In (Await t).CommonPrefixes
                FileList.Items.Add(dir.Substring(lor.Prefix.Length))
            Next
            For Each file As String In (Await t).S3Objects.Select(Function(s3o As S3Object) s3o.Key)
                FileList.Items.Add(file.Substring(lor.Prefix.Length))
            Next
        Else
            'a file was selected
            selectedPath = currentPath
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub FileList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FileList.SelectedIndexChanged
        selectedPath = currentPath + FileList.SelectedItem.ToString
    End Sub

    Shared Function GetBucketName(Path As String) As String
        Path = Path.TrimStart("/"c)
        If Path.IndexOf("/"c) >= 0 Then
            Path = Path.Substring(0, Path.IndexOf("/"c))
        Else
            Path = ""
        End If
        Return Path
    End Function

    Shared Function GetPrefix(path As String) As String
        path = path.TrimStart("/"c)
        If path.IndexOf("/"c) >= 0 Then
            path = path.Substring(path.IndexOf("/"c) + 1)
        Else
            path = ""
        End If
        Return path
    End Function

    Shared Function GetParent(path As String) As String
        path = path.TrimEnd("/"c)
        If path.LastIndexOf("/"c) <> 0 Then
            path = path.Substring(0, path.LastIndexOf("/"c) + 1)
        End If
        Return path
    End Function

End Class
