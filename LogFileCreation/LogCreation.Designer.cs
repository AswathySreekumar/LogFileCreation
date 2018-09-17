namespace LogFileCreation
{
    partial class LogCreation
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
            this.cmbEdition = new System.Windows.Forms.ComboBox();
            this.cmbBranch = new System.Windows.Forms.ComboBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.Gb = new System.Windows.Forms.GroupBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.Gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbEdition
            // 
            this.cmbEdition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEdition.FormattingEnabled = true;
            this.cmbEdition.Items.AddRange(new object[] {
            "ePlus Professional Edition",
            "ePlus Basic Edition"});
            this.cmbEdition.Location = new System.Drawing.Point(83, 19);
            this.cmbEdition.Name = "cmbEdition";
            this.cmbEdition.Size = new System.Drawing.Size(145, 21);
            this.cmbEdition.TabIndex = 1;
            this.cmbEdition.SelectionChangeCommitted += new System.EventHandler(this.cmbEdition_SelectedIndexChanged);
            // 
            // cmbBranch
            // 
            this.cmbBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBranch.FormattingEnabled = true;
            this.cmbBranch.Location = new System.Drawing.Point(251, 19);
            this.cmbBranch.Name = "cmbBranch";
            this.cmbBranch.Size = new System.Drawing.Size(156, 21);
            this.cmbBranch.TabIndex = 2;
            this.cmbBranch.SelectedIndexChanged += new System.EventHandler(this.cmbBranch_SelectedIndexChanged);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(419, 196);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // Gb
            // 
            this.Gb.Controls.Add(this.btnBack);
            this.Gb.Controls.Add(this.cmbEdition);
            this.Gb.Controls.Add(this.cmbBranch);
            this.Gb.Controls.Add(this.btnUpdate);
            this.Gb.Location = new System.Drawing.Point(5, 12);
            this.Gb.Name = "Gb";
            this.Gb.Size = new System.Drawing.Size(504, 225);
            this.Gb.TabIndex = 6;
            this.Gb.TabStop = false;
            this.Gb.Text = "LogFile";
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(339, 196);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "< Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // LogCreation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 249);
            this.Controls.Add(this.Gb);
            this.Name = "LogCreation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogCreation";
            this.Load += new System.EventHandler(this.LogCreation_Load);
            this.Gb.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbEdition;
        private System.Windows.Forms.ComboBox cmbBranch;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.GroupBox Gb;
        private System.Windows.Forms.Button btnBack;
    }
}

