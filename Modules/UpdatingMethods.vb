Module UpdatingMethods

    ''' <summary>
    ''' States whether the user is currently moving the selected body
    ''' </summary>
    Public MovingBody As Boolean = False

    ''' <summary>
    ''' The sub in which all logic and calculations are performed
    ''' </summary>
    Sub Update(delta As Single)

        'Update the orbiting body if the simulation is playing but the mouse is idle
        If SelectedRadio = MouseFunction.AddOrbital Then
            UpdateGravitatingBody()
        End If

        'Set all accelerations to 0
        ResetAccelerations()

        'Stops graphics loop if there are no bodies to render
        If BodyArrayEmpty() Then Return

        'Multiply delta by the time factor so speed up or slow down
        delta *= Timescale

        'Calculate net forces -> calculate accelerations then update body velocities from newly calculated acclerations
        If GravitationEnabled Then
            UpdateAccelerations()
            UpdateVelocitiesFromAccelerations(delta)
        End If

        'Update the positions from the bodies' velocities
        UpdatePositions(delta)

        'Manage collisions if collisions are checked on the UI
        JoinBodies()

        'Update trails buffer
        Trails.UpdateTrails()

    End Sub

    ''' <summary>
    ''' Zeros all bodies' accelerations
    ''' </summary>
    Private Sub ResetAccelerations()
        Array.ForEach(BodyDatas, Sub(x) x.Acceleration = New Vector())
    End Sub

    ''' <summary>
    ''' Calculates the net acceleration for all bodies
    ''' </summary>
    Private Sub UpdateAccelerations()

        For i1 As Integer = 0 To BodyDatas.Length - 2

            For i2 As Integer = i1 + 1 To BodyDatas.Length - 1

                'Declare the two centers of body1 and body2
                Dim p1 As PointF = BodyDatas(i1).Center
                Dim p2 As PointF = BodyDatas(i2).Center

                'Vector from body1 to body2
                Dim p1p2 As Vector = Vector.VectorBetween(p1, p2)

                'Distance between body1 and body2 centers
                Dim p1p2Distance = p1p2.Magnitude()

                'Declare the two masses
                Dim m1 As Single = BodyDatas(i1).Mass
                Dim m2 As Single = BodyDatas(i2).Mass

                'ForceConstantVector allows calculating the top part of the force for two bodies only once (reduces calculations)
                Dim ForceConstantVector As Vector

                If p1p2Distance = 0 Then
                    'Prevents divide by 0 error
                    'If it is being divided by 0 then just return 0
                    'Note that this shouldn't ever happen but just to be safe
                    ForceConstantVector = New Vector()
                Else
                    'Set the top of the equation
                    ForceConstantVector = (BigG / p1p2Distance ^ 3) * p1p2
                End If

                'Perform calculations for the body pair to reduce computation by a half
                BodyDatas(i1).Acceleration += m2 * ForceConstantVector
                BodyDatas(i2).Acceleration += -m1 * ForceConstantVector

            Next
        Next
    End Sub

    ''' <summary>
    ''' Changes the velocities due to the accelerations
    ''' </summary>
    Private Sub UpdateVelocitiesFromAccelerations(delta As Single)
        For i As Integer = 0 To BodyDatas.Length - 1
            BodyDatas(i).Velocity += delta * BodyDatas(i).Acceleration
        Next
    End Sub

    ''' <summary>
    ''' Updates the positions from the velocities
    ''' </summary>
    Private Sub UpdatePositions(delta As Single)
        'This procedure uses a WHILE loop instead of a for because the number of iterations
        'can change part way through if a body has moved outside of the scene bounds (1E35 units)
        Dim i = 0
        While i < BodyDatas.Count
            'This IF statement checks if this is the body which is currently
            'being moved by the user, doesn't change the position if so
            If Not (MovingBody And i = SelectedBodyIndex) Then
                Try
                    'Try to update the position with the exception flag as True
                    BodyDatas(i).Center(True) += delta * BodyDatas(i).Velocity
                Catch ex As OverflowException
                    'So that if an exception is caught then it means that the body has moved outside of
                    'the scene bounds
                    RemoveBody(i)
                    i -= 1
                End Try
            End If
            i += 1
        End While
    End Sub

    ''' <summary>
    ''' Finds all the bodies which are intersecting and performs a join on them
    ''' </summary>
    ''' <return>If the method joins any bodies then returns True, otherwise False</return>
    Public Function JoinBodies() As Boolean

        'This variable was needed to check if any joinings occured
        Dim Result As Boolean = False

        Dim i1 = 0
        While i1 < BodyDatas.Length - 1

            Dim i2 = i1 + 1
            While i2 < BodyDatas.Length

                If BodyDatas(i1).Intersecting(BodyDatas(i2)) Then
                    JoinTwoBodies(i1, i2)
                    'Update the result to say that bodies have joined
                    Result = True
                    'bodyi2 becomes removed so the index needs to decrement
                    i2 -= 1
                End If

                i2 += 1
            End While

            i1 += 1
        End While

        Return Result

    End Function

    ''' <summary>
    ''' Joins the bodies at the two specified indexes. Replaces the first body with the joined body and removes the second.
    ''' </summary>
    Private Sub JoinTwoBodies(i1 As Integer, i2 As Integer)

        'Calculate the mass of the new body (add two body's masses)
        Dim SumOfMasses As Single = BodyDatas(i1).Mass + BodyDatas(i2).Mass

        'Add the momentums of each body to calculate the new momentum
        'Divide the new momentum by the new mass (p=mv) to get the new velocity
        Dim NewVelocity As Vector = (BodyDatas(i1).Mass * BodyDatas(i1).Velocity + BodyDatas(i2).Mass * BodyDatas(i2).Velocity) / SumOfMasses

        'Calculate the unit normal (direction of movement)
        Dim VectorBody1Body2 As Vector = Vector.VectorBetween(BodyDatas(i1).Center, BodyDatas(i2).Center)

        'Calculate by how much the large body should move by using proportions
        Dim Body1DisplaceVector As Vector = VectorBody1Body2.ChangeMagnitude(BodyDatas(i2).Mass / SumOfMasses * VectorBody1Body2.Magnitude)

        'Add vector displacement to position
        Dim NewPosition As PointF = Body1DisplaceVector + BodyDatas(i1).Center

        'Calculate new colour by mixing the two colours weighted by their radii
        Dim NewColour As Color = MixBodyColours(BodyDatas(i2), BodyDatas(i1))

        'If the more massive body is using an image then preserve that image for the joined body
        Dim NewIsUsingBitmap As Boolean
        Dim NewImage As Image
        If BodyDatas(i1).Mass > BodyDatas(i2).Mass And BodyDatas(i1).IsUsingBitmap Then
            NewIsUsingBitmap = True
            NewImage = BodyDatas(i1).Image
        ElseIf BodyDatas(i2).Mass > BodyDatas(i1).Mass And BodyDatas(i2).IsUsingBitmap Then
            NewIsUsingBitmap = True
            NewImage = BodyDatas(i2).Image
        End If

        'Set body1 equal to the joined body
        With BodyDatas(i1)
            .Mass = SumOfMasses
            .Velocity = NewVelocity
            .Center = NewPosition
            .Colour = NewColour
            .IsUsingBitmap = NewIsUsingBitmap
            If NewIsUsingBitmap Then
                .Image = NewImage
            End If
        End With

        'Then remove the second body
        RemoveBody(i2)

    End Sub

    ''' <summary>
    ''' Used by JoinBodies to calculate the new colour of two bodies propertional to their radius
    ''' </summary>
    Private Function MixBodyColours(body1 As Body, body2 As Body) As Color

        Dim r1 = body1.Radius
        Dim r2 = body2.Radius

        Dim c1 = body1.Colour
        Dim c2 = body2.Colour

        Dim SumOfRadii As Double = r1 + r2

        Dim R = (c1.R * r1 + c2.R * r2) / SumOfRadii
        Dim G = (c1.G * r1 + c2.G * r2) / SumOfRadii
        Dim B = (c1.B * r1 + c2.B * r2) / SumOfRadii

        Return Color.FromArgb(R, G, B)
    End Function

End Module
