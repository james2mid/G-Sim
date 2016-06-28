Module MouseMethods

    ''' <summary>
    ''' Holds a boolean value which states whether the mouse is down. Also used to bypass finalising operations when errors occur.
    ''' </summary>
    Private MouseIsDown As Boolean

    ''' <summary>
    ''' Used in Pan to calculate how much the mouse has moved and so how much to pan the view.
    ''' </summary>
    Private PreviousRenderMousePosition As Point

    ''' <summary>
    ''' Used in Move Body so that the cursor is always on the same point on the body.
    ''' </summary>
    Private ScenePointOnBody As PointF

    ''' <summary>
    ''' Holds the point on the view of the cursor position.
    ''' </summary>
    Public RenderMouse As Point

    ''' <summary>
    ''' Holds the value of the currently selected cursor function radio button on main form.
    ''' </summary>
    Public SelectedRadio As MouseFunction

    ''' <summary>
    ''' The delegate for all mouse subs in MouseMethods
    ''' </summary>
    Private Delegate Sub CursorSubDelegate(e As EventType)

    ''' <summary>
    ''' Enumeration which holds the different types of mouse events (Up, Move, Down)
    ''' </summary>
    Public Enum EventType
        Down = 1
        Move = 0
        Up = 2
    End Enum

    ''' <summary>
    ''' Holds values which correspond to the cursor functions on the main form.
    ''' </summary>
    Public Enum MouseFunction
        Empty = 0
        Pan
        AddBody
        AddOrbital
        SelectBody
        MoveBody
        ChangeVelocity
        ChangeMass
        RemoveBody
    End Enum

    ''' <summary>
    ''' Calculates which body should be selected given the render point of the mouse.
    ''' </summary>
    ''' <param name="RenderMouse">The render point of the mouse</param>
    ''' <param name="AcceptClosestBody">Defines whether the sub should allow selection of a body which is closest to the specified point</param>
    ''' <returns>If the method changes the SelectedBodyIndex then return True, else returns False.</returns>
    Private Function SelectBodyFromPoint(RenderMouse As Point, Optional AcceptClosestBody As Boolean = True) As Boolean

        Dim SceneMouse As PointF = RenderPointToScenePoint(RenderMouse)

        Dim ClosestBody As Integer = -1
        Dim ClosestDistanceSquared As Double = -1

        For i As Integer = 0 To BodyDatas.Length - 1
            Dim DistanceSquared As Double = Vector.VectorBetween(SceneMouse, BodyDatas(i).Center).MagnitudeSquared
            If DistanceSquared < BodyDatas(i).Radius ^ 2 Then
                SelectedBodyIndex = i
                Return True
            End If

            Dim SetBodyAsClosest = Sub()
                                       ClosestDistanceSquared = DistanceSquared
                                       ClosestBody = i
                                   End Sub

            If ClosestDistanceSquared = -1 Then
                SetBodyAsClosest()
            Else
                If DistanceSquared < ClosestDistanceSquared Then
                    SetBodyAsClosest()
                End If
            End If
        Next

        If AcceptClosestBody Then
            SelectedBodyIndex = ClosestBody
            Return True
        End If

        Return False

    End Function

    ''' <summary>
    ''' Updates the Orbiting Body index and changes the mouse label
    ''' </summary>
    Public Sub UpdateGravitatingBody()
        'If the user has the mouse held down dragging a body and then moves their mouse near a different body
        'Then do not update to set that body as the orbiting body
        If Not MouseIsDown Then

            Dim SceneMouse As PointF = RenderPointToScenePoint(RenderMouse)
            OrbitingBodyIndex = GetMostForcefulBody(SceneMouse)

            MouseLabelText = DistanceBetween(BodyDatas(OrbitingBodyIndex).Center, SceneMouse).ToString + "m"

        End If
    End Sub

    ''' <summary>
    ''' Runs the neccessary cursor sub by referencing the SelectedRadio global
    ''' </summary>
    ''' <param name="e">The type of mouse event (MouseDown, MouseMove or MouseUp)</param>
    ''' <param name="RenderMouse">The render point of the cursor</param>
    Sub RunCursorSub(e As EventType, RenderMouse As Point)

        MouseMethods.RenderMouse = RenderMouse

        If e = EventType.Down Then
            MouseIsDown = True
        ElseIf e = EventType.Up Then
            If MouseIsDown Then
                MouseIsDown = False
            Else
                Return
            End If

        ElseIf e = EventType.Move Then
            'Whether the mouse is down or not is handled within the cursor subs
        End If

        Dim SubToExecute As CursorSubDelegate

        Select Case SelectedRadio
            Case MouseFunction.Pan
                SubToExecute = AddressOf CursorPan
            Case MouseFunction.AddBody
                SubToExecute = AddressOf CursorAddBody
            Case MouseFunction.AddOrbital
                SubToExecute = AddressOf CursorAddOrbitalBody
            Case MouseFunction.SelectBody
                SubToExecute = AddressOf CursorSelectBody
            Case MouseFunction.MoveBody
                SubToExecute = AddressOf CursorMoveBody
            Case MouseFunction.ChangeVelocity
                SubToExecute = AddressOf CursorChangeVelocity
            Case MouseFunction.ChangeMass
                SubToExecute = AddressOf CursorChangeMass
            Case MouseFunction.RemoveBody
                SubToExecute = AddressOf CursorRemoveBody
        End Select

        SubToExecute(e)

    End Sub

    ''' <summary>
    ''' Zooms the view in or out about the mouse pointer depending on the passed Delta parameter
    ''' </summary>
    Public Sub ZoomInOut(Delta As Integer, RenderMouse As Point)

        Dim ScenePointCursorBeforeZoom As PointF = RenderPointToScenePoint(RenderMouse)

        If Delta < 0 Then
            ZoomScale *= 1 + 1 / 50
        ElseIf Delta > 0 Then
            ZoomScale /= 1 + 1 / 50
        ElseIf Delta = 0 Then
            Return
        End If

        Dim ScenePointCursorAfterZoom = ScenePointCursorBeforeZoom

        Dim RenderPointCursorAfterZoom As PointF = ScenePointToRenderPoint(ScenePointCursorAfterZoom)

        Dim RenderPointTopLeftAfterZoom As New PointF(
            RenderPointCursorAfterZoom.X - RenderMouse.X,
            RenderPointCursorAfterZoom.Y - RenderMouse.Y
        )

        ViewPosition = RenderPointToScenePoint(RenderPointTopLeftAfterZoom)

    End Sub

#Region "Private Mouse Methods"

    ''' <summary>
    ''' Moves the viewport around
    ''' </summary>
    Private Sub CursorPan(e As EventType)

        If e = EventType.Down Then
            PreviousRenderMousePosition = RenderMouse

        ElseIf e = EventType.Move And MouseIsDown Then

            Dim RenderMouseChange As New Size(
                    RenderMouse.X - PreviousRenderMousePosition.X,
                    RenderMouse.Y - PreviousRenderMousePosition.Y
                )

            ViewPosition = New PointF(
                ViewPosition.X - RenderDistanceToSceneDistance(RenderMouseChange.Width),
                ViewPosition.Y - RenderDistanceToSceneDistance(RenderMouseChange.Height)
            )

            PreviousRenderMousePosition = RenderMouse

        ElseIf e = EventType.Up Then

        End If

    End Sub

    ''' <summary>
    ''' Adds a body to the array whos center is the origional mouse position and whos size is however far the user has dragged the mouse
    ''' </summary>
    Private Sub CursorAddBody(e As EventType)
        If e = EventType.Down Then
            GameLoop.StoreAndPause()

            'Check if they're trying to add a body inside a body and stop them if so

            If Not BodyArrayEmpty() Then
                Dim SceneMouse As PointF = RenderPointToScenePoint(RenderMouse)

                For i = 0 To BodyDatas.Length - 1
                    If DistanceBetween(SceneMouse, BodyDatas(i).Center) < BodyDatas(i).Radius Then
                        MsgBox("Creating a body inside another body gives unexpected results so it has been disabled in this application.")
                        MouseIsDown = False
                        GameLoop.ContinueStored()
                        Return
                    End If
                Next

            End If

            Dim body As New Body()
            With body
                .Colour = GetRandomColour()
                .Mass = 0

                Try
                    .Center(True) = RenderPointToScenePoint(RenderMouse)
                Catch ex As OverflowException
                    MsgBox("You can't create a body here. The maximum position of the center is +/- 10 ^ 35 in either the x or the y direction.")
                    MouseIsDown = False
                    GameLoop.ContinueStored()
                    Return
                End Try

                .Velocity = New Vector(0, 0)
            End With

            AddBody(body)
            SelectedBodyIndex = BodyDatas.Count - 1

            MouseLabelText = ""
            ShowMouseLabel = True

        ElseIf e = EventType.Move And MouseIsDown Then

            CursorChangeMass(EventType.Move)

        ElseIf e = EventType.Up Then

            ShowMouseLabel = False

            If BodyDatas(SelectedBodyIndex).Mass = 0 Then
                RemoveBody(SelectedBodyIndex)
                GameLoop.PaintOnce()
                MsgBox("You cannot create a body with mass 0Kg. To create a body, move your cursor to where you want the center of the body to be and then drag to define its mass.", MsgBoxStyle.Information)
            Else
                Changes.AddChange(Changes.ChangeType.AddBody)
            End If

            GameLoop.PaintOnce()
            GameLoop.ContinueStored()

        End If

    End Sub

    ''' <summary>
    ''' Adds a body which is is created at the point of the users mouse. The user then drags to define the mass.
    ''' Calculates which the orbiting body should be by finding which planet has the greatest force on the mouse point.
    ''' </summary>
    Private Sub CursorAddOrbitalBody(e As EventType)

        If BodyArrayEmpty() Then Return

        Dim SceneMouse As PointF = RenderPointToScenePoint(RenderMouse)

        If e = EventType.Move And Not MouseIsDown Then

            PaintOrbital = True
            ShowMouseLabel = True
            UpdateGravitatingBody()

        ElseIf e = EventType.Down Then

            'The loop is paused in this method
            CursorAddBody(EventType.Down)

            PaintOrbital = False
            ShowMouseLabel = False

        ElseIf e = EventType.Move And MouseIsDown Then

            CursorChangeMass(EventType.Move)

        ElseIf e = EventType.Up Then

            If BodyDatas(SelectedBodyIndex).Mass = 0 Then
                RemoveBody(SelectedBodyIndex)
                MsgBox("You cannot create a body with mass 0Kg. To create a body, move your cursor to where you want the center of the body to be and then drag to define its mass.", MsgBoxStyle.Information)
            Else

                'Make a new vector from the orbiting body to the center of the orbital
                Dim VectorBodyToCursor As Vector = Vector.VectorBetween(BodyDatas(OrbitingBodyIndex).Center, SceneMouse)

                'Calculates the magnitude of the velocity of the orbital using v = √(GM/r)
                Dim OrbitalVelocityMagnitude As Single = Math.Sqrt(BigG * BodyDatas(OrbitingBodyIndex).Mass / VectorBodyToCursor.Magnitude)

                'Finds the velocity relative to the body that it is orbiting
                'Gets the tangent of the vector from the orbiting to the orbital and then makes the magnitude equal to the orbital velocity magnitude
                Dim RelativeVelocity As Vector = VectorBodyToCursor.Tangent.ChangeMagnitude(OrbitalVelocityMagnitude)

                'Set the new velocity as the calculated relative velocity plus the orbiting body velocity to make it an absolute value
                BodyDatas(SelectedBodyIndex).Velocity = RelativeVelocity + BodyDatas(OrbitingBodyIndex).Velocity

                Changes.AddChange(Changes.ChangeType.AddOrbital)
            End If

            GameLoop.ContinueStored()

        End If

    End Sub

    ''' <summary>
    ''' Selects a body if the mouse point is over it and the moves it around
    ''' </summary>
    Private Sub CursorMoveBody(e As EventType)

        If BodyArrayEmpty() Then Return

        If e = EventType.Down Then

            GameLoop.StoreAndPause()

            MovingBody = True

            If Not SelectBodyFromPoint(RenderMouse, False) Then
                MovingBody = False
                MouseIsDown = False
                Return
            End If

            Dim SceneMouse As PointF = RenderPointToScenePoint(RenderMouse)

            'Find where the mouse is on the body (from the center)
            ScenePointOnBody = New PointF() With {
                .X = SceneMouse.X - BodyDatas(SelectedBodyIndex).Center.X,
                .Y = SceneMouse.Y - BodyDatas(SelectedBodyIndex).Center.Y
            }

        ElseIf e = EventType.Move And MouseIsDown Then

            'Move the body to the correct place according to where the mouse is placed on it
            Dim SceneMouse = RenderPointToScenePoint(RenderMouse)

            BodyDatas(SelectedBodyIndex).Center = New PointF(
                SceneMouse.X - ScenePointOnBody.X,
                SceneMouse.Y - ScenePointOnBody.Y
            )

            JoinBodies()

            Trails.ClearTrail(SelectedBodyIndex)

        ElseIf e = EventType.Up Then
            MovingBody = False

            Changes.AddChange(Changes.ChangeType.MoveBody)

            GameLoop.ContinueStored()

        End If

    End Sub

    ''' <summary>
    ''' Changes the SelectedBodyIndex when the user clicks a body
    ''' </summary>
    Private Sub CursorSelectBody(e As EventType)

        If BodyArrayEmpty() Then Return

        If e = EventType.Down Then

        ElseIf e = EventType.Move And MouseIsDown Then

        ElseIf e = EventType.Up Then

            SelectBodyFromPoint(RenderMouse, False)

        End If

    End Sub

    ''' <summary>
    ''' Allows the user to drag from inside a body to set the endpoint of its velocity
    ''' </summary>
    Private Sub CursorChangeVelocity(e As EventType)

        If BodyArrayEmpty() Then Return

        If e = EventType.Down Then

            GameLoop.StoreAndPause()

            SelectBodyFromPoint(RenderMouse)

            ShowMouseLabel = True

            CursorChangeVelocity(EventType.Move)

        ElseIf e = EventType.Move And MouseIsDown Then

            Dim RenderVelocityEndPoint As PointF = RenderMouse
            Dim SceneVelocityEndPoint As PointF = RenderPointToScenePoint(RenderVelocityEndPoint)
            Dim VelocityVector As Vector = Vector.VectorBetween(BodyDatas(SelectedBodyIndex).Center, SceneVelocityEndPoint)

            Try
                BodyDatas(SelectedBodyIndex).Velocity(True) = 1 / 3600 * VelocityVector

            Catch ex As Exception
                'If this code runs then the velocity is out of bounds
                MouseIsDown = False
                ShowMouseLabel = False

                GameLoop.PaintOnce()

                MsgBox("The maximum magnitude of velocity which can be set is the speed of light (299792458m/s).", MsgBoxStyle.Information)

            End Try

            MouseLabelText = BodyDatas(SelectedBodyIndex).Velocity.Magnitude.ToString + "m/s"

        ElseIf e = EventType.Up Then

            ShowMouseLabel = False

            GameLoop.PaintOnce()

            Changes.AddChange(Changes.ChangeType.ChangeVelocity)
            GameLoop.ContinueStored()

        End If
    End Sub

    ''' <summary>
    ''' Allows the user to drag from inside a body to change the size (and mass) of the body
    ''' </summary>
    Private Sub CursorChangeMass(e As EventType)

        If BodyArrayEmpty() Then Return

        If e = EventType.Down Then

            GameLoop.StoreAndPause()
            SelectBodyFromPoint(RenderMouse)

            MouseLabelText = ""
            ShowMouseLabel = True

        ElseIf e = EventType.Move And MouseIsDown Then

            Dim SceneMousePosition As PointF = RenderPointToScenePoint(RenderMouse)

            Dim RadiusVector As New Vector(
                SceneMousePosition.X - BodyDatas(SelectedBodyIndex).Center.X,
                SceneMousePosition.Y - BodyDatas(SelectedBodyIndex).Center.Y
            )

            Dim Radius As Single = RadiusVector.Magnitude()

            Try
                BodyDatas(SelectedBodyIndex).Mass(True) = Body.GetMassFromSize(Radius)

            Catch ex As Exception
                'Stop the mouse input subs thinking that the mouse is still down
                'Otherwise this code would run everytime the cursor was moved
                MouseIsDown = False
                ShowMouseLabel = False

                Changes.AddChange(Changes.ChangeType.ChangeMass)

                'Update the view
                GameLoop.PaintOnce()

                'Alert the user of the maximum error
                MsgBox("The maximum mass supported by the application is 10 ^ 35Kg. The body will be created with mass 10 ^ 35Kg.", MsgBoxStyle.Information)

            End Try

            MouseLabelText = BodyDatas(SelectedBodyIndex).Mass.ToString + "Kg"

            If JoinBodies() Then
                ShowMouseLabel = False
                MouseIsDown = False
                GameLoop.PaintOnce()
                MsgBox("One or more bodies have joined. To prevent undesired changes, G-Sim has paused what you were doing.", MsgBoxStyle.Information)

            End If

        ElseIf e = EventType.Up Then

            ShowMouseLabel = False

            If BodyDatas(SelectedBodyIndex).Mass = 0 Then
                My.Forms.MainForm.Invoke(Sub()
                                             My.Forms.MainForm.lbChanges.SelectedIndex = 0
                                         End Sub)
                GameLoop.PaintOnce()

                MsgBox("You cannot set the mass of a body to be 0Kg. The body has been restored to how it was before changing the mass.", MsgBoxStyle.Information)
                Return
            End If


            Changes.AddChange(Changes.ChangeType.ChangeMass)
            GameLoop.ContinueStored()

        End If

    End Sub

    ''' <summary>
    ''' Removes a body when the user clicks on it
    ''' </summary>
    Private Sub CursorRemoveBody(e As EventType)

        If BodyArrayEmpty() Then Return

        If e = EventType.Down Then

            GameLoop.StoreAndPause()

        ElseIf e = EventType.Move And MouseIsDown Then

        ElseIf e = EventType.Up Then

            If SelectBodyFromPoint(RenderMouse, False) Then
                RemoveBody(SelectedBodyIndex)
                Changes.AddChange(Changes.ChangeType.RemoveBody)
            End If

            GameLoop.ContinueStored()

        End If

    End Sub
#End Region

End Module
