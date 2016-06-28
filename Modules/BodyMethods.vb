Module BodyMethods

    ''' <summary>
    ''' The array in which all the bodies in the scene are stored
    ''' </summary>
    Public BodyDatas() As Body

    ''' <summary>
    ''' Holds the index of the currently selected body
    ''' </summary>
    Public SelectedBodyIndex As Integer

    ''' <summary>
    ''' Returns whether the body array has no bodies in it
    ''' </summary>
    Public Function BodyArrayEmpty() As Boolean
        If IsNothing(BodyDatas) Then
            Return True
        ElseIf BodyDatas.Length = 0 Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Adds the passed body to the BodyDatas array, resizes the body array when appropriate and creates space for a trail for the new body
    ''' </summary>
    Public Sub AddBody(b As Body)

        'Resize the body array to fit the new body
        If IsNothing(BodyDatas) Then
            ReDim BodyDatas(0)
            BodyDatas(0) = b
        Else
            ReDim Preserve BodyDatas(BodyDatas.Length)
            BodyDatas(BodyDatas.Length - 1) = b
        End If

        'The value is -1 when there are no bodies in the array so select the just-added body
        If SelectedBodyIndex = -1 Then
            SelectedBodyIndex = BodyDatas.Length - 1
        End If

        'Declare in Trails that there is a new body
        Trails.AddBodyTrail()

        'Show the changes on the form
        MainForm.UpdateUI()

    End Sub

    ''' <summary>
    ''' Removes the body and removes the trail at the given index
    ''' </summary>
    Public Sub RemoveBody(i As UInteger)
        'Let Trails know that a body is to be removed
        Trails.RemoveBodyTrail(i)

        'Convert the body array to a list in order to remove List's RemoveAt method
        Dim asList As List(Of Body) = BodyDatas.ToList()
        asList.RemoveAt(i)

        'Convert it back and update the array
        BodyDatas = asList.ToArray()

        'Ensure that the currently selected body is in the bounds of the array
        If SelectedBodyIndex >= BodyDatas.Length Then
            SelectedBodyIndex = BodyDatas.Length - 1
        End If

        'Fixes a bug caused because the program made a new instance of MainForm when MainForm was referenced.
        'This made it set the checked properties of the checkboxes,
        'The handler of these checkbox's CheckedChanged event set a global (shared) variable equal to its checked property.
        'This changed the global variables read by the simulation so for example,
        'the velocity lines would show and the gravity become enabled regardless of the state of the checkboxes
        'on the main MainForm instance.
        If Threading.Thread.CurrentThread.Name = "MainForm Thread" Then
            MainForm.UpdateUI()
        End If

    End Sub

End Module
