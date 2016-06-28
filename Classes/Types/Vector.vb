Option Explicit On
Imports G_Sim

Public Class Vector

    Property x As Double
    Property y As Double

	Sub New(Optional xValue As Double = 0, Optional yValue As Double = 0)
		x = xValue
		y = yValue
	End Sub

    ''' <summary>
    ''' Calculates a vector from A to B (return a positive j when moving upwards)
    ''' </summary>
	Public Shared Function VectorBetween(p1 As PointF, p2 As PointF) As Vector
		Dim changeX As Double = p2.X - p1.X
		Dim changeY As Double = p2.Y - p1.Y
		Return New Vector(
			changeX,
			changeY
		)
	End Function

    ''' <summary>
    ''' Returns a vector with each of the inputted components divided by the magnitude of inputted vector
    ''' </summary>
	Function UnitVector() As Vector
		Dim magnitude As Double = Me.Magnitude()
		Return New Vector(
			IIf(x = 0 And magnitude = 0, 0, x / magnitude),
			IIf(y = 0 And magnitude = 0, 0, y / magnitude)
		)
	End Function

    ''' <summary>
    ''' Returns the squared magnitude or length of the vector (uses Pythagoras)
    ''' </summary>
	Function MagnitudeSquared() As Double
		Return x ^ 2 + y ^ 2
	End Function

    ''' <summary>
    ''' Returns the magnitude or length of the vector (uses Pythagoras)
    ''' </summary>
	Function Magnitude() As Double
		Return Math.Sqrt(x ^ 2 + y ^ 2)
	End Function

	Function ChangeMagnitude(magnitude As Double) As Vector
		Dim UnitVector As Vector = Me.UnitVector()
		Return magnitude * UnitVector
	End Function

	Function Tangent() As Vector
		Return New Vector(-y, x)
	End Function

    Function SecondaryTangent() As Vector
        Return New Vector(y, -x)
    End Function

	Shared Operator +(left As Vector, right As Vector) As Vector
		Return New Vector(
			left.x + right.x,
			left.y + right.y
		)
	End Operator

	Shared Operator +(left As PointF, right As Vector) As PointF
		Return New PointF(
			left.X + right.x,
			left.Y + right.y
		)
	End Operator

	Shared Operator +(left As Vector, right As PointF) As PointF
		Return New PointF(
			left.x + right.X,
			left.y + right.Y
		)
	End Operator

	Shared Operator *(scalar As Double, op2 As Vector) As Vector
		Return New Vector(
			scalar * op2.x,
			scalar * op2.y
		)
	End Operator

	Shared Operator /(left As Vector, right As Double) As Vector
		Return New Vector(
			left.x / right,
			left.y / right
		)
	End Operator

	Shared Operator /(left As Vector, right As Vector) As Vector
		Return New Vector(
			left.x / right.x,
			left.y / right.y
		)
	End Operator

    Shared Operator -(right As Vector) As Vector
        Return New Vector(-right.x, -right.y)
    End Operator

	Shared Operator -(left As Vector, right As Vector) As Vector
		Return New Vector(
			left.x - right.x,
			left.y - right.y
		)
	End Operator
End Class
