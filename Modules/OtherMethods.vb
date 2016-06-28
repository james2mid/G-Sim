Module OtherMethods

    ''' <summary>
    ''' Returns the specified number of seconds as a string in terms of secs/mins/hrs/days/years rounded to the given number of decimal places
    ''' </summary>
    Function GetTimeString(seconds As Double, decimals As Integer) As String

        'Store whether the value is negative
        Dim NegativeTime As Boolean = seconds < 0

        'Use the absolute value in calculations below because it simplifies the code
        Dim time As Double = Math.Abs(seconds)

        'Return the value rounded to the specified dp
        Dim returnValue As Func(Of String) = Function()
                                                 Return (Math.Round(time, decimals) * IIf(NegativeTime, -1, 1)).ToString()
                                             End Function

        'In seconds
        If time < 60 Then
            Return returnValue() + " secs"
        Else
            time /= 60
        End If

        'In minutes
        If time < 60 Then
            Return returnValue() + " mins"
        Else
            time /= 60
        End If

        'In hours
        If time < 24 Then
            Return returnValue() + " hrs"
        Else
            time /= 24
        End If

        'In days
        If time < 365 Then
            Return returnValue() + " days"
        Else
            time /= 365
        End If

        'In years
        Return returnValue() + " years"

    End Function

    ''' <summary>
    ''' Ensures that a variable is less than the specified max (Double)
    ''' </summary>
    ''' <param name="value">The value to check and change if neccessary</param>
    ''' <param name="Maximum">The maximum value which value can have</param>
    ''' <param name="CheckNegative">States whether the value will ever be negative and so to check if the value is greater than the negative of the specified max</param>
    ''' <returns>If the value is changed returns False, if it is left then returns True</returns>
    Function ForceMaxBounds(ByRef value As Double, ByVal Maximum As Double, ByVal CheckNegative As Boolean) As Boolean
        If value > Maximum Then
            value = Maximum
            Return False
        ElseIf CheckNegative And value < -Maximum Then
            value = -Maximum
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Ensures that a variable is less than the specified max (Single)
    ''' </summary>
    ''' <param name="value">The value to check and change if neccessary</param>
    ''' <param name="Maximum">The maximum value which value can have</param>
    ''' <param name="CheckNegative">States whether the value will ever be negative and so to check if the value is greater than the negative of the specified max</param>
    ''' <returns>If the value is changed returns False, if it is left then returns True</returns>
    Function ForceMaxBounds(ByRef value As Single, ByVal Maximum As Single, ByVal CheckNegative As Boolean) As Boolean
        If value > Maximum Then
            value = Maximum
            Return False
        ElseIf CheckNegative And value < -Maximum Then
            value = -Maximum
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Ensures that a value is greater than the specified minimum (positive only)
    ''' </summary>
    ''' <param name="value">The value to check and change if neccessary</param>
    ''' <param name="Minimum">The lowest value which value can have</param>
    ''' <returns>If the value is changed returns False, if it is left then returns True</returns>
    Function ForceMinBound(ByRef value As Double, ByVal Minimum As Double) As Boolean
        If value < Minimum Then
            value = Minimum
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Generates an entirely random colour
    ''' </summary>
    Function GetRandomColour() As Color
        Dim R As Integer = Math.Floor(RandomNumber(0, 255))
        Dim G As Integer = Math.Floor(RandomNumber(0, 255))
        Dim B As Integer = Math.Floor(RandomNumber(0, 255))

        Return Color.FromArgb(R, G, B)

    End Function

    ''' <summary>
    ''' Generates a random number 
    ''' </summary>
    ''' <param name="lowerBound"></param>
    ''' <param name="upperBound"></param>
    ''' <returns></returns>
    Public Function RandomNumber(lowerBound As Single, upperBound As Single) As Single
        Randomize()
        Return (upperBound - lowerBound + 1) * Rnd() + lowerBound
    End Function

End Module
