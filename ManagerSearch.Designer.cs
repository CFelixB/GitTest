namespace FolioBot
{
    partial class ManagerSearch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cbSearchBy = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbSortDirection = new System.Windows.Forms.ComboBox();
            this.dgvManagerFilter = new System.Windows.Forms.DataGridView();
            this.colManagerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colManager = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProgramCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStrategy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubStrategy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMarketFocus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVolatility = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSortino = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCTACorrelation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVIXCorrelation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManagerFilter)).BeginInit();
            this.SuspendLayout();
            // 
            // cbSearchBy
            // 
            this.cbSearchBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSearchBy.FormattingEnabled = true;
            this.cbSearchBy.Items.AddRange(new object[] {
            "Volatility",
            "Sortino",
            "CTA Correlation",
            "VIX Correlation"});
            this.cbSearchBy.Location = new System.Drawing.Point(92, 17);
            this.cbSearchBy.Name = "cbSearchBy";
            this.cbSearchBy.Size = new System.Drawing.Size(121, 24);
            this.cbSearchBy.TabIndex = 0;
            this.cbSearchBy.SelectedIndexChanged += new System.EventHandler(this.cbSearchBy_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search by";
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(1053, 525);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(1134, 525);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.cbSortDirection);
            this.panel1.Controls.Add(this.dgvManagerFilter);
            this.panel1.Controls.Add(this.cbSearchBy);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1197, 507);
            this.panel1.TabIndex = 5;
            // 
            // cbSortDirection
            // 
            this.cbSortDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSortDirection.FormattingEnabled = true;
            this.cbSortDirection.Items.AddRange(new object[] {
            "Ascending",
            "Descending"});
            this.cbSortDirection.Location = new System.Drawing.Point(219, 17);
            this.cbSortDirection.Name = "cbSortDirection";
            this.cbSortDirection.Size = new System.Drawing.Size(121, 24);
            this.cbSortDirection.TabIndex = 3;
            this.cbSortDirection.SelectedIndexChanged += new System.EventHandler(this.cbSortDirection_SelectedIndexChanged);
            // 
            // dgvManagerFilter
            // 
            this.dgvManagerFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvManagerFilter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvManagerFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvManagerFilter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colManagerCode,
            this.colManager,
            this.colProgramCode,
            this.colStrategy,
            this.colSubStrategy,
            this.colMarketFocus,
            this.colVolatility,
            this.colSortino,
            this.colCTACorrelation,
            this.colVIXCorrelation,
            this.colSelected});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvManagerFilter.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvManagerFilter.Location = new System.Drawing.Point(19, 122);
            this.dgvManagerFilter.Name = "dgvManagerFilter";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvManagerFilter.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvManagerFilter.RowTemplate.Height = 24;
            this.dgvManagerFilter.Size = new System.Drawing.Size(1160, 367);
            this.dgvManagerFilter.TabIndex = 2;
            // 
            // colManagerCode
            // 
            this.colManagerCode.HeaderText = "Mgr Code";
            this.colManagerCode.Name = "colManagerCode";
            // 
            // colManager
            // 
            this.colManager.HeaderText = "Manager";
            this.colManager.Name = "colManager";
            // 
            // colProgramCode
            // 
            this.colProgramCode.HeaderText = "Program Code";
            this.colProgramCode.Name = "colProgramCode";
            // 
            // colStrategy
            // 
            this.colStrategy.HeaderText = "Strategy";
            this.colStrategy.Name = "colStrategy";
            // 
            // colSubStrategy
            // 
            this.colSubStrategy.HeaderText = "Sub Strategy";
            this.colSubStrategy.Name = "colSubStrategy";
            // 
            // colMarketFocus
            // 
            this.colMarketFocus.HeaderText = "Markets Focus";
            this.colMarketFocus.Name = "colMarketFocus";
            // 
            // colVolatility
            // 
            this.colVolatility.HeaderText = "Volatility";
            this.colVolatility.Name = "colVolatility";
            // 
            // colSortino
            // 
            this.colSortino.HeaderText = "Sortino";
            this.colSortino.Name = "colSortino";
            // 
            // colCTACorrelation
            // 
            this.colCTACorrelation.HeaderText = "CTA Correlation";
            this.colCTACorrelation.Name = "colCTACorrelation";
            // 
            // colVIXCorrelation
            // 
            this.colVIXCorrelation.HeaderText = "VIX Correlation";
            this.colVIXCorrelation.Name = "colVIXCorrelation";
            // 
            // colSelected
            // 
            this.colSelected.HeaderText = "Select";
            this.colSelected.Name = "colSelected";
            // 
            // ManagerSearch
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(1221, 552);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.MinimizeBox = false;
            this.Name = "ManagerSearch";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search for Managers";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvManagerFilter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cbSearchBy;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvManagerFilter;
        private System.Windows.Forms.DataGridViewTextBoxColumn colManagerCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colManager;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProgramCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStrategy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubStrategy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMarketFocus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolatility;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSortino;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCTACorrelation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVIXCorrelation;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelected;
        public System.Windows.Forms.ComboBox cbSortDirection;
    }
}