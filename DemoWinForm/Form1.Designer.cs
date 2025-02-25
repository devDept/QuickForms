namespace DemoWinForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.quickControlwf1 = new QuickForms.WinForm.QuickControl();
            this.quickControlwf2 = new QuickForms.WinForm.QuickControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // quickControlwf1
            // 
            this.quickControlwf1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickControlwf1.Location = new System.Drawing.Point(0, 0);
            this.quickControlwf1.Margin = new System.Windows.Forms.Padding(0);
            this.quickControlwf1.Name = "quickControlwf1";
            this.quickControlwf1.Size = new System.Drawing.Size(400, 450);
            this.quickControlwf1.TabIndex = 0;
            // 
            // quickControlwf2
            // 
            this.quickControlwf2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quickControlwf2.Location = new System.Drawing.Point(400, 0);
            this.quickControlwf2.Margin = new System.Windows.Forms.Padding(0);
            this.quickControlwf2.Name = "quickControlwf2";
            this.quickControlwf2.Size = new System.Drawing.Size(400, 450);
            this.quickControlwf2.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.quickControlwf2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.quickControlwf1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.MinimumSize = new Size(500, 100);
        }

        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        public QuickForms.WinForm.QuickControl quickControlwf1;
        public QuickForms.WinForm.QuickControl quickControlwf2;
    }
}