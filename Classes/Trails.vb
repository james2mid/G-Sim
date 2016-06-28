Public NotInheritable Class Trails

    ''' <summary>
    ''' The array which holds the scene points for each body's trail
    ''' </summary>
    Private Shared ScenePoints As New List(Of List(Of PointF))

    ''' <summary>
    ''' Holds the value which the MaxBodyPoints variable holds to begin with.
    ''' </summary>
    Public Const MaxBodyPointsStartValue As Integer = 1000

    ''' <summary>
    ''' Holds the value which is the max number of points stored for each body.
    ''' </summary>
    Public Shared MaxBodyPoints As Integer = MaxBodyPointsStartValue

    ''' <summary>
    ''' Refreshes the values stored in the array
    ''' </summary>
	Shared Sub UpdateTrails()

		'Return if there are no bodies present as there is nothing to update
		If BodyArrayEmpty() Then
			Return
		End If

		'First add new points and remove excess
		Try
			For BodyDatasIndex = 0 To BodyDatas.Length - 1
				Dim ScenePointsIndex = BodyDatasIndex
				Dim SceneCenterPoint As PointF = BodyDatas(BodyDatasIndex).Center
				ScenePoints(ScenePointsIndex).Add(SceneCenterPoint)
				RemoveExcessPoints(ScenePointsIndex)
			Next
		Catch ex As Exception
		End Try

	End Sub

    ''' <summary>
    ''' Removes the oldest points in the specified index
    ''' </summary>
	Private Shared Sub RemoveExcessPoints(ScenePointsIndex As Integer)
		Dim NumberOfPointsToRemove As Short = ScenePoints(ScenePointsIndex).Count - MaxBodyPoints

		If NumberOfPointsToRemove > 0 Then
			For n = 0 To NumberOfPointsToRemove
				ScenePoints(ScenePointsIndex).RemoveAt(0)
			Next
		End If
	End Sub

    ''' <summary>
    ''' Declares a new body on the end of the point list
    ''' </summary>
	Shared Sub AddBodyTrail()
		ScenePoints.Add(New List(Of PointF))
	End Sub

    ''' <summary>
    ''' Removes trail at specified index
    ''' </summary>
	Shared Sub RemoveBodyTrail(index As UShort)
		ScenePoints.RemoveAt(index)
	End Sub

    ''' <summary>
    ''' Removes all the stored body trails in the array
    ''' </summary>
	Shared Sub RemoveAllTrails()
		ScenePoints.Clear()
	End Sub

    ''' <summary>
    ''' Renders the curves from the points stored in this class to the buffer passed as a parameter
    ''' </summary>
Shared Sub RenderTrails()

	Dim RenderPoints As New List(Of List(Of Point))

	'Run through each body in the ScenePointsIndexArray
	For ScenePointsIndex = 0 To ScenePoints.Count - 1

		'Create a new list for the body
		RenderPoints.Add(New List(Of Point))

		'If the body is really small then don't bother calculating render points
		If SceneDistanceToRenderDistance(BodyDatas(ScenePointsIndex).Size) > 2 Then
			'Go through each scene point for the current body and calculate render points for that body
			For PointIndex = 0 To ScenePoints(ScenePointsIndex).Count - 1
				'Convert scene point to render point
				Dim RenderPoint As Point = Point.Round(ScenePointToRenderPoint(ScenePoints(ScenePointsIndex)(PointIndex)))
				'Add to RenderPoints list
				RenderPoints(ScenePointsIndex).Add(RenderPoint)
			Next
		End If

	Next

	'Run through each body which has points stored in the RenderPoints array
	For RenderPointsIndex As Integer = 0 To RenderPoints.Count - 1

		If RenderPoints(RenderPointsIndex).Count >= 2 Then

			Dim BodyIndex As Integer = RenderPointsIndex

			Dim BaseColour As Color

			If BodyDatas(BodyIndex).IsUsingBitmap Then
				BaseColour = BodyDatas(BodyIndex).GetDominantImageColour()
			Else
				BaseColour = BodyDatas(BodyIndex).Colour
			End If

			Dim BaseLineAlphaPercentage As Single =
				SceneDistanceToRenderDistance(BodyDatas(BodyIndex).Size) / 10

			Dim BaseLineAlpha As Byte
			
			If BaseLineAlphaPercentage > 1 Then
				BaseLineAlpha = 255
			Else
				BaseLineAlpha = 255 * BaseLineAlphaPercentage
			End If

			For i As Integer = 1 To RenderPoints(RenderPointsIndex).Count - 1
				Dim LineSegmentAlpha As Byte = BaseLineAlpha * i / RenderPoints(RenderPointsIndex).Count
				Dim LineSegmentColour As Color = Color.FromArgb(LineSegmentAlpha, BaseColour)
				g.Graphics.DrawLine(
					New Pen(LineSegmentColour),
					RenderPoints(RenderPointsIndex)(i - 1),
					RenderPoints(RenderPointsIndex)(i)
				)
			Next

		End If
	Next

End Sub

    ''' <summary>
    ''' Clears the trail of the body at the specified index
    ''' </summary>
    ''' <param name="i">Index to clear</param>
	Shared Sub ClearTrail(i As Integer)
		ScenePoints(i).Clear()
	End Sub

    ''' <summary>
    ''' Clears all trails
    ''' </summary>
	Shared Sub ClearAllTrails()
		ScenePoints.Clear()

		If Not IsNothing(BodyDatas) Then
			If BodyDatas.Length > 0 Then
				For n = 1 To BodyDatas.Length
					ScenePoints.Add(New List(Of PointF))
				Next
			End If
		End If
	End Sub

End Class
