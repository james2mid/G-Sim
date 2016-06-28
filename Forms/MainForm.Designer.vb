<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.pnlControls = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnLoadScene = New System.Windows.Forms.Button()
        Me.btnPlayPause = New System.Windows.Forms.Button()
        Me.btnClearSimulation = New System.Windows.Forms.Button()
        Me.btnSaveScene = New System.Windows.Forms.Button()
        Me.btnQuit = New System.Windows.Forms.Button()
        Me.grpChanges = New System.Windows.Forms.GroupBox()
        Me.lbChanges = New System.Windows.Forms.ListBox()
        Me.grpCursorFunctions = New System.Windows.Forms.GroupBox()
        Me.radioAddOrbital = New System.Windows.Forms.RadioButton()
        Me.radioChangeMass = New System.Windows.Forms.RadioButton()
        Me.radioChangeVelocity = New System.Windows.Forms.RadioButton()
        Me.radioMoveBody = New System.Windows.Forms.RadioButton()
        Me.radioRemoveBody = New System.Windows.Forms.RadioButton()
        Me.radioSelectBody = New System.Windows.Forms.RadioButton()
        Me.radioMoveView = New System.Windows.Forms.RadioButton()
        Me.radioAddBody = New System.Windows.Forms.RadioButton()
        Me.grpSimulationOptions = New System.Windows.Forms.GroupBox()
        Me.chkGravitation = New System.Windows.Forms.CheckBox()
        Me.lblTimescale = New System.Windows.Forms.Label()
        Me.trackerTimescale = New System.Windows.Forms.TrackBar()
        Me.grpViewOptions = New System.Windows.Forms.GroupBox()
        Me.btnViewZoomOut = New System.Windows.Forms.Button()
        Me.btnViewZoomIn = New System.Windows.Forms.Button()
        Me.chkShowVelocity = New System.Windows.Forms.CheckBox()
        Me.btnClearTrails = New System.Windows.Forms.Button()
        Me.chkShowTrails = New System.Windows.Forms.CheckBox()
        Me.grpSelectedBody = New System.Windows.Forms.GroupBox()
        Me.lbSelectedBodyInfo = New System.Windows.Forms.ListBox()
        Me.btnSelectedBodySetImage = New System.Windows.Forms.Button()
        Me.pnlRender = New System.Windows.Forms.Panel()
        Me.pnlControls.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.grpChanges.SuspendLayout()
        Me.grpCursorFunctions.SuspendLayout()
        Me.grpSimulationOptions.SuspendLayout()
        CType(Me.trackerTimescale, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpViewOptions.SuspendLayout()
        Me.grpSelectedBody.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlControls
        '
        Me.pnlControls.AutoScroll = True
        Me.pnlControls.Controls.Add(Me.Panel1)
        Me.pnlControls.Controls.Add(Me.grpChanges)
        Me.pnlControls.Controls.Add(Me.grpCursorFunctions)
        Me.pnlControls.Controls.Add(Me.grpSimulationOptions)
        Me.pnlControls.Controls.Add(Me.grpViewOptions)
        Me.pnlControls.Controls.Add(Me.grpSelectedBody)
        Me.pnlControls.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlControls.Location = New System.Drawing.Point(966, 0)
        Me.pnlControls.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(456, 1106)
        Me.pnlControls.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnLoadScene)
        Me.Panel1.Controls.Add(Me.btnPlayPause)
        Me.Panel1.Controls.Add(Me.btnClearSimulation)
        Me.Panel1.Controls.Add(Me.btnSaveScene)
        Me.Panel1.Controls.Add(Me.btnQuit)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 898)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(456, 208)
        Me.Panel1.TabIndex = 37
        '
        'btnLoadScene
        '
        Me.btnLoadScene.Location = New System.Drawing.Point(27, 72)
        Me.btnLoadScene.Margin = New System.Windows.Forms.Padding(6)
        Me.btnLoadScene.Name = "btnLoadScene"
        Me.btnLoadScene.Size = New System.Drawing.Size(186, 44)
        Me.btnLoadScene.TabIndex = 29
        Me.btnLoadScene.Text = "Load Scene"
        Me.btnLoadScene.UseVisualStyleBackColor = True
        '
        'btnPlayPause
        '
        Me.btnPlayPause.Location = New System.Drawing.Point(27, 16)
        Me.btnPlayPause.Margin = New System.Windows.Forms.Padding(6)
        Me.btnPlayPause.Name = "btnPlayPause"
        Me.btnPlayPause.Size = New System.Drawing.Size(186, 44)
        Me.btnPlayPause.TabIndex = 10
        Me.btnPlayPause.Text = "Play"
        Me.btnPlayPause.UseVisualStyleBackColor = True
        '
        'btnClearSimulation
        '
        Me.btnClearSimulation.Location = New System.Drawing.Point(225, 16)
        Me.btnClearSimulation.Margin = New System.Windows.Forms.Padding(6)
        Me.btnClearSimulation.Name = "btnClearSimulation"
        Me.btnClearSimulation.Size = New System.Drawing.Size(198, 44)
        Me.btnClearSimulation.TabIndex = 8
        Me.btnClearSimulation.Text = "Clear Scene"
        Me.btnClearSimulation.UseVisualStyleBackColor = True
        '
        'btnSaveScene
        '
        Me.btnSaveScene.Location = New System.Drawing.Point(225, 72)
        Me.btnSaveScene.Margin = New System.Windows.Forms.Padding(6)
        Me.btnSaveScene.Name = "btnSaveScene"
        Me.btnSaveScene.Size = New System.Drawing.Size(198, 44)
        Me.btnSaveScene.TabIndex = 28
        Me.btnSaveScene.Text = "Save Scene"
        Me.btnSaveScene.UseVisualStyleBackColor = True
        '
        'btnQuit
        '
        Me.btnQuit.Location = New System.Drawing.Point(27, 128)
        Me.btnQuit.Margin = New System.Windows.Forms.Padding(6)
        Me.btnQuit.Name = "btnQuit"
        Me.btnQuit.Size = New System.Drawing.Size(396, 66)
        Me.btnQuit.TabIndex = 31
        Me.btnQuit.Text = "Quit"
        Me.btnQuit.UseVisualStyleBackColor = True
        '
        'grpChanges
        '
        Me.grpChanges.Controls.Add(Me.lbChanges)
        Me.grpChanges.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpChanges.Location = New System.Drawing.Point(0, 659)
        Me.grpChanges.Name = "grpChanges"
        Me.grpChanges.Size = New System.Drawing.Size(456, 239)
        Me.grpChanges.TabIndex = 38
        Me.grpChanges.TabStop = False
        Me.grpChanges.Text = "Undo Changes"
        '
        'lbChanges
        '
        Me.lbChanges.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbChanges.FormattingEnabled = True
        Me.lbChanges.ItemHeight = 25
        Me.lbChanges.Location = New System.Drawing.Point(3, 27)
        Me.lbChanges.Name = "lbChanges"
        Me.lbChanges.Size = New System.Drawing.Size(450, 209)
        Me.lbChanges.TabIndex = 0
        '
        'grpCursorFunctions
        '
        Me.grpCursorFunctions.Controls.Add(Me.radioAddOrbital)
        Me.grpCursorFunctions.Controls.Add(Me.radioChangeMass)
        Me.grpCursorFunctions.Controls.Add(Me.radioChangeVelocity)
        Me.grpCursorFunctions.Controls.Add(Me.radioMoveBody)
        Me.grpCursorFunctions.Controls.Add(Me.radioRemoveBody)
        Me.grpCursorFunctions.Controls.Add(Me.radioSelectBody)
        Me.grpCursorFunctions.Controls.Add(Me.radioMoveView)
        Me.grpCursorFunctions.Controls.Add(Me.radioAddBody)
        Me.grpCursorFunctions.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpCursorFunctions.Location = New System.Drawing.Point(0, 454)
        Me.grpCursorFunctions.Margin = New System.Windows.Forms.Padding(6)
        Me.grpCursorFunctions.Name = "grpCursorFunctions"
        Me.grpCursorFunctions.Padding = New System.Windows.Forms.Padding(6)
        Me.grpCursorFunctions.Size = New System.Drawing.Size(456, 205)
        Me.grpCursorFunctions.TabIndex = 30
        Me.grpCursorFunctions.TabStop = False
        Me.grpCursorFunctions.Text = "Cursor Function"
        '
        'radioAddOrbital
        '
        Me.radioAddOrbital.AutoSize = True
        Me.radioAddOrbital.Location = New System.Drawing.Point(258, 88)
        Me.radioAddOrbital.Name = "radioAddOrbital"
        Me.radioAddOrbital.Size = New System.Drawing.Size(150, 29)
        Me.radioAddOrbital.TabIndex = 10
        Me.radioAddOrbital.TabStop = True
        Me.radioAddOrbital.Text = "Add Orbital"
        Me.radioAddOrbital.UseVisualStyleBackColor = True
        '
        'radioChangeMass
        '
        Me.radioChangeMass.AutoSize = True
        Me.radioChangeMass.Location = New System.Drawing.Point(258, 164)
        Me.radioChangeMass.Name = "radioChangeMass"
        Me.radioChangeMass.Size = New System.Drawing.Size(176, 29)
        Me.radioChangeMass.TabIndex = 8
        Me.radioChangeMass.TabStop = True
        Me.radioChangeMass.Text = "Change Mass"
        Me.radioChangeMass.UseVisualStyleBackColor = True
        '
        'radioChangeVelocity
        '
        Me.radioChangeVelocity.AutoSize = True
        Me.radioChangeVelocity.Location = New System.Drawing.Point(17, 164)
        Me.radioChangeVelocity.Name = "radioChangeVelocity"
        Me.radioChangeVelocity.Size = New System.Drawing.Size(200, 29)
        Me.radioChangeVelocity.TabIndex = 6
        Me.radioChangeVelocity.TabStop = True
        Me.radioChangeVelocity.Text = "Change Velocity"
        Me.radioChangeVelocity.UseVisualStyleBackColor = True
        '
        'radioMoveBody
        '
        Me.radioMoveBody.AutoSize = True
        Me.radioMoveBody.Location = New System.Drawing.Point(17, 126)
        Me.radioMoveBody.Margin = New System.Windows.Forms.Padding(6)
        Me.radioMoveBody.Name = "radioMoveBody"
        Me.radioMoveBody.Size = New System.Drawing.Size(151, 29)
        Me.radioMoveBody.TabIndex = 4
        Me.radioMoveBody.Text = "Move Body"
        Me.radioMoveBody.UseVisualStyleBackColor = True
        '
        'radioRemoveBody
        '
        Me.radioRemoveBody.AutoSize = True
        Me.radioRemoveBody.Location = New System.Drawing.Point(258, 126)
        Me.radioRemoveBody.Margin = New System.Windows.Forms.Padding(6)
        Me.radioRemoveBody.Name = "radioRemoveBody"
        Me.radioRemoveBody.Size = New System.Drawing.Size(177, 29)
        Me.radioRemoveBody.TabIndex = 3
        Me.radioRemoveBody.Text = "Remove Body"
        Me.radioRemoveBody.UseVisualStyleBackColor = True
        '
        'radioSelectBody
        '
        Me.radioSelectBody.AutoSize = True
        Me.radioSelectBody.Location = New System.Drawing.Point(17, 88)
        Me.radioSelectBody.Margin = New System.Windows.Forms.Padding(6)
        Me.radioSelectBody.Name = "radioSelectBody"
        Me.radioSelectBody.Size = New System.Drawing.Size(158, 29)
        Me.radioSelectBody.TabIndex = 2
        Me.radioSelectBody.Text = "Select Body"
        Me.radioSelectBody.UseVisualStyleBackColor = True
        '
        'radioMoveView
        '
        Me.radioMoveView.AutoSize = True
        Me.radioMoveView.Checked = True
        Me.radioMoveView.Location = New System.Drawing.Point(17, 50)
        Me.radioMoveView.Margin = New System.Windows.Forms.Padding(6)
        Me.radioMoveView.Name = "radioMoveView"
        Me.radioMoveView.Size = New System.Drawing.Size(81, 29)
        Me.radioMoveView.TabIndex = 1
        Me.radioMoveView.TabStop = True
        Me.radioMoveView.Text = "Pan"
        Me.radioMoveView.UseVisualStyleBackColor = True
        '
        'radioAddBody
        '
        Me.radioAddBody.AutoSize = True
        Me.radioAddBody.Location = New System.Drawing.Point(258, 50)
        Me.radioAddBody.Margin = New System.Windows.Forms.Padding(6)
        Me.radioAddBody.Name = "radioAddBody"
        Me.radioAddBody.Size = New System.Drawing.Size(136, 29)
        Me.radioAddBody.TabIndex = 0
        Me.radioAddBody.Text = "Add Body"
        Me.radioAddBody.UseVisualStyleBackColor = True
        '
        'grpSimulationOptions
        '
        Me.grpSimulationOptions.Controls.Add(Me.chkGravitation)
        Me.grpSimulationOptions.Controls.Add(Me.lblTimescale)
        Me.grpSimulationOptions.Controls.Add(Me.trackerTimescale)
        Me.grpSimulationOptions.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpSimulationOptions.Location = New System.Drawing.Point(0, 332)
        Me.grpSimulationOptions.Name = "grpSimulationOptions"
        Me.grpSimulationOptions.Size = New System.Drawing.Size(456, 122)
        Me.grpSimulationOptions.TabIndex = 35
        Me.grpSimulationOptions.TabStop = False
        Me.grpSimulationOptions.Text = "Simulation Options"
        '
        'chkGravitation
        '
        Me.chkGravitation.AutoSize = True
        Me.chkGravitation.Checked = True
        Me.chkGravitation.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGravitation.Location = New System.Drawing.Point(17, 33)
        Me.chkGravitation.Margin = New System.Windows.Forms.Padding(6)
        Me.chkGravitation.Name = "chkGravitation"
        Me.chkGravitation.Size = New System.Drawing.Size(112, 29)
        Me.chkGravitation.TabIndex = 17
        Me.chkGravitation.Text = "Gravity"
        Me.chkGravitation.UseVisualStyleBackColor = True
        '
        'lblTimescale
        '
        Me.lblTimescale.AutoSize = True
        Me.lblTimescale.Location = New System.Drawing.Point(22, 82)
        Me.lblTimescale.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblTimescale.Name = "lblTimescale"
        Me.lblTimescale.Size = New System.Drawing.Size(110, 25)
        Me.lblTimescale.TabIndex = 8
        Me.lblTimescale.Text = "Timescale"
        '
        'trackerTimescale
        '
        Me.trackerTimescale.LargeChange = 0
        Me.trackerTimescale.Location = New System.Drawing.Point(162, 73)
        Me.trackerTimescale.Margin = New System.Windows.Forms.Padding(6)
        Me.trackerTimescale.Minimum = -10
        Me.trackerTimescale.Name = "trackerTimescale"
        Me.trackerTimescale.Size = New System.Drawing.Size(261, 90)
        Me.trackerTimescale.SmallChange = 0
        Me.trackerTimescale.TabIndex = 5
        Me.trackerTimescale.TickStyle = System.Windows.Forms.TickStyle.None
        '
        'grpViewOptions
        '
        Me.grpViewOptions.Controls.Add(Me.btnViewZoomOut)
        Me.grpViewOptions.Controls.Add(Me.btnViewZoomIn)
        Me.grpViewOptions.Controls.Add(Me.chkShowVelocity)
        Me.grpViewOptions.Controls.Add(Me.btnClearTrails)
        Me.grpViewOptions.Controls.Add(Me.chkShowTrails)
        Me.grpViewOptions.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpViewOptions.Location = New System.Drawing.Point(0, 208)
        Me.grpViewOptions.Name = "grpViewOptions"
        Me.grpViewOptions.Size = New System.Drawing.Size(456, 124)
        Me.grpViewOptions.TabIndex = 34
        Me.grpViewOptions.TabStop = False
        Me.grpViewOptions.Text = "View Options"
        '
        'btnViewZoomOut
        '
        Me.btnViewZoomOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnViewZoomOut.Location = New System.Drawing.Point(371, 69)
        Me.btnViewZoomOut.Name = "btnViewZoomOut"
        Me.btnViewZoomOut.Size = New System.Drawing.Size(50, 50)
        Me.btnViewZoomOut.TabIndex = 35
        Me.btnViewZoomOut.Text = "-"
        Me.btnViewZoomOut.UseVisualStyleBackColor = True
        '
        'btnViewZoomIn
        '
        Me.btnViewZoomIn.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnViewZoomIn.Location = New System.Drawing.Point(371, 19)
        Me.btnViewZoomIn.Name = "btnViewZoomIn"
        Me.btnViewZoomIn.Size = New System.Drawing.Size(50, 50)
        Me.btnViewZoomIn.TabIndex = 34
        Me.btnViewZoomIn.Text = "+"
        Me.btnViewZoomIn.UseVisualStyleBackColor = True
        '
        'chkShowVelocity
        '
        Me.chkShowVelocity.AutoSize = True
        Me.chkShowVelocity.Checked = True
        Me.chkShowVelocity.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowVelocity.Location = New System.Drawing.Point(17, 40)
        Me.chkShowVelocity.Margin = New System.Windows.Forms.Padding(6)
        Me.chkShowVelocity.Name = "chkShowVelocity"
        Me.chkShowVelocity.Size = New System.Drawing.Size(179, 29)
        Me.chkShowVelocity.TabIndex = 13
        Me.chkShowVelocity.Text = "Show Velocity"
        Me.chkShowVelocity.UseVisualStyleBackColor = True
        '
        'btnClearTrails
        '
        Me.btnClearTrails.Location = New System.Drawing.Point(182, 75)
        Me.btnClearTrails.Name = "btnClearTrails"
        Me.btnClearTrails.Size = New System.Drawing.Size(146, 39)
        Me.btnClearTrails.TabIndex = 33
        Me.btnClearTrails.Text = "Clear Trails"
        Me.btnClearTrails.UseVisualStyleBackColor = True
        '
        'chkShowTrails
        '
        Me.chkShowTrails.AutoSize = True
        Me.chkShowTrails.Checked = True
        Me.chkShowTrails.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowTrails.Location = New System.Drawing.Point(17, 81)
        Me.chkShowTrails.Margin = New System.Windows.Forms.Padding(6)
        Me.chkShowTrails.Name = "chkShowTrails"
        Me.chkShowTrails.Size = New System.Drawing.Size(156, 29)
        Me.chkShowTrails.TabIndex = 32
        Me.chkShowTrails.Text = "Show Trails"
        Me.chkShowTrails.UseVisualStyleBackColor = True
        '
        'grpSelectedBody
        '
        Me.grpSelectedBody.Controls.Add(Me.lbSelectedBodyInfo)
        Me.grpSelectedBody.Controls.Add(Me.btnSelectedBodySetImage)
        Me.grpSelectedBody.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpSelectedBody.Location = New System.Drawing.Point(0, 0)
        Me.grpSelectedBody.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSelectedBody.Name = "grpSelectedBody"
        Me.grpSelectedBody.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSelectedBody.Size = New System.Drawing.Size(456, 208)
        Me.grpSelectedBody.TabIndex = 0
        Me.grpSelectedBody.TabStop = False
        Me.grpSelectedBody.Text = "Selected Body"
        '
        'lbSelectedBodyInfo
        '
        Me.lbSelectedBodyInfo.Enabled = False
        Me.lbSelectedBodyInfo.FormattingEnabled = True
        Me.lbSelectedBodyInfo.ItemHeight = 25
        Me.lbSelectedBodyInfo.Location = New System.Drawing.Point(7, 40)
        Me.lbSelectedBodyInfo.Name = "lbSelectedBodyInfo"
        Me.lbSelectedBodyInfo.Size = New System.Drawing.Size(435, 104)
        Me.lbSelectedBodyInfo.TabIndex = 21
        '
        'btnSelectedBodySetImage
        '
        Me.btnSelectedBodySetImage.Location = New System.Drawing.Point(116, 152)
        Me.btnSelectedBodySetImage.Name = "btnSelectedBodySetImage"
        Me.btnSelectedBodySetImage.Size = New System.Drawing.Size(202, 44)
        Me.btnSelectedBodySetImage.TabIndex = 20
        Me.btnSelectedBodySetImage.Text = "Set Image"
        Me.btnSelectedBodySetImage.UseVisualStyleBackColor = True
        '
        'pnlRender
        '
        Me.pnlRender.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlRender.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlRender.Location = New System.Drawing.Point(0, 0)
        Me.pnlRender.Margin = New System.Windows.Forms.Padding(6)
        Me.pnlRender.Name = "pnlRender"
        Me.pnlRender.Size = New System.Drawing.Size(966, 1106)
        Me.pnlRender.TabIndex = 1
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1422, 1106)
        Me.Controls.Add(Me.pnlRender)
        Me.Controls.Add(Me.pnlControls)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlControls.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.grpChanges.ResumeLayout(False)
        Me.grpCursorFunctions.ResumeLayout(False)
        Me.grpCursorFunctions.PerformLayout()
        Me.grpSimulationOptions.ResumeLayout(False)
        Me.grpSimulationOptions.PerformLayout()
        CType(Me.trackerTimescale, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpViewOptions.ResumeLayout(False)
        Me.grpViewOptions.PerformLayout()
        Me.grpSelectedBody.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpSelectedBody As GroupBox
    Friend WithEvents btnClearSimulation As Button
    Friend WithEvents lblTimescale As Label
    Friend WithEvents btnPlayPause As Button
    Friend WithEvents chkShowVelocity As CheckBox
    Friend WithEvents chkGravitation As CheckBox
    Friend WithEvents btnSaveScene As Button
    Friend WithEvents btnLoadScene As Button
    Friend WithEvents grpCursorFunctions As GroupBox
    Friend WithEvents radioMoveBody As RadioButton
    Friend WithEvents radioRemoveBody As RadioButton
    Friend WithEvents radioSelectBody As RadioButton
    Friend WithEvents radioMoveView As RadioButton
    Friend WithEvents radioAddBody As RadioButton
    Friend WithEvents btnQuit As Button
    Friend WithEvents chkShowTrails As CheckBox
    Friend WithEvents btnClearTrails As Button
    Friend WithEvents grpSimulationOptions As GroupBox
    Friend WithEvents grpViewOptions As GroupBox
    Friend WithEvents radioChangeVelocity As RadioButton
    Friend WithEvents radioChangeMass As RadioButton
    Public WithEvents pnlControls As Panel
    Public WithEvents pnlRender As Panel
    Public WithEvents trackerTimescale As TrackBar
    Friend WithEvents btnSelectedBodySetImage As Button
    Friend WithEvents lbSelectedBodyInfo As ListBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnViewZoomOut As Button
    Friend WithEvents btnViewZoomIn As Button
    Friend WithEvents radioAddOrbital As RadioButton
    Friend WithEvents grpChanges As GroupBox
    Friend WithEvents lbChanges As ListBox
End Class
