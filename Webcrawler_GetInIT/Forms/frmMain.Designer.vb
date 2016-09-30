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
        Me.gridJobOffers = New System.Windows.Forms.DataGridView()
        Me.btnQuitCrawling = New System.Windows.Forms.Button()
        Me.tbInfo = New System.Windows.Forms.RichTextBox()
        Me.comboSitemapToCompare = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.gridJobOffers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnStartCrawling
        '
        Me.btnStartCrawling.Location = New System.Drawing.Point(12, 603)
        Me.btnStartCrawling.Name = "btnStartCrawling"
        Me.btnStartCrawling.Size = New System.Drawing.Size(130, 46)
        Me.btnStartCrawling.TabIndex = 0
        Me.btnStartCrawling.Text = "Crawling starten"
        Me.btnStartCrawling.UseVisualStyleBackColor = True
        '
        'gridJobOffers
        '
        Me.gridJobOffers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridJobOffers.Location = New System.Drawing.Point(12, 12)
        Me.gridJobOffers.Name = "gridJobOffers"
        Me.gridJobOffers.Size = New System.Drawing.Size(1060, 535)
        Me.gridJobOffers.TabIndex = 1
        '
        'btnQuitCrawling
        '
        Me.btnQuitCrawling.Enabled = False
        Me.btnQuitCrawling.Location = New System.Drawing.Point(150, 603)
        Me.btnQuitCrawling.Name = "btnQuitCrawling"
        Me.btnQuitCrawling.Size = New System.Drawing.Size(130, 46)
        Me.btnQuitCrawling.TabIndex = 2
        Me.btnQuitCrawling.Text = "Crawling abbrechen"
        Me.btnQuitCrawling.UseVisualStyleBackColor = True
        '
        'tbInfo
        '
        Me.tbInfo.Location = New System.Drawing.Point(286, 553)
        Me.tbInfo.Name = "tbInfo"
        Me.tbInfo.Size = New System.Drawing.Size(786, 98)
        Me.tbInfo.TabIndex = 3
        Me.tbInfo.Text = ""
        '
        'comboSitemapToCompare
        '
        Me.comboSitemapToCompare.FormattingEnabled = True
        Me.comboSitemapToCompare.Location = New System.Drawing.Point(12, 576)
        Me.comboSitemapToCompare.Name = "comboSitemapToCompare"
        Me.comboSitemapToCompare.Size = New System.Drawing.Size(268, 21)
        Me.comboSitemapToCompare.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 553)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(159, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Neuste Sitemap vergleichen mit:"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1084, 661)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.comboSitemapToCompare)
        Me.Controls.Add(Me.tbInfo)
        Me.Controls.Add(Me.btnQuitCrawling)
        Me.Controls.Add(Me.gridJobOffers)
        Me.Controls.Add(Me.btnStartCrawling)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmMain"
        Me.Text = "Webcrawler"
        CType(Me.gridJobOffers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnStartCrawling As Button
    Friend WithEvents gridJobOffers As DataGridView
    Friend WithEvents btnQuitCrawling As Button
    Friend WithEvents tbInfo As RichTextBox
    Friend WithEvents comboSitemapToCompare As ComboBox
    Friend WithEvents Label1 As Label
End Class
