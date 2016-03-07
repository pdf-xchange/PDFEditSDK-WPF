namespace WpfSimpleApp
{
	partial class UserControl1
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl1));
			this.axPXV_Control1 = new AxPDFXEdit.AxPXV_Control();
			((System.ComponentModel.ISupportInitialize)(this.axPXV_Control1)).BeginInit();
			this.SuspendLayout();
			// 
			// axPXV_Control1
			// 
			this.axPXV_Control1.Enabled = true;
			this.axPXV_Control1.Location = new System.Drawing.Point(3, 0);
			this.axPXV_Control1.Name = "axPXV_Control1";
			this.axPXV_Control1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPXV_Control1.OcxState")));
			this.axPXV_Control1.Size = new System.Drawing.Size(620, 428);
			this.axPXV_Control1.TabIndex = 0;
			// 
			// UserControl1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.axPXV_Control1);
			this.Name = "UserControl1";
			this.Size = new System.Drawing.Size(1070, 678);
			((System.ComponentModel.ISupportInitialize)(this.axPXV_Control1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public AxPDFXEdit.AxPXV_Control axPXV_Control1;
	}
}
