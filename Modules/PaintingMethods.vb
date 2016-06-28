Module PaintingMethods

    ''' <summary>
    ''' States whether paint should render an orbital ring
    ''' </summary>
    Public PaintOrbital As Boolean = False

    ''' <summary>
    ''' Holds the index of the body which the user wishes to create an orbital around
    ''' </summary>
    Public OrbitingBodyIndex As Integer = -1

    ''' <summary>
    ''' States whether paint should display a label next to the cursor with a text of value MouseLabel
    ''' </summary>
    Public ShowMouseLabel As Boolean = False

    ''' <summary>
    ''' The string to be rendered inside the label next to the cursor
    ''' </summary>
    Public MouseLabelText As String = ""

    ''' <summary>
    ''' The sub in which all graphics are rendered and then painted to the view
    ''' </summary>
    Sub Paint()

        ClearBuffer()

        'Render an empty scene (with information) if there are no bodies in the array
        If BodyArrayEmpty() Then
            DrawTopLeftString()
            RenderBuffer()
            Return
        End If

        'Draw trails on bottom
        If ShowTrails Then
            Trails.RenderTrails()
        End If

        'Draw orbit circle just above trails
        If PaintOrbital Then
            DrawOrbitalRing()
        End If

        'Draw bodies above trails and orbital ring
        DrawBodies()

        'Draw velocity arrow on top of bodies
        If ShowVelocity Then
            DrawVelocities()
        End If

        'Draw mouse label on top of everything else
        If ShowMouseLabel Then
            DrawMouseLabel()
        End If

        'Draw technical information on the top
        DrawTopLeftString()

        'Paint the buffer to the panel
        RenderBuffer()

    End Sub

    Private Sub DrawBodies()

        For i = 0 To BodyDatas.Length - 1
            'Don't render body if it's not visible (reduces processing)
            If BodyDatas(i).VisibleInView Then

                If BodyDatas(i).IsUsingBitmap Then
                    g.Graphics.DrawImage(BodyDatas(i).Image, BodyDatas(i).RenderRectangle)
                Else
                    g.Graphics.FillEllipse(New SolidBrush(BodyDatas(i).Colour), BodyDatas(i).RenderRectangle)
                End If
            End If
        Next

    End Sub

    Private Sub DrawOrbitalRing()

        Dim RenderCenter As Point

        Try
            RenderCenter = Point.Round(ScenePointToRenderPoint(BodyDatas(OrbitingBodyIndex).Center))
        Catch ex As IndexOutOfRangeException
            Return
        End Try

        Dim RenderRadius As Integer = DistanceBetween(RenderCenter, RenderMouse)
        Dim RenderRectangle As New Rectangle(
            RenderCenter.X - RenderRadius, RenderCenter.Y - RenderRadius,
            RenderRadius * 2, RenderRadius * 2
        )
        g.Graphics.DrawEllipse(Pens.Blue, RenderRectangle)
    End Sub

    Private Sub DrawVelocities()

        For i As Integer = 0 To BodyDatas.Length - 1

            Dim RenderCenter As PointF = ScenePointToRenderPoint(BodyDatas(i).Center)
            Dim RenderVelocity As Vector = 3600 * ZoomScale * BodyDatas(i).Velocity

            g.Graphics.DrawLine(
                New Pen(Color.Blue, 1),
                RenderCenter,
                RenderCenter + RenderVelocity
            )
        Next

    End Sub

    Private Sub DrawMouseLabel()
        Dim StringSize = g.Graphics.MeasureString(MouseLabelText, Control.DefaultFont)
        Dim RenderPoint = New Point(RenderMouse.X, RenderMouse.Y - StringSize.Height)

        g.Graphics.FillRectangle(New SolidBrush(Color.White), New Rectangle(RenderPoint, Size.Round(StringSize)))
        g.Graphics.DrawString(MouseLabelText, Control.DefaultFont, New SolidBrush(Color.Black), RenderPoint)
    End Sub

    Private Sub DrawTopLeftString()

        'TODO: include RAM and CPU usage

        g.Graphics.DrawString(
            "Frame Rate: " + GameLoop.FPS.ToString + "Hz" + vbCrLf +
            "Zoom: " + Math.Round(GetZoomScalePercentage(), 1, MidpointRounding.AwayFromZero).ToString + "%" + vbCrLf +
            "Simulated Time: " + GetTimeString(GameLoop.SimulatedTime, 1) + vbCrLf +
            "Timescale: " + GetTimeString(Timescale, 2).ToString + "/s",
            Control.DefaultFont, New SolidBrush(Color.Black), New Point()
        )
    End Sub

End Module
