Option Explicit On
Option Strict On

Class GameLoop

    Private Shared _Running As Boolean = False
	Public Shared Property Running As Boolean
		Get
			Return _Running
		End Get
		Private Set(value As Boolean)
			_Running = value
			MainForm.GameLoopStateChanged()
		End Set
	End Property

    Public Shared Property SimulatedTime As Double = 0
    Public Shared FPS As Integer = 0

    Private Shared Property FrameMilliseconds As Integer = 0
    Private Shared FrameStopwatch As New Stopwatch()
    Private Shared SecondStopwatch As New Stopwatch()
    Private Shared FramesInSecond As Integer = 0
    Private Shared bgLoop As Task

	Private Shared Sub Game_Loop()

		SecondStopwatch.Restart()

		While Running
			FrameStopwatch.Restart()

			Update(CSng(FrameMilliseconds / 1000))
			Paint()

			FramesInSecond += 1

			If SecondStopwatch.ElapsedMilliseconds > 999 Then
				FPS = CInt(1000 * FramesInSecond / SecondStopwatch.ElapsedMilliseconds)
				FramesInSecond = 0
				SimulatedTime += Timescale * SecondStopwatch.ElapsedMilliseconds / 1000

				If ShowTrails Then
					If FPS < 10 And Trails.MaxBodyPoints >= 200 Then
						Trails.MaxBodyPoints -= 100
					ElseIf FPS < 20 And Trails.MaxBodyPoints >= 110 Then
						Trails.MaxBodyPoints -= 10
					ElseIf FPS > 60 And Trails.MaxBodyPoints <= Trails.MaxBodyPointsStartValue - 100 Then
						Trails.MaxBodyPoints += 100
					ElseIf FPS > 50 And Trails.MaxBodyPoints <= Trails.MaxBodyPointsStartValue - 10 Then
						Trails.MaxBodyPoints += 10
					End If
				End If

				SecondStopwatch.Restart()
			End If

			FrameMilliseconds = FrameStopwatch.Elapsed.Milliseconds
		End While
		SecondStopwatch.Stop()
		FrameStopwatch.Stop()

	End Sub

	Private Shared Sub StartLoop()
		bgLoop = New Task(AddressOf Game_Loop)
		bgLoop.Start()
	End Sub

	Shared Sub Start()
		If Running = False Then
			Running = True
			StartLoop()
		End If
	End Sub

	Shared Sub Pause()
		If Running Then
			Running = False
			Wait()
		End If
	End Sub

	Private Shared Sub Wait()

		If bgLoop Is Nothing Then
			Return
		End If

		bgLoop.Wait()

	End Sub

	Shared Sub PaintOnce()
		If Not Running And Not BufferEmpty() Then
			FPS = 0
			FramesInSecond = 0
			PaintingMethods.Paint()
		End If
	End Sub

	Shared Sub ResetSimulatedTime()
		SimulatedTime = 0
	End Sub

    Private Shared StoredState As Boolean = False

	Public Shared Sub StoreAndPause()
		StoredState = Running
		Pause()
	End Sub

	Public Shared Sub ContinueStored()
		If Not StoredState = Running Then
			If StoredState Then
				Start()
			End If
		End If
	End Sub
End Class