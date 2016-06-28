Option Strict On

Public Class Body

#Region "Constructor and Clone"

    ''' <summary>
    ''' Generates an empty body with default / empty values
    ''' </summary>
	Sub New()
		Velocity = New Vector()
		Acceleration = New Vector()
		Center = New PointF(0, 0)
		Mass = 0
		IsUsingBitmap = False
		Image = Nothing
		Colour = Color.Empty
	End Sub

	Function Clone() As Body
		Return DirectCast(MemberwiseClone(), Body)
	End Function

#End Region

#Region "Base Properties"

    'Variables here are ones used by properties which require additional code in their get or set parts
	Private _Velocity As Vector
	Private _Center As PointF
	Private _Mass As Single
	Private _Image As Image
	Private _DominantImageColour As Color

    ''' <summary>
    ''' Gets and sets the velocity of the body (max is 10 ^ 8)
    ''' </summary>
Property Velocity(Optional ThrowExceptionOnOutOfBounds As Boolean = False) As Vector
	Get
		Return _Velocity
	End Get
	Set(value As Vector)

		If value.Magnitude > 299792458 Then
			_Velocity = value.ChangeMagnitude(299792458)

			If ThrowExceptionOnOutOfBounds Then
				Throw New OverflowException("Value is too big")
			End If

		Else
			_Velocity = value
		End If

	End Set
End Property

    ''' <summary>
    ''' Gets and sets the acceleration of the body
    ''' </summary>
    Property Acceleration() As Vector

    ''' <summary>
    ''' Gets and sets the center of the body (the primary position property) (max is 10 ^ 35, exception is thrown upon setting a value greater than this which can be tested for)
    ''' </summary>
	Property Center(Optional ThrowExceptionOnOutOfBounds As Boolean = False) As PointF
		Get
			Return _Center
		End Get
		Set(value As PointF)

			Dim x As Single = value.X
			Dim y As Single = value.Y

			Dim ValueChanged = Not ForceMaxBounds(x, 10 ^ 35, True) Or Not ForceMaxBounds(y, 10 ^ 35, True)

			_Center = New PointF(x, y)

			If ValueChanged And ThrowExceptionOnOutOfBounds Then
				Throw New OverflowException()
			End If

		End Set
	End Property

    ''' <summary>
    ''' Gets or sets the height / width of the body on the scene
    ''' </summary>
    Property Size() As Single

    ''' <summary>
    ''' Gets and sets the mass of the body (max is 10 ^ 35) [sets the size of the body upon setting]
    ''' </summary>
	Property Mass(Optional ThrowExceptionOnOutOfBounds As Boolean = False) As Single
		Get
			Return _Mass
		End Get
		Set(value As Single)

			Dim ValueChanged As Boolean = Not ForceMaxBounds(value, 10 ^ 35, False)

			'Update the mass variable to hold the new value
			_Mass = value

			'Set the size from the new mass
			Size = GetSizeFromMass(Mass)

			If ValueChanged And ThrowExceptionOnOutOfBounds Then
				Throw New OverflowException()
			End If

		End Set
	End Property

    ''' <summary>
    ''' Gets and sets the colour of the body
    ''' </summary>
    Property Colour() As Color

    ''' <summary>
    ''' States whether the body is currently using a bitmap (its image) to be rendered
    ''' </summary>
    Property IsUsingBitmap() As Boolean

    ''' <summary>
    ''' Gets and sets the bitmap (image) of the body and generates DominantImageColour from the specified image
    ''' </summary>
	Property Image() As Image
		Get
			Return _Image
		End Get
		Set(value As Image)
			_Image = value
			If Image IsNot Nothing Then
				UpdateDominantImageColour()
			End If
		End Set
	End Property


#End Region

#Region "Calculated Properties"

    ''' <summary>
    ''' Gets the PointF of the top left of the body
    ''' </summary>
	ReadOnly Property TopLeft() As PointF
		Get
			Return New PointF(Center.X - Radius, Center.Y - Radius)
		End Get
	End Property

    ''' <summary>
    ''' Gets the radius (half of the size)
    ''' </summary>
	ReadOnly Property Radius() As Single
		Get
			Return Size / 2
		End Get
	End Property

    ''' <summary>
    ''' Used within the body class to set DominantImageColour
    ''' </summary>
	Private Sub UpdateDominantImageColour()

		If Image Is Nothing Then
			Throw New Exception()
			Return
		End If

		Dim bmp As New Bitmap(Image)

		_DominantImageColour = GetDominantImageColourFromBitmap(bmp)

	End Sub

#End Region

    ''' <summary>
    ''' Gets the height/width of a body with the specified mass given the density
    ''' </summary>
	Shared Function GetSizeFromMass(mass As Single) As Single
		Return CSng(2 * Math.Pow(3 * mass / (4 * Math.PI * 5000), 1 / 3))
	End Function

    ''' <summary>
    ''' Gets the mass of a body given its size and density
    ''' </summary>
	Shared Function GetMassFromSize(radius As Single) As Single
		Return CSng(4 / 3 * Math.PI * radius ^ 3 * 5000)
	End Function

    ''' <summary>
    ''' Gets the colour which is mathematically average in the set image
    ''' </summary>
    Public Function GetDominantImageColour() As Color
        Return _DominantImageColour
    End Function

    ''' <summary>
    ''' Used within the body class to calculate the dominant colour in the specified bitmap
    ''' </summary>
	Private Shared Function GetDominantImageColourFromBitmap(bmp As Bitmap) As Color

		Dim totalR As ULong = 0
		Dim totalG As ULong = 0
		Dim totalB As ULong = 0

		Dim totalPixels As Long = 0

		For y = 0 To bmp.Height - 1 Step 4
			For x = 0 To bmp.Width - 1 Step 4
				Dim pixel As Color = bmp.GetPixel(x, y)
				If pixel.A > 200 Then
					totalR += pixel.R
					totalG += pixel.G
					totalB += pixel.B

					totalPixels += 1
				End If

			Next
		Next

		If totalPixels = 0 Then
			Throw New Exception("Failure reading image")
			Return Color.Gray
		End If

		Dim averageR As Byte = CByte(totalR / totalPixels)
		Dim averageG As Byte = CByte(totalG / totalPixels)
		Dim averageB As Byte = CByte(totalB / totalPixels)

		Return Color.FromArgb(averageR, averageG, averageB)
	End Function

	''' <summary>
	''' Calculates whether this body is currently intersecting the specified body
	''' </summary>
	Public Function Intersecting(bodyToTest As Body) As Boolean
		Dim SumRadiiSquared As Double = (Radius + bodyToTest.Radius) ^ 2
		Dim DistanceCentersSquared As Double = (Center.X - bodyToTest.Center.X) ^ 2 + (Center.Y - bodyToTest.Center.Y) ^ 2

		Return DistanceCentersSquared <= SumRadiiSquared

	End Function

    ''' <summary>
    ''' Calculates whether the body is visible in the viewport
    ''' </summary>
	Public Function VisibleInView() As Boolean
		Dim topLeft As New PointF(Center.X - Radius, Center.Y - Radius)
		Dim topRight As New PointF(Center.X + Radius, Center.Y - Radius)
		Dim bottomRight As New PointF(Center.X + Radius, Center.Y + Radius)
		Dim bottomLeft As New PointF(Center.X - Radius, Center.Y + Radius)

		Dim ViewportSceneRect As RectangleF = GetViewportSceneRectangle()
		For Each p As PointF In {topLeft, topRight, bottomRight, bottomLeft}
			If InsideRectangle(p, ViewportSceneRect) Then
				Return True
			End If
		Next

		Dim ScenePointCenterViewport As New PointF(
			(ViewportSceneRect.Left + ViewportSceneRect.Right) / 2,
			(ViewportSceneRect.Top + ViewportSceneRect.Bottom) / 2
		)

		Dim BodySceneRectangle As New RectangleF(topLeft, New SizeF(Size, Size))

		If InsideRectangle(ScenePointCenterViewport, GetSceneRectangle()) Then
			Return True
		End If

		Return False

	End Function

	Public Function GetSceneRectangle() As RectangleF
		Return New RectangleF(
			TopLeft,
			New SizeF(Size, Size)
		)
	End Function

    ''' <summary>
    ''' Returns the rectangle which will be used to render the body.
    ''' </summary>
	Public Function RenderRectangle() As Rectangle

		Dim RenderDistance As Integer = SceneDistanceToRenderDistance(Size)

		Return New Rectangle(
			Point.Round(ScenePointToRenderPoint(TopLeft)),
			New Size(RenderDistance, RenderDistance)
		)
	End Function

End Class
