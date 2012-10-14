<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class S5Form
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtLocalPath = New System.Windows.Forms.TextBox()
        Me.txtRemotePath = New System.Windows.Forms.TextBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.DontHashFiles = New System.Windows.Forms.CheckBox()
        Me.lblRemote = New System.Windows.Forms.Label()
        Me.lblLocal = New System.Windows.Forms.Label()
        Me.lblSecretAccessKey = New System.Windows.Forms.Label()
        Me.lblAccessKeyId = New System.Windows.Forms.Label()
        Me.ClearCompare = New System.Windows.Forms.Button()
        Me.ClearList = New System.Windows.Forms.Button()
        Me.cbxAcl = New System.Windows.Forms.ComboBox()
        Me.keySecret = New System.Windows.Forms.TextBox()
        Me.KeyId = New System.Windows.Forms.TextBox()
        Me.Synchronize = New System.Windows.Forms.Button()
        Me.Compare = New System.Windows.Forms.Button()
        Me.Add = New System.Windows.Forms.Button()
        Me.cbxAction = New System.Windows.Forms.ComboBox()
        Me.cbxRemoteMissing = New System.Windows.Forms.ComboBox()
        Me.cbxLocalMissing = New System.Windows.Forms.ComboBox()
        Me.cbxRemoteNewer = New System.Windows.Forms.ComboBox()
        Me.cbxLocalNewer = New System.Windows.Forms.ComboBox()
        Me.DontDownloadMetadata = New System.Windows.Forms.CheckBox()
        Me.OpenRemote = New System.Windows.Forms.Button()
        Me.OpenLocal = New System.Windows.Forms.Button()
        Me.lvStatus = New S5.SortedListView()
        Me.Local = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Remote = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Status = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Action = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Progress = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtLocalPath
        '
        Me.txtLocalPath.Location = New System.Drawing.Point(51, 32)
        Me.txtLocalPath.Name = "txtLocalPath"
        Me.txtLocalPath.Size = New System.Drawing.Size(404, 20)
        Me.txtLocalPath.TabIndex = 0
        '
        'txtRemotePath
        '
        Me.txtRemotePath.Location = New System.Drawing.Point(547, 32)
        Me.txtRemotePath.Name = "txtRemotePath"
        Me.txtRemotePath.Size = New System.Drawing.Size(375, 20)
        Me.txtRemotePath.TabIndex = 1
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.DontHashFiles)
        Me.SplitContainer1.Panel1.Controls.Add(Me.lblRemote)
        Me.SplitContainer1.Panel1.Controls.Add(Me.lblLocal)
        Me.SplitContainer1.Panel1.Controls.Add(Me.lblSecretAccessKey)
        Me.SplitContainer1.Panel1.Controls.Add(Me.lblAccessKeyId)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ClearCompare)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ClearList)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cbxAcl)
        Me.SplitContainer1.Panel1.Controls.Add(Me.keySecret)
        Me.SplitContainer1.Panel1.Controls.Add(Me.KeyId)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Synchronize)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Compare)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Add)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cbxAction)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cbxRemoteMissing)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cbxLocalMissing)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cbxRemoteNewer)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cbxLocalNewer)
        Me.SplitContainer1.Panel1.Controls.Add(Me.DontDownloadMetadata)
        Me.SplitContainer1.Panel1.Controls.Add(Me.OpenRemote)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtRemotePath)
        Me.SplitContainer1.Panel1.Controls.Add(Me.OpenLocal)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtLocalPath)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lvStatus)
        Me.SplitContainer1.Size = New System.Drawing.Size(1082, 644)
        Me.SplitContainer1.SplitterDistance = 206
        Me.SplitContainer1.TabIndex = 2
        '
        'DontHashFiles
        '
        Me.DontHashFiles.AutoSize = True
        Me.DontHashFiles.Location = New System.Drawing.Point(466, 172)
        Me.DontHashFiles.Name = "DontHashFiles"
        Me.DontHashFiles.Size = New System.Drawing.Size(103, 17)
        Me.DontHashFiles.TabIndex = 22
        Me.DontHashFiles.Text = "Don't Hash Files"
        Me.DontHashFiles.UseVisualStyleBackColor = True
        '
        'lblRemote
        '
        Me.lblRemote.AutoSize = True
        Me.lblRemote.Location = New System.Drawing.Point(497, 35)
        Me.lblRemote.Name = "lblRemote"
        Me.lblRemote.Size = New System.Drawing.Size(44, 13)
        Me.lblRemote.TabIndex = 21
        Me.lblRemote.Text = "Remote"
        '
        'lblLocal
        '
        Me.lblLocal.AutoSize = True
        Me.lblLocal.Location = New System.Drawing.Point(12, 35)
        Me.lblLocal.Name = "lblLocal"
        Me.lblLocal.Size = New System.Drawing.Size(33, 13)
        Me.lblLocal.TabIndex = 20
        Me.lblLocal.Text = "Local"
        '
        'lblSecretAccessKey
        '
        Me.lblSecretAccessKey.AutoSize = True
        Me.lblSecretAccessKey.Location = New System.Drawing.Point(497, 9)
        Me.lblSecretAccessKey.Name = "lblSecretAccessKey"
        Me.lblSecretAccessKey.Size = New System.Drawing.Size(97, 13)
        Me.lblSecretAccessKey.TabIndex = 19
        Me.lblSecretAccessKey.Text = "Secret Access Key"
        '
        'lblAccessKeyId
        '
        Me.lblAccessKeyId.AutoSize = True
        Me.lblAccessKeyId.Location = New System.Drawing.Point(12, 9)
        Me.lblAccessKeyId.Name = "lblAccessKeyId"
        Me.lblAccessKeyId.Size = New System.Drawing.Size(77, 13)
        Me.lblAccessKeyId.TabIndex = 18
        Me.lblAccessKeyId.Text = "Access Key ID"
        '
        'ClearCompare
        '
        Me.ClearCompare.Location = New System.Drawing.Point(161, 168)
        Me.ClearCompare.Name = "ClearCompare"
        Me.ClearCompare.Size = New System.Drawing.Size(143, 23)
        Me.ClearCompare.TabIndex = 17
        Me.ClearCompare.Text = "Clear Compare"
        Me.ClearCompare.UseVisualStyleBackColor = True
        '
        'ClearList
        '
        Me.ClearList.Location = New System.Drawing.Point(12, 168)
        Me.ClearList.Name = "ClearList"
        Me.ClearList.Size = New System.Drawing.Size(143, 23)
        Me.ClearList.TabIndex = 16
        Me.ClearList.Text = "Clear List"
        Me.ClearList.UseVisualStyleBackColor = True
        '
        'cbxAcl
        '
        Me.cbxAcl.DisplayMember = "0"
        Me.cbxAcl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxAcl.FormattingEnabled = True
        Me.cbxAcl.Location = New System.Drawing.Point(313, 58)
        Me.cbxAcl.Name = "cbxAcl"
        Me.cbxAcl.Size = New System.Drawing.Size(178, 21)
        Me.cbxAcl.TabIndex = 15
        '
        'keySecret
        '
        Me.keySecret.Location = New System.Drawing.Point(600, 6)
        Me.keySecret.Name = "keySecret"
        Me.keySecret.Size = New System.Drawing.Size(378, 20)
        Me.keySecret.TabIndex = 14
        '
        'KeyId
        '
        Me.KeyId.Location = New System.Drawing.Point(95, 6)
        Me.KeyId.Name = "KeyId"
        Me.KeyId.Size = New System.Drawing.Size(396, 20)
        Me.KeyId.TabIndex = 13
        '
        'Synchronize
        '
        Me.Synchronize.Enabled = False
        Me.Synchronize.Location = New System.Drawing.Point(313, 139)
        Me.Synchronize.Name = "Synchronize"
        Me.Synchronize.Size = New System.Drawing.Size(143, 23)
        Me.Synchronize.TabIndex = 12
        Me.Synchronize.Text = "Synchronize"
        Me.Synchronize.UseVisualStyleBackColor = True
        '
        'Compare
        '
        Me.Compare.Enabled = False
        Me.Compare.Location = New System.Drawing.Point(161, 139)
        Me.Compare.Name = "Compare"
        Me.Compare.Size = New System.Drawing.Size(143, 23)
        Me.Compare.TabIndex = 11
        Me.Compare.Text = "Compare"
        Me.Compare.UseVisualStyleBackColor = True
        '
        'Add
        '
        Me.Add.Location = New System.Drawing.Point(12, 139)
        Me.Add.Name = "Add"
        Me.Add.Size = New System.Drawing.Size(143, 23)
        Me.Add.TabIndex = 10
        Me.Add.Text = "Add"
        Me.Add.UseVisualStyleBackColor = True
        '
        'cbxAction
        '
        Me.cbxAction.DisplayMember = "0"
        Me.cbxAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxAction.FormattingEnabled = True
        Me.cbxAction.Location = New System.Drawing.Point(12, 58)
        Me.cbxAction.Name = "cbxAction"
        Me.cbxAction.Size = New System.Drawing.Size(293, 21)
        Me.cbxAction.TabIndex = 9
        '
        'cbxRemoteMissing
        '
        Me.cbxRemoteMissing.DisplayMember = "0"
        Me.cbxRemoteMissing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxRemoteMissing.FormattingEnabled = True
        Me.cbxRemoteMissing.Items.AddRange(New Object() {"If Remote is Missing: No action.", "If Remote is Missing: Copy to Remote.", "If Remote is Missing: Delete from Local."})
        Me.cbxRemoteMissing.Location = New System.Drawing.Point(313, 112)
        Me.cbxRemoteMissing.Name = "cbxRemoteMissing"
        Me.cbxRemoteMissing.Size = New System.Drawing.Size(293, 21)
        Me.cbxRemoteMissing.TabIndex = 8
        '
        'cbxLocalMissing
        '
        Me.cbxLocalMissing.DisplayMember = "0"
        Me.cbxLocalMissing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxLocalMissing.FormattingEnabled = True
        Me.cbxLocalMissing.Items.AddRange(New Object() {"If Local is Missing: No action.", "If Local is Missing: Delete from Remote.", "If Local is Missing: Copy to Local."})
        Me.cbxLocalMissing.Location = New System.Drawing.Point(313, 85)
        Me.cbxLocalMissing.Name = "cbxLocalMissing"
        Me.cbxLocalMissing.Size = New System.Drawing.Size(293, 21)
        Me.cbxLocalMissing.TabIndex = 7
        '
        'cbxRemoteNewer
        '
        Me.cbxRemoteNewer.DisplayMember = "0"
        Me.cbxRemoteNewer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxRemoteNewer.FormattingEnabled = True
        Me.cbxRemoteNewer.Items.AddRange(New Object() {"If Remote is Newer: No action.", "If Remote is Newer: Overwite Remote.", "If Remote is Newer: Overwite Local."})
        Me.cbxRemoteNewer.Location = New System.Drawing.Point(12, 112)
        Me.cbxRemoteNewer.Name = "cbxRemoteNewer"
        Me.cbxRemoteNewer.Size = New System.Drawing.Size(293, 21)
        Me.cbxRemoteNewer.TabIndex = 6
        '
        'cbxLocalNewer
        '
        Me.cbxLocalNewer.DisplayMember = "0"
        Me.cbxLocalNewer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxLocalNewer.FormattingEnabled = True
        Me.cbxLocalNewer.Items.AddRange(New Object() {"If Local is Newer: No action.", "If Local is Newer: Overwite Remote.", "If Local is Newer: Overwite Local."})
        Me.cbxLocalNewer.Location = New System.Drawing.Point(12, 85)
        Me.cbxLocalNewer.Name = "cbxLocalNewer"
        Me.cbxLocalNewer.Size = New System.Drawing.Size(293, 21)
        Me.cbxLocalNewer.TabIndex = 4
        '
        'DontDownloadMetadata
        '
        Me.DontDownloadMetadata.AutoSize = True
        Me.DontDownloadMetadata.Location = New System.Drawing.Point(313, 172)
        Me.DontDownloadMetadata.Name = "DontDownloadMetadata"
        Me.DontDownloadMetadata.Size = New System.Drawing.Size(147, 17)
        Me.DontDownloadMetadata.TabIndex = 3
        Me.DontDownloadMetadata.Text = "Don't download metadata"
        Me.DontDownloadMetadata.UseVisualStyleBackColor = True
        '
        'OpenRemote
        '
        Me.OpenRemote.Location = New System.Drawing.Point(928, 31)
        Me.OpenRemote.Name = "OpenRemote"
        Me.OpenRemote.Size = New System.Drawing.Size(30, 20)
        Me.OpenRemote.TabIndex = 2
        Me.OpenRemote.Text = "..."
        Me.OpenRemote.UseVisualStyleBackColor = True
        '
        'OpenLocal
        '
        Me.OpenLocal.Location = New System.Drawing.Point(461, 31)
        Me.OpenLocal.Name = "OpenLocal"
        Me.OpenLocal.Size = New System.Drawing.Size(30, 20)
        Me.OpenLocal.TabIndex = 1
        Me.OpenLocal.Text = "..."
        Me.OpenLocal.UseVisualStyleBackColor = True
        '
        'lvStatus
        '
        Me.lvStatus.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Local, Me.Remote, Me.Status, Me.Action, Me.Progress})
        Me.lvStatus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvStatus.Location = New System.Drawing.Point(0, 0)
        Me.lvStatus.Name = "lvStatus"
        Me.lvStatus.Size = New System.Drawing.Size(1082, 434)
        Me.lvStatus.TabIndex = 1
        Me.lvStatus.UseCompatibleStateImageBehavior = False
        Me.lvStatus.View = System.Windows.Forms.View.Details
        '
        'Local
        '
        Me.Local.Text = "Local"
        Me.Local.Width = 157
        '
        'Remote
        '
        Me.Remote.Text = "Remote"
        Me.Remote.Width = 169
        '
        'Status
        '
        Me.Status.Text = "Status"
        Me.Status.Width = 145
        '
        'Action
        '
        Me.Action.Text = "Action"
        Me.Action.Width = 149
        '
        'Progress
        '
        Me.Progress.Text = "Progress"
        Me.Progress.Width = 92
        '
        'S5Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1082, 644)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "S5Form"
        Me.Text = "S5"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtLocalPath As System.Windows.Forms.TextBox
    Friend WithEvents txtRemotePath As System.Windows.Forms.TextBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents OpenRemote As System.Windows.Forms.Button
    Friend WithEvents OpenLocal As System.Windows.Forms.Button
    Friend WithEvents lvStatus As SortedListView
    Friend WithEvents Local As System.Windows.Forms.ColumnHeader
    Friend WithEvents Remote As System.Windows.Forms.ColumnHeader
    Friend WithEvents Status As System.Windows.Forms.ColumnHeader
    Friend WithEvents cbxLocalNewer As System.Windows.Forms.ComboBox
    Friend WithEvents DontDownloadMetadata As System.Windows.Forms.CheckBox
    Friend WithEvents cbxRemoteNewer As System.Windows.Forms.ComboBox
    Friend WithEvents cbxLocalMissing As System.Windows.Forms.ComboBox
    Friend WithEvents cbxAction As System.Windows.Forms.ComboBox
    Friend WithEvents cbxRemoteMissing As System.Windows.Forms.ComboBox
    Friend WithEvents Synchronize As System.Windows.Forms.Button
    Friend WithEvents Compare As System.Windows.Forms.Button
    Friend WithEvents Add As System.Windows.Forms.Button
    Friend WithEvents keySecret As System.Windows.Forms.TextBox
    Friend WithEvents KeyId As System.Windows.Forms.TextBox
    Friend WithEvents Action As System.Windows.Forms.ColumnHeader
    Friend WithEvents cbxAcl As System.Windows.Forms.ComboBox
    Friend WithEvents Progress As System.Windows.Forms.ColumnHeader
    Friend WithEvents ClearCompare As System.Windows.Forms.Button
    Friend WithEvents ClearList As System.Windows.Forms.Button
    Friend WithEvents DontHashFiles As System.Windows.Forms.CheckBox
    Friend WithEvents lblRemote As System.Windows.Forms.Label
    Friend WithEvents lblLocal As System.Windows.Forms.Label
    Friend WithEvents lblSecretAccessKey As System.Windows.Forms.Label
    Friend WithEvents lblAccessKeyId As System.Windows.Forms.Label

End Class
