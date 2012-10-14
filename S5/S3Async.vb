Imports Amazon.S3.Model

Public Module S3Async
    <System.Runtime.CompilerServices.Extension>
    Function ListBucketsAsync(a As Amazon.S3.AmazonS3Client) As Task(Of Amazon.S3.Model.ListBucketsResponse)
        Return Task.Factory.FromAsync(AddressOf a.BeginListBuckets, AddressOf a.EndListBuckets, Nothing)
    End Function

    <System.Runtime.CompilerServices.Extension>
    Function ListObjectsAsync(a As Amazon.S3.AmazonS3Client, l As Amazon.S3.Model.ListObjectsRequest) As Task(Of Amazon.S3.Model.ListObjectsResponse)
        Return Task.Factory.FromAsync(AddressOf a.BeginListObjects, AddressOf a.EndListObjects, l, Nothing)
    End Function

    <System.Runtime.CompilerServices.Extension>
    Function GetObjectMetadataAsync(a As Amazon.S3.AmazonS3Client, l As Amazon.S3.Model.GetObjectMetadataRequest) As Task(Of Amazon.S3.Model.GetObjectMetadataResponse)
        Return Task.Factory.FromAsync(AddressOf a.BeginGetObjectMetadata, AddressOf a.EndGetObjectMetadata, l, Nothing)
    End Function

    <System.Runtime.CompilerServices.Extension>
    Function PutObjectAsync(a As Amazon.S3.AmazonS3Client, l As Amazon.S3.Model.PutObjectRequest) As Task(Of Amazon.S3.Model.PutObjectResponse)
        Return Task.Factory.FromAsync(AddressOf a.BeginPutObject, AddressOf a.EndPutObject, l, Nothing)
    End Function

    <System.Runtime.CompilerServices.Extension>
    Function DeleteObjectAsync(a As Amazon.S3.AmazonS3Client, l As Amazon.S3.Model.DeleteObjectRequest) As Task(Of Amazon.S3.Model.DeleteObjectResponse)
        Return Task.Factory.FromAsync(AddressOf a.BeginDeleteObject, AddressOf a.EndDeleteObject, l, Nothing)
    End Function

    <System.Runtime.CompilerServices.Extension>
    Function CopyObjectAsync(a As Amazon.S3.AmazonS3Client, l As Amazon.S3.Model.CopyObjectRequest) As Task(Of Amazon.S3.Model.CopyObjectResponse)
        Return Task.Factory.FromAsync(AddressOf a.BeginCopyObject, AddressOf a.EndCopyObject, l, Nothing)
    End Function

    <System.Runtime.CompilerServices.Extension>
    Function GetObjectAsync(a As Amazon.S3.AmazonS3Client, l As Amazon.S3.Model.GetObjectRequest) As Task(Of Amazon.S3.Model.GetObjectResponse)
        Return Task.Factory.FromAsync(AddressOf a.BeginGetObject, AddressOf a.EndGetObject, l, Nothing)
    End Function


    <System.Runtime.CompilerServices.Extension>
    Function ContainsKey(keys As System.Collections.Specialized.NameObjectCollectionBase.KeysCollection, ByVal s As String) As Boolean
        For Each key As String In keys
            If key = s Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function Interleaved(ByVal tasks As IEnumerable(Of Task)) As Task(Of Task)()
        Dim inputTasks As List(Of Task) = tasks.ToList
        Dim buckets(0 To inputTasks.Count - 1) As TaskCompletionSource(Of Task)
        Dim results(0 To buckets.Length - 1) As Task(Of Task)
        Dim i As Integer = 0
        Do While (i < buckets.Length)
            buckets(i) = New TaskCompletionSource(Of Task)
            results(i) = buckets(i).Task
            i += 1
        Loop
        Dim nextTaskIndex As Integer = -1
        Dim continuation As Action(Of Task) = Sub(completed)
                                                  Dim bucket As TaskCompletionSource(Of Task) = buckets(Interlocked.Increment(nextTaskIndex))
                                                  bucket.TrySetResult(completed)
                                              End Sub

        For Each inputTask As Task In inputTasks
            inputTask.ContinueWith(continuation, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default)
        Next
        Return results
    End Function

    Async Function CTypeAsync(Of Tinput, TOutput As Tinput)(a As Task(Of Tinput)) As Task(Of TOutput)
        Return CType(Await a, TOutput)
    End Function
End Module
