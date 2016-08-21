<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.btnStartCrawling = New System.Windows.Forms.Button()
        Me.dgvJobOffers = New System.Windows.Forms.DataGridView()
        Me.btnPauseCrawling = New System.Windows.Forms.Button()
        Me.tbInfo = New System.Windows.Forms.RichTextBox()
        CType(Me.dgvJobOffers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnStartCrawling
        '
        Me.btnStartCrawling.Location = New System.Drawing.Point(12, 651)
        Me.btnStartCrawling.Name = "btnStartCrawling"
        Me.btnStartCrawling.Size = New System.Drawing.Size(141, 46)
        Me.btnStartCrawling.TabIndex = 0
        Me.btnStartCrawling.Text = "Crawling starten"
        Me.btnStartCrawling.UseVisualStyleBackColor = True
        '
        'dgvJobOffers
        '
        Me.dgvJobOffers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvJobOffers.Location = New System.Drawing.Point(12, 12)
        Me.dgvJobOffers.Name = "dgvJobOffers"
        Me.dgvJobOffers.Size = New System.Drawing.Size(1160, 633)
        Me.dgvJobOffers.TabIndex = 1
        '
        'btnPauseCrawling
        '
        Me.btnPauseCrawling.Location = New System.Drawing.Point(12, 703)
        Me.btnPauseCrawling.Name = "btnPauseCrawling"
        Me.btnPauseCrawling.Size = New System.Drawing.Size(141, 46)
        Me.btnPauseCrawling.TabIndex = 2
        Me.btnPauseCrawling.Text = "Crawling pausieren"
        Me.btnPauseCrawling.UseVisualStyleBackColor = True
        '
        'tbInfo
        '
        Me.tbInfo.Location = New System.Drawing.Point(159, 651)
        Me.tbInfo.Name = "tbInfo"
        Me.tbInfo.Size = New System.Drawing.Size(1013, 98)
        Me.tbInfo.TabIndex = 3
        Me.tbInfo.Text = ""
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1184, 761)
        Me.Controls.Add(Me.tbInfo)
        Me.Controls.Add(Me.btnPauseCrawling)
        Me.Controls.Add(Me.dgvJobOffers)
        Me.Controls.Add(Me.btnStartCrawling)
        Me.Name = "frmMain"
        Me.Text = "Webcrawler"
        CType(Me.dgvJobOffers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnStartCrawling As Button
    Friend WithEvents dgvJobOffers As DataGridView
    Friend WithEvents btnPauseCrawling As Button
    Friend WithEvents tbInfo As RichTextBox
End Class
