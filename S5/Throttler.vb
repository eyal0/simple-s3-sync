Class Throttler
    Property MaxCount As Integer

    Sub New(Optional MaxCount As Integer = 1)
        Me.MaxCount = MaxCount
    End Sub

    Private RunningSemaphore As New Semaphore(1, 1)
    Private Running As New List(Of Task)
    Private Waiting As New Concurrent.ConcurrentQueue(Of System.Tuple(Of Func(Of Task(Of Object)), TaskCompletionSource(Of Object)))
    Private AlreadyWaiting As Boolean

    Async Sub MakeWaiter()
        If AlreadyWaiting Then Exit Sub
        AlreadyWaiting = True
        Do While Waiting.Count > 0
            Dim CurrentWait As System.Tuple(Of Func(Of Task(Of Object)), TaskCompletionSource(Of Object)) = Nothing
            Do While Running.Count < MaxCount AndAlso Waiting.TryDequeue(CurrentWait)
                Dim NewFunc As Func(Of Task(Of Object)) = CurrentWait.Item1
                Dim NewTask As Task(Of Object) = NewFunc()
                Dim CurrentTcs As TaskCompletionSource(Of Object) = CurrentWait.Item2
                NewTask.ContinueWith(Sub(t2 As Task(Of Object))
                                         If Not t2.IsFaulted Then
                                             CurrentTcs.SetResult(t2.Result)
                                         Else
                                             Throw t2.Exception
                                         End If
                                     End Sub)
                RunningSemaphore.WaitOne()
                Running.Add(NewTask)
                RunningSemaphore.Release()
            Loop
            If Waiting.Count > 0 Then
                Dim Waiter As Task(Of Task)

                Waiter = Task.WhenAny(Running)
                Dim WaiterTimer As DateTimeOffset = DateTimeOffset.Now
                Dim FinishedTask As Task = Await Waiter
                Await FinishedTask
                RunningSemaphore.WaitOne()
                Running.Remove(FinishedTask)
                RunningSemaphore.Release()
            End If
        Loop
        AlreadyWaiting = False
    End Sub

    Function Run(f As Func(Of Task(Of Object))) As Task(Of Object)
        Dim NewTcs As New TaskCompletionSource(Of Object)
        Waiting.Enqueue(New System.Tuple(Of Func(Of Task(Of Object)), TaskCompletionSource(Of Object))(f, NewTcs))
        MakeWaiter()
        Return CType(NewTcs.Task, Task(Of Object))
    End Function

    Function Run(Of TInput)(f As Func(Of TInput, Task), input As TInput) As Task
        Dim NewF As Func(Of Task)
        NewF = Function() As Task
                   Return f(input)
               End Function
        Return Me.Run(NewF)
    End Function

    Function Run(Of TInput)(f As Func(Of TInput, Task(Of Object)), input As TInput) As Task(Of Object)
        Dim NewF As Func(Of Task(Of Object))
        NewF = Function() As Task(Of Object)
                   Return f(input)
               End Function
        Return CType(Me.Run(NewF), Task(Of Object))
    End Function

    Function Run(f As Func(Of Task)) As Task
        Dim NewF As Func(Of Task(Of Object))
        NewF = Function() As Task(Of Object)
                   Return f().ContinueWith(Function(t As task) As Object
                                               Return Nothing
                                           End Function)
               End Function
        Return CType(Me.Run(NewF), Task(Of Object))
    End Function

    Function Run(Of TInput)(f As Func(Of TInput, Object), input As TInput) As Task(Of Object)
        Dim NewF As Func(Of Task(Of Object))
        NewF = Function() As Task(Of Object)
                   Return Task.Run(Function() As Object
                                       Return f(input)
                                   End Function)
               End Function
        Return CType(Me.Run(NewF), Task(Of Object))
    End Function

    Function Run(Of TInput)(f As Action(Of TInput), input As TInput) As Task
        Dim NewF As Func(Of Task)
        NewF = Function() As Task
                   Return Task.Run(Sub()
                                       f(input)
                                   End Sub)
               End Function
        Return Me.Run(NewF)
    End Function

    Function Run(f As Func(Of Object)) As Task(Of Object)
        Dim NewF As Func(Of Task(Of Object))
        NewF = Function() As Task(Of Object)
                   Return Task.Run(Function()
                                       Return f()
                                   End Function)
               End Function
        Return CType(Me.Run(NewF), Task(Of Object))
    End Function

    Function Run(f As Action) As Task
        Dim NewF As Func(Of Task)
        NewF = Function() As Task
                   Return Task.Run(Sub()
                                       f()
                                   End Sub)
               End Function
        Return Me.Run(NewF)
    End Function
End Class