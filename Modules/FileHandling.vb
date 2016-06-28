
Imports System.IO

Module FileHandling

    Private InitialHeaderRowString As String = "viewport,zoom,timescale:"
    Private BodyHeaderRowString As String = "velocity,position,mass,colour,size,isusingbitmap,bitmap:"

    Private AppDataFolderPath As String

    ''' <summary>
    ''' Returns the CSV line corresponding to the inputted body.
    ''' </summary>
    Private Function GetCSVLine(b As Body) As String

        Dim returnString As String = ""

        Dim numElements = 7
        For i As Integer = 1 To numElements
            Select Case i
                Case 1 'velocity
                    returnString &= (
                        b.Velocity.x.ToString() & "|" &
                        b.Velocity.y.ToString()
                    )
                Case 2 'position
                    returnString &= (
                        b.Center.X.ToString() & "|" &
                        b.Center.Y.ToString()
                    )
                Case 3 'mass
                    returnString &= (b.Mass.ToString())
                Case 4 'colour
                    If Not IsNothing(b.Colour) Then
                        returnString &= (b.Colour.ToArgb().ToString())
                    End If
                Case 5 'size
                    returnString &= (b.Size.ToString())
                Case 6
                    returnString &= b.IsUsingBitmap.ToString()
                Case 7
                    If Not IsNothing(b.Image) Then
                        returnString &= ImageToBase64(b.Image)
                    End If
            End Select

            If Not i = numElements Then
                returnString &= (",")
            End If
        Next

        Return returnString

    End Function

    ''' <summary>
    ''' Converts a CSV line and gives out a body.
    ''' </summary>
    Private Function CSVToBody(line As String) As Body

        Dim rtnBody As New Body()

        Try
            Dim elements As String() = line.Split(",")

            Array.ForEach(elements, Sub(s) s.Trim())

            With rtnBody
                .Velocity = New Vector(
                    elements(0).Split("|")(0),
                    elements(0).Split("|")(1)
                )
                .Center = New PointF(
                            elements(1).Split("|")(0),
                            elements(1).Split("|")(1)
                        )
                .Mass = elements(2)
                .Colour = Color.FromArgb(elements(3))
                .Size = elements(4)
                .IsUsingBitmap = Boolean.Parse(elements(5))

                Try
                    .Image = Base64ToImage(elements(6))
                Catch ex As Exception
                    .Image = Nothing
                    .IsUsingBitmap = False
                End Try

            End With

        Catch ex As Exception
            Return Nothing
        End Try

        Return rtnBody

    End Function

    ''' <summary>
    ''' Taks in an image and returns the corresponding base64 code.
    ''' </summary>
    Private Function ImageToBase64(img As Image) As String
        Dim ms As New IO.MemoryStream()
        img.Save(ms, Imaging.ImageFormat.Png)
        Return Convert.ToBase64String(ms.ToArray())
    End Function

    ''' <summary>
    ''' Takes in base64 code and converts it to an image.
    ''' </summary>
    Private Function Base64ToImage(base64 As String) As Image
        Dim imageBytes As Byte() = Convert.FromBase64String(base64)
        Dim MS As New MemoryStream(imageBytes, 0, imageBytes.Length)

        MS.Write(imageBytes, 0, imageBytes.Length)
        Return Image.FromStream(MS)
    End Function

    ''' <summary>
    ''' Guides the user through saving the scene file.
    ''' </summary>
    Public Sub SaveCSVFile()
        Dim sfd As New SaveFileDialog()

        With sfd
            .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.Desktop
            .DefaultExt = ".gsim"
            .Filter = "G-Sim Scene Information (*.gsim)|*.gsim"
            .FileName = DateTime.Now.ToString("dd-MM-yyyy") & "_" & DateTime.Now.ToString("HH-mm-ss") & ".gsim"
            .Title = "Save G-Sim Scene Information"
        End With

        If sfd.ShowDialog = DialogResult.OK Then

            Dim stringToWrite As String = ""

            Dim add = Sub(x As String) stringToWrite &= x
            Dim newLine = Sub() add(Environment.NewLine)

            add(InitialHeaderRowString)
            newLine()

            For i As Integer = 1 To 3
                Select Case i
                    Case 1 'viewport
                        add(ViewPosition.X.ToString() & "|" &
                            ViewPosition.Y.ToString())
                    Case 2 'zoom
                        add(ZoomScale.ToString())
                    Case 3 'timescale
                        add(Timescale.ToString())
                End Select

                If Not i = 3 Then
                    add(",")
                End If
            Next

            newLine()

            add(BodyHeaderRowString)
            newLine()

            For Each b As Body In BodyDatas
                add(GetCSVLine(b))
                newLine()
            Next

            Dim sw As StreamWriter

            Try
                sw = New IO.StreamWriter(sfd.FileName, False)
            Catch ex As IOException
                MsgBox("The file is currently being used by another process. Try again after exiting that process.")
                Return
            End Try

            sw.Write(stringToWrite)
            sw.Close()

            If MsgBox("Scene information saved successfully") = MsgBoxResult.Ok Then
                Process.Start("""" & IO.Path.GetDirectoryName(sfd.FileName) & """")
            End If

        End If
    End Sub

    ''' <summary>
    ''' Guides the user through saving the scene file.
    ''' </summary>
    ''' <return>If a scene file has been loaded then returns True, else returns False.</return>
    Public Function LoadCSVFile() As Boolean
        Dim ofd As New OpenFileDialog()

        With ofd
            .InitialDirectory = AppDataFolderPath & "\Scenes"
            .DefaultExt = ".gsim"
            .Filter = "G-Sim Scene Information (*.gsim)|*.gsim"
            .FileName = "scene.gsim"
            .Title = "Load G-Sim Scene Information"
        End With

        If ofd.ShowDialog() = DialogResult.OK Then

            Dim sr As New IO.StreamReader(ofd.FileName)
            Dim fileContents As String = sr.ReadToEnd()
            sr.Close()

            Dim lines As String() = fileContents.Split(vbCrLf)

            Dim LinesIndex As Integer = 0
            Do Until LinesIndex = lines.Count
                lines(LinesIndex) = lines(LinesIndex).Trim()
                If lines(LinesIndex) = Environment.NewLine Or lines(LinesIndex) = "" Or lines(LinesIndex) = Nothing Then
                    Dim list As List(Of String) = lines.ToList
                    list.RemoveAt(LinesIndex)
                    lines = list.ToArray()
                    LinesIndex -= 1
                End If
                LinesIndex += 1
            Loop

            Dim InitialHeaderRowIndex As Integer = Array.IndexOf(lines, InitialHeaderRowString)
            If InitialHeaderRowIndex = -1 Then
                MsgBox("Initial header row corrupt in " & ofd.SafeFileName, MsgBoxStyle.Critical)
                Return False
            End If

            Dim BodyHeaderRowIndex As Integer = Array.IndexOf(lines, BodyHeaderRowString)
            If BodyHeaderRowIndex = -1 Then
                MsgBox("Body header row corrupt in " & ofd.SafeFileName, MsgBoxStyle.Critical)
                Return False
            End If

            Dim SettingsValues As String() = lines(InitialHeaderRowIndex + 1).Split(",")

            Dim ViewportSplitValues As String() = SettingsValues(0).Split("|")

            ViewPosition = New PointF(
                ViewportSplitValues(0),
                ViewportSplitValues(1)
            )

            ZoomScale = SettingsValues(1)
            Timescale = SettingsValues(2)

            'Clear the scene
            BodyDatas = Nothing
            Trails.ClearAllTrails()
            GameLoop.ResetSimulatedTime()

            'Add each body line
            For i As Integer = BodyHeaderRowIndex + 1 To lines.Length - 1

                Dim addingBody As Body = CSVToBody(lines(i))

                If IsNothing(addingBody) Then
                    MsgBox("Line " & i & " containing a body is corrupt", MsgBoxStyle.Exclamation)
                Else
                    AddBody(addingBody)
                End If

            Next

            MsgBox("Load completed")

            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Copies all of the images and example scene files into the application data.
    ''' </summary>
    Public Sub SetupResources()

        AppDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\G-Sim"

		Directory.CreateDirectory(AppDataFolderPath)
		Directory.CreateDirectory(AppDataFolderPath & "\Images")
		Directory.CreateDirectory(AppDataFolderPath & "\Scenes")

		Dim runTimeResourceSet As Resources.ResourceSet =
			My.Resources.ResourceManager.GetResourceSet(Globalization.CultureInfo.CurrentCulture, True, True)

		Dim dictEntry As DictionaryEntry

		For Each dictEntry In runTimeResourceSet
			If dictEntry.Value.GetType() Is GetType(Bitmap) Then
				Dim FilePath As String = AppDataFolderPath & "\Images\" & dictEntry.Key.ToString() & ".png"

                If Not File.Exists(FilePath) Then
                    Dim bmp As Bitmap = dictEntry.Value
                    bmp.Save(FilePath, bmp.RawFormat)
                End If

            ElseIf dictEntry.Value.GetType() Is GetType(Byte()) Then
				Dim FilePath As String = AppDataFolderPath & "\Scenes\" & dictEntry.Key.ToString() & ".gsim"

                If Not File.Exists(FilePath) Then
                    Dim array As Byte() = dictEntry.Value
                    Dim stream As New FileStream(FilePath, FileMode.Create)
                    stream.Write(array, 0, array.Length)
                    stream.Close()
                End If

            End If
		Next

    End Sub

    ''' <summary>
    ''' Guides the user through setting the image of the selected body.
    ''' </summary>
    ''' <param name="SelfCalled">Used when the image is removed to maintain the stored state of the gameloop when recalled.</param>
    Public Sub ProcessSelectImage(Optional SelfCalled As Boolean = False)
        'Lets the sub call itself while GameLoop retains the correct origional state
        If Not SelfCalled Then
            GameLoop.StoreAndPause()
        End If

        'If the body /doesn't have/ an image then let the user give it an image
        If Not BodyDatas(SelectedBodyIndex).IsUsingBitmap Then
            'Display a message box announcing the details about adding an image
            If MsgBox(
"A box will appear after clicking 'OK' for you to select the image file.
Supported file types are BMP, GIF, JPEG/JPG, PNG, TIFF.
You can find image files to download by searching online.
Try NOT to use JPEG/JPG files As they do not support transparency and will result in a white square being shown around the body.
Also, try and find images which are circular and whos edges are close to the edge of the image.

A folder has been created which holds the images for all planets in the Solar System apart from Saturn. You can use these or you can find your own.

Press 'OK' to continue.",
            MsgBoxStyle.OkCancel, "Select an Image") = MsgBoxResult.Cancel Then
                'If they cancel adding of the image then leave the sub and continue the loop
                GameLoop.ContinueStored()
                Return
            End If
            'Otherwise carry on to add an image

            'Declare the OpenFileDialog and prepare it to open the correct filetypes (images)
            Dim ofd As New OpenFileDialog()
            With ofd
                .CheckFileExists = True
                .Filter = "Supported Image Files (*.bmp, *.gif, *.jpeg, *.jpg, *.png, *.tiff)|*.bmp;*.gif;*.jpeg;*.jpg;*.png;*.tiff"
                .InitialDirectory = AppDataFolderPath & "\Images"
                .Title = "Select an Image"
            End With

            'If they cancel opening an image then leave the sub and continue the loop
            If ofd.ShowDialog() = DialogResult.Cancel Then
                GameLoop.ContinueStored()
                Return
            End If

            'Declare this variable outside of the try block so that the body image can be set out of the try block
            'Because I like to try and have the least amount of code take place in the TRY block as possible
            Dim NewImage As Bitmap

            'Put the image reading/manipulating code into a TRY block so that if the application attempts to read a corrupt image
            'Then it can be handled within the code by a message box saying there was an issue
            Try
                'Read the image from a file into a variable
                Dim OrigImage As Image = Image.FromFile(ofd.FileName)

                'Resize the image to reduce computation when rendering
                NewImage = New Bitmap(OrigImage, 200, 200)

            Catch ex As Exception
                'Alert the user of the error and return from the sub
                MsgBox("Failure reading the specified image. Either try loading it again, redownload the image or find a new image.", MsgBoxStyle.Exclamation)
                GameLoop.ContinueStored()
                Return
            End Try

            'Set the new image and set that the body will be rendered using the bitmap
            BodyDatas(SelectedBodyIndex).Image = NewImage
            BodyDatas(SelectedBodyIndex).IsUsingBitmap = True

        Else
            'Let the user know the possible options
            Dim result = MsgBox("Do you want to set a new image (Yes), remove the current image (No) or keep the image as it is (Cancel)?", MsgBoxStyle.YesNoCancel, "Change Image")

            'Opt to set a new image
            If result = MsgBoxResult.Yes Then
                'Run the sub again thinking that the body has no image assigned to it
                BodyDatas(SelectedBodyIndex).IsUsingBitmap = False
                ProcessSelectImage(True)
                Return

                'Opt to remove the image
            ElseIf result = MsgBoxResult.No Then
                'Remove the image
                BodyDatas(SelectedBodyIndex).IsUsingBitmap = False
                BodyDatas(SelectedBodyIndex).Image = Nothing
            End If
            'If they say cancel then continue running to the end of the sub

        End If

        'Refresh changes and continue loop
        GameLoop.PaintOnce()
        GameLoop.ContinueStored()
    End Sub

End Module
