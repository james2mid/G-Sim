Option Explicit On

Public Class MainForm

    ''' <summary>
    ''' The timer which updates the current body data
    ''' </summary>
    Private WithEvents SelectedBodyRefreshTimer As New Timer()

    ''' <summary>
    ''' The timer which checks whether the cursor is still over the view (used for the orbital ring and mouse label visibility)
    ''' </summary>
    Private WithEvents CheckCursorLeaveViewTimer As New Timer()

    ''' <summary>
    ''' The timer upon each tick, ZoomScale is increased and the view is zoomed in
    ''' </summary>
    Private WithEvents ZoomInTimer As New Timer()

    ''' <summary>
    ''' The timer upon each tick, ZoomScale is decreased and the view is zoomed out
    ''' </summary>
    Private WithEvents ZoomOutTimer As New Timer()

    ''' <summary>
    ''' The timer upon each tick, checks whether the user is sliding the time factor tracker and so to update TimeFactor accordingly
    ''' </summary>
    Private WithEvents TimescaleTimer As New Timer()

    ''' <summary>
    ''' The timer upon each tick, if the simulation is running, stores a change using Changes
    ''' </summary>
    Private WithEvents AutoStoreSimulationChangesTimer As New Timer()

#Region "Loading and Form-Wide Updates"

    ''' <summary>
    ''' Performs the correct tasks in order to begin the application
    ''' </summary>
	Private Sub LoadApplication(sender As Object, e As EventArgs) Handles MyBase.Load

		'Add the unhandled exception handler before anything else is done
		AddHandler My.Application.UnhandledException, AddressOf MyApplication_UnhandledException

		'Set the name of the thread (used by some functions because of multithreading)
		Threading.Thread.CurrentThread.Name = "MainForm Thread"

		'Set the global RenderBoxSize
		RenderBoxSize = pnlRender.Size

		'Copies the resources into the AppData folder
		SetupResources()

		'Sets up the graphics buffer and allocate it to pnlRender
		g = BufferedGraphicsManager.Current.Allocate(pnlRender.CreateGraphics(), pnlRender.DisplayRectangle)
        g.Graphics.InterpolationMode = Drawing2D.InterpolationMode.Low
        g.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        ClearBuffer()

        'Setup the timers

        SelectedBodyRefreshTimer.Interval = 500
		SelectedBodyRefreshTimer.Start()

		CheckCursorLeaveViewTimer.Interval = 50
		CheckCursorLeaveViewTimer.Start()

		TimescaleTimer.Interval = 200

		ZoomInTimer.Interval = 50
		ZoomOutTimer.Interval = 50

		AutoStoreSimulationChangesTimer.Interval = Changes.AutoStoreWhileRunningIntervalMilliseconds

		'Disable the necessary controls to start
		UpdateUI()
	End Sub

    ''' <summary>
    ''' Paints a blank view (otherwise the view starts up grey) and then checks for updates
    ''' </summary>
	Private Sub MainForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
		'Show a blank view (otherwise starts grey)
		GameLoop.PaintOnce()

		'Check for updates
		CheckVersion()

	End Sub

    ''' <summary>
    ''' Stops the graphics loop as the form is closing
    ''' </summary>
    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        GameLoop.Pause()
    End Sub

    ''' <summary>
    ''' Disables/enables controls when body array is (not) empty then repaints
    ''' </summary>
    Public Sub UpdateUI()

        If BodyArrayEmpty() Then 'There are no bodies

            'Disable current body controls
            grpSelectedBody.Enabled = False

            'Disable the Play and SaveScene buttons
            btnPlayPause.Enabled = False
            btnSaveScene.Enabled = False

            'Disable Clear Scene button
            btnClearSimulation.Enabled = False

        Else 'There are bodies

            'Enable current body controls
            grpSelectedBody.Enabled = True

            'Enable the Play and SaveScene buttons
            btnPlayPause.Enabled = True
            btnSaveScene.Enabled = True

            'Enable Clear Scene button
            btnClearSimulation.Enabled = True

        End If

        'Update the render panel
        GameLoop.PaintOnce()

    End Sub

#End Region

#Region "Selected Body"

    ''' <summary>
    ''' Allows the user to add/change/remove the image from the selected body
    ''' </summary>
    Private Sub SetImageSelectedBody(sender As Object, e As EventArgs) Handles btnSelectedBodySetImage.Click

        ProcessSelectImage()

    End Sub

    ''' <summary>
    ''' Updates the selected body information on the UI
    ''' </summary>
    Private Sub RefreshCurrentBodyInfo() Handles SelectedBodyRefreshTimer.Tick

        If BodyArrayEmpty() Then

            lbSelectedBodyInfo.Items.Clear()
            btnSelectedBodySetImage.Text = "Set Image"

            Return
        End If

        Dim UsingBody As Body
        Try
            UsingBody = BodyDatas(SelectedBodyIndex).Clone()
        Catch ex As Exception
            'Catches an exception which is thrown when two threads are accessing the same element
            Return
        End Try

        'Clear the previous information
        lbSelectedBodyInfo.Items.Clear()

        'Declare a lambda to shorten code and improve readibility
        Dim AddLine = Sub(x As String)
                          lbSelectedBodyInfo.Items.Add(x)
                      End Sub

        'Update the information
        With UsingBody
            AddLine("Mass:  " + .Mass.ToString + "Kg")
            AddLine("Radius: " + .Radius.ToString + "m")
            AddLine("Velocity magnitude: " + .Velocity.Magnitude.ToString + "m/s")
            AddLine("Acceleration magnitude: " + .Acceleration.Magnitude.ToString + "m/s²")
        End With

        'Update the selected body image button text
        If UsingBody.IsUsingBitmap Then
            btnSelectedBodySetImage.Text = "Change Image"
        Else
            btnSelectedBodySetImage.Text = "Set Image"
        End If

    End Sub

#End Region

#Region "View Options"

    ''' <summary>
    ''' Updates the ShowVelocity global and repaints
    ''' </summary>
    Private Sub ShowVelocity_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowVelocity.CheckedChanged
        ShowVelocity = chkShowVelocity.Checked
        GameLoop.PaintOnce()
    End Sub

    ''' <summary>
    ''' Updates the ShowTrails global and repaints
    ''' </summary>
    Private Sub ShowTrails_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowTrails.CheckedChanged
        ShowTrails = chkShowTrails.Checked
        GameLoop.PaintOnce()
    End Sub

    ''' <summary>
    ''' Clears all trails in the scene
    ''' </summary>
    Private Sub ClearAllTrails(sender As Object, e As EventArgs) Handles btnClearTrails.Click
        Trails.ClearAllTrails()
        GameLoop.PaintOnce()
    End Sub

    ''' <summary>
    ''' Starts ZoomInTimer
    ''' </summary>
    Private Sub btnViewZoomIn_Click(sender As Object, e As EventArgs) Handles btnViewZoomIn.MouseDown
        ZoomInTimer.Start()
    End Sub

    ''' <summary>
    ''' Calls MouseInput's ZoomInOut method to increase the ZoomScale global (zoom in)
    ''' </summary>
    Private Sub ZoomInTick() Handles ZoomInTimer.Tick
        ZoomInOut(-1, New Point(RenderBoxSize.Width / 2, RenderBoxSize.Height / 2))
        GameLoop.PaintOnce()
    End Sub

    ''' <summary>
    ''' Stops ZoomInTimer
    ''' </summary>
    Private Sub btnViewZoomIn_MouseUp(sender As Object, e As MouseEventArgs) Handles btnViewZoomIn.MouseUp
        ZoomInTimer.Stop()
    End Sub

    ''' <summary>
    ''' Starts ZoomOutTimer
    ''' </summary>
    Private Sub btnViewZoomOut_MouseDown(sender As Object, e As EventArgs) Handles btnViewZoomOut.MouseDown
        ZoomOutTimer.Start()
    End Sub

    ''' <summary>
    ''' Calls MouseInput's ZoomInOut method to decrease the ZoomScale global (zoom out)
    ''' </summary>
    Private Sub ZoomOutTick() Handles ZoomOutTimer.Tick
        ZoomInOut(1, New Point(RenderBoxSize.Width / 2, RenderBoxSize.Height / 2))
        GameLoop.PaintOnce()
    End Sub

    ''' <summary>
    ''' Stops ZoomOutTimer
    ''' </summary>
    Private Sub btnViewZoomOut_MouseUp(sender As Object, e As MouseEventArgs) Handles btnViewZoomOut.MouseUp
        ZoomOutTimer.Stop()
    End Sub

#End Region

#Region "Simulation Options"

    ''' <summary>
    ''' Updates the GravitationEnabled and repaints
    ''' </summary>
    Private Sub EnableGravitation_CheckedChanged(sender As Object, e As EventArgs) Handles chkGravitation.CheckedChanged
        GravitationEnabled = chkGravitation.Checked
        GameLoop.PaintOnce()
    End Sub

    ''' <summary>
    ''' Starts the TimeFactorTimer
    ''' </summary>
    Private Sub trackerTimescale_MouseDown() Handles trackerTimescale.MouseDown
        TimescaleTimer.Start()
    End Sub

    ''' <summary>
    ''' Stops the TimescaleTimer and resets the slider back to the center
    ''' </summary>
    Private Sub trackerTimescale_MouseUp() Handles trackerTimescale.MouseUp
        TimescaleTimer.Stop()
        trackerTimescale.Value = 0
        pnlRender.Select()
    End Sub

    ''' <summary>
    ''' Updates Timescale with respect to the displacement of the slider
    ''' </summary>
    Private Sub TimescaleTimer_Tick() Handles TimescaleTimer.Tick
        Timescale *= 1 + trackerTimescale.Value / 50
        GameLoop.PaintOnce()
    End Sub

#End Region

#Region "Undo Changes"

    ''' <summary>
    ''' Updates the changes listbox (called by Changes)
    ''' </summary>
    Public Sub ChangeListHasChanged()

        lbChanges.Items.Clear()
        Changes.ChangesIntoListBox(lbChanges)

    End Sub

    ''' <summary>
    ''' Changes BodyDatas so that it mirrors the selected change in the changes listbox
    ''' </summary>
    Private Sub Changes_SelectedIndexChanged() Handles lbChanges.SelectedIndexChanged

        If lbChanges.SelectedIndex = -1 Then Return

        GameLoop.StoreAndPause()

        Dim OldestToNewestIndex = lbChanges.Items.Count - 1 - lbChanges.SelectedIndex
        Changes.RollbackChanges(OldestToNewestIndex)

        lbChanges.SelectedIndex = -1

        'Performs single paint within this call
        UpdateUI()
        GameLoop.ContinueStored()

    End Sub

    ''' <summary>
    ''' Stores a change in Changes with the ChangeType of SimulationRunTime
    ''' </summary>
    Private Sub AutoStoreSimulationWhileRunning() Handles AutoStoreSimulationChangesTimer.Tick
        Try
            Changes.AddChange(Changes.ChangeType.SimulationRunTime)
        Catch ex As Exception
        End Try
    End Sub

#End Region

#Region "Other Controls"

    ''' <summary>
    ''' Starts or stops the loop depending on current state
    ''' </summary>
    Private Sub TogglePlayPause(sender As Object, e As EventArgs) Handles btnPlayPause.Click
        If GameLoop.Running() Then
            GameLoop.Pause()
            Changes.AddChange(Changes.ChangeType.SimulationPaused)
        Else
            GameLoop.Start()
        End If
    End Sub

    ''' <summary>
    ''' Changes the text of the PlayPause button (called by the GameLoop) and changes the state of the auto-store timer
    ''' </summary>
    Public Sub GameLoopStateChanged()
        If GameLoop.Running Then
            btnPlayPause.Text = "Pause"
            AutoStoreSimulationChangesTimer.Start()
        Else
            btnPlayPause.Text = "Play"
            AutoStoreSimulationChangesTimer.Stop()
        End If
    End Sub

    ''' <summary>
    ''' Removes all bodies from BodyData, removes all trails and then updates the UI
    ''' </summary>
    Private Sub ClearScene(sender As Object, e As EventArgs) Handles btnClearSimulation.Click
        GameLoop.Pause()
        BodyDatas = Nothing
        Trails.ClearAllTrails()
        GameLoop.ResetSimulatedTime()
        Changes.AddChange(Changes.ChangeType.ClearSimulaton)
        UpdateUI()
    End Sub

    ''' <summary>
    ''' Loads a .gsim file and refreshes applications to show file contents
    ''' </summary>
    Private Sub LoadScene(sender As Object, e As EventArgs) Handles btnLoadScene.Click

        GameLoop.StoreAndPause()

        'The scene is already cleared within LoadCSVFile()

        If LoadCSVFile() Then
            Changes.AddChange(Changes.ChangeType.LoadScene)
            UpdateUI()
        Else
            GameLoop.ContinueStored()
        End If

    End Sub

    ''' <summary>
    ''' Guides the user through saving the current state of the application into a .gsim file
    ''' </summary>
    Private Sub SaveScene(sender As Object, e As EventArgs) Handles btnSaveScene.Click
        SaveCSVFile()
    End Sub

    ''' <summary>
    ''' Quits the application
    ''' </summary>
    Private Sub Quit(sender As Object, e As EventArgs) Handles btnQuit.Click
        Application.Exit()
    End Sub

#End Region

#Region "Mouse Input"

    ''' <summary>
    ''' Retrieves the name of the cursor type radio which is selected
    ''' </summary>
    Private Sub SetSelectedCursorFunction()

        'Runs through each radio in mouse function and returns the MouseFunction corresponding to the selected radio
        If radioAddBody.Checked Then
            SelectedRadio = MouseFunction.AddBody
        ElseIf radioAddOrbital.Checked Then
            SelectedRadio = MouseFunction.AddOrbital
        ElseIf radioChangeMass.Checked Then
            SelectedRadio = MouseFunction.ChangeMass
        ElseIf radioChangeVelocity.Checked Then
            SelectedRadio = MouseFunction.ChangeVelocity
        ElseIf radioMoveBody.Checked Then
            SelectedRadio = MouseFunction.MoveBody
        ElseIf radioMoveView.Checked Then
            SelectedRadio = MouseFunction.Pan
        ElseIf radioRemoveBody.Checked Then
            SelectedRadio = MouseFunction.RemoveBody
        ElseIf radioSelectBody.Checked Then
            SelectedRadio = MouseFunction.SelectBody
        End If

    End Sub

    ''' <summary>
    ''' Performs the correct method for a MouseDown event inside the viewport using the MouseInput module
    ''' </summary>
    Private Sub View_MouseDown(sender As Object, e As MouseEventArgs) Handles pnlRender.MouseDown

        SetSelectedCursorFunction()
        RunCursorSub(EventType.Down, e.Location)
        UpdateUI()

    End Sub

    ''' <summary>
    ''' Performs the correct method for a MouseMove event inside the viewport using the MouseInput module
    ''' </summary>
    Private Sub View_MouseMove(sender As Object, e As MouseEventArgs) Handles pnlRender.MouseMove

        SetSelectedCursorFunction()

        RunCursorSub(EventType.Move, e.Location)
        UpdateUI()

    End Sub

    ''' <summary>
    ''' Performs the correct method for a MouseUp event inside the viewport using the MouseInput module
    ''' </summary>
    Private Sub View_MouseUp(sender As Object, e As MouseEventArgs) Handles pnlRender.MouseUp

        SetSelectedCursorFunction()
        RunCursorSub(EventType.Up, e.Location)
        UpdateUI()

    End Sub

    ''' <summary>
    ''' Checks if the cursor is still over the viewport and if not then stop painting the orbital ring and the mouse label (used for Add Orbital)
    ''' </summary>
    Private Sub CheckCursorStillInView() Handles CheckCursorLeaveViewTimer.Tick

        Dim MousePosition As Point = Cursor.Position

        If Not InsideRectangle(MousePosition, New Rectangle(New Point(0, 0), RenderBoxSize)) Then
            PaintingMethods.PaintOrbital = False
            PaintingMethods.ShowMouseLabel = False
            GameLoop.PaintOnce()
        End If

    End Sub

    ''' <summary>
    ''' Uses MouseInput to manage changes to the zoom in the form of the mouse wheel
    ''' </summary>
    Private Sub View_MouseWheel(sender As Object, e As MouseEventArgs) Handles pnlRender.MouseWheel

        ZoomInOut(e.Delta, e.Location)
        GameLoop.PaintOnce()

    End Sub

#End Region

End Class
