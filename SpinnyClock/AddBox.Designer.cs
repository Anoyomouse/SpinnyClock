namespace WindowsApplication4
{
	partial class AddBox
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkDST = new System.Windows.Forms.CheckBox();
            this.txtOffset = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdOK.Location = new System.Drawing.Point(12, 88);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(102, 34);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdCancel.Location = new System.Drawing.Point(120, 88);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(102, 34);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(70, 16);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(142, 20);
            this.txtName.TabIndex = 2;
            // 
            // chkDST
            // 
            this.chkDST.AutoSize = true;
            this.chkDST.Location = new System.Drawing.Point(70, 68);
            this.chkDST.Name = "chkDST";
            this.chkDST.Size = new System.Drawing.Size(15, 14);
            this.chkDST.TabIndex = 3;
            // 
            // txtOffset
            // 
            this.txtOffset.Location = new System.Drawing.Point(70, 42);
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(44, 20);
            this.txtOffset.TabIndex = 4;
            // 
            // AddBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 134);
            this.Controls.Add(this.txtOffset);
            this.Controls.Add(this.chkDST);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddBox";
            this.Text = "AddBox";
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EndDrag);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DoDrag);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StartDrag);
            this.Load += new System.EventHandler(this.AddBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.CheckBox chkDST;
		private System.Windows.Forms.TextBox txtOffset;
	}
}