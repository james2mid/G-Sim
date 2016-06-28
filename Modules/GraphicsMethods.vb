Module GraphicsMethods

    ''' <summary>
    ''' The main buffer which everything is painted to before being rendered to the graphics target.
    ''' </summary>
    Public g As BufferedGraphics

    ''' <summary>
    ''' Clears the buffer and paints it white.
    ''' </summary>
    Public Sub ClearBuffer()
        g.Graphics.Clear(Color.White)
    End Sub

    ''' <summary>
    ''' Renders the buffer to the default graphics object (the render panel).
    ''' </summary>
    Public Sub RenderBuffer()
        g.Render()
    End Sub

    ''' <summary>
    ''' Returns whether the BufferedGraphics has an object assigned to it.
    ''' </summary>
    Public Function BufferEmpty() As Boolean
        Return IsNothing(g)
    End Function

End Module
