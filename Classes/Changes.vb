Public Class Changes

    ''' <summary>
    ''' The list which holds the changes as the type Change (below)
    ''' </summary>
    Private Shared ChangeList As New List(Of Change)

    ''' <summary>
    ''' Holds the index of the currently selected / current state of the BodyDatas
    ''' </summary>
    Private Shared CurrentIndex As Integer = -1

    ''' <summary>
    ''' The event which is called upon the list being changed
    ''' </summary>
    Private Shared Event ListChanged()

    ''' <summary>
    ''' The constant which holds the max number of changes which should be stored
    ''' </summary>
    Private Const MaxStoredChanges As Byte = 100

    ''' <summary>
    ''' The constant which is used by the main form to decide how often the BodyDatas should automatically be stored while running.
    ''' </summary>
    Public Const AutoStoreWhileRunningIntervalMilliseconds As Integer = 5000

    ''' <summary>
    ''' Used locally to update the list box when adding ChangeType.SimulationRunTime
    ''' </summary>
    Private Const AutoStoreWhileRunningIntervalSeconds As Integer = AutoStoreWhileRunningIntervalMilliseconds / 1000

    Public Shared Sub AddChange(ChangeType As ChangeType)

        'If they are adding a change and the current state is part way through the array and not at the end
        'Then remove the newer changes
        RemoveNewerChanges(CurrentIndex)

        'Used in the next-code block
        Dim BodyDatas As Body()

        'If the current array is empty
        If BodyArrayEmpty() Then

            'Set the BodyDatas array to empty
            ReDim BodyDatas(0)

        Else

            'Otherwise fill up the array with shallow-cloned bodies
            'This prevents object reference
            ReDim BodyDatas(BodyMethods.BodyDatas.Length - 1)
            For i As Integer = 0 To BodyDatas.Length - 1
                BodyDatas(i) = BodyMethods.BodyDatas(i).Clone()
            Next

        End If

        'Used in the next code-block
        Dim RunTime As Long = 0

        'If it is a simulation run time change then calculate the seconds which should be displayed
        If ChangeType = ChangeType.SimulationRunTime Then

            'Find how long the simulation has been running for
            RunTime = AutoStoreWhileRunningIntervalSeconds

            Dim i = ChangeList.Count - 1
            Do While ChangeList(i).ChangeType = ChangeType.SimulationRunTime And i > 0
                RunTime += AutoStoreWhileRunningIntervalSeconds
                i -= 1
            Loop

        End If

        'Add the new change to the array
        ChangeList.Add(New Change(ChangeType, BodyDatas, RunTime))

        'Removes the oldest changes if the list is greater than the specified max
        RemoveExcess()

        'Update the current index to show that the current state it the latest added change
        CurrentIndex = ChangeList.Count - 1

        'Raise the event so that mainform updates its listbox of changes
        RaiseEvent ListChanged()

    End Sub

    Public Shared Sub RollbackChanges(OldestToNewestIndex As Integer)

        Trails.RemoveAllTrails()

        If Not IsNothing(ChangeList(OldestToNewestIndex).BodyDatas) Then
            If Not ChangeList(OldestToNewestIndex).BodyDatas.Count = 0 Then
                ReDim BodyDatas(ChangeList(OldestToNewestIndex).BodyDatas.Count - 1)
                For BodyDatasIndex As Integer = 0 To ChangeList(OldestToNewestIndex).BodyDatas.Count - 1
                    BodyDatas(BodyDatasIndex) = ChangeList(OldestToNewestIndex).BodyDatas(BodyDatasIndex).Clone()
                Next
            End If
        Else
            ReDim BodyDatas(0)
        End If

        If Not BodyArrayEmpty() Then
            For Each body In BodyDatas
                Trails.AddBodyTrail()
            Next
        End If

        CurrentIndex = OldestToNewestIndex

        RaiseEvent ListChanged()

    End Sub

    Public Shared Sub RemoveNewerChanges(AfterIndex As Integer)

        'If there are elements in the array after the specified index 
        If Not AfterIndex = ChangeList.Count - 1 Then

            'Remove a range starting from the element at the index after AfterIndex
            'And remove the amount of elements which is the difference between the last index and AfterIndex
            ChangeList.RemoveRange(AfterIndex + 1, ChangeList.Count - AfterIndex - 1)
            RaiseEvent ListChanged()

        End If

    End Sub

    Public Shared Sub RemoveExcess()
        If ChangeList.Count > MaxStoredChanges Then
            ChangeList.RemoveRange(0, ChangeList.Count - MaxStoredChanges)
        End If
    End Sub

    Public Shared Sub ChangesIntoListBox(ByRef lb As ListBox)

        If ChangeList.Count = 0 Then Return

        For i = ChangeList.Count - 1 To 0 Step -1

            If i = CurrentIndex Then
                lb.Items.Add(GetChangeString(ChangeList(i)) + " (Current)")
            Else
                lb.Items.Add(GetChangeString(ChangeList(i)))
            End If

        Next

    End Sub

    Private Shared Function GetChangeString(change As Change) As String

        Dim RunTime As Long = change.RunTime

        Select Case change.ChangeType
            Case ChangeType.AddBody
                Return "Added Body"
            Case ChangeType.AddOrbital
                Return "Added Orbital"
            Case ChangeType.MoveBody
                Return "Moved Body"
            Case ChangeType.ChangeVelocity
                Return "Changed Velocity"
            Case ChangeType.ChangeMass
                Return "Changed Mass"
            Case ChangeType.RemoveBody
                Return "Removed Body"
            Case ChangeType.SimulationRunTime
                Return "Simulation Run For " + GetTimeString(RunTime, 1)
            Case ChangeType.SimulationPaused
                Return "Simulation Paused"
            Case ChangeType.ClearSimulaton
                Return "Cleared Scene"
            Case ChangeType.LoadScene
                Return "Loaded Scene"
            Case Else
                Return ""
        End Select
    End Function

    Public Enum ChangeType
        AddBody
        AddOrbital
        MoveBody
        ChangeVelocity
        ChangeMass
        RemoveBody
        SimulationRunTime
        SimulationPaused
        ClearSimulaton
        LoadScene
    End Enum

    Private Shared Sub ListHasChanged() Handles Me.ListChanged
        MainForm.Invoke(New MethodInvoker(AddressOf MainForm.ChangeListHasChanged))
    End Sub

    Private Class Change
        Public ChangeType As ChangeType
        Public BodyDatas As Body()
        Public RunTime As Long

        Sub New(ChangeType As ChangeType, BodyDatas As Body(), RunTime As Long)
            Me.ChangeType = ChangeType
            Me.BodyDatas = BodyDatas
            Me.RunTime = RunTime
        End Sub
    End Class

End Class
