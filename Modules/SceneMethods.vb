Module SceneMethods

    Private _ZoomScale As Single = 10 ^ -5
    Private _ViewPosition As New PointF(0, 0)

    ''' <summary>
    ''' Gets and sets the current value for the zoom of the view (min is 10 ^ -8, max is 10 ^ -3) (default is 10 ^ -5)
    ''' </summary>
    Public Property ZoomScale As Single
        Get
            Return _ZoomScale
        End Get
        Set(value As Single)
            ForceMinBound(value, 10 ^ -8)
            ForceMaxBounds(value, 10 ^ -3, False)
            _ZoomScale = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the ZoomScale as a percentage of its initial value
    ''' </summary>
    ''' <returns></returns>
    Public Function GetZoomScalePercentage() As Single
        Return ZoomScale / (10 ^ -5) * 100
    End Function

    ''' <summary>
    ''' Gets and sets the position of the viewport on the scene (max is 10 ^ 35)
    ''' </summary>
    Public Property ViewPosition As PointF
        Get
            Return _ViewPosition
        End Get
        Set(value As PointF)

            Dim ViewSceneWidth As Single = RenderDistanceToSceneDistance(RenderBoxSize.Width)
            Dim ViewSceneHeight As Single = RenderDistanceToSceneDistance(RenderBoxSize.Height)

            Dim NewSceneViewRectangle As New RectangleF(value.X, value.Y, ViewSceneWidth, ViewSceneHeight)

            If NewSceneViewRectangle.Left < -10 ^ 35 Then
                NewSceneViewRectangle.X = -10 ^ 35
            ElseIf NewSceneViewRectangle.Right > 10 ^ 35 Then
                NewSceneViewRectangle.X = 10 ^ 35 - ViewSceneWidth
            End If

            If NewSceneViewRectangle.Top < -10 ^ 35 Then
                NewSceneViewRectangle.Y = -10 ^ 35
            ElseIf NewSceneViewRectangle.Bottom > 10 ^ 35 Then
                NewSceneViewRectangle.Y = 10 ^ 35 - ViewSceneHeight
            End If

            _ViewPosition = NewSceneViewRectangle.Location

        End Set
    End Property

    ''' <summary>
    ''' The current size of the viewport
    ''' </summary>
    Public RenderBoxSize As New Size()

    ''' <summary>
    ''' Gets and sets the time scale in terms of seconds (default is 3600 seconds / 1 hour)
    ''' </summary>
    Private _Timescale As Single = 3600
    Public Property Timescale As Single
        Get
            Return _Timescale
        End Get
        Set(value As Single)
            ForceMaxBounds(value, 1576800000, False)
            ForceMinBound(value, 1)
            _Timescale = value
        End Set
	End Property

    ''' <summary>
    ''' The value of the universal gravitational constant used in the simulation
    ''' </summary>
    Public Const BigG As Single = 6.6740831 * 10 ^ -11

    ''' <summary>
    ''' A UI variable. States whether the velocity line should be rendered
    ''' </summary>
    Private _ShowVelocity As Boolean = True
    Public Property ShowVelocity As Boolean
        Get
            Return _ShowVelocity
        End Get
        Set(value As Boolean)
            _ShowVelocity = value
        End Set
    End Property

    ''' <summary>
    ''' A UI variable. States whether the trails should be rendered
    ''' </summary>
    Public ShowTrails As Boolean = True

    ''' <summary>
    ''' A UI variable. States whether the simulation should involve gravitation in the interactions
    ''' </summary>
    Public GravitationEnabled As Boolean = True

    ''' <summary>
    ''' Calculates the render point which will represent the given scene point
    ''' </summary>
    Function ScenePointToRenderPoint(scenePoint As PointF) As PointF
        Return New PointF(
            ZoomScale * (scenePoint.X - ViewPosition.X),
            ZoomScale * (scenePoint.Y - ViewPosition.Y)
        )
    End Function

    ''' <summary>
    ''' Calculates the point on the scene which is represented by the given render point
    ''' </summary>
    Function RenderPointToScenePoint(renderPoint As PointF) As PointF
        Return New PointF(
            (renderPoint.X / ZoomScale) + ViewPosition.X,
            (renderPoint.Y / ZoomScale) + ViewPosition.Y
        )
    End Function

    ''' <summary>
    ''' Calculates the render distance of the given scene distance
    ''' </summary>
    Function SceneDistanceToRenderDistance(SceneDistance As Single) As Integer
        Return Math.Round(SceneDistance * ZoomScale)
    End Function

    ''' <summary>
    ''' Calculates the scene distance of the given render distance
    ''' </summary>
    Function RenderDistanceToSceneDistance(RenderDistance As Integer) As Single
        Return RenderDistance / ZoomScale
    End Function

    ''' <summary>
    ''' Calculates the distance between the two specified points
    ''' </summary>
    Function DistanceBetween(p1 As PointF, p2 As PointF) As Single
        Return Vector.VectorBetween(p1, p2).Magnitude
    End Function

    ''' <summary>
    ''' Calculates and returns the index of the body which has the greatest gravitational force on the specified scene point
    ''' </summary>
    Public Function GetMostForcefulBody(ScenePoint As PointF) As Integer

        Dim BodyGreatestForce As Integer = -1
        Dim GreatestForce As Single = -1

        For i As Integer = 0 To BodyDatas.Count - 1
            Dim CenterBody As PointF = BodyDatas(i).Center
            Dim DistanceSquared As Single = Vector.VectorBetween(CenterBody, ScenePoint).MagnitudeSquared

            Dim ForceMagnitude As Single = BodyDatas(i).Mass / DistanceSquared

            If GreatestForce = -1 Then
                GreatestForce = ForceMagnitude
                BodyGreatestForce = i
            Else
                If ForceMagnitude > GreatestForce Then
                    GreatestForce = ForceMagnitude
                    BodyGreatestForce = i
                End If
            End If
        Next

        Return BodyGreatestForce

    End Function

    ''' <summary>
    ''' Finds whether the specified point is inside the given rectangle
    ''' </summary>
    Public Function InsideRectangle(pt As PointF, rect As RectangleF) As Boolean
        If pt.X > rect.Left And pt.X < rect.Right And
           pt.Y > rect.Top And pt.Y < rect.Bottom Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Returns the viewport as a rectangle on the scene
    ''' </summary>
    Public Function GetViewportSceneRectangle() As RectangleF
        Return New RectangleF(
            ViewPosition.X,
            ViewPosition.Y,
            RenderDistanceToSceneDistance(RenderBoxSize.Width),
            RenderDistanceToSceneDistance(RenderBoxSize.Height)
        )
    End Function

End Module
